using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;
using DKCESTL1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace DKCESTL1
{
    [ApiController]
    [Route("[controller]")]
    public class ModelUtilsController : ControllerBase
    {
        private DatabaseLookupController lookupController;

        public ModelUtilsController()
        {
            lookupController = new DatabaseLookupController();

        }


    public int convertCitynameToCityId(string cityname)
        {
            List<string[]> cityIdMapping = lookupController.queryForCityIdMapping().Value;

            int cityId = 0;

            foreach (string[] row in cityIdMapping)
            {
                if (((string) row.GetValue(1)).ToUpper().Equals(cityname))
                {
                    cityId = Convert.ToInt32(row.GetValue(0));
                }
            }

            return cityId;
        }

    public string convertCityIdToCityname(int cityid)
    {
        List<string[]> cityIdMapping = lookupController.queryForCityIdMapping().Value;

        string cityname = "";

        foreach (string[] row in cityIdMapping)
        {
            if ( Convert.ToInt32(row.GetValue(0)) == cityid)
            {
                cityname = (string) row.GetValue(1);
            }
        }

        return cityname;
    }


        public List<int[]> parseMapToFastestDijkstraList(string parcelType,int parcelWeight)
        {
            List<Edge> map = lookupController.queryForMap().Value;

            List<int[]> cityIdMap = new List<int[]>();

            



            foreach (Edge edge in map)
            {
                if (!edge.available)
                {
                    continue;
                }

                int city1 = convertCitynameToCityId(edge.fromCity.name.ToUpper());
                int city2 = convertCitynameToCityId(edge.toCity.name.ToUpper());

                int weight;

                

               /* if (edge.node == 0)
                {
                    ExternalIntegrationController externalRest = new ExternalIntegrationController();
                    ExternalIntegration EI = externalRest.Get(edge.fromCity.name, edge.toCity.name, parcelType,
                        parcelWeight).Value;
                    if (EI.possible)
                    {
                        weight = EI.duration;
                    }
                    else
                    {
                        continue;
                    }

                }
                else
                {*/
                    weight = edge.node * 4;
                //}

                int[] cityIdEdge = new int[] {city1, city2, weight };


                cityIdMap.Add(cityIdEdge);
            }


            int[][] jaggedArray = new int[cityIdMap.Count][];
            int[] weights = new int[cityIdMap.Count];

            int i = 0;
            foreach (int[] row in cityIdMap)
            {
                jaggedArray[i] = row;
                weights[i] = row[2];
                i++;
            }


            Array.Sort(weights, jaggedArray);

            List<int[]> finalList = new List<int[]>();
            foreach (int[] row in jaggedArray)
            {
                finalList.Add(row);
            }


            //i = 0;
            //foreach (int[] row in jaggedArray)
            //{
            //    for (int j=i+1; j <= jaggedArray.Length-1; j++)
            //    {
            //        int value1 = (int) jaggedArray[j].GetValue(0);
            //        int value2 = (int) row.GetValue((1));
            //        int value3 = (int) jaggedArray[j].GetValue(1);
            //        int value4 = (int) row.GetValue(0);
            //        if (value1 == value2 && value3 == value4)
            //        {
            //            finalList.RemoveAt(j);
            //        }
            //    }

            //    i++;
            //}

            return finalList;
        }

        [HttpGet("calculate/{parceltype}/{parcelweight}/{fromCity}/{toCity}/{recommended}")]
        public string[] calculateFastestRoute(string fromCity, string toCity, string parceltype, int parcelweight, Boolean recommended)
        {

            int fromCityId = convertCitynameToCityId(fromCity.ToUpper());
            int toCityId = convertCitynameToCityId(toCity.ToUpper());

            List<int[]> nodestuff = parseMapToFastestDijkstraList(parceltype, parcelweight);

            var graph = new Graph<int, string>();
            for (int k = 1; k <= 28; k++)
            {
                graph.AddNode(k);
            }

            foreach (int[] row in nodestuff)
            {
                graph.Connect((uint)row[0], (uint)row[1], row[2], "");
                graph.Connect((uint)row[1], (uint)row[0], row[2], "");
            }


            var result = graph.Dijkstra((uint)fromCityId, (uint)toCityId);

            uint[] path = result.GetPath().ToArray();

        
            int[] intArr = new int[path.Length];
            for (int n = 0; n < path.Length; n++)
            {
                intArr[n] = (int)path[n];
            }

            
            for (int o = 1; o < path.Length; o++)
            {
                var node = graph >> intArr[o - 1];
                var vehicle = node.GetFirstEdgeCustom(path[o]);
            }

            string[] citynames = new string[path.Length+2];

            for (int m = 0; m < path.Length; m++)
            {
                citynames[m] = convertCityIdToCityname(intArr[m]);
            }

            citynames[path.Length] = result.Distance.ToString();


            List<string[]> prices = lookupController.queryForPricingData().Value;

            List<string> formattedPrices = new List<string>();

            foreach (string[] price in prices)
            {
                string formattedPrice = Convert.ToString(price.GetValue(1));
                formattedPrice.Replace(",", ".");
                formattedPrices.Add(formattedPrice);
            }


            double priceOfDelivery = 0;
            int nodesEdgeForDelivery = result.Distance/4;
            if (parceltype.Equals("standard"))
            {
                priceOfDelivery = Convert.ToDouble(formattedPrices[0]) * nodesEdgeForDelivery;
            }
            else if (parceltype.Equals("weapons"))
            {
                priceOfDelivery = Convert.ToDouble(formattedPrices[0]) * Convert.ToDouble(formattedPrices[2]) * nodesEdgeForDelivery;
            }
            else if (parceltype.Equals("live animals"))
            {
                priceOfDelivery = Convert.ToDouble(formattedPrices[0]) * Convert.ToDouble(formattedPrices[3]) * nodesEdgeForDelivery;
            }
            else if (parceltype.Equals("cautious parcels"))
            {
                priceOfDelivery = Convert.ToDouble(formattedPrices[0]) * Convert.ToDouble(formattedPrices[4]) * nodesEdgeForDelivery;
            }
            else if (parceltype.Equals("refrigerated goods"))
            {
                priceOfDelivery = Convert.ToDouble(formattedPrices[0]) * Convert.ToDouble(formattedPrices[5]) * nodesEdgeForDelivery;
            }
            if (recommended)
            {
                priceOfDelivery = priceOfDelivery + Convert.ToDouble(formattedPrices[1]);
            }

            priceOfDelivery = Math.Round(priceOfDelivery, 2);

            citynames[path.Length + 1] = priceOfDelivery.ToString();

            return citynames;
        }

        /*
        [HttpGet("parseCheapestDijkstraMap")]
        public List<int[]> parseMapToCheapestDijkstraList()
        {
            List<Edge> map = lookupController.queryForMap().Value;

            List<int[]> cityIdMap = new List<int[]>();

            foreach (Edge edge in map)
            {
                int city1 = convertCitynameToCityId(edge.fromCity.city.ToUpper());
                int city2 = convertCitynameToCityId(edge.toCity.city.ToUpper());
                int weight = edge.node * 4; 

                int[] cityIdEdge = new int[] { city1, city2, weight };

                cityIdMap.Add(cityIdEdge);
            }

            return cityIdMap;
        }
        */


    }
}
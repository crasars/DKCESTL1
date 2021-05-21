using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
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

    
        [HttpGet("parseFastestDijkstraMap")]
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

                int city1 = convertCitynameToCityId(edge.fromCity.city.ToUpper());
                int city2 = convertCitynameToCityId(edge.toCity.city.ToUpper());

                int weight;

                

                if (edge.node == 0)
                {
                    ExternalIntegrationController externalRest = new ExternalIntegrationController();
                    ExternalIntegration EI = externalRest.Get(edge.fromCity.city, edge.toCity.city, parcelType,
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
                {
                    weight = edge.node * 4;
                }

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


            i = 0;
            foreach (int[] row in jaggedArray)
            {
                for (int j=i+1; j <= jaggedArray.Length-1; j++)
                {
                    int value1 = (int) jaggedArray[j].GetValue(0);
                    int value2 = (int) row.GetValue((1));
                    int value3 = (int) jaggedArray[j].GetValue(1);
                    int value4 = (int) row.GetValue(0);
                    if (value1 == value2 && value3 == value4)
                    {
                        finalList.RemoveAt(j);
                    }
                }

                i++;
            }

            return finalList;
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
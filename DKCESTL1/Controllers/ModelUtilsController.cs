using System;
using System.Collections.Generic;
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
        public List<int[]> parseMapToFastestDijkstraList()
        {
            List<Edge> map = lookupController.queryForMap().Value;

            List<int[]> cityIdMap = new List<int[]>();

            //ExternalIntegrationController externalRest = new ExternalIntegrationController();

            

            foreach (Edge edge in map)
            {
                if (!edge.available)
                {
                    continue;
                }

                int city1 = convertCitynameToCityId(edge.fromCity.city.ToUpper());
                int city2 = convertCitynameToCityId(edge.toCity.city.ToUpper());
                int weight = edge.node*4;

                int[] cityIdEdge = new int[] {city1, city2, weight };

                cityIdMap.Add(cityIdEdge);
            }




            return cityIdMap;
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
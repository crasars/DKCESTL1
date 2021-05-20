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

        [HttpGet("parseDijkstraMap")]
        public List<int[]> parseMapToCityId()
        {
            List<Edge> map = lookupController.queryForMap().Value;

            List<int[]> cityIdMap = new List<int[]>();

            foreach (Edge edge in map)
            {
                int city1 = convertCitynameToCityId(edge.fromCity.city.ToUpper());
                int city2 = convertCitynameToCityId(edge.toCity.city.ToUpper());
                int nodes = edge.node;

                int[] cityIdEdge = new int[] {city1, city2, nodes};

                cityIdMap.Add(cityIdEdge);
            }

            return cityIdMap;
        }



    }
    

    
}
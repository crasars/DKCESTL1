using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DKCESTL1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OutboundAPIController : ControllerBase
    {
        public OutboundAPIController()
        {
        }
        /*
        [HttpGet("{id}")]
        public ActionResult<List<SegmentInformation>> GetAll() => SegmentInformationService.GetAll();
        */

        [HttpGet("{City1}/{City2}/{parcelType}/{parcelWeight}/{signed}")]
        public ActionResult<SegmentInformation> Get(string City1, string City2, string parcelType, int parcelWeight, bool signed)
        {
            DatabaseLookupController databaseLookup = new DatabaseLookupController();
            List<Edge> allEdges = databaseLookup.queryForMap().Value;
            List<String[]> prices = databaseLookup.queryForPricingData().Value;
            SegmentInformation newSeg = new SegmentInformation();
            foreach (Edge edge in allEdges)
            {
                if (edge.fromCity.city.Equals(City1) || edge.toCity.city.Equals(City1))
                {
                    if (edge.fromCity.city.Equals(City2) || edge.toCity.city.Equals(City2))
                    {
                        Edge edgeForDelivery = edge;
                        int nodesEdgeForDelivery = Convert.ToInt32(edgeForDelivery.node);
                        int priceOfDelivery = 3 * nodesEdgeForDelivery;
                        int timeOfDelivery = 4 * nodesEdgeForDelivery;
                        newSeg.price = priceOfDelivery;
                        newSeg.time = timeOfDelivery;
                        newSeg.available = true;
                    }
                }
            }
            if (newSeg == null)
            {
                return NotFound();
            }
            return newSeg;


        }
        }
}

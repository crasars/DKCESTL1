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

        [HttpGet("{City1}/{City2}/{parcelType}/{parcelWeight}/{signed}")]
        public ActionResult<SegmentInformation> Get(string City1, string City2, string parcelType, int parcelWeight, bool signed)
        {
            City1 = City1.ToLower();
            City2 = City2.ToLower();
            parcelType = parcelType.ToLower();
            DatabaseLookupController databaseLookup = new DatabaseLookupController();
            List<Edge> allEdges = databaseLookup.queryForMap().Value;
            List<string[]> prices = databaseLookup.queryForPricingData().Value;
            SegmentInformation newSeg = new SegmentInformation();
            List<string> formattedPrices = new List<string>();
            foreach (string[] price in prices)
            {
                string formattedPrice = Convert.ToString(price.GetValue(1));
                formattedPrice.Replace(",", ".");
                formattedPrices.Add(formattedPrice);
            }
       
            foreach (Edge edge in allEdges)
            {
                string startCity = edge.fromCity.name.ToLower();
                string endCity = edge.toCity.name.ToLower();
                if (startCity.Equals(City1) || endCity.Equals(City1))
                {
                    if (startCity.Equals(City2) || endCity.Equals(City2))
                    {
                        double priceOfDelivery = 0;
                        Edge edgeForDelivery = edge;
                        int nodesEdgeForDelivery = Convert.ToInt32(edgeForDelivery.node);
                        if (parcelType.Equals("standard"))
                        {
                            priceOfDelivery = Convert.ToDouble(formattedPrices[0]) * nodesEdgeForDelivery;
                        } else if (parcelType.Equals("weapons"))
                        {
                            priceOfDelivery = Convert.ToDouble(formattedPrices[0]) * Convert.ToDouble(formattedPrices[2]) * nodesEdgeForDelivery;
                        } else if (parcelType.Equals("live animals"))
                        {
                            priceOfDelivery = Convert.ToDouble(formattedPrices[0]) * Convert.ToDouble(formattedPrices[3]) * nodesEdgeForDelivery;
                        } else if (parcelType.Equals("cautious parcels"))
                        {
                            priceOfDelivery = Convert.ToDouble(formattedPrices[0]) * Convert.ToDouble(formattedPrices[4]) * nodesEdgeForDelivery;
                        } else if (parcelType.Equals("refrigerated goods"))
                        {
                            priceOfDelivery = Convert.ToDouble(formattedPrices[0]) * Convert.ToDouble(formattedPrices[5]) * nodesEdgeForDelivery;
                        }
                        if (parcelWeight > 40)
                        {
                            priceOfDelivery = 0;
                        }
                        int timeOfDelivery = 4 * nodesEdgeForDelivery;
                        newSeg.price = priceOfDelivery;
                        newSeg.time = timeOfDelivery;
                        if (newSeg.price == 0)
                        { 
                            newSeg.available = false; 
                        }
                        else
                        {
                            newSeg.available = true;
                        }
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

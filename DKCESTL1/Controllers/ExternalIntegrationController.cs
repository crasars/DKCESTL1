using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace DKCESTL1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExternalIntegrationController : ControllerBase
    {
        [HttpGet("plane/{city1}/{city2}/{parcelWeight}/{parcelType}")]
        public ActionResult<ExternalIntegration> Get(string city1, string city2, string parcelType, int parcelWeight)
        {
            var client = new RestClient("http://wa-oa-dk1.azurewebsites.net/external/transportInfo/");
            var request = new RestRequest(city1 + "/" + city2 + "/" + parcelWeight.ToString() + "/" + parcelType, DataFormat.None);
            var response = client.Get(request);

            ExternalIntegration newExternalIntegration = JsonConvert.DeserializeObject<ExternalIntegration>(response.Content);

            return newExternalIntegration;
        }
        [HttpGet("ship/{city1}/{city2}/{parcelWeight}/{parcelType}/{token}")]
        public ActionResult<ExternalIntegration> Get(string city1, string city2, string parcelType, int parcelWeight, string token)
        {
            token = "jhflidhfæishfsæohføosjøfllkclkc";
            int type = 0;
            if (parcelType == "Live Animals")
            {
                type = 1;
            }else if (parcelType == "Recorded Delivery")
            {
                type = 2;
            }else if (parcelType == "Weapons")
            {
                type = 3;
            }else if (parcelType == "Cautious Parcel")
            {
                type = 4;
            }else if (parcelType == "Refrigerated Goods")
            {
                type = 5;
            }

            if (parcelWeight > 100 || type == 0)
            {
                ExternalIntegration noResultFromShip = new ExternalIntegration { available = false, duration = 0, price = 0 };
                return noResultFromShip;
            }


            var client = new RestClient("http://wa-eit-dk1.azurewebsites.net/api/parcelinformation?");
            var request = new RestRequest("source=" + city1 + "&destination=" + city2 + "&type=" + type + "&token=" + token, DataFormat.None);
            var response = client.Get(request);

            ExternalIntegration newExternalIntegration = JsonConvert.DeserializeObject<ExternalIntegration>(response.Content);

            return newExternalIntegration;
        }
    }
}

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
            var request = new RestRequest(city1 + "/" + city2 + "/" + parcelWeight + "/" + parcelType, DataFormat.None);
            var response = client.Get(request);

            ExternalIntegration newExternalIntegration = JsonConvert.DeserializeObject<ExternalIntegration>(response.Content);

            return newExternalIntegration;
        }
    }
}

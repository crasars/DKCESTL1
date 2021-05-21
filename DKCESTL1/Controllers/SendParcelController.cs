using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DKCESTL1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendParcelController : ControllerBase
    {
        private readonly ILogger<SendParcelController> _logger;

        public SendParcelController(ILogger<SendParcelController> logger)
        {
            _logger = logger;
        }

        [HttpPost("sendParcel")]
        public ActionResult SendParcel([FromForm]string parcel)
        {
            if (parcel == null)
            {
                return Ok();
            }

            Parcel testParcel = JsonConvert.DeserializeObject<Parcel>(parcel);
            //return Ok(parcel.endCity.city);

            return Ok($"SendParcel SendParcel startCity=={testParcel.startCity.name} endCity=={testParcel.endCity.name}");
        }
    }
}

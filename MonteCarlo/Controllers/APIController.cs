using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MonteCarlo.Models;
using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Controllers
{
    [Route("api/")]
    public class APIController : Controller
    {
        [HttpPost]
        public JsonResult Post([FromBody]DataModel value)
        {
            var runner = new SimulationManager(value);
            var result = runner.Run();
            return Json(result);
        }
    }
}

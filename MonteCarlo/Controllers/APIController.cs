using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MonteCarlo.Models;
using System.Threading.Tasks;

namespace MonteCarlo.Controllers
{
    [Route("api/")]
    public class APIController : Controller
    {
        [HttpPost]
        public JsonResult Post([FromBody]DataModel inputs)
        {
            var result = new Dictionary<string, Result>();
            var projectedInputs = inputs.Copy();
            projectedInputs.BondsDataStartDate = DataStartDate._2000;
            projectedInputs.StocksDataStartDate = DataStartDate._2000;
            
            result.Add("historical", Task.Factory.StartNew(() =>
            {
                var runner = new SimulationManager(inputs);
                return runner.Run();
            }).Result);

            result.Add("projected", Task.Factory.StartNew(() =>
            {
                var runner = new SimulationManager(projectedInputs);
                return runner.Run();
            }).Result);

            return Json(result);
        }
    }
}

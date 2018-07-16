using Microsoft.AspNetCore.Mvc;
using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Controllers
{
    [Route("api/")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public JsonResult Get()
        {
            var initialDistribution = DistributionPool.GetDistribution(Distribution.Normal, withPeakAt: 0.093, withScale: 27.814);
            var stepDistribution = DistributionPool.GetDistribution(Distribution.Normal, withPeakAt: 10.82, withScale: 17.16);

            var mc = new Models.Statistics.MonteCarlo(
                initialDistribution: initialDistribution,
                stepDistribution: stepDistribution
            );

            var result = mc.Run();

            DistributionPool.ReleaseObject(initialDistribution);
            DistributionPool.ReleaseObject(stepDistribution);

            return Json(result);
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
    }
}

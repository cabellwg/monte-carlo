using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MonteCarlo.Models;
using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Controllers
{
    [Route("api/")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public JsonResult Get()
        {
            // Stocks
            var stocksMC = new Models.MonteCarlo();
            var stocks = stocksMC.Run(withProfile: new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 10.82, withScale: 17.16),
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 0.093, withScale: 27.814),
                TrialLength = 30,
                ContributionLength = 15,
                InitialAmount = 10000,
                ContributionAmount = 500,
                WithdrawalAmount = 5000
            });

            // Bonds
            var bondsMC = new Models.MonteCarlo();
            var bonds = stocksMC.Run(withProfile: new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 4.80, withScale: 3.68),
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 0.00, withScale: 3.88),
                TrialLength = 30,
                ContributionLength = 15,
                InitialAmount = 5000,
                ContributionAmount = 100,
                WithdrawalAmount = 5000
            });

            // Savings
            var savingsMC = new Models.MonteCarlo();
            var savings = savingsMC.Run(withProfile: new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta, withPeakAt: 0.001, withScale: 0.000),
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 0.0001, withScale: 1),
                TrialLength = 30,
                ContributionLength = 15,
                InitialAmount = 15000,
                ContributionAmount = 7500,
                WithdrawalAmount = 5000
            });

            var result = new Dictionary<string, double[][]>()
            {
                { "stocks", stocks },
                { "bonds", bonds },
                { "savings", savings }
            };

            return Json(result);
        }

        [HttpPost]
        public void Post([FromBody]DataModel value)
        {
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MonteCarlo.Models;
using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Controllers
{
    [Route("api/")]
    public class APIController : Controller
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
                TrialLength = 55,
                ContributionLength = 10,
                InitialAmount = 0,
                ContributionAmount = 6000,
                WithdrawalAmount = 9000
            });

            // Bonds
            var bondsMC = new Models.MonteCarlo();
            var bonds = bondsMC.Run(withProfile: new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 4.80, withScale: 3.68),
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 0.00, withScale: 3.88),
                TrialLength = 55,
                ContributionLength = 10,
                InitialAmount = 0,
                ContributionAmount = 3500,
                WithdrawalAmount = 5250
            });

            // Savings
            var savingsMC = new Models.MonteCarlo();
            var savings = savingsMC.Run(withProfile: new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta, withPeakAt: 0.001, withScale: 0.000),
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 0.0001, withScale: 1),
                TrialLength = 55,
                ContributionLength = 10,
                InitialAmount = 0,
                ContributionAmount = 500,
                WithdrawalAmount = 750
            });

            var result = new Dictionary<string, double[][]>()
            {
                { "stocks", stocks },
                { "bonds", bonds },
                { "savings", savings }
            };

            return Json(result);
        }

        [HttpGet("historical")]
        public JsonResult GetHistorical()
        {
            // Stocks
            var stocksMC = new Models.MonteCarlo();
            var stocks = stocksMC.Run(withProfile: new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 10.82, withScale: 17.16),
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 0.093, withScale: 27.814),
                TrialLength = 55,
                ContributionLength = 10,
                InitialAmount = 0,
                ContributionAmount = 6000,
                WithdrawalAmount = 15000
            });

            // Historical
            var historicalMC = new Models.MonteCarlo();
            var historical = historicalMC.Run(withProfile: new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta, withPeakAt: 20.8, withScale: 17.16),
                StepDistribution = new TestableDistribution(new List<double>()
                {
                    -18.5, -15.1, 46.8, -17.6, -25.7, 28, -29.5, 27.8, -2.4, -3.7, -29.8, 34.1, -10.9, -19.5, 20, 1.3, 8.5, -31.2, -11, 65.9, -20.42, -35.17, 14.16, 7.32, 10.72, -24.13, 28.82, 0.68, -24.01, 31.4, -5.09, -20.31, 9.55, 15.16, -31.30, 24.63, -16.13, 9.53, -11.59, 31.36, -7.47, -3.39, -6.52, 9.11, -31.4, -0.91, -9.68, 42.09, -22.19, -3.73, 16.9, -9.88, 40.24, 52.63, -7.8, -5.5, 1.77, 19.22, -18.99
                }),
                TrialLength = 55,
                ContributionLength = 10,
                InitialAmount = 0,
                ContributionAmount = 6000,
                WithdrawalAmount = 15000
            });

            var result = new Dictionary<string, double[][]>()
            {
                { "stocks", stocks },
                { "historical", historical }
            };

            return Json(result);
        }

        [HttpPost]
        public JsonResult Post([FromBody]DataModel value)
        {
            var runner = new PortfolioSimulator(value);
            var result = runner.Run();
            return Json(result);
        }
    }
}

using Xunit;
using MonteCarlo.Models.Statistics;
using Newtonsoft.Json;
using MonteCarlo.Models;

namespace MonteCarlo.Tests.StatisticsTests
{
    public class MonteCarloTests
    {
        [Fact]
        public void TestMonteCarlo()
        {
            var mc = new BondsSimulation();

            var result = mc.Run(withProfile: new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 0.093, withScale: 27.814),
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 10.82, withScale: 17.16),
                TrialLength = 30,
                ContributionLength = 15,
                InitialAmount = 10000,
                ContributionAmount = 500,
                WithdrawalAmount = 5000
            });
            string jsonResult = JsonConvert.SerializeObject(result);
        }
    }
}

using Xunit;
using MonteCarlo.Models.Statistics;
using Newtonsoft.Json;

namespace MonteCarlo.Tests.StatisticsTests
{
    public class MonteCarloTests
    {
        [Fact]
        public void TestMonteCarlo()
        {
            ProbabilityDistribution normal = new NormalDistribution(mean: 0, standardDeviation: 1);
            Models.Statistics.MonteCarlo mc = new Models.Statistics.MonteCarlo(random: normal);
            double[][] result = mc.Run();
            string jsonResult = JsonConvert.SerializeObject(result);
        }
    }
}

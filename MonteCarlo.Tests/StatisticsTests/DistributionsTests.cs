using Xunit;
using MonteCarlo.Models.Statistics;
using Newtonsoft.Json;

namespace MonteCarlo.Tests.StatisticsTests
{
    public class DistributionsTests
    {
        [Fact]
        public void TestNormal()
        {
            ProbabilityDistribution normal = new NormalDistribution(mean: 0.093, standardDeviation: 27.814);
            Models.Statistics.MonteCarlo mc = new Models.Statistics.MonteCarlo(random: normal);
            double[][] result = mc.Run();
            string jsonResult = JsonConvert.SerializeObject(result);
        }

        [Fact]
        public void TestLogNormal()
        {
            ProbabilityDistribution logNormal = new LogNormalDistribution(mean: 10.82, standardDeviation: 17.16);
            Models.Statistics.MonteCarlo mc = new Models.Statistics.MonteCarlo(random: logNormal);
            double[][] result = mc.Run();
            string jsonResult = JsonConvert.SerializeObject(result);
        }

        [Fact]
        public void TestCauchy()
        {
            ProbabilityDistribution logNormal = new LogNormalDistribution(mean: 10.82, standardDeviation: 17.16);
            Models.Statistics.MonteCarlo mc = new Models.Statistics.MonteCarlo(random: logNormal);
            double[][] result = mc.Run();
            string jsonResult = JsonConvert.SerializeObject(result);
        }

        [Fact]
        public void TestLaplace()
        {
            ProbabilityDistribution logNormal = new LogNormalDistribution(mean: 10.82, standardDeviation: 17.16);
            Models.Statistics.MonteCarlo mc = new Models.Statistics.MonteCarlo(random: logNormal);
            double[][] result = mc.Run();
            string jsonResult = JsonConvert.SerializeObject(result);
        }

        [Fact]
        public void TestT()
        {
            ProbabilityDistribution logNormal = new LogNormalDistribution(mean: 10.82, standardDeviation: 17.16);
            Models.Statistics.MonteCarlo mc = new Models.Statistics.MonteCarlo(random: logNormal);
            double[][] result = mc.Run();
            string jsonResult = JsonConvert.SerializeObject(result);
        }
    }
}

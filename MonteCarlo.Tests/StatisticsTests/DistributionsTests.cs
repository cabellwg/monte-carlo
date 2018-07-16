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
            ProbabilityDistribution cauchy = new CauchyDistribution(location: 0.093, scale: 27.814);
            Models.Statistics.MonteCarlo mc = new Models.Statistics.MonteCarlo(random: cauchy);
            double[][] result = mc.Run();
            string jsonResult = JsonConvert.SerializeObject(result);
        }

        [Fact]
        public void TestLaplace()
        {
            ProbabilityDistribution laplace = new LaplaceDistribution(location: 10.82, diversity: 17.16);
            Models.Statistics.MonteCarlo mc = new Models.Statistics.MonteCarlo(random: laplace);
            double[][] result = mc.Run();
            string jsonResult = JsonConvert.SerializeObject(result);
        }

        [Fact]
        public void TestT()
        {
            ProbabilityDistribution t = new TDistribution(location: 10.82, scale: 17.16);
            Models.Statistics.MonteCarlo mc = new Models.Statistics.MonteCarlo(random: t);
            double[][] result = mc.Run();
            string jsonResult = JsonConvert.SerializeObject(result);
        }
    }
}

using System;

namespace MonteCarlo.Models.Statistics
{
    public class LogNormalDistribution : ProbabilityDistribution
    {
        private ProbabilityDistribution normalSampler;

        public LogNormalDistribution(double mean, double standardDeviation)
        {
            normalSampler = DistributionPool.GetDistribution(Statistics.Distribution.Normal, 0, 1);
            PeakX = mean;
            Scale = standardDeviation;
            Type = Statistics.Distribution.LogNormal;
        }

        public override MathFunction Distribution => x =>
        {
            return x;
        };

        public override double NextDouble()
        {
            return Math.Exp(PeakX + Scale * normalSampler.NextDouble());
        }
    }
}

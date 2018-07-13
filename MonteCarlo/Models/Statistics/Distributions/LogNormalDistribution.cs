using System;

namespace MonteCarlo.Models.Statistics
{
    public class LogNormalDistribution : ProbabilityDistribution
    {
        private NormalDistribution normalSampler;
        private readonly double sigma;

        public LogNormalDistribution(double mean, double standardDeviation)
        {
            normalSampler = new NormalDistribution(0, 1);
            Mean = mean;
            sigma = standardDeviation;
        }

        public override MathFunction Distribution => x =>
        {
            return x;
        };

        public override double NextDouble()
        {
            return Math.Exp(Mean + sigma * normalSampler.NextDouble());
        }
    }
}

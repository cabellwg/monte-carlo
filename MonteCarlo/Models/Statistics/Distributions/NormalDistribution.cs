using System;

namespace MonteCarlo.Models.Statistics
{
    public class NormalDistribution : ProbabilityDistribution
    {
        private Ziggurat z;

        public NormalDistribution(double mean, double standardDeviation)
        {
            Scale = standardDeviation;
            PeakX = mean;
            Type = Statistics.Distribution.Normal;

            z = new Ziggurat(Distribution, mean, 10 * Scale, new UniformDistribution());
        }

        public override double NextDouble() => z.NextDouble();

        public override MathFunction Distribution => x =>
        {
            var variance = Math.Pow(Scale, 2);
            var scale = (1 / Math.Sqrt(2 * Math.PI) / Scale);
            return scale * Math.Exp(-(Math.Pow(x - PeakX, 2) / (2 * variance)));
        };
    }
}

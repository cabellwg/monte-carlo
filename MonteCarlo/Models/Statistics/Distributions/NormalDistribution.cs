using System;

namespace MonteCarlo.Models.Statistics
{
    public class NormalDistribution : ProbabilityDistribution
    {
        private Ziggurat z;
        private readonly double sigma;

        public NormalDistribution(double mean, double standardDeviation)
        {
            sigma = standardDeviation;
            Mean = mean;

            z = new Ziggurat(Distribution, mean, 10 * sigma, new ThreadSafeRandom());
        }

        public override double NextDouble() => z.NextDouble();

        public override MathFunction Distribution => x =>
        {
            var variance = Math.Pow(sigma, 2);
            var scale = (1 / Math.Sqrt(2 * Math.PI) / sigma);
            return scale * Math.Exp(-(Math.Pow(x - Mean, 2) / (2 * variance)));
        };
    }
}

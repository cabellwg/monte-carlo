using System;

namespace MonteCarlo.Models.Statistics
{
    public class TDistribution : ProbabilityDistribution
    {
        private Ziggurat z;
        private readonly double gamma;

        public TDistribution(double location, double scale)
        {
            gamma = scale;
            Mean = location;

            z = new Ziggurat(Distribution, Mean, 10 * scale, new ThreadSafeRandom());
        }

        public override double NextDouble() => z.NextDouble();

        public override MathFunction Distribution => x =>
        {
            var normalizer = gamma * Math.PI;
            return 1 / (normalizer * (1 + Math.Pow((x - Mean) / gamma, 2)));
        };
    }
}

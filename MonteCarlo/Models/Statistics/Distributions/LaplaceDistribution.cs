using System;

namespace MonteCarlo.Models.Statistics
{
    public class LaplaceDistribution : ProbabilityDistribution
    {
        private Ziggurat z;
        private readonly double b;

        public LaplaceDistribution(double location, double diversity)
        {
            b = diversity;
            Mean = location;

            z = new Ziggurat(Distribution, Mean, 10 * diversity, new ThreadSafeRandom());
        }

        public override double NextDouble() => z.NextDouble();

        public override MathFunction Distribution => x =>
        {
            var normalizer = 1 / (2 * b);
            return normalizer * Math.Exp(-Math.Abs(x - Mean) / b);
        };
    }
}

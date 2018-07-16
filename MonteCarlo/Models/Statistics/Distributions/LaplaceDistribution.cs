using System;

namespace MonteCarlo.Models.Statistics
{
    public class LaplaceDistribution : ProbabilityDistribution
    {
        private Ziggurat z;

        public LaplaceDistribution(double location, double diversity)
        {
            Scale = diversity;
            PeakX = location;
            Type = Statistics.Distribution.Laplace;

            z = new Ziggurat(Distribution, PeakX, 10 * diversity, new UniformDistribution());
        }

        public override double NextDouble() => z.NextDouble();

        public override MathFunction Distribution => x =>
        {
            var normalizer = 1 / (2 * Scale);
            return normalizer * Math.Exp(-Math.Abs(x - PeakX) / Scale);
        };
    }
}

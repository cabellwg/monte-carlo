using System;

namespace MonteCarlo.Models.Statistics
{
    public class LaplaceDistribution : ProbabilityDistribution
    {
        private ProbabilityDistribution z;

        public LaplaceDistribution(double location, double diversity)
        {
            Scale = diversity;
            PeakX = location;
            Type = Statistics.Distribution.Laplace;

            z = new UniformDistribution();
        }

        public override double NextDouble()
        {
            var u = z.NextDouble();
            return PeakX - Scale * Math.Sign(u) * Math.Log(1 - 2 * Math.Abs(u));
        }

        public override MathFunction Distribution => x =>
        {
            var normalizer = 1 / (2 * Scale);
            return normalizer * Math.Exp(-Math.Abs(x - PeakX) / Scale);
        };
    }
}

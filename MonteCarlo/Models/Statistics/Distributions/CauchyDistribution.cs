using System;

namespace MonteCarlo.Models.Statistics
{
    public class CauchyDistribution : ProbabilityDistribution
    {
        private Ziggurat z;

        public CauchyDistribution(double location, double scale)
        {
            Scale = scale;
            PeakX = location;
            Type = Statistics.Distribution.Cauchy;

            z = new Ziggurat(Distribution, PeakX, 175 * scale, new UniformDistribution());
        }

        public override double NextDouble() => z.NextDouble();

        public override MathFunction Distribution => x =>
        {
            var normalizer = Scale * Math.PI;
            return 1 / (normalizer * (1 + Math.Pow((x - PeakX) / Scale, 2)));
        };
    }
}

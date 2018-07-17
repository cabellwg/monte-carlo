using System;

namespace MonteCarlo.Models.Statistics
{
    public class NormalDistribution : ProbabilityDistribution
    {
        private NormalDistribution normalized;
        private Ziggurat z;

        public NormalDistribution(double mean, double standardDeviation)
        {
            Scale = standardDeviation;
            PeakX = mean;
            Type = Statistics.Distribution.Normal;

            if (Scale == 1 && PeakX == 0)
            {
                z = new Ziggurat(NormalizedDistribution, 0, 4, new UniformDistribution());
            }
            else
            {
                normalized = DistributionPool.Instance.GetDistribution(Statistics.Distribution.Normal, 0, 1) as NormalDistribution;
            }
        }

        public override double NextDouble()
        {
            if (Scale == 1 && PeakX == 0)
            {
                return z.NextDouble();
            }
            else
            {
                return normalized.NextDouble() * Scale + PeakX;
            }
        }

        public override MathFunction Distribution => x =>
        {
            var variance = Math.Pow(Scale, 2);
            var scale = (1 / Math.Sqrt(2 * Math.PI) / Scale);
            return scale * Math.Exp(-(Math.Pow(x - PeakX, 2) / (2 * variance)));
        };

        private static readonly MathFunction NormalizedDistribution = x =>
        {
            var variance = 1;
            var scale = (1 / Math.Sqrt(2 * Math.PI));
            return scale * Math.Exp(-(Math.Pow(x, 2) / (2 * variance)));
        };
    }
}

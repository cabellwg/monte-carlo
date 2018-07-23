using System;

namespace MonteCarlo.Models.Statistics
{
    public class LogisticDistribution : ProbabilityDistribution
    {
        private Ziggurat z;

        public LogisticDistribution(double location, double scale)
        {
            Scale = scale;
            PeakX = location;
            Type = Statistics.Distribution.Logistic;

            z = new Ziggurat(Distribution, PeakX, 175 * scale, new UniformDistribution());
        }

        public override MathFunction Distribution => x =>
        {
            return Math.Exp(-(x - PeakX) / Scale) / (Scale * Math.Pow(1 + Math.Exp(-(x - PeakX) / Scale), 2));
        };

        public override double NextDouble() => z.NextDouble();
    }
}

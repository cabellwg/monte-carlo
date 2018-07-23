using System;

namespace MonteCarlo.Models.Statistics
{
    public class LogisticDistribution : ProbabilityDistribution
    {
        private ProbabilityDistribution z;

        public LogisticDistribution(double location, double scale)
        {
            Scale = scale;
            PeakX = location;
            Type = Statistics.Distribution.Logistic;

            z = new UniformDistribution();
        }

        public override MathFunction Distribution => x =>
        {
            return Math.Exp(-(x - PeakX) / Scale) / (Scale * Math.Pow(1 + Math.Exp(-(x - PeakX) / Scale), 2));
        };

        public override double NextDouble()
        {
            var u = z.NextDouble() ;
            return PeakX + Scale * Math.Log(u / (1 - u));
        }
    }
}

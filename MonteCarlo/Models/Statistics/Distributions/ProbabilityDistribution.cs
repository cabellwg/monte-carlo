namespace MonteCarlo.Models.Statistics
{
    public abstract class ProbabilityDistribution : IRandom
    {
        public double Mean { get; protected set; }
        public abstract MathFunction Distribution { get; }
        public MathFunction InverseOfCdf { get; } = null;

        public abstract double NextDouble();
    }
}

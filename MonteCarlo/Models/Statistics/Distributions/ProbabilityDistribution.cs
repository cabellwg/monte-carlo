namespace MonteCarlo.Models.Statistics
{
    public abstract class ProbabilityDistribution : IRandom
    {
        public virtual Distribution Type { get; set; }
        public double PeakX { get; protected set; }
        public double Scale { get; protected set; }
        public abstract MathFunction Distribution { get; }
        public MathFunction InverseOfCdf { get; } = null;

        public abstract double NextDouble();
    }
}

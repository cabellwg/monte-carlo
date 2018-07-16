namespace MonteCarlo.Models.Statistics
{
    public class UniformDistribution : ProbabilityDistribution
    {
        public override Distribution Type { get; set; } = Statistics.Distribution.Uniform;

        public override MathFunction Distribution => x => 1;

        public override double NextDouble() => StaticThreadSafeRandom.NextDouble();
    }
}

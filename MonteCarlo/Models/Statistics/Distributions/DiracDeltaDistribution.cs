namespace MonteCarlo.Models.Statistics
{
    public class DiracDeltaDistribution : ProbabilityDistribution
    {
        public override MathFunction Distribution => x => 0;

        public DiracDeltaDistribution(double peak)
        {
            Type = Statistics.Distribution.DiracDelta;
            PeakX = peak;
        }

        public override double NextDouble()
        {
            return PeakX;
        }
    }
}

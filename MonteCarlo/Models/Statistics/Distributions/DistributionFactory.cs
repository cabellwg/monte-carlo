namespace MonteCarlo.Models.Statistics
{

    // Distribution instantiation takes a long time, use the DistributionPool
    public class DistributionFactory
    {
        public static ProbabilityDistribution Create(Distribution type, double withPeakAt, double withScale)
        {
            switch (type)
            {
                case Distribution.Uniform:
                    return new UniformDistribution();
                case Distribution.Normal:
                    return new NormalDistribution(withPeakAt, withScale);
                case Distribution.T:
                    return new TDistribution(withPeakAt, withScale);
                case Distribution.Laplace:
                    return new LaplaceDistribution(withPeakAt, withScale);
                case Distribution.LogNormal:
                    return new LogNormalDistribution(withPeakAt, withScale);
                case Distribution.Cauchy:
                    return new CauchyDistribution(withPeakAt, withScale);
                case Distribution.Logistic:
                    return new LogisticDistribution(withPeakAt, withScale);
                case Distribution.DiracDelta:
                    return new DiracDeltaDistribution(withPeakAt);
                default:
                    return null;
            }
        }
    }
}

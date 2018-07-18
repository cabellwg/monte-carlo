using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Models
{
    public class RunProfile
    {
        public ProbabilityDistribution SeedDistribution { get; set; }
        public ProbabilityDistribution StepDistribution { get; set; }
        public int TrialLength { get; set; }
        public int ContributionLength { get; set; }
        public double InitialAmount { get; set; }
        public double ContributionAmount { get; set; }
        public double WithdrawalAmount { get; set; }

        public double Drift { get; set; }
        public double Volatility { get; set; }
    }
}

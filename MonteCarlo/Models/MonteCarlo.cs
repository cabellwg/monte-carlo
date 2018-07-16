using System.Threading;
using System.Threading.Tasks;

namespace MonteCarlo.Models
{
    public class MonteCarlo
    {
        private readonly double[][] trials;
        private Mutex mutex = new Mutex();

        private const int NUM_TRIALS = 1000;

        public MonteCarlo()
        {
            trials = new double[NUM_TRIALS][];
        }

        public double[][] Run(RunProfile withProfile)
        {
            Parallel.For(0, NUM_TRIALS, i =>
            {
                RunTrial(i, withProfile);
            });

            return trials;
        }

        private void RunTrial(int trialNumber, RunProfile profile)
        {
            double[] trial = new double[profile.TrialLength];

            // Populate trial data
            double interestRate = profile.SeedDistribution.NextDouble() / 100;
            trial[0] = (profile.InitialAmount + profile.ContributionAmount) * (1 + interestRate);

            // Contribution period
            for (var i = 1; i < profile.ContributionLength; i++)
            {
                interestRate += profile.StepDistribution.NextDouble() / 100;
                trial[i] = trial[i - 1] > 0 ? (trial[i - 1] + profile.ContributionAmount) * (1 + interestRate) : 0;
                trial[i] = trial[i] > 0 ? trial[i] : 0;
            }

            // Withdrawal period
            for (var i = profile.ContributionLength; i < profile.TrialLength; i++)
            {
                interestRate += profile.StepDistribution.NextDouble() / 100;
                trial[i] = trial[i - 1] > 0 ? (trial[i - 1] - profile.WithdrawalAmount) * (1 + interestRate) : 0;
                trial[i] = trial[i] > 0 ? trial[i] : 0;
            }

            // Add data to return value
            mutex.WaitOne();
            trials[trialNumber] = trial;
            mutex.ReleaseMutex();
        }
    }
}

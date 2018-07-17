using System.Threading;
using System.Threading.Tasks;
using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Models
{
    public class MonteCarlo
    {
        public static int NUM_TRIALS = 1000;

        private readonly double[][] trials;
        private Mutex mutex = new Mutex();

        public MonteCarlo()
        {
            trials = new double[NUM_TRIALS][];
        }

        public double[][] Run(RunProfile withProfile)
        {
            if (withProfile.StepDistribution.Type == Distribution.DiracDelta ||
                withProfile.StepDistribution.Type == Distribution.Testable)
            {
                RunTrial(0, withProfile);
                for (var i = 1; i < NUM_TRIALS; i++)
                {
                    trials[i] = trials[0];
                }
            }
            else
            {
                Parallel.For(0, NUM_TRIALS, i =>
                {
                    RunTrial(i, withProfile);
                });
            }

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

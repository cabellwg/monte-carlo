namespace MonteCarlo.Models
{
    public class BondsSimulation : MonteCarloSimulation
    {
        public BondsSimulation() : base() { }

        protected override void RunTrial(int trialNumber, RunProfile profile)
        {
            double[] trial = new double[profile.TrialLength];

            // Populate trial data
            double interestRate = profile.SeedDistribution.NextDouble() / 100;
            trial[0] = (profile.InitialAmount + profile.ContributionAmount) * (1 + interestRate);

            // Contribution period
            for (var i = 1; i < profile.ContributionLength; i++)
            {
                interestRate += profile.StepDistribution.NextDouble() / 100;

                trial[i] = trial[i - 1] > 0 ?
                    i < profile.ContributionLength ?
                        // Contribution period
                        (trial[i - 1] + profile.ContributionAmount) * (1 + interestRate) :
                        // Withdrawal period
                        (trial[i - 1] - profile.WithdrawalAmount) * (1 + interestRate)
                    : 0;

                trial[i] = trial[i] > 0 ? trial[i] : 0;
                if (trial[i] == 0)
                {
                    break;
                }
            }

            // Add data to return value
            mutex.WaitOne();
            trials[trialNumber] = trial;
            mutex.ReleaseMutex();
        }
    }
}

namespace MonteCarlo.Models
{
    public class SavingsSimulation : MonteCarloSimulation
    {
        protected override void RunTrial(int trialNumber, RunProfile profile)
        {
            Trial trial = new Trial();
            double[] balances = new double[profile.TrialLength];

            // Populate trial data
            double interestRate = profile.SeedDistribution.NextDouble() * 0.01;
            balances[0] = (profile.InitialAmount + profile.ContributionAmount) * (1 + interestRate);

            // Contribution period
            for (var i = 1; i < profile.TrialLength; i++)
            {
                interestRate += profile.StepDistribution.NextDouble() * 0.01;

                balances[i] = balances[i - 1] > 0 ?
                    (i < profile.ContributionLength ?
                        // Contribution period
                        (balances[i - 1] + profile.ContributionAmount) * (1 + interestRate) :
                        // Withdrawal period
                        (balances[i - 1] - profile.WithdrawalAmount) * (1 + interestRate))
                    : 0;

                balances[i] = balances[i] > 0 ? balances[i] : 0;

                if (i == profile.ContributionLength - 1)
                {
                    trial.Peak = balances[i];
                }

                if (balances[i] == 0)
                {
                    break;
                }
            }
            
            trial.Balances = balances;
            trial.ReturnRates = null;

            // Add data to return value
            mutex.WaitOne();
            trials[trialNumber] = trial;
            mutex.ReleaseMutex();
        }
    }
}

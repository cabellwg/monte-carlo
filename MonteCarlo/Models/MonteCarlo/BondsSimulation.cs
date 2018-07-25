namespace MonteCarlo.Models
{
    public class BondsSimulation : MonteCarloSimulation
    {
        public BondsSimulation() : base() { }

        protected override void RunTrial(int trialNumber, RunProfile profile)
        {
            Trial trial = new Trial();
            var addAmt = profile.ContributionAmount;
            var withdrawAmt = profile.WithdrawalAmount;
            var trialLength = profile.TrialLength;
            var contribLength = profile.ContributionLength;
            var seed = profile.SeedDistribution.NextDouble();
            var stepSampler = profile.StepDistribution;

            double[] balances = new double[trialLength];
            double[] bondRates = new double[trialLength];
            double[] returnRates = new double[trialLength - 1];

            // Populate trial data
            double interestRate = seed * 0.01;
            bondRates[0] = interestRate;
            balances[0] = (profile.InitialAmount + addAmt) * (1 + interestRate);

            // Contribution period
            for (var i = 1; i < trialLength; i++)
            {
                interestRate += stepSampler.NextDouble() * 0.01;
                interestRate = interestRate < 0.03 ? 0.03 : interestRate;
                bondRates[i] = interestRate;

                var prevBalance = balances[i - 1];

                var balance = prevBalance > 0 ?
                    (i < contribLength ?
                        // Contribution period
                        (prevBalance + addAmt) * (1 + interestRate) :
                        // Withdrawal period
                        (prevBalance - withdrawAmt) * (1 + interestRate))
                    : 0;

                balances[i] = balance > 0 ? balance : 0;

                returnRates[i - 1] = interestRate;

                if (i == contribLength - 1)
                {
                    trial.Peak = balance;
                }

                if (balance == 0)
                {
                    break;
                }
            }

            trial.Balances = balances;
            trial.ReturnRates = returnRates;
            trial.Final = balances[trialLength - 1];

            // Add data to return value
            mutex.WaitOne();
            trials[trialNumber] = trial;
            mutex.ReleaseMutex();
        }
    }
}

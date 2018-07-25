using System;

namespace MonteCarlo.Models
{
    public class StocksSimulation : MonteCarloSimulation
    {
        protected override void RunTrial(int trialNumber, RunProfile profile)
        {
            Trial trial = new Trial();
            var addAmt = profile.ContributionAmount;
            var withdrawAmt = profile.WithdrawalAmount;
            var trialLength = profile.TrialLength;
            var contribLength = profile.ContributionLength;
            var stepSampler = profile.StepDistribution;
            var sigma = profile.Volatility;
            var mu = profile.Drift;

            double[] balances = new double[trialLength];
            double[] unitValues = new double[trialLength];
            double[] returnRates = new double[trialLength - 1];
            unitValues[0] = 1.0;

            // Populate trial data
            double step = 0;
            balances[0] = profile.InitialAmount + addAmt;
            
            for (var i = 1; i < trialLength; i++)
            {
                // Discretized Levy process (random walk with steps distributed as stepDistribution)
                step += stepSampler.NextDouble();

                // Discretized geometric Brownian motion
                var unitValue = (1 + mu + sigma * step + 0.5 * sigma * sigma * (step * step - 1)) * unitValues[i - 1];
                //var unitValue = Math.Exp(mu * i + sigma * step);

                // Get return rate
                var returnRate = unitValue / unitValues[i - 1];

                var prevBalance = balances[i - 1];

                var balance = prevBalance > 0 ?
                    (i < contribLength ?
                        // Contribution period
                        (prevBalance + addAmt) * returnRate :
                        // Withdrawal period
                        (prevBalance - withdrawAmt) * returnRate)
                    : 0;

                if (i == contribLength - 1)
                {
                    trial.Peak = balance;
                }

                balances[i] = balance > 0 ? balance : 0;
                unitValues[i] = unitValue;
                returnRates[i - 1] = Math.Log(returnRate);
                
                if (balance == 0) {
                    break;
                }
                
            }

            trial.Balances = balances;
            trial.ReturnRates = returnRates;

            // Add data to return value
            mutex.WaitOne();
            trials[trialNumber] = trial;
            mutex.ReleaseMutex();
        }
    }
}

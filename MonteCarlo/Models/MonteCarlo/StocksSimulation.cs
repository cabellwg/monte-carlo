using System;

namespace MonteCarlo.Models
{
    public class StocksSimulation : MonteCarloSimulation
    {
        protected override void RunTrial(int trialNumber, RunProfile profile)
        {
            double[] trial = new double[profile.TrialLength];

            double unitValue = 1.0;

            // Populate trial data
            double step = 0;
            trial[0] = (profile.InitialAmount + profile.ContributionAmount) * unitValue;
            
            for (var i = 1; i < profile.TrialLength; i++)
            {
                // Discretized Levy process (random walk with steps distributed as stepDistribution)
                step += profile.StepDistribution.NextDouble();

                // Discretized geometric Brownian motion
                unitValue = Math.Exp((profile.Drift - (Math.Pow(profile.Volatility, 2) / 2) * i) + profile.Volatility * step);

                trial[i] = trial[i - 1] > 0 ?
                    (i < profile.ContributionLength ?
                        // Contribution period
                        (trial[i - 1] + profile.ContributionAmount) * unitValue :
                        // Withdrawal period
                        (trial[i - 1] - profile.WithdrawalAmount) * unitValue)
                    : 0;

                trial[i] = trial[i] > 0 ? trial[i] : 0;
                if (trial[i] == 0) {
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

using System;

namespace MonteCarlo.Models
{
    public class StocksSimulation : MonteCarloSimulation
    {
        protected override void RunTrial(int trialNumber, RunProfile profile)
        {
            double[] trial = new double[profile.TrialLength];
            double[] unitValues = new double[profile.TrialLength];
            unitValues[0] = 1.0;

            // Populate trial data
            double step = 0;
            trial[0] = (profile.InitialAmount + profile.ContributionAmount) * unitValues[0];
            
            for (var i = 1; i < profile.TrialLength; i++)
            {
                // Discretized Levy process (random walk with steps distributed as stepDistribution)
                step += profile.StepDistribution.NextDouble();

                // Discretized geometric Brownian motion
                unitValues[i] = Math.Exp((profile.Drift - (Math.Pow(profile.Volatility, 2) / 2) * i) + profile.Volatility * step);

                trial[i] = trial[i - 1] > 0 ?
                    (i < profile.ContributionLength ?
                        // Contribution period
                        (trial[i - 1] + profile.ContributionAmount) * (unitValues[i] / unitValues[i - 1]) :
                        // Withdrawal period
                        (trial[i - 1] - profile.WithdrawalAmount) * (unitValues[i] / unitValues[i - 1]))
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

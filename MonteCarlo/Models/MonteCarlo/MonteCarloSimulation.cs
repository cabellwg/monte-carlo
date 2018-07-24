using System.Threading;
using System.Threading.Tasks;
using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Models
{
    public struct Trial
    {
        public double[] Balances;
        public double[] ReturnRates;
        public double Peak;
    }

    public abstract class MonteCarloSimulation
    {
        // Make sure that NUM_TRIALS - 1 is an even multiple of NUM_PERCENTILES - 1 in SimulationManager
        public static int NUM_TRIALS = 9999;

        protected readonly Trial[] trials;
        protected Mutex mutex = new Mutex();

        public MonteCarloSimulation()
        {
            trials = new Trial[NUM_TRIALS];
        }

        public Trial[] Run(RunProfile withProfile)
        {
            if (withProfile.StepDistribution.Type == Distribution.DiracDelta ||
                withProfile.StepDistribution.Type == Distribution.Testable)
            {
                RunTrial(0, withProfile);
                var trial = trials[0];
                for (var i = 1; i < NUM_TRIALS; i++)
                {
                    trials[i] = trial;
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

        protected abstract void RunTrial(int trialNumber, RunProfile profile);
    }
}

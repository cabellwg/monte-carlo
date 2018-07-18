using System.Threading;
using System.Threading.Tasks;
using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Models
{
    public abstract class MonteCarloSimulation
    {
        public static int NUM_TRIALS = 1000;

        protected readonly double[][] trials;
        protected Mutex mutex = new Mutex();

        public MonteCarloSimulation()
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

        protected abstract void RunTrial(int trialNumber, RunProfile profile);
    }
}

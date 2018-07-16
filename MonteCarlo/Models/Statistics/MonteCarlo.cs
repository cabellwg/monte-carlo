using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MonteCarlo.Models.Statistics
{
    public class MonteCarlo
    {
        public int TrialLength { get; set; } = 30;

        private readonly double[][] trials;
        private ProbabilityDistribution initialDistribution;
        private ProbabilityDistribution stepDistribution;
        private Mutex mutex = new Mutex();

        private const int NUM_TRIALS = 10000;

        public MonteCarlo(ProbabilityDistribution initialDistribution, ProbabilityDistribution stepDistribution)
        {
            this.initialDistribution = initialDistribution;
            this.stepDistribution = stepDistribution;
            trials = new double[NUM_TRIALS][];
        }

        public double[][] Run()
        {
            Parallel.For(0, NUM_TRIALS, i =>
            {
                RunTrial(i);
            });

            return trials;
        }

        private void RunTrial(int trialNumber)
        {
            double[] trial = new double[TrialLength];
            // populate trial data
            trial[0] = initialDistribution.NextDouble();

            for (var i = 1; i < TrialLength; i++)
            {
                trial[i] = trial[i - 1] + stepDistribution.NextDouble();
            }

            mutex.WaitOne();
            trials[trialNumber] = trial;
            mutex.ReleaseMutex();
        }
    }
}

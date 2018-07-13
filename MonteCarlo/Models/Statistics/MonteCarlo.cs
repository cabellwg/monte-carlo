using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MonteCarlo.Models.Statistics
{
    public class MonteCarlo
    {
        private readonly double[][] trials;
        private IRandom random;
        private Mutex mutex = new Mutex();

        private const int NUM_TRIALS = 10;

        public MonteCarlo(IRandom random)
        {
            this.random = random;
            trials = new double[NUM_TRIALS][];
        }

        public double[][] Run()
        {
            Parallel.For(0, NUM_TRIALS - 1, i =>
            {
                RunTrial(i);
            });

            return trials;
        }

        private void RunTrial(int trialNumber)
        {
            double[] trial = new double[10];
            // populate trial data
            trial[0] = random.NextDouble();

            for (var i = 1; i < 10; i++)
            {
                trial[i] = trial[i - 1] + random.NextDouble();
            }

            mutex.WaitOne();
            trials[trialNumber] = trial;
            mutex.ReleaseMutex();
        }
    }
}

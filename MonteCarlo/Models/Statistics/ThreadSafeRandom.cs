using System;

namespace MonteCarlo.Models.Statistics
{
    public class ThreadSafeRandom
    {
        private static readonly Random global = new Random();

        [ThreadStatic]
        private static Random local;

        public ThreadSafeRandom()
        {
            if (local == null)
            {
                int seed;
                lock (global)
                {
                    seed = global.Next();
                }
                local = new Random(seed);
            }
        }
        public double NextDouble()
        {
            return local.NextDouble();
        }
    }
}

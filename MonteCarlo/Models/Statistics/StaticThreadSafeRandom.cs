using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonteCarlo.Models.Statistics
{
    public static class StaticThreadSafeRandom
    {
        private static readonly Random global = new Random();

        [ThreadStatic]
        private static Random local;

        public static double NextDouble()
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

            return local.NextDouble();
        }
    }
}

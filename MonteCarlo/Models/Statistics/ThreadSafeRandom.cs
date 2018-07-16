using System;

namespace MonteCarlo.Models.Statistics
{
    public class ThreadSafeRandom : IRandom
    {
        public double NextDouble() => StaticThreadSafeRandom.NextDouble();
    }
}

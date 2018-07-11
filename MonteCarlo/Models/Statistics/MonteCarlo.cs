using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonteCarlo.Models.Statistics
{
    public class MonteCarlo
    {
        private int maxThreads = Environment.ProcessorCount;

        public void Run(Action<int> action)
        {
            Parallel.For(0, maxThreads, action);
        }
    }
}

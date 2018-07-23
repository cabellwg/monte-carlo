using System.Collections.Generic;
using System.Linq;

namespace MonteCarlo.Tests.Tools
{
    class TestableRandom
    {
        public IEnumerable<double> sequence { get; set; }
        private int index;

        public double NextDouble()
        {
            return sequence.ElementAt(index++);
        }
    }
}

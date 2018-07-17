using System.Collections.Generic;
using System.Linq;
using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Models
{
    class TestableRandom : IRandom
    {
        public IEnumerable<double> sequence { get; set; }
        private int index;

        public double NextDouble()
        {
            return sequence.ElementAt(index++);
        }
    }
}

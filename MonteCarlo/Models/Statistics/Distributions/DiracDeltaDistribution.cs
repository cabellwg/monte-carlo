using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonteCarlo.Models.Statistics
{
    public class DiracDeltaDistribution : ProbabilityDistribution
    {
        public override MathFunction Distribution => x => 0;

        public DiracDeltaDistribution(double peak)
        {
            PeakX = peak;
        }

        public override double NextDouble()
        {
            return PeakX;
        }
    }
}

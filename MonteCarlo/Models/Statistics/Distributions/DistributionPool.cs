using System;
using System.Collections.Generic;
using System.Threading;

namespace MonteCarlo.Models.Statistics
{
    public static class DistributionPool
    {
        [ThreadStatic]
        private static List<ProbabilityDistribution> distributions;
        private static Mutex m = new Mutex();

        public static ProbabilityDistribution GetDistribution(Distribution type, double withPeakAt, double withScale)
        {            
            if (distributions == null)
            {                
                distributions = new List<ProbabilityDistribution>();
            }

            var toReturn = distributions.Find(dist =>
            {
                return dist.Type == type &&
                    dist.PeakX == withPeakAt &&
                    dist.Scale == withScale;
            });

            if (type == Distribution.Uniform)
            {
                toReturn = distributions.Find(dist => dist.Type == Distribution.Uniform);
            }
            
            if (toReturn == null)
            {
                m.WaitOne();
                toReturn = DistributionFactory.Create(type, withPeakAt, withScale);
                distributions.Add(toReturn);
                m.ReleaseMutex();
            }
            
            return toReturn;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace MonteCarlo.Models.Statistics
{
    public class DistributionPool
    {
        private static DistributionPool instance;
        private static readonly object padlock = new object();

        public static DistributionPool Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DistributionPool();
                    }
                    return instance;
                }
            }
        }

        private List<ProbabilityDistribution> distributions;
        private Mutex m = new Mutex();

        private DistributionPool()
        {
            distributions = new List<ProbabilityDistribution>();
        }

        public ProbabilityDistribution GetDistribution(Distribution type, double withPeakAt, double withScale)
        {

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

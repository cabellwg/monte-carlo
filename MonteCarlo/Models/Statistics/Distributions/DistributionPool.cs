using System;
using System.Collections.Generic;

namespace MonteCarlo.Models.Statistics
{
    public static class DistributionPool
    {
        [ThreadStatic]
        private static List<ProbabilityDistribution> _available;
        [ThreadStatic]
        private static List<ProbabilityDistribution> _inUse;

        public static ProbabilityDistribution GetDistribution(Distribution type, double withPeakAt, double withScale)
        {
            if (_available == null)
            {
                _available = new List<ProbabilityDistribution>();
            }
            if (_inUse == null)
            {
                _inUse = new List<ProbabilityDistribution>();
            }

            lock (_available)
            {
                var toReturn = _available.Find(dist =>
                {
                    return dist.Type == type &&
                        dist.PeakX == withPeakAt &&
                        dist.Scale == withScale;
                });

                if (type == Distribution.Uniform)
                {
                    toReturn = _available.Find(dist => dist.Type == Distribution.Uniform);
                }

                if (toReturn != null)
                {
                    _inUse.Add(toReturn);
                    _available.Remove(toReturn);
                    return toReturn;
                }
                else
                {
                    toReturn = DistributionFactory.Create(type, withPeakAt, withScale);
                    _inUse.Add(toReturn);
                    return toReturn;
                }
            }
        }

        public static void ReleaseObject(ProbabilityDistribution distribution)
        {
            lock (_available)
            {
                _available.Add(distribution);
                _inUse.Remove(distribution);
            }
        }
    }
}

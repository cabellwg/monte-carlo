using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonteCarlo.Models.Statistics
{
    public interface IRandom
    {
        double NextDouble();
    }
}

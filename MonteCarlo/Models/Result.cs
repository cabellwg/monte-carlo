using System.Collections.Generic;

namespace MonteCarlo.Models
{
    public class Result
    {
        public int SuccessRate { get; set; }
        public IEnumerable<double[]> PortfolioPercentiles;
    }
}

using System.Collections.Generic;

namespace MonteCarlo.Models
{
    public class Result
    {
        public int SuccessRate { get; set; }

        public IEnumerable<double[]> PortfolioPercentiles { get; set; }

        public int[] ReturnRateFrequencies { get; set; }
        public double FrequencyPeak { get; set; }
        public double FrequencyScale { get; set; }
    }
}

using System.Collections.Generic;

namespace MonteCarlo.Models
{
    public class Result
    {
        public int SuccessRate { get; set; }


        // Line graph
        public IEnumerable<double[]> PortfolioPercentiles { get; set; }


        // Histograms
        public int[] StocksReturnRateFrequencies { get; set; } = new int[20];
        public double StocksFrequencyPeak { get; set; }
        public double StocksFrequencyScale { get; set; }

        public int[] BondsReturnRateFrequencies { get; set; } = new int[20];
        public double BondsFrequencyPeak { get; set; }
        public double BondsFrequencyScale { get; set; }


        // Stacked bar
        public List<double> StocksRetirementAmount { get; set; } = new List<double>();
        public List<double> StocksEndAmount { get; set; } = new List<double>();

        public List<double> BondsRetirementAmount { get; set; } = new List<double>();
        public List<double> BondsEndAmount { get; set; } = new List<double>();
    }
}

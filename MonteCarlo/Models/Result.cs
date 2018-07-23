using System.Collections.Generic;

namespace MonteCarlo.Models
{
    public class Result
    {
        public int SuccessRate { get; set; }


        // Line graph
        public IEnumerable<double[]> PortfolioPercentiles { get; set; }


        // Histograms
        public int[] StocksReturnRateFrequencies { get; set; }
        public double StocksFrequencyPeak { get; set; }
        public double StocksFrequencyScale { get; set; }

        public int[] BondsReturnRateFrequencies { get; set; }
        public double BondsFrequencyPeak { get; set; }
        public double BondsFrequencyScale { get; set; }


        // Stacked bar
        public double StocksPeakAmount { get; set; }
        public double StocksInflow { get; set; }

        public double BondsPeakAmount { get; set; }
        public double BondsInflow { get; set; }
    }
}

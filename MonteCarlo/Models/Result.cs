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
        public double[] StocksReturnRateXLabels { get; set; }

        public int[] BondsReturnRateFrequencies { get; set; }
        public double[] BondsReturnRateXLabels { get; set; }


        // Stacked bar
        public List<double> StocksRetirementAmounts { get; set; } = new List<double>();
        public List<double> StocksEndAmounts { get; set; } = new List<double>();

        public List<double> BondsRetirementAmounts { get; set; } = new List<double>();
        public List<double> BondsEndAmounts { get; set; } = new List<double>();
    }
}

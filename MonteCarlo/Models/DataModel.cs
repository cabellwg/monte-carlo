using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Models
{
    public class DataModel
    {
        public int Age { get; set; }
        public int RetirementAge { get; set; }
        public int DeathAge { get; set; }
        public double YearlyStocksContributions { get; set; }
        public double YearlyBondsContributions { get; set; }
        public double YearlySavingsContributions { get; set; }
        public double StocksAmount { get; set; }
        public double BondsAmount { get; set; }
        public double SavingsAmount { get; set; }
        public double DesiredRetirementIncome { get; set; }
        public Distribution StocksDistributionType { get; set; }
        public DataStartDate StocksDataStartDate { get; set; }
        public Distribution BondsDistributionType { get; set; }
        public DataStartDate BondsDataStartDate { get; set; }

        public DataModel Copy()
        {
            return MemberwiseClone() as DataModel;
        }
    }

    public enum DataStartDate
    {
        _1928,
        _1975,
        _2000
    }
}

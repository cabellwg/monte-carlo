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
    }
}

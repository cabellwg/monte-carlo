using System;
using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Models
{
    public enum InvestmentType
    {
        Stocks,
        Bonds,
        Savings
    }

    public class RunProfile
    {
        public ProbabilityDistribution SeedDistribution { get; set; }
        public ProbabilityDistribution StepDistribution { get; set; }
        public int TrialLength { get; set; }
        public int ContributionLength { get; set; }
        public double InitialAmount { get; set; }
        public double ContributionAmount { get; set; }
        public double WithdrawalAmount { get; set; }

        public double Drift { get; set; }
        public double Volatility { get; set; }

        public RunProfile() { }

        public RunProfile(InvestmentType investmentType, DataModel dataModel)
        {
            // Process data into usable form
            ContributionLength = dataModel.RetirementAge - dataModel.Age;
            ContributionLength = ContributionLength > 0 ? ContributionLength : 0;

            TrialLength = dataModel.DeathAge - dataModel.Age;
            TrialLength = TrialLength > 0 ? TrialLength : 0;

            WithdrawalAmount = dataModel.DesiredRetirementIncome;

            // Calculate proportions to set withdrawals
            double totalAdditions = dataModel.YearlyStocksContributions +
                                    dataModel.YearlyBondsContributions +
                                    dataModel.YearlySavingsContributions;

            double stockContribProportion = dataModel.YearlyStocksContributions / totalAdditions;
            double bondsContribProportion = dataModel.YearlyBondsContributions / totalAdditions;
            double savingsContribProportion = dataModel.YearlySavingsContributions / totalAdditions;

            double stocksWithdrawalAmount = WithdrawalAmount * stockContribProportion;
            double bondsWithdrawalAmount = WithdrawalAmount * bondsContribProportion;
            double savingsWithdrawalAmount = WithdrawalAmount * savingsContribProportion;

            // Set defaults
            SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta);
            StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withScale: 1);

            // Set specific data
            switch (investmentType)
            {
                case InvestmentType.Stocks:
                    InitialAmount = dataModel.StocksAmount;
                    ContributionAmount = dataModel.YearlyStocksContributions;
                    WithdrawalAmount = stocksWithdrawalAmount;

                    // Stock GBM parameters
                    Drift = Constants.GBMValues[dataModel.StocksDataStartDate]["drift"];
                    Volatility = Constants.GBMValues[dataModel.StocksDataStartDate]["volatility"];

                    // Stock GBM generator
                    StepDistribution = DistributionPool.Instance.GetDistribution(dataModel.StocksDistributionType, withScale: 1.0);
                    break;
                case InvestmentType.Bonds:
                    InitialAmount = dataModel.BondsAmount;
                    ContributionAmount = dataModel.YearlyBondsContributions;
                    WithdrawalAmount = bondsWithdrawalAmount;

                    // Bonds random walk generators
                    SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta, withPeakAt: 2.94);
                    StepDistribution = DistributionPool.Instance.GetDistribution(dataModel.BondsDistributionType,
                            withPeakAt: Constants.BondValues[dataModel.BondsDataStartDate][dataModel.BondsDistributionType]["peak"],
                            withScale: Constants.BondValues[dataModel.BondsDataStartDate][dataModel.BondsDistributionType]["scale"] / Math.Sqrt(TrialLength));
                    break;
                case InvestmentType.Savings:
                default:
                    InitialAmount = dataModel.SavingsAmount;
                    ContributionAmount = dataModel.YearlySavingsContributions;
                    WithdrawalAmount = savingsWithdrawalAmount;
                    // Interest on savings accounts
                    SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta, withPeakAt: 0.1);
                    StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta);
                    break;
            }

            Volatility /= TrialLength <= 0 ? 1 : Math.Sqrt(TrialLength);
        }
    }
}

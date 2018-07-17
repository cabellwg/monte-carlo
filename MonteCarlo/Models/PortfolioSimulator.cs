using System.Collections.Generic;
using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Models
{
    public class PortfolioSimulator
    {
        private readonly RunProfile stocksProfile;
        private readonly RunProfile bondsProfile;
        private readonly RunProfile savingsProfile;

        private MonteCarlo stocksSimulation;
        private MonteCarlo bondsSimulation;
        private MonteCarlo savingsSimulation;

        private readonly int trialLength;
        private readonly double withdrawalAmount;
        private readonly double stocksWithdrawalAmount;
        private readonly double bondsWithdrawalAmount;
        private readonly double savingsWithdrawalAmount;

        public PortfolioSimulator(DataModel dataModel)
        {
            int contributionLength = dataModel.RetirementAge - dataModel.Age;
            contributionLength = contributionLength > 0 ? contributionLength : 0;

            trialLength = dataModel.DeathAge - dataModel.Age;
            trialLength = trialLength > 0 ? trialLength : 0;

            withdrawalAmount = dataModel.DesiredRetirementIncome;

            // Calculate proportions to set withdrawals
            double totalAdditions = dataModel.YearlyStocksContributions +
                                    dataModel.YearlyBondsContributions +
                                    dataModel.YearlySavingsContributions;

            double stockContribProportion = dataModel.YearlyStocksContributions / totalAdditions;
            double bondsContribProportion = dataModel.YearlyBondsContributions / totalAdditions;
            double savingsContribProportion = dataModel.YearlySavingsContributions / totalAdditions;

            stocksWithdrawalAmount = withdrawalAmount * stockContribProportion;
            bondsWithdrawalAmount = withdrawalAmount * bondsContribProportion;
            savingsWithdrawalAmount = withdrawalAmount * savingsContribProportion;


            // Set up simulations
            // Stocks
            stocksSimulation = new MonteCarlo();
            stocksProfile = new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 10.82, withScale: 17.16),
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 0.093, withScale: 27.814),
                TrialLength = trialLength,
                ContributionLength = contributionLength,
                InitialAmount = dataModel.StocksAmount,
                ContributionAmount = dataModel.YearlyStocksContributions,
                WithdrawalAmount = stocksWithdrawalAmount
            };

            // Bonds
            bondsSimulation = new MonteCarlo();
            bondsProfile = new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 4.80, withScale: 3.68),
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal, withPeakAt: 0.00, withScale: 3.88),
                TrialLength = trialLength,
                ContributionLength = contributionLength,
                InitialAmount = dataModel.BondsAmount,
                ContributionAmount = dataModel.YearlyBondsContributions,
                WithdrawalAmount = bondsWithdrawalAmount
            };

            // Savings
            savingsSimulation = new MonteCarlo();
            savingsProfile = new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta, withPeakAt: 0.001, withScale: 0),
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta, withPeakAt: 0.0001, withScale: 0),
                TrialLength = trialLength,
                ContributionLength = contributionLength,
                InitialAmount = dataModel.SavingsAmount,
                ContributionAmount = dataModel.YearlySavingsContributions,
                WithdrawalAmount = savingsWithdrawalAmount
            };
        }

        public Result Run()
        {
            var stocksResult = stocksSimulation.Run(withProfile: stocksProfile);
            var bondsResult = bondsSimulation.Run(withProfile: bondsProfile);
            var savingsResult = savingsSimulation.Run(withProfile: savingsProfile);

            Result result = new Result()
            {
                Trials = new Dictionary<string, double[][]>()
                {
                    { "stocks", stocksResult },
                    { "bonds", bondsResult },
                    { "savings", savingsResult }
                }
            };

            int numberOfSuccesses = 0;

            for (var i = 0; i < MonteCarlo.NUM_TRIALS; i++)
            {
                if (stocksResult[i][trialLength - 2] +
                    bondsResult[i][trialLength - 2] +
                    savingsResult[i][trialLength - 2] >= withdrawalAmount)
                {
                    numberOfSuccesses++;
                }
            }

            result.SuccessRate = numberOfSuccesses / (double)MonteCarlo.NUM_TRIALS;

            return result;
        }
    }
}

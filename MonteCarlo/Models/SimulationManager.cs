using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonteCarlo.Models.Statistics;
using Sto;

namespace MonteCarlo.Models
{
    public class SimulationManager
    {
        // Make sure that NUM_TRIALS - 1 in MonteCarloSimulation is an even multiple of NUM_PERCENTILES - 1
        private const int NUM_PERCENTILES = 10;

        private readonly RunProfile stocksProfile;
        private readonly RunProfile bondsProfile;
        private readonly RunProfile savingsProfile;

        private MonteCarloSimulation stocksSimulation;
        private MonteCarloSimulation bondsSimulation;
        private MonteCarloSimulation savingsSimulation;

        private readonly int trialLength;
        private readonly double withdrawalAmount;
        private readonly double stocksWithdrawalAmount;
        private readonly double bondsWithdrawalAmount;
        private readonly double savingsWithdrawalAmount;

        public SimulationManager(DataModel dataModel)
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

            /* **********************************************************************
             *                                                                      *
             *           Change parameters for the Postman tests below              *
             *                                                                      *
             *                                                                      *
             *                                                                      *
             *                                                                      *
             * **********************************************************************/

            // Set up simulations
            // Stocks
            stocksSimulation = new StocksSimulation();
            stocksProfile = new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta),
                // This should always be the unit distribution, the scaling is done by Drift and Volatility
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal,
                    withPeakAt: 0,
                    withScale: 1),
                Drift = 0.056331,
                Volatility = 0.188768 / trialLength,
                TrialLength = trialLength,
                ContributionLength = contributionLength,
                InitialAmount = dataModel.StocksAmount,
                ContributionAmount = dataModel.YearlyStocksContributions,
                WithdrawalAmount = stocksWithdrawalAmount
            };

            // Bonds
            bondsSimulation = new BondsSimulation();
            bondsProfile = new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta, withPeakAt: 1.18),
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.Normal,
                    withPeakAt: 0.00,
                    withScale: 3.88 / trialLength),
                TrialLength = trialLength,
                ContributionLength = contributionLength,
                InitialAmount = dataModel.BondsAmount,
                ContributionAmount = dataModel.YearlyBondsContributions,
                WithdrawalAmount = bondsWithdrawalAmount
            };

            // Savings
            savingsSimulation = new SavingsSimulation();
            savingsProfile = new RunProfile()
            {
                SeedDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta,
                    withPeakAt: 0.001,
                    withScale: 0),
                StepDistribution = DistributionPool.Instance.GetDistribution(Distribution.DiracDelta,
                    withPeakAt: 0.0001,
                    withScale: 0 / trialLength),
                TrialLength = trialLength,
                ContributionLength = contributionLength,
                InitialAmount = dataModel.SavingsAmount,
                ContributionAmount = dataModel.YearlySavingsContributions,
                WithdrawalAmount = savingsWithdrawalAmount
            };
        }

        public Result Run()
        {
            Task<double[][]> stocks = Task<double[][]>.Factory.StartNew(() =>
            {
                return stocksSimulation.Run(withProfile: stocksProfile);
            });

            Task<double[][]> bonds = Task<double[][]>.Factory.StartNew(() =>
            {
                return bondsSimulation.Run(withProfile: bondsProfile);
            });

            Task<double[][]> savings = Task<double[][]>.Factory.StartNew(() =>
            {
                return savingsSimulation.Run(withProfile: savingsProfile);
            });

            double[][] stocksResult = stocks.Result;
            double[][] bondsResult = bonds.Result;
            double[][] savingsResult = savings.Result;


            var trials = new Dictionary<string, double[][]>()
            {
                { "stocks", stocksResult },
                { "bonds", bondsResult },
                { "savings", savingsResult }
            };

            return ProcessTrials(trials);
        }

        private Result ProcessTrials(Dictionary<string, double[][]> trials)
        {
            Result result = new Result();

            var portfolios = trials["stocks"].Select((trial, i) =>
            {
                return trial.Select((value, j) =>
                {
                    return value + trials["bonds"][i][j] + trials["savings"][i][j];
                }).ToArray();
            }).ToList();

            int numberOfSuccesses = 0;

            for (var i = 0; i < MonteCarloSimulation.NUM_TRIALS; i++)
            {
                if (portfolios[i][trialLength - 2] >= withdrawalAmount)
                {
                    numberOfSuccesses++;
                }
            }

            result.SuccessRate = (int)Math.Round(100 * numberOfSuccesses / (double)MonteCarloSimulation.NUM_TRIALS);
            
            portfolios.ParallelMergeSort(CompareTrials);

            result.PortfolioPercentiles = portfolios.Where((trial, index) =>
            {
                // Make sure that NUM_TRIALS - 1 in MonteCarloSimulation is an even multiple of NUM_PERCENTILES - 1
                return index % ((portfolios.Count - 1) / (NUM_PERCENTILES - 1)) == 0;
            }).ToList();
            
            return result;
        }

        private int CompareTrials(double[] a, double[] b)
        {
            if (a.Length > b.Length)
            {
                return 1;
            }
            else if (b.Length > a.Length)
            {
                return -1;
            }

            for (var i = a.Length - 1; i >= 0; i--)
            {
                if (a[i] == 0 && b[i] != 0)
                {
                    return -1;
                }
                else if (b[i] == 0 && a[i] != 0)
                {
                    return 1;
                }
                else if (a[i] != 0 && b[i] != 0)
                {
                    if (a[i] == b[i])
                    {
                        continue;
                    }
                    return a[i] > b[i] ? 1 : -1;
                }
            }

            return 0;
        }
    }
}

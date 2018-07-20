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

        public SimulationManager(DataModel dataModel)
        {
            // Set up simulations
            // Stocks
            stocksSimulation = new StocksSimulation();
            stocksProfile = new RunProfile(InvestmentType.Stocks, dataModel);

            // Bonds
            bondsSimulation = new BondsSimulation();
            bondsProfile = new RunProfile(InvestmentType.Bonds, dataModel);

            // Savings
            savingsSimulation = new SavingsSimulation();
            savingsProfile = new RunProfile(InvestmentType.Savings, dataModel);
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

            int trialLength = stocksProfile.TrialLength;
            double withdrawalAmount = stocksProfile.WithdrawalAmount +
                                      bondsProfile.WithdrawalAmount +
                                      savingsProfile.WithdrawalAmount;

            // Aggregate trials
            var portfolios = trials["stocks"].Select((trial, i) =>
            {
                return trial.Select((value, j) =>
                {
                    return value + trials["bonds"][i][j] + trials["savings"][i][j];
                }).ToArray();
            }).ToArray();

            // Get success rate
            int numberOfSuccesses = 0;
            for (var i = 0; i < MonteCarloSimulation.NUM_TRIALS; i++)
            {
                if (portfolios[i][trialLength - 2] >= withdrawalAmount)
                {
                    numberOfSuccesses++;
                }
            }
            result.SuccessRate = (int)Math.Round(100 * numberOfSuccesses / (double)MonteCarloSimulation.NUM_TRIALS);
            
            // Get percentiles
            portfolios.ParallelMergeSort(CompareTrials);
            result.PortfolioPercentiles = portfolios.Where((trial, index) =>
            {
                // Make sure that NUM_TRIALS - 1 in MonteCarloSimulation is an even multiple of NUM_PERCENTILES - 1
                return index % ((portfolios.Length - 1) / (NUM_PERCENTILES - 1)) == 0;
            });

            // Get return rates
            var portfolioContribution = stocksProfile.ContributionAmount +
                                        bondsProfile.ContributionAmount +
                                        savingsProfile.ContributionAmount;
            var portfolioWithdrawal = stocksProfile.WithdrawalAmount +
                                      bondsProfile.WithdrawalAmount +
                                      savingsProfile.WithdrawalAmount;

            var returnRates = portfolios.SelectMany(trial =>
            {
                return trial.Skip(1).Select((value, index) =>
                {
                    if (index < savingsProfile.ContributionLength - 1)
                    {
                        return trial[index] == 0 || value == 0 ? 0 : (value - trial[index] - portfolioContribution) / (trial[index] + portfolioContribution);
                    }
                    else
                    {
                        return trial[index] == 0 || value == 0 ? 0 : (value - trial[index] + portfolioWithdrawal) / (trial[index] - portfolioWithdrawal);
                    }
                });
            });

            // Get frequencies of return rates
            result.FrequencyPeak = returnRates.Where(x => x != 0).Average();
            switch (stocksProfile.StepDistribution.Type)
            {
                default:
                    result.FrequencyScale = Math.Sqrt(returnRates.Where(x => x != 0).Aggregate((sum, next) => sum + Math.Pow(result.FrequencyPeak - next, 2))
                        / (returnRates.Count() - 1));
                    break;
            }

            double[] rateBrackets = new double[19];
            for (var i = -9; i <= 9; i++)
            {
                rateBrackets[i + 9] = result.FrequencyPeak + result.FrequencyScale * i / 3.0;
            }

            int[] rateFrequencies = new int[20];
            foreach (var rate in returnRates)
            {
                if (rate == 0)
                {
                    continue;
                }
                var i = 0;
                while (i < 19 && rate > rateBrackets[i])
                {
                    i++;
                }
                rateFrequencies[i]++;
            }
            result.ReturnRateFrequencies = rateFrequencies;
            


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

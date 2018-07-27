using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            PortfoliosEnsemble trials = new PortfoliosEnsemble();

            Task<Trial[]> stocks = Task<Trial[]>.Factory.StartNew(() =>
            {
                return stocksSimulation.Run(withProfile: stocksProfile);
            });

            Task<Trial[]> bonds = Task<Trial[]>.Factory.StartNew(() =>
            {
                return bondsSimulation.Run(withProfile: bondsProfile);
            });

            Task<Trial[]> savings = Task<Trial[]>.Factory.StartNew(() =>
            {
                return savingsSimulation.Run(withProfile: savingsProfile);
            });

            trials.StocksTrials = stocks.Result;
            trials.BondsTrials = bonds.Result;
            trials.SavingsTrials = savings.Result;
            

            return ProcessTrials(trials);
        }

        private Result ProcessTrials(PortfoliosEnsemble trials)
        {
            Result result = new Result();

            int trialLength = stocksProfile.TrialLength;
            double withdrawalAmount = stocksProfile.WithdrawalAmount +
                                      bondsProfile.WithdrawalAmount +
                                      savingsProfile.WithdrawalAmount;

            // Aggregate trials
            var portfolios = new Portfolio[MonteCarloSimulation.NUM_TRIALS];
            for (var i = 0; i < portfolios.Length; i++)
            {
                portfolios[i] = new Portfolio
                {
                    Stocks = trials.StocksTrials[i],
                    Bonds = trials.BondsTrials[i],
                    Savings = trials.SavingsTrials[0], // They are all the same, 0 takes least time to access probably
                    Total = new Trial()
                };

                portfolios[i].Total.Balances = new double[trialLength];
                for (var j = 0; j < trialLength; j++)
                {
                    portfolios[i].Total.Balances[j] = trials.StocksTrials[i].Balances[j] +
                                        trials.BondsTrials[i].Balances[j] +
                                        trials.SavingsTrials[i].Balances[j];
                }
            }

            
            // Get success rate
            Task<int> successRate = new Task<int>(() =>
            {
                int numberOfSuccesses = 0;
                for (var i = 0; i < MonteCarloSimulation.NUM_TRIALS; i++)
                {
                    if (portfolios[i].Total.Balances[trialLength - 2] >= withdrawalAmount)
                    {
                        numberOfSuccesses++;
                    }
                }
                return (int)Math.Round(100 * numberOfSuccesses / (double)MonteCarloSimulation.NUM_TRIALS);
            });

            
            // Get percentiles
            IEnumerable<double[]> findPercentiles()
            {
                for (int i = 0; i < portfolios.Length; i++)
                {
                    // Make sure that NUM_TRIALS - 1 in MonteCarloSimulation is an even multiple of NUM_PERCENTILES - 1
                    if (i % ((portfolios.Length - 1) / (NUM_PERCENTILES - 1)) == 0)
                    {
                        yield return portfolios[i].Total.Balances;
                    }
                }
            }
            Task<IEnumerable<double[]>> getPercentiles = new Task<IEnumerable<double[]>>(findPercentiles);

            
            // Look at distribution of stock returns
            Task getStocksReturns = new Task(() =>
            {
                (result.StocksReturnRateFrequencies, result.StocksReturnRateXLabels) =
                    GetDistributionOf(trials.StocksTrials.SelectMany(trial => trial.ReturnRates));
            });

            
            // Look at distribution of bond returns
            Task getBondsReturns = new Task(() =>
            {
                (result.BondsReturnRateFrequencies, result.BondsReturnRateXLabels) =
                    GetDistributionOf(trials.BondsTrials.SelectMany(trial => trial.ReturnRates));
            });


            Task getPeaksAndEnds = new Task(() =>
            {
                List<double> stockPeaks = new List<double>(3);
                List<double> bondPeaks = new List<double>(3);
                List<double> stockEnds = new List<double>(3);
                List<double> bondEnds = new List<double>(3);

                var n = MonteCarloSimulation.NUM_TRIALS;

                double[] allStockPeaks = new double[n];
                double[] allStockEnds = new double[n];
                double[] allBondPeaks = new double[n];
                double[] allBondEnds = new double[n];

                for (int i = 0; i < n; i++)
                {
                    allStockPeaks[i] = trials.StocksTrials[i].Peak;
                    allStockEnds[i] = trials.StocksTrials[i].Final;
                    allBondPeaks[i] = trials.BondsTrials[i].Peak;
                    allBondEnds[i] = trials.BondsTrials[i].Final;
                }

                allStockPeaks.ParallelMergeSort();
                allStockEnds.ParallelMergeSort();
                allBondPeaks.ParallelMergeSort();
                allBondEnds.ParallelMergeSort();

                for (var i = 1; i < 4; i++)
                {
                    var index = n / 4 * i;
                    stockPeaks.Add(allStockPeaks[index] - (i == 1 ? 0 : stockPeaks.Sum()));
                    bondPeaks.Add(allBondPeaks[index] - (i == 1 ? 0 : bondPeaks.Sum()));
                    stockEnds.Add(allStockEnds[index] - (i == 1 ? 0 : stockEnds.Sum()));
                    bondEnds.Add(allBondEnds[index] - (i == 1 ? 0 : bondEnds.Sum()));
                }

                result.StocksRetirementAmounts = stockPeaks;
                result.BondsRetirementAmounts = bondPeaks;
                result.StocksEndAmounts = stockEnds;
                result.BondsEndAmounts = bondEnds;
            });

            // Sort the trials
            portfolios.ParallelMergeSort(CompareTrials);


            // Run each task
            getPercentiles.Start();
            successRate.Start();
            getStocksReturns.Start();
            getBondsReturns.Start();
            getPeaksAndEnds.Start();
            

            result.PortfolioPercentiles = getPercentiles.Result;
            result.SuccessRate = successRate.Result;
            getBondsReturns.Wait();
            getStocksReturns.Wait();
            getPeaksAndEnds.Wait();

            return result;
        }

        #region Helper Methods

        private (int[], double[]) GetDistributionOf(IEnumerable<double> samples)
        {
            var sum = 0.0;
            var number = 0;
            foreach (var sample in samples)
            {
                if (sample != 0)
                {
                    sum += sample;
                    number++;
                }
            }
            var mean = sum / number;

            var absDev = 0.0;
            foreach (var sample in samples)
            {
                if (sample != 0)
                {
                    absDev += (sample - mean) * (sample - mean);
                }
            }
            var stdDev = Math.Sqrt(absDev / number);

            double[] brackets = new double[20];
            for (var i = -9; i <= 10; i++)
            {
                brackets[i + 9] = mean + stdDev * i / 3.0 + (stdDev / 6.0);
            }

            int[] frequencies = new int[21];
            foreach (var sample in samples)
            {
                if (sample != 0)
                {
                    var i = 0;
                    while (i < 20 && sample > brackets[i])
                    {
                        i++;
                    }
                    frequencies[i]++;
                }
            }

            return (frequencies, brackets);
        }

        private int CompareTrials(Portfolio a, Portfolio b)
        {
            if (a.Total.Balances.Length > b.Total.Balances.Length)
            {
                return 1;
            }
            else if (b.Total.Balances.Length > a.Total.Balances.Length)
            {
                return -1;
            }

            for (var i = a.Total.Balances.Length - 1; i >= 0; i--)
            {
                if (a.Total.Balances[i] == 0 && b.Total.Balances[i] != 0)
                {
                    return -1;
                }
                else if (b.Total.Balances[i] == 0 && a.Total.Balances[i] != 0)
                {
                    return 1;
                }
                else if (a.Total.Balances[i] != 0 && b.Total.Balances[i] != 0)
                {
                    if (a.Total.Balances[i] == b.Total.Balances[i])
                    {
                        continue;
                    }
                    return a.Total.Balances[i] > b.Total.Balances[i] ? 1 : -1;
                }
            }

            return 0;
        }

        #endregion

        #region Structs

        private struct PortfoliosEnsemble
        {
            public Trial[] StocksTrials;
            public Trial[] BondsTrials;
            public Trial[] SavingsTrials;
        }

        private struct Portfolio
        {
            public Trial Stocks;
            public Trial Bonds;
            public Trial Savings;
            public Trial Total;
        }

        #endregion
    }
}

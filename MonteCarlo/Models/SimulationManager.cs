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
            Trials trials = new Trials();

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

        private Result ProcessTrials(Trials trials)
        {
            Result result = new Result();

            int trialLength = stocksProfile.TrialLength;
            double withdrawalAmount = stocksProfile.WithdrawalAmount +
                                      bondsProfile.WithdrawalAmount +
                                      savingsProfile.WithdrawalAmount;

            // Aggregate trials
            var portfolios = new double[MonteCarloSimulation.NUM_TRIALS][];
            for (var i = 0; i < portfolios.Length; i++)
            {
                portfolios[i] = new double[trialLength];
                for (var j = 0; j < trialLength; j++)
                {
                    portfolios[i][j] = trials.StocksTrials[i].Balances[j] +
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
                    if (portfolios[i][trialLength - 2] >= withdrawalAmount)
                    {
                        numberOfSuccesses++;
                    }
                }
                return (int)Math.Round(100 * numberOfSuccesses / (double)MonteCarloSimulation.NUM_TRIALS);
            });

            
            // Get percentiles
            Task<IEnumerable<double[]>> percentiles = new Task<IEnumerable<double[]>>(() =>
            {
                return portfolios.Where((trial, index) =>
                {
                    // Make sure that NUM_TRIALS - 1 in MonteCarloSimulation is an even multiple of NUM_PERCENTILES - 1
                    return index % ((portfolios.Length - 1) / (NUM_PERCENTILES - 1)) == 0;
                });
            });

            
            // Look at distribution of stock returns
            Task stocksReturns = new Task(() =>
            {
                var distribution = GetDistributionOf(trials.StocksTrials.SelectMany(trial => trial.ReturnRates));
                result.StocksFrequencyPeak = distribution.Mean;
                result.StocksFrequencyScale = distribution.StdDev;
                result.StocksReturnRateFrequencies = distribution.Distribution;
            });

            
            // Look at distribution of bond returns
            Task bondsReturns = new Task(() =>
            {
                var distribution = GetDistributionOf(trials.BondsTrials.SelectMany(trial => trial.ReturnRates));
                result.BondsFrequencyPeak = distribution.Mean;
                result.BondsFrequencyScale = distribution.StdDev;
                result.BondsReturnRateFrequencies = distribution.Distribution;
            });


            //Task calculatePeaks = new Task(() =>
            //{
            //    List<double> stockPeaks = new List<double>(3);
            //    List<double> bondPeaks = new List<double>(3);
            //    for (var i = 1; i < 4; i++)
            //    {
                    
            //    }
            //});

            // Sort the trials
            portfolios.ParallelMergeSort(CompareTrials);


            // Run each task
            percentiles.Start();
            successRate.Start();
            stocksReturns.Start();
            bondsReturns.Start();
            

            result.PortfolioPercentiles = percentiles.Result;
            result.SuccessRate = successRate.Result;
            bondsReturns.Wait();
            stocksReturns.Wait();

            return result;
        }

        private ExperimentalDistribution GetDistributionOf(IEnumerable<double> samples)
        {
            var toReturn = new ExperimentalDistribution();

            if (samples.Count() == 0)
            {
                toReturn.Mean = 0;
                toReturn.StdDev = 0;
                toReturn.Distribution = new int[0];
            }

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

            double[] brackets = new double[19];
            for (var i = -9; i <= 9; i++)
            {
                brackets[i + 9] = mean + stdDev * i / 3.0;
            }

            int[] frequencies = new int[20];
            foreach (var sample in samples)
            {
                if (sample != 0)
                {
                    var i = 0;
                    while (i < 19 && sample > brackets[i])
                    {
                        i++;
                    }
                    frequencies[i]++;
                }
            }

            toReturn.Mean = mean;
            toReturn.StdDev = stdDev;
            toReturn.Distribution = frequencies;

            return toReturn;
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

        private struct Trials
        {
            public Trial[] StocksTrials;
            public Trial[] BondsTrials;
            public Trial[] SavingsTrials;
        }

        private struct ExperimentalDistribution
        {
            public double Mean;
            public double StdDev;
            public int[] Distribution;
        }
    }
}

using System;
using System.Collections.Generic;
using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Models
{
    public class Constants
    {
        public static Dictionary<DataStartDate, Dictionary<string, double>> GBMValues = new Dictionary<DataStartDate, Dictionary<string, double>>()
        {
            {
                DataStartDate._1928, new Dictionary<string, double>()
                {
                    { "drift", 0.05369 },
                    { "volatility", 0.18962 }
                }
            },
            {
                DataStartDate._1975, new Dictionary<string, double>()
                {
                    { "drift", 0.09383 },
                    { "volatility", 0.16956 }
                }
            },
            {
                DataStartDate._2000, new Dictionary<string, double>()
                {
                    { "drift", 0.0817 },
                    { "volatility", 0.12694 }
                }
            }
        };

        public static Dictionary<DataStartDate, Dictionary<Distribution, Dictionary<string, double>>> BondValues = new Dictionary<DataStartDate, Dictionary<Distribution, Dictionary<string, double>>>()
        {
            {
                DataStartDate._1928, new Dictionary<Distribution, Dictionary<string, double>>()
                {
                    {
                        Distribution.Normal, new Dictionary<string, double>()
                        {
                            { "peak", -0.013 },
                            { "scale", 0.745 }
                        }
                    },
                    {
                        Distribution.Laplace, new Dictionary<string, double>()
                        {
                            { "peak", -0.01 },
                            { "scale", 0.61 }
                        }
                    },
                    {
                        Distribution.Logistic, new Dictionary<string, double>()
                        {
                            { "peak", -0.013 },
                            { "scale", 0.745 * Math.Sqrt(3) / Math.PI }
                        }
                    },
                    {
                        Distribution.LogNormal, new Dictionary<string, double>()
                        {
                            { "peak", -0.013 },
                            { "scale", 0.745 }
                        }
                    }
                }
            },
            {
                DataStartDate._1975, new Dictionary<Distribution, Dictionary<string, double>>()
                {
                    {
                        Distribution.Normal, new Dictionary<string, double>()
                        {
                            { "peak", -0.123 },
                            { "scale", 0.961 }
                        }
                    },
                    {
                        Distribution.Laplace, new Dictionary<string, double>()
                        {
                            { "peak", -0.12 },
                            { "scale", 0.811 }
                        }
                    },
                    {
                        Distribution.Logistic, new Dictionary<string, double>()
                        {
                            { "peak", -0.123 },
                            { "scale", 0.961 * Math.Sqrt(3) / Math.PI }
                        }
                    },
                    {
                        Distribution.LogNormal, new Dictionary<string, double>()
                        {
                            { "peak", -0.123 },
                            { "scale", 0.961 }
                        }
                    }
                }
            },
            {
                DataStartDate._2000, new Dictionary<Distribution, Dictionary<string, double>>()
                {
                    {
                        Distribution.Normal, new Dictionary<string, double>()
                        {
                            { "peak", -0.176 },
                            { "scale", 0.573 }
                        }
                    },
                    {
                        Distribution.Laplace, new Dictionary<string, double>()
                        {
                            { "peak", -0.13 },
                            { "scale", 0.552 }
                        }
                    },
                    {
                        Distribution.Logistic, new Dictionary<string, double>()
                        {
                            { "peak", -0.176 },
                            { "scale", 0.573 * Math.Sqrt(3) / Math.PI }
                        }
                    },
                    {
                        Distribution.LogNormal, new Dictionary<string, double>()
                        {
                            { "peak", -0.176 },
                            { "scale", 0.573 }
                        }
                    }
                }
            }
        };
    }
}

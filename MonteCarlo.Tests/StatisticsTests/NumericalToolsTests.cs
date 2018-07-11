using System;
using Xunit;
using MonteCarlo.Models.Statistics;

namespace MonteCarlo.Tests.StatisticsTests
{
    public class NumericalToolsTests
    {
        [Fact]
        public void TestDifferentiate()
        {
            MathFunction f = Math.Exp;
            MathFunction Df = Math.Exp;

            Assert.Equal(Df(-5), NumericalTools.Differentiate(f)(-5), 2);
            Assert.Equal(Df(-1), NumericalTools.Differentiate(f)(-1), 2);
            Assert.Equal(Df(0), NumericalTools.Differentiate(f)(0), 2);
            Assert.Equal(Df(1), NumericalTools.Differentiate(f)(1), 2);
            Assert.Equal(Df(5), NumericalTools.Differentiate(f)(5), 2);

            f = Math.Sin;
            Df = Math.Cos;

            Assert.Equal(Df(-5), NumericalTools.Differentiate(f)(-5), 2);
            Assert.Equal(Df(-1), NumericalTools.Differentiate(f)(-1), 2);
            Assert.Equal(Df(0), NumericalTools.Differentiate(f)(0), 2);
            Assert.Equal(Df(1), NumericalTools.Differentiate(f)(1), 2);
            Assert.Equal(Df(5), NumericalTools.Differentiate(f)(5), 2);

            f = x => Math.Pow(x, 3);
            Df = x => 3 * Math.Pow(x, 2);

            Assert.Equal(Df(-5), NumericalTools.Differentiate(f)(-5), 2);
            Assert.Equal(Df(-1), NumericalTools.Differentiate(f)(-1), 2);
            Assert.Equal(Df(0), NumericalTools.Differentiate(f)(0), 2);
            Assert.Equal(Df(1), NumericalTools.Differentiate(f)(1), 2);
            Assert.Equal(Df(5), NumericalTools.Differentiate(f)(5), 2);
        }

        [Fact]
        public void TestIntegrate()
        {
            // Values found with WolframAlpha

            double a = -4;
            double b = 5;
            MathFunction f = Math.Exp;

            Assert.Equal(148.39, NumericalTools.Integrate(f, a, b), 2);

            a = 1;
            b = 2;
            f = x => 1 / x;

            Assert.Equal(0.69315, NumericalTools.Integrate(f, a, b), 2);
        }

        [Fact]
        public void TestFindRoot()
        {
            double guess = 0;
            MathFunction f = x => Math.Pow(x, 2);

            Assert.Equal(0, NumericalTools.FindRoot(f, guess), 2);

            guess = 5;
            f = x => Math.Pow(x, 2) - 1;

            Assert.Equal(1, NumericalTools.FindRoot(f, guess), 2);

            guess = -3;
            MathFunction Df = x => 2 * x;

            Assert.Equal(-1, NumericalTools.FindRoot(f, Df, guess), 2);
        }
    }
}

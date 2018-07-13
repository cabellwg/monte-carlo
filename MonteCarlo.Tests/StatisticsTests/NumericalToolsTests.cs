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
            MathFunction f = x => Math.Exp(x) - 1;

            Assert.Equal(0, NumericalTools.FindRoot(f, -1, 5), 2);

            f = x => Math.Pow(x, 3) - 1;

            Assert.Equal(1, NumericalTools.FindRoot(f, -1, 5), 2);

            f = x => Math.Sin(x);

            Assert.Equal(Math.PI, NumericalTools.FindRoot(f, Math.PI / 2, 3 * Math.PI / 2), 2);
        }

        [Fact]
        public void TestInverse()
        {
            MathFunction f = Math.Exp;

            Assert.Equal(0, NumericalTools.Inverse(f, -10, 10)(1), 2);
            Assert.Equal(1, NumericalTools.Inverse(f, -10, 10)(Math.E), 2);

            f = x => 2 * x;

            Assert.Equal(2, NumericalTools.Inverse(f, -10, 10)(4), 2);
            Assert.Equal(-1, NumericalTools.Inverse(f, -10, 10)(-2), 2);
        }
    }
}

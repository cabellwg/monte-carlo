using System;

namespace MonteCarlo.Models.Statistics
{
    public delegate double MathFunction(double x);

    public class NumericalTools
    {
        private const int SIMPSONS_RULE_RESOLUTION = 1000000;
        private const int NEWTONS_METHOD_RESOLUTION = 1000000;
        private const int NUMERICAL_DIFFERENTIATION_RESOLUTION = 1000000;

        // Simpson's Rule approximation of the integral of a function
        public static double Integrate(MathFunction f,
            double a,
            double b,
            int stepSize = SIMPSONS_RULE_RESOLUTION)
        {
            stepSize = (stepSize % 2 == 1) ? stepSize + 1 : stepSize;

            var h = (b - a) / stepSize;
            var s = f(a) + f(b);

            for (var i = 1; i < stepSize; i += 2)
            {
                s += 4 * f(a + i * h);
            }
            for (var i = 2; i < stepSize - 1; i += 2)
            {
                s += 2 * f(a + i * h);
            }

            return s * h / 3;
        }

        // Newton-Raphson method for root finding
        public static double FindRoot(MathFunction f,
            MathFunction Df,
            double guess,
            int stepSize = NEWTONS_METHOD_RESOLUTION)
        {
            var x = guess;
            var currentError = f(x) / Df(x);
            for (var i = 0; i < stepSize; i++)
            {
                var function = f(x);
                if (function == 0)
                {
                    return x;
                }
                var derivative = Df(x);
                if (derivative != 0) {
                    x -= currentError;
                    currentError = f(x) / derivative;
                } else {
                    // perturb x
                    x += 0.001;
                }
            }

            return x;
        }

        public static double FindRoot(MathFunction f,
            double guess,
            int differentiationStepSize = NUMERICAL_DIFFERENTIATION_RESOLUTION,
            int rootFinderStepSize = NEWTONS_METHOD_RESOLUTION)
        {
            MathFunction Df = Differentiate(f);
            return FindRoot(f, Df, guess);
        }

        // Basic numerical differentiation
        public static MathFunction Differentiate(MathFunction f,
            int stepSize = NUMERICAL_DIFFERENTIATION_RESOLUTION)
        {
            return x => (f(x + 1 / (double)stepSize) - f(x)) / (1 / (double)stepSize);
        }

        public double Inverse(MathFunction f)
        {
            throw new NotImplementedException();
        }
    }
}

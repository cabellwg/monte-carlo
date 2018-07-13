using System;
using System.Threading;
using System.Threading.Tasks;

namespace MonteCarlo.Models.Statistics
{
    public delegate double MathFunction(double x);

    public class NumericalTools
    {
        private static Mutex m = new Mutex();
        private const int NUMERICAL_INTEGRATION_RESOLUTION = 10000;
        private const int ROOT_FIND_RESOLUTION = 10000;
        private const int NUMERICAL_DIFFERENTIATION_RESOLUTION = 10000;

        // Simpson's Rule approximation of the integral of a function
        public static double Integrate(MathFunction f,
            double a,
            double b,
            int stepSize = NUMERICAL_INTEGRATION_RESOLUTION)
        {
            stepSize = (stepSize % 2 == 1) ? stepSize + 1 : stepSize;

            var h = (b - a) / stepSize;
            var s = f(a) + f(b);

            Task<double> odds = Task<double>.Factory.StartNew(() =>
            {
                var t = 0.0;
                for (var i = 1; i < stepSize; i += 2)
                {
                    t += 4 * f(a + i * h);
                }
                return t;
            });

            Task<double> evens = Task<double>.Factory.StartNew(() =>
            {
                var t = 0.0;
                for (var i = 2; i < stepSize - 1; i += 2)
                {
                    t += 2 * f(a + i * h);
                }
                return t;
            });

            s = odds.Result + evens.Result;

            return s * h / 3;
        }


        // Brent's method for root finding
        public static double FindRoot(MathFunction f,
            double min,
            double max,
            int rootFinderStepSize = ROOT_FIND_RESOLUTION)
        {
            var tolerance = 1 / (double)ROOT_FIND_RESOLUTION;
            
            var a = min;
            var b = max;
            var c = a;
            var d = 0.0;
            var s = 0.0;

            var fa = f(a);
            var fb = f(b);
            var fc = fa;
            var fs = 0.0;

            if (fa * fb >= 0)
            {
                return 0;
            }

            if (Math.Abs(fa) < Math.Abs(fb))
            {
                // Swap max and min
                var tmp = a;
                a = b;
                b = tmp;

                tmp = fa;
                fa = fb;
                fb = tmp;
            }

            bool mflag = true;
            for (var i = 1; i < ROOT_FIND_RESOLUTION; i++)
            {
                if (Math.Abs(b - a) < tolerance)
                {
                    return s;
                }

                if (fa != fc && fb != fc)
                {
                    s = (a * fb * fc / ((fa - fb) * (fa - fc)))
                      + (b * fa * fc / ((fb - fa) * (fb - fc)))
                      + (c * fa * fb / ((fc - fa) * (fc - fb)));
                } else
                {
                    s = b - fb * (b - a) / (fb - fa);
                }

                if ((s < ((3 * a + b) / 4) || s > b) ||
                    (mflag && Math.Abs(s - b) >= Math.Abs(b - c) / 2) ||
                    (!mflag && Math.Abs(s - b) >= Math.Abs(c - d) / 2) ||
                    (mflag && Math.Abs(b - c) < Math.Abs(tolerance)) ||
                    (!mflag && Math.Abs(c - d) < Math.Abs(tolerance)))
                {
                    s = (a + b) / 2;
                    mflag = true;
                } else
                {
                    mflag = false;
                }

                fs = f(s);
                d = c;
                c = b;
                fc = fb;

                if (fa * fs < 0)
                {
                    b = s;
                    fb = fs;
                } else
                {
                    a = s;
                    fa = fs;
                }
                
                if (Math.Abs(f(a)) < Math.Abs(f(b)))
                {
                    var tmp = a;
                    a = b;
                    b = tmp;

                    tmp = fa;
                    fa = fb;
                    fb = tmp;
                }
            }

            return s;
        }

        // Basic numerical differentiation
        public static MathFunction Differentiate(MathFunction f,
            int stepSize = NUMERICAL_DIFFERENTIATION_RESOLUTION)
        {
            return x => (f(x + 1 / (double)stepSize) - f(x)) / (1 / (double)stepSize);
        }

        // Find the inverse of a function
        public static MathFunction Inverse(MathFunction f, double min = -0x100000000, double max = 0x100000000) // 2^32
        {
            try
            {
                return y => FindRoot(x => f(x) - y, min, max);
            } catch (ArgumentException)
            {
                throw new ArgumentException("No inverse in specified range");
            }
        }
    }
}

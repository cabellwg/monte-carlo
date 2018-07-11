using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonteCarlo.Models.Statistics
{
    public class Ziggurat
    {
        private const int NUM_RECTS = 256;

        private double[] xLimits;
        private double[] yLimits;
        private double r0Area;
        private double bound;
        private MathFunction pdf;
        private MathFunction inverseOfPdf;
        private ThreadSafeRandom random;

        /* This runs in O(n^3 * O(forPdf) + O(inverseOfPdf)).
         * Call as few times as possible.
         */
        public Ziggurat(MathFunction forPdf, MathFunction inverseOfPdf, double bound, ThreadSafeRandom random)
        {
            this.random = random;
            this.inverseOfPdf = inverseOfPdf;
            this.bound = bound;
            pdf = forPdf;

            xLimits = new double[NUM_RECTS];
            yLimits = new double[NUM_RECTS];

            // Search for suitable area using numerical integration and Newton-Raphson root finder
            double guess = bound / 2;
            double goal = pdf(0);
            MathFunction currentTailArea = x => NumericalTools.Integrate(pdf, x, bound);
            MathFunction currentR0Area = x => pdf(x) * x;

            MathFunction topY = x =>
            {
                xLimits[0] = x;
                yLimits[0] = pdf(x);

                double area = currentTailArea(x) + currentR0Area(x);

                for (var i = 1; i < NUM_RECTS; i++)
                {
                    yLimits[i] = yLimits[i - 1] + (area / xLimits[i - 1]);
                    xLimits[i] = inverseOfPdf(yLimits[i]);
                }

                return yLimits[NUM_RECTS - 1];
            };

            double x0 = NumericalTools.FindRoot(x => goal - topY(x), guess);
            r0Area = currentR0Area(x0);
        }

        public double Sample()
        {
            int segment = (int)Math.Floor(random.NextDouble() * NUM_RECTS);

            if (segment == 0)
            {
                double w = random.NextDouble() * r0Area;
                if (w <= r0Area)
                {
                    // we are in R0
                    return w / yLimits[0];
                } else
                {
                    // we are in the tail
                    double x = random.NextDouble() * (bound - xLimits[0]) + xLimits[0];
                }
            }

            double candidate = random.NextDouble() * xLimits[segment];

            if (candidate < xLimits[segment + 1])
            {
                return candidate;
            }

            double y = random.NextDouble() * (yLimits[segment] - yLimits[segment - 1]);

            return y < pdf(candidate) ? candidate : Sample();
        }
    }
}

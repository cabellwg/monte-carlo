using System;

namespace MonteCarlo.Models.Statistics
{
    /* See http://heliosphan.org/zigguratalgorithm/zigguratalgorithm.html
     * for a full explanation of this algorithm.
     * 
     * See https://en.wikipedia.org/wiki/Ziggurat_algorithm#Generating_the_tables
     * for the setup in the constructor.
     */
    public class Ziggurat
    {
        private const int NUM_RECTS = 256;

        private double[] xLimits;
        private double[] yLimits;
        private double r0Area;
        private double bound;
        private double mean;
        private MathFunction pdf;
        private MathFunction inverseOfCdf;
        private ProbabilityDistribution random;

        /* This runs in O(O(forPdf) + O(inverseOfPdf)).
         * Call as few times as possible.
         */
        public Ziggurat(MathFunction forPdf,
            double mean,
            double bound,
            ProbabilityDistribution randomNumberGenerator,
            int resolution = 1000000,
            MathFunction inverseOfCdf = null,
            MathFunction inverseOfPdf = null)
        {
            this.inverseOfCdf = inverseOfCdf;
            this.bound = bound;
            random = randomNumberGenerator;
            this.mean = mean;
            pdf = x => forPdf(x + mean);

            if (inverseOfCdf == null)
            {
                this.inverseOfCdf = NumericalTools.Inverse(x => NumericalTools.Integrate(pdf, 0, x), 0, 1);
            }

            if (inverseOfPdf == null)
            {
                inverseOfPdf = NumericalTools.Inverse(pdf, 0, bound);
            }

            xLimits = new double[NUM_RECTS];
            yLimits = new double[NUM_RECTS];

            // Search for suitable area using numerical integration
            double goal = pdf(0);
            double lowerBound = pdf(bound);
            double currentTailArea(double x) => NumericalTools.Integrate(pdf, x, bound);
            double currentR0Area(double x) => pdf(x) * x;

            double topY(double x)
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
            }

            double x0 = bound;

            if (topY(x0) > goal + 0.000001)
            {
                double top = topY(x0);
                throw new ArgumentException("Bound too small");
            }

            while (topY(x0) < goal)
            {
                x0 -= bound / 100;
            }

            x0 += bound / 100;

            while (topY(x0) < goal)
            {
                x0 -= bound / 1000;
            }

            x0 += bound / 1000;

            while (topY(x0) < goal)
            {
                var top = topY(x0);
                x0 -= bound / resolution;
            }

            x0 += bound / (2 * resolution);
            topY(x0);

            r0Area = currentR0Area(x0);
        }

        public double NextDouble()
        {
            int sign = random.NextDouble() < 0.5 ? 1 : -1;
            return (sign * RawSample()) + mean;
        }

        private double RawSample()
        {
            int segment = (int)Math.Floor(random.NextDouble() * (NUM_RECTS));

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
                    return inverseOfCdf(x);
                }
            }

            double candidate = random.NextDouble() * xLimits[segment];

            if (segment != NUM_RECTS - 1 && candidate < xLimits[segment + 1])
            {
                return candidate;
            }

            double y = random.NextDouble() * (yLimits[segment] - yLimits[segment - 1]);

            return y < pdf(candidate) ? candidate : RawSample();
        }
    }
}

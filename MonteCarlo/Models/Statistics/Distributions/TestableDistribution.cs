using System.Collections.Generic;

namespace MonteCarlo.Models.Statistics
{
    public class TestableDistribution : ProbabilityDistribution
    {
        public IEnumerable<double> Values { get; set; }

        private IEnumerator<double> iterator;

        public override MathFunction Distribution => x => 0;

        public TestableDistribution(IEnumerable<double> values)
        {
            Type = Statistics.Distribution.Testable;
            Values = values;
            iterator = Values.GetEnumerator();
        }

        public override double NextDouble()
        {
            iterator.MoveNext();
            var value = iterator.Current;
            return value;
        }
    }
}

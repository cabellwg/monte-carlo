using System.Collections.Generic;

namespace MonteCarlo.Models
{
    public class Result
    {
        public double SuccessRate { get; set; }
        public Dictionary<string, double[][]> Trials { get; set; }
    }
}

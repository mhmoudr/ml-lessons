using System;
using System.Linq;

namespace DecisionTree
{
    public static class Entropy
    {
        public static double Calculate(string[] data)
        {
            return data
                .GroupBy(v => v)
                .Select(g =>
                {
                    var p = (double)g.Count() / data.Length;
                    return -p * Math.Log(p, 2);
                })
                .Sum();
        }

    }
}
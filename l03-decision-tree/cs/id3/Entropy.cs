using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace id3
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
                    return -p * math.Log(p);
                })
                .Sum();
        }

    }
}
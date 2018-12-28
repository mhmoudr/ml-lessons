using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace id3
{
    public static class Entropy
    {
        public static double Calculate(Data data, string targetColumn)
        {
            var idx = data.Columns[targetColumn];
            var count = data.Rows.Count;
            return data.Rows.GroupBy(r => r[idx]).Select(g =>
            {
                var p = (double) g.Count() / count;
                return -p * Math.Log(p, 2);
            }).Sum();
        }
    }
}
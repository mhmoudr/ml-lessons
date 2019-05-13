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
            return Calculate(data.Rows.Select(r=>r[idx]).ToArray());
        }
        public static double Calculate(string[] column)
        {
            return column
                .GroupBy(v => v)
                .Select(g =>
                {
                    var p = (double) g.Count() / column.Length;
                    return -p * Math.Log(p, 2);
                })
                .Sum();
        }

    }
}
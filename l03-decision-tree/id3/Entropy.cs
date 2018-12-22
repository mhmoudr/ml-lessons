using System;
using System.Linq;

namespace id3
{
    public class Entropy
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
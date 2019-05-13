using System;
using System.Linq;

namespace id3
{
    public static class InformationGain
    {
        public static double Calculate(Data data, string labelColumn, string targetColumn)
        {
            var tgtIdx = data.Columns[targetColumn];
            var lblIdx = data.Columns[labelColumn];
            return Calculate(data.Rows.Select(r => (r[tgtIdx], r[lblIdx])).ToArray());
        }
        public static double Calculate((string y, string x)[] data)
        {
            return Entropy.Calculate(data.Select(r => r.y).ToArray()) -
                data.GroupBy(r => r.x)
                    .Select(g => ((double)g.Count() / data.Length) * Entropy.Calculate(g.Select(r => r.y).ToArray()))
                    .Sum();
        }
    }
}
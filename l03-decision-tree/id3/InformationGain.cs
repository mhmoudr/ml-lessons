using System;
using System.Linq;

namespace id3
{
    public static class InformationGain
    {
        public static double Calculate(Data data, string labelColumn,string targetColumn)
        {
            var tgtIdx = data.Columns[targetColumn];
            var lblIdx = data.Columns[labelColumn];
            var labelEntropy = Entropy.Calculate(data, labelColumn);
            var dataCount = (double)data.Rows.Count;
            var sumResult = data.Rows.GroupBy(r => r[tgtIdx]).Select(tgtGroup =>
            {
                var tgtGroupCount = (double)tgtGroup.Count();
                Console.WriteLine($"grouping by {targetColumn} / {tgtGroup.Key}: count was: {tgtGroupCount}");
                var tgtGroupEntropy = tgtGroup.GroupBy(gg=>gg[lblIdx]).Select(g =>
                {
                    var p = g.Count() / tgtGroupCount;
                    Console.WriteLine($"probability of {g.Key} is {p}");
                    return -p * Math.Log(p, 2);
                }).Sum();
                Console.WriteLine($"target group entropy={tgtGroupEntropy}");
                return (tgtGroupCount / dataCount) * tgtGroupEntropy;
            }).Sum();
            Console.WriteLine($"entropy: {labelEntropy} and sub Entropy:{sumResult}");
            return labelEntropy - sumResult;
        }
    }
}
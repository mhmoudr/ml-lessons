using System.Linq;

namespace DecisionTree
{
    public static class InformationGainRatio
    {
        public static double Calculate(Data data, string column)
        {
            var colIdx = data.Columns[column];
            return Calculate(data.Rows.Select(r => (r.lable,r.factors[colIdx])).ToArray());
        }
        public static double Calculate((string y,string x)[] data)
        {
            return (Entropy.Calculate(data.Select(r => r.y).ToArray()) - data
                .GroupBy(r => r.x)
                .Select(g => ((double)g.Count() / data.Length) * Entropy.Calculate(g.Select(r => r.y).ToArray()))
                .Sum()) / Entropy.Calculate(data.Select(r => r.x).ToArray());
        }
    }
}
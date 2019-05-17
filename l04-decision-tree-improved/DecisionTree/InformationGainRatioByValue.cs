using System.Linq;

namespace DecisionTree
{
    public static class InformationGainRatioByValue
    {
        public static (double gain, string val) Calculate(Data data, string column)
        {
            var colIdx = data.Columns[column];
            return Calculate(data.Rows.Select(r => (r.lable,r.factors[colIdx])).ToArray());
        }
        public static (double gain, string val) Calculate((string y,string x)[] data)
        {
            var e = Entropy.Calculate(data.Select(r => r.x).ToArray());
            return data
                .GroupBy(r => r.x)
                .Select(g =>((Entropy.Calculate(data.Select(r => r.y).ToArray()) - ((double)g.Count() / data.Length) * Entropy.Calculate(g.Select(r => r.y).ToArray()))/e,g.Key))
                .OrderByDescending(x=>x.Item1)
                .First();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
namespace id3
{
    public static class Id3
    {
        public static Node Train(Data data, string labelColumn, string[] features, int maxDepth = 10)
        {
            var node = new Node(data);
            Train(node, labelColumn, features, 0, maxDepth);
            return node;
        }
        private static void Train(Node node, string labelColumn, string[] features, int currentLevel, int maxDepth)
        {
            var colWithMaxGain = FindBestSplit(node, labelColumn, features);
            node.SplittingColumn = colWithMaxGain;
            var possibleLeaf = IsItLeaf(node, labelColumn, currentLevel, maxDepth);
            if (possibleLeaf != null)
                node.Prediction = possibleLeaf;
            else
            {
                node.Children = GenerateChildrenNodes(node);
                foreach (var n in node.Children)
                    Train(n.Value, labelColumn, features, currentLevel + 1, maxDepth);
            }
        }
        private static string IsItLeaf(Node node, string labelColumn, int currentLevel, int maxDepth)
        {
            var lblIdx = node.Data.Columns[labelColumn];
            var labels = node.Data.Rows.Select(r => r[lblIdx]);
            string leafValue = null;
            if (currentLevel >= maxDepth)
            {
                leafValue = labels.GroupBy(l => l).OrderByDescending(g => g.Count()).First().Key;
            }
            else
            {
                var first = labels.First();
                leafValue = (labels.All(l => l == first)) ? first : null;
            }
            return leafValue;
        }
        private static (string col, double gain) FindBestSplit(Node node, string labelColumn, string[] features)
        {
            var gains = features.Select(col => (col, gain: InformationGain.Calculate(node.Data, labelColumn, col)));
            return gains.Aggregate((l, r) => l.gain > r.gain ? l : r); // similar to fold function in FP
        }
        private static Dictionary<string, Node> GenerateChildrenNodes(Node node)
        {
            var colIdx = node.Data.Columns[node.SplittingColumn.col];
            return node.Data.Rows
                .GroupBy(r => r[colIdx])
                .Select(g =>
                (
                    colName: g.Key,
                    Node: new Node(new Data(node.Data.Columns, g.ToList()))
                ))
                .ToDictionary(i => i.colName, i => i.Node);
        }
    }
}
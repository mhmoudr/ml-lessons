using System.Collections.Generic;
using System.Linq;
namespace id3
{
    public class Id3
    {
        public static Node Train(Data data, string labelColumn, string[] features)
        {
            var node = new Node(data);
            Train(node, labelColumn, features);
            return node;
        }
        private static void Train(Node node, string labelColumn, string[] features)
        {
            var colWithMaxGain = FindBestSplit(node, labelColumn, features);
            node.SplittingColumn = colWithMaxGain;
            node.Children = GenerateChildrenNodes(node);
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
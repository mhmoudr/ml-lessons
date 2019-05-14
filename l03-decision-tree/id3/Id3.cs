using System.Collections.Generic;
using System.Linq;
namespace id3
{
    public class Id3
    {
        public static Tree Train(Data data, int MaxDepth)
        {
            var tree = new Tree();
            Train(tree, data, 1, MaxDepth);
            return tree;
        }
        private static void Train(SplitNode node, Data data, int currentLevel, int MaxDepth)
        {
            var bestSplit = BestSplitColumn(data);
            var splitColIxd = data.Columns[bestSplit.columnName];
            node.Gain = bestSplit.gain;
            node.SplittingColumn = bestSplit.columnName;
            node.Children = data.Rows
                .GroupBy(r => r.factors[splitColIxd])
                .Select(g =>
                {
                    // provide predition with the most frequent response 
                    if (currentLevel >= MaxDepth)
                    {
                        Node res = new LeafNode() { Response = g.GroupBy(r => r.lable).OrderByDescending(gg => gg.Count()).First().Key };
                        return (g.Key, res);
                    }
                    //check if all responses are the same
                    var l0 = g.First().lable;
                    if (g.All(r => r.lable == l0))
                        return (g.Key, new LeafNode() { Response = l0 });
                    var newNode = new SplitNode();
                    Train(newNode, new Data() { 
                            Columns = data.Columns ,//.Where(c=>c.Key != node.SplittingColumn).ToDictionary(i=>i.Key,i=>i.Value), 
                            Rows = g.ToArray() }, 
                        currentLevel + 1, MaxDepth);
                    return (g.Key, newNode);
                })
                .ToDictionary(i => i.Key, i => i.res);

        }
        private static (string columnName, double gain) BestSplitColumn(Data data)
        {
            return data.Columns.Select(col => (col.Key, InformationGain.Calculate(data, col.Key))).OrderByDescending(i => i.Item2).First();
        }
    }
}
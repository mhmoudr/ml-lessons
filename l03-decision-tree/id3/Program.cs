using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
namespace id3
{
    class Program
    {
       static void Main(string[] args)
        {
            var featuresNum = 3;
            var lines = File.ReadLines("../train.csv");
            var data = new Data()
            {
                Columns = lines.First().Split(",").Select((value, index) => (value, index)).Take(featuresNum).ToDictionary(i => i.value, i => i.index),
                Rows = lines.Skip(1).Select(l => l.Split(",")).Select(r => (r[featuresNum], r.Take(featuresNum).ToArray())).ToArray()
            };
            var model = Id3.Train(data, 10);
            var result = model.Predict(data.Columns, new[] { "AfterPublicHoliday", "Rain", "Strong" });
            Console.WriteLine($"You prediction is: {result}");
            model.Print();
        }
    }
    public class Data
    {
        public Dictionary<string, int> Columns;
        public (string lable , string[] factors)[] Rows;
    }
    public abstract class Node
    {
    }
    public class Tree : SplitNode
    {
        public string Predict(Dictionary<string, int> columns, string[] factors)
        {
            return Predict(columns, factors, this);
        }
        private string Predict(Dictionary<string, int> columns, string[] factors, Node node)
        {
            switch (node)
            {
                case LeafNode ln:
                    return ln.Response;
                case SplitNode sn:
                    var splitIndex = columns[sn.SplittingColumn];
                    var childNode = sn.Children[factors[splitIndex]];
                    return Predict(columns, factors, childNode);
                default:
                    return null;
            }
        }
        public void Print()
        {
            Print(this,0);
        }
        private void Print(Node node, int level)
        {
            var indent =String.Join("", Enumerable.Repeat("   ",level));
            switch(node){
                case LeafNode ln:
                    Console.WriteLine($"{indent}Leaf Node with value: {ln.Response}");
                    break;
                case SplitNode sn:
                    foreach(var n in sn.Children){
                        Console.WriteLine($"{indent}When {sn.SplittingColumn} == {n.Key}, with gain of: {sn.Gain}");
                        Print(n.Value,level+1);
                    }
                    break;

            }
        }
    }
    public class LeafNode : Node
    {
        public string Response;
    }
    public class SplitNode : Node
    {
        public double Gain;
        public string SplittingColumn;
        public Dictionary<string, Node> Children;
    }
    public static class Entropy
    {
        public static double Calculate(string[] data) => 
            data.GroupBy(v => v)
                .Select(g =>
                {
                    var p = (double)g.Count() / data.Length;
                    return -p * Math.Log(p, 2);
                })
                .Sum();

    }
    public static class InformationGain
    {
        public static double Calculate(Data data, string column)
        {
            var colIdx = data.Columns[column];
            return Calculate(data.Rows.Select(r => (r.lable,r.factors[colIdx])).ToArray());
        }
        public static double Calculate((string y,string x)[] data)
        {
            return Entropy.Calculate(data.Select(r => r.y).ToArray()) - data
                .GroupBy(r => r.x)
                .Select(g => ((double)g.Count() / data.Length) * Entropy.Calculate(g.Select(r => r.y).ToArray()))
                .Sum();
        }
    }
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

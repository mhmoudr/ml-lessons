using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree
{
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
                    var childval  = factors[splitIndex];
                    if(!sn.Children.Keys.Contains(childval))
                        childval  = "null";
                    var childNode = sn.Children[childval];
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
}
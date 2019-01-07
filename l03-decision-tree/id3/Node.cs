using System;
using System.Collections.Generic;
namespace id3
{
    public class Node
    {
        public (string col, double gain) SplittingColumn { get; set; }
        public Dictionary<string, Node> Children { get; set; }
        public Data Data { get; private set; }
        public Node(Data data)
        {
            Data = data;
        }
        internal void print()
        {
            Console.WriteLine("Printing tree");
            Console.WriteLine("==============");
            print(0, this);
        }
        public String Prediction = null;
        public bool IsLeaf { get { return Prediction != null; } }
        bool HaveSplit { get { return (Children != null); } }
        void print(int level, Node node)
        {
            printSpace(level);
            if (!node.IsLeaf)
            {
                Console.WriteLine($"Node level {level} split by {node.SplittingColumn.col} ({node.SplittingColumn.gain}) for the following values:");
                level++;
                foreach (var n in node.Children)
                {
                    printSpace(level);
                    Console.WriteLine($"value {n.Key}:");
                    print(level + 1, n.Value);
                }
            }
            else
            {
                Console.WriteLine($"leaf node with value {node.Prediction}");
            }
        }
        void printSpace(int level)
        {
            for (var i = 0; i < level; i++)
                Console.Write("  ");
        }
    }
}
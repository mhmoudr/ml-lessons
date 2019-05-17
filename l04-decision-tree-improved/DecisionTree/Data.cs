using System.Collections.Generic;

namespace DecisionTree
{
    public class Data
    {
        public Dictionary<string, int> Columns;
        public (string lable , string[] factors)[] Rows;
    }
}
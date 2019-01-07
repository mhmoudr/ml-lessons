using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace id3
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = Repository.GetTrainStatusData();
            var label = "IsLate";
            var features = data.Columns.Keys.Where(c => c != label && c != "Day").ToArray();
            var node = Id3.Train(data, label, features);
            node.print();
        }
    }
}

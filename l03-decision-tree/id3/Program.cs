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
            var features = data.Columns.Keys.Where(c => c != "IsLate" && c != "Day").ToArray();
            var node = Id3.Train(data, "IsLate", features);
            node.print();
        }
    }
}

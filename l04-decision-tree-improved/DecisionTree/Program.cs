using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
namespace DecisionTree
{
    class Program
    {
       static void Main(string[] args)
        {
            var lines = File.ReadLines("../heart.csv");
            var data = new Data()
            {
                Columns = lines.First().Split(",").Select((value, index) => (value, index)).SkipLast(1).ToDictionary(i => i.value, i => i.index),
                Rows = lines.Skip(1).Select(l => l.Split(",")).Select(r => (r.Last(), r.SkipLast(1).ToArray())).ToArray()
            };
            var model = DecisionTree.Train(data, 3);
            // var result = model.Predict(data.Columns, new[] {"null", "AfterPublicHoliday", "Rain", "Strong" });
            // Console.WriteLine($"You prediction is: {result}");
            model.Print();
        }
    }
}

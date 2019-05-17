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
            var featuresNum = 4;
            var lines = File.ReadLines("../train.day.csv");
            var data = new Data()
            {
                Columns = lines.First().Split(",").Select((value, index) => (value, index)).Take(featuresNum).ToDictionary(i => i.value, i => i.index),
                Rows = lines.Skip(1).Select(l => l.Split(",")).Select(r => (r[featuresNum], r.Take(featuresNum).ToArray())).ToArray()
            };
            var model = DecisionTree.Train(data, 10);
            var result = model.Predict(data.Columns, new[] {"null", "AfterPublicHoliday", "Rain", "Strong" });
            Console.WriteLine($"You prediction is: {result}");
            model.Print();
        }
    }
}

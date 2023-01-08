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
}

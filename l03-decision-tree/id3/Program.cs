using System;
using System.Collections.Generic;
using System.Data;

namespace id3
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = Repository.GetTrainStatusData();
            var entropy = Entropy.Calculate(data, "IsLate");
            Console.WriteLine($"entropy is:{entropy}");
        }
    }
}

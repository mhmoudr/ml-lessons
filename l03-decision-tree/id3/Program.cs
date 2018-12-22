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
        }
    }
}

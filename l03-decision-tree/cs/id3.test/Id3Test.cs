using id3;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace Tests
{
    public class Id3Test
    {
        private Data data;
        private Tree node;
        [SetUp]
        public void Setup()
        {
            var featuresNum = 3;
            var lines = File.ReadLines("../../../../train.csv");
            data = new Data()
            {
                Columns = lines.First().Split(",").Select((value, index) => (value, index)).Take(featuresNum).ToDictionary(i => i.value, i => i.index),
                Rows = lines.Skip(1).Select(l => l.Split(",")).Select(r => (r[featuresNum], r.Take(featuresNum).ToArray())).ToArray()
            };
            var features = data.Columns.Keys.Where(c => c != "IsLate" && c != "Day").ToArray();
            node = Id3.Train(data, 10);
        }

        [Test]
        public void ShouldBreakRootNodeIntoThreeChildren()
        {
            Assert.That(node.Children.Count, Is.EqualTo(3));
        }
    }
}
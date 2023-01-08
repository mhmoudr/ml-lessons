using System.IO;
using System.Linq;
using id3;
using NUnit.Framework;

namespace Tests
{
    public class EntropyTest
    {
        private Data data;

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
        }

        [Test]
        public void EntropyCalculationIsCorrect()
        {
            var e = Entropy.Calculate(data.Rows.Select(r=>r.lable).ToArray());
            Assert.That(e, Is.EqualTo(0.940).Within(0.001));
        }
        [Test]
        public void TestMinEntropy()
        {
            var data = new[] { "A", "A", "A", "A", "A", "A", "A" };
            Assert.AreEqual(0d, Entropy.Calculate(data), 0.0001);
        }
        [Test]
        public void TestHighEntropy()
        {
            var data = new[] { "A", "A", "B", "B", "C", "C", "D", "D" };
            Assert.AreEqual(2d, Entropy.Calculate(data), 0.0001);
        }
        [Test]
        public void TestHighEntropy2()
        {
            var data = new[] { "A", "A", "A", "A", "C", "C", "D", "F" };
            Assert.AreEqual(1.75d, Entropy.Calculate(data), 0.0001);
        }
    }
}
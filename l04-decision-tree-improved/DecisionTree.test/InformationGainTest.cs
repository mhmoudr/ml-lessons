using System.IO;
using System.Linq;
using DecisionTree;
using NUnit.Framework;

namespace Tests
{
    public class InformationGainTest
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
        public void InformationGainReturnsCorrectForWindFactor()
        {
            var e = InformationGain.Calculate(data, "Wind");
            Assert.That(e, Is.EqualTo(0.048).Within(0.001));
        }
        [Test]
        public void InformationGainReturnsCorrectForOutlookFactor()
        {
            var e = InformationGain.Calculate(data, "Timing");
            Assert.That(e, Is.EqualTo(0.246).Within(0.001));
        }
        [Test]
        public void InformationGainReturnsCorrectForHumidityFactor()
        {
            var e = InformationGain.Calculate(data, "Weather");
            Assert.That(e, Is.EqualTo(0.151).Within(0.001));
        }
        [Test]
        public void InformationGainForTrivial1()
        {
            var data = new (string, string)[] { ("Y", "A"), ("Y", "A"), ("Y", "A"), ("Y", "A") };
            Assert.AreEqual(0d, InformationGain.Calculate(data), 0.0001);
        }
        [Test]
        public void InformationGainForHighParity()
        {
            var data = new (string, string)[] { ("Y", "A"), ("Y", "A"), ("N", "B"), ("N", "B") };
            Assert.AreEqual(1d, InformationGain.Calculate(data), 0.0001);
        }
    }
}
using id3;
using NUnit.Framework;

namespace Tests
{
    public class InformationGainTest
    {
        private Data data;

        [SetUp]
        public void Setup()
        {
            data = Repository.GetTrainStatusData();
        }

        [Test]
        public void InformationGainReturnsCorrectForWindFactor()
        {
            var e = InformationGain.Calculate(data, "IsLate", "Wind");
            Assert.That(e, Is.EqualTo(0.048).Within(0.001));
        }
        [Test]
        public void InformationGainReturnsCorrectForOutlookFactor()
        {
            var e = InformationGain.Calculate(data, "IsLate", "Outlook");
            Assert.That(e, Is.EqualTo(0.246).Within(0.001));
        }
        [Test]
        public void InformationGainReturnsCorrectForHumidityFactor()
        {
            var e = InformationGain.Calculate(data, "IsLate", "Humidity");
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
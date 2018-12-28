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
        public void InformationGainReturnsCorrect()
        {
            var e = InformationGain.Calculate(data, "IsLate", "Wind");
            Assert.That(e,Is.EqualTo(0.048).Within(0.001));
        }
    }
}
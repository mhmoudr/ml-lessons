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
            data = Repository.GetTrainStatusData();
        }

        [Test]
        public void EntropyCalculationIsCorrect()
        {
            var e = Entropy.Calculate(data, "IsLate");
            Assert.That(e, Is.EqualTo(0.940).Within(0.001));
        }
    }
}
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
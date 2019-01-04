using id3;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    public class Id3Test
    {
        private Data data;
        private Node node;
        [SetUp]
        public void Setup()
        {
            data = Repository.GetTrainStatusData();
            var features = data.Columns.Keys.Where(c => c != "IsLate" && c != "Day").ToArray();
            node = Id3.Train(data, "IsLate", features);
        }

        [Test]
        public void ShouldBreakRootNodeIntoThreeChildren()
        {
            Assert.That(node.Children.Count, Is.EqualTo(3));
        }
        [Test]
        public void ShouldBreakByOutlook()
        {
            Assert.AreEqual(node.SplittingColumn.col, "Outlook");
        }
        [Test]
        public void ShouldRespectColumnSelection()
        {
            var features = data.Columns.Keys.Where(c => c != "IsLate" && c != "Day" && c != "Outlook").ToArray();
            var n = Id3.Train(data, "IsLate", features);
            Assert.AreNotEqual(n.SplittingColumn.col, "Outlook");
        }
    }
}
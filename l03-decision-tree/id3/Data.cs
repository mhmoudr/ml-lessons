using System.Collections.Generic;

namespace id3
{
    public class Data
    {
        public readonly Dictionary<string, int> Columns;
        public readonly List<string[]> Rows;

        public Data()
        {
            Columns = new Dictionary<string, int>();
            Rows=new List<string[]>();
        }
    }
}
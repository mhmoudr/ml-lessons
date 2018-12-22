using System.Data;

namespace id3
{
    public static class Repository
    {
        public static Data GetTrainStatusData()
        {
            var res = new Data();
            res.Columns.Add("Day",0);
            res.Columns.Add("Outlook",1);
            res.Columns.Add("Humidity",2);
            res.Columns.Add("Wind",3);
            res.Columns.Add("IsLate",4);

            res.Rows.Add(new [] {"1", "Sunny", "High", "Weak", "true"});
            res.Rows.Add(new [] {"2", "Sunny", "High", "Strong", "true"});
            res.Rows.Add(new [] {"3", "Overcast", "High", "Weak", "false"});
            res.Rows.Add(new [] {"4", "Rain", "High", "Weak", "false"});
            res.Rows.Add(new [] {"5", "Rain", "Normal", "Weak", "false"});
            res.Rows.Add(new [] {"6", "Rain", "Normal", "Strong", "true"});
            res.Rows.Add(new [] {"7", "Overcast", "Normal", "Strong", "false"});
            res.Rows.Add(new [] {"8", "Sunny", "High", "Weak", "true"});
            res.Rows.Add(new [] {"9", "Sunny", "Normal", "Weak", "false"});
            res.Rows.Add(new [] {"10", "Rain", "Normal", "Weak", "false"});
            res.Rows.Add(new [] {"11", "Sunny", "Normal", "Strong", "false"});
            res.Rows.Add(new [] {"12", "Overcast", "High", "String", "false"});
            res.Rows.Add(new [] {"13", "Overcast", "Normal", "Weak", "false"});
            res.Rows.Add(new [] {"14", "Rain", "High", "Strong", "true"});
            return res;
        } 

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class trendingList
    {
        private static List<string> trendings = new List<string>();

        public static List<string> Trendings { get => trendings; set => trendings = value; }

        public void add(string value)
        {
            trendings.Add(value);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    static public class trendingList
    {
        private static List<string> trendings;

        public static List<string> Trendings { get => trendings; set => trendings = value; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class urlQuarantinedList
    {
        private static List<string> urlQuarantines = new List<string>();

        public static List<string> UrlQuarantines { get => urlQuarantines; set => urlQuarantines = value; }

        public void add(string value)
        {
            urlQuarantines.Add(value);
        }

        /*public List<string> returnList()
        {
            return 
        }*/
    }
}
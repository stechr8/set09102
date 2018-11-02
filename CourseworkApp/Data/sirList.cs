using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class sirList
    {
        private static List<string[]> sIRlist = new List<string[]>();

        public static List<string[]> SIRlist { get => sIRlist; set => sIRlist = value; }

        public void add(string[] value)
        {
            sIRlist.Add(value);
        }
    }
}
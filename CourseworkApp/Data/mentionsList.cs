using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class mentionsList
    {
        private static List<string> mentions = new List<string>();

        public static List<string> Mentions { get => mentions; set => mentions = value; }

        public void add(string value)
        {
            mentions.Add(value);
        }
    }
}
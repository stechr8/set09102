using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    static public class mentionsList
    {
        private static List<string> mentions;

        public static List<string> Mentions { get => mentions; set => mentions = value; }
    }
}
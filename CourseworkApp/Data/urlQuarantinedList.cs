using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public static class urlQuarantinedList
    {
        private static List<string> urlQuarantines;

        public static List<string> UrlQuarantines { get => urlQuarantines; set => urlQuarantines = value; }
    }
}
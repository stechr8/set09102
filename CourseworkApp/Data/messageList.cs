using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Data
{
    public class messageList
    {
        private static List<message> messages = new List<message>();

        public static List<message> Messages { get => messages; set => messages = value; }

        public void add(message value)
        {
            messages.Add(value);
        }

        public message returnValue(int value)
        {
            return messages.ElementAt(value);
        }

        public int count()
        {
            return messages.Count;
        }
    }
}
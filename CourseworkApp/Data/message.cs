using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public abstract class message
    {
        private string sender;
        private string body;

        public string Sender { get => sender; set => sender = value; }
        public string Body { get => body; set => body = value; }
    }
}
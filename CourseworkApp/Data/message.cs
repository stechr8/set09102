using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public abstract class message
    {
        private string header;
        private string body;

        public string Header { get => header; set => header = value; }
        public string Body { get => body; set => body = value; }
    }
}
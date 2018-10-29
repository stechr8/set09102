using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class email : message
    {
        private string subject;

        public string Subject { get => subject; set => subject = value; }
    }
}
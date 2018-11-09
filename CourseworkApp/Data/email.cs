using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class email : message
    {
        private string subject;
        private bool isSIR = false;

        public string Subject { get => subject; set => subject = value; }
        public bool IsSIR { get => isSIR; set => isSIR = value; }
    }
}
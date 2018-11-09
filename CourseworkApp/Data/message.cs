using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class message
    {
        private int id;
        private string sender;
        private string body;
       // private string subject;
        //private bool isSIR = false;

        public int Id { get => id; set => id = value; }
        public string Sender { get => sender; set => sender = value; }
        public string Body { get => body; set => body = value; }
        //public string Subject { get => subject; set => subject = value; }
        //public bool IsSIR { get => isSIR; set => isSIR = value; }
    }
}
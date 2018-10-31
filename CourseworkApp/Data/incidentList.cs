using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class incidentList
    {
        private List<string> incidents = new List<string>();

        public List<string> Incidents { get => incidents; set => incidents = value; }

        public List<string> getList()
        {
            return incidents;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain.Pdf
{
    public class PermitGrid
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }
        public int DurationMonth { get; set; }
        public string Department { get; set; }
        public string DepartmentDescription { get; set; }
        public string AppendixName { get; set; }
        public string AppendixUrl { get; set; }
    }
}

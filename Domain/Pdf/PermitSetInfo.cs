using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain.Pdf
{
    public class PermitSetInfo
    {
        public string Name { get; set; }
        public string PermitCount { get; set; }
        public List<string> Urls { get; set; }
    }
}

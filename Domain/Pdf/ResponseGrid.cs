using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Domain.Pdf
{
    public class ResponseGrid
    {
        #region Properties

        public string Question { get; set; }
        public string Response { get; set; }        
        public List<PermitSetInfo> PermitSetInfo { get; set; }
        public int PageNo { get; set; }
        public string QuestionSet { get; set; }
        public string Note { get; set; }
        public string PageBreak { get; set; }

        #endregion
    }
}

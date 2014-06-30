using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HawaiiDBEDT.Data.ViewModels
{
    public class PermitSetViewModel
    {
        public int PermitSetId { get; set; }
        public string PermitSetName { get; set; }
        public int PermitCount { get; set; }

        private string permitUrl;
        public string PermitUrl
        {
            get
            {
                if (PermitCount == 1)
                {
                    return permitUrl;
                }
                else
                {
                    return string.Format("/evaluate/permit_set.aspx?id={0}", PermitSetId);
                }
            }
            set { permitUrl = value; }
        }
    }
}

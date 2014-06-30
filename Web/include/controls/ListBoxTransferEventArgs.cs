using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HawaiiDBEDT.Web.include.controls
{
    public class ListBoxTransferEventArgs : EventArgs
    {
        public string TransferControlId { get; set; }

        public IList<string> TransferedItemsIds { get; set; }

        public ListBoxTransferDirection TransferDirection { get; set; }
    }
}
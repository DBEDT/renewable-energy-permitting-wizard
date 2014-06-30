namespace HawaiiDBEDT.Web.utils
{
    using System;
    using System.Web.UI.WebControls;

    [Serializable]
    public class Sorting 
    {
        public string Column { get; set; }
        public SortDirection Direction { get; set; }
    }
}
namespace HawaiiDBEDT.Web.admin.controls
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class TransferableListBox : UserControl
    {
        public object SourceListItems { get; set; }

        public object DestinationListItems { get; set; }

        public string DataValueField { get; set; }

        public string DataTextField { get; set; }

        public string SourceListLabel { get; set; }

        public string DestinationListLabel { get; set; }

        public string ListWidth { get; set; }

        public string ListHeight { get; set; }

        public string SourceListToolTip { get; set; }

        public string DestinationListToolTip { get; set; }

        public bool? AllowReorder { get; set; }

        public event EventHandler ElementsMoved;

        public ListItemCollection DestinationListValues
        {
            get
            {
                return this.cblDestinationList.Items;
            }
        }

        public void SourceListAddItem(string value, string text)
        {
            if (!string.IsNullOrEmpty(value))
            {
                this.cblSourceList.Items.Add(new ListItem(text, value));
            }
        }

        public void DestinationListAddItem(string value, string text)
        {
            if (!string.IsNullOrEmpty(value))
            {
                this.cblDestinationList.Items.Add(new ListItem(text, value));
            }
        }

        public void SourceListRemoveItem(string value)
        {
            if (this.cblSourceList.Items.FindByValue(value) != null)
            {
                this.cblSourceList.Items.Remove(this.cblSourceList.Items.FindByValue(value));
            }
        }

        public void DestinationListRemoveItem(string value)
        {
            if (this.cblDestinationList.Items.FindByValue(value) != null)
            {
                this.cblDestinationList.Items.Remove(this.cblDestinationList.Items.FindByValue(value));
            }
        }

        public void Rebind()
        {
            this.cblSourceList.DataSource = SourceListItems;
            this.cblSourceList.DataBind();

            this.cblDestinationList.DataSource = DestinationListItems;
            this.cblDestinationList.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.lblSourceList.Text = SourceListLabel;
                this.lblDestinationList.Text = DestinationListLabel;
                this.panReorderButtons.Visible = AllowReorder.HasValue && AllowReorder.Value;

                this.cblSourceList.Style.Add("width", ListWidth);
                this.cblSourceList.Style.Add("height", ListHeight);
                this.divLeft.Style.Add("width", ListWidth);
                this.divLeft.Style.Add("height", ListHeight);

                this.cblDestinationList.Style.Add("width", ListWidth);
                this.cblDestinationList.Style.Add("height", ListHeight);
                this.divRight.Style.Add("width", ListWidth);
                this.divRight.Style.Add("height", ListHeight);

                this.cblSourceList.ToolTip = SourceListToolTip;
                this.cblSourceList.DataSource = SourceListItems;
                this.cblSourceList.DataTextField = DataTextField;
                this.cblSourceList.DataValueField = DataValueField;
                this.cblSourceList.DataBind();

                this.cblDestinationList.ToolTip = DestinationListToolTip;
                this.cblDestinationList.DataSource = DestinationListItems;
                this.cblDestinationList.DataTextField = DataTextField;
                this.cblDestinationList.DataValueField = DataValueField;
                this.cblDestinationList.DataBind();
            }
        }

        protected void btnMoveSelected_Click(object sender, EventArgs e)
        {
            if (this.cblSourceList.SelectedItem != null)
            {
                var itemsMoved = new List<ListItem>();
                foreach (ListItem listItem in this.cblSourceList.Items)
                {
                    if (listItem.Selected)
                    {                        
                        itemsMoved.Add(listItem);                        
                    }
                }

                foreach (var listItem in itemsMoved)
                {
                    this.cblDestinationList.Items.Add(listItem);
                    this.cblSourceList.Items.Remove(listItem);
                }
                
                this.btnMoveSelected.Enabled = false;
                this.btnMoveBackSelected.Enabled = true;

                if (ElementsMoved != null)
                {
                    ElementsMoved(sender, e);
                }
            }            
        }

        protected void btnMoveBackSelected_Click(object sender, EventArgs e)
        {
            if (this.cblDestinationList.SelectedItem != null)
            {
                var itemsMoved = new List<ListItem>();
                foreach (ListItem listItem in this.cblDestinationList.Items)
                {
                    if (listItem.Selected)
                    {
                        itemsMoved.Add(listItem);    
                    }
                }

                foreach (var listItem in itemsMoved)
                {
                    this.cblSourceList.Items.Add(listItem);
                    this.cblDestinationList.Items.Remove(listItem);
                }

                this.btnMoveBackSelected.Enabled = false;
                this.btnMoveSelected.Enabled = true;

                if (ElementsMoved != null)
                {
                    ElementsMoved(sender, e);
                }
            }
        }

        protected void btnMoveUpSelected_Click(object sender, EventArgs e)
        {
            if (this.cblDestinationList.SelectedItem != null)
            {
                var selectedIndex = this.cblDestinationList.SelectedIndex;
                if (selectedIndex > 0)
                {
                    SwapItems(this.cblDestinationList.Items, selectedIndex, selectedIndex - 1);
                    this.btnMoveDownSelected.Enabled = true;
                }
            }
        }

        protected void btnMoveDownSelected_Click(object sender, EventArgs e)
        {
            if (this.cblDestinationList.SelectedItem != null)
            {
                var selectedIndex = this.cblDestinationList.SelectedIndex;
                if (selectedIndex < this.cblDestinationList.Items.Count - 1)
                {
                    SwapItems(this.cblDestinationList.Items, selectedIndex, selectedIndex + 1);
                    this.btnMoveUpSelected.Enabled = true;
                }
            }
        }

        private static void SwapItems(ListItemCollection items, int indexA, int indexB)
        {
            var tmp = items[indexA];
            items.RemoveAt(indexA);
            items.Insert(indexB, tmp);
        }

        protected void HandleSourceChange(object sender, EventArgs e)
        {
            this.btnMoveSelected.Enabled = this.cblSourceList.SelectedItem != null;
        }

        protected void HandleDestinationChange(object sender, EventArgs e)
        {
            if (this.cblDestinationList.SelectedItem != null)
            {
                this.btnMoveBackSelected.Enabled = true;
                if (AllowReorder.HasValue && AllowReorder.Value)
                {
                    var selectedIndex = this.cblDestinationList.SelectedIndex;
                    this.btnMoveUpSelected.Enabled = selectedIndex > 0;
                    this.btnMoveDownSelected.Enabled = selectedIndex < this.cblDestinationList.Items.Count - 1;
                }
            }
            else
            {
                this.btnMoveBackSelected.Enabled = false;
                this.btnMoveUpSelected.Enabled = false;
                this.btnMoveDownSelected.Enabled = false;
            }
        }
    }
}
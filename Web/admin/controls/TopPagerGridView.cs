namespace HawaiiDBEDT.Web.admin.controls
{
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class TopPagerGridView : GridView
    {
        protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        {
            if (this.Controls[0].Controls.Count == 0 &&
                (this.PagerSettings.Position == PagerPosition.Top ||
                 this.PagerSettings.Position == PagerPosition.TopAndBottom))
            {
                InitializeTopPager(row, columnSpan, pagedDataSource);
            }
            else
            {
                base.InitializePager(row, columnSpan, pagedDataSource);
            }
        }

        private void InitializeTopPager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        {
            var cell = new TableCell();
            if (columnSpan > 1)
            {
                cell.ColumnSpan = columnSpan;
            }
            this.PagerTemplate.InstantiateIn(cell);
            row.Cells.Add(cell);
        }
    }
}
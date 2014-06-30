using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace CustomControls
{

    /// <summary>
    /// Summary description for FullGridPager
    /// FullGridPager is a very nice and complete custom pager to use inside a GridView's PagerTemplate.
    /// Images files:
    ///     Images/firstpage.gif
    ///     Images/prevpage.gif
    ///     Images/nextpage.gif
    ///     Images/lastpage.gif
    /// CSS:
    ///     .pagerOuterTable    : the style of the html table.
    ///     .pageCounter        : the style of the first table cell "Page 1 of N".
    ///     .pageFirstLast      : the style of the First and Last cells.
    ///     .pagerLink          : the style of the page number links.
    ///     .pagePrevNextNumber : the style of the Previous, Next and page Number cells.
    ///     .pageCurrentNumber  : the style of the currently selected page number link.
    ///     .pageGroups         : the style of the page groups cell.
    /// Parameters:
    ///     MaxVisiblePageNumbers   : the number of VISIBLE page links.
    ///     PageCounterText         : the text to display at the beginning of the page counter.
    ///     PageCounterTotalText    : the text to display before the count of pages inside the page counter.
    ///     TheGrid                 : the gridview control.
    ///     
    /// Written by dpant@yahoo.com, 06-10-07
    /// </summary>
    public class FullGridPager
    {
        private const int DEFAULT_MAX_VISIBLE = 0;
        private const string DEFAULT_COUNTER_TEXT = "Σελίδα";
        private const string DEFAULT_TOTAL_TEXT = "από";

        protected int maxVisiblePageNumbers;
        protected int firstPageNumber;
        protected int lastPageNumber;
        protected string pageCounterText;
        protected string pageCounterTotalText;
        protected GridView theGrid;
        protected GridViewRow theGridViewRow;

        public FullGridPager(GridView TheGrid)
        {
            // Default Constructor		
            maxVisiblePageNumbers = DEFAULT_MAX_VISIBLE;
            pageCounterText = DEFAULT_COUNTER_TEXT;
            pageCounterTotalText = DEFAULT_TOTAL_TEXT;
            theGrid = TheGrid;
        }

        public FullGridPager(GridView theGrid, int MaxVisible, string CounterText, string TotalText)
        {
            // Parameterized constructor
            maxVisiblePageNumbers = MaxVisible;
            pageCounterText = CounterText;
            pageCounterTotalText = TotalText;
            this.theGrid = theGrid;
        }

        public int MaxVisiblePageNumbers
        {
            get { return maxVisiblePageNumbers; }
            set { maxVisiblePageNumbers = value; }
        }

        public string PageCounterText
        {
            get { return pageCounterText; }
            set { pageCounterText = value; }
        }

        public string PageCounterTotalText
        {
            get { return pageCounterTotalText; }
            set { pageCounterTotalText = value; }
        }

        public GridView TheGrid
        {
            get { return this.theGrid; }
        }

        public void CreateCustomPager(GridViewRow pagerRow)
        {
            // Create custom pager inside the Grid's pagerTemplate.
            // An html table is used to format the custom pager.

            // No data to display.
            if (pagerRow == null) return;

            theGridViewRow = pagerRow;

            HtmlTable pagerInnerTable = (HtmlTable)pagerRow.Cells[0].FindControl("pagerInnerTable");
            if (pagerInnerTable != null)
            {
                // default dynamic cell position. 
                // (after the pageCount, the "First" and the "Previous" cells).            
                int insertCellPosition = 1;
                if (theGrid.PageIndex == 0)
                {
                    // The first page is currently displayed.
                    // Hide First and Previous page navigation.
                    pagerInnerTable.Rows[0].Cells[0].Visible = false;
                    pagerInnerTable.Rows[0].Cells[1].Visible = false;
                    // pagerInnerTable.Rows[0].Cells[2].Visible = false;
                    // Change the default dynamic cell to 1.
                    insertCellPosition = 0;
                }

                CalcFirstPageNumber();
                CalcLastPageNumber();

                CreatePageNumbers(pagerInnerTable, insertCellPosition);

                int lastCellPosition = pagerInnerTable.Rows[0].Cells.Count - 1;
                if (theGrid.PageIndex == theGrid.PageCount - 1)
                {
                    // The last page is currently displayed.
                    // Hide Next and Last page navigation.                
                    pagerInnerTable.Rows[0].Cells[lastCellPosition - 1].Visible = false;
                    pagerInnerTable.Rows[0].Cells[lastCellPosition].Visible = false;
                }
            }
        }

        private void CreatePageNumbers(HtmlTable pagerInnerTable, int insertCellPosition)
        {
            for (int i = firstPageNumber, pos = 1; i <= lastPageNumber; i++, pos++)
            {
                // Create a new table cell for every data page number.
                HtmlTableCell tableCell = new HtmlTableCell();
                if (theGrid.PageIndex == i - 1)
                    tableCell.Attributes.Add("class", "pageCurrentNumber");
                else
                    tableCell.Attributes.Add("class", "pagePrevNextNumber");

                // Create a new LinkButton for every data page number.
                LinkButton lnkPage = new LinkButton();
                lnkPage.ID = string.Format("Page{0}", i);
                lnkPage.Text = i.ToString();
                lnkPage.CommandName = "Page";
                lnkPage.CommandArgument = i.ToString();
                lnkPage.CssClass = "pagerLink";

                // Place link inside the table cell; Add the cell to the table row.                
                tableCell.Controls.Add(lnkPage);
                pagerInnerTable.Rows[0].Cells.Insert(insertCellPosition + pos, tableCell);
            }
        }

        private void CalcFirstPageNumber()
        {
            // Calculate the first, visible page number of the pager.             
            firstPageNumber = 1;
            if (MaxVisiblePageNumbers == DEFAULT_MAX_VISIBLE)
                MaxVisiblePageNumbers = theGrid.PageCount;

            if (theGrid.PageCount > MaxVisiblePageNumbers)
            {
                // Seperate page numbers in groups if necessary.
                if ((theGrid.PageIndex + 1) > maxVisiblePageNumbers)
                {
                    // Calculate the group to display.
                    // Example: 
                    //      GridView1.PageCount = 12
                    //      maxVisiblePageNumbers = 4
                    //      GridView1.PageIndex+1 = 7
                    //      --> pageGroup = 2       (Page numbers: 5, 6, 7, 8)
                    decimal pageGroup = Math.Ceiling((decimal)(theGrid.PageIndex + 1) / MaxVisiblePageNumbers);
                    // Calculate the first page number for the group to display.
                    // Example :
                    //      if pageGroup = 1 (Page numbers: 1,2,3,4) --> firstPageNumber = 1
                    //      if pageGroup = 2 (Page numbers: 5,6,7,8) --> firstPageNumber = 5
                    //      if pageGroup = 3 (Page numbers: 9,10,11,12) --> firstPageNumber = 9                        
                    firstPageNumber = (int)(1 + (MaxVisiblePageNumbers * (pageGroup - 1)));
                }
            }
        }

        private void CalcLastPageNumber()
        {
            // Calculate the last, visible page number of the pager.
            lastPageNumber = theGrid.PageCount;
            if (MaxVisiblePageNumbers == DEFAULT_MAX_VISIBLE)
                MaxVisiblePageNumbers = theGrid.PageCount;

            if (theGrid.PageCount > MaxVisiblePageNumbers)
            {
                lastPageNumber = firstPageNumber + (MaxVisiblePageNumbers - 1);
                if (theGrid.PageCount < lastPageNumber)
                    lastPageNumber = theGrid.PageCount;
            }
        }
    }
}

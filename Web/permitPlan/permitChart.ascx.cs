namespace HawaiiDBEDT.Web.permitPlan
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web.UI.DataVisualization.Charting;
    using Controls;
    using Domain;

    public partial class permitChart : BaseControl 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public byte[] GetImageData()
        {
            this.DisplayGanttChart();
            var stream = new MemoryStream();
            this.Chart1.SaveImage(stream);
            return stream.ToArray();
        }

        private void DisplayGanttChart()
        {
            if (CurrentEvaluation.DistinctPermits.Count > 0)
            {
                Chart1.ChartAreas[0].AxisY.Maximum = CurrentEvaluation.DistinctPermits.Max(i => i.EndDuration);
                Chart1.Height = (CurrentEvaluation.DistinctPermits.Count * 70);
                var x = CurrentEvaluation.DistinctPermits.OrderByDescending(i => i.Name).OrderByDescending(i => i.StartDuration).ToList();
                int j = 0;

                var federalList = new List<ChartData>();
                var stateList = new List<ChartData>();
                var countyList = new List<ChartData>();
                foreach (var permit in x)
                {
                    j++;
                    federalList.Add(new ChartData { Index = j - 0.75, Start = permit.StartDuration1, End = permit.EndDuration1, Label = (permit.StartDuration1 == -1 && permit.EndDuration1 == -1) ? string.Empty : CreateLabel(permit.Name, permit.Duration) });
                    stateList.Add(new ChartData { Index = j, Start = permit.StartDuration2, End = permit.EndDuration2, Label = (permit.StartDuration2 == -1 && permit.EndDuration2 == -1) ? string.Empty : CreateLabel(permit.Name, permit.Duration) });
                    countyList.Add(new ChartData { Index = j + 0.75, Start = permit.StartDuration3, End = permit.EndDuration3, Label = (permit.StartDuration3 == -1 && permit.EndDuration3 == -1) ? string.Empty : CreateLabel(permit.Name, permit.Duration) });
                }

                this.BindDataSeries(Chart1.Series["Federal"], federalList);
                Chart1.Series["Federal"]["DrawSideBySide"] = "True";
                this.BindDataSeries(Chart1.Series["State"], stateList);
                this.BindDataSeries(Chart1.Series["County"], countyList);
            }
        }

        private string CreateLabel(string name, short duration)
        {
            string monthVerbiage = duration == 1 ? "month" : "months";
            return string.Format("{0} ({1} {2})", name, duration, monthVerbiage);
        }

        private void BindDataSeries(Series chartSeries, List<ChartData> chartDataItems)
        {
            int indexer = 0;
            int maxGoodLookingLabelIndex = (int)Math.Round(chartDataItems.Max(item => item.End) * 0.8);
            foreach (var chartDataItem in chartDataItems)
            {
                chartSeries.Points.AddXY(chartDataItem.Index, chartDataItem.Start, chartDataItem.End);
                if (chartDataItem.Start < maxGoodLookingLabelIndex)
                {
                    chartSeries.Points[indexer].Label = chartDataItem.Label;
                }
                else
                {
                    var annotation = new TextAnnotation();
                    annotation.Font = new Font("Arial", 8, FontStyle.Bold);
                    annotation.SmartLabelStyle.Enabled = true;
                    annotation.AnchorDataPoint = chartSeries.Points[indexer];
                    annotation.Text = chartDataItem.Label;
                    Chart1.Annotations.Add(annotation);
                }

                indexer++;
            }

            chartSeries["PointWidth"] = "2.0";
        }
    }
}
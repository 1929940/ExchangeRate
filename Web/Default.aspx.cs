using ExchangeRateLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class Default : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void SelectedChanged(object sender, EventArgs e)
        {
            string valFrom = DropDown_From.SelectedItem.Text;
            string valTo = DropDown_To.SelectedItem.Text;

            if (valFrom == null || valTo == null || (valFrom == valTo)) return;

            var tmp = JsonWorker.GetExchangeData(valFrom, valTo);


            lbl_From.Text = tmp.exchangeData.From_Code;
            lbl_To.Text = tmp.exchangeData.To_Code;
            lbl_ExchangeRate.Text = tmp.exchangeData.ExchangeRate.ToString();

            MyPoints.MyPointsList = JsonWorker.GetHistoricPoints(DropDown_From.SelectedItem.Text, DropDown_To.SelectedItem.Text);

            RemakeChart(MyPoints.MyPointsList,30);
        }

        protected void btn_Swap_Click(object sender, ImageClickEventArgs e)
        {

            int SelectedIndex = DropDown_From.SelectedIndex;
            DropDown_From.SelectedIndex = DropDown_To.SelectedIndex;
            DropDown_To.SelectedIndex = SelectedIndex;

            SelectedChanged(this, EventArgs.Empty);
        }

        protected void Chart_Load(object sender, EventArgs e)
        {

        }

        protected void Cbx_ShowTrend_CheckedChanged(object sender, EventArgs e)
        {
            if (Cbx_ShowTrend.Checked)
            {
                RemakeChart(MyPoints.MyPointsList, 30);
            }
            else
            {
                RemakeChart(MyPoints.MyPointsList, 30, false);
            }
        }

        protected void RemakeChart(List<MyPoint> input, int days = 0, bool IsChecked = true)
        {
            if (input == null) return;

            Chart.Series.Clear();

            var series1 = new System.Web.UI.DataVisualization.Charting.Series();
            var series2 = new System.Web.UI.DataVisualization.Charting.Series();


            series2.ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            series1.ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Area;

            days = (days == 0) ? input.Count : 2*days;

            double min = input[0].Value;
            double max = input[0].Value;

            for (int i = 0; i < days; i++)
            {
                series1.Points.AddXY(input[i].Name, input[i].Value);
                series2.Points.AddXY(input[i].Name, input[i].Value);

                if (i > 0)
                {
                    if (input[i].Value > input[i - 1].Value)
                    {
                        series2.Points[i].Color = System.Drawing.Color.Green;
                    }
                    else if (input[i].Value < input[i - 1].Value)
                    {
                        series2.Points[i].Color = System.Drawing.Color.Red;
                    }
                }

                if (min > input[i].Value) min = input[i].Value;
                if (max < input[i].Value) max = input[i].Value;
            }

            Chart.ChartAreas["ChartArea1"].AxisY.Minimum = min;
            Chart.ChartAreas["ChartArea1"].AxisY.Maximum = max;


            Chart.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;

            this.Chart.Series.Add(series1);
            this.Chart.Series.Add(series2); 

            this.Chart.Series["Series2"].Enabled = IsChecked;

        }
    }
}
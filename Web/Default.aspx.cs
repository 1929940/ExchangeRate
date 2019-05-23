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
        int daysChecked;

        protected void Page_Load(object sender, EventArgs e)
        {
            RadioButtons_SetEnable();
            daysChecked = GetRadioButtonCheckedValue();

            //Response.Write("hello world");
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

            RemakeChart(MyPoints.MyPointsList,daysChecked);
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
                RemakeChart(MyPoints.MyPointsList, daysChecked);
            }
            else
            {
                RemakeChart(MyPoints.MyPointsList, daysChecked, false);
            }
        }

        protected void RemakeChart(List<MyPoint> input, int days = 0, bool IsChecked = true)
        {
            if (input == null) return;

            RadioButtons_SetEnable();

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
        #region RadioBoxes

        protected int GetRadioButtonCheckedValue()
        {
            int output;

            if (rdb_30.Checked) output = 30;
            else if (rdb_60.Checked) output = 60;
            else if (rdb_90.Checked) output = 90;
            else if (rdb_180.Checked) output = 180;
            else output = 0;

            return output;
        }
        protected void RadioButtons_SetEnable()
        {
            rdb_30.Enabled = false;
            rdb_60.Enabled = false;
            rdb_90.Enabled = false;
            rdb_180.Enabled = false;

            if (MyPoints.MyPointsList == null) return;

            // 1 day consists 2 points [open & close]
            if (MyPoints.MyPointsList.Count >= 60) rdb_30.Enabled = true;
            if (MyPoints.MyPointsList.Count >= 120) rdb_60.Enabled = true;
            if (MyPoints.MyPointsList.Count >= 180) rdb_90.Enabled = true;
            if (MyPoints.MyPointsList.Count >= 360) rdb_180.Enabled = true;
        }
        protected void Rdb_changed(object sender, EventArgs e)
        {
            RemakeChart(MyPoints.MyPointsList, daysChecked, Cbx_ShowTrend.Checked);
        }

        #endregion

    }
}
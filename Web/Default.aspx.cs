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
            daysChecked = RadioButtons_GetCheckedValue();

            Lbl_CharError.Text = "Ooops! There appears an error has occured generating the chart. <br /><br />" +
                "There are two possibilities, both API related. <br /><br />" +
                "First one: The API provides no historic data for the couple of currencies to draw a chart<br /><br />" +
                "Second one: The API is limited to 5 calls within 30 seconds, please try again in half a minute<br /><br />";

            if (!PageInitialized.Initialized)
            {
                DropDown_From.SelectedIndex = 116;
                DropDown_To.SelectedIndex = 46;

                ExchangeRate_MainWorker("PLN", "EUR");

                PageInitialized.Initialized = true;
            }
        }
        protected void SelectedChanged(object sender, EventArgs e)
        {
            string valFrom = DropDown_From.SelectedItem.Text;
            string valTo = DropDown_To.SelectedItem.Text;

            ExchangeRate_MainWorker(valFrom, valTo);

        }
        /// <summary>
        /// This guy organizes all the heavy lifting. 
        /// </summary>
        /// <param name="from">Currency Code From</param>
        /// <param name="to">Currency Code To</param>
        private void ExchangeRate_MainWorker(string from, string to)
        {

            if (from == null || to == null || (from == to)) return;

            var tmp = JsonWorker.GetExchangeData(from, to);

            lbl_From.Text = from;
            lbl_To.Text = to;

            if (tmp == null)
            {
                lbl_ExchangeRate.Text = "Error";
                return;
            }

            lbl_ExchangeRate.Text = tmp.exchangeData.ExchangeRate.ToString();

            MyPoints.MyPointsList = JsonWorker.GetHistoricPoints(from, to);

            DrawChart(MyPoints.MyPointsList, daysChecked);
        }

        protected void Btn_Swap_Click(object sender, ImageClickEventArgs e)
        {
            int SelectedIndex = DropDown_From.SelectedIndex;
            DropDown_From.SelectedIndex = DropDown_To.SelectedIndex;
            DropDown_To.SelectedIndex = SelectedIndex;

            ExchangeRate_MainWorker(DropDown_From.SelectedItem.Text, DropDown_To.SelectedItem.Text);
        }

        protected void Cbx_ShowTrend_CheckedChanged(object sender, EventArgs e)
        {
            if (Cbx_ShowTrend.Checked)
            {
                DrawChart(MyPoints.MyPointsList, daysChecked, true);
            }
            else
            {
                DrawChart(MyPoints.MyPointsList, daysChecked, false);
            }
        }

        protected void DrawChart(List<MyPoint> input, int days = 0, bool IsChecked = false)
        {
            // Exception Handling

            if (input == null)
            {
                Chart.Visible = false;
                Lbl_CharError.Visible = true;

                return;
            }
            Chart.Visible = true;
            Lbl_CharError.Visible = false;

            RadioButtons_SetEnable();

            // Drawing the Chart
            // One series for data
            // Another for trend lines

            Chart.Series.Clear();

            var series1 = new System.Web.UI.DataVisualization.Charting.Series();
            var series2 = new System.Web.UI.DataVisualization.Charting.Series();

            series2.ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            series1.ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Area;

            days = (days == 0) ? input.Count : 2*days;

            double AxisY_MinValue = input[0].Value;
            double AxisY_MaxValue = input[0].Value;

            for (int i = 0; i < days; i++)
            {
                series1.Points.AddXY(input[i].Name, input[i].Value);
                series2.Points.AddXY(input[i].Name, input[i].Value);

                // Determines the colour of the trend line
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

                if (AxisY_MinValue > input[i].Value) AxisY_MinValue = input[i].Value;
                if (AxisY_MaxValue < input[i].Value) AxisY_MaxValue = input[i].Value;
            }

            Chart.ChartAreas["ChartArea1"].AxisY.Minimum = AxisY_MinValue;
            Chart.ChartAreas["ChartArea1"].AxisY.Maximum = AxisY_MaxValue;


            Chart.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;

            this.Chart.Series.Add(series1);
            this.Chart.Series.Add(series2); 

            // Decides if trend lines should be visible or not

            this.Chart.Series["Series2"].Enabled = IsChecked;

        }

        #region RadioBoxes

        protected int RadioButtons_GetCheckedValue()
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
        protected void Rdb_Changed(object sender, EventArgs e)
        {
            DrawChart(MyPoints.MyPointsList, daysChecked, Cbx_ShowTrend.Checked);
        }

        #endregion

    }
}
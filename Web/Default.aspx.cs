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

            if (!IsPostBack)
            {
                DropDown_From.SelectedIndex = 116;
                DropDown_To.SelectedIndex = 150;

                ExchangeRate_MainWorker("PLN", "USD");
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

                Chart.Visible = false;
                Lbl_CharError.Visible = true;

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
            DrawChart(MyPoints.MyPointsList, daysChecked);
        }

        protected void DrawChart(List<MyPoint> input, int days = 0)
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

            days = (days == 0) ? input.Count : days;

            double AxisY_MinValue = input[0].Value;
            double AxisY_MaxValue = input[0].Value;

            double LastPoint = 0;

            for (int i = 0; i < days; i++)
            {
                series1.Points.AddXY(input[i].Date, input[i].Value);
                if (i % 5 == 0)
                {
                    LastPoint = input[i].Value;
                    series2.Points.AddXY(input[i].Date, input[i].Value);
                    if (i > 0)
                    {
                        if (input[i].Value < input[i - 5].Value)
                        {
                            series2.Points[i / 5].Color = System.Drawing.Color.Green;
                        }
                        else if (input[i].Value > input[i - 5].Value)
                        {
                            series2.Points[i / 5].Color = System.Drawing.Color.Red;
                        }
                    }
                }
                if (i == days - 1)
                {
                    series2.Points.AddXY(input[i].Date, input[i].Value);
                    if (input[i].Value < LastPoint)
                    {
                        series2.Points[series2.Points.Count-1].Color = System.Drawing.Color.Green; // enter last point
                    }
                    else if (input[i].Value > LastPoint)
                    {
                        series2.Points[series2.Points.Count - 1].Color = System.Drawing.Color.Red;
                    }
                }


                if (AxisY_MinValue > input[i].Value) AxisY_MinValue = input[i].Value;
                if (AxisY_MaxValue < input[i].Value) AxisY_MaxValue = input[i].Value;
            }

            System.Diagnostics.Debug.WriteLine("AxisY Values");
            System.Diagnostics.Debug.WriteLine("Max: {0}", AxisY_MaxValue);
            System.Diagnostics.Debug.WriteLine("Min: {0}", AxisY_MinValue);

            if (AxisY_MaxValue != AxisY_MinValue)
            {
                Chart.ChartAreas["ChartArea1"].AxisY.Minimum = AxisY_MinValue;
                Chart.ChartAreas["ChartArea1"].AxisY.Maximum = AxisY_MaxValue;
            }
            else if((AxisY_MaxValue == 0) && (AxisY_MinValue == 0))
            {
                Chart.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
                Chart.ChartAreas["ChartArea1"].AxisY.Maximum = 1;
            }
            else
            {
                Chart.ChartAreas["ChartArea1"].AxisY.Minimum = AxisY_MinValue/2;
                Chart.ChartAreas["ChartArea1"].AxisY.Maximum = 2*AxisY_MaxValue;
            }

            Chart.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;

            this.Chart.Series.Add(series1);
            this.Chart.Series.Add(series2);

            // Decides if trend lines should be visible or not

            Chart.Series["Series2"].Enabled = Cbx_ShowTrend.Checked;

            Chart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd.MM.yy";

            //How many decimal places?
            Chart.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "0.####";
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

            if (MyPoints.MyPointsList.Count >= 30) rdb_30.Enabled = true;
            if (MyPoints.MyPointsList.Count >= 60) rdb_60.Enabled = true;
            if (MyPoints.MyPointsList.Count >= 90) rdb_90.Enabled = true;
            if (MyPoints.MyPointsList.Count >= 180) rdb_180.Enabled = true;
        }
        protected void Rdb_Changed(object sender, EventArgs e)
        {
            DrawChart(MyPoints.MyPointsList, daysChecked);
        }

        #endregion

    }
}
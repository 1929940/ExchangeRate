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

            RemakeChart(JsonWorker.GetHistoricPoints(DropDown_From.SelectedItem.Text, DropDown_To.SelectedItem.Text), 30);
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

        }

        protected void RemakeChart(List<MyPoint> input, int days = 0)
        {
            if (input == null) return;

            Chart.Series.Clear();
            var series1 = new System.Web.UI.DataVisualization.Charting.Series();

            series1.ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Area;

            days = (days == 0) ? input.Count : days;

            for (int i = 0; i < 2*days; i++)
            {
                series1.Points.AddXY(input[i].Name, input[i].Value);
            }


            Chart.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;

            this.Chart.Series.Add(series1);
        }
    }
}
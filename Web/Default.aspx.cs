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

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            lbl_From.Text = (lbl_From.Text == "PLN") ? "EUR" : "PLN"; 
            lbl_To.Text = (lbl_To.Text == "EUR") ? "PLN" : "EUR";
        }

        protected void Chart_Load(object sender, EventArgs e)
        {

        }
    }
}
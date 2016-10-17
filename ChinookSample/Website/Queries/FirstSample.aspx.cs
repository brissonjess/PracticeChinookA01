using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Queries_FirstSample : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //are you logged in?
            if (!Request.IsAuthenticated)
            {
                //if not redirect user to the login page
                Response.Redirect("~/Account/Login.aspx");
            }
        }

    }
}
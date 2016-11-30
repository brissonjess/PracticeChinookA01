using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using ChinookSystem.BLL;
using ChinookSystem.Data.POCOs;
using Chinook.UI;
using ChinookSystem.Security;
using Microsoft.AspNet.Identity;
#endregion

public partial class BusinessProcess_ManagePlayList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //TrackSearchList.DataSource = null;
        }
    }

    protected void PlayListFetch_Click(object sender, EventArgs e)
    {
        int customerid = GetUserCustomerID();
        if (customerid > 0)
        {
            MessageUserControl.TryRun(() => {
                //if(string.IsNullOrEmpty(PlayListName.Text))
            });
        }

    }

    protected int GetUserCustomerID()
    {
        int customerid = 0;
        if (Request.IsAuthenticated)
        {
            string username = User.Identity.Name;
            UserManager sysmgr = new UserManager();

            var applicationuser = sysmgr.FindByName(username);
            customerid = applicationuser.CustomerId == null ? 0 : (int)applicationuser.CustomerId;
        }
        else
        {
            MessageUserControl.ShowInfo("You must log in to manage a playlist.");
        }
        return customerid;
    }
}
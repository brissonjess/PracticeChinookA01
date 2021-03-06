﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces -- Security 
using ChinookSystem.Security;
#endregion 

public partial class Admin_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void RefreshAll(object sender, EventArgs e)
    {
        //refresh the list
        DataBind();
    }

    protected void UnregisteredUsersGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //THIS DIFF BETWEEN "Changing" AND "Changed"
        //Method signatures that end in "ing" process the data during the selection and "ed" means the processing happens AFTER the action
        //you dont want to wait until after the selection and do the processing 


        //position the gridview to the selectindex(row) that caused the postback
        UnregisteredUsersGridView.SelectedIndex = e.NewSelectedIndex;

        //setup a variable that will be the physical pointer to the selected row 
        GridViewRow aGridViewRow = UnregisteredUsersGridView.SelectedRow;

        //you can already check a pointer to see if something has been obtained 
        if(aGridViewRow != null)
        {
            //access information contained in a textbox on the gridview row 
                //to do that you need to use the method .FindControl("controlIdName") as controltype
                //once you have a pointer to the control, you can access the data content of the control using the controls access method
                //ex. how do you access contents of a text box: controlName.Text
            
            //THE LONG, SAFEWAY, OF DOING IT
            string assignedUserName = "";
            TextBox inputControl = aGridViewRow.FindControl("AssignedUserName") as TextBox;
            if(inputControl != null)
            {
                assignedUserName = inputControl.Text;
                //Bind("AssignedUserName");
                //Bind("AssignedEmail");
            }

            //SHORT WAY - DONE IN ONE STATEMENT
            string assignedEmail = (aGridViewRow.FindControl("AssignedEmail") as TextBox).Text;

            //create the unregistered user instance 
            //during creation, I will pass to it the needed data to load the instance attributes

            //THIS IS A FAST WAY OF CREATING A NEW INSTANCE AND LOADING IT AT THE SAME TIME
            //accessing boundfields on a gridview row uses .Cells[index].Text
            //index represents the column of the grid
            //columns are indexed (therefore, they start at zero 0 )
            //NOTE: when using this kind of "new" statement you are passing in parameters, so you use commas (not semicolons) at the end of your statements
            UnRegisteredUserProfile user = new UnRegisteredUserProfile()
            {
                CustomerEmployeeId = int.Parse(UnregisteredUsersGridView.SelectedDataKey.Value.ToString()),
                UserType = (UnRegisteredUserType)Enum.Parse(typeof(UnRegisteredUserType), aGridViewRow.Cells[1].Text),
                FirstName = aGridViewRow.Cells[2].Text,
                LastName = aGridViewRow.Cells[3].Text,
                AssignedUserName = assignedUserName,
                AssignedEmail = assignedEmail
            };

            //register the user via the Chinook.UserManager controller
            UserManager sysmgr = new UserManager();
            sysmgr.RegisterUser(user);

            //assume the successful creation of a user 
            //refresh the form
            DataBind();

        }// \ if != null
    }// eom

    protected void UserListView_ItemInserting(object sender, ListViewInsertEventArgs e)
    {
        //you need to walk through the checkbox list 


        //create the RoleMembership string List<> of selected rows 
        var addtoroles = new List<string>();

        //point to the physical checkbox list control
        var roles = e.Item.FindControl("RoleMemberships") as CheckBoxList;

        //does the control exist - safety check 
        if(roles != null)
        {
            //1. cycle through the checkboxlist 
            foreach(ListItem role in roles.Items)
            {
                //2. find roles that were checked 
                if (role.Selected)
                {//is it checked?
                    
                    //3. add to the List<string>
                    addtoroles.Add(role.Value);
                }
                //4. assign the List<string> to the inserting instance represented by e
                e.Values["RoleMemberships"] = addtoroles;
            }

        }

    }
}
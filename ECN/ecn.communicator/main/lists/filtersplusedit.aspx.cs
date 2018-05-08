using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using System.Data;
using System.Web.SessionState;

namespace ecn.communicator.listsmanager
{
    public partial class filtersplusedit1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS;
            Master.SubMenu = "";
            Master.Heading = "Groups > Manage Groups > Smart Forms > Filters > Edit";
            Master.HelpContent = "<B>Adding/Editing Filters</B><div id='par1'><ul><li>Find the Group you want to create a filter for.</li><li>Click on the <em>Funnel</em> icon for that group.</li><li>Enter a title for your filter (for example, pet owners)</li><li>Click <em>Create new filter</em>.</li><li>Under filter names, click on the <em>pencil (Add/Edit Filter Attributes)</em> icon to define the filter attributes.</li><li>In the Compare Field section, use the drop down menu and click on profile field to define attributes of your filter.</li><li>In the Comparator section you have the option of making the field equal to (=), contains, ends with, or starts with.</li><li>In the Compare Value field, enter the information you would want the system to filter (for example, dog).</li><li>The Join Filters allow you to select And, or, Or.</li><li>To add, click <em>Add this Filter</em>.</li><li>Repeat this process several times to fully develop the attributes you are looking for (for example, dog, dogs, cat, cats, dog owners, etc.)</li><li>After all fields and attributes have been selected and added, click <em>Preview filtered e-mails</em> button to view emails in your filtered list.</li><li>When filter is complete, Click on <em>Return to Filters List</em>.</li></ul></div>";
            Master.HelpTitle = "Filters Manager";
        }

        private int getFilterID()
        {
            if (Request.QueryString["FilterID"] != null)
                return Convert.ToInt32(Request.QueryString["FilterID"].ToString());
            else
                return 0;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Session["Filter"] = null;
            Response.Redirect("filters.aspx?GroupID=" + selectedGroupID.ToString());
        }

        public int selectedGroupID
        {
            get
            {
                if (Request.QueryString["GroupID"] != null)
                    return Convert.ToInt32(Request.QueryString["GroupID"]);
                else if (ViewState["selectedGroupID"] != null)
                    return (int)ViewState["selectedGroupID"];
                else
                    return 0;
            }
            set
            {
                ViewState["selectedGroupID"] = value;
            }
        }
    }
       
}

using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace ecn.communicator.contentmanager.ckeditor.dialog
{	
    public partial class codeSnippets : System.Web.UI.Page
    {
        delegate void HidePopup();
		public string ftfValue = "";
        public string smValue = "";
		string _CodeSnippets = "";

        public string getCustomerID()
        {
            try
            {
                if (ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID > 0)
                    return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID.ToString();
                else
                    return Request.QueryString["cuID"].ToString();
            }
            catch
            {
                return Request.QueryString["cuID"].ToString();
            }
        }

		private void Page_Load(object sender, System.EventArgs e)
        {
            int  custID = Convert.ToInt32(getCustomerID());
            HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
            this.groupExplorer.hideGroupsLookupPopup = delGroupsLookupPopup;
		}


        public string getSMValue(string name)
        {
            try
            {
                smValue = "ECN.SUBSCRIPTIONMGMT." + name + ".ECN.SUBSCRIPTIONMGMT";
            }
            catch
            {
                smValue = "";
            }
            return smValue;
        }


		public string CodeSnippets
        {
			get 
            {
                return _CodeSnippets;
            }
		}

		private List<ECN_Framework_Entities.Communicator.GroupDataFields> _groupDataFields = null;


        protected void imgSelectGroup_Click(object sender, ImageClickEventArgs e)
        {
            hfGroupSelectionMode.Value = "SelectGroup";
            groupExplorer.LoadControl();
            groupExplorer.Visible = true;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("GroupSelected"))
                {
                    int groupID = groupExplorer.selectedGroupID;
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(groupID);
                    if (hfGroupSelectionMode.Value.Equals("SelectGroup"))
                    {
                        lblSelectGroupName.Text = group.GroupName;
                        hfSelectGroupID.Value = groupID.ToString();
                        PopulateFields(groupID);
                    }
                    else
                    {

                    }
                    GroupsLookupPopupHide();
                    
                }
            }
            catch { }
            return true;
        }

        private void GroupsLookupPopupHide()
        {
            groupExplorer.Visible = false;
        }

        private void PopulateFields(int GroupID)
        {
            List<DropDownHolder> listForFields = new List<DropDownHolder>();
            Dictionary<string, string> listSnippets = new Dictionary<string, string>();
            

            listForFields.Add(new DropDownHolder("%%EmailAddress%%", "Email Address", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Title%%", "Title", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%FirstName%%", "FirstName", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%LastName%%", "LastName", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%FullName%%", "Full Name", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Company%%", "Company", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Occupation%%", "Occupation", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Address%%", "Address", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Address2%%", "Address2", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%City%%", "City", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%State%%", "State", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Zip%%", "Zip", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Country%%", "Country", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Voice%%", "Phone #", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Mobile%%", "Cell Phone #", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Fax%%", "Fax #", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Website%%", "Website", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Age%%", "Age", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Income%%", "Income", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Gender%%", "Gender", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%User1%%", "User1 Field", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%User2%%", "User2 Field", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%User3%%", "User3 Field", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%User4%%", "User4 Field", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%User5%%", "User5 Field", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%User6%%", "User6 Field", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%Birthdate%%", "Birthdate [Date Field]", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%UserEvent1%%", "UserEvent1", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%UserEvent1Date%%", "UserEvent1Date [Date Field]", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%UserEvent2%%", "UserEvent2", "Email Profile"));
            listForFields.Add(new DropDownHolder("%%UserEvent2Date%%", "UserEvent2Date [Date Field]", "Email Profile"));

            if (_groupDataFields == null)
            {
                _groupDataFields = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(GroupID);
            }
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in _groupDataFields.OrderBy(x => x.DatafieldSetID).ThenBy(x => x.ShortName).ToList())
            {
                listForFields.Add(new DropDownHolder(string.Format("%%{0}%%", groupDataFields.ShortName), string.Format("{0}", groupDataFields.ShortName), !groupDataFields.DatafieldSetID.HasValue ? "Standalone UDF" : "Transactional UDF"));
            }

            List<ECN_Framework_Entities.Accounts.SubscriptionManagement> listSub = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagement.GetByBaseChannelID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID);

            foreach (ECN_Framework_Entities.Accounts.SubscriptionManagement sm in listSub.OrderBy(x => x.Name))
            {
                listForFields.Add(new DropDownHolder(getSMValue(sm.Name),sm.Name, "Subscription Management"));
            }
            ddlCodeSnippet.DataTextField = "Text";
            ddlCodeSnippet.DataValueField = "Value";

            
            for(int i = 0;i < listForFields.Count; i++)
            {
                ListItem li = new ListItem(listForFields[i].Text, listForFields[i].Value);
                li.Attributes["OptGroup"] = listForFields[i].Group;

                ddlCodeSnippet.Items.Insert(i, li);
            }

            ddlCodeSnippet.Items.Insert(0,new ListItem("--SELECT--","select"));
            
        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		

        public class DropDownHolder
        {
            public DropDownHolder(string value,string text, string group)
            {
                Text = text;
                Value = value;
                Group = group;
            }
            public string Text{get;set;}
            public string Value{get;set;}
            public string Group{get;set;}
        }

        protected void ddlCodeSnippet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlCodeSnippet.SelectedIndex > 0)
            {
                hfCodeSnippet.Value = ddlCodeSnippet.SelectedValue;
            }
            else
            {
                hfCodeSnippet.Value = "";
            }
        }
	}
}
using System;
using System.Collections;
using System.Web.UI.WebControls;
using ecn.communicator.classes;
using ecn.common.classes;

namespace ecn.communicator.contentmanager.feditor.dialog{
	/// <summary>
	/// Summary description for ecn_codeSnippets.
	/// </summary>
    public partial class codeSnippets : System.Web.UI.Page
    {

		public string ftfValue = "";
		public ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
		public ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
        ECN_Framework_BusinessLayer.Application.ECNSession es;
		string _CodeSnippets = "";

        public string getCustomerID()
        {
            try
            {
                if (Convert.ToInt32(sc.CustomerID()) > 0)
                    return sc.CustomerID().ToString();
                else
                    return Request.QueryString["cuID"].ToString();
            }
            catch
            {
                return Request.QueryString["cuID"].ToString();
            }
        }

		private void Page_Load(object sender, System.EventArgs e){
            es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            string custID = getCustomerID();
			if(!(Page.IsPostBack)){
				if (DataLists.GetGroupsDR(custID).HasRows){
					Groups.DataSource=DataLists.GetGroupsDR(custID);
					Groups.DataBind();
					Groups.Items.Insert(0,new ListItem("Please Select a Group","0"));
					Groups.Items.FindByValue("0").Selected = true;
				} else {
					Groups.Items.Add(new ListItem("No Groups Found", "0"));
					Groups.Items.FindByValue("0").Selected = true;
				}
			}
		}

		public string getFTFValue(){
			try{
                ftfValue = "<A href='http://%%emailtofriend%%/'><IMG src='" + System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + "/channels/" + sc.BasechannelID() + "/images/forwardtoafriend.gif' border=0></A>";
				//Response.Write(ftfValue);
			}catch{
				ftfValue = "";
			}
			return ftfValue;
		}

		public string CodeSnippets {
			get { return _CodeSnippets; }
		}

		private ArrayList _groupDataFields = null;
		protected ArrayList GroupDataFields {
			get {
				if (_groupDataFields == null) {					
					_groupDataFields = GroupDataField.GetGroupDataFieldsByGroupID(Convert.ToInt32(Groups.SelectedItem.Value.ToString()));
				}
				return (this._groupDataFields);
			}			
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
			this.Groups.SelectedIndexChanged += new System.EventHandler(this.Groups_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Groups_SelectedIndexChanged(object sender, System.EventArgs e) {
			ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
			_CodeSnippets = ""+
				"<option value='' selected></option>";
            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(es.CurrentCustomer.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ForwardtoaFriend))
            {
				_CodeSnippets += "<option value=\""+getFTFValue()+"\">Forward to Friend</option>";
			}
			_CodeSnippets += 
				"<option value='%%EmailAddress%%'>Email Address</option>"+
				"<option value='%%Title%%'>Title</option>"+
				"<option value='%%FirstName%%'>FirstName</option>"+
				"<option value='%%LastName%%'>LastName</option>"+
				"<option value='%%FullName%%'>Full Name</option>"+
				"<option value='%%Company%%'>Company</option>"+
				"<option value='%%Occupation%%'>Occupation</option>"+
				"<option value='%%Address%%'>Address</option>"+
				"<option value='%%Address2%%'>Address2</option>"+
				"<option value='%%City%%'>City</option>"+
				"<option value='%%State%%'>State</option>"+
				"<option value='%%Zip%%'>Zip</option>"+
				"<option value='%%Country%%'>Country</option>"+
				"<option value='%%Voice%%'>Phone #</option>"+
				"<option value='%%Mobile%%'>Cell Phone #</option>"+
				"<option value='%%Fax%%'>Fax #</option>"+
				"<option value='%%Website%%'>Website</option>"+
				"<option value='%%Age%%'>Age</option>"+
				"<option value='%%Income%%'>Income</option>"+
				"<option value='%%Gender%%'>Gender</option>"+
				"<option value='%%User1%%'>User1 Field</option>"+
				"<option value='%%User2%%'>User2 Field</option>"+
				"<option value='%%User3%%'>User3 Field</option>"+
				"<option value='%%User4%%'>User4 Field</option>"+
				"<option value='%%User5%%'>User5 Field</option>"+
				"<option value='%%User6%%'>User6 Field</option>"+
				"<option value='%%Birthdate%%'>Birthdate [Date Field]</option>"+
				"<option value='%%UserEvent1%%'>UserEvent1</option>"+
				"<option value='%%UserEvent1Date%%'>UserEvent1Date [Date Field]</option>"+
				"<option value='%%UserEvent2%%'>UserEvent2</option>"+
				"<option value='%%UserEvent2Date%%'>UserEvent2Date [Date Field]</option>";

			foreach(GroupDataField field in GroupDataFields) {
				_CodeSnippets += "<option value='%%"+string.Format("{0}", field.ShortName)+"%%'>"+string.Format("{0}", field.ShortName)+"</option>";;
			}
		}
	}
}
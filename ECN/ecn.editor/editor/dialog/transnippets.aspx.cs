using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.communicator.classes;
using ecn.common.classes;


namespace ecn.communicator.contentmanager.feditor.dialog {
	/// <summary>
	/// Summary description for transnippets.
	/// </summary>
	public partial class transnippets : System.Web.UI.Page {
		public ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
        ECN_Framework_BusinessLayer.Application.ECNSession es;

		protected System.Web.UI.HtmlControls.HtmlSelect HDR_FontColor;
		protected System.Web.UI.HtmlControls.HtmlSelect HDR_CellBGColor;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox HDR_Bold;
		protected System.Web.UI.HtmlControls.HtmlSelect ITEM_FontColor;
		protected System.Web.UI.HtmlControls.HtmlSelect ITEM_CellBGColor;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox ITEM_Bold;

		string _groupUDFList = "";
		public string GroupUDFList {
			get { return _groupUDFList; }
		}

		protected void Page_Load(object sender, System.EventArgs e){
            string custID = sc.CustomerID().ToString();
            es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(es.CurrentCustomer.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Transnippets))
            {
				if(!(Page.IsPostBack)){
					if (DataLists.GetGroupsDR(custID).HasRows){
						string groupsWithTransUDFSQL =	" SELECT g.GroupID, g.GroupName "+
							" FROM Groups g JOIN GroupDataFields gdf ON g.GroupID = gdf.GroupID "+
							" WHERE g.CustomerID = "+custID+" AND DataFieldsetID IS NOT Null "+
							" GROUP BY g.GroupID, g.GroupName "+
							" Order by g.GroupName ";

						Groups.DataSource=DataFunctions.GetDataTable(groupsWithTransUDFSQL, ConfigurationManager.AppSettings["com"].ToString());
						Groups.DataBind();
						Groups.Items.Insert(0,new ListItem("Please Select a Group","0"));
						Groups.Items.FindByValue("0").Selected = true;
					} else {
						Groups.Items.Add(new ListItem("No Groups Found", "0"));
						Groups.Items.FindByValue("0").Selected = true;
					}
				}
			}else{
				Response.Redirect("TransnippetsNoAccess.htm");
			}
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
		override protected void OnInit(EventArgs e) {
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
		private void InitializeComponent() {    

		}
		#endregion

		protected void Groups_SelectedIndexChanged(object sender, System.EventArgs e) {
			UDFList.Items.Clear();

			foreach(GroupDataField field in GroupDataFields) {
				//_groupUDFList += "<option value='%%"+string.Format("{0}", field.ShortName)+"%%'>"+string.Format("{0}", field.ShortName)+"</option>";;
				UDFList.Items.Add(new ListItem(string.Format("{0}", field.ShortName), string.Format("{0}", field.ShortName)));
			}
		}
	}
}
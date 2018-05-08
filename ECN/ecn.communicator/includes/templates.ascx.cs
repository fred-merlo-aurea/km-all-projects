namespace ecn.communicator.includes
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
    using System.Linq;
    using System.Collections.Generic;
	
	public partial  class templates : System.Web.UI.UserControl
    {
		public string TemplateID
        {
			set 
            {
				int totaltemplates= templaterepeater.Items.Count;
				int theindex=0;
				for (int i=0; i<totaltemplates; i++)
                {
					string currid=((TextBox)templaterepeater.Items[i].FindControl("TemplateID")).Text;
					if (currid==value)
                    {
						theindex=i;
					}
				}
				templaterepeater.SelectedIndex = theindex;
				RefreshChanges();
			}
			get
            {
				if (templaterepeater.SelectedIndex>0)
                {
					return ((TextBox)templaterepeater.SelectedItem.FindControl("TemplateID")).Text;
				}
                else 
                {
					return ((TextBox) templaterepeater.Items[0].FindControl("TemplateID")).Text;
				}
			}
		}

		public string SlotsTotal
        {
			set {
				templaterepeater.BorderWidth = Convert.ToInt32(value);
			}
			get {
				if (templaterepeater.SelectedIndex>0){
					return ((TextBox)templaterepeater.SelectedItem.FindControl("SlotsTotal")).Text;
				} else {
					return "0";
				}
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
        {
			if (!IsPostBack) {
				loadTable();
			}
		}

		public void loadTable()
        {
            List<ECN_Framework_Entities.Communicator.Template> templateList=
            ECN_Framework_BusinessLayer.Communicator.Template.GetByStyleCode(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, "newsletter", ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser); 
         
            templaterepeater.DataSource = templateList;
            templaterepeater.DataBind();
		}

		public void DoItemSelect(object objSource, DataListCommandEventArgs objArgs) {	
			templaterepeater.SelectedIndex=objArgs.Item.ItemIndex;
			RefreshChanges();
		}

		public void RefreshChanges(){
			loadTable();
			RaiseBubbleEvent(((TextBox)templaterepeater.SelectedItem.FindControl("SlotsTotal")).Text, null);
		}

	}
}

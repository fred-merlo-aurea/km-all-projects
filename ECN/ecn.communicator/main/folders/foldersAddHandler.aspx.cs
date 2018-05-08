using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace ecn.communicator.main.folders 
{
	public partial class foldersAddHandler : System.Web.UI.Page
    {
        #region getters
		private string getAction(){
			string _action = "";
			try { _action = Request.QueryString["atn"].ToString(); } 
			catch{} 
			return _action;
		}

		private int getFolderID(){
            int _fldrID = 0;
            try { _fldrID = Convert.ToInt32(Request.QueryString["fID"].ToString()); }
            catch { } 
			return _fldrID;
		}

		private string getFolderName(){
			string _fldrNm = "";
			try { _fldrNm = Request.QueryString["fName"].ToString(); } 
			catch {} 
			return _fldrNm;
		}

		private string getFolderDesc(){
			string _fldrDesc = "";
			try { _fldrDesc = Request.QueryString["fDesc"].ToString(); } 
			catch {} 
			return _fldrDesc;
		}

		private string getFolderType(){
			string _fldrType = "";
			try { _fldrType = Request.QueryString["fType"].ToString(); } 
			catch {} 
			return _fldrType;
		}
		#endregion

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {

            phError.Visible = false;
			if(getFolderType().Equals("IMG"))
            {
				System.IO.DirectoryInfo dir = null;
				string folderName = getFolderName().ToString();
				folderName = folderName.Replace(" ","_").ToString();
				Match m = Regex.Match(folderName, "^[a-zA-Z0-9_\\s]*$");
				if(m.Success)
                {
                    string path = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID + "/images/" + folderName;
                    dir = new System.IO.DirectoryInfo(Server.MapPath(path));
                    dir.Create();	
				}
                else
                {
					Response.Write("<script>alert('Error occured during Folder Creation.<br>Folder Name cannot have any special Characters. Underscore \"_\" is allowed'); </script>");
				}
			}
            else
            {
                ECN_Framework_Entities.Communicator.Folder folder = new ECN_Framework_Entities.Communicator.Folder();
                folder.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                folder.FolderName = getFolderName();
                folder.FolderDescription = getFolderDesc();
                folder.FolderType = getFolderType();
                folder.ParentID = getFolderID();
                folder.IsSystem = false;
                try
                {
                    ECN_Framework_BusinessLayer.Communicator.Folder.Save(folder, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                }
                catch (ECN_Framework_Common.Objects.ECNException ex)
                {
                    setECNError(ex);
                }
			}

			Response.Redirect("../folders/folderseditor.aspx?fType="+getFolderType());		
		}
	}
}
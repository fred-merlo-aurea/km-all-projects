using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using System.Xml;

namespace ecn.communicator.main.lists
{
	public partial class downloadxmlList : System.Web.UI.Page
	{
        /*
		private int getCustomerID() 
		{
			int theCustomerID = 0;
			try 
			{
				theCustomerID = Convert.ToInt32(Encryption.Decrypt(Request.QueryString["accessKey"].ToString(),"ecn5WSVC"));
			}
			catch{}
			return theCustomerID;
		}


		protected void Page_Load(object sender, System.EventArgs e)
		{
//			string enc = Encryption.Encrypt("1521", "ecn5WSVC");
//			Response.Write(enc + "<BR>");
//			Response.Write(Encryption.Decrypt(enc, "ecn5WSVC")+ "<BR>");
//			Response.End();
			int CustomerID = getCustomerID();

			XmlTextWriter xmlWriter = new XmlTextWriter(Response.OutputStream, Encoding.UTF8); 
			xmlWriter.Formatting = Formatting.Indented;
			xmlWriter.WriteStartDocument(true);

			if (CustomerID > 0)
			{
				string sqlquery=" SELECT case when isnull(f.folderID,'') = '' then 0 else f.folderID end as folderID, case when isnull(ltrim(rtrim(f.foldername)),'') = '' then 'Root' else ltrim(rtrim(f.folderName)) end as Folder, "+
					" g.GroupID, "+					
					" ltrim(rtrim(g.GroupName)) as GroupName, "+
					" COUNT(eg.EmailGroupID) AS Subscribers "+
					" FROM Groups g left outer join  EmailGroups eg on g.groupID = eg.groupID  and eg.SubscribeTypeCode = 'S'   and eg.emailID IN (SELECT EmailID from Emails) left outer join"+
					" folders f on g.FolderID = f.folderID"+
					" WHERE g.CustomerID= " + CustomerID + " GROUP BY f.folderID, g.GroupID, f.foldername, g.GroupName "+
					" ORDER BY f.foldername, g.GroupName ";

			
				DataTable dt = DataFunctions.GetDataTable(sqlquery);

				xmlWriter.WriteStartElement("Lists");

				string oFolderID = string.Empty;
				
				for (int i=0;i<dt.Rows.Count;i++)
				{	
					if (oFolderID != dt.Rows[i]["Folder"].ToString())
					{
						if (oFolderID != string.Empty )
							xmlWriter.WriteEndElement();

						oFolderID = dt.Rows[i]["Folder"].ToString();
						xmlWriter.WriteStartElement("Folder");
						xmlWriter.WriteAttributeString("name",dt.Rows[i]["Folder"].ToString());
					}

					xmlWriter.WriteStartElement("Group");
					xmlWriter.WriteAttributeString("id", dt.Rows[i]["GroupID"].ToString());
					xmlWriter.WriteAttributeString("name", dt.Rows[i]["GroupName"].ToString());
					xmlWriter.WriteRaw(dt.Rows[i]["Subscribers"].ToString());
					xmlWriter.WriteEndElement();

					if (i == dt.Rows.Count-1)
					{
						xmlWriter.WriteEndElement();
					}
				}

				xmlWriter.WriteEndElement();

			}
			else
			{
				xmlWriter.WriteStartElement("Error");
				xmlWriter.WriteRaw("Invalid Customer ID");
				xmlWriter.WriteEndElement();
			}

			xmlWriter.WriteEndDocument();

			if (xmlWriter != null) 
			{
				xmlWriter.Flush();
				xmlWriter.Close();
			}
			Response.ContentType = "text/xml";
			Response.Flush();
			Response.End();
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
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{    

		}
		#endregion
        */
	}
}

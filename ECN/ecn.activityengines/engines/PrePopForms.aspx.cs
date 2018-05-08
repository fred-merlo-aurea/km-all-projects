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
using ecn.common.classes;
using System.Text;
using System.Configuration;

using ecn.communicator.classes;

namespace ecn.activityengines
{
	
	
	
	public partial class PrePopForms : System.Web.UI.Page
	{
		int GroupID = 0;
		private int getSFID() 
		{
			try 
			{
				return Convert.ToInt32(Request.QueryString["sfID"].ToString());
			}
			catch
			{
				return 0;
			}
		}

		private int getEmailID() 
		{
			try 
			{
				return Convert.ToInt32(Request.QueryString["eID"].ToString());
			}
			catch
			{
				return 0;
			}
		}


		Hashtable hFormFields = new Hashtable();
		Hashtable hProfileData  = new Hashtable();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string sfHTML = string.Empty;
			string sfValidation = string.Empty;
			ArrayList aFields = new ArrayList();

			try
			{
				// Get GroupID, sfHTML from SmartForm
				DataTable dtSF = DataFunctions.GetDataTable("select * from smartformshistory where smartformID = " + getSFID() + " and IsDeleted = 0");
				if (dtSF.Rows.Count > 0)
				{
					GroupID = Convert.ToInt32(dtSF.Rows[0]["GroupID"]);
					sfHTML = dtSF.Rows[0]["SmartFormHTML"].ToString();

					LoadFormFields();

					// get Profile/UDF Values for the profile (if exists)
					if (getEmailID() > 0)
						LoadProfileData();

					// get the fields & HTML from SF table
                    DataTable dtSFFields = DataFunctions.GetDataTable("select PrePopFieldID, ProfileFieldName, DisplayName, Isnull(DataType,'') as DataType, ControlType, Isnull(DataValues,'') as DataValues, IsNull(Required,'N') as Required, IsNull(PrePopulate,'N') as PrePopulate from SmartFormsPrePopFields where sfID = " + getSFID() + " and IsDeleted = 0 order by SortOrder asc");
					if (dtSFFields.Rows.Count > 0)
					{
						// read the SF dataTable and load the field definitions/ profile value(if exists) into Collection
						foreach(DataRow dr in dtSFFields.Rows)
							aFields.Add(new PrePop(Convert.ToInt32(dr["prePopFieldID"]), GetFieldCode(dr["ProfileFieldName"].ToString()), dr["DisplayName"].ToString(), dr["ControlType"].ToString(), dr["DataType"].ToString(), dr["Required"].ToString().ToUpper().Equals("Y")?true:false, dr["PrePopulate"].ToString().ToUpper().Equals("Y")?true:false, dr["DataValues"].ToString(), GetFieldData(dr["ProfileFieldName"].ToString())));

						// start appending validations 
						sfValidation = "<script type=\"text/javascript\">var fv = new Array();";
						int arrayCount = 0;

						// Loop thru Collected - get the Control HTMLs and replace the codesnippet in HTML.
						for(int i=0; i<aFields.Count; i++)
						{
							try
							{
								PrePop p = (PrePop) aFields[i];
							
								if (p.ControlType.ToLower() != "hidden")
								{
									sfValidation += "fv[" + arrayCount + "] = \"" + p.FieldName + "|" + (p.IsRequired?"1":"0")  + "|" + p.DataType + "|" + p.DisplayName + "\";";
									arrayCount++;
								}
								sfHTML = sfHTML.Replace("%%" + p.FieldName + "%%", p.getControlHTML());
							
							}
							catch{}
						}
						sfValidation += "</script>";

						// render the HTML in client.
						Response.Write("document.write('" + sfValidation + " " + sfHTML.Replace("'","\'") + "');");				
					}
					else
						DisplayError("Invalid Data");
				}
				else
					DisplayError("Invalid Data");
			}
			catch (Exception ex)
			{
				DisplayError("Error pre populating form.  Customer Service has been notified.");
                //NotifyAdmin(ex);
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "PrePopForms.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                //Helper.LogCriticalError(ex, "PrePopForms.Page_Load");
			}
		}

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<br><b>Smart Form ID:</b>&nbsp;" + getSFID());
                adminEmailVariables.AppendLine("<br><b>Email ID:</b>&nbsp;" + getEmailID());
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        //private void NotifyAdmin(Exception ex)
        //{
        //    StringBuilder adminEmailVariables = new StringBuilder();
        //    string admimEmailBody = string.Empty;

        //    adminEmailVariables.AppendLine("<br><b>Smart Form ID:</b>&nbsp;" + getSFID());
        //    adminEmailVariables.AppendLine("<br><b>Email ID:</b>&nbsp;" + getEmailID());

        //    admimEmailBody = ActivityError.CreateMessage(ex, Request, adminEmailVariables.ToString());

        //    Helper.SendMessage("Error in Activity Engine: Pre Pop Forms", admimEmailBody);
        //}

		private void LoadFormFields()
		{
			hFormFields.Add("emailid", "ei");
			hFormFields.Add("emailaddress", "e");
			hFormFields.Add("customerid", "c");
			hFormFields.Add("title", "t");
			hFormFields.Add("firstname", "fn");
			hFormFields.Add("lastname", "ln");
			hFormFields.Add("fullname", "n,");
			hFormFields.Add("company", "compname");
			hFormFields.Add("occupation", "occ");
			hFormFields.Add("address", "adr");
			hFormFields.Add("address2", "adr2");
			hFormFields.Add("city", "city");
			hFormFields.Add("state", "state");
			hFormFields.Add("zip", "zc");
			hFormFields.Add("country", "ctry");
			hFormFields.Add("voice", "ph");
			hFormFields.Add("mobile", "mph");
			hFormFields.Add("fax", "fax");
			hFormFields.Add("website", "website");
			hFormFields.Add("age", "age");
			hFormFields.Add("income", "income");
			hFormFields.Add("gender", "gndr");
			hFormFields.Add("user1", "usr1");
			hFormFields.Add("user2", "usr2");
			hFormFields.Add("user3", "usr3");
			hFormFields.Add("user4", "usr4");
			hFormFields.Add("user5", "usr5");
			hFormFields.Add("user6", "usr6");
			hFormFields.Add("birthdate", "bdt");
			hFormFields.Add("userevent1", "usrevt1");
			hFormFields.Add("userevent1eate", "usrevtdt1");
			hFormFields.Add("userevent2", "usrevt2");
			hFormFields.Add("userevent2date", "usrevtdt2");

		}

		private void LoadProfileData()
		{
			SqlCommand cmd = new SqlCommand("sp_getUserProfileData");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 0;

			cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
			cmd.Parameters["@GroupID"].Value = GroupID;

			cmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int));
			cmd.Parameters["@EmailID"].Value = getEmailID();

			DataTable dtUserProfile = DataFunctions.GetDataTable(cmd);

			if (dtUserProfile.Rows.Count > 0)
			{
				for (int i=0; i<=dtUserProfile.Columns.Count-1; i++)
				{
					hProfileData.Add(dtUserProfile.Columns[i].ColumnName.ToString().ToLower(), dtUserProfile.Rows[0][i]);
				}
			}
		}

		private string GetFieldData(string FieldName)
		{
			try
			{
				if (hProfileData.Count > 0)
					return hProfileData[FieldName.ToLower()].ToString();
			}
			catch{}
			return string.Empty;
		}

		private string GetFieldCode(string FieldName)
		{
			if (FieldName.ToLower().StartsWith("user_"))
				return FieldName.ToLower();
			else
			{
				return hFormFields[FieldName.ToLower()].ToString();
			}
		}

		private void DisplayError(string strError)
		{
			Response.Write("document.write(\"<table cellpadding='0' cellspacing='0' border='0'><tr><td><strong>" + strError + "</strong><BR><BR></td></tr></table>\")");
			Response.Flush();
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
	}
}

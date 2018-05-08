using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.IO;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;

namespace ecn.communicator.ckeditor.dialog
{
    public partial class mobilehtmlUpload : System.Web.UI.Page
    {

		public string ftfValue = "";
		string _CodeSnippets = "";

        public string getCustomerID()
        {
            try
            {
                if (ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID > 0)
                    return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID.ToString();
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
			
		}

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string htmlSrc = string.Empty;
            string csource = string.Empty;
            if (fileUpload1.PostedFile != null && fileUpload1.PostedFile.FileName.Length > 0)
            {
                //Start - Load the Files to the server & get the data.
                try
                {
                    string ImagePath = "/customers/" + getCustomerID() + "/msgContentFiles";

                    if (!Directory.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + ImagePath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + ImagePath));
                    }

                    string baseLocation = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + ImagePath);
                    string htmlFileToUpload = "";
                    htmlFileToUpload = StringFunctions.Replace(System.IO.Path.GetFileName(fileUpload1.PostedFile.FileName), " ", "_");
                    htmlFileToUpload = StringFunctions.Replace(htmlFileToUpload, "\'", "_");
                    fileUpload1.PostedFile.SaveAs(baseLocation + "\\" + htmlFileToUpload);
                    StreamReader htmlReader = new StreamReader(baseLocation + "\\" + htmlFileToUpload, Encoding.Default);
                    htmlContentSource.Text = htmlReader.ReadToEnd();
                    htmlReader.Close();

                    if (htmlContentSource.Text.ToString().Length > 0)
                    {
                        htmlSrc = StringFunctions.CleanString(htmlContentSource.Text.ToString());
                        StringReader sr = new StringReader(htmlSrc);
                        while (true)
                        {
                            string newLine = sr.ReadLine();
                            if (newLine != null)
                            {
                                if (newLine.StartsWith("."))
                                {
                                    newLine = " " + newLine;
                                }
                                csource += newLine + "\r\n";
                            }
                            else
                            {
                                break;
                            }
                        }
                        sr.Close();
                        hfContentSourceMobile.Value = csource;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "ok();", true);
                    }


                }
                catch (Exception ex)
                {
                    throw new Exception("Error occured during Content import process. Please try again");
                }
            }
        }
	}
}
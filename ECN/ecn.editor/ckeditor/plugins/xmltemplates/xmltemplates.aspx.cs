using System;
using System.Collections;
using System.Web.UI.WebControls;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Xml;
using System.Data;
using System.IO;


namespace ecn.communicator.contentmanager.ckeditor.dialog
{	
    public partial class xmltemplates : System.Web.UI.Page
    {		
		public ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
		public ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();
        ECN_Framework_BusinessLayer.Application.ECNSession es;
	

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

        public string getChannelID()
        {
            try
            {
                if (Convert.ToInt32(sc.BasechannelID()) > 0)
                    return sc.BasechannelID().ToString();
                else
                    return Request.QueryString["chID"].ToString();
            }
            catch
            {
                return Request.QueryString["chID"].ToString();
            }
        }

        public void Page_Load(object sender, EventArgs e)
        {      
            es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            string custID = getCustomerID();
            string chID = getChannelID();

            if (!IsPostBack)
            {
                string filepath="";
                if (File.Exists(Server.MapPath("/ecn.editor/ckeditor/plugins/xmltemplates/" + chID + "_" + custID + "_" + "fcktemplates.xml")))
                {
                    filepath = Server.MapPath("/ecn.editor/ckeditor/plugins/xmltemplates/" + chID + "_" + custID + "_" + "fcktemplates.xml");
                }
                else if (File.Exists(Server.MapPath("/ecn.editor/ckeditor/plugins/xmltemplates/" + chID + "_" + "fcktemplates.xml")))
                {
                    filepath = Server.MapPath("/ecn.editor/ckeditor/plugins/xmltemplates/" + chID + "_" + "fcktemplates.xml");
                }
                else
                {
                    filepath = Server.MapPath("/ecn.editor/ckeditor/plugins/xmltemplates/fcktemplates.xml");
                }
                readxml(filepath);
            }
			  
		}

        public void readxml(string filepath)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("templateTitle", typeof(string));
                dt.Columns.Add("templateImage", typeof(string));
                dt.Columns.Add("templateHTML", typeof(string));

                XmlTextReader reader = new XmlTextReader(filepath);
                reader.ReadToFollowing("Templates");
                reader.MoveToFirstAttribute();
                string imagepath = "/ecn.editor/ckeditor/plugins/xmltemplates/" + reader.Value;               

                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);
                XmlNodeList templates = doc.SelectNodes("//Template");
                foreach (XmlNode template in templates)
                {
                    DataRow dr = dt.NewRow();
                    dr["templateTitle"] = template.Attributes["title"].Value;
                    dr["templateImage"] = imagepath +  template.Attributes["image"].Value;
                    dr["templateHTML"] = template.ChildNodes[1].ChildNodes[0].Value;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
                Repeater1.DataSource = dt;
                Repeater1.DataBind();
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;

            }
        }

       


		
		
	}
}
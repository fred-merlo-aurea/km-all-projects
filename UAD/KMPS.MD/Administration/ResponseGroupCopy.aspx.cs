using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using System.Xml;

namespace KMPS.MD.Administration
{
    public partial class ResponseGroupCopy : KMPS.MD.Main.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Products";
            Master.SubMenu = "Response Group Copy";
            divError.Visible = false;
            lblErrorMessage.Text = "";
            lblMessage.Text = "";

            if (!IsPostBack)
            {
                drpPubsFrom.DataSource = Pubs.GetActive(Master.clientconnections);
                drpPubsFrom.DataBind();
                drpPubsFrom.Items.Insert(0, new ListItem("Select Product", "0"));

                GetPubs();

                List<Pubs> lp = Pubs.GetAll(Master.clientconnections);

                foreach (Pubs p in lp)
                {
                    List<ResponseGroup> lrg = KMPS.MD.Objects.ResponseGroup.GetByPubID(Master.clientconnections, p.PubID);

                    foreach (ResponseGroup rg in lrg)
                    {
                        CodeSheet.DeleteCache(Master.clientconnections, rg.ResponseGroupID);
                    }

                    KMPS.MD.Objects.ResponseGroup.DeleteCache(Master.clientconnections, p.PubID);
                }
            }
        }

        protected void GetPubs()
        {
            lstPubs.DataSource = Pubs.GetActive(Master.clientconnections);
            lstPubs.DataBind();
        }

        protected void drpPubsFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpPubsFrom.SelectedItem.Value != "0")
            {
                drpResponseGroupFrom.DataSource = ResponseGroup.GetActiveByPubID(Master.clientconnections, Convert.ToInt32(drpPubsFrom.SelectedItem.Value));
                drpResponseGroupFrom.DataBind();
                drpResponseGroupFrom.Items.Insert(0, new ListItem("Select Response Group", "0"));
                GetPubs();
                lstPubs.Items.Remove(lstPubs.Items.FindByValue(drpPubsFrom.SelectedItem.Value));
            }
          
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedvalues = string.Empty;
                string selectedPubIDs = string.Empty;

                XmlDocument xmlDoc = new XmlDocument();
                XmlElement xmlNode = xmlDoc.CreateElement("XML");
                xmlDoc.AppendChild(xmlNode);

                foreach (ListItem item in lstPubs.Items)
                {
                    if (item.Selected)
                    {
                        XmlElement xmlPub;
                        xmlPub = xmlDoc.CreateElement("Pub");

                        XmlAttribute xmlID;
                        xmlID = xmlDoc.CreateAttribute("ID");
                        xmlID.InnerText = item.Value.ToString();
                        xmlPub.Attributes.Append(xmlID);

                        xmlNode.AppendChild(xmlPub);

                        selectedvalues += selectedvalues == string.Empty ? item.Text : "," + item.Text;
                        selectedPubIDs += selectedPubIDs == string.Empty ? item.Value : "," + item.Value;
                    }
                }

                ResponseGroup.Copy(Master.clientconnections, Convert.ToInt32(drpResponseGroupFrom.SelectedItem.Value), xmlDoc.OuterXml, selectedPubIDs, Master.LoggedInUser);
                lblMessage.Text = "Copied info from " + drpResponseGroupFrom.SelectedItem.ToString() + " to " + selectedvalues;
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                divError.Visible = true;
            }
        }
    }
}
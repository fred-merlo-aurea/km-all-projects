using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;

namespace ecn.communicator.main.dripmarketing
{
    public partial class blastDesigner : System.Web.UI.Page
    {
        private int CampaignID
        {
            get
            {
                if (campaignID.Value.Equals("0"))
                    return 0;
                else
                    return Convert.ToInt32(campaignID.Value);
            }

            set { campaignID.Value = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
             msglabel.Visible = false;
             if (!IsPostBack)
             {
                 if (Request.QueryString["campaignID"] != null)
                 {
                     ECN_Framework_Entities.Communicator.Campaign c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(Convert.ToInt32(Request.QueryString["campaignID"].ToString()), Master.UserSession.CurrentUser, false);
                     campaignID.Value = c.CampaignID.ToString();
                     xmlSaveData.Value = c.DripDesign;
                 }
                 else
                 {
                     mdlPopSave.Show();
                 }
             }
        }  

        protected void btnPopupSaveCampaign_Click(object sender, EventArgs e)
        {
            UpdateCampaign();
            mdlPopSave.Hide();
        }

        protected void btnMdlPopupSave_Click(object sender, EventArgs e)
        {
            if (CampaignID == 0)
            {               
                mdlPopSave.Show();
            }
            else
            {
                UpdateCampaign();
            }
        }

        private void UpdateCampaign()
        {
            string JSON = string.Empty;
            if (!xmlSaveData.Value.Equals(""))
            {
                try
                {
                    string saveXML = xmlSaveData.Value;
                    msglabel.Text = saveXML;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(saveXML);
                    JSON = XmlToJSON(doc);
                    JSON = JSON.Replace(@"\", @"\\");
                }
                catch (Exception ex)
                {
                    JSON = xmlSaveData.Value;
                }
            }
            UpdateCampaign_DB(JSON);
        }

        protected void btnDeletePostback_Click(object sender, EventArgs e)
        {
            try
            {
                string thisNode = currentNode.Value.ToString();
                ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByNodeID(thisNode, Master.UserSession.CurrentUser, true);
                if (item != null)
                {
                    ECN_Framework_BusinessLayer.Communicator.CampaignItem.Delete(item.CampaignID.Value, item.CampaignItemID, Master.UserSession.CurrentUser);
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "deleteNode('"  + thisNode + "');", true);                    
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "alert('This action cannot be deleted');", true);
                       
            }
        }

        protected void btnConfigurePostback_Click(object sender, EventArgs e)
        {
            string thisNode = currentNode.Value.ToString();
            ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByNodeID(thisNode, Master.UserSession.CurrentUser, true);

            if (item != null)
            {
                if (item.BlastList.Count > 0 )
                {
                    var blastExists = item.BlastList.Where(x => x.BlastID != null);
                    if (blastExists.Any() == true)
                    {
                        ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(item.BlastList[0].BlastID.Value, Master.UserSession.CurrentUser, false);
                        if (blast.SendTime < DateTime.Now)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "viewReport_confirm(" + item.CampaignItemID.ToString() + ");", true);
                            return;
                        }
                    }
                }
            }

            if (!originNode.Value.Equals("0"))
            {
                string[] originNodeVal = originNode.Value.ToString().Split('-');
                regBlast1.myparentNodeId = originNodeVal[0].ToString() + "-" + originNodeVal[1].ToString();
                regBlast1.myparentFilterType = originNodeVal[2].ToString();
                ECN_Framework_Entities.Communicator.CampaignItem item_parent = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByNodeID(regBlast1.myparentNodeId, Master.UserSession.CurrentUser, true);


                if (item_parent == null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "alert('Incorrect order. Please configure the preceeding Campagin Items');", true);
                    return;
                }
                if (item_parent.BlastList.Count>1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "alert('Preceeding Campaign Items contain more than one group');", true);
                    return;
                }


                if (regBlast1.myparentFilterType.Equals("CLICK"))
                {
                    regBlast1.myparentFilterID = (int.MaxValue - 5);
                }
                else if (regBlast1.myparentFilterType.Equals("NOCLICK"))
                {
                    regBlast1.myparentFilterID = int.MaxValue;
                }
                else if (regBlast1.myparentFilterType.Equals("OPEN"))
                {
                    regBlast1.myparentFilterID = (int.MaxValue - 4);
                }
                else if (regBlast1.myparentFilterType.Equals("NOOPEN"))
                {
                    regBlast1.myparentFilterID = (int.MaxValue - 2);
                }
                regBlast1.Initialize(item_parent);
            }
            else
            {
                regBlast1.myparentNodeId = string.Empty;
                regBlast1.myparentFilterID = 0 ;
                regBlast1.myparentFilterType = "";
                regBlast1.Initialize(null);
            }
            regBlast1.myNodeID = thisNode;

            if (item != null)
            {
                regBlast1.loadData(item);
            }
            mdlPopNewBlast.Show();
            
        }

        protected void Schedule_Close(object sender, EventArgs e)
        {          
            regBlast1.Reset();
            this.mdlPopNewBlast.Hide();
        }

        protected void btnPopupCloseSaveCampaign_Click(object sender, EventArgs e)
        {
            txtCampaignName.Text = "";
            mdlPopSave.Hide();
        }

        

        protected void Schedule_Save(object sender, EventArgs e)
        {
            string thisNode = currentNode.Value.ToString();
            ECN_Framework_Entities.Communicator.CampaignItem item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByNodeID(thisNode, Master.UserSession.CurrentUser, true);
            if (originNode.Value.Equals("0"))
            {
                if (regBlast1.saveCampaignItem_regular(item, CampaignID))
                {
                    update_LayoutName_Campaign(thisNode);
                }
            }
            else
            {
                string[] originNodeVal = originNode.Value.ToString().Split('-');
                regBlast1.myparentNodeId = originNodeVal[0].ToString() + "-" + originNodeVal[1].ToString();
                ECN_Framework_Entities.Communicator.CampaignItem itemParent = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByNodeID(regBlast1.myparentNodeId, Master.UserSession.CurrentUser, true);
          
                string oldFilterID = originNodeVal[2].ToString();
                if (regBlast1.saveCampaignItem_smartsegment(item, itemParent, CampaignID))
                {
                    update_LayoutName_Campaign(thisNode);
                }
            }
        }

        private void update_LayoutName_Campaign(string thisNode)
        {
            ECN_Framework_Entities.Communicator.CampaignItem  item = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByNodeID(thisNode, Master.UserSession.CurrentUser, true);
            if (item.BlastList != null)
            {
                if (item.BlastList.Count > 0)
                {
                    ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(item.BlastList[0].LayoutID.Value, Master.UserSession.CurrentUser, false);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "updateNodeContent('" + thisNode + "', '" + layout.LayoutName + "');", true);
                }
            }
            regBlast1.Reset();
            this.mdlPopNewBlast.Hide();
        }

        protected void UpdateCampaign_DB(string JSON)
        {
            ECN_Framework_Entities.Communicator.Campaign c;
            if (CampaignID == 0)
            {
                c = new ECN_Framework_Entities.Communicator.Campaign();
                c.CampaignName = txtCampaignName.Text;
                c.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            }
            else
            {
                c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(CampaignID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                c.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
          
            }
            c.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID;
            c.DripDesign = JSON;
            ECN_Framework_BusinessLayer.Communicator.Campaign.Save(c, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            CampaignID = c.CampaignID;
        }

        private static string XmlToJSON(XmlDocument xmlDoc)
        {
            StringBuilder sbJSON = new StringBuilder();
            sbJSON.Append("{ ");
            XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, true);
            sbJSON.Append("}");
            return sbJSON.ToString();
        }

        //  XmlToJSONnode:  Output an XmlElement, possibly as part of a higher array
        private static void XmlToJSONnode(StringBuilder sbJSON, XmlElement node, bool showNodeName)
        {
            if (showNodeName)
                sbJSON.Append("\"" + SafeJSON(node.Name) + "\": ");
            sbJSON.Append("{");
            // Build a sorted list of key-value pairs
            //  where   key is case-sensitive nodeName
            //          value is an ArrayList of string or XmlElement
            //  so that we know whether the nodeName is an array or not.
            SortedList childNodeNames = new SortedList();

            //  Add in all node attributes
            if (node.Attributes != null)
                foreach (XmlAttribute attr in node.Attributes)
                    StoreChildNode(childNodeNames, attr.Name, attr.InnerText);

            //  Add in all nodes
            foreach (XmlNode cnode in node.ChildNodes)
            {
                if (cnode is XmlText)
                    StoreChildNode(childNodeNames, "value", cnode.InnerText);
                else if (cnode is XmlElement)
                    StoreChildNode(childNodeNames, cnode.Name, cnode);
            }

            // Now output all stored info
            foreach (string childname in childNodeNames.Keys)
            {
                ArrayList alChild = (ArrayList)childNodeNames[childname];
                if (alChild.Count == 1)
                    OutputNode(childname, alChild[0], sbJSON, true);
                else
                {
                    sbJSON.Append(" \"" + SafeJSON(childname) + "\": [ ");
                    foreach (object Child in alChild)
                        OutputNode(childname, Child, sbJSON, false);
                    sbJSON.Remove(sbJSON.Length - 2, 2);
                    sbJSON.Append(" ], ");
                }
            }
            sbJSON.Remove(sbJSON.Length - 2, 2);
            sbJSON.Append(" }");
        }

        //  StoreChildNode: Store data associated with each nodeName
        //                  so that we know whether the nodeName is an array or not.
        private static void StoreChildNode(SortedList childNodeNames, string nodeName, object nodeValue)
        {
            // Pre-process contraction of XmlElement-s
            if (nodeValue is XmlElement)
            {
                // Convert  <aa></aa> into "aa":null
                //          <aa>xx</aa> into "aa":"xx"
                XmlNode cnode = (XmlNode)nodeValue;
                if (cnode.Attributes.Count == 0)
                {
                    XmlNodeList children = cnode.ChildNodes;
                    if (children.Count == 0)
                        nodeValue = null;
                    else if (children.Count == 1 && (children[0] is XmlText))
                        nodeValue = ((XmlText)(children[0])).InnerText;
                }
            }
            // Add nodeValue to ArrayList associated with each nodeName
            // If nodeName doesn't exist then add it
            object oValuesAL = childNodeNames[nodeName];
            ArrayList ValuesAL;
            if (oValuesAL == null)
            {
                ValuesAL = new ArrayList();
                childNodeNames[nodeName] = ValuesAL;
            }
            else
                ValuesAL = (ArrayList)oValuesAL;
            ValuesAL.Add(nodeValue);
        }

        private static void OutputNode(string childname, object alChild, StringBuilder sbJSON, bool showNodeName)
        {
            if (alChild == null)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                sbJSON.Append("null");
            }
            else if (alChild is string)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                string sChild = (string)alChild;
                sChild = sChild.Trim();
                sbJSON.Append("\"" + SafeJSON(sChild) + "\"");
            }
            else
                XmlToJSONnode(sbJSON, (XmlElement)alChild, showNodeName);
            sbJSON.Append(", ");
        }

        // Make a string safe for JSON
        private static string SafeJSON(string sIn)
        {
            StringBuilder sbOut = new StringBuilder(sIn.Length);
            foreach (char ch in sIn)
            {
                if (Char.IsControl(ch) || ch == '\'')
                {
                    int ich = (int)ch;
                    sbOut.Append(@"\u" + ich.ToString("x4"));
                    continue;
                }
                //else if (ch == '\"' || ch == '\\' || ch == '/')
                //{
                //    sbOut.Append('\\');
                //}
                sbOut.Append(ch);
            }
            return sbOut.ToString();
        }
    }
}
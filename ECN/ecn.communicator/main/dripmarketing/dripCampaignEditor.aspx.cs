﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Xml;
using System.Text;
using System.Data;
using System.Configuration;


namespace ecn.communicator.main.dripmarketing
{
    public partial class dripCampaignEditor : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT;
            Master.SubMenu = "new content";
            Master.Heading = "Blast Designer";
            Master.HelpContent = "<b>HELP CONTENT NOT AVAILABLE</b>";
            Master.HelpTitle = "Blast Designer";

            msglabel.Visible = false;

            if (!IsPostBack)
            {
                GetData();
            }
        }

                    
        protected void btnPopupSaveCampaign_Click(object sender, EventArgs e)
        {
            string saveXML = xmlSaveData.Value;
            msglabel.Text = saveXML;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(saveXML);
            string JSON = XmlToJSON(doc);
            //error here on updating every time
            JSON = JSON.Replace(@"\", @"\\");
            UpdateDB(JSON, txtCampaignName.Text);
            mdlPopSave.Hide();
        }

        private void GetData()
        {
            //SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Communicator"].ToString());
            //try
            //{
            //    connection.Open();
            //    SqlCommand command = new SqlCommand("select JSONcode from  DripCampaigns where DripCampaignID=@dripCampaignID", connection);
            //    command.Parameters.AddWithValue("@dripCampaignID", dripCampaignID);
            //    SqlDataReader dr= command.ExecuteReader();
            //    DataTable dt = new DataTable();
            //    dt.Load(dr);
            //    xmlSaveData.Value = dt.Rows[0][0].ToString();

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    connection.Close();
            //}
        }


        private void UpdateDB(string JSON, string campaignname)
        {          
            ////update db with new JSON string
            ////Code to insert JSON string into DB
            //SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Communicator"].ToString());
            //try
            //{
            //    connection.Open();
            //    SqlCommand command = new SqlCommand("update DripCampaigns set UserID=@userid, CustomerID=@custid,  JSONcode=@json where DripCampaignID=@dripCampaignID", connection);
            //    command.Parameters.AddWithValue("@userid", userID);
            //    command.Parameters.AddWithValue("@custid", customerID);
            //    command.Parameters.AddWithValue("@json", JSON);
            //    command.Parameters.AddWithValue("@dripCampaignID", dripCampaignID);
            //    command.ExecuteNonQuery();

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    connection.Close();
            //}
           

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
                else if (ch == '\"' || ch == '\\' || ch == '/')
                {
                    sbOut.Append('\\');
                }
                sbOut.Append(ch);
            }
            return sbOut.ToString();
        }

       

      
    }
}
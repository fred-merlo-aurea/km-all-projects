using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using KMPS.MD.Objects;
using System.IO;
using System.Xml;

using KMCommonDataFunctions = KM.Common.DataFunctions;

namespace KMPS.MD.Main
{
    public partial class MarketsNew : KMPS.MD.Main.WebPageHelper
    {
        #region Protected Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Markets";
            if (!IsPostBack)
            {
                CreateXMLStructure();
                //xmlCurrent.DocumentContent = Session["CurrentXML"].ToString();
                //xmlCurrent.DocumentSource = "c:\\projects\\xmlbindingtest\\example.xml";
                //xmlCurrent.TransformSource = "c:\\projects\\xmlbindingtest\\example.xslt";
                LoadMarkets();
                LoadPubTypes();
                ddlPubTypes.SelectedIndex = 0;
                LoadMasterGroups();
                ddlMasterGroups.SelectedIndex = 0;
                LoadSelectedList();
                LoadAvailableList();
            }
            
        }

         protected void Page_LoadComplete(object sender, EventArgs e)
        {
            xmlCurrent.DocumentSource = "c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml";
            xmlCurrent.TransformSource = "c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\XML\\market.xslt";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string pubIds = string.Empty;
                string MasterIDs = string.Empty;

                int marketID = Convert.ToInt32(lblMarketID.Text);

                if (Convert.ToInt32(KMCommonDataFunctions.ExecuteScalar(
                        "SELECT count(MarketID) from Markets where MarketName = '" + txtMarketName.Text.Trim() + "' and MarketID <> " + marketID.ToString(), 
                        DataFunctions.GetClientSqlConnection(Master.clientconnections))) > 0)
                {
                    divError.Visible = true;
                    lblErrorMessage.Text = "The Market Name you entered already exists. Please enter the different name.";
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load("c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml");

                    SqlCommand cmd = new SqlCommand("sp_SaveMarketsWithXML");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MarketID", SqlDbType.Int)).Value = marketID;
                    cmd.Parameters.Add(new SqlParameter("@MarketName", SqlDbType.VarChar)).Value = txtMarketName.Text.Trim();
                    cmd.Parameters.Add(new SqlParameter("@CurrentXML", SqlDbType.Xml)).Value = doc.OuterXml;
                    DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(Master.clientconnections));

                    divError.Visible = false;
                    CreateXMLStructure();
                    LoadMarkets();
                    LoadPubTypes();
                    ddlPubTypes.SelectedIndex = 0;
                    LoadMasterGroups();
                    ddlMasterGroups.SelectedIndex = 0;
                    LoadSelectedList();
                    LoadAvailableList();
                    lblMarketID.Text = "0";
                    txtMarketName.Text = "";
                    bpAddMarkets.Title = " Add Markets";     
                }
            }
            catch (Exception ex) 
            {
                string temp = ex.Message;
            }



        }


        public void dlMarkets_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {

            try
            {
                if (e.CommandName == "DeleteMarket")
                {
                    KMCommonDataFunctions.ExecuteScalar(
                        "DELETE FROM MasterMarkets where MarketID = " + e.CommandArgument.ToString() + ";DELETE FROM PubMarkets where MarketID = " + e.CommandArgument.ToString() + "; DELETE FROM Markets where MarketID = " + e.CommandArgument, 
                        DataFunctions.GetClientSqlConnection(Master.clientconnections));
                    lblMarketID.Text = "0";
                    txtMarketName.Text = "";

                    bpAddMarkets.Title = " Add Markets";

                    CreateXMLStructure();
                    LoadMarkets();
                    ddlPubTypes.SelectedIndex = 0;
                    ddlMasterGroups.SelectedIndex = 0;
                    LoadSelectedList();
                    LoadAvailableList();
                }
                else if (e.CommandName == "EditMarket")
                {
                    DataTable dt = DataFunctions.getDataTable("SELECT MarketID,MarketName, MarketXML FROM Markets where MarketID = " + e.CommandArgument.ToString(), DataFunctions.GetClientSqlConnection(Master.clientconnections));
                    txtMarketName.Text = dt.Rows[0]["MarketName"].ToString();
                    lblMarketID.Text = e.CommandArgument.ToString();
                    bpAddMarkets.Title = " Edit Market";
                    divError.Visible = false;
                    RemoveFile();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(dt.Rows[0]["MarketXML"].ToString());
                    string fileName = "c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml";
                    doc.Save(fileName);
                    ddlPubTypes.SelectedIndex = 0;
                    ddlMasterGroups.SelectedIndex = 0;
                    LoadSelectedList();
                    LoadAvailableList();
                }

            }
            catch (Exception) { }
        }

        #region Pub Events
        protected void ddlPubTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedPubs();
            LoadAvailablePubs();
        }

        protected void lbAvailablePubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbSelectedPubs.SelectedIndex = -1;
        }

        protected void lbSelectedPubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbAvailablePubs.SelectedIndex = -1;
        }

        protected void btnAddPub_Click(object sender, EventArgs e)
        {
            ListBox lbToRemove = new ListBox();
            ListItemCollection items = new ListItemCollection();
            foreach (ListItem item in lbAvailablePubs.Items)
            {
                if (item.Selected)
                {
                    lbSelectedPubs.Items.Add(item);
                    items.Add(item);
                    lbToRemove.Items.Add(item);
                }
            }
            AddPubToXML(items);

            foreach (ListItem item in lbToRemove.Items)
            {
                lbAvailablePubs.Items.Remove(item);
            }

            if (lbAvailablePubs.Items.Count > 0)
            {
                btnAddPub.Enabled = true;
            }
            else
            {
                btnAddPub.Enabled = false;
            }
            btnRemovePub.Enabled = true;

            xmlCurrent.DocumentSource = "c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml";
            xmlCurrent.TransformSource = "c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\XML\\market.xslt";
        }

        protected void btnRemovePub_Click(object sender, EventArgs e)
        {
            ListBox lbToRemove = new ListBox();
            ListItemCollection items = new ListItemCollection();
            foreach (ListItem item in lbSelectedPubs.Items)
            {
                if (item.Selected)
                {
                    lbAvailablePubs.Items.Add(item);
                    items.Add(item);
                    lbToRemove.Items.Add(item);
                }
            }
            RemovePubFromXML(items);

            foreach (ListItem item in lbToRemove.Items)
            {
                lbSelectedPubs.Items.Remove(item);
            }

            if (lbSelectedPubs.Items.Count > 0)
            {
                btnRemovePub.Enabled = true;
            }
            else
            {
                btnRemovePub.Enabled = false;
            }
            btnAddPub.Enabled = true;
        }
        #endregion

        #region Dimension Events
        protected void ddlMasterGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedDimensions();
            LoadAvailableDimensions();
        }

        protected void lbAvailableDimensions_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbSelectedDimensions.SelectedIndex = -1;
        }

        protected void lbSelectedDimensions_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbAvailableDimensions.SelectedIndex = -1;
        }

        protected void btnAddDimension_Click(object sender, EventArgs e)
        {
            ListBox lbToRemove = new ListBox();
            ListItemCollection items = new ListItemCollection();
            foreach (ListItem item in lbAvailableDimensions.Items)
            {
                if (item.Selected)
                {
                    lbSelectedDimensions.Items.Add(item);
                    items.Add(item);
                    lbToRemove.Items.Add(item);
                }
            }
            AddDimensionToXML(items);

            foreach (ListItem item in lbToRemove.Items)
            {
                lbAvailableDimensions.Items.Remove(item);
            }

            if (lbAvailableDimensions.Items.Count > 0)
            {
                btnAddDimension.Enabled = true;
            }
            else
            {
                btnAddDimension.Enabled = false;
            }
            btnRemoveDimension.Enabled = true;
        }

        protected void btnRemoveDimension_Click(object sender, EventArgs e)
        {
            ListBox lbToRemove = new ListBox();
            ListItemCollection items = new ListItemCollection();
            foreach (ListItem item in lbSelectedDimensions.Items)
            {
                if (item.Selected)
                {
                    lbAvailableDimensions.Items.Add(item);
                   items.Add(item);
                    lbToRemove.Items.Add(item);
                }
            }
            RemoveDimensionFromXML(items);

            foreach (ListItem item in lbToRemove.Items)
            {
                lbSelectedDimensions.Items.Remove(item);
            }

            if (lbSelectedDimensions.Items.Count > 0)
            {
                btnRemoveDimension.Enabled = true;
            }
            else
            {
                btnRemoveDimension.Enabled = false;
            }
            btnAddDimension.Enabled = true;
        }
        #endregion

        #endregion

        #region Private Methods
        private void RemoveFile()
        {
            FileInfo TheFile = new FileInfo("c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml");
            if (TheFile.Exists)
            {
                File.Delete("c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml");
            }
        }

        private void LoadMarkets()
        {
            dlMarkets.DataSource = DataFunctions.getDataTable("select MarketID, MarketName from Markets order by Marketname asc", DataFunctions.GetClientSqlConnection(Master.clientconnections));
            dlMarkets.DataBind();
        }

        private void LoadSelectedList()
        {
            LoadSelectedDimensions();
            LoadSelectedPubs();
        }

        private void LoadAvailableList()
        {
            LoadAvailableDimensions();
            LoadAvailablePubs();
        }

        private void LoadGrid()
        {

        }

        private void CreateXMLStructure()
        {
            string currentXML = "";

            currentXML = @"<Market><PubTypes></PubTypes><Dimensions></Dimensions></Market>";
            //for testing
            //currentXML = @"<?xml version=""1.0"" encoding=""utf-8"" ?><Market><PubTypes><PubType ID=""2""><Pub ID=""127"" Title=""WPOU - WATT POULTRY""/></PubType></PubTypes><Dimensions><MasterGroup ID=""6""><Entry ID=""58"" Title=""3. Macro Market Wood""/></MasterGroup><MasterGroup ID=""2""><Entry ID=""1"" Title=""MG2 - Entry 1""/><Entry ID=""2"" Title=""MG2 - Entry 2""/></MasterGroup></Dimensions></Market>";
            
            RemoveFile();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(currentXML);
            string fileName = "c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml";
            doc.Save(fileName);

            return;
        }

        #region Pub Methods
        private void LoadPubTypes()
        {
            ddlPubTypes.Items.Clear();
            ddlPubTypes.DataSource = DataFunctions.getDataTable("select PubTypeID as 'ID', PubTypeDisplayName as 'Title' from PubTypes where IsActive = 1 order by SortOrder ASC", DataFunctions.GetClientSqlConnection(Master.clientconnections));
            ddlPubTypes.DataTextField = "Title";
            ddlPubTypes.DataValueField = "ID";
            ddlPubTypes.DataBind();
        }

        private void LoadSelectedPubs()
        {
            XmlDataSource xmldsSelected = new XmlDataSource();
            xmldsSelected.ID = "xmldsSelectedID";
            xmldsSelected.DataFile = "c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml";
            xmldsSelected.XPath = "//Market/PubTypes/PubType[@ID ='" + ddlPubTypes.SelectedValue + "']/Pub";
            lbSelectedPubs.DataSource = null;

            lbSelectedPubs.DataSource = xmldsSelected;
            lbSelectedPubs.DataValueField = "ID";
            lbSelectedPubs.DataTextField = "Title";
            lbSelectedPubs.DataBind();
            if (lbSelectedPubs.Items.Count > 0)
            {
                btnRemovePub.Enabled = true;
            }
            else
            {
                btnRemovePub.Enabled = false;
            }


        }

        private void LoadAvailablePubs()
        {
            lbAvailablePubs.Items.Clear();
            lbAvailablePubs.DataSource = DataFunctions.getDataTable("select PubID as 'ID', PubName as 'Title' from Pubs where PubTypeID = " + ddlPubTypes.SelectedValue.ToString() + " order by 2 asc", DataFunctions.GetClientSqlConnection(Master.clientconnections));
            lbAvailablePubs.DataTextField = "Title";
            lbAvailablePubs.DataValueField = "ID";
            lbAvailablePubs.DataBind();

            for (int i = lbAvailablePubs.Items.Count; i > 0; i--)
            {
                ListItem item = lbAvailablePubs.Items[i - 1];
                foreach (ListItem item2 in lbSelectedPubs.Items)
                {
                    if (item.Value == item2.Value)
                    {
                        lbAvailablePubs.Items.Remove(item);
                        break;
                    }
                }
            }
            if (lbAvailablePubs.Items.Count > 0)
            {
                btnAddPub.Enabled = true;
            }
            else
            {
                btnAddPub.Enabled = false;
            }
        }

        private void AddPubToXML(ListItemCollection items)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml");
            XmlNode node = doc.SelectSingleNode("//Market/PubTypes/PubType[@ID ='" + ddlPubTypes.SelectedValue + "']");
            if (node == null)
            {
                XmlNode nodePubTypes = doc.SelectSingleNode("//Market/PubTypes");
                XmlNode nodePubType = doc.CreateNode(XmlNodeType.Element, "PubType", null);
                XmlAttribute attribPubTypeID = doc.CreateAttribute("ID");
                attribPubTypeID.Value = ddlPubTypes.SelectedValue;
                nodePubType.Attributes.Append(attribPubTypeID);
                nodePubTypes.AppendChild(nodePubType);
                node = doc.SelectSingleNode("//Market/PubTypes/PubType[@ID ='" + ddlPubTypes.SelectedValue + "']");
            }
            foreach (ListItem item in items)
            {
                XmlNode nodeNew = doc.CreateNode(XmlNodeType.Element, "Pub", null);
                XmlAttribute attribID = doc.CreateAttribute("ID");
                attribID.Value = item.Value;
                XmlAttribute attribTitle = doc.CreateAttribute("Title");
                attribTitle.Value = item.Text;
                nodeNew.Attributes.Append(attribID);
                nodeNew.Attributes.Append(attribTitle);
                node.AppendChild(nodeNew);
            }
            
            RemoveFile();
            doc.Save("c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml");
        }

        private void RemovePubFromXML(ListItemCollection items)
        {
            RemoveItemsFromXml(
                $"//Market/PubTypes/PubType[@ID ='{ddlPubTypes.SelectedValue}']", 
                "/Pub[@ID ='{0}']", 
                items);
        }
        #endregion

        #region Dimension Methods
        private void LoadMasterGroups()
        {
            ddlMasterGroups.Items.Clear();
            ddlMasterGroups.DataSource = DataFunctions.getDataTable("select MasterGroupID as 'ID', DisplayName as 'Title' from MasterGroups where IsActive = 1 and EnableSubReporting = 1 order by SortOrder ASC", DataFunctions.GetClientSqlConnection(Master.clientconnections));
            ddlMasterGroups.DataTextField = "Title";
            ddlMasterGroups.DataValueField = "ID";
            ddlMasterGroups.DataBind();
        }

        private void LoadSelectedDimensions()
        {
            XmlDataSource xmldsSelected = new XmlDataSource();
            xmldsSelected.ID = "xmldsSelectedID";
            xmldsSelected.DataFile = "c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml";
            xmldsSelected.XPath = "//Market/Dimensions/MasterGroup[@ID ='" + ddlMasterGroups.SelectedValue + "']/Entry";
            lbSelectedDimensions.DataSource = xmldsSelected;
            lbSelectedDimensions.DataValueField = "ID";
            lbSelectedDimensions.DataTextField = "Title";
            lbSelectedDimensions.DataBind();
            if (lbSelectedDimensions.Items.Count > 0)
            {
                btnRemoveDimension.Enabled = true;
            }
            else
            {
                btnRemoveDimension.Enabled = false;
            }
        }

        private void LoadAvailableDimensions()
        {
            lbAvailableDimensions.Items.Clear();
            lbAvailableDimensions.DataSource = DataFunctions.getDataTable("select masterID as 'ID', mastervalue + '. ' + masterDesc as 'Title' from mastercodesheet where mastergroupid = " + ddlMasterGroups.SelectedValue.ToString() + " order by 2 asc", DataFunctions.GetClientSqlConnection(Master.clientconnections));
            lbAvailableDimensions.DataTextField = "Title";
            lbAvailableDimensions.DataValueField = "ID";
            lbAvailableDimensions.DataBind();


            for (int i = lbAvailableDimensions.Items.Count; i > 0; i--)
            {
                ListItem item = lbAvailableDimensions.Items[i - 1];
                foreach (ListItem item2 in lbSelectedDimensions.Items)
                {
                    if (item.Value == item2.Value)
                    {
                        lbAvailableDimensions.Items.Remove(item);
                        break;
                    }
                }
            }
            if (lbAvailableDimensions.Items.Count > 0)
            {
                btnAddDimension.Enabled = true;
            }
            else
            {
                btnAddDimension.Enabled = false;
            }
        }

        private void AddDimensionToXML(ListItemCollection items)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml");
            XmlNode node = doc.SelectSingleNode("//Market/Dimensions/MasterGroup[@ID ='" + ddlMasterGroups.SelectedValue + "']");
            if (node == null)
            {
                XmlNode nodeMasterGroups = doc.SelectSingleNode("//Market/Dimensions");
                XmlNode nodeMasterGroup = doc.CreateNode(XmlNodeType.Element, "MasterGroup", null);
                XmlAttribute attribMasterGroupID = doc.CreateAttribute("ID");
                attribMasterGroupID.Value = ddlMasterGroups.SelectedValue;
                nodeMasterGroup.Attributes.Append(attribMasterGroupID);
                nodeMasterGroups.AppendChild(nodeMasterGroup);
                node = doc.SelectSingleNode("//Market/Dimensions/MasterGroup[@ID ='" + ddlMasterGroups.SelectedValue + "']");
            }
            foreach (ListItem item in items)
            {
                XmlNode nodeNew = doc.CreateNode(XmlNodeType.Element, "Entry", null);
                XmlAttribute attribID = doc.CreateAttribute("ID");
                attribID.Value = item.Value;
                XmlAttribute attribTitle = doc.CreateAttribute("Title");
                attribTitle.Value = item.Text;
                nodeNew.Attributes.Append(attribID);
                nodeNew.Attributes.Append(attribTitle);
                node.AppendChild(nodeNew);
            }
            
            RemoveFile();
            doc.Save("c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml");
        }

        private void RemoveDimensionFromXML(ListItemCollection items)
        {
            RemoveItemsFromXml(
                $"//Market/Dimensions/MasterGroup[@ID ='{ddlMasterGroups.SelectedValue}']", 
                "/Entry[@ID ='{0}']", 
                items);
        }
        
        #endregion
        
        #endregion

        private void RemoveItemsFromXml(string collectionPath, string itemPath, ListItemCollection items)
        {
            var doc = new XmlDocument();
            doc.Load("c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml");
            var node = doc.SelectSingleNode(collectionPath);
            foreach (ListItem item in items)
            {
                var nodeToRemove = doc.SelectSingleNode(string.Format(collectionPath + itemPath, item.Value));
                if (nodeToRemove != null)
                {
                    node.RemoveChild(nodeToRemove);
                }
            }
            RemoveFile();
            doc.Save("c:\\projects\\2010\\KMPS.Projects.Net4.0\\KMPS.MD\\Temp\\" + HttpContext.Current.Session.SessionID + ".xml");
        }
    }
}
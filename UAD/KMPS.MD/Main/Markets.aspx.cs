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
using System.Collections;
using System.Diagnostics;
using System.Text;
using KM.Common;
using KM.Common.Extensions;
using DataFunctions = KMPS.MD.Objects.DataFunctions;
using KMCommonDataFunctions = KM.Common.DataFunctions;

namespace KMPS.MD.Main
{
    public partial class Markets : BrandsPageBase
    {
        private const string ItemTypeProduct = "P";
        private const string NameId = "ID";
        private const string NameDisplayName = "DisplayName";
        private const string NameTitle = "Title";
        private const string NamePubTypeId = "PubTypeId";
        private const string NameEqual = "EQUAL";
        private const string NameGreater = "GREATER";
        private const string NameLesser = "LESSER";
        private const char VBarChar = '|';

        private ArrayList EntryItems = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Markets";
            Master.SubMenu = "Market Creation";
            MarketErrorDiv.Visible = false;
            MarketErrorLabel.Text = string.Empty;
            divError.Visible = false;
            lblErrorMessage.Text = string.Empty;

            if (!IsPostBack)
            {
                gvMarkets.PageSize = 10;

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Market, KMPlatform.Enums.Access.View))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Market, KMPlatform.Enums.Access.Edit))
                {
                    bpAddMarkets.Visible = false;
                }
                
                LoadBrands();
                LoadMarkets();
                LoadPubTypes();
                LoadPubs();

                grdItems.DataSource = null;
                grdItems.DataBind();
            }
        }

        protected void grdItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int newItemID = 0;
            try
            {
                foreach (GridViewRow row in grdItems.Rows)
                {
                    Label lblItemType = (Label)row.FindControl("lblItemType");
                    Label lblGroupID = (Label)row.FindControl("lblGroupID");
                    Label lblGroupTitle = (Label)row.FindControl("lblGroupTitle");
                    Label lblEntryID = (Label)row.FindControl("lblEntryID");
                    Label lblEntryTitle = (Label)row.FindControl("lblEntryTitle");
                    EntryItems.Add(new EntryItem(++newItemID, lblItemType.Text, lblGroupID.Text, lblGroupTitle.Text, lblEntryID.Text, lblEntryTitle.Text));
                }

                int itemID = Convert.ToInt32(grdItems.DataKeys[Convert.ToInt32(e.RowIndex)].Value);
                EntryItem deletedEntryItem = EntryItems.OfType<EntryItem>().Where(x => x.ItemID == itemID).First();
                //DataTable pubsToRemove = DataFunctions.getDataTable("select p.PubId, p.PubName from Pubs p " +
                //                                                    "join pubtypes pt on p.pubtypeid = pt.pubtypeid " +
                //                                                    "where pt.PubTypeDisplayName = '" + deletedEntryItem.GroupTitle + "' " +
                //                                                    "and PubID in (" + deletedEntryItem.EntryID + ")", DataFunctions.GetClientSqlConnection(Master.clientconnections));

                List<Pubs> pubsToRemove =  Pubs.GetAll(Master.clientconnections).FindAll(x=>x.PubTypeDisplayName == deletedEntryItem.GroupTitle && deletedEntryItem.EntryID.Contains(x.PubID.ToString()));

                for (int i = 0; i < EntryItems.Count; i++) 
                {
                    EntryItem entryItem = (EntryItem)EntryItems[i];

                    if (entryItem.ItemID != deletedEntryItem.ItemID)
                    {
                        foreach (GridViewRow row in grdItems.Rows)
                        {
                            Label lblGroupTitle = (Label)row.FindControl("lblGroupTitle");
                            Label lblEntryID = (Label)row.FindControl("lblEntryID");
                            Label lblEntryTitle = (Label)row.FindControl("lblEntryTitle");
                            Label lblItemType = (Label)row.FindControl("lblItemType");

                            if (pubsToRemove != null && pubsToRemove.Count > 0)
                            {
                                List<string> entries = pubsToRemove.Select(x =>x.PubID.ToString()).ToList();
                                List<string> entryTitles = pubsToRemove.Select(x => x.PubName).ToList();

                                List<string> entryItemEntries = entryItem.EntryID.Split(new char[] { ',' }).ToList();
                                List<string> entryItemTitles = entryItem.EntryTitle.Split(new char[] { ',' }).ToList();

                                foreach (string pubIdRemoved in entries)
                                {
                                    entryItemEntries.Remove(pubIdRemoved);
                                    entryItemTitles.Remove((from p in pubsToRemove.AsEnumerable()
                                                            where p.PubID.ToString() == pubIdRemoved
                                                            select p.PubName).First());
                                    lblEntryID.Text = string.Join<string>(",", entryItemEntries);
                                    entryItem.EntryID = lblEntryID.Text;
                                    lblEntryTitle.Text = string.Join<string>(",", entryItemTitles);
                                    entryItem.EntryTitle = lblEntryTitle.Text;
                                }
                            }
                        }
                    }
                }

                EntryItems.Remove(deletedEntryItem);
            }
            catch
            {
            }

            LoadItemGrid();
            lbSelectedPubs.Items.Clear();
            ddlPubTypes.SelectedIndex = 0;
            LoadPubs();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            SetupPage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Market, KMPlatform.Enums.Access.Edit))
            {
                try
                {
                    string pubIds = string.Empty;
                    string MasterIDs = string.Empty;

                    int marketID = Convert.ToInt32(lblMarketID.Text);

                    if (KMPS.MD.Objects.Markets.ExistsByName(Master.clientconnections, marketID, txtMarketName.Text.Trim()))
                    {
                        divError.Visible = true;
                        lblErrorMessage.Text = "The Market Name you entered already exists. Please enter the different name.";
                    }
                    else if (grdItems.Rows.Count <= 0)
                    {
                        divError.Visible = true;
                        lblErrorMessage.Text = "You must select an item before you can save.";
                    }
                    else
                    {
                        Objects.Markets m = new Objects.Markets();
                        m.MarketID = marketID;
                        m.MarketName = txtMarketName.Text.Trim();
                        m.MarketXML = CreateXMLFromGrid().OuterXml;
                        m.BrandID = Convert.ToInt32(hfBrandID.Value);
                        Objects.Markets.Save(Master.clientconnections, m);

                        SetupPage();
                    }
                }
                catch (Exception ex)
                {
                    divError.Visible = true;
                    lblErrorMessage.Text = ex.Message;
                }
            }
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfBrandID.Value = drpBrand.SelectedValue;
            SetupPage();

            if (Convert.ToInt32(drpBrand.SelectedValue) >= 0)
            {
                LoadMarkets();
                LoadPubTypes();
                LoadPubs();
            }
        }
        
        protected void ddlPubTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbSelectedPubs.Items.Clear();
            LoadPubs();
        }

        protected void btnAddPub_Click(object sender, EventArgs e)
        {
            ListItemCollection itemsToRemove = new ListItemCollection();
            foreach (ListItem item in lbAvailablePubs.Items)
            {
                if (item.Selected)
                {
                    lbSelectedPubs.Items.Add(item);
                    itemsToRemove.Add(item);
                }
            }

            foreach (ListItem item in itemsToRemove)
            {
                lbAvailablePubs.Items.Remove(item);

                foreach (GridViewRow row in grdItems.Rows)
                {
                    Label lblItemType = (Label)row.FindControl("lblItemType");
                    Label lblGroupID = (Label)row.FindControl("lblGroupID");
                    Label lblGroupTitle = (Label)row.FindControl("lblGroupTitle");
                    Label lblEntryID = (Label)row.FindControl("lblEntryID");
                    Label lblEntryTitle = (Label)row.FindControl("lblEntryTitle");
                    List<string> entries = lblEntryID.Text.Split(new char[] { ',' }).ToList();
                    List<string> entryTitles = lblEntryTitle.Text.Split(new char[] { ',' }).ToList();

                    if (ddlPubTypes.SelectedValue == lblGroupTitle.Text)
                    {
                        DataTable dt = new DataTable();
                        if (GetMarketType(ddlPubTypes.SelectedValue) == "P")
                             dt = DataFunctions.getDataTable("SELECT PubID, Pubname FROM Pubs WHERE PubId =" + item.Value, DataFunctions.GetClientSqlConnection(Master.clientconnections));
                        else
                            dt = DataFunctions.getDataTable("SELECT MasterID, MasterDesc FROM mastercodesheet WHERE MasterID =" + item.Value, DataFunctions.GetClientSqlConnection(Master.clientconnections));
                      
                        entries.Add(item.Value);
                        entryTitles.Add(dt.Rows[0][1].ToString());
                        lblEntryID.Text = string.Join<string>(",", entries);
                        lblEntryTitle.Text = string.Join<string>(",", entryTitles);
                    }
                }
            }

            if (lbAvailablePubs.Items.Count > 0)
                btnAddPub.Enabled = true;
            else
                btnAddPub.Enabled = false;

            btnRemovePub.Enabled = true;

            UpdateGrid();
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

            foreach (ListItem item in lbToRemove.Items)
            {
                lbSelectedPubs.Items.Remove(item);

                foreach (GridViewRow row in grdItems.Rows)
                {
                    Label lblEntryID = (Label)row.FindControl("lblEntryID");
                    Label lblEntryTitle = (Label)row.FindControl("lblEntryTitle");
                    List<string> entries = lblEntryID.Text.Split(new char[] { ',' }).ToList();
                    List<string> entryTitles = lblEntryTitle.Text.Split(new char[] { ',' }).ToList();

                    if(entries.Contains(item.Value))
                    {
                         DataTable dt = new DataTable();

                         if (GetMarketType(ddlPubTypes.SelectedValue) == "P")
                             dt = DataFunctions.getDataTable("SELECT PubID, Pubname FROM Pubs WHERE PubId =" + item.Value, DataFunctions.GetClientSqlConnection(Master.clientconnections));
                         else
                             dt = DataFunctions.getDataTable("SELECT MasterID, MasterDesc FROM mastercodesheet WHERE MasterID =" + item.Value, DataFunctions.GetClientSqlConnection(Master.clientconnections));

                        int index = entries.IndexOf(item.Value);
                        entries.Remove(item.Value);
                        entryTitles.Remove(dt.Rows[0][1].ToString());
                        lblEntryID.Text = string.Join<string>(",", entries);
                        lblEntryTitle.Text = string.Join<string>(",", entryTitles);
                    }
                }
            }

            if (lbSelectedPubs.Items.Count > 0)
                btnRemovePub.Enabled = true;
            else
                btnRemovePub.Enabled = false;

            btnAddPub.Enabled = true;
            UpdateGrid();
        }

        private string GetMarketType(string id)
        {
            string MarketType = "D";

            PubTypes p = PubTypes.GetByColumnReference(Master.clientconnections, id);

            if (p != null)
            {
                MarketType = "P";
            }

            return MarketType;
        }

        private void SetupPage()
        {
            divError.Visible = false;
            lblErrorMessage.Text = "";
            lblMarketID.Text = "0";
            txtMarketName.Text = "";
            bpAddMarkets.Title = " Add Markets";
            drpAdhocInt.ClearSelection();
            txtAdhocIntFrom.Text = "";
            txtAdhocIntTo.Text = "";
            txtAdhocIntTo.Enabled = true;
            gvMarkets.DataSource = null;
            gvMarkets.DataBind();
            grdItems.DataSource = null;
            grdItems.DataBind();
            lbSelectedPubs.Items.Clear();
            lbAvailablePubs.Items.Clear();

            if (drpBrand.Visible)
            {
                lbAvailablePubs.Items.Clear();
                ddlPubTypes.Items.Clear();
                LoadPubTypes();
                LoadPubs();
                LoadMarkets();
            }
            else
            {
                ddlPubTypes.SelectedIndex = 0;
                LoadPubs();
                LoadMarkets();
            }
        }

        private void LoadItemGridFromDB(XmlDocument doc)
        {
            Guard.NotNull(doc, nameof(doc));
            var itemId = 0;
            var nodeMarket = doc.SelectSingleNode("//Market");

            try
            {
                foreach (XmlNode nodeMarketType in nodeMarket.ChildNodes)
                {
                    var itemType = nodeMarketType.Attributes[NameId].Value;

                    if (itemType == ItemTypeProduct)
                    {
                        LoadMarketTypeProduct(nodeMarketType, itemType, ref itemId);
                    }
                    else
                    {
                        LoadMarketTypeOther(nodeMarketType, itemType, ref itemId);
                    }
                }
            }
            catch (Exception ex)
            {
                // Possible bug: error not handled.
                Debug.WriteLine(ex);
            }

            var nodeFilter = doc.SelectSingleNode("//Market/FilterType[@ID ='A']");
            try
            {
                foreach (XmlNode nodeEntry in nodeFilter.ChildNodes)
                {
                    var strValue = nodeEntry.ChildNodes[0].Attributes[NameId].Value.Split(VBarChar);
                    txtAdhocIntFrom.Text = strValue[0];
                    txtAdhocIntTo.Text = strValue[1];
                    var idAttribute = nodeEntry.ChildNodes[1].Attributes[NameId].Value;
                    txtAdhocIntTo.Enabled = !idAttribute.EqualsAnyIgnoreCase(NameEqual, NameGreater, NameLesser);

                    drpAdhocInt.SelectedIndex = -1;

                    var listItem = drpAdhocInt.Items.FindByValue(nodeEntry.ChildNodes[1].Attributes[NameId].Value);
                    if (listItem != null)
                    {
                        listItem.Selected = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Possible bug: error not handled.
                Debug.WriteLine(ex);
            }

            grdItems.DataSource = EntryItems;
            grdItems.DataBind();
            ddlPubTypes.SelectedIndex = 0;
            lbSelectedPubs.Items.Clear();
            LoadPubs();
        }

        private void LoadMarketTypeProduct(XmlNode nodeMarketType, string itemType, ref int itemId)
        {
            Guard.NotNull(nodeMarketType, nameof(nodeMarketType));

            if (nodeMarketType.HasChildNodes)
            {
                var pubIds = string.Join<string>(
                    ",",
                    nodeMarketType.ChildNodes.OfType<XmlNode>()
                        .Select(x => x.Attributes[NameId].Value)
                        .Distinct());
                var pubTypes = DataFunctions.getDataTable(
                    $"SELECT PubTypeId, PubTypeDisplayName FROM PubTypes WHERE PubTypeId IN (select PubTypeId from Pubs where PubId IN ({pubIds}) GROUP BY PubTypeId)",
                    DataFunctions.GetClientSqlConnection(Master.clientconnections));

                foreach (DataRow pubTypeRecord in pubTypes.Rows)
                {
                    var entryTitle = string.Empty;
                    var groupTitle = pubTypeRecord["PubTypeDisplayName"].ToString();
                    var dataTable = DataFunctions.getDataTable(
                        $"SELECT PubName as 'Title' FROM Pubs where PubID in ({pubIds}) and PubTypeId = '{pubTypeRecord[NamePubTypeId]}' order by PubName",
                        DataFunctions.GetClientSqlConnection(Master.clientconnections));

                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow recordDataRow in dataTable.Rows)
                        {
                            entryTitle += $"{recordDataRow[NameTitle]},";
                        }
                    }

                    if (entryTitle.Length > 0)
                    {
                        entryTitle = entryTitle.Remove(entryTitle.Length - 1, 1);
                    }

                    EntryItems.Add(new EntryItem(++itemId, itemType, nodeMarketType.Attributes[NameId].Value, groupTitle, pubIds, entryTitle));
                }
            }
        }

        private void LoadMarketTypeOther(XmlNode nodeMarketType, string itemType, ref int itemId)
        {
            Guard.NotNull(nodeMarketType, nameof(nodeMarketType));

            foreach (XmlNode nodeGroup in nodeMarketType.ChildNodes)
            {
                var groupId = nodeGroup.Attributes[NameId].Value;
                var entryId = string.Empty;
                var entryTitle = string.Empty;
                var groupTitle = string.Empty;

                var dataTable = DataFunctions.getDataTable(
                    $"SELECT DisplayName FROM MasterGroups where ColumnReference = '{groupId}'",
                    DataFunctions.GetClientSqlConnection(Master.clientconnections));

                if (dataTable.Rows.Count > 0)
                {
                    groupTitle = dataTable.Rows[0][NameDisplayName].ToString();
                }

                foreach (XmlNode nodeEntry in nodeGroup.ChildNodes)
                {
                    entryId += $"{nodeEntry.Attributes[NameId].Value},";
                }

                if (entryId.Length > 0)
                {
                    entryId = entryId.Remove(entryId.Length - 1, 1);

                    var dtMasterCodeSheet = DataFunctions.getDataTable(
                        $"SELECT mastervalue + '. ' + masterDesc as 'Title' FROM Mastercodesheet where MasterID in ({entryId}) order by masterDesc",
                        DataFunctions.GetClientSqlConnection(Master.clientconnections));

                    foreach (DataRow row in dtMasterCodeSheet.Rows)
                    {
                        entryTitle += $"{row[NameTitle]},";
                    }

                    if (entryTitle.Length > 0)
                    {
                        entryTitle = entryTitle.Remove(entryTitle.Length - 1, 1);
                    }

                    EntryItems.Add(new EntryItem(++itemId, itemType, groupId, groupTitle, entryId, entryTitle));
                }
            }
        }

        private XmlDocument CreateXMLFromGrid()
        {
            XmlDocument doc = new XmlDocument();
            string currentXML = @"<Market></Market>";
            doc.LoadXml(currentXML);

            try
            {
                XmlNode node;
                foreach (GridViewRow row in grdItems.Rows)
                {
                    Label lblItemType = (Label)row.FindControl("lblItemType");
                    Label lblGroupID = (Label)row.FindControl("lblGroupID");
                    Label lblGroupTitle = (Label)row.FindControl("lblGroupTitle");
                    Label lblEntryID = (Label)row.FindControl("lblEntryID");
                    Label lblEntryTitle = (Label)row.FindControl("lblEntryTitle");

                    node = doc.SelectSingleNode("//Market/MarketType[@ID ='" + lblItemType.Text + "']");
                    if (node == null)
                    {
                        XmlNode nodeMarket = doc.SelectSingleNode("//Market");
                        XmlNode nodeMarketType = doc.CreateNode(XmlNodeType.Element, "MarketType", null);
                        XmlAttribute attribMarketTypeID = doc.CreateAttribute("ID");
                        attribMarketTypeID.Value = lblItemType.Text;
                        nodeMarketType.Attributes.Append(attribMarketTypeID);
                        nodeMarket.AppendChild(nodeMarketType);
                        node = doc.SelectSingleNode("//Market/MarketType[@ID ='" + lblItemType.Text + "']");
                    }

                    node = lblItemType.Text != "P"
                        ? doc.SelectSingleNode("//Market/MarketType[@ID ='" + lblItemType.Text + "']/Group[@ID ='" + lblGroupID.Text + "']")
                        : doc.SelectSingleNode("//Market/MarketType[@ID ='" + lblItemType.Text + "']");

                    if (node == null)
                    {
                        XmlNode nodeMarketType = doc.SelectSingleNode("//Market/MarketType[@ID ='" + lblItemType.Text + "']");

                        if (lblItemType.Text != "P")
                        {
                            XmlNode nodeGroup = doc.CreateNode(XmlNodeType.Element, "Group", null);
                            XmlAttribute attribGroupID = doc.CreateAttribute("ID");
                            attribGroupID.Value = lblGroupID.Text;
                            nodeGroup.Attributes.Append(attribGroupID);
                            nodeMarketType.AppendChild(nodeGroup);
                        }

                        node = doc.SelectSingleNode("//Market/MarketType/Group[@ID ='" + lblGroupID.Text + "']");
                    }
                    char[] separator = new char[] { ',' };
                    string[] strSplitArr = lblEntryID.Text.Split(separator);
                    foreach (string arrStr in strSplitArr)
                    {
                        if (node.ChildNodes.OfType<XmlNode>().Where(x => x.Attributes["ID"].Value == arrStr).FirstOrDefault() == null)
                        {
                            XmlNode nodeNew = doc.CreateNode(XmlNodeType.Element, "Entry", null);
                            XmlAttribute attribID = doc.CreateAttribute("ID");
                            attribID.Value = arrStr;
                            nodeNew.Attributes.Append(attribID);
                            node.AppendChild(nodeNew);
                        }
                    }
                }
                XmlNode nodeMkt = doc.SelectSingleNode("//Market");
                XmlNode nodeMType = doc.CreateNode(XmlNodeType.Element, "FilterType", null);
                XmlAttribute attribMTypeID = doc.CreateAttribute("ID");
                attribMTypeID.Value = "A";
                nodeMType.Attributes.Append(attribMTypeID);
                nodeMkt.AppendChild(nodeMType);
                node = doc.SelectSingleNode("//Market/FilterType[@ID ='A']");

                if (txtAdhocIntFrom.Text != "" || txtAdhocIntTo.Text != "")
                {
                    XmlNode nodeGrp = null;
                    XmlNode nodeEntry = null;
                    XmlNode nodeText = null;
                    XmlAttribute attID = null;

                    nodeGrp = doc.CreateNode(XmlNodeType.Element, "Group", null);
                    attID = doc.CreateAttribute("ID");
                    attID.Value = "i|[SCORE]";
                    nodeGrp.Attributes.Append(attID);
                    node.AppendChild(nodeGrp);
                    nodeEntry = doc.CreateNode(XmlNodeType.Element, "Entry", null);
                    attID = doc.CreateAttribute("ID");
                    attID.Value = txtAdhocIntFrom.Text + "|" + txtAdhocIntTo.Text;
                    nodeEntry.Attributes.Append(attID);
                    nodeGrp.AppendChild(nodeEntry);
                    nodeText = doc.CreateNode(XmlNodeType.Element, "Entry", null);
                    attID = doc.CreateAttribute("ID");
                    attID.Value = drpAdhocInt.SelectedItem.Value;
                    nodeText.Attributes.Append(attID);
                    nodeGrp.AppendChild(nodeText);
                }
            }
            catch
            {
            }
            return doc;
        }

        private void UpdateGrid()
        {
            int itemID = 0;
            try
            {
                foreach (GridViewRow row in grdItems.Rows)
                {
                    Label lblItemType = (Label)row.FindControl("lblItemType");
                    Label lblGroupID = (Label)row.FindControl("lblGroupID");
                    Label lblGroupTitle = (Label)row.FindControl("lblGroupTitle");
                    Label lblEntryID = (Label)row.FindControl("lblEntryID");
                    Label lblEntryTitle = (Label)row.FindControl("lblEntryTitle");
                    if (lblItemType.Text != GetMarketType(ddlPubTypes.SelectedValue) || lblGroupID.Text != ddlPubTypes.SelectedValue)
                        EntryItems.Add(new EntryItem(++itemID, lblItemType.Text, lblGroupID.Text, lblGroupTitle.Text, lblEntryID.Text, lblEntryTitle.Text));
                }

                bool groupIsAlreadyInGrid = EntryItems.OfType<EntryItem>().Any(x => x.GroupTitle == ddlPubTypes.SelectedValue);

                if (GetMarketType(ddlPubTypes.SelectedValue) != "P" || ! groupIsAlreadyInGrid)
                {
                    string titles = "";
                    string ids = "";
                    grdItems.DataSource = null;
                    foreach (ListItem item in lbSelectedPubs.Items)
                    {
                        titles += item.Text + ",";
                        ids += item.Value + ",";
                    }
                    if (ids.Length > 0)
                        EntryItems.Add(new EntryItem(++itemID, GetMarketType(ddlPubTypes.SelectedValue), ddlPubTypes.SelectedValue, ddlPubTypes.SelectedItem.Text, ids.Remove(ids.Length - 1, 1), titles.Remove(titles.Length - 1, 1)));
                }

                grdItems.DataSource = EntryItems;
                grdItems.DataBind();
            }
            catch (Exception)
            {
            }
        }

        private void LoadMarkets()
        {
            gvMarkets.DataSource = null;

            List<KMPS.MD.Objects.Markets> lst = null;

            if (Convert.ToInt32(hfBrandID.Value) >= 0)
            {
                if (Convert.ToInt32(hfBrandID.Value) > 0)
                    lst = Objects.Markets.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                else
                    lst = Objects.Markets.GetNotInBrand(Master.clientconnections);
            }

            gvMarkets.DataSource = lst;
            gvMarkets.DataBind();

            if (lst != null)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Market, KMPlatform.Enums.Access.Edit))
                {
                    gvMarkets.Columns[1].Visible = false;
                }

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Market, KMPlatform.Enums.Access.Delete))
                {
                    gvMarkets.Columns[2].Visible = false;
                }
            }
        }

        private void LoadItemGrid()
        {
            grdItems.DataSource = EntryItems;
            grdItems.DataBind();
        }

        private void LoadPubTypes()
        {
            ddlPubTypes.Items.Clear();
            if (Convert.ToInt32(hfBrandID.Value) >= 0)
            {
                List<PubTypes> pt = new List<PubTypes>();
                List<MasterGroup> masterGroupList = new List<MasterGroup>();

                if (Convert.ToInt32(hfBrandID.Value) > 0)
                {
                    pt = PubTypes.GetActiveByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                    masterGroupList = MasterGroup.GetActiveByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                }
                else
                {
                    pt = PubTypes.GetActive(Master.clientconnections);
                    masterGroupList = MasterGroup.GetActiveMasterGroupsSorted(Master.clientconnections);
                }

                var pubquery = (from p in pt
                                select new { ID = p.ColumnReference, Title = p.PubTypeDisplayName });

                var mgquery = (from m in masterGroupList select new { ID = m.ColumnReference, Title = m.DisplayName });

                var mergedList = pubquery.Union(mgquery).ToList();

                ddlPubTypes.DataSource = mergedList.ToList();
                ddlPubTypes.DataTextField = "Title";
                ddlPubTypes.DataValueField = "ID";
                ddlPubTypes.DataBind();
            }
        }

        private void LoadPubs()
        {
            lbAvailablePubs.Items.Clear();
            lbSelectedPubs.Items.Clear();
            if (Convert.ToInt32(hfBrandID.Value) >= 0)
            {
                LoadAvailablePubs();
                try
                {
                    foreach (GridViewRow row in grdItems.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            Label lblItemType = (Label)row.FindControl("lblItemType");
                            Label lblGroupID = (Label)row.FindControl("lblGroupID");

                            if (GetMarketType(ddlPubTypes.SelectedValue) == "P"
                                || (lblItemType.Text == GetMarketType(ddlPubTypes.SelectedValue) && lblGroupID.Text == ddlPubTypes.SelectedValue))
                            {
                                Label lblEntryID = (Label)row.FindControl("lblEntryID");
                                char[] separator = new char[] { ',' };
                                string[] strSplitArr = lblEntryID.Text.Split(separator);
                                foreach (string arrStr in strSplitArr)
                                {
                                    foreach (ListItem item in lbAvailablePubs.Items)
                                    {
                                        if (item.Value == arrStr)
                                            lbSelectedPubs.Items.Add(item);

                                    }
                                }
                                if (lbSelectedPubs.Items.Count > 0)
                                    break;
                            }
                        }
                    }

                    if (lbSelectedPubs.Items.Count > 0)
                    {
                        foreach (ListItem item in lbSelectedPubs.Items)
                        {
                            lbAvailablePubs.Items.Remove(item);
                        }
                        btnRemovePub.Enabled = true;
                    }
                    else
                        btnRemovePub.Enabled = false;
                    if (lbAvailablePubs.Items.Count > 0)
                        btnAddPub.Enabled = true;
                    else
                        btnAddPub.Enabled = false;
                }
                catch (Exception)
                {
                }
            }
        }

        private void LoadAvailablePubs()
        {
            lbAvailablePubs.Items.Clear();
            if (GetMarketType(ddlPubTypes.SelectedValue) == "P")
            {
                PubTypes pt = PubTypes.GetActive(Master.clientconnections).Find(x => x.ColumnReference == ddlPubTypes.SelectedValue.ToString());
                List<Pubs> pubs = new List<Pubs>();

                if (Convert.ToInt32(hfBrandID.Value) > 0)
                    pubs = Pubs.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value)).FindAll(x => x.PubTypeID == pt.PubTypeID);
                else
                    pubs = Pubs.GetSearchEnabled(Master.clientconnections).FindAll(x => x.PubTypeID == pt.PubTypeID);

                var pubquery = (from p in pubs
                                select new { ID = p.PubID, Title = p.PubName });

                lbAvailablePubs.DataSource = pubquery.ToList();
            }
            else
            {
                MasterGroup masterGroupList = MasterGroup.GetActiveMasterGroupsSorted(Master.clientconnections).Find(x => x.ColumnReference == ddlPubTypes.SelectedValue.ToString());
                List<MasterCodeSheet> mcs = new List<MasterCodeSheet>();

                if (Convert.ToInt32(hfBrandID.Value) > 0)
                    mcs = MasterCodeSheet.GetSearchEnabledByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value)).FindAll(x => x.MasterGroupID == masterGroupList.MasterGroupID);
                else
                    mcs = MasterCodeSheet.GetSearchEnabled(Master.clientconnections).FindAll(x => x.MasterGroupID == masterGroupList.MasterGroupID);

                var mcsquery = (from m in mcs
                                select new { ID = m.MasterID, Title = m.MasterValue + ". " + m.MasterDesc });

                lbAvailablePubs.DataSource = mcsquery.ToList();
            }

            lbAvailablePubs.DataTextField = "Title";
            lbAvailablePubs.DataValueField = "ID";
            lbAvailablePubs.DataBind();
        }

        protected void drpAdhocInt_selectedindexchanged(object sender, EventArgs e)
        {
            if (drpAdhocInt.SelectedItem.Value.ToUpper() == "EQUAL" || drpAdhocInt.SelectedItem.Value.ToUpper() == "GREATER" || drpAdhocInt.SelectedItem.Value.ToUpper() == "LESSER")
            {
                txtAdhocIntTo.Enabled = false;
                txtAdhocIntTo.Text = "";
            }
            else
            {
                txtAdhocIntTo.Enabled = true;
            }
        }

        protected void gvMarkets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMarkets.PageIndex = e.NewPageIndex;
            LoadMarkets();
        }

        protected void gvMarkets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string errorMessage = string.Empty;

            try
            {
                if (e.CommandName == "DeleteMarket")
                {
                    if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Market, KMPlatform.Enums.Access.Delete))
                    {
                        if (KMCommonDataFunctions.ExecuteReader(
                            "SELECT * FROM PenetrationReports_Markets WHERE MarketID = " + e.CommandArgument.ToString(), 
                            DataFunctions.GetClientSqlConnection(Master.clientconnections)).HasRows)
                            errorMessage = "Used in penetration report. Cannot be deleted.";

                        if (!string.IsNullOrEmpty(errorMessage))
                            throw new Exception(errorMessage);
                        else
                        {
                            Objects.Markets.DeleteCache(Master.clientconnections);
                            KMCommonDataFunctions.ExecuteScalar(
                                "DELETE FROM MasterMarkets where MarketID = " + e.CommandArgument.ToString() + "; DELETE FROM PubMarkets where MarketID = " + e.CommandArgument.ToString() + "; DELETE FROM PenetrationReports_Markets WHERE MarketID = " + e.CommandArgument.ToString() + "; DELETE FROM Markets where MarketID = " + e.CommandArgument.ToString(), 
                                DataFunctions.GetClientSqlConnection(Master.clientconnections)); ;
                            lblMarketID.Text = "0";
                            txtMarketName.Text = "";
                            SetupPage();
                        }
                    }
                }
                else if (e.CommandName == "EditMarket")
                {
                    if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Market, KMPlatform.Enums.Access.Edit))
                    {
                        Objects.Markets m = Objects.Markets.GetByID(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));
                        txtMarketName.Text = m.MarketName;
                        if (drpBrand.Visible)
                            drpBrand.SelectedValue = m.BrandID.ToString() == "" ? "0" : m.BrandID.ToString();

                        hfBrandID.Value = m.BrandID.ToString() == "" ? "0" : m.BrandID.ToString();
                        lblMarketID.Text = e.CommandArgument.ToString();
                        bpAddMarkets.Title = " Edit Market";
                        divError.Visible = false;

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(m.MarketXML);
                        drpAdhocInt.ClearSelection();
                        txtAdhocIntFrom.Text = "";
                        txtAdhocIntTo.Text = "";
                        txtAdhocIntTo.Enabled = true;
                        if (drpBrand.Visible)
                            LoadPubTypes();

                        LoadItemGridFromDB(doc);
                        ddlPubTypes.SelectedIndex = 0;
                        lbSelectedPubs.Items.Clear();
                        LoadPubs();
                    }
                }
            }
            catch (SqlException)
            {
                MarketErrorLabel.Text = "An error occured during market deletion.";
                MarketErrorDiv.Visible = true;
            }
            catch (Exception ex)
            {
                MarketErrorLabel.Text = ex.Message;
                MarketErrorDiv.Visible = true;
            }
        }

        protected override Panel BrandPanel => pnlBrand;
        protected override DropDownList BrandDropDown => drpBrand;
        protected override HiddenField BrandIdHiddenField => hfBrandID;
        protected override Label BrandNameLabel => lblBrandName;

        protected override void LoadPageFilters()
        {
            //Nothing to do here, but had to implement abstract method
        }
    }
}

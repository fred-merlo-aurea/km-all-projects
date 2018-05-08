using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace ecn.communicator.contentmanager.ckeditor.dialog
{
    public partial class transnippets : System.Web.UI.Page
    {
        protected System.Web.UI.HtmlControls.HtmlSelect HDR_FontColor;
        protected System.Web.UI.HtmlControls.HtmlSelect HDR_CellBGColor;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox HDR_Bold;
        protected System.Web.UI.HtmlControls.HtmlSelect ITEM_FontColor;
        protected System.Web.UI.HtmlControls.HtmlSelect ITEM_CellBGColor;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox ITEM_Bold;
        delegate void HidePopup();
        string _groupUDFList = "";
        public string GroupUDFList
        {
            get { return _groupUDFList; }
        }

        protected void imgSelectGroup_Click(object sender, ImageClickEventArgs e)
        {
            hfGroupSelectionMode.Value = "SelectGroup";
            groupExplorer.LoadControl();
            groupExplorer.Visible = true;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("GroupSelected"))
                {
                    int groupID = groupExplorer.selectedGroupID;
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(groupID);
                    if (hfGroupSelectionMode.Value.Equals("SelectGroup"))
                    {
                        lblSelectGroupName.Text = group.GroupName;
                        hfSelectGroupID.Value = groupID.ToString();
                        UDFList.Items.Clear();
                        List<ECN_Framework_Entities.Communicator.DataFieldSets> listDFS = ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByGroupID(groupID);
                        ddlTransaction.DataSource = listDFS;
                        ddlTransaction.DataTextField = "Name";
                        ddlTransaction.DataValueField = "DatafieldSetID";
                        ddlTransaction.DataBind();
                        ddlTransaction.Items.Insert(0, new ListItem("--SELECT--", "0"));
                    }
                    else
                    {

                    }
                    GroupsLookupPopupHide();
                    pnlMain.Update();
                }
            }
            catch { }
            return true;
        }

        private void GroupsLookupPopupHide()
        {
            groupExplorer.HideControl();
            groupExplorer.Visible = false;
            pnlMain.Update();
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            int custID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
            this.groupExplorer.hideGroupsLookupPopup = delGroupsLookupPopup;
            if (KMPlatform.BusinessLogic.Client.HasServiceFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Transnippets))
            {
                if (!Page.IsPostBack)
                {

                }
            }
            else
            {
                Response.Redirect("TransnippetsNoAccess.htm");
            }
        }


        private List<ECN_Framework_Entities.Communicator.GroupDataFields> _groupDataFields = null;
        protected List<ECN_Framework_Entities.Communicator.GroupDataFields> GroupDataFieldsList
        {
            get
            {
                if (_groupDataFields == null)
                {
                    _groupDataFields = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(Convert.ToInt32(hfSelectGroupID.Value.ToString()));
                }
                return (this._groupDataFields);
            }
        }



        protected void ddlTransaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            UDFList.Items.Clear();
            UDFSelectedList.Items.Clear();
            if (ddlTransaction.SelectedIndex > 0)
            {
                List<ECN_Framework_Entities.Communicator.GroupDataFields> lstGDF = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByDataFieldSetID(Convert.ToInt32(hfSelectGroupID.Value.ToString()), Convert.ToInt32(ddlTransaction.SelectedValue.ToString()), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in lstGDF.OrderBy(x => x.ShortName))
                {
                    UDFList.Items.Add(new ListItem(gdf.ShortName, gdf.GroupDataFieldsID.ToString()));
                }
                pnlMain.Update();
            }

        }

        protected void btnLoadOptions_Click(object sender, EventArgs e)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> lstGDF = new List<ECN_Framework_Entities.Communicator.GroupDataFields>();
            string HTML = tranEditor.Text;
            Regex rxGroups = new Regex("<table(.*?)group_id=[\"'](.*?)[\"'](.*?)id=[\"']transnippet_(.*?)[\"'](.*?)transaction=[\"'](.*?)[\"'].*?>.*?</table>", RegexOptions.Singleline);
            Match mcGroups = rxGroups.Match(HTML);
            if (mcGroups != null)
            {
                if (mcGroups.Groups[4] != null && !string.IsNullOrEmpty(mcGroups.Groups[4].Value))
                {
                    transnippetName.Value = mcGroups.Groups[4].Value;
                }
                if (mcGroups.Groups[2] != null && !string.IsNullOrEmpty(mcGroups.Groups[2].Value))
                {
                    string GroupID = mcGroups.Groups[2].Value;
                    hfSelectGroupID.Value = GroupID;
                    try
                    {
                        ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(Convert.ToInt32(GroupID));
                        lblSelectGroupName.Text = g.GroupName;
                    }
                    catch { }
                    List<ECN_Framework_Entities.Communicator.DataFieldSets> listDFS = ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByGroupID(Convert.ToInt32(GroupID));
                    ddlTransaction.DataSource = listDFS;
                    ddlTransaction.DataTextField = "Name";
                    ddlTransaction.DataValueField = "DatafieldSetID";
                    ddlTransaction.DataBind();
                    ddlTransaction.Items.Insert(0, new ListItem("--SELECT--", "0"));
                }
                else
                {
                    if (ddlTransaction.Items.Count == 0)
                    {
                        ddlTransaction.Items.Insert(0, new ListItem("--SELECT--", "0"));
                    }
                }

                if (mcGroups.Groups[6] != null && !string.IsNullOrEmpty(mcGroups.Groups[6].Value))
                {
                    string TranID = mcGroups.Groups[6].Value;
                    ddlTransaction.SelectedValue = TranID;

                    Regex rxUDFs = new Regex("<tr.*?id=[\"']detail[\"'].*?>(.*?)</tr>", RegexOptions.Singleline);
                    Match mUDF = rxUDFs.Match(HTML);
                    List<string> UDFs = new List<string>();
                    if (mUDF != null && mUDF.Groups[1] != null)
                    {
                        Regex splitUDF = new Regex("##(.*?)##");
                        MatchCollection mcUDFs = splitUDF.Matches(mUDF.Groups[1].Value);

                        foreach (Match m in mcUDFs)
                        {
                            if (!UDFs.Contains(m.Groups[1].Value))
                            {
                                UDFs.Add(m.Groups[1].Value);
                            }
                        }

                    }

                    lstGDF = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByDataFieldSetID(Convert.ToInt32(hfSelectGroupID.Value.ToString()), Convert.ToInt32(ddlTransaction.SelectedValue.ToString()), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in lstGDF.OrderBy(x => x.ShortName))
                    {
                        if (!UDFs.Contains(gdf.ShortName))
                        {
                            UDFList.Items.Add(new ListItem(gdf.ShortName, gdf.GroupDataFieldsID.ToString()));

                        }
                        else
                        {
                            UDFSelectedList.Items.Add(new ListItem(gdf.ShortName, gdf.GroupDataFieldsID.ToString()));
                            sortField.Items.Add(new ListItem(gdf.ShortName, gdf.GroupDataFieldsID.ToString()));
                            filterFields.Items.Add(new ListItem(gdf.ShortName, gdf.GroupDataFieldsID.ToString()));
                        }
                    }

                }
                sortField.Items.Insert(0, new ListItem("--SELECT--", "0"));
                filterFields.Items.Insert(0, new ListItem("--SELECT--", "0"));

                if (mcGroups.Groups[5] != null && !string.IsNullOrEmpty(mcGroups.Groups[5].Value))
                {
                    Regex rxSort = new Regex(".*?sort=[\"'](.*?)[\"'].*?");
                    Match sort = rxSort.Match(mcGroups.Groups[5].Value);

                    if (sort != null && sort.Groups[1] != null && !string.IsNullOrEmpty(sort.Groups[1].Value))
                        sortField.SelectedValue = lstGDF.First(x => x.ShortName == sort.Groups[1].Value).GroupDataFieldsID.ToString();


                    Regex rxFilterField = new Regex(".*?filter_field=[\"'](.*?)[\"'].*?");
                    Match FilterF = rxFilterField.Match(mcGroups.Groups[1].Value);

                    if (FilterF != null && FilterF.Groups[1] != null && !string.IsNullOrEmpty(FilterF.Groups[1].Value))
                    {
                        filterFields.SelectedValue = lstGDF.First(x => x.ShortName == FilterF.Groups[1].Value).GroupDataFieldsID.ToString();
                    }

                    Regex rxFilterValue = new Regex(".*?filter_value=[\"'](.*?)[\"'].*?");
                    Match FilterV = rxFilterValue.Match(mcGroups.Groups[1].Value);
                    if (FilterV != null && FilterV.Groups[1] != null && !string.IsNullOrEmpty(FilterV.Groups[1].Value))
                    {
                        filterValue.Text = FilterV.Groups[1].Value;
                    }
                }
            }
            else
            {
                //not enough data in transnippet code
                //adding this in so we don't end up in a loop
                ddlTransaction.Items.Insert(0, new ListItem("--SELECT--", "0"));
            }
            pnlMain.Update();
        }
    }
}
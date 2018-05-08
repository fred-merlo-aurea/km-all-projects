using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ecn.communicator.main.ECNWizard.Group
{
    public partial class filtergrid : System.Web.UI.UserControl
    {
        private static bool FilterEditVisible = false;
        public event EventHandler EmojiEvent;
        public string IsTestBlast
        {
            get
            {
                if (ViewState["IsTestBlast" + this.ClientID] != null)
                    return ViewState["IsTestBlast" + this.ClientID].ToString();
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["IsTestBlast" + this.ClientID] = value;
            }
        }

        private static int _RowIndex;

        public int RowIndex
        {
            get
            {
                if (_RowIndex != null)
                {
                    return _RowIndex;
                }
                else
                    return -1;
            }
            set
            {
                _RowIndex = value;
            }
        }

        private static int _GroupID;

        public int GroupID
        {
            get
            {
                if (_GroupID != null)
                    return _GroupID;
                else
                    return -1;
            }
            set
            {
                _GroupID = value;
            }
        }

        private static bool _hideEdit;
        public bool HideEdit
        {
            get
            {
                return _hideEdit;
            }
            set
            {
                _hideEdit = value;
            }
        }

        public string SuppressOrSelect
        {
            get
            {
                if (ViewState["SuppressOrSelect" + this.ClientID] != null)
                    return ViewState["SuppressOrSelect" + this.ClientID].ToString();
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["SuppressOrSelect" + this.ClientID] = value;
            }
        }

        private static List<ECN_Framework_Entities.Communicator.SmartSegment> _ssList;

        private List<ECN_Framework_Entities.Communicator.SmartSegment> ssList
        {
            get
            {
                if (_ssList != null)
                    return _ssList;
                else
                {
                    _ssList = ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetSmartSegments().Where(x => x.SmartSegmentID != 2).ToList();
                    return _ssList;
                }
            }
        }
        public void SetFilters(GroupObject group, int rowIndex, bool hideEdit = false)
        {

            imgbtnAddFilter.CommandArgument = rowIndex.ToString();
            gvFilters.DataSource = group.filters;
            gvFilters.DataBind();

            HideEdit = hideEdit;
        }

        public GroupObject GetFilters
        {
            get
            {

                GroupObject dt = new GroupObject();
                dt.GroupID = GroupID;
                dt.GroupName = string.Empty;
                foreach (GridViewRow gvr in gvFilters.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                        Label filterType = (Label)gvr.FindControl("lblFilterType");
                        Label filterName = (Label)gvr.FindControl("lblFilterName");
                        ImageButton imgbtnEditFilter = (ImageButton)gvr.FindControl("imgbtnEditFilter");

                        if (filterType.Text.ToLower().Equals("smart"))
                        {
                            cibf.SmartSegmentID = ssList.Find(x => x.SmartSegmentName == filterName.Text).SmartSegmentID;
                            GridView gvBlasts = (GridView)gvr.FindControl("gvBlastFilters");
                            string refBlasts = string.Empty;
                            foreach (GridViewRow gvrBlast in gvBlasts.Rows)
                            {
                                if (gvrBlast.RowType == DataControlRowType.DataRow)
                                {
                                    refBlasts += gvBlasts.DataKeys[gvrBlast.RowIndex].Value.ToString() + ",";
                                }
                            }
                            refBlasts = refBlasts.TrimEnd(',');
                            cibf.RefBlastIDs = refBlasts;
                        }
                        else
                        {
                            cibf.FilterID = Convert.ToInt32(imgbtnEditFilter.CommandArgument.ToString());
                        }

                        dt.filters.Add(cibf);

                    }
                }
                return dt;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                if (EmojiEvent != null)
                {
                    EmojiEvent(sender, e);
                }
            }
        }

        private void LoadFilterGrid()
        {
            List<GroupObject> dt = new List<GroupObject>();
            if (SuppressOrSelect.ToLower().Equals("select"))
            {
                dt = (List<GroupObject>)Session["SelectedGroups_List"];
            }
            else if (SuppressOrSelect.ToLower().Equals("suppress"))
            {
                dt = (List<GroupObject>)Session["SupressionGroups_List"];
            }
            else if (SuppressOrSelect.ToLower().Equals("testselect"))
            {
                dt = (List<GroupObject>)Session["SelectedTestGroups_List"];
            }


            GroupObject currentGroup = dt.Find(x => x.GroupID == GroupID);
            if (currentGroup != null)
            {
                gvFilters.DataSource = currentGroup.filters;
                gvFilters.DataBind();
            }
            upMain.Update();
            //populate smart segments in grid
            //List<string> keys = ViewState.Keys.Cast<string>().ToList();
            //foreach (var s in keys)
            //{
            //    if (s.Contains(this.ClientID.ToString() + "RefBlastSelected" + GroupID.ToString()))
            //    {
            //        ECN_Framework_Entities.Communicator.CampaignItemBlastFilter ssFilter = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
            //        ssFilter.SmartSegmentID = ssList.Find(x => x.SmartSegmentName == s.Substring(s.IndexOf("_") + 1)).SmartSegmentID;
            //        ssFilter.RefBlastIDs = ViewState[s].ToString();


            //        selectedFilters.Add(ssFilter);
            //    }
            //    else if (s.Contains(this.ClientID.ToString() + "FilterSelected" + GroupID.ToString()))
            //    {
            //        DataTable dtCustomFilters = (DataTable)ViewState[this.ClientID.ToString() + "FilterSelected" + GroupID.ToString()];
            //        foreach (DataRow dr in dtCustomFilters.Rows)
            //        {
            //            FilterClass custFilter = new FilterClass();
            //            custFilter.SmartSegment = false;
            //            custFilter.Name = dr["FilterName"].ToString();
            //            custFilter.ID = Convert.ToInt32(dr["FilterID"].ToString());

            //            selectedFilters.Add(custFilter);
            //        }
            //    }

            //    gvFilters.DataSource = selectedFilters;
            //    gvFilters.DataBind();
            //}

        }

        protected void gvFilters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter fc = (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter)e.Row.DataItem;
                Label filterType = (Label)e.Row.FindControl("lblFilterType");
                filterType.Text = fc.SmartSegmentID != null ? "Smart" : "Custom";

                ImageButton imgbtnDeleteFilter = (ImageButton)e.Row.FindControl("imgbtnDeleteFilter");
                Label filterName = (Label)e.Row.FindControl("lblFilterName");
                ImageButton imgbtnEditFilter = (ImageButton)e.Row.FindControl("imgbtnEditFilter");
                GridView gvBlasts = (GridView)e.Row.FindControl("gvBlastFilters");
                
                if (fc.SmartSegmentID != null)
                {
                    filterName.Text = ssList.Find(x => x.SmartSegmentID == fc.SmartSegmentID).SmartSegmentName;
                    imgbtnDeleteFilter.CommandName = "deletessfilter";
                    imgbtnDeleteFilter.CommandArgument = fc.SmartSegmentID.ToString() + "_" + GroupID.ToString();

                    string[] blastIDs = fc.RefBlastIDs.Split(',');
                    List<ECN_Framework_Entities.Communicator.BlastAbstract> blastList = new List<ECN_Framework_Entities.Communicator.BlastAbstract>();
                    foreach(string s in blastIDs)
                    {
                        ECN_Framework_Entities.Communicator.BlastAbstract b = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(Convert.ToInt32(s),  false);
                        if (b != null)
                            blastList.Add(b);
                    }
                    
                    
                    var result = (from src in blastList
                                  orderby src.BlastID descending
                                  select new
                                  {
                                      BlastID = src.BlastID,
                                      EmailSubject = "[" + src.BlastID.ToString() + "] " + src.EmailSubject
                                  }).ToList();

                    result = result.Where(x => blastIDs.Contains(x.BlastID.ToString())).ToList();

                    imgbtnEditFilter.Visible = false;
                    gvBlasts.Visible = true;
                    gvBlasts.DataSource = result;
                    gvBlasts.DataBind();
                }
                else if (fc.FilterID != null)
                {
                    ECN_Framework_Entities.Communicator.Filter filt = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(fc.FilterID.Value);
                    filterName.Text = filt.FilterName;
                    imgbtnDeleteFilter.CommandName = "deletecustomfilter";
                    imgbtnDeleteFilter.CommandArgument = fc.FilterID.ToString() + "_" + GroupID.ToString();

                    imgbtnEditFilter.Visible = KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFilter, KMPlatform.Enums.Access.Edit);
                    imgbtnEditFilter.CommandArgument = fc.FilterID.ToString();
                    gvBlasts.Visible = false;

                    if (HideEdit)
                    {
                        filterType.Visible = false;
                        imgbtnEditFilter.Visible = false;
                    }
                }
            }
        }

        

        protected void gvFilters_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("editcustomfilter"))
            {
                int filterID = Convert.ToInt32(e.CommandArgument.ToString());
                //filterEdit1.selectedFilterID = filterID;
                //filterEdit1.selectedGroupID = GroupID;
                //filterEdit1.loadData();
                //modalPopupFilterEdit.Show();
            }

            if (EmojiEvent != null)
            {
                EmojiEvent(sender, e);
            }

        }

    }

}
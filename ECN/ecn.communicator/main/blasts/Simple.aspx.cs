using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using WebUiImage = System.Web.UI.WebControls.Image;

using KM.Common;

namespace ecn.communicator.blastsmanager
{
    public partial class Simple : System.Web.UI.Page
    {
        private const string ControlSocialChart = "SocialChart";
        private const string ControlImageNoResults = "imgNoResults";
        private const string AxisImpressions = "Impressions";
        private const string AxisClicks = "Clicks";
        private const string AxisLikes = "Likes";
        private const string AxisShares = "Shares";
        private const string AxisComments = "Comments";
        private int _BlastID = 0;
        private int _CampaignItemID = 0;
        private int _CampaignItemSocialMediaID = 0;
        private static List<SocialActivity> listSocialActivity;
        #region Get Request Variables
        private void GetValuesFromQuerystring(string queryString)
        {
            KM.Common.QueryString qs = KM.Common.QueryString.GetECNParameters(queryString);
            try
            {
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.BlastID).ParameterValue, out _BlastID);
            }
            catch (Exception)
            {
            }
            try
            {
                int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.CampaignItemID).ParameterValue, out _CampaignItemID);
            }
            catch (Exception)
            {
            }
            int.TryParse(qs.ParameterList.Single(x => x.Parameter == KM.Common.ECNParameterTypes.CampaignItemSocialMediaID).ParameterValue, out _CampaignItemSocialMediaID);

            if(_CampaignItemID == 0 && _BlastID > 0)
            {
                try
                {
                    ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID(_BlastID, Master.UserSession.CurrentUser, false);
                    _CampaignItemID = ci.CampaignItemID;
                }
                catch { }
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS;
            Master.SubMenu = "";
            Master.Heading = "Social Reporting";
            Master.HelpContent = "<p><b>Social</b><br />Lists activity around Simple Sharing.";
            Master.HelpTitle = "Blast Manager";

            KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
            string querystring = System.Web.HttpUtility.UrlDecode(Request.Url.Query.Substring(1, Request.Url.Query.Length - 1));
            string Decrypted = KM.Common.Encryption.Decrypt(querystring, ec);
            if (Decrypted != string.Empty)
            {
                GetValuesFromQuerystring(Decrypted);
            }

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "blastpriv") || KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "viewreport") || KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReport, KMPlatform.Enums.Access.View))				
            {
                if (!(Page.IsPostBack))
                {
                    //get all CampaignItemSocialMedia objects for blast or campaign item
                    //Load the dropdown with all CampaignItemSocialMedia of type facebook
                    //if _CampaignItemSocialMediaID is of Facebook type, select it in the dropdown, if not and there is another one for facebook, select it.
                    //if there are no CampaignItemSocialMedia of type facebook for the blast or campaign item show "no data" image
                    //Load the graph based on _CampaignItemSocialMediaID
                    listSocialActivity = new List<SocialActivity>();
                    GetData();

                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        private void GetData()
        {
            
            ECN_Framework_Entities.Communicator.CampaignItemSocialMedia cism = ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia.GetByCampaignItemSocialMediaID(_CampaignItemSocialMediaID);
            if (cism != null)
            {
                List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> listCISM = ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia.GetByCampaignItemID(_CampaignItemID).Where(x => x.SocialMediaID == 1 && x.SimpleShareDetailID != null && x.Status.ToLower() == "sent").ToList();
                if (listSocialActivity == null)
                    listSocialActivity = new List<SocialActivity>();
                foreach (ECN_Framework_Entities.Communicator.CampaignItemSocialMedia currentCISM in listCISM)
                {
                    ECN_Framework_Entities.Communicator.SocialMediaAuth sma = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.GetBySocialMediaAuthID(currentCISM.SocialMediaAuthID.Value);
                    switch (currentCISM.SocialMediaID)
                    {
                        case 1:
                            if (sma != null)
                            {
                                try
                                {
                                    List<ECN_Framework_Common.Objects.SocialMediaHelper.FBAccount> fbAccounts = ECN_Framework_Common.Objects.SocialMediaHelper.GetUserAccounts(sma.Access_Token);

                                    Dictionary<string, int> FBPostResults = ECN_Framework_Common.Objects.SocialMediaHelper.GetFBPostData(currentCISM.PostID, sma.Access_Token);

                                    SocialActivity fbSA = new SocialActivity();
                                    fbSA.UniqueComments = FBPostResults["Unique_Comments"];
                                    fbSA.TotalComments = FBPostResults["Total_Comments"];
                                    fbSA.Shares = FBPostResults[AxisShares];
                                    fbSA.Title = "Facebook - " + fbAccounts.Find(x => x.id == currentCISM.PageID).name;
                                    fbSA.TotalFBLikes = FBPostResults[AxisLikes];
                                    fbSA.TotalFBImpressions = FBPostResults["Total_Impressions"];
                                    fbSA.UniqueFBImpressions = FBPostResults["Unique_Impressions"];
                                    fbSA.TotalClicks = FBPostResults["Total_Clicks"];
                                    fbSA.UniqueClicks = FBPostResults["Unique_Clicks"];
                                    fbSA.SocialMediaID = 1;
                                    listSocialActivity.Add(fbSA);
                                }
                                catch(Exception ex)
                                {
                                    SocialActivity fbSA = new SocialActivity();
                                    fbSA.SocialMediaID = -1;
                                    listSocialActivity.Add(fbSA);
                                }
                            }
                            break;
                        case 2:
                            //twitter
                            break;
                        case 3:
                            //linked in
                            break;
                    }
                }
                DataBindGrid();
            }
        }

        private void DataBindGrid()
        {
            gvCharts.DataSource = listSocialActivity;
            gvCharts.DataBind();
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("reports.aspx?" + (_BlastID > 0 ? "BlastID=" + _BlastID : "CampaignItemID=" + _CampaignItemID));
        }

        public class SocialActivity
        {
            public SocialActivity() { }

            public int SocialMediaID { get; set; }

            public string Title { get; set; }

            public int TotalFBImpressions { get; set; }

            public int UniqueFBImpressions { get; set; }

            public int TotalFBLikes { get; set; }

            public int TotalClicks { get; set; }

            public int UniqueClicks { get; set; }

            public int Shares { get; set; }

            public int UniqueComments { get; set; }

            public int TotalComments { get; set; }


        }

        protected void gvCharts_RowDataBound(object sender, GridViewRowEventArgs args)
        {
            if (args.Row.RowType == DataControlRowType.DataRow)
            {
                var socialActivity = (SocialActivity)args.Row.DataItem;
                var chart = (Chart)args.Row.FindControl(ControlSocialChart);
                var imgNoResults = (WebUiImage)args.Row.FindControl(ControlImageNoResults);
                if (socialActivity.SocialMediaID > 0)
                {
                    InitChartBase(chart, imgNoResults, socialActivity);

                    var unique = CreateUniqueSeries();
                    var total = CreateTotalSerias();

                    chart.Series.Add(unique);
                    chart.Series.Add(total);

                    CreateDatapoints(socialActivity, unique, total);

                    var lTotal = new Legend();
                    var lUnique = new Legend();

                    chart.Legends.Add(lTotal);
                    chart.Legends.Add(lUnique);
                    chart.Legends[0].Enabled = true;
                    chart.Legends[1].Enabled = true;
                    chart.Legends[0].Docking = Docking.Right;
                    chart.Legends[1].Docking = Docking.Right;

                    chart.DataBind();
                }
                else
                {
                    chart.Visible = false;
                    imgNoResults.Visible = true;
                }
            }
        }

        private static void InitChartBase(Chart chart, Control imgNoResults, SocialActivity socialActivity)
        {
            Guard.NotNull(chart, nameof(chart));
            Guard.NotNull(imgNoResults, nameof(imgNoResults));
            Guard.NotNull(socialActivity, nameof(socialActivity));

            chart.Visible = true;
            imgNoResults.Visible = false;
            chart.Height = 400;
            chart.Width = 600;
            chart.Titles.Add(new Title(
                socialActivity.Title, Docking.Top,
                new Font("Times New Roman", 14f, FontStyle.Bold), 
                Color.Black));
            chart.Titles[0].Alignment = ContentAlignment.TopLeft;
            var chartArea = new ChartArea();
            chartArea.Name = "ca1";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IsStartedFromZero = true;
            chartArea.AxisX.LineColor = Color.Gray;
            chartArea.AxisY.LineColor = Color.Gray;
            chart.ChartAreas.Add(chartArea);
        }

        private static Series CreateTotalSerias()
        {
            var total = new Series();
            const string totalLegend = "Total";
            total.LegendText = totalLegend;
            total.Name = totalLegend;
            total.ChartType = SeriesChartType.Bar;
            const string kmBlue = "#045DA4";
            total.Color = System.Drawing.ColorTranslator.FromHtml(kmBlue);
            return total;
        }

        private static Series CreateUniqueSeries()
        {
            var unique = new Series();
            const string uniqueLegend = "Unique";
            unique.LegendText = uniqueLegend;
            unique.Name = uniqueLegend;
            unique.ChartType = SeriesChartType.Bar;
            const string kmOrange = "#F47E1F";
            unique.Color = System.Drawing.ColorTranslator.FromHtml(kmOrange);
            return unique;
        }

        private static void CreateDatapoints(SocialActivity socialActivity, Series unique, Series total)
        {
            CreateUniquePoints(socialActivity, unique);
            CreateTotalPoints(socialActivity, total);
        }

        private static void CreateUniquePoints(SocialActivity socialActivity, Series unique)
        {
            Guard.NotNull(socialActivity, nameof(socialActivity));
            Guard.NotNull(unique, nameof(unique));

            var point = new DataPoint
            {
                AxisLabel = AxisImpressions,
                IsValueShownAsLabel = true,
                ToolTip = socialActivity.UniqueFBImpressions.ToString()
            };
            point.SetValueY(socialActivity.UniqueFBImpressions);
            unique.Points.Add(point);

            point = new DataPoint
            {
                AxisLabel = AxisClicks,
                IsValueShownAsLabel = true,
                ToolTip = socialActivity.UniqueClicks.ToString()
            };
            point.SetValueY(socialActivity.UniqueClicks);
            unique.Points.Add(point);

            point = new DataPoint
            {
                AxisLabel = AxisLikes,
                IsValueShownAsLabel = true,
                ToolTip = socialActivity.TotalFBLikes.ToString()
            };
            point.SetValueY(socialActivity.TotalFBLikes);
            unique.Points.Add(point);

            point = new DataPoint
            {
                AxisLabel = AxisShares
            };
            unique.Points.Add(point);

            point = new DataPoint
            {
                AxisLabel = AxisComments,
                IsValueShownAsLabel = true,
                ToolTip = socialActivity.UniqueComments.ToString()
            };
            point.SetValueY(socialActivity.UniqueComments);
            unique.Points.Add(point);
        }

        private static void CreateTotalPoints(SocialActivity socialActivity, Series total)
        {
            Guard.NotNull(socialActivity, nameof(socialActivity));
            Guard.NotNull(total, nameof(total));

            DataPoint point;
            point = new DataPoint
            {
                AxisLabel = AxisImpressions,
                IsValueShownAsLabel = true,
                ToolTip = socialActivity.TotalFBImpressions.ToString()
            };
            point.SetValueY(socialActivity.TotalFBImpressions);
            total.Points.Add(point);

            point = new DataPoint
            {
                AxisLabel = AxisClicks,
                IsValueShownAsLabel = true,
                ToolTip = socialActivity.TotalClicks.ToString()
            };
            point.SetValueY(socialActivity.TotalClicks);
            total.Points.Add(point);

            point = new DataPoint
            {
                AxisLabel = AxisLikes,
                IsValueShownAsLabel = true,
                ToolTip = socialActivity.TotalFBLikes.ToString()
            };
            point.SetValueY(socialActivity.TotalFBLikes);
            total.Points.Add(point);

            point = new DataPoint
            {
                AxisLabel = AxisShares,
                IsValueShownAsLabel = true,
                ToolTip = socialActivity.Shares.ToString()
            };
            point.SetValueY(socialActivity.Shares);
            total.Points.Add(point);

            point = new DataPoint
            {
                AxisLabel = AxisComments,
                IsValueShownAsLabel = true,
                ToolTip = socialActivity.TotalComments.ToString()
            };
            point.SetValueY(socialActivity.TotalComments);
            total.Points.Add(point);
        }
    }
}
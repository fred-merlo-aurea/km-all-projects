using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using KMPS.MD.Objects;
using System.Web.UI.DataVisualization.Charting;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;
using System.Xml;
using System.Web.Security;
using System.Threading;
using System.Collections.Generic;

namespace KMPS.MD.Main.Widgets
{
    public partial class MarketPenetration : System.Web.UI.UserControl, IWidgetControl
    {
        DashboardSurface surface;

        public Dictionary<int, int> dSummary = new Dictionary<int, int>();
        Filters fc;

        private int CampaignID
        {
            get
            {
                return Convert.ToInt32(ViewState["CampaignID"]);
            }
            set
            {
                ViewState["CampaignID"] = value;
            }
        }

        private int CampaignFilterID
        {
            get
            {
                return Convert.ToInt32(ViewState["CampaignFilterID"]);
            }
            set
            {
                ViewState["CampaignFilterID"] = value;
            }
        }

        #region IWidgetControl Members

        public void Bind(WidgetInstance instance)
        {
            PnlChart.Visible = true;
            PnlSettings.Visible = false;
        }

        public UpdatePanel[] Command(WidgetInstance instance, Kalitte.Dashboard.Framework.WidgetCommandInfo commandData, ref UpdateMode updateMode)
        {
            if (commandData.CommandName.Equals("Settings"))
            {
                if (PnlChart.Visible)
                {
                    PnlSettings.Visible = true;
                    PnlChart.Visible = false;
                }
            }
            return new UpdatePanel[] { UpdatePanelMarketPenetration };
        }

        public void InitControl(Kalitte.Dashboard.Framework.WidgetInitParameters parameters)
        {
            surface = parameters.Surface;
        }

        #endregion

        private ECNSession _usersession = null;

        public ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECNSession.CurrentSession() : _usersession;
            }
        }

        private KMPlatform.Object.ClientConnections _clientconnections = null;
        public KMPlatform.Object.ClientConnections clientconnections
        {
            get
            {
                if (_clientconnections == null)
                {
                    KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(UserSession.ClientID, true);
                    _clientconnections = new KMPlatform.Object.ClientConnections(client);
                    return _clientconnections;
                }
                else
                    return _clientconnections;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                fc = new Filters(clientconnections, UserSession.CurrentUser.UserID);
                if (!IsPostBack)
                {
                    bool reports_available = loadsavedreports();
                    if (reports_available)
                    {
                        Label1.Text = "";
                        loadchart();
                        RadHtml1.Visible = true;
                    }
                    else
                    {
                        Label1.Text = "THERE ARE NO SAVED REPORTS";
                        RadHtml1.Visible = false;
                    }
                }
            }
            catch
            { }

        }

        protected void loadchart()
        {
            DataTable dt = DataFunctions.getDataTable("select MarketID from PenetrationReports_Markets where ReportID='" + drpdownReports.SelectedValue + "'", DataFunctions.GetClientSqlConnection(clientconnections));
            Session["fc"] = null;
            Session["MarketPenetration"] = null;
            LoadGrid(dt);
        }

        protected void lbtnApplyFilters_Click(object sender, EventArgs e)
        {
            loadchart();
            PnlChart.Visible = true;
            PnlSettings.Visible = false;
        }

        protected void reset_chart(Chart chart)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();
            chart.Legends.Clear();
        }

        private void CreateChart(DataTable dt)
        {
            try
            {
                RadHtml1.DataSource = dt;
                RadHtml1.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        private void LoadGrid(DataTable dt1)
        {
            dSummary.Clear();
            try
            {
                GetPenetrationData(dt1);
                if (Session["MarketPenetration"] != null)
                {
                    DataTable dt = (DataTable)Session["MarketPenetration"];
                    var query= (from src in dt.AsEnumerable()
                                group src by  new 
                                    {
                                        Markets = src.Field<string>("Markets")
                                    }
                                into grp1              
                                select new
                                        {
                                            Markets = grp1.Key.Markets,
                                            Counts = grp1.Sum(x => Int32.Parse(x.Field<string>("Counts")))
                                           
                                        }).ToList();

                    DataTable result = new DataTable();
                    result.Columns.Add("Markets", typeof(string));
                    result.Columns.Add("Counts", typeof(int));

                    for (int i = 0; i < query.Count; i++)
                    {
                        result.Rows.Add(query[i].Markets.ToString(), query[i].Counts);
                    }

                    CreateChart(result);

                }
                Label1.Text = "";
            }
            catch (FilterNoRecordsException)
            {
                Label1.Text = "FilterNoRecordsException";
            }
            catch (DuplicateFilterException)
            {
                Label1.Text="Duplicate filter";
            }
            catch (Exception ex)
            {
                  Label1.Text="Error:" + ex.Message;
            }
        }

        private void GetPenetrationData(DataTable dt1)
        {
            if (Session["MarketPenetration"] == null)
            {
                fc.Clear();
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    Filter f = new Filter();
                    //f.FilterName = i.ToString();

                    string selectedvalues = string.Empty;

                    DataTable dtMarkets = DataFunctions.getDataTable("select MarketXML, MarketName from Markets where MarketID = '" + dt1.Rows[i][0].ToString() + "'", DataFunctions.GetClientSqlConnection(clientconnections));
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(dtMarkets.Rows[0]["MarketXML"].ToString());

                    f.FilterName = dtMarkets.Rows[0]["MarketName"].ToString();

                    XmlNode node = doc.SelectSingleNode("//Market/MarketType[@ID ='P']");
                    if (node != null)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            selectedvalues += selectedvalues == string.Empty ? child.Attributes["ID"].Value : "," + child.Attributes["ID"].Value;
                        }
                    }

                    if (selectedvalues != string.Empty)
                    {
                        f.Fields.Add(new Field("Product", selectedvalues, dtMarkets.Rows[0]["MarketName"].ToString(), "", Enums.FiltersType.Product, "Product"));
                    }

                    List<MasterGroup> masterGroups = MasterGroup.GetAll(clientconnections);
                    foreach (MasterGroup mg in masterGroups)
                    {
                        selectedvalues = string.Empty;

                        node = doc.SelectSingleNode("//Market/MarketType[@ID ='D']/Group[@ID = '" + mg.ColumnReference.ToString() + "']");

                        if (node != null)
                        {
                            foreach (XmlNode child in node.ChildNodes)
                            {
                                selectedvalues += selectedvalues == string.Empty ? child.Attributes["ID"].Value : "," + child.Attributes["ID"].Value;
                            }

                            if (selectedvalues.Length > 0)
                                f.Fields.Add(new Field(mg.DisplayName, selectedvalues, mg.DisplayName, "",  Enums.FiltersType.Dimension, mg.ColumnReference));
                        }
                    }

                    node = doc.SelectSingleNode("//Market/FilterType[@ID ='A']");

                    if (node != null)
                    {
                        foreach (XmlNode nodeEntry in node.ChildNodes)
                        {
                            f.Fields.Add(new Field("Adhoc", nodeEntry.ChildNodes[0].Attributes["ID"].Value, "", nodeEntry.ChildNodes[1].Attributes["ID"].Value, Enums.FiltersType.Adhoc, nodeEntry.Attributes["ID"].Value));
                        }
                    }

                    if (f.Fields != null && f.Fields.Count > 0)
                    {
                        f.FilterNo = fc.Count + 1;
                        fc.Add(f);
                    }

                }

                if (fc.Count > 0)
                {
                    Session["fc"] = fc;

                    DataTable dt = fc.GetCrossTabData("Markets");
                    Session["MarketPenetration"] = dt;
                }
            }
        }

        private bool loadsavedreports()
        {
            List<PenetrationReports> lpr = PenetrationReports.GetNotInBrand(clientconnections);
            drpdownReports.DataSource = drpdownReports.DataSource = lpr;
            drpdownReports.DataValueField = "ReportID";
            drpdownReports.DataTextField = "ReportName";
            drpdownReports.DataBind();
            if (lpr.Any())
            {
                drpdownReports.Items[0].Selected = true;
                return true;
            }
            else
                return false;
        }
    }
}
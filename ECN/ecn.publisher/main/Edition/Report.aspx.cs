using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace ecn.publisher.main.Edition
{
    public partial class Report : ECN_Framework_BusinessLayer.Application.WebPageHelper
	{
		protected ActiveUp.WebControls.PagerBuilder pageClickDetails;
		public int VDCurrentPagerIndex
		{
			set 
			{
                ViewState["VDCurrentPagerIndex"] = value;
			}
			get 
			{
                try
                {
                    return (Convert.ToInt32(ViewState["VDCurrentPagerIndex"]));
                }
                catch 
                {
                    return 0;
                }
			}
		}

		private int getEditionID() 
		{
			try 
			{
				return Convert.ToInt32(Request.QueryString["EditionID"].ToString());
			}
			catch
			{
				return 0;
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
                Master.CurrentMenuCode = ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.EDITION;
                Master.SubMenu = "Edition List";
                Master.Heading = "";
                Master.HelpContent = "";
                Master.HelpTitle = "";

				if (!IsPostBack)
				{
					ViewState["CurrentTab"] = "visit";
                    
                    ECN_Framework_Entities.Publisher.Edition ed = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(getEditionID(), Master.UserSession.CurrentUser);
                    ECN_Framework_Entities.Publisher.Publication pb = ECN_Framework_BusinessLayer.Publisher.Publication.GetByPublicationID(ed.PublicationID, Master.UserSession.CurrentUser);

                    if (ed != null)
					{
                        //imgThumbnail.ImageUrl= ConfigurationManager.AppSettings["ImagePath"] + "/ecn.images/customers/" + pb.CustomerID + "/publisher/" + ed.EditionID + "/"  + "150/1.png";
						imgThumbnail.BorderColor=Color.Black;
						imgThumbnail.BorderStyle=BorderStyle.Solid;
						imgThumbnail.BorderWidth = Unit.Pixel(1);

                        lblPublication.Text = pb.PublicationName;
                        lblEdition.Text = ed.EditionName;

                        List<ECN_Framework_Entities.Publisher.EditionActivityLog> eal = ECN_Framework_BusinessLayer.Publisher.EditionActivityLog.GetByEditionID(getEditionID(), Master.UserSession.CurrentUser);

                        List<ECN_Framework_Entities.Communicator.Blast> lblast = new List<ECN_Framework_Entities.Communicator.Blast>();

                        if (eal != null && eal.Count > 0)
                        {
                            foreach (ECN_Framework_Entities.Publisher.EditionActivityLog al in eal)
                            {
                                if (al.BlastID.Value != 0)
                                {
                                    ECN_Framework_Entities.Communicator.Blast b = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(al.BlastID.Value, Master.UserSession.CurrentUser, false);
                                    if(b != null)
                                        lblast.Add(b);
                                }
                            }
                        }

                        var query = (from b in 
                                         lblast select new {b.BlastID, BlastName = "["  + b.BlastID + "]" + b.EmailSubject });

                        drpBlasts.DataSource=query.ToList();
                        drpBlasts.DataBind();

						drpBlasts.Items.Insert(0, new ListItem("----- ALL ------", "-1"));
						drpBlasts.Items.Insert(1, new ListItem("----- Excluding Blasts -----", "0"));
						drpBlasts.ClearSelection();
						drpBlasts.Items.FindByValue("-1").Selected = true;
					}
					else
					{
						Response.Redirect("../Error.aspx",true);
					}
					loadSummary();
					LoadReport();

				}
			}
			catch(ECN_Framework_Common.Objects.SecurityException)
			{
				Response.Redirect("../securityAccessError.aspx",true);
			}

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		
//		this.drpBlasts.SelectedIndexChanged += new System.EventHandler(this.drpBlasts_SelectedIndexChanged);
//		this.tsEdition.SelectedIndexChange += new System.EventHandler(this.tsEdition_SelectedIndexChange);
//		this.PagerVisitPerPage.IndexChanged += new System.EventHandler(this.Pager_IndexChanged);
//		this.PagerVisitDetails.IndexChanged += new System.EventHandler(this.Pager_IndexChanged);
//		this.PagerTopClicks.IndexChanged += new System.EventHandler(this.Pager_IndexChanged);
//		this.pagerClickDetails.IndexChanged += new System.EventHandler(this.Pager_IndexChanged);
//		this.PagerForwards.IndexChanged += new System.EventHandler(this.Pager_IndexChanged);
//		this.PagerSubscribes.IndexChanged += new System.EventHandler(this.Pager_IndexChanged);
//		this.PagerPrintsPerPage.IndexChanged += new System.EventHandler(this.Pager_IndexChanged);
//		this.PagerSearch.IndexChanged += new System.EventHandler(this.Pager_IndexChanged);
//		this.Load += new System.EventHandler(this.Page_Load);

		private void InitializeComponent()
		{   

		}
		#endregion

		private void LoadReport()
		{
			//loadSummary();
			switch (ViewState["CurrentTab"].ToString().ToLower())
			{
				case "visit": 
					Top20Visits();
					VisitPerPage();
					VisitDetails();
					break;
				case "click":
					TopClicks();
					ClickDetails();
					break;
				case "forward":
					ForwardsDetails();
					break;
				case "subscribe":
					SubscribesDetails();
					break;
				case "print":
					PrintsPerPage();
					break;
				case "search":
					SearchDetails();
					break;
			}
			hide();
		}

		private void hide()
		{
			string tab = ViewState["CurrentTab"].ToString().ToLower();
			phVisits.Visible=false;
			phClicks.Visible=false;
			phForwards.Visible=false;
			phSubscribes.Visible=false;
			phPrints.Visible=false;
			phSearch.Visible=false;

			lbVisits.CssClass="";
			lbClicks.CssClass="";
			lbForwards.CssClass="";
			lbSubscribes.CssClass="";
			lbPrints.CssClass="";
			lbSearch.CssClass="";

			if (tab=="visit")
			{
				phVisits.Visible=true;
				lbVisits.CssClass = "selected";
			}
			else if (tab=="click")
			{
				phClicks.Visible=true;
				lbClicks.CssClass = "selected";
			}
			else if (tab=="forward")
			{
				phForwards.Visible=true;
				lbForwards.CssClass = "selected";
			}
			else if (tab=="subscribe")
			{
				phSubscribes.Visible=true;
				lbSubscribes.CssClass = "selected";
			}
			else if (tab=="print")
			{
				phPrints.Visible=true;
				lbPrints.CssClass = "selected";
			}
			else if (tab=="search")
			{
				phSearch.Visible=true;
				lbSearch.CssClass = "selected";
			}
		}
	
		private void loadSummary()
		{
            List<ECN_Framework_Entities.Publisher.EditionActivityLog> eal = ECN_Framework_BusinessLayer.Publisher.EditionActivityLog.GetByEditionID(getEditionID(), Master.UserSession.CurrentUser);

            //var query = (from e in eal
            //             where e.SessionID != null
            //             group e by e.ActionDate into g
            //             select new { minDate = g.Min(a => a.ActionDate),                                      
            //                 maxDate = g.Max(a => a.ActionDate)
            //             })
            //             .Select(g=>new {seconds = g.minDate-g.minDate })
            //             ;


            //lblAverageTime.Text = query.ToString();

            //lblAverageTime.Text = DataFunctions.ExecuteScalar("select convert(decimal(18,2),round(avg(inn.seconds)/60.00,2)) from (select SessionID, datediff(ss, Min(actiondate), Max(actiondate)) as seconds from editionactivitylog where editionID = " + getEditionID() + " and Isnull(SessionID, '') <> '' group by SessionID) inn").ToString();


            ReportGrid.DataSource = ECN_Framework_BusinessLayer.Publisher.Report.ActivitySummary.GetList(getEditionID(), drpBlasts.SelectedValue == string.Empty ? -1 : Convert.ToInt32(drpBlasts.SelectedValue));
			ReportGrid.DataBind(); 
		}

		#region Load Visits

		private void Top20Visits()
		{
            dgTopVisit.DataSource = ECN_Framework_BusinessLayer.Publisher.Report.ActivityVisitTop20.GetList(getEditionID(), drpBlasts.SelectedValue == string.Empty ? -1 : Convert.ToInt32(drpBlasts.SelectedValue));
			dgTopVisit.DataBind();
		}

		private void VisitPerPage()
		{
            List<ECN_Framework_Entities.Publisher.Report.ActivityVisitPerPage> aVisit = ECN_Framework_BusinessLayer.Publisher.Report.ActivityVisitPerPage.GetList(getEditionID(), drpBlasts.SelectedValue == string.Empty ? -1 : Convert.ToInt32(drpBlasts.SelectedValue));
            dgVisitPerPage.DataSource = aVisit;
			dgVisitPerPage.DataBind();

            PagerVisitPerPage.RecordCount = aVisit.Count;
		}

		private void VisitDetails()
		{
            int recordCount = 0;
            List<ECN_Framework_BusinessLayer.Publisher.Report.ActivityDEReport> aVisit = ECN_Framework_BusinessLayer.Publisher.Report.ActivityDEReport.GetList(getEditionID(), drpBlasts.SelectedValue == string.Empty ? 0 : Convert.ToInt32(drpBlasts.SelectedValue), "visit", VDCurrentPagerIndex, PagerVisitDetails.PageSize, ref recordCount);

            dgVisitDetails.DataSource = aVisit;
            dgVisitDetails.CurrentPageIndex = VDCurrentPagerIndex;
			dgVisitDetails.DataBind();
			PagerVisitDetails.RecordCount = recordCount;
		}

		#endregion

		#region Load Clicks

        protected void drpTopClicks_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            dgTopClicks.CurrentPageIndex = 0;
            PagerTopClicks.CurrentPage = 1;
            PagerTopClicks.CurrentIndex = 0;		
            LoadReport();
        }
        
		private void TopClicks()
		{
            List<ECN_Framework_Entities.Publisher.Report.ActivityTopClicks> aclicks = ECN_Framework_BusinessLayer.Publisher.Report.ActivityTopClicks.GetList(getEditionID(), drpBlasts.SelectedValue == string.Empty ? -1 : Convert.ToInt32(drpBlasts.SelectedValue), drpTopClicks.SelectedValue == string.Empty ? 20 : Convert.ToInt32(drpTopClicks.SelectedValue));

            dgTopClicks.DataSource = aclicks;
			dgTopClicks.DataBind();
            PagerTopClicks.RecordCount = aclicks.Count;
		}

		private void ClickDetails()
		{
            List<ECN_Framework_Entities.Publisher.Report.ActivityClicksDetails> aClickDetails = ECN_Framework_BusinessLayer.Publisher.Report.ActivityClicksDetails.GetList(getEditionID(), drpBlasts.SelectedValue == string.Empty ? -1 : Convert.ToInt32(drpBlasts.SelectedValue));

            dgClickDetails.DataSource = aClickDetails;
			dgClickDetails.DataBind();
			pagerClickDetails.RecordCount = aClickDetails.Count;
		}
		#endregion

		#region Load Forwards
		private void ForwardsDetails()
		{
            List<ECN_Framework_Entities.Publisher.Report.ActivityForwardsDetails> aFowardsDetails = ECN_Framework_BusinessLayer.Publisher.Report.ActivityForwardsDetails.GetList(getEditionID(), drpBlasts.SelectedValue == string.Empty ? -1 : Convert.ToInt32(drpBlasts.SelectedValue));

            dgForwards.DataSource = aFowardsDetails;
			dgForwards.DataBind();
            PagerForwards.RecordCount = aFowardsDetails.Count;
		}
		#endregion

		#region Load Subscribes

		private void SubscribesDetails()
		{
            List<ECN_Framework_Entities.Publisher.Report.ActivitySubscribesDetails> SubscribesDetails = ECN_Framework_BusinessLayer.Publisher.Report.ActivitySubscribesDetails.GetList(getEditionID(), drpBlasts.SelectedValue == string.Empty ? -1 : Convert.ToInt32(drpBlasts.SelectedValue));

            dgSubscribes.DataSource = SubscribesDetails;
			dgSubscribes.DataBind();
            PagerSubscribes.RecordCount = SubscribesDetails.Count;
		}

		#endregion

		#region Load Prints
		private void PrintsPerPage()
		{
            List<ECN_Framework_Entities.Publisher.Report.ActivityPrintsPerPage> aPrintsPerPage = ECN_Framework_BusinessLayer.Publisher.Report.ActivityPrintsPerPage.GetList(getEditionID(), drpBlasts.SelectedValue == string.Empty ? -1 : Convert.ToInt32(drpBlasts.SelectedValue));

            dgPrintsPerPage.DataSource = aPrintsPerPage;
			dgPrintsPerPage.DataBind();
            PagerPrintsPerPage.RecordCount = aPrintsPerPage.Count;
		}

		#endregion

		#region Load Search
		private void SearchDetails()
		{
            List<ECN_Framework_Entities.Publisher.Report.ActivitySearchDetails> aSearchDetails = ECN_Framework_BusinessLayer.Publisher.Report.ActivitySearchDetails.GetList(getEditionID(), drpBlasts.SelectedValue == string.Empty ? -1 : Convert.ToInt32(drpBlasts.SelectedValue));

            dgSearch.DataSource = aSearchDetails;
			dgSearch.DataBind();
            PagerSearch.RecordCount = aSearchDetails.Count;
		}

		#endregion

		protected void drpBlasts_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			resetAllPager();
            loadSummary();
		}

		protected void PagerVisitDetails_IndexChanged(object sender, EventArgs e) 
		{
            VDCurrentPagerIndex = PagerVisitDetails.CurrentPage-1;
			LoadReport();	
		}

		protected void Pager_IndexChanged(object sender, System.EventArgs e)
		{
			LoadReport();
		}

		private void resetAllPager()
		{
			dgTopVisit.CurrentPageIndex = 0; 
			
			dgVisitPerPage.CurrentPageIndex = 0; 
			PagerVisitPerPage.CurrentPage = 1;
			PagerVisitPerPage.CurrentIndex = 0;			

			dgVisitDetails.CurrentPageIndex = 0; 
			PagerVisitDetails.CurrentPage = 1;
			PagerVisitDetails.CurrentIndex = 0;			

			dgTopClicks.CurrentPageIndex = 0; 
			PagerTopClicks.CurrentPage = 1;
			PagerTopClicks.CurrentIndex = 0;			

			dgClickDetails.CurrentPageIndex = 0; 
			pagerClickDetails.CurrentPage = 1;
			pagerClickDetails.CurrentIndex = 0;			

			dgForwards.CurrentPageIndex = 0; 
			PagerForwards.CurrentPage = 1;
			PagerForwards.CurrentIndex = 0;			

			dgSubscribes.CurrentPageIndex = 0; 
			PagerSubscribes.CurrentPage = 1;
			PagerSubscribes.CurrentIndex = 0;			

			dgPrintsPerPage.CurrentPageIndex = 0; 
			PagerPrintsPerPage.CurrentPage = 1;
			PagerPrintsPerPage.CurrentIndex = 0;
	
			dgSearch.CurrentPageIndex = 0; 
			PagerSearch.CurrentPage = 1;
			PagerSearch.CurrentIndex = 0;	

			LoadReport();
		}


		#region Tab Clicks
		protected void lbVisits_Click(object sender, System.EventArgs e)
		{
			ViewState["CurrentTab"]= "visit";
			LoadReport();
		}

		protected void lbClicks_Click(object sender, System.EventArgs e)
		{
			ViewState["CurrentTab"]= "click";
			LoadReport();

		}

		protected void lbForwards_Click(object sender, System.EventArgs e)
		{
			ViewState["CurrentTab"]= "forward";
			LoadReport();

		}

		protected void lbSubscribes_Click(object sender, System.EventArgs e)
		{
			ViewState["CurrentTab"]= "subscribe";
			LoadReport();

		}

		protected void lbPrints_Click(object sender, System.EventArgs e)
		{
			ViewState["CurrentTab"]= "print";
			LoadReport();

		}

		protected void lbSearch_Click(object sender, System.EventArgs e)
		{
			ViewState["CurrentTab"]= "search";
			LoadReport();
		}

        protected void Clickdownload(object sender, CommandEventArgs e)
        {
            //ViewState["CurrentTab"] = "click";
            //LoadReport();
            ArrayList columnHeadings = new ArrayList();

            string newline = "";
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentCustomer.CustomerID.ToString() + "/downloads/");

            string downloadType = ".xls";

            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

            DateTime date = DateTime.Now;
            String tfile = getEditionID().ToString() + "_" + e.CommandName + "_clicks" + downloadType;
            string outfileName = txtoutFilePath + tfile;


            if (File.Exists(outfileName))
            {
                File.Delete(outfileName);
            }

            TextWriter txtfile = File.AppendText(outfileName);

            List<ECN_Framework_Entities.Publisher.Report.ActivityTopClicksDownload> atcDetails = ECN_Framework_BusinessLayer.Publisher.Report.ActivityTopClicksDownload.GetList(getEditionID(), drpBlasts.SelectedValue == string.Empty ? -1 : Convert.ToInt32(drpBlasts.SelectedValue), Convert.ToInt32(e.CommandArgument), drpTopClicks.SelectedValue == string.Empty ? 20 : Convert.ToInt32(drpTopClicks.SelectedValue), e.CommandName);


            Type type = typeof(ECN_Framework_Entities.Publisher.Report.ActivityTopClicksDownload);

            foreach (System.Reflection.PropertyInfo info in type.GetProperties())
            {
                newline += info.Name + "\t";
            }

            txtfile.WriteLine(newline);

            foreach (ECN_Framework_Entities.Publisher.Report.ActivityTopClicksDownload atc in atcDetails)
            {
                newline = "";
                foreach (System.Reflection.PropertyInfo info in type.GetProperties())
                {

                    newline += info.GetValue(atc, null) + "\t";
                } 
                txtfile.WriteLine(newline);

            }

            txtfile.Close();
         
            Response.ContentType = "application/vnd.ms-excel";
            
            Response.AddHeader("content-disposition", "attachment; filename=" + tfile);
            Response.WriteFile(outfileName);
            Response.Flush();
            Response.End();
        }

  
		#endregion
	}
}

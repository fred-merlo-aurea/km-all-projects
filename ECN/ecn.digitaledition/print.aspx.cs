using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using ecn.common.classes;
using Ecn.DigitalEdition.Helpers;
using ecn.publisher.classes;
using pdftron;
using pdftron.PDF;
using pdftron.SDF;

namespace ecn.digitaledition
{
	/// <summary>
	/// Summary description for print.
	/// </summary>
	public partial class print : System.Web.UI.Page
	{
        private string BlastIdQueryStringKey = "b";
        private string EmailIdQueryStringKey = "e";
        private string EditionIdQueryStringKey = "eID";
        private string SessionIdQueryStringKey = "s";
        private HttpRequestBase _request;

        public print()
        {
            _request = new HttpRequestAdapter(Request);
        }

        public print(HttpRequestBase request)
        {
            _request = request;
        }

        private int getBlastID() 
		{
            return QueryStringHelper.GetIntValue(_request, BlastIdQueryStringKey);
		}

		private int getEmailID() 
		{
            return QueryStringHelper.GetIntValue(_request, EmailIdQueryStringKey);
        }

        private int getEditionID() 
		{
            return QueryStringHelper.GetIntValue(_request, EditionIdQueryStringKey);
		}

		private string getSessionID() 
		{
            return QueryStringHelper.GetStringValue(_request, SessionIdQueryStringKey);
		}


		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblMessage.Visible = false;

			if (getEditionID() != 0)
			{
				if (!IsPostBack)
				{

                    DataTable dt = DataFunctions.GetDataTable("select '/ecn.images/customers/' + convert(varchar,c.customerID) + '/publisher/' + convert(varchar,editionID) + '/' as imgpath, Pages, isnull(Isloginrequired,0) as Isloginrequired  from edition e join Publication m on e.PublicationID = m.PublicationID join ecn5_accounts..customer c on m.customerID = c.customerID where e.Status in('Active','Archieve') and m.active=1 and m.IsDeleted=0 and e.IsDeleted=0 and e.editionID=" + getEditionID());

					if (dt.Rows.Count == 1)
					{
                        lbltotalpages.Text = dt.Rows[0]["pages"].ToString();
                        imgPath.Text = dt.Rows[0]["imgpath"].ToString();

                        if (Convert.ToBoolean(dt.Rows[0]["Isloginrequired"]))
                        {
                            rdCurrent.Checked = true;
                            Button1_Click(sender, e);
                        }
                        else
                        {
                            for (int i = Convert.ToInt32(lbltotalpages.Text); i >= 1; i--)
                            {
                                drpStart.Items.Insert(0, new ListItem(i.ToString(), i.ToString()));
                                if (i == 1) drpStart.Items.FindByValue("1").Selected = true;
                                drpEnd.Items.Insert(0, new ListItem(i.ToString(), i.ToString()));
                                if (i == Convert.ToInt32(lbltotalpages.Text)) drpEnd.Items.FindByValue(lbltotalpages.Text).Selected = true;
                            }
                        }
					}
					else
					{
						Response.Redirect("error.aspx");
					}

				}
			}
			else
			{
				Response.Redirect("error.aspx");
			}
		}
        
		protected void Button1_Click(object sender, System.EventArgs e)
        {
            #region get start and end page number
            //Create an instance of DataTable
			DataTable dt = new DataTable();

			DataColumn dcol = new DataColumn("imgPath",typeof(System.String));
			dt.Columns.Add(dcol);
			
			int sNo = 0;
			int eNo = 0;

			if (rdAll.Checked)
			{
				sNo = 1;
				eNo = Convert.ToInt32(lbltotalpages.Text);
			}
			else if (rdCurrent.Checked)
			{
				sNo = Convert.ToInt32(Request.QueryString["sp"]);
				eNo = Convert.ToInt32(Request.QueryString["ep"]);
			}
			else 
			{
				sNo = Convert.ToInt32(drpStart.SelectedValue);
				eNo = Convert.ToInt32(drpEnd.SelectedValue);
            }
            #endregion

            if (sNo <= eNo)
			{
				Edition.CreateActivity(getEditionID(), getEmailID(),  getBlastID(), 0, 0,   "print", string.Format("{0},{1}",sNo, eNo), Request.ServerVariables["REMOTE_ADDR"].ToString(),getSessionID());

                Edition ed = Edition.GetEditionbyID(getEditionID());
                

                string tempFileName = "Print-" + System.Guid.NewGuid().ToString().Substring(0, 5) + ".pdf";

                PDFNet.Initialize(ConfigurationManager.AppSettings["PDFTron_LicenseKey"].ToString());

                PDFNet.SetResourcesPath("resources");

                PDFDoc new_doc = new PDFDoc();
                ArrayList copy_pages = new ArrayList(); 

                // Sample 1 - Delete every second page
                PDFDoc printPDF = new PDFDoc(Server.MapPath(imgPath.Text + ed.FileName));
                printPDF.InitSecurityHandler();

                int page_num = sNo; ;

                PageIterator itr;
                while (page_num <= printPDF.GetPageCount())
                {
                    if (page_num >= Convert.ToInt32(sNo) && page_num <= Convert.ToInt32(eNo))
                    {
                        itr = printPDF.GetPageIterator(page_num);
                        copy_pages.Add(itr.Current());
                    }
                    page_num++;
                }

                ArrayList imported_pages = new_doc.ImportPages(copy_pages);
                for (int i = 0; i != imported_pages.Count; ++i)
                {
                    new_doc.PagePushBack((pdftron.PDF.Page)imported_pages[i]); // Order pages in reverse order. 
                    // Use PagePushBack() if you would like to preserve the same order.
                }

                new_doc.Save(Server.MapPath("temp\\" + tempFileName), SDFDoc.SaveOptions.e_linearized);

                printPDF.Close();
                new_doc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Type", "application/pdf");
                Response.Write("<script>window.print();</script>");
                //Response.AddHeader("Content-Disposition", "attachment;filename=" + tempFileName);
                Response.WriteFile(Server.MapPath("temp\\" + tempFileName));
                Response.Flush();
                Response.Close();
                //System.IO.File.Delete(Server.MapPath("temp\\" + tempFileName));

			}
			else
			{
				lblMessage.Visible = true;
				lblMessage.Text = "Starting page number should be less than or equal to ending page number.!";
			}
		}
	}
}

using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.communicator.classes;
using ecn.common.classes;
using KM.Framework.Web.WebForms.EmailProfile;

namespace ecn.communicator.includes
{
    public partial class emailProfile_EmailActivity : EmailProfileBaseControl
    {
        private string _emailId = string.Empty;
        private string _emailActivity = string.Empty;

        protected override Label lblResultMessage
        {
            get
            {
                return this.MessageLabel;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _emailId = GetFromQueryString("eID", "EmailID specified does not Exist. Please click on the 'Profile' link in the email message that you received");
            _emailActivity = GetFromQueryString("eactivity", "Cannot find Email Activity. Please click on the 'Profile' link in the email message that you received");

            EmailActivityLabel.Text = string.Format("{0} - Email Activity", _emailActivity);

            if (_emailActivity.Equals("opens"))
            {
                LoadEmailActivityOpens(_emailId);
            }
            else if (_emailActivity.Equals("clicks"))
            {
                LoadEmailActivityClicks(_emailId);
            }
        }

        private void LoadEmailActivityOpens(string eID)
        {
            //string emailOpensActivity_sql = "  SELECT " +
            //    " (SELECT COUNT(ActionTypeCode) FROM EmailActivityLog WHERE blastID = eal.blastID AND EmailID = eal.EMailID AND ActionTypeCode='open') " +
            //    " AS opens, eal.BlastID, b.EmailSubject, b.SendTime " +
            //    " FROM Blasts b JOIN EmailActivityLog eal ON b.blastID = eal.blastID JOIN Emails e ON eal.emailID = e.emailID " +
            //    " WHERE " +
            //    " eal.EMailID=" + eID + " AND ActionTypeCode='open' " +
            //    " GROUP BY eal.EmailID, eal.BlastID, b.EmailSubject, b.sendTime ORDER BY b.sendTime ";


            string emailOpensActivity_sql =
                " SELECT (SELECT COUNT(baop.OpenID) FROM BlastActivityOpens WHERE blastID = baop.blastID AND EmailID = baop.EMailID) " +
                " AS opens, baop.BlastID, b.EmailSubject, b.SendTime " +
                " FROM ecn5_communicator..Blasts b JOIN BlastActivityOpens baop ON b.blastID = baop.blastID JOIN ecn5_communicator..Emails e " +
                " ON baop.emailID = e.emailID  WHERE baop.EMailID =  " + eID +
                " GROUP BY baop.EmailID, baop.BlastID, b.EmailSubject, b.sendTime ORDER BY b.sendTime";

            DataTable dt = DataFunctions.GetDataTable(emailOpensActivity_sql);
            if (dt.Rows.Count > 0)
            {
                OpensEmailActivityGrid.DataSource = dt.DefaultView;
                OpensEmailActivityGrid.DataBind();
            }
            else
            {
                OpensEmailActivityGrid.Visible = false;
            }
        }

        private void LoadEmailActivityClicks(string eID)
        {
            //string emailClickssActivity_sql = "  SELECT " +
            //" (SELECT COUNT(ActionTypeCode) FROM EmailActivityLog WHERE blastID = eal.blastID AND EmailID = eal.EMailID AND ActionTypeCode='click' AND " +
            //"	ActionValue = eal.ActionValue) AS clicks, " +
            //" eal.BlastID, eal.ActionValue, b.EmailSubject, eal.ActionDate " +
            //" FROM Blasts b JOIN EmailActivityLog eal ON b.blastID = eal.blastID " +
            //" WHERE " +
            //" eal.EMailID=" + eID + " AND ActionTypeCode='click' " +
            //" GROUP BY eal.EMailID, eal.BlastID, eal.ActionValue, b.EmailSubject, eal.actiondate ORDER BY eal.actiondate ";

            string emailClickssActivity_sql =
                " SELECT " +
                " (SELECT COUNT(bacl.ClickTime) FROM BlastActivityClicks bacl WHERE blastID = bacl.blastID AND EmailID = bacl.EMailID) AS clicks, " +
                " bacl.BlastID, '' as ActionValue, b.EmailSubject, bacl.ClickTime " +
                " FROM ecn5_communicator..Blasts b JOIN BlastActivityClicks bacl ON b.blastID = bacl.blastID " +
                " WHERE bacl.EMailID = " + eID + " GROUP BY bacl.EMailID, bacl.BlastID, b.EmailSubject, bacl.ClickTime ORDER BY bacl.ClickTime";


            DataTable dt = DataFunctions.GetDataTable(emailClickssActivity_sql);
            if (dt.Rows.Count > 0)
            {
                //ClickssEmailActivityGrid.DataSource=dt.DefaultView;
                //ClickssEmailActivityGrid.DataBind();
                DataRow newDR;
                DataTable newDT = new DataTable();
                newDT.Columns.Add(new DataColumn("EmailSubject"));
                newDT.Columns.Add(new DataColumn("ActionValue"));
                newDT.Columns.Add(new DataColumn("ActionDate"));
                newDT.Columns.Add(new DataColumn("Clicks"));
                foreach (DataRow dr in dt.Rows)
                {
                    string clickCount = dr["clicks"].ToString();
                    string fullLink = dr["ActionValue"].ToString();
                    string smallLink = fullLink;
                    try
                    {
                        smallLink = fullLink.Substring(0, 50) + "...";
                    }
                    catch
                    {
                        //ignore
                    }
                    string linkORalias = "";

                    newDR = newDT.NewRow();
                    newDR[0] = dr["EmailSubject"].ToString();
                    string alias = getLinkAlias(Convert.ToInt32(dr["BlastID"].ToString()), fullLink);
                    if (alias.Length > 0)
                    {
                        linkORalias = alias;
                    }
                    else
                    {
                        linkORalias = smallLink;
                    }
                    newDR[1] = "<a href='" + fullLink.ToString() + "' target='_blank'>" + linkORalias.ToString() + "</a>";
                    newDR[2] = dr["ActionDate"].ToString();
                    newDR[3] = dr["clicks"].ToString();

                    newDT.Rows.Add(newDR);
                }

                ClickssEmailActivityGrid.DataSource = new DataView(newDT);
                ClickssEmailActivityGrid.DataBind();
            }
            else
            {
                ClickssEmailActivityGrid.Visible = false;
            }
        }

        private string getLinkAlias(int BlastID, String Link)
        {
            string sqlquery = " SELECT Alias FROM " +
                " Blasts b, Layouts l, Content c, linkAlias la " +
                " WHERE " +
                " b.blastID = " + BlastID + " AND b.layoutID = l.layoutID AND " +
                " (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR " +
                " l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID OR l.ContentSlot7 = c.contentID OR l.ContentSlot8 = c.contentID OR " +
                " l.ContentSlot9 = c.contentID) AND " +
                " la.ContentID = c.ContentID AND la.Link = '" + Link + "'";
            string alias = "";
            try
            {
                alias = DataFunctions.ExecuteScalar(sqlquery).ToString();
            }
            catch (Exception)
            {
                alias = "";
            }

            return alias;
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


        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion
    }
}

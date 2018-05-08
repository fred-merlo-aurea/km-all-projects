using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.admin.landingpages
{
    public partial class BaseChannelMain : System.Web.UI.Page
    {
        private static List<ECN_Framework_Entities.Accounts.LandingPage> LPList = null;
        private static int CurrentLPAID = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BASECHANNEL;
            if (!IsPostBack)
            {
                LPList = ECN_Framework_BusinessLayer.Accounts.LandingPage.GetAll().Where(x => x.BaseChannel == true).ToList();
                gvLandingPage.DataSource = LPList;
                gvLandingPage.DataBind();

            }
        }

        protected void gvLandingPage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string LandingPageURL = "";
            if (e.CommandName.Equals("Edit"))
            {
                switch (e.CommandArgument.ToString())
                {
                    case "1":
                        LandingPageURL = "BaseChannelUnsubscribe.aspx";
                        break;
                    case"2":
                        LandingPageURL = "BaseChannelError.aspx";
                        break;
                    case"3":
                        LandingPageURL = "BaseChannelForwardToFriend.aspx";
                        break;
                    case"4":
                        LandingPageURL = "BaseChannelAbuse.aspx";
                        break;
                    case "5":
                        LandingPageURL = "BaseChannelUpdateEmail.aspx";
                        break;
                }

                Response.Redirect(LandingPageURL);
            }
           
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.LandingPage, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }

        }

       
        private string getPreviewParams(int LPAID)
        {
            string parameters = "";
            //returns BlastID, GroupID, CustomerID and EmailAddress
            DataTable dt = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetPreviewParameters_BaseChannel(LPAID, Master.UserSession.CurrentBaseChannel.BaseChannelID);
            ECN_Framework_Entities.Accounts.LandingPageAssign lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByLPAID(LPAID);
            if (dt.Rows.Count > 0)
            {
               
                int blastID = -1;
                int groupID = -1;
                int customerID = -1;
                string emailAddress = "";
                int emailID = -1;
                switch (lpa.LPID.ToString())
                {
                    case "1":
                        blastID = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                        groupID = Convert.ToInt32(dt.Rows[0].ItemArray[1]);
                        customerID = Convert.ToInt32(dt.Rows[0].ItemArray[2]);
                        emailAddress = dt.Rows[0].ItemArray[3].ToString();
                        parameters = "e=" + emailAddress + "&g=" + groupID + "&b=" + blastID + "&c=" + customerID + "&s=U&f=html&preview=" + LPAID;
                        break;
                    case "2":
                        //parameters for Error page
                        break;
                    case "3":
                        blastID = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                        emailID = Convert.ToInt32(dt.Rows[0].ItemArray[1]);
                        parameters = "e=" + emailID + "&b=" + blastID + "&preview=" + LPAID;
                        break;
                    case "4":
                        //parameters for Abuse page
                        parameters = "p=" + dt.Rows[0][0].ToString() + "&preview=" + LPAID;
                        break;

                }
                
                return parameters;
            }
            else if(lpa.LPID == 5)//this is for the update email address landing page
            {
                parameters = "preview=" + LPAID;
                return parameters;
            }

            return null;
        }

        protected void gvLandingPage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Accounts.LandingPage lp = (ECN_Framework_Entities.Accounts.LandingPage)e.Row.DataItem;
                ECN_Framework_Entities.Accounts.LandingPageAssign lpa = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID, lp.LPID);

                
                HyperLink hlPreview = (HyperLink)e.Row.FindControl("hlBtnPreview");
                if (lpa != null)
                {
                    
                    string PreviewURL = "";
                    string URLParams = "";
                    switch (lp.LPID.ToString())
                    {
                        case "1":
                            URLParams = getPreviewParams(lpa.LPAID);
                            PreviewURL = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/Unsubscribe.aspx?" + URLParams;
                            break;
                        case "3":
                             URLParams = getPreviewParams(lpa.LPAID);
                             PreviewURL = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/emailtofriend.aspx?" + URLParams;
                            break;
                        case "4":
                            URLParams = getPreviewParams(lpa.LPAID);
                            PreviewURL = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/reportSpam.aspx?" + URLParams;
                            break;
                        case "5":
                            URLParams = getPreviewParams(lpa.LPAID);
                            PreviewURL = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/UpdateEmailAddress.aspx?" + URLParams;
                            break;
                    }

                    if (URLParams != null)
                    {
                        hlPreview.Visible = true;
                        hlPreview.NavigateUrl = PreviewURL;
                    }
                    
                }
                else
                {
                    hlPreview.Visible = false;
                }
            }
        }

    }
}
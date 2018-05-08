using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.CommonControls
{
    public partial class ErrorViewer : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ECNError> errorListTmp = new List<ECNError>
            {
                new ECNError(Enums.Entity.Link, Enums.Method.Get, "Error One"),
                new ECNError(Enums.Entity.Link, Enums.Method.Get, "Error Two"),
                new ECNError(Enums.Entity.Link, Enums.Method.Get, "Error Three")
            };
            HttpContext.Current.Session["ErrorViewer.ErrorsList"] = errorListTmp;


            bool hasError = false;
            string errorMsg = string.Empty;
            List<ECNError> errorList = (List<ECNError>)HttpContext.Current.Session["ErrorViewer.ErrorsList"];
            if (errorList.Any())
            {
                BulletRunTime.DataSource = errorList;
                BulletRunTime.DataTextField = "ErrorMessage";
                BulletRunTime.DataValueField = "ErrorMessage";
                BulletRunTime.DataBind();
                lblErrorMessage.Text = errorMsg;
                phError.Visible = true;
            }
            else
            {
                lblErrorMessage.Text = string.Empty;
                phError.Visible = false;
            }
            HttpContext.Current.Session["ErrorViewer.ErrorsList"] = new List<ECNError>();
        }

        public void AddErrorMsg(string newMsg)
        {
            List<ECNError> errorList = (List<ECNError>)HttpContext.Current.Session["ErrorViewer.ErrorsList"];
            if (!errorList.Any()) { errorList = new List<ECNError>(); }
            errorList.Add(new ECNError(Enums.Entity.Link, Enums.Method.Get, newMsg));
        }

        public void ClearErrorMsgList()
        {
            HttpContext.Current.Session["ErrorViewer.ErrorsList"] = new List<ECNError>();
        }

        //private void throwECNException(string message)
        //{
        //    ECNError ecnError = new ECNError(Enums.Entity.Link, Enums.Method.Get, message);
        //    List<ECNError> errorList = new List<ECNError>();
        //    errorList.Add(ecnError);
        //    setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        //}
        //private void setECNError(ECNException ecnException)
        //{
        //    phError.Visible = true;
        //    lblErrorMessage.Text = "";
        //    foreach (ECNError ecnError in ecnException.ErrorList)
        //    {
        //        lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
        //    }
        //}

    }
}
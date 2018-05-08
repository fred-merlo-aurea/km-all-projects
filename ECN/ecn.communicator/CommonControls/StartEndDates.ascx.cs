using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.CommonControls
{
    public partial class StartEndDates : System.Web.UI.UserControl
    {
        public TextBox TbStart;
        public TextBox TbEnd;
        protected bool AllowNullDates = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            btnCheckDates.Style["visibility"] = "hidden";
            if (!IsPostBack)
            {
                DateTime date = DateTime.Now;
                txtEndDate.Text = date.ToShortDateString();
                txtStartDate.Text = date.AddMonths(-1).ToShortDateString();    
            }
        }

        public static class ControlExtension
        {
            public static void RemoveCssClass(HtmlControl control, string cssClassName)
            {
                var val = control.Attributes["class"];
                val = val.Replace(cssClassName, string.Empty);
                control.Attributes["class"] = val;
            }
        }

        protected void btnCheckDates_Click(object sender, EventArgs e)
        {
            const bool bDatesRequired = false;
            const int archiveYears = 2;

            txtStartDate.CssClass = txtEndDate.CssClass = "formfield";
            clearECNError();

            DateTime startDateOut;
            DateTime.TryParse(txtStartDate.Text, out startDateOut);
            DateTime endDateOut;
            DateTime.TryParse(txtEndDate.Text, out endDateOut);

            if (bDatesRequired && (string.IsNullOrWhiteSpace(txtStartDate.Text) || string.IsNullOrWhiteSpace(txtStartDate.Text)))
            {
                throwECNException("Dates are required fields");
                txtStartDate.CssClass = "errorClass";
            }
            else if (!string.IsNullOrWhiteSpace(txtStartDate.Text) && (startDateOut == DateTime.MinValue || !IsValidDate(txtStartDate.Text)))
            {
                throwECNException("Start Date is invalid ");
                txtStartDate.CssClass = "errorClass";
            }
            else if (!string.IsNullOrWhiteSpace(txtEndDate.Text) && (endDateOut == DateTime.MinValue || !IsValidDate(txtEndDate.Text)))
            {
                throwECNException("End Date is invalid ");
                txtEndDate.CssClass = "formfield errorClass";
            }
            else if (!string.IsNullOrWhiteSpace(txtStartDate.Text) && startDateOut < DateTime.Now.AddYears(-archiveYears))
            {
                throwECNException("Start Date is outside of the archive window of " + archiveYears + " year(s)");
                txtStartDate.CssClass = "errorClass";
            }
            else if (!string.IsNullOrWhiteSpace(txtStartDate.Text) && !string.IsNullOrWhiteSpace(txtEndDate.Text) && startDateOut > endDateOut)
            {
                throwECNException("Start Date can not come after the End Date");
                txtStartDate.CssClass = "formfield errorClass";
            }
            else
            {
                txtStartDate.CssClass = txtEndDate.CssClass = "formfield";
                clearECNError();
            }
        }

        protected bool IsValidDate(string date)
        {
            string[] dateParts = date.Split('/');
            if (dateParts[2].Count() == 2 || dateParts[2].Count() == 4)
            {
                bool tmp = Regex.IsMatch(date, @"(?<Month>\d{1,2})/(?<Day>\d{1,2})/(?<Year>(?:\d{4}|\d{2}))");
                return tmp;    
            }
            return false;
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Link, Enums.Method.Get, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        private void setECNError(ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = string.Empty;
            foreach (ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void clearECNError()
        {
            phError.Visible = false;
            lblErrorMessage.Text = string.Empty;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ECN_Framework_Common.Objects;
using System.Web.UI.WebControls;

namespace ecn.communicator.CommonControls
{
    [Serializable]
    public class DateValidator
    {
        public bool ValidateDates(TextBox tbStart, TextBox tbEnd, int archiveYears, PlaceHolder phError, Label lblErrorMessage, bool bDatesRequired = false)
        {
            tbStart.CssClass = tbEnd.CssClass = "formfield";
            clearECNError(phError, lblErrorMessage);

            if (bDatesRequired && (string.IsNullOrWhiteSpace(tbStart.Text) || string.IsNullOrWhiteSpace(tbEnd.Text)))
            {
                throwECNException("Dates are required fields", phError, lblErrorMessage);
                tbStart.CssClass = "errorClass";
                tbEnd.CssClass = "errorClass";
                return false;
            }

            DateTime startDateOut;
            DateTime.TryParse(tbStart.Text, out startDateOut);
            DateTime endDateOut;
            DateTime.TryParse(tbEnd.Text, out endDateOut);

            if (!string.IsNullOrWhiteSpace(tbStart.Text) && (startDateOut == DateTime.MinValue || !IsValidDate(tbStart.Text)))
            {
                throwECNException("Start Date is invalid ", phError, lblErrorMessage);
                tbStart.CssClass = "errorClass";
            }
            else if (!string.IsNullOrWhiteSpace(tbEnd.Text) && (endDateOut == DateTime.MinValue || !IsValidDate(tbEnd.Text)))
            {
                throwECNException("End Date is invalid ", phError, lblErrorMessage);
                tbEnd.CssClass = "formfield errorClass";
            }
            else if (!string.IsNullOrWhiteSpace(tbStart.Text) && startDateOut < DateTime.Now.AddYears(-archiveYears))
            {
                throwECNException("Start Date is outside of the archive window of " + archiveYears + " year(s)", phError, lblErrorMessage);
                tbStart.CssClass = "errorClass";
            }
            else if (!string.IsNullOrWhiteSpace(tbStart.Text) && !string.IsNullOrWhiteSpace(tbEnd.Text) && startDateOut > endDateOut)
            {
                throwECNException("Start Date can not come after the End Date", phError, lblErrorMessage);
                tbStart.CssClass = "formfield errorClass";
            }
            else
            {
                tbStart.CssClass = tbEnd.CssClass = "formfield";
                clearECNError(phError,lblErrorMessage);
                return true;
            }
            return false;
        }

        protected bool IsRecentYear(string year)
        {
            int fourDigitYear = int.Parse(year);
            if (year.Count() == 2)
            {
                fourDigitYear = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar.ToFourDigitYear(Convert.ToInt32(year));    
            }
            int thisYear = DateTime.Now.Year;
            return Math.Abs(fourDigitYear - thisYear) < 500;
        }

        protected bool IsValidDate(string date)
        {
            string[] dateParts = date.Split('/');
            //if ((dateParts[2].Count() == 2 || dateParts[2].Count() == 4) && (IsRecentYear(dateParts[2])))
            if (dateParts[2].Count() == 2 || dateParts[2].Count() == 4)
            {
                bool tmp = Regex.IsMatch(date, @"(?<Month>\d{1,2})/(?<Day>\d{1,2})/(?<Year>(?:\d{4}|\d{2}))");
                return tmp;
            }
            return false;
        }

        private void throwECNException(string message, PlaceHolder phError, Label lblErrorMessage)
        {
            ECNError ecnError = new ECNError(Enums.Entity.DateRange, Enums.Method.Get, message);
            List<ECNError> errorList = new List<ECNError> {ecnError};
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite), phError, lblErrorMessage);
        }

        private void setECNError(ECNException ecnException, PlaceHolder phError, Label lblErrorMessage)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void clearECNError(PlaceHolder phError, Label lblErrorMessage)
        {
            phError.Visible = false;
            lblErrorMessage.Text = string.Empty;
        }
    }
}
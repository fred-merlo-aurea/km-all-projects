using System.Web.UI.WebControls;

namespace ECN_Framework_Common.Objects
{
    /// <summary>
    /// Helper class for EcnError
    /// </summary>
    public static class EcnErrorHelper
    {
        public static void SetEcnError(PlaceHolder placeHolder, Label lblErrorMessage, ECNException ecnException)
        {
            placeHolder.Visible = true;
            lblErrorMessage.Text = string.Empty;
            foreach (var ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = $"{lblErrorMessage.Text}<br/>{ecnError.Entity}: {ecnError.ErrorMessage}";
            }
        }
    }
}

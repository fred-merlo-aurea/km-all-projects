using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KM.Framework.Web.WebForms.EmailProfile
{
    public abstract class EmailProfileBaseControl : UserControl
    {
        /// <summary>
        /// Label control that shows result message or error message on it. This controls must be overrided.
        /// </summary>
        protected abstract Label lblResultMessage { get; }

        /// <summary>
        /// Reads data from QueryString. If specified key does not exist in the QueryString sets the error message on <see cref="lblResultMessage"/>
        /// </summary>
        /// <param name="key">QueryString key</param>
        /// <param name="messageOnError">Error message which is set if key is not exist in QueryString</param>
        /// <returns>Returns string value from QueryString</returns>
        protected virtual string GetFromQueryString(string key, string messageOnError)
        {
            var queryStringValue = string.Empty;

            if (Request.QueryString.AllKeys.Any(k => k == key))
            {
                queryStringValue = Request.QueryString[key].ToString();
            }
            else
            {
                ShowMessageLabel(string.Format("<br>ERROR: {0}", messageOnError));
            }

            return queryStringValue;
        }

        protected void ShowMessageLabel(string message)
        {
            lblResultMessage.Visible = true;
            lblResultMessage.Text = message;
        }
    }
}
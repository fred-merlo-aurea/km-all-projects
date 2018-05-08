using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KMPS_JF_Objects.Controls
{
    public class DecimalTextBox : TextBox
    {
        private string _onblurattributes = string.Empty;

        public string OnBlurAttributes
        {
            get
            {
                return _onblurattributes;
            }
            set { _onblurattributes = value; }
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.AddAttribute("onkeypress", "return checkKeyPressForDecimal(this, event)");
            output.AddAttribute("onblur", "validateDecimal(this);" + OnBlurAttributes);
            output.AddAttribute("decimalSeparator", ".");
            output.AddAttribute("groupSeparator", "");
            output.AddAttribute("decimalDigits", "2");
            output.AddAttribute("style", "text-align:right;padding-right:1px;");
            base.Render(output);
        }
    }
}

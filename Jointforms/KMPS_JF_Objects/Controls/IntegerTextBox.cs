using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KMPS_JF_Objects.Controls
{
    public class IntegerTextBox : TextBox
    {
        private string _onblurattributes = string.Empty;
        private bool _allownegative = false;


        public string OnBlurAttributes
        {
            get
            {
                return _onblurattributes;
            }
            set { _onblurattributes = value; }
        }

        public bool AllowNegative
        {
            get
            {
                return _allownegative;
            }
            set { _allownegative = value; }
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.AddAttribute("onkeypress", "return checkKeyPressForInteger(this, event, " + (_allownegative?"1":"0") + ")");
            output.AddAttribute("onblur", "validateInteger(this);" + OnBlurAttributes);
            output.AddAttribute("style", "text-align:right;padding-right:1px;");
            base.Render(output);
        }
    }
}

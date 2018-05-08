using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMModels.Wrappers;

namespace KMModels.Controls.Standard.Uncommon
{
    public class Password : TextBox
    {
        public Password() { }

        public Password(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Password";
                LabelHTML = "Password";
                DataType = TextboxDataTypes.Text;
                ConfirmPassword = false;
                ConfirmPasswordLabelHTML = "Confirm Password";
            }
        }

        public bool ConfirmPassword { get; set; }

        public string ConfirmPasswordLabelHTML { get; set; }

        public override ControlType Type
        {
            get
            {
                return ControlType.Password;
            }
        }

        public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            base.Fill(control, properties);
            ConfirmPassword = control.GetFormPropertyValue("Confirm Password", properties) == null ? false : control.GetFormPropertyValue("Confirm Password", properties).ToLower() == "true" ? true : false;
            ConfirmPasswordLabelHTML = control.GetFormPropertyValue("Confirm Password LabelHTML", properties) == null ? "Confirm Password" : System.Web.HttpUtility.HtmlDecode(control.GetFormPropertyValue("Confirm Password LabelHTML", properties));
        }
    }
}

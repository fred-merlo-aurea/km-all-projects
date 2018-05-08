using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMModels.Wrappers;

namespace KMModels.Controls.Standard.Common
{
    public class Email : TextBox
    {
        public Email() { }

        public Email(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Email";
                LabelHTML = "Email";
                DataType = TextboxDataTypes.Email;
                AllowChanges = "yes";
            }
        }

        public string AllowChanges { get; set; }

        public override ControlType Type
        {
            get
            {
                return ControlType.Email;
            }
        }

        public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            base.Fill(control, properties);
            AllowChanges = control.GetFormPropertyValue("Allow Changes", properties) == null ? "yes" : control.GetFormPropertyValue("Allow Changes", properties).ToLower();
        }
    }
}

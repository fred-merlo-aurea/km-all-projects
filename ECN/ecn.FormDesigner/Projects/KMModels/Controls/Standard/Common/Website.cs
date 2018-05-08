using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Common
{
    public class Website : TextBox
    {
        public Website() { }

        public Website(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Website";
                LabelHTML = "Website";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Website;
            }
        }
    }
}

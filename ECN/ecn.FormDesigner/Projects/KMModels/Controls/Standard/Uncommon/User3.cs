using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class User3 : TextBox
    {
        public User3() { }

        public User3(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "User3";
                LabelHTML = "User3";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.User3;
            }
        }
    }
}

using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class User1 : TextBox
    {
        public User1() { }

        public User1(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "User1";
                LabelHTML = "User1";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.User1;
            }
        }
    }
}

using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class User2 : TextBox
    {
        public User2() { }

        public User2(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "User2";
                LabelHTML = "User2";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.User2;
            }
        }
    }
}

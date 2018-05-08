using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class User5 : TextBox
    {
        public User5() { }

        public User5(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "User5";
                LabelHTML = "User5";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.User5;
            }
        }
    }
}

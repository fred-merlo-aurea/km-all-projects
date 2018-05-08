using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class Age : TextBox
    {
        public Age() { }

        public Age(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Age";
                LabelHTML = "Age";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Age;
            }
        }
    }
}

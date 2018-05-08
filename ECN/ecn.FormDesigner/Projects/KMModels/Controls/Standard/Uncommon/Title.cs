using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class Title : TextBox
    {
        public Title() { }

        public Title(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Title";
                LabelHTML = "Title";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Title;
            }
        }
    }
}

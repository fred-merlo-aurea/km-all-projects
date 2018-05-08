using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class Occupation : TextBox
    {
        public Occupation() { }

        public Occupation(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Occupation";
                LabelHTML = "Occupation";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Occupation;
            }
        }
    }
}

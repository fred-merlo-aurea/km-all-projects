using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class Notes : TextBox
    {
        public Notes() { }

        public Notes(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Notes";
                LabelHTML = "Notes";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Notes;
            }
        }
    }
}

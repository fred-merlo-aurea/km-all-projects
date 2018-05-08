using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class Birthdate : TextBox
    {
        public Birthdate() { }

        public Birthdate(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Birthdate";
                LabelHTML = "Birthdate";
                DataType = TextboxDataTypes.Date;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Birthdate;
            }
        }
    }
}

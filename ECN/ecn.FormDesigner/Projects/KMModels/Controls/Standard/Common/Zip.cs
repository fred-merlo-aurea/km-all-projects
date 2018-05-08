using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Common
{
    public class Zip : TextBox
    {
        public Zip() { }

        public Zip(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Zip";
                LabelHTML = "Zip";
                DataType = TextboxDataTypes.Zip;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Zip;
            }
        }
    }
}

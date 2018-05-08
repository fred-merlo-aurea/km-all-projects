using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Common
{
    public class City : TextBox
    {
        public City() { }

        public City(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "City";
                LabelHTML = "City";
                DataType = TextboxDataTypes.Text;                
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.City;
            }
        }
    }
}

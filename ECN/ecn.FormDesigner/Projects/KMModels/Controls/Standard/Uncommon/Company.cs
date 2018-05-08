using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class Company : TextBox
    {
        public Company() { }

        public Company(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Company";
                LabelHTML = "Company";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Company;
            }
        }
    }
}

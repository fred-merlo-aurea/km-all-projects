using System;
using System.Collections.Generic;
using KMEnums;
using KMModels.Wrappers;

namespace KMModels.Controls.Standard.Common
{
    public class Country : DropDown 
    {
        public Country()
        {
            Categories = new List<ControlCategory>();
            Items = new List<ListItem>();
        }

        public Country(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Country";
                LabelHTML = "Country";
                Items = new List<ListItem>();
                Categories = new List<ControlCategory>();
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Country;
            }
        }

        //public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        //{
        //    base.Fill(control, properties);
        //    SetPopulationType(control, properties);
        //    DataType = TextboxDataTypes.Text;
        //}
    }
}
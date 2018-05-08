using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMModels.Wrappers;

namespace KMModels.Controls
{
    public class RadioButton : HeadedControl
    {
        public bool IsPrepopulateFromDb { get; set; }

        public RadioButton()
        {
            Categories = new List<ControlCategory>();
            Items = new List<ListItem>();
        }

        public RadioButton(bool setDefaultValues = false) : this()
        {
            if (setDefaultValues) 
            {
                Label = "RadioButton";
                LabelHTML = "RadioButton";
                Items = new List<ListItem>();
                Categories = new List<ControlCategory>();
            }
        }

        public override ControlType Type
        {
            get { return ControlType.RadioButton; }
        }

        public bool Required { get; set; }
        public string Parameter { get; set; }

        public PopulationType PopulationType { get; set; }

        public int Columns { get; set; }

        public IEnumerable<ControlCategory> Categories { get; set; }

        public IEnumerable<ListItem> Items { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            base.Fill(control, properties);
            Required = control.IsRequired(properties);
            SetPopulationType(control, properties);
            string columns_value = control.GetFormPropertyValue(numberofcolumns_property, properties);
            if (columns_value != null)
            {
                Columns = int.Parse(columns_value);
            }
            KMEntities.ControlProperty property_values = properties.GetValuePropertyByControl(control);
            Items = control.GetProperties(property_values).ConvertToModels<ListItem>();

            List<ControlCategory> CCats = new List<ControlCategory>();
            foreach (var cc in control.ControlCategories)
            {
                ControlCategory controlCategory = new ControlCategory();
                controlCategory.CategoryID = cc.ControlCategoryID;
                controlCategory.CategoryName = cc.LabelHTML;
                CCats.Add(controlCategory);
            }
            Categories = CCats;
        }

        protected void SetPopulationType(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            int from = 0;
            string data = control.GetFormPropertyValue(prepopulatefrom_property, properties);
            if (data != null)
            {
                try
                {
                    from = int.Parse(data);
                }
                catch
                { }
            }
            PopulationType = (KMEnums.PopulationType)from;
            if (PopulationType == KMEnums.PopulationType.Querystring)
            {
                Parameter = control.GetFormPropertyValue(querystring_property, properties);
            }
        }
    }
}

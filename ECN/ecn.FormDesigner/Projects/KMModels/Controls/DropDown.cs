using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMModels.Wrappers;

namespace KMModels.Controls
{
    public class DropDown : HeadedControl
    {
        public bool IsPrepopulateFromDb { get; set; }

        public DropDown()
        {
            Categories = new List<ControlCategory>();
            Items = new List<ListItem>();
        }

        public DropDown(bool setDefaultValues = false) : this()
        {
            if (setDefaultValues)
            {
                Label = "DropDown";
                LabelHTML = "DropDown";
                Items = new List<ListItem>();
                Categories = new List<ControlCategory>();
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.DropDown;
            }
        }

        public bool Required { get; set; }

        public string Parameter { get; set; }

        public PopulationType PopulationType { get; set; }

        public IEnumerable<ControlCategory> Categories { get; set; }

        public IEnumerable<ListItem> Items { get; set; }

        public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            base.Fill(control, properties);
            Required = control.IsRequired(properties);
            SetPopulationType(control, properties);
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

using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMModels.Wrappers;

namespace KMModels.Controls
{
    public class ListBox : HeadedControl
    {
        public bool IsPrepopulateFromDb { get; set; }

        public ListBox()
        {
            Categories = new List<ControlCategory>();
            Items = new List<ListItem>();
        }

        public ListBox(bool setDefaultValues = false) : this()
        {
            if (setDefaultValues)
            {
                Label = "ListBox";
                LabelHTML = "ListBox";
                Size = FieldSize.Medium;
                Items = new List<ListItem>();
                Categories = new List<ControlCategory>();
            }           
        }

        public override ControlType Type
        {
            get { return ControlType.ListBox; }
        }

        public bool Required { get; set; }

        public FieldSize Size { get; set; }

        public IEnumerable<ControlCategory> Categories { get; set; }

        public IEnumerable<ListItem> Items { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public PopulationType PopulationType { get; set; }

        public string Parameter { get; set; }


        public string MinMax
        {
            get
            {
                string res = null;
                if (Min > 0 || Max > 0)
                {
                    res = string.Empty;
                    if (Min > 0)
                    {
                        res += Min;
                    }
                    res += ';';
                    if (Max > 0)
                    {
                        res += Max;
                    }
                }

                return res;
            }
        }

        public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            base.Fill(control, properties);
            Required = control.IsRequired(properties);
            string values_allowed = control.GetFormPropertyValue(numberofvaluesallowed_property, properties);
            if (values_allowed != null && values_allowed.Length > 1)
            {
                string[] data = values_allowed.Split(';');
                if (data[0].Length > 0)
                {
                    Min = int.Parse(data[0]);
                }
                if (data[1].Length > 0)
                {
                    Max = int.Parse(data[1]);
                }
                
            }
            SetPopulationType(control, properties);
            Size = (FieldSize)int.Parse(control.GetFormPropertyValue(fieldsize_property, properties));
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

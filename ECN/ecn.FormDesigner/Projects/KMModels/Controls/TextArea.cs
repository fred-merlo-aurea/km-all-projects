using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMModels.Wrappers;

namespace KMModels.Controls
{
    public class TextArea : HeadedControl
    {
        public TextArea() { }

        public TextArea(bool setDefaultValues = false) 
        {
            if (setDefaultValues) 
            {
                Label = "TextArea";
                LabelHTML = "TextArea";
                Size = FieldSize.Medium;
            }
        }

        public override ControlType Type
        {
            get { return ControlType.TextArea; }
        }

        public bool Required { get; set; }

        public FieldSize Size { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

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
            Size = (FieldSize)int.Parse(control.GetFormPropertyValue(fieldsize_property, properties));
            string characters = control.GetFormPropertyValue(characters_property, properties);
            if (characters != null && characters.Length > 1)
            {
                string[] data = characters.Split(';');
                if (data[0].Length > 0)
                {
                    Min = int.Parse(data[0]);
                }
                if (data[1].Length > 0)
                {
                    Max = int.Parse(data[1]);
                }
            }
        }
    }
}

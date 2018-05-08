using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMEnums;
using KMModels.Wrappers;

namespace KMModels.Controls
{
    public class TextBox : HeadedControl
    {
        public TextBox() { }

        public TextBox(bool setDefaultValues = false) 
        {
            if (setDefaultValues)
            {
                Label = "TextBox";
                LabelHTML = "TextBox";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get 
            { 
                return ControlType.TextBox;
            }
        }

        public bool Required { get; set; }

        //public bool Populate { get; set; }

        public PopulationType PopulationType { get; set; }

        public string Parameter { get; set; }

        public TextboxDataTypes DataType { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public string CustomRex { get; set; }

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
            if (!IsStandard || (ControlType)control.ControlType.MainType_ID == ControlType.TextBox)
            {
                SetPopulationType(control, properties);
                DataType = (TextboxDataTypes)int.Parse(control.GetFormPropertyValue(datatype_property, properties));
                if (DataType == TextboxDataTypes.Custom)
                {
                    CustomRex = control.GetFormPropertyValue(regex_property, properties);
                }
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
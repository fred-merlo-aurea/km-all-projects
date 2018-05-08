using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMModels.Wrappers;
using KMEnums;

namespace KMModels.Controls
{
    public class Hidden : HeadedControl
    {
        public Hidden() { }

        public Hidden(bool setDefaultValues = false) 
        {
            if (setDefaultValues)
            {
                Label = "Hidden";
            }
        }

        public override ControlType Type
        {
            get { return ControlType.Hidden; }
        }

        public string Value { get; set; }

        public PopulationType PopulationType { get; set; }

        public string Parameter { get; set; }

        public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            base.Fill(control, properties);
            SetPopulationType(control, properties);
            Value = control.GetFormPropertyValue(value_property, properties);
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
            switch(PopulationType)
            {
                case KMEnums.PopulationType.Querystring:
                    Parameter = control.GetFormPropertyValue(querystring_property, properties);
                    break;
                case KMEnums.PopulationType.Database:
                    PopulationType = KMEnums.PopulationType.None;
                    break;
            }
        }
    }
}

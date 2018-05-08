using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMModels.Wrappers;

namespace KMModels.Controls
{
    public class Literal : HeadedControl
    {
        public Literal() { }

        public Literal(bool setDefaultValues = false) 
        {
            if (setDefaultValues)
            {
                Label = "Literal";
                Text = "<h1>Default</h1>";
            }
        }

        public override ControlType Type
        {
            get { return ControlType.Literal; }
        }

        public string Text { get; set; }

        public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            base.Fill(control, properties);
            Text = control.GetFormPropertyValue(value_property, properties);
        }
    }
}

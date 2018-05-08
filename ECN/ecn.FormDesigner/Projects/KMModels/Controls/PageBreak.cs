using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMModels.Wrappers;

namespace KMModels.Controls
{
    public class PageBreak : Control
    {
        public PageBreak() { }

        public PageBreak(bool setDefaultValues = false) 
        {
            if (setDefaultValues) 
            {
                Previous = "Previous";
                Next = "Next";
            }
        }

        public override ControlType Type
        {
            get { return ControlType.PageBreak; }
        }

        public string PageName { get; set; }

        public string Previous { get; set; }

        public string Next { get; set; }

        public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            base.Fill(control, properties);

            PageName = control.FieldLabel;
            string data = control.GetFormPropertyValue(previousbutton_property, properties);
            if (!string.IsNullOrEmpty(data))
            {
                Previous = data;
            }
            data = control.GetFormPropertyValue(nextbutton_property, properties);
            if (!string.IsNullOrEmpty(data))
            {
                Next = data;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls
{
    public abstract class HeadedControl : Control
    {
        public string Label { get; set; }

        public string LabelHTML { get; set; }

        public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            base.Fill(control, properties);
            Label = control.FieldLabel;
            LabelHTML = System.Web.HttpUtility.HtmlDecode(control.FieldLabelHTML);
        }
    }
}

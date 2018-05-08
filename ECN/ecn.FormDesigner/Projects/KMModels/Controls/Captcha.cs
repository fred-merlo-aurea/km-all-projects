using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls
{
    public class Captcha : HeadedControl
    {
        public override ControlType Type
        {
            get { return ControlType.Captcha; }
        }

        public override void Fill(KMEntities.Control control, IEnumerable<KMEntities.ControlProperty> properties)
        {
            base.Fill(control, properties);
        }
    }
}

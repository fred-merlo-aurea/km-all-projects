using System.Collections.Generic;
using System.Web.UI;

namespace ECN.Communicator.Tests.Main.ECNWizard.Content
{
    public class DLTemplateItem : ITemplate
    {
        public List<Control> controls = new List<Control> { };
        public void InstantiateIn(Control container)
        {
            foreach (Control control in controls)
            {
                container.Controls.Add(control);
            }
        }
    }
}

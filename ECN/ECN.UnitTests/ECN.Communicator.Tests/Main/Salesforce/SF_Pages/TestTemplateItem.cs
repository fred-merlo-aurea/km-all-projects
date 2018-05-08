using System;
using System.Web.UI;

namespace ECN.Communicator.Tests.Main.Salesforce.SF_Pages
{
    public class TestTemplateItem : ITemplate
    {
        public Control control { get; set; }
        public void InstantiateIn(Control container)
        {
            var newControl = Activator.CreateInstance(control.GetType()) as Control;
            newControl.ID = control.ID;
            container.Controls.Add(newControl);
        }
    }
}

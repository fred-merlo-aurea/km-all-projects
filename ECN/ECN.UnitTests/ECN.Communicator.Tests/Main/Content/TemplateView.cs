using System;
using System.Web.UI;

namespace ECN.Communicator.Tests.Main.Content
{
    public class TemplateView : ITemplate
    {
        private readonly Action<Control> _instantiateIn;

        public TemplateView(Action<Control> instantiateIn)
        {
            _instantiateIn = instantiateIn;
        }
        public void InstantiateIn(Control container)
        {
            _instantiateIn(container);
        }
    }
}
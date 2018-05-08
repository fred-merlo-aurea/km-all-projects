using System;

namespace ecn.communicator.main.Salesforce.SF_Pages
{
    public class ItemViewModel
    {
        public ItemViewModel(string ecn, string sf)
        {
            EcnText = ecn;
            SfText = sf;
            var color = ecn.Equals(sf, StringComparison.OrdinalIgnoreCase) ? ColorName.Transparent : ColorName.GreyDark;
            SetColor(color);
            Visible = color != ColorName.Transparent;
        }

        public string EcnText { get; set; }
        public string SfText { get; set; }
        public ColorName EcnColor { get; set; }
        public ColorName SfColor { get; set; }
        public bool Visible { get; set; }

        public void SetColor(ColorName color)
        {
            EcnColor = color;
            SfColor = color;
        }
    }
}
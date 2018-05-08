using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ControlCenter.Modules.ClientControls
{
    public partial class DragDropPopUp : Popup
    {

        public DragDropPopUp(Canvas c)
        {
            var thumb = new Thumb
            {
                Width = 0,
                Height = 0,
            };

            c.Children.Add(thumb);
            this.Child = c;

            MouseDown += (sender, e) =>
            {
                thumb.RaiseEvent(e);
            };

            thumb.DragDelta += (sender, e) =>
            {
                HorizontalOffset += e.HorizontalChange;
                VerticalOffset += e.VerticalChange;
            };
        }
    }
}

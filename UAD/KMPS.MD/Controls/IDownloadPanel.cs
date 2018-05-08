using System.Web.UI.WebControls;

namespace KMPS.MD.Controls
{
    public interface IDownloadPanel
    {
        PlaceHolder PhProfileFields { get; }
        ListBox LstAvailableProfileFields { get; }
        ListBox LstSelectedFields { get; }
        PlaceHolder PhDemoFields { get; }
        ListBox LstAvailableDemoFields { get; }
        ListBox LstAvailableAdhocFields { get; }
    }
}

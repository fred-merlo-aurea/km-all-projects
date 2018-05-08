namespace ActiveUp.WebControls.Common.Interface
{
    /// <summary>
    /// This interface to be implemented by the <see cref="System.Web.UI.WebControls.WebControl"/> to expose
    /// its viewstate methods for the purpose of unit testing.
    /// </summary>
    public interface IControl
    {
        void LoadViewState(object savedState);
        object SaveViewState();
        void TrackViewState();
    }
}

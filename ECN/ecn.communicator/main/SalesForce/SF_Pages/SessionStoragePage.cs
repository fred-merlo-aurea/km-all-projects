using System.Web.UI;

namespace ecn.communicator.main.Salesforce.SF_Pages
{
    public abstract class SessionStoragePage : Page
    {
        public T Get<T>(string key, T defaultValue)
        {
            return Session[key] != null ? (T)Session[key] : defaultValue;
        }

        public void Set<T>(string key, T value)
        {
            Session[key] = value;
        }
    }
}
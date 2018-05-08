using System.Web;
using Ecn.Communicator.Main.Lists.Interfaces;

namespace Ecn.Communicator.Main.Lists.Helpers
{
    public class RequestAdapter : IRequest
    {
        private HttpRequest _request;
        public RequestAdapter(HttpRequest request)
        {
            _request = request;
        }
        public string GetQueryStringValue(string key)
        {
            return _request.QueryString[key].ToString();
        }
    }
}

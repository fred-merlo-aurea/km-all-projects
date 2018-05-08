using System.Web;

namespace Ecn.Communicator.Mvc.Helpers
{
    public class HttpResponseAdapter : HttpResponseBase
    {
        private HttpResponseBase _httpResponse;
        public HttpResponseAdapter(HttpResponseBase response)
        {
            _httpResponse = response;
        }
    }
}

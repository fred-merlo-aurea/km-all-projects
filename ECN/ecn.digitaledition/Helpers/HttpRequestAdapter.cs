using System.Web;

namespace Ecn.DigitalEdition.Helpers
{
    public class HttpRequestAdapter : HttpRequestBase
    {
        private HttpRequest _httpRequest;
        public HttpRequestAdapter(HttpRequest request)
        {
            _httpRequest = request;
        }
    }
}

using System.Web;

namespace Ecn.Communicator.Main.Helpers
{
    public class HttpResponseAdapter : HttpResponseBase
    {
        private HttpResponse httpResponse;
        public HttpResponseAdapter(HttpResponse response)
        {
            httpResponse = response;
        }
    }
}

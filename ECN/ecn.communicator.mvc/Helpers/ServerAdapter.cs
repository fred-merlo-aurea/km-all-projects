using System.Web;

namespace Ecn.Communicator.Mvc.Helpers
{
    public class ServerAdapter : HttpServerUtilityBase
    {
        private HttpServerUtilityBase _server;
        public ServerAdapter(HttpServerUtilityBase server)
        {
            _server = server;
        }
    }
}

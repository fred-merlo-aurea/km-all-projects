using System.Web;
using Ecn.Communicator.Main.Interfaces;

namespace Ecn.Communicator.Main.Helpers
{
    public class ServerAdapter : IServer
    {
        private HttpServerUtility _server;
        public ServerAdapter(HttpServerUtility server)
        {
            _server = server;
        }

        public string MapPath(string path)
        {
            return _server.MapPath(path);
        }
    }
}

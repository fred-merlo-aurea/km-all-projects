using ecn.webservice.classes;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using KMPlatform.Entity;

namespace ecn.webservice
{
    public class WebMethodExecutionContext
    {
        public string ServiceMethodName { get; set; }
        public string MethodName { get; set; }
        public int ApiLogId { get; set; }
        public User User { get; set; }
        public IResponseManager ResponseManager { get; set; }
        public IAPILoggingManager ApiLoggingManager { get; set; }
    }
}
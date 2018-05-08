using System;
using FrameworkUAS.Service;
using KMPlatform.Object;

namespace WebServiceFramework
{
    /// <summary>
    /// Represents a model to hold request parameters.
    /// </summary>
    public class ServiceRequestModel<TResult>
    {
        public Guid AccessKey { get; set; }
        public string AuthenticateRequestData { get; set; }
        public string AuthenticateEntity { get; set; }
        public string AuthenticateMethod { get; set; }
        public Func<Authentication, bool> AuthenticateFunc { get; set; }
        public Func<ServiceRequestModel<TResult>, TResult> WorkerFunc { get; set; }
        public bool? Succeeded { get; set; }
        public ClientConnections ClientConnection { get; set; }
    }
}

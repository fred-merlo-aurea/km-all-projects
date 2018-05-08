using System;

namespace UAD_WS
{
    /// <summary>
    /// Represents a model to hold request parameters.
    /// </summary>
    public class RequestModel<TResult>
    {
        public Guid AccessKey { get; set; }
        public string AuthenticateRequestData { get; set; }
        public string AuthenticateEntity { get; set; }
        public string AuthenticateMethod { get; set; }
        public Func<RequestModel<TResult>, TResult> WorkerFunc { get; set; }
        public bool? Succeeded { get; set; }
    }
}

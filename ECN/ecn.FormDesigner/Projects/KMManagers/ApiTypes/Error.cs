using System;

namespace KMManagers.APITypes
{
    public class Error
    {
        public string Message { get; set; }
        public string HttpStatusCode { get; set; }
    }
}
using System;
using System.Runtime.Serialization;

namespace KMWeb.Services
{
    [Serializable]
    public class SelfServicingException : Exception
    {
        public string Code { get; }

        public SelfServicingException()
        {
        }
        
        public SelfServicingException(string message)
            : base(message)
        {
            
        }
        
        public SelfServicingException(string message, string code) : this(message)
        {
            Code = code;
        }

        public SelfServicingException(string message, Exception innerException, string code) 
            : base(message, innerException)
        {
            Code = code;
        }

        public SelfServicingException(SerializationInfo serializationInfo, StreamingContext context, string code)
            : base(serializationInfo, context)
        {
            Code = code;
        }
    }
}
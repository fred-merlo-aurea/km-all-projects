using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.Service
{
    [Serializable]
    [DataContract(Name = "ResponseOf{0}")]
    public class Response<T>
    {
        public Response() 
        {
            ProcessCode = Core_AMS.Utilities.StringFunctions.GenerateProcessCode();
            Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Access_Denied;
            Message = string.Empty;
            Result = default(T);
        }


        public Response(T result, string processCode = "")
        {
            this.Result = result;
            this.ProcessCode = processCode;
            if (result != null)
            {
                this.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
            }
            else
            {
                this.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
            }
        }

        public Response(T result, FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes status, string processCode = "")
        {
            this.Status = status;
            this.Result = result;
            this.ProcessCode = processCode;
        }

        public Response(T result, FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes status, string message, string processCode = "")
        {
            this.Status = status;
            this.Message = message;
            this.Result = result;
            this.ProcessCode = processCode;
        }

        #region Properties
        [DataMember]
        public string ProcessCode { get; set; }

        [DataMember]
        public FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes Status { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public T Result { get; set; }
        #endregion
    }
}

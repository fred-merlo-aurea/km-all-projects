using System;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IValidationResult
    {
        [OperationContract]
        Response<string> GetCustomerErrorMessage(Guid accessKey, FrameworkUAD.Object.ValidationResult vr, bool isTextQualifier);

        [OperationContract]
        Response<string> GetBadData(Guid accessKey, FrameworkUAD.Object.ValidationResult vr, bool isTextQualifier);

        [OperationContract]
        Response<string> GetBadDataFileValidator(Guid accessKey, FrameworkUAD.Object.ValidationResult vr, string fileName, bool isTextQualifier);
    }
}

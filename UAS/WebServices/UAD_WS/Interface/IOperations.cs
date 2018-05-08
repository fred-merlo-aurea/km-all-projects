using System;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IOperations
    {
        [OperationContract]
        Response<bool> RemovePubCode(Guid accessKey, string pubCode, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> RemoveProcessCode(Guid accessKey, string processCode, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> QSourceValidation(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> FileValidator_QSourceValidation(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client);
    }
}

using System;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ICodeSheetMasterCodeSheetBridge
    {
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.CodeSheetMasterCodeSheetBridge x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> DeleteCodeSheetID(Guid accessKey, int codeSheetID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> DeleteMasterID(Guid accessKey, int masterID, KMPlatform.Object.ClientConnections client);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ICodeSheet
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.CodeSheet>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForProduct")]
        Response<List<FrameworkUAD.Entity.CodeSheet>> Select(Guid accessKey, int pubID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.CodeSheet x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> DeleteCodeSheetID(Guid accessKey, int codeSheetID, KMPlatform.Object.ClientConnections client);        

        [OperationContract]
        Response<bool> CodeSheetValidation(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> CodeSheetValidation_Delete(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> FileValidator_CodeSheetValidation(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ICodeType
    {
        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.CodeType>> Select(Guid accessKey);
        [OperationContract (Name="SelectForCodeTypeId")]
        Response<FrameworkUAD_Lookup.Entity.CodeType> Select(Guid accessKey, int codeTypeId);
        [OperationContract (Name="SelectForCodeType")]
        Response<FrameworkUAD_Lookup.Entity.CodeType> Select(Guid accessKey, FrameworkUAD_Lookup.Enums.CodeType codeType);
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.CodeType x);
    }
}

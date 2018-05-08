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
    public interface ICode
    {
        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.Code>> Select(Guid accessKey);
        [OperationContract (Name="SelectForCodeTypeId")]
        Response<List<FrameworkUAD_Lookup.Entity.Code>> Select(Guid accessKey, int codeTypeId);
        [OperationContract (Name="SelectForCodeType")]
        Response<List<FrameworkUAD_Lookup.Entity.Code>> Select(Guid accessKey, FrameworkUAD_Lookup.Enums.CodeType codeType);
        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.Code>> SelectForDemographicAttribute(Guid accessKey, FrameworkUAD_Lookup.Enums.CodeType codeType, int dataCompareResultQueId, string ftpFolder);
        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.Code>> SelectChildren(Guid accessKey, int parentCodeID);
        [OperationContract]
        Response<FrameworkUAD_Lookup.Entity.Code> SelectCodeId(Guid accessKey, int codeId);
        [OperationContract]
        Response<FrameworkUAD_Lookup.Entity.Code> SelectCodeName(Guid accessKey, FrameworkUAD_Lookup.Enums.CodeType codeType, string codeName);
        [OperationContract]
        Response<FrameworkUAD_Lookup.Entity.Code> SelectCodeValue(Guid accessKey, FrameworkUAD_Lookup.Enums.CodeType codeType, string codeValue);
        [OperationContract]
        Response<bool> CodeExist(Guid accessKey, string codeName, int codeTypeID);
        [OperationContract (Name="CodeExistForCodeType")]
        Response<bool> CodeExist(Guid accessKey, string codeName, FrameworkUAD_Lookup.Enums.CodeType codeType);
        [OperationContract]
        Response<bool> CodeValueExist(Guid accessKey, string codeValue, int codeTypeID);
        [OperationContract (Name="CodeValueExistForCodeType")]
        Response<bool> CodeValueExist(Guid accessKey, string codeValue, FrameworkUAD_Lookup.Enums.CodeType codeType);
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.Code x);
    }
}

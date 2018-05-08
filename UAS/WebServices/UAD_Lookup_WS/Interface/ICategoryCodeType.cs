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
    public interface ICategoryCodeType
    {
        [OperationContract]
        Response<bool> Exists(Guid accessKey, string categoryCodeTypeName);

        [OperationContract(Name="SelectForCategoryCodeTypeEnum")]
        Response<FrameworkUAD_Lookup.Entity.CategoryCodeType> Select(Guid accessKey, FrameworkUAD_Lookup.Enums.CategoryCodeType categoryCodeTypeName);

        [OperationContract(Name = "SelectForCategoryCodeType")]
        Response<FrameworkUAD_Lookup.Entity.CategoryCodeType> Select(Guid accessKey, string categoryCodeTypeName);

        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> Select(Guid accessKey);

        [OperationContract(Name = "SelectForIsFree")]
        Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> Select(Guid accessKey, bool isFree);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.CategoryCodeType x);
    }
}

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
    public interface ICategoryCode
    {
        [OperationContract]
        Response<bool> Exists(Guid accessKey, int categoryCodeTypeID, int categoryCodeValue);

        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> SelectActiveIsFree(Guid accessKey, bool isFree);

        [OperationContract(Name="SelectForCategoryCodeType")]
        Response<FrameworkUAD_Lookup.Entity.CategoryCode> Select(Guid accessKey, int categoryCodeTypeID, int categoryCodeValue);

        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> Select(Guid accessKey);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.CategoryCode x);
    }
}

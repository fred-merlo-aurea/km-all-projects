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
    public interface ITransactionCodeType
    {
        [OperationContract]
        Response<bool> Exists(Guid accessKey, string transactionCodeTypeName);

        [OperationContract(Name="SelectForTransactionCodeTypeEnum")]
        Response<FrameworkUAD_Lookup.Entity.TransactionCodeType> Select(Guid accessKey, FrameworkUAD_Lookup.Enums.TransactionCodeType transactionCodeTypeName);

        [OperationContract(Name = "SelectForTransactionCodeType")]
        Response<FrameworkUAD_Lookup.Entity.TransactionCodeType> Select(Guid accessKey, string transactionCodeTypeName);

        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> Select(Guid accessKey);

        [OperationContract(Name = "SelectForIsFree")]
        Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> Select(Guid accessKey, bool isFree);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.TransactionCodeType x);
    }
}

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
    public interface ITransactionCode
    {
        [OperationContract]
        Response<bool> Exists(Guid accessKey, int transactionCodeTypeID, int transactionCodeValue);

        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> SelectActiveIsFree(Guid accessKey, bool isFree);

        [OperationContract]
        Response<FrameworkUAD_Lookup.Entity.TransactionCode> SelectTransactionCodeValue(Guid accessKey, int transactionCodeValue);

        [OperationContract(Name = "SelectForTransactionCodeTypeAndTransactionCodeValue")]
        Response<FrameworkUAD_Lookup.Entity.TransactionCode> Select(Guid accessKey, int transactionCodeTypeID, int transactionCodeValue);

        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> Select(Guid accessKey);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.TransactionCode x);
    }
}

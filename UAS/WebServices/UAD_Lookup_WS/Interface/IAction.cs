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
    public interface IAction
    {
        [OperationContract]
        Response<bool> Exists(Guid accessKey, int actionTypeID, int categoryCodeID, int transactionCodeID);

        [OperationContract]
        Response<FrameworkUAD_Lookup.Entity.Action> GetByActionID(Guid accessKey, int actionID, bool getChildren = false);

        [OperationContract(Name="SelectForCatTran")]
        Response<FrameworkUAD_Lookup.Entity.Action> Select(Guid accessKey, int categoryCodeID, int transactionCodeID);

        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.Action>> Select(Guid accessKey);

        [OperationContract]
        Response<bool> Validate(Guid accessKey, FrameworkUAD_Lookup.Entity.Action action);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.Action action);
    }
}

using System;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace SubGen_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ISubGenUtils
    {
        [OperationContract]
        Response<string> GetLoginToken(Guid accessKey, FrameworkSubGen.Entity.Enums.Client sgClient, KMPlatform.Entity.User user, bool isSgAdmin);

        [OperationContract]
        Response<string> GetTestingLoginToken(Guid accessKey, KMPlatform.Entity.User user, bool isSgAdmin);
    }
}

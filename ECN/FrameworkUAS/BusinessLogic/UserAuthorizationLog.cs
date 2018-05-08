using System;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class UserAuthorizationLog
    {
        public bool LogOut(int userAuthLogId)
        {
            return DataAccess.UserAuthorizationLog.LogOut(userAuthLogId); 
        }
        public Entity.UserAuthorizationLog SetUserAuthLog(Object.UserAuthorization uAuth, int userID)
        {
            //log the attempt
            UserAuthorizationLog ualWorker = new UserAuthorizationLog();
            Entity.UserAuthorizationLog ual = new Entity.UserAuthorizationLog();
            ual.AuthAccessKey = null;
            ual.AuthAttemptDate = uAuth.AuthAttemptDate;
            ual.AuthAttemptTime = uAuth.AuthAttemptTime;
            ual.AuthMode = uAuth.AuthorizationMode;
            ual.AuthSource = uAuth.AuthSource;
            ual.AuthUserName = uAuth.AuthUserName;
            ual.IpAddress = uAuth.IpAddress;
            ual.IsAuthenticated = uAuth.IsAuthenticated;
            ual.LogOutDate = null;
            ual.LogOutTime = null;
            Core_AMS.Utilities.JsonFunctions jfunct = new Core_AMS.Utilities.JsonFunctions();
            //ual.ServerVariables = jfunct.ToJson<Object.ServerVariable>(uAuth.ServerVariables);
            ual.UserID = userID;

            ual.UserAuthLogID = ualWorker.Save(ual);

            return ual;
        }
        public int Save(Entity.UserAuthorizationLog x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.UserAuthLogID = DataAccess.UserAuthorizationLog.Save(x);
                scope.Complete();
            }

            return x.UserAuthLogID;
        }
    }
}

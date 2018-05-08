//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ECN_Framework_Common.Functions;
//using ECN_Framework_Common.Objects;
//using System.Transactions;

//namespace ECN_Framework_BusinessLayer.Accounts
//{
//    [Serializable]
//    public class User_OLD
//    {

//        private static readonly string BC_User_CacheName = "CACHE_BASECHANNEL_USERS_";


//        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.User;
        
//        public static bool Exists(int userID, int customerID)
//        {
//            return ECN_Framework_DataLayer.Accounts.User.Exists(userID, customerID);
//        }

//        public static bool ExistsByBaseChannelID(int userID, int baseChannelID)
//        {
//            return ECN_Framework_DataLayer.Accounts.User.ExistsByBaseChannelID(userID, baseChannelID);
//        }

//        public static bool Exists(string username, int userID, int customerID)
//        {
//            return ECN_Framework_DataLayer.Accounts.User.Exists(username, userID, customerID);
//        }

//        public static KMPlatform.Entity.User Login(string username, string password)
//        {
//            KMPlatform.Entity.User user = null;

//            if (ECN_Framework_Common.Functions.StringFunctions.HasValue(username) && ECN_Framework_Common.Functions.StringFunctions.HasValue(password))
//            {
//                user = ECN_Framework_DataLayer.Accounts.User.Login(username, password);
//            }
//            else
//            {
//                List<ECNError> errorlist = new List<ECNError>();
//                errorlist.Add(new ECNError( Enums.Entity.User, Enums.Method.None, "Invalid Username or Password"));
//                throw new ECNException(errorlist, Enums.ExceptionLayer.Business);
//            }
               
//            return user;
//        }

//        //TODO:SUNIL - Remove this method - fetch for MASTERUSERID data from Customerlogin DO NOT HAVE customerID - added overloaded as TEMP fix) -- 11/16/2012
//        public static KMPlatform.Entity.User GetByUserID(int userID, bool getChildren)
//        {
//            KMPlatform.Entity.User user = null;
//            if (userID > 0)
//            {
//                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
//                {
//                    user = ECN_Framework_DataLayer.Accounts.User.GetByUserID(userID);
//                    scope.Complete();
//                }

//                if (user != null && getChildren)
//                {
//                    user.UserAction = ECN_Framework_BusinessLayer.Accounts.UserAction.GetbyUserID(user.UserID);
//                    user.UserGroup = ECN_Framework_BusinessLayer.Communicator.UserGroup.Get(user.UserID);
//                }
//            }
//            return user;
//        }

//        public static KMPlatform.Entity.User GetByUserID(int userID, int customerID, bool getChildren)
//        {
//            KMPlatform.Entity.User user = null;
//            if (userID > 0 && customerID > 0)
//            {
//                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
//                {
//                    user = ECN_Framework_DataLayer.Accounts.User.GetByUserID(userID, customerID);
//                    scope.Complete();
//                }
               
//                if (user != null && getChildren)
//                {
//                    user.UserAction = ECN_Framework_BusinessLayer.Accounts.UserAction.GetbyUserID(user.UserID);
//                    user.UserGroup = ECN_Framework_BusinessLayer.Communicator.UserGroup.Get(user.UserID);
//                }
//            }
//            return user;
//        }

//        public static KMPlatform.Entity.User GetByUserName(string userName, int customerID, bool getChildren)
//        {
//            KMPlatform.Entity.User user = null;
//            if (userName.Trim().Length > 0 && customerID > 0)
//            {
//                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
//                {
//                    user = ECN_Framework_DataLayer.Accounts.User.GetByUserName(userName, customerID);
//                    scope.Complete();
//                }

//                if (user != null && getChildren)
//                {
//                    user.UserAction = ECN_Framework_BusinessLayer.Accounts.UserAction.GetbyUserID(user.UserID);
//                    user.UserGroup = ECN_Framework_BusinessLayer.Communicator.UserGroup.Get(user.UserID);
//                }
//            }
//            return user;
//        }

//        public static KMPlatform.Entity.User GetByAccessKey(string accessKey, bool getChildren)
//        {
//            KMPlatform.Entity.User user = null;
//            if (accessKey.Trim().Length > 0)
//            {
//                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
//                {
//                    user = ECN_Framework_DataLayer.Accounts.User.GetByAccessKey(accessKey);
//                    scope.Complete();
//                }

//                if (user != null && getChildren)
//                {
//                    user.UserAction = ECN_Framework_BusinessLayer.Accounts.UserAction.GetbyUserID(user.UserID);
//                    user.UserGroup = ECN_Framework_BusinessLayer.Communicator.UserGroup.Get(user.UserID);
//                }
//            }
//            return user;
//        }

//        public static List<KMPlatform.Entity.User> GetByCustomerID(int customerID)
//        {
//            List<KMPlatform.Entity.User> userList = new List<KMPlatform.Entity.User>();
//            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
//            {
//                userList = ECN_Framework_DataLayer.Accounts.User.GetByCustomerID(customerID);
//                scope.Complete();
//            }
//            return userList;
//        }

//        public static List<KMPlatform.Entity.User> GetUsersByChannelID(int baseChannelID)
//        {
//            string cacheKey = BC_User_CacheName + baseChannelID.ToString();
//            List<KMPlatform.Entity.User> userList = new List<KMPlatform.Entity.User>();
//            if (!CacheHelper.IsCacheEnabled())
//            {
//                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
//                {
//                    userList = ECN_Framework_DataLayer.Accounts.User.GetByBaseChannelID(baseChannelID);
//                    scope.Complete();
//                }
//            }
//            else if (CacheHelper.GetCurrentCache(cacheKey) == null)
//            {
//                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
//                {
//                    userList = ECN_Framework_DataLayer.Accounts.User.GetByBaseChannelID(baseChannelID);
//                    scope.Complete();
//                }
//                CacheHelper.AddToCache(cacheKey, userList);
//            }
//            else
//            {
//                userList = (List<KMPlatform.Entity.User>)CacheHelper.GetCurrentCache(cacheKey);
//            }
//            return userList;
//        }

//        public static bool Save(KMPlatform.Entity.User user, KMPlatform.Entity.User currentUser, int baseChannelID)
//        {
//            if (Save(user))
//            {
//                CacheHelper.ClearCache(BC_User_CacheName + baseChannelID.ToString());
//                return true;
//            }
//            return false;
//        }

//        private static bool Save(KMPlatform.Entity.User user)
//        {
//            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
//            if (user.UserID > 0)
//            {
//                if (!Exists(user.UserID, user.CustomerID))
//                {
//                    List<ECNError> errorList = new List<ECNError>();
//                    errorList.Add(new ECNError(Entity, Method, "UserID is invalid"));
//                    throw new ECNException(errorList);
//                }
//            }
//            Validate(user);

           
//                user.UserID = ECN_Framework_DataLayer.Accounts.User.Save(user);
//                //USERGROUP  BULK insert/update;
//                if (user.UserGroup != null)
//                {
//                    foreach (ECN_Framework_Entities.Communicator.UserGroup userGroup in user.UserGroup)
//                    {
//                        userGroup.UserID = user.UserID;
//                        ECN_Framework_BusinessLayer.Communicator.UserGroup.Save(userGroup, user);
//                    }
//                }

//                //USERAction BULK insert/update
//                if (user.UserAction != null)
//                {
//                    List<ECN_Framework_Entities.Accounts.UserAction> userActionListCopy = new List<ECN_Framework_Entities.Accounts.UserAction>();
//                    foreach (ECN_Framework_Entities.Accounts.UserAction userAction in user.UserAction)
//                    {
//                        userAction.UserID = user.UserID;
//                        ECN_Framework_Entities.Accounts.UserAction userActionCopy = userAction;
//                        UserAction.Save(userActionCopy);
//                        userActionListCopy.Add(userActionCopy);
//                    }
//                    user.UserAction = userActionListCopy;
//                }
                
//            return true;
//        }

//        public static void Validate(KMPlatform.Entity.User user)
//        {
//            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
//            List<ECNError> errorList = new List<ECNError>();

//            if (user.CustomerID == null)
//                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
//            else
//            {
//                using (TransactionScope supressscope = new TransactionScope(TransactionScopeOption.Suppress))
//                {
//                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(user.CustomerID))
//                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));

//                    if (user.UserID <= 0 && user.CreatedUserID == null)
//                        errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

//                    if (user.UserID > 0 && user.UpdatedUserID == null)
//                        errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));
//                    if (user.RoleID == null || user.RoleID == -1 || !ECN_Framework_BusinessLayer.Accounts.Role.Exists(user.RoleID.Value, user.CustomerID))
//                        errorList.Add(new ECNError(Entity, Method, "User role is invalid"));
//                    if (Exists(user.UserName, user.UserID, user.CustomerID))
//                        errorList.Add(new ECNError(Entity, Method, "Username already exists. Please enter different UserName"));

//                    supressscope.Complete();
//                }
//            }
//            //if (!ValidatePassword(user.Password))
//            //    errorList.Add(new ECNError(Entity, Method, "Password does not meet the requirements"));

//            if (user.UserName.Trim() == string.Empty)
//                errorList.Add(new ECNError(Entity, Method, "User name is empty"));
//            if (user.Password.Trim() == string.Empty)
//                errorList.Add(new ECNError(Entity, Method, "User password is empty"));
//            if (user.CommunicatorOptions.Trim() == string.Empty)
//                errorList.Add(new ECNError(Entity, Method, "User communicator options is empty"));
//            if (user.CollectorOptions.Trim() == string.Empty)
//                errorList.Add(new ECNError(Entity, Method, "User collector options is empty"));
//            if (user.CreatorOptions.Trim() == string.Empty)
//                errorList.Add(new ECNError(Entity, Method, "User creator options is empty"));
//            if (user.AccountsOptions.Trim() == string.Empty)
//                errorList.Add(new ECNError(Entity, Method, "User accounts options is empty"));
//            if (user.ActiveFlag.Trim() == string.Empty || (user.ActiveFlag.Trim().ToUpper() != "Y" && user.ActiveFlag.Trim().ToUpper() != "N"))
//                errorList.Add(new ECNError(Entity, Method, "User active flag is invalid"));
            
//            if (errorList.Count > 0)
//            {
//                throw new ECNException(errorList);
//            }
//        }

//        public static bool bIsContentCreator(int userID)
//        {
//            var errorList = GetErrorList(userID);

//            if (errorList.Count > 0)
//            {
//                return true;
//            }
//            return false;
//        }

//        public static void IsContentCreator(int userID)
//        {
//            var errorList = GetErrorList(userID);

//            if (errorList.Count > 0)
//            {
//                throw new ECNException(errorList);
//            }
//        }

//        private static List<ECNError> GetErrorList(int userID)
//        {
//            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
//            List<ECNError> errorList = new List<ECNError>();
//            using (TransactionScope supressscope = new TransactionScope(TransactionScopeOption.Suppress))
//            {
//                if (ECN_Framework_BusinessLayer.Communicator.Content.CreatedUserExists(userID))
//                {
//                    errorList.Add(new ECNError(Entity, Method, "User is assigned to Content"));
//                }
//                if (ECN_Framework_BusinessLayer.Communicator.Layout.CreatedUserExists(userID))
//                {
//                    errorList.Add(new ECNError(Entity, Method, "User is assigned to Layout"));
//                }
//                if (ECN_Framework_BusinessLayer.Communicator.Blast.CreatedUserExists(userID))
//                {
//                    errorList.Add(new ECNError(Entity, Method, "User is assigned to Blast"));
//                }
//                supressscope.Complete();
//            }
//            return errorList;
//        }

//        public static bool Delete(int userID, KMPlatform.Entity.User loggedInUser)
//        {
//            IsContentCreator(userID);

//            if (loggedInKM.Platform.User.IsSystemAdministrator(user) || ((loggedInKM.Platform.User.IsChannelAdministrator(user) || loggedInUser.IsAdmin) && loggedInUser.HasUserAccess))
//            {
//                KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByUserID(userID, false);

//                return ECN_Framework_DataLayer.Accounts.User.Delete(userID, user.CustomerID, loggedInUser.UserID);
//            }
//            else
//            {
//                throw new SecurityException("Logged in User does not have permission to delete this user.");
//            }
//        }
        
//        public static bool HasPermission(int userID, string ActionCode)
//        {
//            return KM.Platform.User.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, "Communicator", ActionCode);
//            //List<ECN_Framework_Entities.Accounts.UserAction> lUserActions = ECN_Framework_BusinessLayer.Accounts.UserAction.GetbyUserID(userID);

//            //var ua = from lua in lUserActions
//            //         join la in ECN_Framework_BusinessLayer.Accounts.Action.GetAll()
//            //         on lua.ActionID.Value equals la.ActionID
//            //         where lua.UserID.Value == userID && la.ActionCode.ToUpper() == ActionCode.ToUpper() && lua.Active.ToUpper() == "Y"
//            //         select lua;

//            //return (ua.Any() ? true:false);
//        }


//        public static bool ValidatePassword_Temp(string Password)
//        {
//            bool validation = true;

//            //Cannot contain special characters
//            if (!ECN_Framework_Common.Functions.RegexUtilities.IsAplhaNumeric(Password))
//            {
//                validation = false;
//            }
//            return validation;
//        }

//        public static bool ValidatePassword(string Password)
//        {
//            return ECN_Framework_Common.Functions.RegexUtilities.IsValidPassword(Password);
//        }
//    }
//}

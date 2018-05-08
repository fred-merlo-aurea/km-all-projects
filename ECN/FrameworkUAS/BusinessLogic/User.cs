using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Data;
using System.Text;
using KMPlatform.BusinessLogic.Interfaces;
using CommonEncryption = KM.Common.Encryption;

namespace KMPlatform.BusinessLogic
{
    public class User : IUser
    {
        public List<Entity.User> Select(bool includeObjects = false)
        {
            List<Entity.User> x = null;
            x = DataAccess.User.Select().ToList();
            if (x != null)
            {
                FillObjects(x, includeObjects);
            }
            return x;
        }
        public List<Entity.User> Select(int clientGroupID, bool includeObjects = false)
        {
            List<Entity.User> x = null;
            x = DataAccess.User.Select(clientGroupID).ToList();
            if (x != null)
            {
                FillObjects(x, includeObjects);
            }
            return x;
        }
        public List<Entity.User> Select(int clientID, int securityGroupID, bool includeObjects = false)
        {
            List<Entity.User> x = null;
            x = DataAccess.User.Select(clientID, securityGroupID).ToList();
            if (x != null)
            {
                FillObjects(x, includeObjects);
            }
            return x;
        }
        public List<Entity.User> Select(int clientID, string securityGroupName, bool includeObjects = false)
        {
            securityGroupName = securityGroupName.Replace("_", " ");

            List<Entity.User> x = null;
            x = DataAccess.User.Select(clientID, securityGroupName).ToList();
            if (x != null)
            {
                FillObjects(x, includeObjects);
            }
            return x;
        }

        public List<Entity.User> SelectByClientGroupID(int clientGroupID, bool includeObjects = false)
        {
            List<Entity.User> x = null;
            x = DataAccess.User.SelectByClientGroupID(clientGroupID).ToList();
            if (x != null)
            {
                FillObjects(x, includeObjects);
            }
            return x;
        }




        public List<Entity.User> SelectByClientID(int clientID, bool includeObjects = false)
        {
            List<Entity.User> x = null;
            x = DataAccess.User.SelectByClientID(clientID).ToList();
            if (x != null)
            {
                FillObjects(x, includeObjects);
            }
            return x;
        }

        public void FillObjects(List<Entity.User> users, bool includeObjects)
        {
            foreach (Entity.User u in users)
            {
                DecryptPassword(u);
                if (includeObjects == true)
                {
                    ClientGroup cgWorker = new ClientGroup();
                    UserClientSecurityGroupMap ucsgWorker = new UserClientSecurityGroupMap();
                    Client cWorker = new Client();
                    SecurityGroup sgWorker = new SecurityGroup();

                    u.ClientGroups = cgWorker.SelectForUser(u.UserID, true);
                    u.UserClientSecurityGroupMaps = ucsgWorker.SelectForUser(u.UserID);
                    u.CurrentClientGroup = cgWorker.Select(u.DefaultClientGroupID, true);
                    u.CurrentClient = cWorker.Select(u.DefaultClientID, true);
                    u.CurrentSecurityGroup = sgWorker.Select(u.UserID, u.DefaultClientID, u.IsKMStaff);
                }
            }
        }

        ////public List<Entity.User> Select(int clientID, int securityGroupID, bool includeObjects = false)
        ////{
        ////    List<Entity.User> x = null;
        ////    x = DataAccess.User.Select(clientID, securityGroupID).ToList();
        ////    if (x != null)
        ////    {
        ////        foreach (Entity.User u in x)
        ////        {
        ////            DecryptPassword(u);
        ////            if (includeObjects == true)
        ////            {
        ////                ClientGroup cgWorker = new ClientGroup();
        ////                UserClientSecurityGroupMap ucsgWorker = new UserClientSecurityGroupMap();
        ////                Client cWorker = new Client();
        ////                SecurityGroup sgWorker = new SecurityGroup();

        ////                u.ClientGroups = cgWorker.SelectForUser(u.UserID, true);
        ////                u.UserClientSecurityGroupMaps = ucsgWorker.SelectForUser(u.UserID);
        ////                u.CurrentClientGroup = cgWorker.Select(u.DefaultClientGroupID, true);
        ////                u.CurrentClient = cWorker.Select(u.DefaultClientID, true);
        ////                u.CurrentSecurityGroup = sgWorker.Select(u.UserID, u.DefaultClientID);
        ////            }
        ////        }
        ////    }
        ////    return x;
        ////}
        
        ////public List<Entity.User> Select(int clientID, string securityGroupName, bool includeObjects = false)
        ////{
        ////    securityGroupName = securityGroupName.Replace("_", " ");

        ////    List<Entity.User> x = null;
        ////    x = DataAccess.User.Select(clientID, securityGroupName).ToList();
        ////    if (x != null)
        ////    {
        ////        foreach (Entity.User u in x)
        ////        {
        ////            DecryptPassword(u);
        ////            if (includeObjects == true)
        ////            {
        ////                ClientGroup cgWorker = new ClientGroup();
        ////                UserClientSecurityGroupMap ucsgWorker = new UserClientSecurityGroupMap();
        ////                Client cWorker = new Client();
        ////                SecurityGroup sgWorker = new SecurityGroup();

        ////                u.ClientGroups = cgWorker.SelectForUser(u.UserID, true);
        ////                u.UserClientSecurityGroupMaps = ucsgWorker.SelectForUser(u.UserID);
        ////                u.CurrentClientGroup = cgWorker.Select(u.DefaultClientGroupID, true);
        ////                u.CurrentClient = cWorker.Select(u.DefaultClientID, true);
        ////                u.CurrentSecurityGroup = sgWorker.Select(u.UserID, u.DefaultClientID);
        ////            }
        ////        }
        ////    }
        ////    return x;
        ////}

        public Entity.User SelectUser(int userID, bool includeObjects = false)
        {
            Entity.User x = DataAccess.User.SelectUser(userID);

            if (x == null)
            {
                return null;
            }

            DecryptPassword(x);
            if (includeObjects == true)
            {
                x = SetUserObjects(x);
            }

            return x;
        }

        public Entity.User SearchUserName(string userName)
        {
            Entity.User x = null;
            x = DataAccess.User.SearchUserName(userName);

            if (x != null && !string.IsNullOrEmpty(x.Salt))
                DecryptPassword(x);
            return x;
        }

        public  Entity.User LogIn(string userName, string password, bool includeObjects = true)
        {
            Entity.User x = SearchUserName(userName);

            if (x != null)
            {
                if (!x.Password.Equals(password))
                {
                    throw new Object.UserLoginException() { UserStatus = KMPlatform.Enums.UserLoginStatus.InvalidPassword, CurrentUser = x };
                    
                }
            }

            if(x != null && x.Status == KMPlatform.Enums.UserStatus.Disabled)
            {
                throw new Object.UserLoginException() { UserStatus = KMPlatform.Enums.UserLoginStatus.DisabledUser, CurrentUser = x };
            }
            else if (x != null && x.Status == KMPlatform.Enums.UserStatus.Locked)
            {
                throw new Object.UserLoginException() { UserStatus = KMPlatform.Enums.UserLoginStatus.LockedUser, CurrentUser = x };
            }

            if (x != null && x.IsActive == false)
                x = null;

            if (x != null && includeObjects == true)
            {
                x = SetAuthorizedUserObjects(x, x.DefaultClientGroupID, x.DefaultClientID);
                if (!x.IsPlatformAdministrator && x.UserClientSecurityGroupMaps.Count == 0)
                {
                    throw new Object.UserLoginException() { UserStatus = KMPlatform.Enums.UserLoginStatus.NoRoles, CurrentUser = x };
                    
                }
                else
                {
                    return x;
            }
            }
            else return x;
        }
        public Entity.User LogInLite(string userName, string password, bool includeObjects = true)
        {
            Entity.User x = SearchUserName(userName);

            if (x != null)
            {
                if (!x.Password.Equals(password))
                {
                    throw new Object.UserLoginException() { UserStatus = KMPlatform.Enums.UserLoginStatus.InvalidPassword, CurrentUser = x };

                }
            }

            if (x != null && x.Status == KMPlatform.Enums.UserStatus.Disabled && x.IsActive == false)
            {
                throw new Object.UserLoginException() { UserStatus = KMPlatform.Enums.UserLoginStatus.DisabledUser, CurrentUser = x };
            }
            else if (x != null && x.Status == KMPlatform.Enums.UserStatus.Locked && x.IsActive == false)
            {
                throw new Object.UserLoginException() { UserStatus = KMPlatform.Enums.UserLoginStatus.LockedUser, CurrentUser = x };
            }


            if (x != null && x.IsActive == false)
                x = null;

            if (x != null && includeObjects == true)
            {
                x = SetAuthorizedUserObjectsLite(x, x.DefaultClientGroupID, x.DefaultClientID);
                if (!x.IsPlatformAdministrator && x.UserClientSecurityGroupMaps.Count == 0)
                {
                    throw new Object.UserLoginException() { UserStatus = KMPlatform.Enums.UserLoginStatus.NoRoles, CurrentUser = x };

                }
                else
                {
                    return x;
                }
                
            } 
            else return x;
        }

        public Entity.User LogIn(Guid accessKey, bool includeObjects = true)
        {
            string _CacheRegion = "USERACCESSKEY";

            Entity.User x = null;

            if (KM.Common.CacheUtil.IsCacheEnabled())
            {
                x = (Entity.User)KM.Common.CacheUtil.GetFromCache(string.Format("{0}-{1}", accessKey.ToString(), includeObjects.ToString()), _CacheRegion);

                if (x != null)
                    return x;
            }

            x = DataAccess.User.LogIn(accessKey);
            if (x != null)
                DecryptPassword(x);

            if (x != null && x.Status == KMPlatform.Enums.UserStatus.Disabled && x.IsActive == false)
            {
                throw new Object.UserLoginException() { UserStatus = KMPlatform.Enums.UserLoginStatus.DisabledUser, CurrentUser = x };
            }
            else if (x != null && x.Status == KMPlatform.Enums.UserStatus.Locked && x.IsActive == false)
            {
                throw new Object.UserLoginException() { UserStatus = KMPlatform.Enums.UserLoginStatus.LockedUser, CurrentUser = x };
            }

            if (x != null && x.IsActive == false)
                x = null;

            if (x != null && includeObjects == true)
            {
                x = SetAuthorizedUserObjects(x, x.DefaultClientGroupID, x.DefaultClientID);
                if (!x.IsPlatformAdministrator && x.UserClientSecurityGroupMaps.Count == 0)
                {
                    throw new Object.UserLoginException() { UserStatus = KMPlatform.Enums.UserLoginStatus.NoRoles, CurrentUser = x };
                }
            }

            if (KM.Common.CacheUtil.IsCacheEnabled())
            {
                KM.Common.CacheUtil.AddToCache(string.Format("{0}-{1}", accessKey, includeObjects.ToString()), x, _CacheRegion);
            }

            return x;
        }

        public Entity.User SetAuthorizedUserObjects(Entity.User user, int clientID)
        {
            ClientGroup cgWorker = new ClientGroup();
            UserClientSecurityGroupMap ucsgWorker = new UserClientSecurityGroupMap();
            Client cWorker = new Client();
            SecurityGroup sgWorker = new SecurityGroup();

            //all these methods must only set items that user is authorized for

            if (user.IsPlatformAdministrator == true)
                user.ClientGroups = cgWorker.Select(false);
            else
                user.ClientGroups = cgWorker.SelectForUserAuthorization(user.UserID);//done

            user.UserClientSecurityGroupMaps = ucsgWorker.SelectForUserAuthorization(user.UserID);//done
            //user.CurrentClientGroup = cgWorker.Select(clientgroupID, true);//done
            user.CurrentClient = cWorker.Select(clientID, true);//done
            user.CurrentSecurityGroup = sgWorker.Select(user.UserID, clientID, true);//done

            return user;
        }

        public Entity.User SetAuthorizedUserObjects(Entity.User user, int clientgroupID, int clientID)
        {
            ClientGroup cgWorker = new ClientGroup();
            UserClientSecurityGroupMap ucsgWorker = new UserClientSecurityGroupMap();
            Client cWorker = new Client();
            SecurityGroup sgWorker = new SecurityGroup();

            if (user.IsPlatformAdministrator == true)
                user.ClientGroups = cgWorker.Select(false);
            else
                user.ClientGroups = cgWorker.SelectForUserAuthorization(user.UserID, false);//done

            user.UserClientSecurityGroupMaps = ucsgWorker.SelectForUserAuthorization(user.UserID);//done
            user.CurrentClientGroup = cgWorker.Select(clientgroupID, true);//done
            user.CurrentClient = cWorker.Select(clientID, true);//done
            user.CurrentSecurityGroup = sgWorker.Select(user.UserID, clientID, true);//done

            return user;
        }
        public Entity.User SetAuthorizedUserObjectsLite(Entity.User user, int clientgroupID, int clientID)
        {
            ClientGroup cgWorker = new ClientGroup();
            UserClientSecurityGroupMap ucsgWorker = new UserClientSecurityGroupMap();
            Client cWorker = new Client();
            SecurityGroup sgWorker = new SecurityGroup();

            //all these methods must only set items that user is authorized for
            //if (user.IsKMStaff == true)
            //    user.ClientGroups = cgWorker.SelectForAMS(true);
            //else
            user.ClientGroups = cgWorker.SelectForUserAuthorizationLite(user.UserID, true);//done

            user.UserClientSecurityGroupMaps = ucsgWorker.SelectForUserAuthorization(user.UserID);//done
            user.CurrentClientGroup = cgWorker.Select(clientgroupID, true);//done
            user.CurrentClient = cWorker.Select(clientID, true);//done
            user.CurrentSecurityGroup = sgWorker.Select(user.UserID, clientID, user.IsKMStaff, true);//done

            return user;
        }
        /// <summary>
        /// gets only items that are enabled/active for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Entity.User SetUserObjects(Entity.User user)
        {
            ClientGroup cgWorker = new ClientGroup();
            UserClientSecurityGroupMap ucsgWorker = new UserClientSecurityGroupMap();
            Client cWorker = new Client();
            SecurityGroup sgWorker = new SecurityGroup();
            Application aWorker = new Application();
            ServiceFeature sfWorker = new ServiceFeature();
            Service sWorker = new Service();

            if (user.IsPlatformAdministrator == true)
                user.ClientGroups = cgWorker.Select(false);
            else
                user.ClientGroups = cgWorker.SelectForUserAuthorization(user.UserID, false);//done

            user.UserClientSecurityGroupMaps = ucsgWorker.SelectForUser(user.UserID);
            user.CurrentClientGroup = cgWorker.Select(user.DefaultClientGroupID,true);
            user.CurrentClient = cWorker.Select(user.DefaultClientID, true);
            user.CurrentSecurityGroup = sgWorker.Select(user.UserID, user.DefaultClientID, true);
            //user.Applications = aWorker.SelectForUserClient(user.UserID, user.DefaultClientID);
            //user.MenuFeatures = new Dictionary<string,List<Entity.MenuFeature>>();
            //user.Services = sWorker.SelectForSecurityGroupAndUserID(user.CurrentSecurityGroup.SecurityGroupID, user.UserID,true);
            //user.Applications.ForEach( x => user.MenuFeatures.Add( 
            //    x.ApplicationName, 
            //    fWorker.SelectApplicationNameSecurityGroupID(x.ApplicationName, user.CurrentSecurityGroup.SecurityGroupID, true, true)
            //    ));
            //user.Services.ForEach(x => user.ServiceFeatures.Add(
            //    x.ServiceName,
            //    sfWorker.SelectOnlyEnabledSecurityGroupApplicationService(x.ServiceID, user.CurrentSecurityGroup.SecurityGroupID, x.DefaultApplicationID)
            //    ));

            return user;
        }

        public bool EmailExist(string email)
        {
            bool emailExist = false;
            Entity.User u = SearchEmail(email);
            if (u != null)
                emailExist = true;
            return emailExist;
        }
        public List<Entity.User> Search(string searchValue, List<Entity.User> searchList)
        {
            searchValue = searchValue.ToLower();
            List<Entity.User> matchList = new List<Entity.User>();
            //SecurityGroup s = new SecurityGroup();
            //List<Entity.SecurityGroup> listSG = s.Select().Where(x => x.SecurityGroupName.ToLower().Contains(searchValue)).ToList();
            //if (listSG != null && listSG.Count > 0)
            //{
            //    int sgID = listSG.First().SecurityGroupID;
            //    //matchList.AddRange(searchList.FindAll(x => x.SecurityGroupID == sgID));
            //}

            matchList.AddRange(searchList.FindAll(x => x.UserID.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.FirstName.ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.LastName.ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.UserName.ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.EmailAddress.ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.IsActive.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.DateCreated.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.DateUpdated.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.CreatedByUserID.ToString().ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.UpdatedByUserID.ToString().ToLower().Contains(searchValue)));
            return matchList.Distinct().ToList();
        }
        public string GetEmailAddress(string userName)
        {
            string email = string.Empty;
            Entity.User user = SearchUserName(userName);
            if (user != null)
                email = user.EmailAddress;
            return email;
        }
        public int Save(Entity.User x)
        {
            if (string.IsNullOrEmpty(x.UserName))
                x.UserName = x.EmailAddress;

           
            if (x.CreatedByUserID < 1)
                x.CreatedByUserID = -1;
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;
            //ensure password is encrypted n salted
            //if password is null or empty generate a random password
            if (string.IsNullOrEmpty(x.Password))
            {
                var encryption = new CommonEncryption();
                x.Password = encryption.GeneratePassword(18, 6);
            }

            x = EncryptPassword(x);

            using (TransactionScope scope = new TransactionScope())
            {
                x.UserID = DataAccess.User.Save(x);
                scope.Complete();
            }

            return x.UserID;
        }
        private  Entity.User DecryptPassword(Entity.User user)
        {
            if(string.IsNullOrEmpty(user.Salt))
            {
                return user;
            }
            Object.Encryption ec = new Object.Encryption();
            ec.EncryptedText = user.Password;
            ec.SaltValue = user.Salt;
            BusinessLogic.Encryption e = new Encryption();
            ec = e.Decrypt(ec);
            user.Password = ec.PlainText;

            return user;
        }
        private  Entity.User EncryptPassword(Entity.User user)
        {
            Object.Encryption ec = new Object.Encryption();
            ec.PlainText = user.Password;
            BusinessLogic.Encryption e = new Encryption();
            ec = e.Encrypt(ec);
            user.Password = ec.EncryptedText;
            user.Salt = ec.SaltValue;

            return user;
        }
        /// <summary>
        /// Doing this to get the Salt for the user account
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Entity.User SearchEmail(string email)
        {
            Entity.User x = null;
            x = DataAccess.User.SearchEmail(email);

            if (x != null)
                DecryptPassword(x);
            return x;
        }

        public bool IsKmUser(Entity.User user)
        {
            if (user.ClientGroups == null || user.ClientGroups.Count == 0)
                user = SelectUser(user.UserID, true);

            return user.ClientGroups.Exists(x => x.Clients.Exists(c => c.ClientName.Equals("KM") == true));
        }
        public bool IsKmUserNoDbCheck(Entity.User user)
        {
            if (user.ClientGroups == null || user.ClientGroups.Count == 0)
            {
                return false;
            }
            else
                return user.ClientGroups.Exists(x => x.Clients.Exists(c => c.ClientName.Equals("KM") == true));
        }
        
        public static bool Exists(int userID, int customerID)
        {

            KMPlatform.Entity.User currentUser = new KMPlatform.BusinessLogic.User().SelectUser(userID, false);
            if (!currentUser.IsPlatformAdministrator)
            {
                List<KMPlatform.Entity.UserClientSecurityGroupMap> listUserClients = (new KMPlatform.BusinessLogic.UserClientSecurityGroupMap()).SelectForUserAuthorization(userID);

                if (listUserClients != null && listUserClients.Count > 0)
                {
                    int clientID = KMPlatform.DataAccess.ECN.getClientIDbyCustomerID(customerID);

                    if (listUserClients.Find(x => x.ClientID == clientID) != null)
                        return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }


        public static KMPlatform.Entity.User GetByUserID(int userID, bool getChildren)
        {
            KMPlatform.Entity.User user = (new User()).SelectUser(userID, getChildren);
           
            return user;
        }

        public static KMPlatform.Entity.User GetByUserID(int userID, int customerID, bool getChildren)
        {
            KMPlatform.Entity.User user = null;

            List<KMPlatform.Entity.UserClientSecurityGroupMap> listUserClients = (new KMPlatform.BusinessLogic.UserClientSecurityGroupMap()).SelectForUserAuthorization(userID);

            if (listUserClients != null && listUserClients.Count > 0)
            {
                int clientID = KMPlatform.DataAccess.ECN.getClientIDbyCustomerID(customerID);

                if (listUserClients.Find(x => x.ClientID == clientID) != null)
                {
                    user = (new User()).SelectUser(userID, getChildren);

                    return (new User()).SetAuthorizedUserObjects(user, clientID);
                }
            }
            return user;
        }

        public static KMPlatform.Entity.User GetByAccessKey(string accessKey, bool getChildren = false)
        {
            return (new User()).LogIn(Guid.Parse(accessKey), getChildren);
        }

        public static List<KMPlatform.Entity.User> GetByClientGroupID(int clientgroupID)
        {
            return KMPlatform.DataAccess.User.SelectByClientGroupID(clientgroupID); 
        }

        public static List<KMPlatform.Entity.User> GetByClientGroupIDServiceCode(int clientgroupID, KMPlatform.Enums.Services serviceCode)
        {
            return KMPlatform.DataAccess.User.SelectbyClientGroupIDServiceCode(clientgroupID, serviceCode); 
        }

        public static List<KMPlatform.Entity.User> GetByClientIDServiceCode(int clientID, KMPlatform.Enums.Services serviceCode)
        {
            return KMPlatform.DataAccess.User.SelectbyClientIDServiceCode(clientID, serviceCode); 
        }

        public static List<KMPlatform.Entity.User> GetByCustomerID(int customerID)
        {
            List<KMPlatform.Entity.User> userList = new List<KMPlatform.Entity.User>();

            int clientID = KMPlatform.DataAccess.ECN.getClientIDbyCustomerID(customerID);

            if (clientID > 0)
            {
                userList = KMPlatform.DataAccess.User.Select(clientID);
            }

            return userList;
        }

        public static List<KMPlatform.Entity.User> GetUsersByChannelID(int baseChannelID)
        {

            List<KMPlatform.Entity.User> userList = new List<KMPlatform.Entity.User>();

            int clientgroupID = KMPlatform.DataAccess.ECN.getClientGroupIDbyBaseChannelID(baseChannelID);

            if (clientgroupID > 0)
            {
                userList = KMPlatform.DataAccess.User.SelectByClientGroupID(clientgroupID);
            }

            return userList;
        }
        public static KMPlatform.Entity.User GetByUserName(string userName, int customerID, bool getChildren)
        {
            KMPlatform.Entity.User user = null;

            List<KMPlatform.Entity.User> userList = new List<KMPlatform.Entity.User>();

            int clientID = KMPlatform.DataAccess.ECN.getClientIDbyCustomerID(customerID);

            if (clientID > 0)
            {
                userList = KMPlatform.DataAccess.User.Select(clientID);

                user = userList.Find(x => x.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));

                if (user != null && getChildren)
                    return (new User()).SetUserObjects(user);
            }
            return user;
        }

        public static bool ValidatePassword_Temp(string Password)
        {
            bool validation = true;

            //Cannot contain special characters
            if (!ECN_Framework_Common.Functions.RegexUtilities.IsAplhaNumeric(Password))
            {
                validation = false;
            }
            return validation;
        }

        public static bool ValidatePassword(string Password)
        {
            return ECN_Framework_Common.Functions.RegexUtilities.IsValidPassword(Password);
        }

        public bool Validate_UserName(string userName, int userID)
        {
            bool exists = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = DataAccess.User.Validate_UserName(userName,userID);
                scope.Complete();
            }
            return exists;
        }

        public void Delete(int userID)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                DataAccess.User.Delete(userID);
                scope.Complete();
            }
        }

        //sunil -TODO
        public static bool ExistsByBaseChannelID(int userID, int baseChannelID)
        {
            int cgID = KMPlatform.DataAccess.ECN.getClientGroupIDbyBaseChannelID(baseChannelID);

            try
            {
                return KMPlatform.DataAccess.ClientGroup.SelectForUser(userID).Exists(x => x.ClientGroupID == cgID);
            }
            catch
            {
                return false;
            }
        }

        public DataTable SelectUserForGrid(int clientID, int? ClientGroupID, int pageSize, int pageIndex, bool IncludePlatformAdmins, bool UserIsCAdmin, bool IncludeAllClients,bool IncludeBCAdmins, bool IsKMstaff , string searchText,bool ShowDisabledUsers, bool ShowDisabledUserRoles)
        {
            DataTable retList = new DataTable();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.User.SelectUserForGrid(clientID, ClientGroupID, pageSize, pageIndex, IncludePlatformAdmins, UserIsCAdmin, IncludeAllClients, IncludeBCAdmins, IsKMstaff, searchText, ShowDisabledUsers, ShowDisabledUserRoles);
                scope.Complete();
            }
            return retList;
        }

        public DataTable DownloadUserGrid(int clientID, int? ClientGroupID, bool IncludePlatformAdmins, bool UserIsCAdmin, bool IncludeAllClients, bool IncludeBCAdmins, bool IsKMstaff, string searchText, bool ShowDisabledUsers, bool ShowDisabledUserRoles)
        {
            DataTable retList = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.User.DownloadUserGrid(clientID, ClientGroupID, IncludePlatformAdmins, UserIsCAdmin, IncludeAllClients, IncludeBCAdmins, IsKMstaff, searchText, ShowDisabledUsers, ShowDisabledUserRoles);
                scope.Complete();
            }
            return retList;
        }

        #region ECN methods

        public Entity.User ECN_SetAuthorizedUserObjects(Entity.User user, int clientgroupID, int clientID)
        {
            ClientGroup cgWorker = new ClientGroup();
            UserClientSecurityGroupMap ucsgWorker = new UserClientSecurityGroupMap();
            Client cWorker = new Client();
            SecurityGroup sgWorker = new SecurityGroup();

            if (user.IsPlatformAdministrator)
                user.ClientGroups = cgWorker.Select(false);
            else
                user.ClientGroups = cgWorker.SelectForUserAuthorization(user.UserID, false);//done

            user.UserClientSecurityGroupMaps = ucsgWorker.SelectForUserAuthorization(user.UserID);//done
            user.CurrentClientGroup = cgWorker.Select(clientgroupID, true);//done
            user.CurrentClient = cWorker.ECN_Select(clientID, true);//done
            user.CurrentSecurityGroup = sgWorker.Select(user.UserID, clientID, true);//done

            return user;
        }

        public Entity.User ECN_SelectUser(int userID, bool includeObjects = false)
        {
            Entity.User x = DataAccess.User.SelectUser(userID);

            if (x == null)
            {
                return null;
            }

            DecryptPassword(x);
            if (includeObjects == true)
            {
                x = ECN_SetAuthorizedUserObjects(x, x.DefaultClientGroupID, x.DefaultClientID);
            }

            return x;
        }

        public static KMPlatform.Entity.User ECN_GetByAccessKey(string accessKey, bool getChildren = false)
        {
            return (new User()).ECN_LogIn(Guid.Parse(accessKey), getChildren);
        }

        public Entity.User ECN_LogIn(Guid accessKey, bool includeObjects = true)
        {
            Entity.User x = null;

            x = DataAccess.User.LogIn(accessKey);
            if (x != null)
                DecryptPassword(x);

            if (x != null && x.IsActive == false)
                x = null;

            if (x != null && includeObjects == true)
            {
                return SetAuthorizedUserObjects(x, x.DefaultClientGroupID, x.DefaultClientID);
            }
            else return x;
        }

        #endregion

        #region User Role Checks
        /// <summary>
        /// Tests that user is non-null and has ActiveFlag set to "Y"
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsActive(KMPlatform.Entity.User user)
        {
            if (user == null)
            {
                return false;
            }
            return user.IsActive;
            //return null != user && (user.ActiveFlag.ToUpper() ?? "") == "Y";
        }

        /// <summary>
        /// Tests that customer is non-null and has ActiveFlag set to "Y"
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// luke/Sunil commented this method - This creates circular reference

        //public static bool IsActive(ECN_Framework_Entities.Accounts.Customer customer)
        //{
        //    if (customer == null)
        //    {
        //        return false;
        //    }
        //    return (customer.ActiveFlag.ToUpper() ?? "") == "Y";
        //}

        /// <summary>
        /// True if user IsActive and the current security group is called "system administrator"; was: 
        /// True if the user has IsSystemAdmin set to true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsSystemAdministrator(KMPlatform.Entity.User user)
        {
            if (false == IsActive(user))
            {
                return false;
            }
            return user.IsPlatformAdministrator;
        }

        /// <summary>
        /// True if user IsSystemAdministrator or IsActive and the current security group is called "Base-Channel Administrator"; was: 
        /// True if the user is a System Administrator or has IsChannelAdmin set to true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsChannelAdministrator(KMPlatform.Entity.User user)
        {
            if (IsSystemAdministrator(user))
            {
                return true;
            }
            else if (false == IsActive(user))
            {
                return false;
            }
            return user.CurrentSecurityGroup.AdministrativeLevel.Equals(KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator);
        }

        /// <summary>
        /// True if user IsChannelAdministrator or IsActive and the current security group is called "Customer Administrator"; was: 
        /// True if the user is a System Administrator, a Channel Administrator or has IsAdmin is set to true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsAdministrator(KMPlatform.Entity.User user)
        {
            if (IsSystemAdministrator(user) || IsChannelAdministrator(user))
            {
                return true;
            }
            else if (false == IsActive(user))
            {
                return false;
            }
            return user.CurrentSecurityGroup.AdministrativeLevel.Equals(KMPlatform.Enums.SecurityGroupAdministrativeLevel.Administrator);
        }

        /// <summary>
        /// True if user is active and HasUserAccess is set to true
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool HasUserAdministrativePrivilege(KMPlatform.Entity.User user)
        {
            return IsActive(user) && IsAdministrator(user);
        }

        /// <summary>
        /// True if the user is System Administrator or Channel Administrator
        /// or is Administrator with User Administrative Privilege
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CanAdministrateUsers(KMPlatform.Entity.User user)
        {
            return IsChannelAdministrator(user) || (IsAdministrator(user) && HasUserAdministrativePrivilege(user));
        }

        public static bool HasService(KMPlatform.Entity.User user, KMPlatform.Enums.Services serviceCode)
        {
            try
            {
                if (false == IsActive(user))
                {
                    return false;
                }
                else if (user.CurrentClient.Services == null)
                {
                    return false;
                }
                else if (!user.CurrentClient.Services.Any(x => x.ServiceCode.Equals(serviceCode.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    return false;
                }
                else if (IsSystemAdministrator(user) || IsChannelAdministrator(user) || IsAdministrator(user))
                {
                    return true;
                }
                else if (user.CurrentSecurityGroup.Services == null)
                {
                    return false;
                }
                return user.CurrentSecurityGroup.Services.Any(a => a.ServiceCode.Equals(serviceCode.ToString(), StringComparison.InvariantCultureIgnoreCase));
            }
            catch
            {
                return false;
            }
        }

        public static bool HasServiceFeature(KMPlatform.Entity.User user, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures servicefeatureCode)
        {
            try
            {
                if (false == IsActive(user))
                {
                    return false;
                }
                else if (HasService(user, serviceCode))
                {
                    KMPlatform.Entity.Service s = user.CurrentClient.Services.Find(a => a.ServiceCode.Equals(serviceCode.ToString(), StringComparison.InvariantCultureIgnoreCase));

                    if (s == null)
                        return false;

                    if (!s.ServiceFeatures.Any(x => x.SFCode.Equals(servicefeatureCode.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                    {
                        return false;
                    }
                    else if (IsSystemAdministrator(user) || IsChannelAdministrator(user) || IsAdministrator(user))
                    {
                        return true;
                    }

                    Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures> pk = Tuple.Create(serviceCode, servicefeatureCode);

                    return user.CurrentSecurityGroup.Permissions.ContainsKey(pk);
                }

                return false;
            }
            catch
            {
                return false;
            }

        }


        public static bool HasAccess(KMPlatform.Entity.User user, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures servicefeatureCode, KMPlatform.Enums.Access accessCode)
        {
            try
            {
                if (HasServiceFeature(user, serviceCode, servicefeatureCode))
                {
                    if (IsSystemAdministrator(user) || IsChannelAdministrator(user) || IsAdministrator(user))
                    {
                        return true;
                    }

                    Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures> pk = Tuple.Create(serviceCode, servicefeatureCode);

                    if (false == user.CurrentSecurityGroup.Permissions.ContainsKey(pk))
                    {
                        return false;
                    }
                    return user.CurrentSecurityGroup.Permissions[pk].Any(x => x.Equals(accessCode));

                }

                return false;
            }
            catch 
            {
                return false;
            }

        }

        #endregion

        #region Reset password
        public string ResetPasswordHTML()
        {
            return @"<!DOCTYPE HTML PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN'>
<html>
   <head>
      <title>Reset My Password</title>
   </head>
   <body style='width:100%;'>
      <table style='width:600px;margin-left:auto;margin-right:auto;'>
				 <tr>
				 <td style='text-align:center;'>
				 		 <img src='http://images.ecn5.com/KMNew/KMNewWebLogo.jpg' alt='KMLogo' />
				 </td>
				 </tr>
         <tr>
            <td style='padding-bottom:10px;padding-top:10px;'>
               <p>Dear %%FirstName%%</p>
            </td>
         </tr>
         <tr>
            <td style='padding-bottom:10px;'>
               <p>As you requested, below is your temporary password which will allow you to access your KM Platform account.</p>
            </td>
         </tr>
         <tr>
            <td style='padding-bottom:10px;'>
               <p><b>Temporary password: %%TempPassword%%</b></p>
            </td>
         </tr>
         <tr>
            <td style='padding-bottom:10px;'>
               <p>Please click the link below to reset your password using the temporary password provided.</p>
            </td>
         </tr>
         <tr>
            <td style='text-align:center;'>
               <div id='redirectbutton' style='margin-right:auto;margin-left:auto;margin-top:10px;margin-bottom:10px;width:200px;text-align:center; border-radius:8px; background-color: #FFFFFF;  moz-border-radius: 8px;  -webkit-border-radius: 8px;  border: 2px solid #000000;  '>
                  <p style='width:200px;height:100%;'><a href='%%RedirectLink%%' style='color:black;text-decoration:none;width:100%;height:100%;'>Reset My Password</a></p>
               </div>
            </td>
         </tr>
         <tr>
            <td>
               <p>If you did not request a temporary password and you believe someone attempted to access your account information, please contact us immediately at 1.866.844.6275.</p>
            </td>
         </tr>
      </table>
   </body>
</html>";
        }

        public string GenerateTempPassword()
        {

            int length = 8;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }
        #endregion
    }
}

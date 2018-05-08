using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace KMPlatform.BusinessLogic
{
    public class UserAuthorization
    {
        /// <summary>
        /// returns only items that are enable/active for the user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="ipAddress"></param>
        /// <param name="serverVariables"></param>
        /// <returns></returns>
        public Object.UserAuthorization Login(string userName, string password, string saltValue, string authSource, string ipAddress = "", Object.ServerVariable serverVariables = null, string appVersion = "", bool useLite = true)
        {
            Object.UserAuthorization retItem = new Object.UserAuthorization();
            retItem.AuthAccessKey = Guid.Empty;
            retItem.AuthAttemptDate = DateTime.Now;
            retItem.AuthAttemptTime = DateTime.Now.TimeOfDay;
            retItem.AuthorizationMode = Enums.AuthorizationModeTypes.User_Name_Password;
            retItem.AuthSource = authSource;
            retItem.AuthPassword = password;
            retItem.AuthUserName = userName;
            retItem.IpAddress = ipAddress;
            retItem.IsAuthenticated = false;

            if (serverVariables != null)
            {
                //retItem.ServerVariables = serverVariables;
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = serverVariables.REMOTE_ADDR;
                    retItem.IpAddress = serverVariables.REMOTE_ADDR;
                }
            }
            else
            {
                ServerVariable svWorker = new ServerVariable();
                serverVariables = svWorker.GetServerVariables();
            }

            if (string.IsNullOrEmpty(ipAddress))
            {
                IPAddress host = IPAddress.None;
                foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    host = ip;
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        break;
                }

                int address = BitConverter.ToInt32(host.GetAddressBytes(), 0);
                retItem.IpAddress = address.ToString();
            }

            Object.Encryption ec = new Object.Encryption();
            ec.EncryptedText = password;
            ec.SaltValue = saltValue;
            BusinessLogic.Encryption e = new Encryption();
            ec = e.Decrypt(ec);

            User uWorker = new KMPlatform.BusinessLogic.User();
            if (useLite == true)
                retItem.User = uWorker.LogInLite(userName, ec.PlainText, true);
            else
                retItem.User = uWorker.LogIn(userName, ec.PlainText, true);
            if (retItem.User == null || retItem.User.IsActive == false)
            {
                retItem.User = null;
                retItem.IsAuthenticated = false;
                retItem.IsKmStaff = false;
            }

            if (retItem.User != null && retItem.User.IsActive == true)
            {
                retItem.IsAuthenticated = true;
                retItem.AuthAccessKey = retItem.User.AccessKey;
                //retItem.IsKmStaff = retItem.User.ClientGroups.Exists(x => x.Clients.Exists(c => c.ClientName.Equals("Knowledge Marketing") == true));
            }

            //log the attempt
            UserAuthorizationLog ualWorker = new UserAuthorizationLog();
            Entity.UserAuthorizationLog ual = new Entity.UserAuthorizationLog();
            ual.AuthAccessKey = null;
            ual.AuthAttemptDate = retItem.AuthAttemptDate;
            ual.AuthAttemptTime = retItem.AuthAttemptTime;
            ual.AuthMode = retItem.AuthorizationMode;
            ual.AuthSource = authSource;
            ual.AuthUserName = retItem.AuthUserName;
            ual.IpAddress = retItem.IpAddress;
            ual.IsAuthenticated = retItem.IsAuthenticated;
            ual.LogOutDate = null;
            ual.LogOutTime = null;
            Core_AMS.Utilities.JsonFunctions jfunct = new Core_AMS.Utilities.JsonFunctions();
            ual.ServerVariables = jfunct.ToJson<Object.ServerVariable>(serverVariables);
            ual.AppVersion = appVersion;
            if (retItem.User != null)
                ual.UserID = retItem.User.UserID;

            ual.UserAuthLogID = ualWorker.Save(ual);

            //retItem.ServerVariables = null;
            //retItem.UserAuthLog = null;//ual;
            retItem.UserAuthLogId = ual.UserAuthLogID;
            return retItem;
        }
        /// <summary>
        /// returns only items that are enable/active for the user
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="ipAddress"></param>
        /// <param name="serverVariables"></param>
        /// <returns></returns>
        public Object.UserAuthorization Login(Guid accessKey, string authSource, string ipAddress = "", Object.ServerVariable serverVariables = null, string appVersion = "")
        {
            Object.UserAuthorization retItem = new Object.UserAuthorization();
            retItem.AuthAccessKey = accessKey;
            retItem.AuthAttemptDate = DateTime.Now;
            retItem.AuthAttemptTime = DateTime.Now.TimeOfDay;
            retItem.AuthorizationMode = Enums.AuthorizationModeTypes.Access_Key;
            retItem.AuthPassword = string.Empty;
            retItem.AuthUserName = string.Empty;
            retItem.IpAddress = ipAddress;
            retItem.IsAuthenticated = false;
            if (serverVariables != null)
            {
                //retItem.ServerVariables = serverVariables;
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = serverVariables.REMOTE_ADDR;
                    retItem.IpAddress = serverVariables.REMOTE_ADDR;
                }
            }
            else
            {
                ServerVariable svWorker = new ServerVariable();
                serverVariables = svWorker.GetServerVariables();
            }

            if (string.IsNullOrEmpty(ipAddress))
            {
                IPAddress host = IPAddress.None;
                foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    host = ip;
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        break;
                }

                int address = BitConverter.ToInt32(host.GetAddressBytes(), 0);
                retItem.IpAddress = address.ToString();
            }

            User uWorker = new User();
            retItem.User = uWorker.LogIn(accessKey, true);
            if (retItem.User != null)
            {
                retItem.IsAuthenticated = true;
                retItem.AuthPassword = retItem.User.Password;
                retItem.AuthUserName = retItem.User.UserName;
            }

            //log the attempt
            UserAuthorizationLog ualWorker = new UserAuthorizationLog();
            Entity.UserAuthorizationLog ual = new Entity.UserAuthorizationLog();
            ual.AuthAccessKey = retItem.AuthAccessKey;
            ual.AuthAttemptDate = retItem.AuthAttemptDate;
            ual.AuthAttemptTime = retItem.AuthAttemptTime;
            ual.AuthMode = retItem.AuthorizationMode;
            ual.AuthSource = authSource;
            ual.AuthUserName = retItem.AuthUserName;
            ual.IpAddress = retItem.IpAddress;
            ual.IsAuthenticated = retItem.IsAuthenticated;
            ual.LogOutDate = null;
            ual.LogOutTime = null;
            Core_AMS.Utilities.JsonFunctions jfunct = new Core_AMS.Utilities.JsonFunctions();
            ual.ServerVariables = jfunct.ToJson<Object.ServerVariable>(serverVariables);
            ual.AppVersion = appVersion;
            if (retItem.User != null)
                ual.UserID = retItem.User.UserID;

            ual.UserAuthLogID = ualWorker.Save(ual);

            retItem.UserAuthLogId = ual.UserAuthLogID;

            return retItem;
        }
        public string GetIpAddress()
        {
            string ipAddress = string.Empty;
            IPAddress host = IPAddress.None;
            foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                host = ip;
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    break;
            }

            int address = BitConverter.ToInt32(host.GetAddressBytes(), 0);
            ipAddress = address.ToString();

            return ipAddress;
        }
    }
}

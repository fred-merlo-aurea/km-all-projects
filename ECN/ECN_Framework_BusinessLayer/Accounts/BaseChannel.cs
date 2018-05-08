using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class BaseChannel
    {
        private static readonly string CacheName = "CACHE_BASECHANNELS";
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BaseChannel;

        public static bool Exists(int baseChannelID)
        {
            return ECN_Framework_DataLayer.Accounts.BaseChannel.Exists(baseChannelID);
        }

        public static bool Exists(string Name, int baseChannelID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Accounts.BaseChannel.Exists(Name, baseChannelID);
                scope.Complete();
            }
            return exists;
        }

        public static string GetHeaderSource(string headerSource)
        {
            return headerSource.Replace("%%MainTitle%% ", "");
        }

        public static List<ECN_Framework_Entities.Accounts.BaseChannel> GetAll()
        {
            List<ECN_Framework_Entities.Accounts.BaseChannel> channelList = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                channelList = ECN_Framework_DataLayer.Accounts.BaseChannel.GetAll();
                scope.Complete();
            }

            return channelList;
        }


        public static ECN_Framework_Entities.Accounts.BaseChannel GetByBaseChannelID(int baseChannelID)
        {
            ECN_Framework_Entities.Accounts.BaseChannel basechannel = GetAll().Find(x => x.BaseChannelID == baseChannelID);
            //basechannel.Channels = ECN_Framework_BusinessLayer.Accounts.Channel.GetByBaseChannelID(basechannel.BaseChannelID);

            return basechannel;
        }

        public static ECN_Framework_Entities.Accounts.BaseChannel GetByPlatformClientGroupID(int ClientGroupID)
        {
            ECN_Framework_Entities.Accounts.BaseChannel basechannel = GetAll().Find(x => x.PlatformClientGroupID == ClientGroupID);
            return basechannel;
        }


        public static void Validate(ECN_Framework_Entities.Accounts.BaseChannel basechannel, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user) && !KM.Platform.User.IsChannelAdministrator(user))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            if (basechannel.BaseChannelID <= 0 && basechannel.CreatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (basechannel.BaseChannelID > 0 && basechannel.UpdatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (string.IsNullOrWhiteSpace(basechannel.BaseChannelName))
                errorList.Add(new ECNError(Entity, Method, "Base Channel Name is missing"));
            else if (Exists(basechannel.BaseChannelName, basechannel.BaseChannelID))
                errorList.Add(new ECNError(Entity, Method, "Base Channel Name already exists"));

            if (string.IsNullOrWhiteSpace(basechannel.ContactName))
                errorList.Add(new ECNError(Entity, Method, "Contact Name is missing"));

            if (string.IsNullOrWhiteSpace(basechannel.ContactTitle))
                errorList.Add(new ECNError(Entity, Method, "Title is missing"));

            if (string.IsNullOrWhiteSpace(basechannel.Phone))
                errorList.Add(new ECNError(Entity, Method, "Phone is missing"));

            //if (string.IsNullOrWhiteSpace(basechannel.Fax))
            //    errorList.Add(new ECNError(Entity, Method, "Fax is missing"));

            if (string.IsNullOrWhiteSpace(basechannel.Email))
                errorList.Add(new ECNError(Entity, Method, "Email is missing"));

            if (string.IsNullOrWhiteSpace(basechannel.Address))
                errorList.Add(new ECNError(Entity, Method, "Address is missing"));

            if (string.IsNullOrWhiteSpace(basechannel.City))
                errorList.Add(new ECNError(Entity, Method, "City is missing"));

            if (string.IsNullOrWhiteSpace(basechannel.Zip))
                errorList.Add(new ECNError(Entity, Method, "Zip is missing"));

            if (string.IsNullOrWhiteSpace(basechannel.Country))
                errorList.Add(new ECNError(Entity, Method, "Country is missing"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Accounts.BaseChannel basechannel, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Functions.CacheHelper.ClearCache(CacheName);

            Validate(basechannel, user);

            using (TransactionScope scope = new TransactionScope())
            {

                basechannel.BaseChannelID = ECN_Framework_DataLayer.Accounts.BaseChannel.Save(basechannel);

                //if (basechannel.Channels != null)
                //{
                //    List<ECN_Framework_Entities.Accounts.Channel> itemListCopy = new List<ECN_Framework_Entities.Accounts.Channel>();
                //    foreach (ECN_Framework_Entities.Accounts.Channel channel in basechannel.Channels)
                //    {
                //        channel.BaseChannelID = basechannel.BaseChannelID;
                //        ECN_Framework_Entities.Accounts.Channel itemCopy = channel;
                //        itemCopy.UpdatedUserID = basechannel.UpdatedUserID;
                //        Channel.Save(itemCopy, user);
                //        itemListCopy.Add(itemCopy);
                //    }
                //    basechannel.Channels = itemListCopy;
                //}

                scope.Complete();
            }
        }

        public static void Delete(int BaseChannelID, KMPlatform.Entity.User user)
        {
            if (!KM.Platform.User.IsSystemAdministrator(user))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            ECN_Framework_Common.Functions.CacheHelper.ClearCache(CacheName);
            ECN_Framework_DataLayer.Accounts.BaseChannel.Delete(BaseChannelID, user.UserID);
        }


        public static ECN_Framework_Entities.Accounts.BaseChannel GetByDomain(string SubDomain)
        {
            ECN_Framework_Entities.Accounts.BaseChannel BaseChannel = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                BaseChannel = ECN_Framework_DataLayer.Accounts.BaseChannel.GetByDomain(SubDomain);
                scope.Complete();
            }
            return BaseChannel;
        }

        public static bool HasProductFeature(int baseChannelID, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures servicefeatureCode)
        {
            ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(baseChannelID);

            return KMPlatform.BusinessLogic.ClientGroup.HasServiceFeature(bc.PlatformClientGroupID, serviceCode, servicefeatureCode);

        }

    }
}

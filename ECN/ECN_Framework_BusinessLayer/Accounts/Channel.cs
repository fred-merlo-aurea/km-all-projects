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
    public class Channel
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Channel;

        public static List<ECN_Framework_Entities.Accounts.Channel> GetAll()
        {
            return ECN_Framework_DataLayer.Accounts.Channel.GetAll();
        }

        public static ECN_Framework_Entities.Accounts.Channel GetByProductTypeAndID(ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode productTypeCode, int baseChannelID)
        {
            ECN_Framework_Entities.Accounts.Channel channel = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                channel = ECN_Framework_DataLayer.Accounts.Channel.GetByProductTypeAndID(productTypeCode, baseChannelID);
                scope.Complete();
            }

            return channel;
        }

        public static List<ECN_Framework_Entities.Accounts.Channel> GetByBaseChannelID(int baseChannelID)
        {
            List<ECN_Framework_Entities.Accounts.Channel> lchannel = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lchannel = ECN_Framework_DataLayer.Accounts.Channel.GetByBaseChannelID(baseChannelID);
                scope.Complete();
            }

            return lchannel;
        }

        public static ECN_Framework_Entities.Accounts.Channel GetByChannelID(int ChannelID)
        {
            ECN_Framework_Entities.Accounts.Channel channel = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                channel = ECN_Framework_DataLayer.Accounts.Channel.GetByChannelID(ChannelID);
                scope.Complete();
            }

            return channel;
        }

        public static void Validate(ECN_Framework_Entities.Accounts.Channel channel, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!KM.Platform.User.IsSystemAdministrator(user) && !KM.Platform.User.IsChannelAdministrator(user))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            if (channel.ChannelID <= 0 && channel.CreatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "CreatedUserID is invalid"));

            if (channel.ChannelID > 0 && channel.UpdatedUserID == null)
                errorList.Add(new ECNError(Entity, Method, "UpdatedUserID is invalid"));

            if (string.IsNullOrWhiteSpace(channel.ChannelName))
                errorList.Add(new ECNError(Entity, Method, "Channel Name is missing"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }

        }

        public static void Save(ECN_Framework_Entities.Accounts.Channel channel, KMPlatform.Entity.User user)
        {
            Validate(channel, user);
            ECN_Framework_DataLayer.Accounts.Channel.Save(channel);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ECN_Framework_Entities.Accounts;

using EmailMarketing.Site.Infrastructure.Abstract;

namespace EmailMarketing.Site.Infrastructure.Concrete
{
    public class BaseChannelBusinessLayerProvider : IBaseChannelProvider
    {
        private Dictionary<int, BaseChannel> baseChannelIdMap = 
            new Dictionary<int, BaseChannel>();
        private Dictionary<string, BaseChannel> baseChannelDomainMap = 
            new Dictionary<string, BaseChannel>();
        public BaseChannel GetBaseChannelForDomainWithDefault(string domainHost, int defaultBaseChannelId)
        {
            BaseChannel bc = GetBaseChannelByDomain(domainHost);
            if (bc != null)
            {
                return bc;
            }

            return GetBaseChannelById(defaultBaseChannelId);
        }

        protected BaseChannel GetBaseChannelByDomain(string domainHost)
        {
            if(baseChannelDomainMap.ContainsKey(domainHost))
            {
                return baseChannelDomainMap[domainHost];
            }
            else
            {
                BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByDomain(domainHost);
                if(null != bc)
                {
                    baseChannelDomainMap.Add(domainHost, bc);
                    return bc;
                }
            }

            return null;
        }

        protected BaseChannel GetBaseChannelById(int baseChannelId)
        {
            if (baseChannelIdMap.ContainsKey(baseChannelId))
            {
                return baseChannelIdMap[baseChannelId];
            }
            else
            {
                BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(baseChannelId);
                if (null != bc)
                {
                    baseChannelIdMap.Add(baseChannelId, bc);
                    return bc;
                }
            }

            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMarketing.Site.Infrastructure.Abstract
{
    public interface IBaseChannelProvider
    {
        ECN_Framework_Entities.Accounts.BaseChannel GetBaseChannelForDomainWithDefault(string domainHost, int defaultBaseChannelId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMarketing.Site.Infrastructure.Abstract.Settings
{
    /*
        <add key="Accounts_VirtualPath" value="/ecn.accounts" />
        <add key="Communicator_VirtualPath" value="/ecn.communicator" />
        <add key="Creator_VirtualPath" value="/ecn.creator" />
        <add key="Publisher_VirtualPath" value="/ecn.publisher" />
        <add key="Collector_VirtualPath" value="/ecn.collector" />
        <add key="Images_VirtualPath" value="/ecn.images" />
     */
    public interface IPathProvider
    {
        //string Accounts();
        //string Communicator();
        //string Creator();
        //string Publisher();
        //string Collector();
        string Images();
    }
}

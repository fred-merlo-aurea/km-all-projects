using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMarketing.Site.Infrastructure.Abstract.Settings
{
    /*
        <add key="ECN_Login_Redirect" value="/ecn.accounts/login.aspx" />
        <add key="ECN_Home_Redirect" value="/ecn.accounts/default.aspx" />
        <add key="ECN_Logoff_Redirect" value="/ecn.accounts/login.aspx" />
     */
    public interface IRedirectProvider
    {
        string Login();
        string Home();
        string Logoff();
        string Reset();

        IRedirectProvider ForBaseChannel(int baseChannelId);
        IRedirectProvider ForBaseChannelCustomer(int baseChannelId, int customerId);
    }
}

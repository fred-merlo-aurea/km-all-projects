using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

using EmailMarketing.Site.Infrastructure.Abstract.Settings;

namespace EmailMarketing.Site.Infrastructure.Concrete.Settings
{
    public class RedirectConfigurationProvider : ConfigurationSection , IRedirectProvider
    {
        public static RedirectConfigurationElement GetSection()
        {
            return (RedirectConfigurationElement)System.Configuration.ConfigurationManager.GetSection("redirect");
        }

        [ConfigurationProperty("login", IsRequired = true)]
        public RedirectConfigurationElement Login
        {
            get
            {
                return (RedirectConfigurationElement)this["login"];
            }
            set
            {
                this["login"] = value;
            }
        }

        [ConfigurationProperty("reset", IsRequired = true)]
        public RedirectConfigurationElement Reset
        {
            get
            {
                return (RedirectConfigurationElement)this["reset"];
            }
            set
            {
                this["reset"] = value;
            }
        }

        string IRedirectProvider.Reset()
        {
            return Reset.Reset;
        }


        string IRedirectProvider.Login()
        {
            return Login.Login;
        }

        string IRedirectProvider.Home()
        {
            throw new NotImplementedException();
        }

        string IRedirectProvider.Logoff()
        {
            throw new NotImplementedException();
        }

        IRedirectProvider IRedirectProvider.ForBaseChannel(int baseChannelId)
        {
            throw new NotImplementedException();
        }

        IRedirectProvider IRedirectProvider.ForBaseChannelCustomer(int baseChannelId, int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
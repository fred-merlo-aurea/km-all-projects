using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

using EmailMarketing.Site.Infrastructure.Abstract.Settings;

namespace EmailMarketing.Site.Infrastructure.Concrete.Settings
{
    public class RedirectConfgurationSection : ConfigurationElement, IRedirectProvider
    {
        [ConfigurationProperty("login", IsRequired = true)]
        public string LoginProperty
        {
            get { return (string)this["login"]; }
            set { this["login"] = value; }
        }

        [ConfigurationProperty("reset", IsRequired = true)]
        public string ResetProperty
        {
            get { return (string)this["reset"]; }
            set { this["reset"] = value; }
        }


        public string Login()
        {
            throw new NotImplementedException();
        }

        public string Home()
        {
            throw new NotImplementedException();
        }

        public string Logoff()
        {
            throw new NotImplementedException();
        }

        public string Reset()
        {
            throw new NotImplementedException();
        }

        public IRedirectProvider ForBaseChannel(int baseChannelId)
        {
            throw new NotImplementedException();
        }

        public IRedirectProvider ForBaseChannelCustomer(int baseChannelId, int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EmailMarketing.Site.Infrastructure.Concrete.Settings
{
    public class RedirectConfigurationElement :ConfigurationElement
    {
        [ConfigurationProperty("login", IsRequired = true)]
        public string Login
        {
            get { return (string)this["login"]; }
            set { this["login"] = value; }
        }

        [ConfigurationProperty("home", IsRequired = true)]
        public string Home
        {
            get { return (string)this["home"]; }
            set { this["home"] = value; }
        }

        [ConfigurationProperty("logoff", IsRequired = true)]
        public string Logoff
        {
            get { return (string)this["logoff"]; }
            set { this["logoff"] = value; }
        }

        [ConfigurationProperty("reset",IsRequired=true)]
        public string Reset
        {
            get { return (string)this["reset"]; }
            set { this["reset"] = value; }
        }
    }
}
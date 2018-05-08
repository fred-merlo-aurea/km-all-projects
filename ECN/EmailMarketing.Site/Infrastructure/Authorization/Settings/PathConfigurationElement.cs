using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EmailMarketing.Site.Infrastructure.Concrete.Settings
{
    public class PathConfigurationElement: ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired=true)]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }
    }
}
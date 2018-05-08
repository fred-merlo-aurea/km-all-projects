using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using EmailMarketing.Site.Infrastructure.Abstract.Settings;
using EmailMarketing.Site.Infrastructure.Concrete.Settings;

namespace EmailMarketing.Site.Infrastructure.Concrete.Settings
{
    public class PathConfigurationProvider : ConfigurationSection, IPathProvider
    {
        public static PathConfigurationProvider GetSection()
        {
            return (PathConfigurationProvider)System.Configuration.ConfigurationManager.GetSection("virtualPathGroup/virtualPath");
        }

        [ConfigurationProperty("images", IsRequired=true)]
        public PathConfigurationElement Images
        {
            get
            {
                return (PathConfigurationElement)this["images"];
            }
            set
            {
                this["images"] = value;
            }
        }

        string IPathProvider.Images()
        {
            return GetSection().Images.Path;
        }
    }
}
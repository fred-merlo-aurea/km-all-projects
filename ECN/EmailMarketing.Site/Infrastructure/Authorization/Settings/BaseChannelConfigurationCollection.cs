using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EmailMarketing.Site.Infrastructure.Concrete.Settings
{
    public class BaseChannelConfigurationCollection : ConfigurationElement
    {
        [ConfigurationProperty("id")]
        public int BaseChannelId
        {
            get {  return (int)base["id"]; }
        }

        [ConfigurationCollection(typeof(ConfigurationSectionCollection),AddItemName="foo")]
        public ConfigurationSectionCollection Foo
        {
            get { return (ConfigurationSectionCollection)base["foo"]; }
        }
    }
}
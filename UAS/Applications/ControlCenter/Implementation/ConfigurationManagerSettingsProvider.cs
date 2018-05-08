using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlCenter.Interfaces;

namespace ControlCenter.Implementation
{
    public class ConfigurationManagerSettingsProvider: IAppSettingsProvider
    {
        public string GetAppSettingsValue(string appSettingskey)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return config.AppSettings.Settings[appSettingskey].Value;
        }
    }
}

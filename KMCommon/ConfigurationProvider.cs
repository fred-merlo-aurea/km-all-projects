using System.ComponentModel;
using System.Configuration;
using System.Globalization;

namespace KM.Common
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public T GetValue<T>(string configurationKey)
        {
            Guard.NotNullOrWhitespace(configurationKey, nameof(configurationKey));

            var configuredValue = ConfigurationManager.AppSettings[configurationKey];
            if (string.IsNullOrWhiteSpace(configuredValue))
            {
                return default(T);
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T) converter.ConvertFromString(null, CultureInfo.InvariantCulture, configuredValue);
        }
    }
}
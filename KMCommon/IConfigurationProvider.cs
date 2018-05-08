namespace KM.Common
{
    /// <summary>
    /// Provides access to configuration values
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Get a configuration Value based on a configuration Key
        /// </summary>
        /// <typeparam name="T">The type of the Value being retrieved</typeparam>
        /// <param name="configurationKey">The configuration key for the Value</param>
        /// <returns>The configured Value automatically cast as the specified type (T).</returns>
        T GetValue<T>(string configurationKey);
    }
}
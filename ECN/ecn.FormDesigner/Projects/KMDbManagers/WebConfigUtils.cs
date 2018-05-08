using System.Configuration;
using System.IO;
using System.Web.Configuration;

namespace KMDbManagers
{
    static public class WebConfigUtils
    {
        static public string KMDesignerRoot()
        {
            var path = WebConfigurationManager.AppSettings["KMRoot_Path"];
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new SettingsPropertyNotFoundException("KMRoot_Path | Not set.");
            }
            if (!Directory.Exists(path))
            {
                throw new FileNotFoundException("KMRoot_Path | invalid path: " + path);
            }
            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }
            return path;
        }
    }
}

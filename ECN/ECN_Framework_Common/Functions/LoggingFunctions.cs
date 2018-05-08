using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ECN_Framework_Common.Functions
{
    public class LoggingFunctions
    {
        public static bool LogStatistics()
        {
            bool logStatistics = false;
            try
            {
                logStatistics = Convert.ToBoolean(ConfigurationManager.AppSettings["LogStatistics"]);
            }
            catch (Exception) { }
            return logStatistics;
        }
    }
}

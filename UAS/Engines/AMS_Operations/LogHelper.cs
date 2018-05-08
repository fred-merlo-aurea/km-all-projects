using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;

namespace AMS_Operations
{
    internal class LogHelper
    {
        public static void LogError(Exception ex, string method, string className)
        {
            var alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
            var formatException = StringFunctions.FormatException(ex);
            alWorker.LogCriticalError(
                formatException,
                string.IsNullOrWhiteSpace(className) ? method : $"{className}.{method}" ,
                KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations);
        }
    }
}

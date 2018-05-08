using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KM.Common.Utilities.Logging
{
    public static class StreamLogger
    {
        public static void LogAndFlushWriter(StreamWriter streamWriter, string textToLog)
        {
            Guard.NotNull(streamWriter, nameof(streamWriter));

            streamWriter.WriteLine("{0} {1}", DateTime.Now, textToLog);
            streamWriter.Flush();
        }
    }
}

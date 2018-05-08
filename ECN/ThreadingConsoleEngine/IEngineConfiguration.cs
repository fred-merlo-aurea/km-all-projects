using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_EngineFramework
{
    public interface IEngineConfiguration
    {
        bool TestMode { get; }
        string LogFile { get; }
        int MaxConcurrentThreads { get; }

        int MaxIterations { get; }

        int MaxRetriesPerOperation { get; }
    }
}

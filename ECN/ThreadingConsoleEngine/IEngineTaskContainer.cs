using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECN_EngineFramework;

namespace ECN_EngineFramework
{
    public interface IEngineTaskContainer
    {
        Task<EngineActionResult> Task { get; set; }
    }
}

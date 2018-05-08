using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlCenter.Interfaces
{
    public interface IAppSettingsProvider
    {
        string GetAppSettingsValue(string appSettingskey);
    }
}

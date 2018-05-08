using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KM.Platform.User.Mock
{
    public interface IPlatformUserMenuItem
    {
        string DisplayName { get; }
        string Href { get; }
    }
}

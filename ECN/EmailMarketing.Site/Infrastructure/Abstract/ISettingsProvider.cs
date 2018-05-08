using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMarketing.Site.Infrastructure.Abstract
{
    interface ISettingsProvider
    {
        ISettingsProvider GetSettings();
    }
}

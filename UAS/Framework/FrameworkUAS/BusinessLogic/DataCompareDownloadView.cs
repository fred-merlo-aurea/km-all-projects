using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareDownloadView
    {
        public List<Entity.DataCompareDownloadView> SelectForClient(int clientId)
        {
            List<Entity.DataCompareDownloadView> x = null;
            x = DataAccess.DataCompareDownloadView.SelectForClient(clientId);

            return x;
        }
    }
}

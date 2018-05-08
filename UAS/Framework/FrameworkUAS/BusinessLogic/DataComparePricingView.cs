using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAS.BusinessLogic
{
    public class DataComparePricingView
    {
        public List<Entity.DataComparePricingView> SelectForClient(int clientId)
        {
            List<Entity.DataComparePricingView> x = null;
            x = DataAccess.DataComparePricingView.SelectForClient(clientId);
           
            return x;
        }
    }
}

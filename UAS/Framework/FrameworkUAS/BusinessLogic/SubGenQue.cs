using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAS.BusinessLogic
{
    public class SubGenQue
    {
        public List<Entity.SubGenQue> SelectClientId(int clientID, bool? isProcessed = null)
        {
            List<Entity.SubGenQue> retItem = null;
            retItem = DataAccess.SubGenQue.SelectClientId(clientID, isProcessed);
            return retItem;
        }
        public List<Entity.SubGenQue> SelectClientId(int clientID, DateTime createdDate)
        {
            List<Entity.SubGenQue> retItem = null;
            retItem = DataAccess.SubGenQue.SelectClientId(clientID, createdDate);
            return retItem;
        }
        public List<Entity.SubGenQue> SelectProductId(int productId, bool? isProcessed = null)
        {
            List<Entity.SubGenQue> retItem = null;
            retItem = DataAccess.SubGenQue.SelectProductId(productId, isProcessed);
            return retItem;
        }
        public List<Entity.SubGenQue> SelectProductId(int productId, DateTime createdDate)
        {
            List<Entity.SubGenQue> retItem = null;
            retItem = DataAccess.SubGenQue.SelectProductId(productId, createdDate);
            return retItem;
        }
        public int Save(Entity.SubGenQue x)
        {
            return DataAccess.SubGenQue.Save(x);
        }
        public bool SetIsProcessedTrue(int subGenQueId)
        {
            return DataAccess.SubGenQue.SetIsProcessedTrue(subGenQueId);
        }
    }
}

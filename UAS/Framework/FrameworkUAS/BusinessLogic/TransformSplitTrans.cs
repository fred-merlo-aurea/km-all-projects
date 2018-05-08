using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class TransformSplitTrans
    {
        public List<Entity.TransformSplitTrans> Select()
        {
            List<Entity.TransformSplitTrans> x = null;
            x = DataAccess.TransformSplitTrans.Select().ToList();

            return x;
        }
        public List<Entity.TransformSplitTrans> SelectSourceFileID(int sourceFileID)
        {
            List<Entity.TransformSplitTrans> x = null;
            x = DataAccess.TransformSplitTrans.SelectSourceFileID(sourceFileID).ToList();

            return x;
        }
        public List<Entity.TransformSplitTrans> Select(int TransformationID)
        {
            List<Entity.TransformSplitTrans> x = null;
            x = DataAccess.TransformSplitTrans.Select(TransformationID).ToList();
 
            return x;
        }

        public int Save(Entity.TransformSplitTrans x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.TransformationID = DataAccess.TransformSplitTrans.Save(x);
                scope.Complete();
            }

            return x.TransformationID;
        }
    }
}

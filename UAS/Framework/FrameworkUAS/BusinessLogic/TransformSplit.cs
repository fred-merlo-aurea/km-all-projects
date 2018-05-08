using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class TransformSplit
    {
        public  List<Entity.TransformSplit> Select()
        {
            List<Entity.TransformSplit> x = null;
            x = DataAccess.TransformSplit.Select().ToList();

            return x;
        }
        public  List<Entity.TransformSplit> SelectSourceFileID(int sourceFileID)
        {
            List<Entity.TransformSplit> x = null;
            x = DataAccess.TransformSplit.SelectSourceFileID(sourceFileID).ToList();

            return x;
        }
        public  List<Entity.TransformSplit> Select(int TransformationID)
        {
            List<Entity.TransformSplit> x = null;
            x = DataAccess.TransformSplit.Select(TransformationID).ToList();

            return x;
        }

        public  int Save(Entity.TransformSplit x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.TransformationID = DataAccess.TransformSplit.Save(x);
                scope.Complete();
            }

            return x.TransformationID;
        }

        #region Object.
        public List<Object.TransformSplitInfo> SelectObject(int sourceFileID)
        {
            List<Object.TransformSplitInfo> x = null;
            x = DataAccess.TransformSplit.SelectObject(sourceFileID);
            return x;
        }
        #endregion
    }
}

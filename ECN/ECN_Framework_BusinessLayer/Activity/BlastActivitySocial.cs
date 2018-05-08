using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Activity
{
    [Serializable]
    public class BlastActivitySocial
    {
        public static ECN_Framework_Entities.Activity.BlastActivitySocial GetByBlastSocialID(int socialID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivitySocial.GetByBlastSocialID(socialID);
        }

        public static int Insert(ECN_Framework_Entities.Activity.BlastActivitySocial social)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivitySocial.Insert(social);
        }

        public static bool FBHasBeenSharedAlready(int blastid)
        {
            bool hasBeenShared = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                hasBeenShared = ECN_Framework_DataLayer.Activity.BlastActivitySocial.FBHasBeenSharedAlready(blastid);
                scope.Complete();
            }
            return hasBeenShared;
           
        }
    }
}

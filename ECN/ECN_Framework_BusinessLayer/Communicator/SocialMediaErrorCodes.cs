using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class SocialMediaErrorCodes
    {
        public static ECN_Framework_Entities.Communicator.SocialMediaErrorCodes GetByErrorCode(int errorCode, int mediaType, bool repostCodes)
        {
            ECN_Framework_Entities.Communicator.SocialMediaErrorCodes retList = new ECN_Framework_Entities.Communicator.SocialMediaErrorCodes();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.SocialMediaErrorCodes.GetByErrorCode(errorCode, mediaType, repostCodes);
                scope.Complete();
            }
            return retList;
        }

        public static List<ECN_Framework_Entities.Communicator.SocialMediaErrorCodes> GetByMediaType(int mediaType)
        {
            List<ECN_Framework_Entities.Communicator.SocialMediaErrorCodes> retList = new List<ECN_Framework_Entities.Communicator.SocialMediaErrorCodes>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.SocialMediaErrorCodes.GetByMediaType(mediaType);
                scope.Complete();
            }
            return retList;
        }

    }
}

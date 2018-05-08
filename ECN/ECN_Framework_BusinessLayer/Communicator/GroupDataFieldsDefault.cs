using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class GroupDataFieldsDefault
    {
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.GroupUDFs;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.GroupDataFieldsDefault;


        public static ECN_Framework_Entities.Communicator.GroupDataFieldsDefault GetByGDFID(int GDFID)
        {
            ECN_Framework_Entities.Communicator.GroupDataFieldsDefault gdfd = new ECN_Framework_Entities.Communicator.GroupDataFieldsDefault();
            
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                gdfd = ECN_Framework_DataLayer.Communicator.GroupDataFieldsDefault.GetByGDFID(GDFID);
                scope.Complete();

            }
            return gdfd;
        }

        public static void Save(ECN_Framework_Entities.Communicator.GroupDataFieldsDefault gdfd)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.GroupDataFieldsDefault.Save(gdfd);
                scope.Complete();
            }
        }

        public static void Delete(int GDFID)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.GroupDataFieldsDefault.Delete(GDFID);
                scope.Complete();
            }
        }
    }
}

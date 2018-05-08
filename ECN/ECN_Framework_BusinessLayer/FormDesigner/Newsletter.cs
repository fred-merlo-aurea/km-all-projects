using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.FormDesigner
{
    [Serializable]
    public class Newsletter
    {
        //static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.FORMSDESIGNER;
        //static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Groups;

        public static bool ActiveByGDF(int groupDataFieldsID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.FormDesigner.Newsletter.ActiveByGDF(groupDataFieldsID, customerID);
                scope.Complete();
            }
            return exists;
        }
    }
}

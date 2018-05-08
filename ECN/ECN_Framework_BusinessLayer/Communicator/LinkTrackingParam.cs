using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
      [Serializable]
    public class LinkTrackingParam
    {
          public static List<ECN_Framework_Entities.Communicator.LinkTrackingParam> GetByLinkTrackingID(int LTID)
          {
              List<ECN_Framework_Entities.Communicator.LinkTrackingParam> LinkTrackingParamList = new List<ECN_Framework_Entities.Communicator.LinkTrackingParam>();
              using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
              {
                  LinkTrackingParamList = ECN_Framework_DataLayer.Communicator.LinkTrackingParam.GetByLTID(LTID);
                  scope.Complete();
              }
              return LinkTrackingParamList;
          }
    }
}

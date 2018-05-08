using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using System.Configuration;


namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class PersonalizationContentErrorCodes
    {
        public static List<ECN_Framework_Entities.Communicator.PersonalizationContentErrorCodes> GetAll()
        {
            List<ECN_Framework_Entities.Communicator.PersonalizationContentErrorCodes> retList = new List<ECN_Framework_Entities.Communicator.PersonalizationContentErrorCodes>();

            using (TransactionScope scope = new TransactionScope())
            {
                retList = ECN_Framework_DataLayer.Communicator.PersonalizationContentErrorCodes.GetAll();
                scope.Complete();
            }
            return retList;
        }
    }
}

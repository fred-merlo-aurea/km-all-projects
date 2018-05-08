using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using System.Configuration;


namespace ECN_Framework_BusinessLayer.Content
{
    public class PersonalizedContentErrorCodes
    {
        public static List<ECN_Framework_Entities.Content.PersonalizedContentErrorCodes> GetAll()
        {
            List<ECN_Framework_Entities.Content.PersonalizedContentErrorCodes> retList = new List<ECN_Framework_Entities.Content.PersonalizedContentErrorCodes>();

            using (TransactionScope scope = new TransactionScope())
            {
                retList = ECN_Framework_DataLayer.Content.PersonalizedContentErrorCodes.GetAll();
                scope.Complete();
            }
            return retList;
        }
    }
}

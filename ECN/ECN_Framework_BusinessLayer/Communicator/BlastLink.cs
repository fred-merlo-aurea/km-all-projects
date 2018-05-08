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
    public class BlastLink
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastLink;

        public static int Insert(ECN_Framework_Entities.Communicator.BlastLink blastLink)
        {
            blastLink.LinkURL = LinkCleanUP(blastLink.LinkURL);
            Validate(blastLink);

            using (TransactionScope scope = new TransactionScope())
            {
                blastLink.BlastLinkID = ECN_Framework_DataLayer.Communicator.BlastLink.Insert(blastLink);
                scope.Complete();
            }
            
            return blastLink.BlastLinkID;
        }

        private static string LinkCleanUP(string link)
        {
            link = link.Replace("&amp;", "&");
            link = link.Replace("&lt;", "<");
            link = link.Replace("&gt;", ">");
            link = link.Replace("%23", "#");

            return link;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.BlastLink blastLink)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (blastLink.BlastID <= 0)
            {
                errorList.Add(new ECNError(Entity, Method, "BlastID is invalid"));
            }
            if (blastLink.LinkURL.Trim().Length == 0 || blastLink.LinkURL.Trim().Length > 1792)
            {
                errorList.Add(new ECNError(Entity, Method, "LinkURL length is invalid"));
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static ECN_Framework_Entities.Communicator.BlastLink GetByBlastLinkID(int blastID, int blastLinkID)
        {
            ECN_Framework_Entities.Communicator.BlastLink blastLink = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blastLink = ECN_Framework_DataLayer.Communicator.BlastLink.GetByBlastLinkID(blastID, blastLinkID);
                scope.Complete();
            }
            return blastLink;
        }

        public static ECN_Framework_Entities.Communicator.BlastLink GetByLinkURL(int blastID, string linkURL)
        {
            ECN_Framework_Entities.Communicator.BlastLink blastLink = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blastLink = ECN_Framework_DataLayer.Communicator.BlastLink.GetByLinkURL(blastID, LinkCleanUP(linkURL));
                scope.Complete();
            }
            return blastLink;
        }

        public static ECN_Framework_Entities.Communicator.BlastLink GetByLinkURL_ECNID(int blastid, string linkURL, string ecn_id)
        {
            ECN_Framework_Entities.Communicator.BlastLink blastLink = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blastLink = ECN_Framework_DataLayer.Communicator.BlastLink.GetByLinkURL_ECNID(blastid, linkURL, ecn_id);
                scope.Complete();
            }
            return blastLink;
        }

        public static List<ECN_Framework_Entities.Communicator.BlastLink> GetByBlastID(int blastID)
        {
            List<ECN_Framework_Entities.Communicator.BlastLink> blastLinkList = new List<ECN_Framework_Entities.Communicator.BlastLink>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blastLinkList = ECN_Framework_DataLayer.Communicator.BlastLink.GetByBlastID(blastID);
                scope.Complete();
            }

            return blastLinkList;
        }

        public static Dictionary<string, int> GetDictionaryByBlastID(int blastID)
        {
            Dictionary<string, int> blastLinkList = new Dictionary<string, int>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                blastLinkList = ECN_Framework_DataLayer.Communicator.BlastLink.GetDictionaryByBlastID(blastID);
                scope.Complete();
            }

            return blastLinkList;
        }

        //public static bool Exists(int blastID, int customerID)
        //{
        //    bool exists = false;
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        exists = ECN_Framework_DataLayer.Communicator.BlastFields.Exists(blastID, customerID);
        //        scope.Complete();
        //    }
        //    return exists;
        //}
    }
}

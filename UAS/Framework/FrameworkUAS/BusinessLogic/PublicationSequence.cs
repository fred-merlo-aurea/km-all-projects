using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class PublicationSequence
    {
        public List<Entity.PublicationSequence> SelectPublisher(int publisherID)
        {
            List<Entity.PublicationSequence> x = null; 
            x = DataAccess.PublicationSequence.SelectPublisher(publisherID).ToList();
            return x;
        }
        public List<Entity.PublicationSequence> SelectPublicationID(int publicationID)
        {
            List<Entity.PublicationSequence> x = null;
            x = DataAccess.PublicationSequence.SelectPublicationID(publicationID).ToList();
            return x;
        }
        public int GetNextSequenceID(int publicationID,int userID)
        {
            int x = 0;

            using (


                TransactionScope scope = new TransactionScope())
            {
                x = DataAccess.PublicationSequence.GetNextSequenceID(publicationID,userID);
                scope.Complete();
            }
            return x;
        }
        
    }
}

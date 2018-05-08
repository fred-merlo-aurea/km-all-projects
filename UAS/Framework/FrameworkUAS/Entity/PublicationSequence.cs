using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class PublicationSequence
    {
        public PublicationSequence() { }
        public PublicationSequence(int publicationID, int sequenceID, int userID) 
        {
            PublicationID = publicationID;
            SequenceID = sequenceID;
            DateCreated = DateTime.Now;
            CreatedByUserID = userID;
        }
        public PublicationSequence(int publicationID, int userID)
        {
            PublicationID = publicationID;
            BusinessLogic.PublicationSequence worker = new BusinessLogic.PublicationSequence();
            SequenceID = worker.GetNextSequenceID(publicationID, userID);
            DateCreated = DateTime.Now;
            CreatedByUserID = userID;
        }
        #region Properties
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public int SequenceID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        #endregion
    }
}

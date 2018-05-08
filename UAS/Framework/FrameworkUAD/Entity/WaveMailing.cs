using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class WaveMailing
    {
        public WaveMailing()
        {
            WaveMailingID = 0;
            IssueID = 0;
            WaveMailingName = string.Empty;
            PublicationID = 0;
            WaveNumber = 0;
            DateSubmittedToPrinter = null;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            SubmittedToPrinterByUserID = 0;
            CreatedByUserID = 0;
            UpdatedByUserID = 0;
        }

        #region Properties

        [DataMember]
        public int WaveMailingID { get; set; }
        [DataMember]
        public int IssueID { get; set; }
        [DataMember]
        public string WaveMailingName { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public int WaveNumber { get; set; }
        [DataMember]
        public DateTime? DateSubmittedToPrinter { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int SubmittedToPrinterByUserID { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int UpdatedByUserID { get; set; }

        #endregion
    }
}

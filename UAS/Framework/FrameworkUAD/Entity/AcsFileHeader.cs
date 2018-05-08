using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class AcsFileHeader
    {
        public AcsFileHeader()
        {
            AcsFileHeaderId = 0;
            RecordType = string.Empty;
            FileVersion = string.Empty;
            CustomerID = 0;
            CreateDate = DateTime.Now;
            ShipmentNumber = 0;
            TotalAcsRecordCount = 0;
            TotalCoaCount = 0;
            TotalNixieCount = 0;
            TrdRecordCount = 0;
            TrdAcsFeeAmount = 0;
            TrdCoaCount = 0;
            TrdCoaAcsFeeAmount = 0;
            TrdNixieCount = 0;
            TrdNixieAcsFeeAmount = 0;
            OcdRecordCount = 0;
            OcdAcsFeeAmount = 0;
            OcdCoaCount = 0;
            OcdCoaAcsFeeAmount = 0;
            OcdNixieCount = 0;
            OcdNixieAcsFeeAmount = 0;
            FsRecordCount = 0;
            FsAcsFeeAmount = 0;
            FsCoaCount = 0;
            FsCoaAcsFeeAmount = 0;
            FsNixieCount = 0;
            FsNixieAcsFeeAmount = 0;
            ImpbRecordCount = 0;
            ImpbAcsFeeAmount = 0;
            ImpbCoaCount = 0;
            ImpbCoaAcsFeeAmount = 0;
            ImpbNixieCount = 0;
            ImpbNixieAcsFeeAmount = 0;
            Filler = string.Empty;
            EndMarker = string.Empty;
            ProcessCode = Core_AMS.Utilities.StringFunctions.GenerateProcessCode();
        }
        #region Properties
        [DataMember]
        public int AcsFileHeaderId { get; set; }
        [DataMember]
        public string RecordType { get; set; }
        [DataMember]
        public string FileVersion { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public DateTime CreateDate { get; set; }// (CCYYMMDD)
        [DataMember]
        public Int64 ShipmentNumber { get; set; }
        [DataMember]
        public int TotalAcsRecordCount { get; set; }
        [DataMember]
        public int TotalCoaCount { get; set; }
        [DataMember]
        public int TotalNixieCount { get; set; }
        [DataMember]
        public int TrdRecordCount { get; set; }
        [DataMember]
        public decimal TrdAcsFeeAmount { get; set; }
        [DataMember]
        public int TrdCoaCount { get; set; }
        [DataMember]
        public decimal TrdCoaAcsFeeAmount { get; set; }
        [DataMember]
        public int TrdNixieCount { get; set; }
        [DataMember]
        public decimal TrdNixieAcsFeeAmount { get; set; }
        [DataMember]
        public int OcdRecordCount { get; set; }
        [DataMember]
        public decimal OcdAcsFeeAmount { get; set; }
        [DataMember]
        public int OcdCoaCount { get; set; }
        [DataMember]
        public decimal OcdCoaAcsFeeAmount { get; set; }
        [DataMember]
        public int OcdNixieCount { get; set; }
        [DataMember]
        public decimal OcdNixieAcsFeeAmount { get; set; }
        [DataMember]
        public int FsRecordCount { get; set; }
        [DataMember]
        public decimal FsAcsFeeAmount { get; set; }
        [DataMember]
        public int FsCoaCount { get; set; }
        [DataMember]
        public decimal FsCoaAcsFeeAmount { get; set; }
        [DataMember]
        public int FsNixieCount { get; set; }
        [DataMember]
        public decimal FsNixieAcsFeeAmount { get; set; }
        [DataMember]
        public int ImpbRecordCount { get; set; }
        [DataMember]
        public decimal ImpbAcsFeeAmount { get; set; }
        [DataMember]
        public int ImpbCoaCount { get; set; }
        [DataMember]
        public decimal ImpbCoaAcsFeeAmount { get; set; }
        [DataMember]
        public int ImpbNixieCount { get; set; }
        [DataMember]
        public decimal ImpbNixieAcsFeeAmount { get; set; }
        [DataMember]
        public string Filler { get; set; }
        [DataMember]
        public string EndMarker { get; set; }
        [DataMember]
        public string ProcessCode { get; set; }
        #endregion
    }
}

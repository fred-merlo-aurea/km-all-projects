using System;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class SubGenQue
    {
        public SubGenQue()
        {
            SubGenQueId = 0;
            QueTypeCodeId = 0;
            SubGenEntityCodeId = 0;
            ClientId = 0;
            SubGenAccountId = 0;
            ProductId = 0;
            SubGenPublicationId = 0;
            JsonData = string.Empty;
            IsProcessed = false;
            ProcessedDate = DateTimeFunctions.GetMinDate();
            ProcessedTime = DateTimeFunctions.GetMinTime();
            ProcessCode = string.Empty;
            DateCreated = DateTime.Now;
            DateUpdated = null;
        }
        #region Properties
        public int SubGenQueId { get; set; }
        public int QueTypeCodeId { get; set; }
        public int SubGenEntityCodeId { get; set; }
        public int ClientId { get; set; }
        public int SubGenAccountId { get; set; }
        public int ProductId { get; set; }
        public int SubGenPublicationId { get; set; }
        public string JsonData { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime ProcessedDate { get; set; }
        public TimeSpan ProcessedTime { get; set; }
        public string ProcessCode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        #endregion
    }
}

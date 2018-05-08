using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class ChampionAudit
    {
        public ChampionAudit()
        {
            ChampionAuditID = -1;
            AuditTime = null;
            SampleID = null;
            BlastIDA = null;
            BlastIDB = null;
            BlastIDChampion = null;
            ClicksA = null;
            ClicksB = null;
            OpensA = null;
            OpensB = null;
            BouncesA = null;
            BouncesB = null;
            BlastIDWinning = null;
            //These are hard-coded for now until we allow customers the choice
            //Seems like championByProc (which uses spGetSampleInfoForChampion) 
            //returns a champion by a combination of open and click percentages
            SendToNonWinner = true;
            Reason = "clicks";
        }
        [DataMember]
        public int ChampionAuditID { get; set; }
        [DataMember]
        public DateTime? AuditTime { get; set; }
        [DataMember]
        public int? SampleID { get; set; }
        [DataMember]
        public int? BlastIDA { get; set; }
        [DataMember]
        public int? BlastIDB { get; set; }
        [DataMember]
        public int? BlastIDChampion { get; set; }
        [DataMember]
        public int? ClicksA { get; set; }
        [DataMember]
        public int? ClicksB { get; set; }
        [DataMember]
        public int? OpensA { get; set; }
        [DataMember]
        public int? OpensB { get; set; }
        [DataMember]
        public int? BouncesA { get; set; }
        [DataMember]
        public int? BouncesB { get; set; }
        [DataMember]
        public int? BlastIDWinning { get; set; }
        [DataMember]
        public bool? SendToNonWinner
        {
            get;
            private set;
        }
        [DataMember]
        public string Reason
        {
            get;
            set;
        }
    }
}

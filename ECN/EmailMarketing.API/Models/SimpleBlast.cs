using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models
{
    [Serializable]
    [DataContract]
    [XmlRoot("SimpleBlast")]
    public class SimpleBlast : BlastBase
    {
        /// <summary>
        /// Create a SimpleBlast
        /// </summary>
        public SimpleBlast()
        {
            SendTime = DateTime.Now;
            BlastType = "Regular";
        }

        [DataMember]
        [XmlElement]
        public SimpleBlastSchedule Schedule { get; set; }

        /// <summary>
        /// supply true to create a test blast, otherwise a standard blast will be created.
        /// </summary>  
        [DataMember]
        [XmlElement]
        public bool IsTestBlast { get; set; }

        /// <summary>
        /// Read-only, Smart Segment ID filled by ECN when a SimpleBlast is posted.
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? SmartSegmentID { get; set; }

        /// <summary>
        /// allow internal components the ability to set the smart segment ID
        /// </summary>
        /// <param name="smartSegmentID"></param>
        internal void SetSmartSegmentID(int smartSegmentID) { SmartSegmentID = smartSegmentID; }

        /// <summary>
        /// Optional, for SmartSegments an array of ID values for reference blasts.
        /// </summary>
        [DataMember]
        [XmlElement]
        public int[] ReferenceBlasts { get; set; }

        /// <summary>
        /// Blasts returned by Get or Search may fill this.  Blasts created by simple blast may 
        /// select a filter by setting FilterID.
        /// </summary>
        [DataMember]
        [XmlElement]
        public int[] Filters { get; set; }

        /// <summary>
        /// Blasts returned by Get or Search may fill this.  Blasts created by simple blast may 
        /// select a filter by setting FilterID.
        /// </summary>
        [DataMember]
        [XmlElement]
        public int[] SmartSegments { get; set; }

        /// <summary>
        /// SimpleBlasts are sent immediately.
        /// </summary>
        [DataMember]
        [XmlElement]
        public DateTime SendTime { get; set; }
        internal void SetSendTime(DateTime sendTime) { SendTime = sendTime; }
    }
}
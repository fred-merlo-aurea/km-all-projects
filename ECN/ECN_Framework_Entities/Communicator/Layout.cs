using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class Layout : IUserValidate
    {
        public Layout()
        {
            LayoutID = -1;
            TemplateID = null;
            CustomerID = null;
            FolderID = null;
            LayoutName = string.Empty;
            ContentSlot1 = null;
            ContentSlot2 = null;
            ContentSlot3 = null;
            ContentSlot4 = null;
            ContentSlot5 = null;
            ContentSlot6 = null;
            ContentSlot7 = null;
            ContentSlot8 = null;
            ContentSlot9 = null;
            TableOptions = string.Empty;
            DisplayAddress = string.Empty;
            SetupCost = string.Empty;
            OutboundCost = string.Empty;
            InboundCost = string.Empty;
            DesignCost = string.Empty;
            OtherCost = string.Empty;
            MessageTypeID = null;
            Slot1 = null;
            Slot2 = null;
            Slot3 = null;
            Slot4 = null;
            Slot5 = null;
            Slot6 = null;
            Slot7 = null;
            Slot8 = null;
            Slot9 = null;
            Template = null;
            Folder = null;
            MessageType = null;
            ConvLinks = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            Archived = false;
        }

        public bool HasValidID
        {
            get { return LayoutID > 0; }
        }
        [DataMember]
        public int LayoutID { get; set; }
        [DataMember]
        public int? TemplateID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? FolderID { get; set; }
        [DataMember]
        public string LayoutName { get; set; }
        [DataMember]
        public int? ContentSlot1 { get; set; }
        [DataMember]
        public int? ContentSlot2 { get; set; }
        [DataMember]
        public int? ContentSlot3 { get; set; }
        [DataMember]
        public int? ContentSlot4 { get; set; }
        [DataMember]
        public int? ContentSlot5 { get; set; }
        [DataMember]
        public int? ContentSlot6 { get; set; }
        [DataMember]
        public int? ContentSlot7 { get; set; }
        [DataMember]
        public int? ContentSlot8 { get; set; }
        [DataMember]
        public int? ContentSlot9 { get; set; }
        [DataMember]
        public string TableOptions { get; set; }
        [DataMember]
        public string DisplayAddress { get; set; }
        [DataMember]
        public string SetupCost { get; set; }
        [DataMember]
        public string OutboundCost { get; set; }
        [DataMember]
        public string InboundCost { get; set; }
        [DataMember]
        public string DesignCost { get; set; }
        [DataMember]
        public string OtherCost { get; set; }
        [DataMember]
        public int? MessageTypeID { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public bool? Archived { get; set; }
        //Optional
        public ECN_Framework_Entities.Communicator.Content Slot1 { get; set; }
        public ECN_Framework_Entities.Communicator.Content Slot2 { get; set; }
        public ECN_Framework_Entities.Communicator.Content Slot3 { get; set; }
        public ECN_Framework_Entities.Communicator.Content Slot4 { get; set; }
        public ECN_Framework_Entities.Communicator.Content Slot5 { get; set; }
        public ECN_Framework_Entities.Communicator.Content Slot6 { get; set; }
        public ECN_Framework_Entities.Communicator.Content Slot7 { get; set; }
        public ECN_Framework_Entities.Communicator.Content Slot8 { get; set; }
        public ECN_Framework_Entities.Communicator.Content Slot9 { get; set; }
        public ECN_Framework_Entities.Communicator.Template Template { get; set; }
        public ECN_Framework_Entities.Communicator.Folder Folder { get; set; }
        public ECN_Framework_Entities.Communicator.MessageType MessageType { get; set; }
        public List<ECN_Framework_Entities.Communicator.ConversionLinks> ConvLinks { get; set; }
    }
}

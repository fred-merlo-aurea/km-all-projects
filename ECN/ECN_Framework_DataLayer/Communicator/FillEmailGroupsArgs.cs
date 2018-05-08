using System;

namespace ECN_Framework_DataLayer.Communicator
{
    internal class FillEmailGroupsArgs
    {
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public int? GroupId { get; set; }
        public string XmlProfile { get; set; }
        public string XmlUdf { get; set; }
        public string FormatTypeCode { get; set; }
        public string SubscribeTypeCode { get; set; }
        public bool? EmailAddressOnly { get; set; }
        public string Filename { get; set; }
        public bool? OverwriteWithNull { get; set; }
        public string CompositeKey { get; set; }
        public string Source { get; set; }
        public string SourceNotRequired { get; set; }
    }
}

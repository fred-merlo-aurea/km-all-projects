using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace EmailPreview
{
    [Serializable]
    [DataContract]
    public class LinkTest
    {
        public LinkTest() { Error = false; }
        #region Properties
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public List<Link> Links { get; set; }
        [DataMember]
        public string SourceUrl { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public bool? Error { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string HTML { get; set; }
        #endregion
    }

    public class Link
    {
        public Link() { IsBlacklisted = false; HasGoogleAnalytics = false; HasClickTracking = false; }
        #region Properties
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public bool? IsValid { get; set; }
        [DataMember]
        public string Exception { get; set; }
        [DataMember]
        public bool? IsBlacklisted { get; set; }
        [DataMember]
        public bool? HasGoogleAnalytics { get; set; }
        [DataMember]
        public bool? HasClickTracking { get; set; }
        [DataMember]
        public string PageTitle { get; set; }
        [DataMember]
        public int TopLeftX { get; set; }
        [DataMember]
        public int TopLeftY { get; set; }
        [DataMember]
        public Dictionary<string, string> GoogleAnalyticsParameters { get; set; }
        [DataMember]
        public string ThumbnailUri { get; set; }
        #endregion
    }
}

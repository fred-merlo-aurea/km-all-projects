using System;
using System.Runtime.Serialization;

namespace EmailPreview
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    public class SpamHeader
    {
        public SpamHeader() { }
        #region Properties
        [Newtonsoft.Json.JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Id")]
        public int ID { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Rating")]
        public int Rating { get; set; }
        #endregion
    }
}

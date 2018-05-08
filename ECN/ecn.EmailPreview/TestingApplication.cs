using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace EmailPreview
{
    [Serializable]
    [DataContract]
    public class TestingApplication
    {
        #region Properties
        [Newtonsoft.Json.JsonProperty(PropertyName ="ID")]
        public int ID { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ZipFile")]
        public string ZipFile { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ApplicationLongName")]
        public string ApplicationLongName { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ApplicationName")]
        public string ApplicationName { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "AverageTimeToProcess")]
        public int AverageTimeToProcess { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "BusinessOrPopular")]
        public bool? BusinessOrPopular { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Completed")]
        public bool? Completed { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "DesktopClient")]
        public bool? DesktopClient { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FoundInSpam")]
        public bool? FoundInSpam { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "PlatformLongName")]
        public string PlatformLongName { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "PlatformName")]
        public string PlatformName { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ResultType")]
        public string ResultType { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "SpamScore")]
        public double SpamScore { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "State")]
        public string State { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Status")]
        public int Status { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "SupportsContentBlocking")]
        public bool? SupportsContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "WindowImage")]
        public string WindowImage { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "WindowImageContentBlocking")]
        public string WindowImageContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "WindowImageNoContentBlocking")]
        public string WindowImageNoContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "WindowImageThumb")]
        public string WindowImageThumb { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "WindowImageThumbContentBlocking")]
        public string WindowImageThumbContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "WindowImageThumbNoContentBlocking")]
        public string WindowImageThumbNoContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FullpageImage")]
        public string FullpageImage { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FullpageImageContentBlocking")]
        public string FullpageImageContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FullpageImageNoContentBlocking")]
        public string FullpageImageNoContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FullpageImageThumb")]
        public string FullpageImageThumb { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FullpageImageThumbContentBlocking")]
        public string FullpageImageThumbContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FullpageImageThumbNoContentBlocking")]
        public string FullpageImageThumbNoContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "SpamHeaders")]
        public List<SpamHeader> SpamHeaders { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "SupportsSpamScoring")]
        public bool? SupportsSpamScoring { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Summary")]
        public System.Text.StringBuilder Summary { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "RenderedHtmlUrl")]
        public string RenderedHtmlUrl { get; set; }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.API.Models
{
    public class BaseChannel
    {
        /// <summary>
        /// Unique Base Channel Identification Number
        /// </summary>
        public int BaseChannelID { get; set; }

        /// <summary>
        /// Unique Name for Base Channel
        /// </summary>
        public string BaseChannelName { get; set; }

        /// <summary>
        /// Base Channel Level API Access Key
        /// </summary>
        public Guid AccessKey { get; set; }
   }
}
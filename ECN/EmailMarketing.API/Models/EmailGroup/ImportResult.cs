using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;


namespace EmailMarketing.API.Models.EmailGroup
{
    [XmlRoot("ImportResult")]
    public class ImportResult
    {
        /// <summary>
        /// New emails added to group
        /// </summary>
        public int New { get; set; }
        /// <summary>
        /// Emails updated
        /// </summary>
        public int Updated { get; set; }
        /// <summary>
        /// Duplicate emails in request
        /// </summary>
        public int Duplicate { get; set; }
        /// <summary>
        /// Emails that are master suppressed
        /// </summary>
        public int MasterSuppressed { get; set; }
        /// <summary>
        /// Total emails in request
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Emails skipped
        /// </summary>
        public int Skipped { get; set; }

        public ImportResult()
        {
            New = 0;
            Updated = 0;
            Duplicate = 0;
            MasterSuppressed = 0;
            Total = 0;
            Skipped = 0;
        }

    }
}
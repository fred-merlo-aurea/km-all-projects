using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models
{
    public class PersonalizationContentResponse
    {
        public PersonalizationContentResponse()
        {
            Created = 0;
            Failed = new FailedContent();
            Total = 0;
        }

        /// <summary>
        /// Personalized Content created successfully
        /// </summary>
        public int Created { get; set; }
        /// <summary>
        /// Personalized Content that failed
        /// </summary>
        public FailedContent Failed { get; set; }
        /// <summary>
        /// Total 
        /// </summary>
        public int Total { get; set; }
        
    }

    public class FailedContent
    {
        public FailedContent()
        {
            Total = 0;
            Failures = new List<FailedEmail>();
        }

        /// <summary>
        /// Total failures
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// List of Emails and their Error codes
        /// </summary>
        public List<FailedEmail> Failures { get; set; }
    }

    public class FailedEmail
    {
        public FailedEmail()
        {
            EmailAddress = "";
            ErrorCode = new List<int>();
        }

        /// <summary>
        /// Email Address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Error code
        /// </summary>
        public List<int> ErrorCode { get; set; }
    }
}
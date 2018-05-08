using System;
using System.Linq;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The User object.
    /// </summary>
    public class User
    {
        /// <summary>
        /// User ID for each User object.
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// Customer ID for the User object.
        /// </summary>
        public int CustomerID { get; set; }
        /// <summary>
        /// Username of the User object.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// AccessKey for the User object.
        /// </summary>
        public Guid AccessKey { get; set; }
    }
}
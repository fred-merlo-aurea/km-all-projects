using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.API.Models
{
    public class FormsCustomer
    {
        /// <summary>
        /// The CustomerID value of the Forms Customer
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// The CustomerName of the Forms Customer
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// The AccessKey for use by the forms application when access data related to the Forms Customer
        /// </summary>
        public Guid AccessKey { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;

using System.Web.Http.Filters;

namespace UAD.API.Attributes
{
    public class OrderedAuthorizationFilterAttribute : AuthorizationFilterAttribute, IOrderAttributes
    {
        public int Order { get; set; }

        public OrderedAuthorizationFilterAttribute()
        {
            Order = 0;
        }

        public OrderedAuthorizationFilterAttribute(int order)
        {
            Order = order;
        }
    }
}
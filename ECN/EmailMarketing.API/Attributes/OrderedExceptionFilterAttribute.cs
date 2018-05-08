using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;

using System.Web.Http.Filters;

namespace EmailMarketing.API.Attributes
{
    public class OrderedExceptionFilterAttribute : ExceptionFilterAttribute, IOrderAttributes
    {
        public int Order { get; set; }

        public OrderedExceptionFilterAttribute()
        {
            Order = 0;
        }

        public OrderedExceptionFilterAttribute(int order)
        {
            Order = order;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;

using System.Web.Http.Filters;

namespace EmailMarketing.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class OrderedActionFilterAttribute : ActionFilterAttribute, IOrderAttributes
    {
        public int Order { get; set; }

        public OrderedActionFilterAttribute()
        {
            Order = 0;
        }

        public OrderedActionFilterAttribute(int order)
        {
            Order = order;
        }
    }
}
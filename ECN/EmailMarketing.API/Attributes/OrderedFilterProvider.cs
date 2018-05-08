using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EmailMarketing.API.Attributes
{
    /// <summary>
    /// Custom implementation of <see cref="System.Web.Http.Filters.IFilterProvider"/>
    /// honoring the <code>Order</code> attribute exposed by Attributes implementing the
    /// <see cref="EmailMarketing.API.Attributes.IOrderAttributes"/> interface.
    /// </summary>
    public class OrderedFilterProvider : IFilterProvider
    {
        /// <summary>
        /// Creates an explicitly order enumeration of <see cref="System.Web.Http.Filters.FilterInfo"/>
        /// by delegating to the IComparable implementation within 
        /// <see cref="EmailMarketing.API.Attributes.OrderedFilterInfo"/>
        /// </summary>
        /// <param name="configuration">HTTP (server) configuration instance</param>
        /// <param name="actionDescriptor">descriptor for the HTTP action to be dispatched</param>
        /// <returns></returns>
        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration","configuration may not be null");
            }

            if (actionDescriptor == null)
            {
                throw new ArgumentNullException("actionDescriptor","actionDescriptor may not be null");
            }

            IEnumerable<OrderedFilterInfo> actionFilters = actionDescriptor.GetFilters().Select(i => new OrderedFilterInfo(i, FilterScope.Action));
            IEnumerable<OrderedFilterInfo> controllerFilters = actionDescriptor.ControllerDescriptor.GetFilters().Select(i => new OrderedFilterInfo(i, FilterScope.Controller));
            IEnumerable<OrderedFilterInfo> globalFilters = actionDescriptor.Configuration.Filters
                .Where(f => f.Scope == FilterScope.Global)
                .Select(i => new OrderedFilterInfo(i.Instance, FilterScope.Global));
            var orderedFilters = globalFilters.Concat(controllerFilters).Concat(actionFilters).OrderBy(i => i).Select(i => i.ConvertToFilterInfo());
            return orderedFilters;
        }
    }
}
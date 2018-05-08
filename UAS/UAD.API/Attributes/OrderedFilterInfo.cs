using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;

using System.Web.Http.Filters;

namespace UAD.API.Attributes
{
    /// <summary>
    /// Custom HTTP Filter Info supporting explicit action/exception/authorization filter handler 
    /// invocation. <seealso cref="System.Web.Http.Filters.FilterInfo"/>
    /// </summary>
    public class OrderedFilterInfo : IComparable
    {
        /// <summary>
        /// Instance of an object implementing the IFilter interface.
        /// </summary>
        public IFilter Instance { get; set; }

        /// <summary>
        /// FilterScope object instance
        /// </summary>
        public FilterScope Scope { get; set; }
        public OrderedFilterInfo(IFilter instance, FilterScope scope)
        {
            Instance = instance;
            Scope = scope;
        }

        /// <summary>
        /// Implements the IComparable interface to provide ordering of filter handler
        /// invocation based on the <code>Order</code> property
        /// </summary>
        /// <param name="obj">instance of an object implementing the IOrderAttribute interface</param>
        /// <returns>-1 of the given object comes before the context object per Order, 1 if after or zero 
        /// if the objects have the same value for <code>Order</code>.</returns>
        public int CompareTo(object obj)
        {
            if(obj is OrderedFilterInfo)
            {
                var item = obj as OrderedFilterInfo;
                if(item.Instance is IOrderAttributes)
                {
                    var attr = item.Instance as IOrderAttributes;
                    return (this.Instance as IOrderAttributes).Order.CompareTo(attr.Order);
                }
                else
                {
                    throw new ArgumentException("Unsupported attribute type: " + item.Instance.GetType().Name);
                }
            }
            else
            {
                throw new ArgumentException("Unsupported filter type: " + obj.GetType().Name);
            }
        }

        /// <summary>
        /// Allows ingestion of our customized FilterInfo implementation back into the general
        /// Web API pipeline by converting the object back into the framework provided base-class.
        /// This is neeeded because the Framework provided base-class 
        /// (<see cref="System.Web.Http.Filters.FilterInfo"/>) is sealed and cannot simply be 
        /// sub-classed.
        /// </summary>
        /// <returns>a new instance of <see cref="System.Web.Http.Filters.FilterInfo"/> having the same
        /// <see cref="Instance"/> and <see cref="Scope"/> as the instance of <code>OrderFilterInfo</code>.
        /// </returns>
        public FilterInfo ConvertToFilterInfo()
        {
            return new FilterInfo(this.Instance, this.Scope);
        }
    }
}
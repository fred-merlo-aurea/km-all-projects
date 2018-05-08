using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using ECN_Framework_Entities.Salesforce.Helpers;

namespace ECN_Framework_Entities.Salesforce.Sorting
{
    public class EntitySort
    {
        private HashSet<string> _notSortingProperties = new HashSet<string>();

        public void ExcludeFromSorting<T,TKey>(Expression<Func<T, TKey>> propertyExp)
        {
            var name = PropertyHelper.GetPropertyName(propertyExp);
            _notSortingProperties.Add(name);
        }

        public IEnumerable<T> Sort<T>(IEnumerable<T> colection, string propertyName, bool isAscending)
        {
            if (_notSortingProperties.Contains(propertyName))
            {
                return colection;
            }

            var property = PropertyHelper.GetProperty<T>(propertyName);
            if (property == null)
            {
                return colection;
            }

            return Sort(colection, property, isAscending);
        }

        private IOrderedEnumerable<T> Sort<T>(IEnumerable<T> colection, PropertyDescriptor property, bool isAscending)
        {
            Func<T, object> expression = x => property.GetValue(x);

            return isAscending
                  ? colection.OrderBy(expression)
                  : colection.OrderByDescending(expression);
        }
    }
}

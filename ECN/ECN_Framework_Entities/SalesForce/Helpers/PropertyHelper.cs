using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ECN_Framework_Entities.Salesforce.Helpers
{
    public static class PropertyHelper
    {
        public static string GetPropertyName<T, TKey>(Expression<Func<T, TKey>> propertyExp)
        {
            var expression = (MemberExpression)propertyExp.Body;
            return expression.Member.Name;
        }

        public static PropertyDescriptor GetProperty<T>(string propertyName)
        {
            return TypeDescriptor.GetProperties(typeof(T)).Find(propertyName, true);
        }
    }
}

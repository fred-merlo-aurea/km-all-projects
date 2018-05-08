using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq.Expressions;

namespace Core_AMS.Utilities
{
    public static class ObjectFunctions
    {
        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            MemberExpression body = (MemberExpression) expression.Body;
            return body.Member.Name;
        }
        public static T DeepCopy<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }
        public static bool ChangeKey<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                           TKey oldKey, TKey newKey)
        {
            TValue value;
            if (!dict.TryGetValue(oldKey, out value))
                return false;

            dict.Remove(oldKey);  // do not change order
            dict[newKey] = value;  // or dict.Add(newKey, value) depending on ur comfort
            return true;
        }
        //public static Control FindControlRecursive(Control controlToSearch, string id)
        //{
        //    if (null == controlToSearch)
        //        throw new ArgumentNullException("controlToSearch");

        //    if (string.IsNullOrEmpty("id"))
        //        throw new ArgumentNullException("id");

        //    Control result = controlToSearch.FindControl(id);
        //    if (null == result)
        //    {
        //        if (null != controlToSearch.Controls && controlToSearch.Controls.Count > 0)
        //        {
        //            foreach (Control c in controlToSearch.Controls)
        //            {
        //                result = FindControlRecursive(c, id);
        //                if (null != result)
        //                    break;
        //            }
        //        }
        //    }
        //    return result;
        //}
    }
}

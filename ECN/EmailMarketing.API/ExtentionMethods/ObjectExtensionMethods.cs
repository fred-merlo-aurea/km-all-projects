using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Collections;
using System.Reflection;
using System.Xml;
using System.Collections.Generic;

namespace EmailMarketing.API.ExtentionMethods
{
    public class ObjectExtensionMethods
    {
        /// <summary>
        /// Converts an anonymous type to an XElement.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns the object as it's XML representation in an XElement.</returns>
        public static XElement ToXml2(object input)
        {
            return ToXml2(input, null);
        }

        /// <summary>
        /// Converts an anonymous type to an XElement.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="element">The element name.</param>
        /// <returns>Returns the object as it's XML representation in an XElement.</returns>
        public static XElement ToXml2(object input, string element)
        {
            return _ToXml(input, element);
        }

        private static XElement _ToXml(object input, string element, int? arrayIndex = null, string arrayName = null)
        {
            if (input == null)
                return null;

            if (String.IsNullOrEmpty(element))
            {
                string name = input.GetType().Name;
                //element = name.Contains("AnonymousType")
                //    ? "Object"
                //    : arrayIndex != null
                //        ? arrayName + "_" + arrayIndex
                //        : name;
                element = name;
            }

            element = XmlConvert.EncodeName(element);
            var ret = new XElement(element);

            if (input != null)
            {
                var type = input.GetType();
                var props = type.GetProperties();

                var elements = props.Select(p =>
                {
                    try
                    {
                        var pType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                        var name = XmlConvert.EncodeName(p.Name);
                        var val = pType.IsArray ? "array" : p.GetValue(input, null);
                        var inter = pType.GetInterfaces();
                        var value = pType.GetInterfaces().Count(x => x.IsGenericType == true && x.GetGenericTypeDefinition() == typeof(IList<>)) > 0
                            ? GetEnumerableElements(p, (IEnumerable)p.GetValue(input, null))
                            : IsSimpleType2(pType) || pType.IsEnum
                                ? new XElement(name, val)
                                : ToXml2(val, name);
                        return value;
                    }
                    catch { return null; }
                })
                .Where(v => v != null);

                ret.Add(elements);
            }

            return ret;
        }

        #region helpers

        private static XElement GetEnumerableElements(PropertyInfo info, IEnumerable input)
        {
            var name = XmlConvert.EncodeName(info.Name);


            XElement rootElement = new XElement(name);

            int i = 0;

                foreach (var v in input)
                {
                    XElement childElement = IsSimpleType2(v.GetType()) || v.GetType().IsEnum ? new XElement(name + "_" + i, v) : _ToXml(v, null, i, name);
                    rootElement.Add(childElement);
                    i++;
                }

            return rootElement;
        }

        private static readonly Type[] WriteTypes = new[] {
    typeof(string), typeof(DateTime), typeof(Enum), 
    typeof(decimal), typeof(Guid),typeof(char)
};
        public static bool IsSimpleType2(Type type)
        {
            return type.IsPrimitive || WriteTypes.Contains(type);
        }

        private static readonly Type[] FlatternTypes = new[] {
    typeof(string)
};
        public static bool IsEnumerable(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && !FlatternTypes.Contains(type);
        }
        #endregion
    }
}
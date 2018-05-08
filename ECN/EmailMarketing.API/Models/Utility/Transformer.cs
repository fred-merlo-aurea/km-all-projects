using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using EmailMarketing.API.Attributes;

namespace EmailMarketing.API.Models.Utility
{
    /// <summary>
    /// Utility class providing transformation from a type <code>SourceT</code> to another type <code>DestinationT</code>
    /// by performing name based copying of properties from a given SourceT object to a TargetT object.
    /// </summary>
    /// <typeparam name="SourceT">A <see cref="System.Type"/> implementing a parameterless constructor.</typeparam>
    /// <typeparam name="DestinationT">A <see cref="System.Type"/> implementing a parameterless constructor.</typeparam>
    public static class Transformer<SourceT,DestinationT> where SourceT : new()  where DestinationT : new()
    {
        /// <summary>
        /// Transforms a object of type SourceT into a <b>new</b> object of type DestinationT
        /// </summary>
        /// <param name="source">instance object of type SourceT</param>
        /// <param name="propertyNames">property names to copy as an enumerable of strings</param>
        /// <returns>new instance of DestinationT</returns>
        public static DestinationT Transform(SourceT source, IEnumerable<string> propertyNames)
        {
            return Transform(source, new DestinationT(), propertyNames);
        }

        /// <summary>
        /// Transforms a object of type SourceT into a given object of type DestinationT by copying property values
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static DestinationT Transform(SourceT source, DestinationT destination, IEnumerable<string> propertyNames, bool useCustomProperties = false)
        {
            PropertyInfo[] sourceProperties = GetPropertiesByName(propertyNames, typeof(SourceT));
            PropertyInfo[] destinationProperties = GetPropertiesByName(propertyNames, typeof(DestinationT));

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                foreach (PropertyInfo destinationProperty in destinationProperties)
                {
                    if (sourceProperty.Name == destinationProperty.Name && true == destinationProperty.CanWrite)
                    {
                        ExcludeFromSearch exclude = null;
                        if(useCustomProperties)
                        {
                            exclude = (ExcludeFromSearch)destinationProperty.GetCustomAttributes(typeof(ExcludeFromSearch)).SingleOrDefault();
                        }
                       
                        
                        if (exclude == null || !exclude.Exclude)
                        {
                            destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                        }
                        else
                        {
                            destinationProperty.SetValue(destination, exclude.ExclusionMessage);
                        }
                        break;
                    }
                }
            }

            return destination;
        }

        /// <summary>
        /// Extract <code>Type</code>'s property info for all existent properties named
        /// </summary>
        /// <param name="propertyNames">properties to be extracted as an enumeration of strings</param>
        /// <param name="type">Type from which to extract PropertyInfo</param>
        /// <returns>Array of PropertyInfo</returns>
        public static PropertyInfo[] GetPropertiesByName(IEnumerable<string> propertyNames, Type type)
        {
            List<PropertyInfo> rv = new List<PropertyInfo>();
            PropertyInfo[] properties = type.GetProperties();
            foreach (var name in propertyNames)
            {
                PropertyInfo property = (from x in properties where ((PropertyInfo)x).Name == name select x as PropertyInfo).FirstOrDefault();
                if (null != property)
                {
                    rv.Add(property);
                }
            }

            return rv.ToArray();
        }
    }
}
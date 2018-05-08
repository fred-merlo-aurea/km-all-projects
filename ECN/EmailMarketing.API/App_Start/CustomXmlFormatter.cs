using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Serialization;
using ServiceStack.Text;
using System.Xml;

// allows us to serialize certain objects to XML which out-of-the-box WebAPI
// can only serialize to JSON.  
// http://stackoverflow.com/questions/18951521/web-api-dynamic-to-xml-serialization
namespace EmailMarketing.API.Formatters
{
    using GlobalConfiguration = System.Web.Http.GlobalConfiguration;

    // TODO: move this out to a separate field for reuse
    public static class TypeExtentions
    {
        public static bool IsEnumerable(this Type type)
        {
            return type.IsArray
                || (type.IsGenericType && type.GetInterfaces().Any(ti => (ti == typeof(IEnumerable<>) || ti.Name == "IEnumerable")));
        }
    }

    public class CustomXmlFormatter : MediaTypeFormatter
    {
        public CustomXmlFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
        }

        public override bool CanReadType(Type type)
        {
            if (type == (Type)null)
                throw new ArgumentNullException("type");

            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task WriteToStreamAsync(Type type, object value,
            Stream writeStream, System.Net.Http.HttpContent content,
            System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
                {
                    //value.GetType().IsArray
                    //var xmlRootElementName = value.GetType().IsEnumerable() ? "Array" : "Object"; // value.GetType().Name; //"Root"
                    string xmlRootElementName = "";
                    if (value.GetType().IsEnumerable())
                    {
                        try
                        {
                            
                            var listOfT = value.GetType().GetElementType();
                            xmlRootElementName = listOfT.CustomAttributes.First(x => x.AttributeType == typeof(XmlRootAttribute)).ConstructorArguments[0].ToString().Replace("\"", "");
                        }
                        catch
                        {
                            var ListType = value.GetType();
                            System.Reflection.PropertyInfo propInfo = ListType.GetProperty("Item");
                            var listOfT = propInfo.PropertyType;
                            
                            //object[] listOfAtt = listOfT.GetCustomAttributes(false);
                            xmlRootElementName = listOfT.CustomAttributes.First(x => x.AttributeType == typeof(XmlRootAttribute)).ConstructorArguments[0].ToString().Replace("\"", "");
                        }
                    }
                    else
                    {
                        xmlRootElementName = value.GetType().CustomAttributes.First(x => x.AttributeType == typeof(XmlRootAttribute)).ConstructorArguments[0].ToString().Replace("\"", "");
                        //var jsonSerilizationProxFormat = value.GetType().IsEnumerable() ? 
                    }
                    var json = JsonConvert.SerializeObject(value, GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings );
                    var typeOfObject = EmailMarketing.API.ExtentionMethods.ObjectExtensionMethods.ToXml2(value);

                    //xml.LoadXml(ServiceStack.Text.XmlSerializer.SerializeToString<value.GetType()>(json, typeOfObject).ToString());
                    //JsonConvert                .DeserializeXmlNode("{\"" + xmlRootElementName + "\":" + json + "}");

                    typeOfObject.Save(writeStream);
                });
        }

        
    }
}
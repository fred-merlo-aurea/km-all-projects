using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Infrastructure
{
    public class JavascriptHelperMethods
    {
        public static List<string> GetPropertyNames(Type type)
        {
            List<string> names = new List<string>();
            System.Reflection.PropertyInfo[] props = type.GetProperties();
            foreach(var prop in props)
            {
                names.Add(prop.Name);
            }
            return names;
        }

        // Example: 
        // C# list a,b,c,d
        // Goes to JS array ["a","b","c","d"]
        // Simple string conversion and list join. 
        // Returns a string to be used as javascript in a razor view (.cshtml)
        public static HtmlString MakeJavascriptArray<T>(IEnumerable<T> tlist)
        {
            List<string> stringlist = new List<string>();
            foreach(T element in tlist)
                stringlist.Add(element.ToString());
            string delimited = String.Join("\",\"", stringlist);
            string capped = "[\"" + delimited + "\"]";
            return new HtmlString(capped);
        }

        // There's probably a better way to do this. Regardless, I don't want to have to write 75 lines of code everytime I want to update a model. 
        // This looks at the properties of the model, and creates a javascript object reflecting the new values it found on page (elements with 
        // id equal to the property name). 
        // Returns a string to be used as javascript in a razor view (.cshtml)
        public static HtmlString MakeGetModelFunction(Type type)
        {
            string functionName = "GetModel";
            
            string function = 
                @"function " + functionName + "(){" +
                    "var model = {}; " +
                    "var propList = " + MakeJavascriptArray(GetPropertyNames(type)) + "; " +
                    "for(var i = 0; i < propList.length; i++){\r\n" +
                        "var input = $('#' + propList[i]); \r\n" +
                        "if(input !== null && input.length > 0){\r\n" +
                            //"if(input[0].tagName && input[0].tagName.toLowerCase() == 'textarea'){\r\n" +
                            //    "model[propList[i]] = input.text();\r\n" +
                            //"}else{\n" +
                                "model[propList[i]] = input.val(); \r\n" +
                            //"}\r\n" +
                        "}" + 
                    "}" + 
                    "return model; " + 
                "};";
            return new HtmlString(function);
        }
    }
}
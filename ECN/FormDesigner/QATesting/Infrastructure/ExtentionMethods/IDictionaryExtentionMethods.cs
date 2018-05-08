using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ecn.qatools.Infrastructure.ExtentionMethods
{
    public static class IDictionaryExtentionMethods
    {
        public static void SaveAsJson(this IDictionary<string,object> dictionary, string filename)
        {
            using (FileStream fs = File.Open(filename, FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, dictionary);
            }
        }
    }
}
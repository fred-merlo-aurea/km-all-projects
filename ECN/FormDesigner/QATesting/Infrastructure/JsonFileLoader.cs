using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ecn.qatools.Infrastructure
{
    public class JsonFileLoader
    {
        public static dynamic Load(string filename)
        {
            if (File.Exists(filename))
            {
                using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read))
                using (StreamReader sr = new StreamReader(fs))
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return serializer.Deserialize<Dictionary<string,object>>(jr);
                }
            }

            return new System.Dynamic.ExpandoObject();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ecn.qatools.Infrastructure.ExtentionMethods;
using ecn.qatools.Infrastructure.Abstract;

namespace ecn.qatools.Infrastructure.Concrete
{
    public class JsonFileKeyValueDataStore : IKeyValueDataStore, IDisposable
    {
        public const string JsonFilesFolderName = @"~/json";
        public static string DefaultFilename { 
            get 
            {
                return HttpContext.Current.Server.MapPath(JsonFilesFolderName + "/default.json" );
            } 
        }
        System.Collections.Generic.IDictionary<string, object> data;// = new System.Dynamic.ExpandoObject();

        string _filename;
        public string Filename 
        {
            get 
            {
                if(String.IsNullOrEmpty(_filename))
                {
                    _filename = DefaultFilename;
                }
                return _filename;
            }            
            set
            {
                _filename = value;
            }
        }

        public JsonFileKeyValueDataStore() : this(DefaultFilename) { }
        public JsonFileKeyValueDataStore(string filename)
        {
            Filename = filename;

            dynamic o = Load(Filename);
            data = o;
        }

        public dynamic Load(string filename=null)
        {
            return Infrastructure.JsonFileLoader.Load(filename??Filename);
        }

        public void Save(string filename=null)
        {
            data.SaveAsJson(filename ?? Filename);
        }

        public void Remove(string key)
        {
            if(data.ContainsKey(key))
            {
                data.Remove(key);
            }
        }

        public void Set(string key, string value)
        {
            data[key] = value;
            Save();
        }

        public string Get(string key)
        {
            if(false == data.ContainsKey(key))
            {
                return String.Empty;
            }

            return data[key].ToString();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAll()
        {
            Dictionary<string,string> rv = new Dictionary<string,string>();
            foreach(string key in data.Keys)
            {
                rv.Add(key, data[ key].ToString());
            }
            return rv;
        }

        public void Dispose()
        {
            //Save();
            data = null;
        }
    }
}
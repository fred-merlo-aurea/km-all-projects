using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.qatools.Infrastructure.Concrete
{
    public class AspApplicationStateKeyValueDataStore : Infrastructure.Abstract.IKeyValueDataStore

    {
        public void Remove(string key)
        {
            if (false == String.IsNullOrWhiteSpace(key))
            {
                HttpContext.Current.Application.Remove(key);
            }
        }

        public void Set(string key, string value)
        {
            if (false == String.IsNullOrWhiteSpace(key))
            {
                HttpContext.Current.Application[key] = value;
            }
        }

        public string Get(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                return String.Empty;
            }
            return (string)HttpContext.Current.Application[key];
        }

        public IEnumerable<KeyValuePair<string, string>> GetAll()
        {
            HttpApplicationState sessionState = HttpContext.Current.Application;
            Dictionary<string, string> rv = new Dictionary<string, string>();
            foreach (object key in sessionState.Keys)
            {
                string keyString = key.ToString();
                rv.Add(keyString, (sessionState[keyString] == null ? "" : sessionState[keyString].ToString()));
            }
            return rv;
        }
    }
}
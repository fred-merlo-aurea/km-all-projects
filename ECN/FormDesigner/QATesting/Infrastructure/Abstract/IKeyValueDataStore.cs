using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecn.qatools.Infrastructure.Abstract
{
    public interface IKeyValueDataStore
    {
        void Remove(string key);
        void Set(string key, string value);
        string Get(string key);
        IEnumerable<KeyValuePair<string, string>> GetAll();
    }
}

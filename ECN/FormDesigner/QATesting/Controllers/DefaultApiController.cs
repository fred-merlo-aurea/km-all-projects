using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ecn.qatools.Controllers
{
    public class DefaultApiController : ApiController
    {
        Infrastructure.Abstract.IKeyValueDataStore DataStore { get; set; }
        public DefaultApiController(Infrastructure.Abstract.IKeyValueDataStore dataStore)
        {
            DataStore = dataStore;
        }
        
        [HttpGet]
        public IEnumerable<KeyValuePair<string, string>> GetAll()
        {
            return DataStore.GetAll();
        }

        [HttpGet]
        public string Get(string id)
        {
            return DataStore.Get(id);
        }

        [HttpPost]
        public void Post(string id, [FromBody] string value)
        {
            DataStore.Set(id, value);
        }

        [HttpDelete]
        public void Delete(string id)
        {
            DataStore.Remove(id);
        }
    }
}

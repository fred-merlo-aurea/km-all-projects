using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ECN.Tests.Helpers
{
    public class TestHelperException<T> : HttpException
        where T : Exception, new()
    {
        public override Exception GetBaseException()
        {
            var exp = base.GetBaseException();
            return new T();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KM.Integration.Marketo
{
    public class Response
    {
        public string requestId { get; set; }
        public string success { get; set; }
        public List<Result> result { get; set; }
        public List<Error> errors { get; set; }

    }
}

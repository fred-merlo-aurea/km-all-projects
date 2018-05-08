using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class BaseWrapper
    {
        public BaseWrapper()
        {
            Errors = new List<ECN_Framework_Common.Objects.ECNError>();
        }
        public List<ECN_Framework_Common.Objects.ECNError> Errors { get; set; }
    }
}
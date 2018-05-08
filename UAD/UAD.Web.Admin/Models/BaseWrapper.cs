using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAD.Web.Admin.Models
{
    public class BaseWrapper
    {
        public BaseWrapper()
        {
            Errors = new List<FrameworkUAD.Object.UADError>();
        }
        public List<FrameworkUAD.Object.UADError> Errors { get; set; }
    }
}
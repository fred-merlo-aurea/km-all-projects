﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.communicator.mvc.Models
{
    public class CopyUDF
    {
        public CopyUDF() { }

        public int CurrentGroupID { get; set; }

        public List<ECN_Framework_Entities.Communicator.Group> Groups { get; set; }
    }
}
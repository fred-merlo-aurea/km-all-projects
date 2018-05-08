using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.MarketingAutomation.Models.PostModels
{
    public class DiagramJsonObject
    {
        public DiagramJsonObject()
        {
            shapes = new List<PostModels.ControlBase>();
            connections = new List<Connector>();
        }

        public List<ControlBase> shapes { get; set; }

        public List<Connector> connections { get; set; }
    }
}
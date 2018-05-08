using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.MarketingAutomation.Models.PostModels
{
    public class Connector
    {
        public Connector()
        {
            cursor = "pointer";
            selectable = true;
            serializable = true;
            enable = true;
            startCap = "None";
            endCap = "ArrowEnd";
            fromConnector = "Auto";
            toConnector = "Auto";
            stroke = new stroke();
            selection = new selection();
            type = "cascading";
            from = new from();
            to = new to();
            id = "";
            content = new content() { align = "center middle", color = "#2e2e2e" };
            editable = new shapeEditable() { remove = true, snap = false };
            HeatMapStats = 0;
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "HeatMapStats")]
        public int? HeatMapStats
        {
            get; set;
        }

        [Newtonsoft.Json.JsonProperty(PropertyName = "MAConnectorID")]
        public int MAConnectorID { get; set; }

        public hover hover { get { return new hover() { stroke = new stroke(2, "#CCCCCC") }; } }

        public string cursor { get; set; }

        public content content { get; set; }

        public bool selectable { get; set; }

        public bool serializable { get; set; }

        public bool enable { get; set; }

        public string startCap { get; set; }

        public string endCap { get; set; }

        public string fromConnector { get; set; }

        public stroke stroke { get; set; }

        public from from { get; set; }

        public to to { get; set; }

        public string toConnector { get; set; }

        public selection selection { get; set; }

        public string type { get; set; }

        public shapeEditable editable { get; set; }

        public static List<Connector> GetPostModelFromObject(List<ECN_Framework_Entities.Communicator.MAConnector> connectors)
        {
            List<Connector> retList = new List<PostModels.Connector>();
            foreach(ECN_Framework_Entities.Communicator.MAConnector ma in connectors)
            {
                Connector c = new PostModels.Connector();
                c.from = new from() { shapeId = ma.From };
                c.to = new to() { shapeId = ma.To };
                c.MAConnectorID = ma.ConnectorID;
                c.id = ma.ControlID;
                retList.Add(c);
            }
            
            return retList;
        }

        public static List<Connector> GetConnectorsForCopy(List<ECN_Framework_Entities.Communicator.MAConnector> connectors, int MAID)
        {
            List<PostModels.Connector> retList = new List<PostModels.Connector>();
            foreach (ECN_Framework_Entities.Communicator.MAConnector conn in connectors)
            {
                Connector c = new PostModels.Connector();
                c.from = new from() { shapeId = conn.From };
                c.to = new to() { shapeId = conn.To };
                c.MAConnectorID = -1;
                c.id = conn.ControlID;
                
                retList.Add(c);
            }
            return retList;
        }

        public static List<Connector> GetConnectorsForCopy(List<Connector> connectors, int MAID)
        {
            List<PostModels.Connector> retList = new List<PostModels.Connector>();
            foreach (Connector conn in connectors)
            {
                Connector c = new PostModels.Connector();
                c.from = new from() { shapeId = conn.from.shapeId };
                c.to = new to() { shapeId = conn.to.shapeId };
                c.MAConnectorID = -1;
                c.id = conn.id;

                retList.Add(c);
            }
            return retList;
        }

      
        public static void GetControlsToRemove(List<ControlBase> origControls, List<Connector> origConnectors,ref List<ControlBase> controls,ref List<Connector> connectors, string ControlID)
        {
            List<PostModels.Connector> retList = new List<PostModels.Connector>();
            List<ControlBase> retControlList = new List<PostModels.ControlBase>();
            Connector ToConn = origConnectors.FirstOrDefault(x => x.to.shapeId == ControlID);//this is the connector that points to the control that is cancelled
            Connector FromConn = origConnectors.FirstOrDefault(x => x.from.shapeId == ControlID);//this is the connector that points away from the control
            retControlList.Add(origControls.FirstOrDefault(x => x.ControlID == ControlID));
            retList.Add(ToConn);
            retList.Add(FromConn);
            foreach (ControlBase con in origControls)
            {

                if (con.ControlID == FromConn.to.shapeId)
                {
                    if(!retControlList.Exists(x => x.ControlID == con.ControlID))
                          retControlList.Add(con);

                    if (con.ControlType != ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.End)
                    {
                        FromConn = origConnectors.FirstOrDefault(x => x.from.shapeId == con.ControlID);
                        retList.Add(ToConn);
                        retList.Add(FromConn);
                    }
                    else
                    {
                       break;
                    }
                }
            }
            controls.AddRange(retControlList);
            connectors.AddRange(retList);
        }

    }

    #region other Properties/Classes

    public class selection
    {
        public selection() {
            handles = new handles();
        }

        public handles handles { get; set; }
    }

    public class handles
    {
        public handles() {
            width = 8;
            height = 8;
            fill = new fill("#fff");
            stroke = new stroke(2,"#282828");
        }
        public  int width { get; set; }

        public  int height { get; set; }


        public  fill fill { get; set; }

        public  stroke stroke { get; set; }


    }

    public class fill
    {
        public fill()
        {

        }

        public fill(string startColor)
        {
            color = startColor;
        }
        public  string color { get; set; }
    }

    public class stroke
    {
        public stroke() {
            width = 2;
            color = "#586477";
        }

        public stroke(int startWidth, string startColor)
        {
            width = startWidth;
            color = startColor;
        }

        public  int width { get; set; }
        public  string color { get; set; }
    }

    public class from
    {
        public from() { }

        public string shapeId { get; set; }
    }

    public class to
    {
        public to() { }

        public  string shapeId { get; set; }
    }

    public class content
    {
        public content() { }

        public  string align { get; set; }

        public  string color { get; set; }
    }

    public class editable
    {
        public editable()
        {
            drag = new drag();
            remove = true;
        }

        public  drag drag { get; set; }

        public  bool remove { get; set; }
        public bool connect { get { return true; } }

        public List<string> tools { get { return new List<string>(); } }

    }

    public class drag
    {
        public drag() {
            snap = new snap();
        }

        public  snap snap { get; set; }
    }

    public class snap
    {
        public snap() {
            size = 10;
            angle = 10;
        }

        public  int size { get; set; }

        public  int angle { get; set; }

    }

    public class hover
    {
        public hover()
        {
            stroke = null;
        }

        public stroke stroke { get; set; }
    }

    #endregion
}
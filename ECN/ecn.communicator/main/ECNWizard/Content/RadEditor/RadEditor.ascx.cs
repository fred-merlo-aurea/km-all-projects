using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace ecn.communicator.main.ECNWizard.Content.RadEditor
{
    public partial class RadEditor : System.Web.UI.UserControl
    {
        public string Text { 
            get{ return radEditor.Content; }
            set { radEditor.Content = value; }
        }

        public bool UseDivMode
        {
            get { return radEditor.ContentAreaMode == Telerik.Web.UI.EditorContentAreaMode.Div ? true : false; }
            set { radEditor.ContentAreaMode = value ? Telerik.Web.UI.EditorContentAreaMode.Div : Telerik.Web.UI.EditorContentAreaMode.Iframe;}
        }

        public string[] EditModes
        {
            set
            {
                bool hasDesign = false;
                bool hasHTML = false;
                bool hasPreview = false;
                if (value.Contains("Design"))
                    hasDesign = true;
                if (value.Contains("HTML"))
                    hasHTML = true;
                if (value.Contains("Preview"))
                    hasPreview = true;

                if (value.Length == 1)
                {
                    if (hasDesign)
                        radEditor.EditModes = Telerik.Web.UI.EditModes.Design;
                    else if (hasHTML)
                        radEditor.EditModes = Telerik.Web.UI.EditModes.Html;
                    else if (hasPreview)
                        radEditor.EditModes = Telerik.Web.UI.EditModes.Preview;
                }
                else if (value.Length == 2)
                {
                    if (hasDesign && hasHTML)
                    {
                        radEditor.EditModes = Telerik.Web.UI.EditModes.Design | Telerik.Web.UI.EditModes.Html;
                    }
                    else if (hasDesign && hasPreview)
                    {
                        radEditor.EditModes = Telerik.Web.UI.EditModes.Design | Telerik.Web.UI.EditModes.Preview;
                    }
                    else
                        radEditor.EditModes = Telerik.Web.UI.EditModes.Preview | Telerik.Web.UI.EditModes.Html;
                }
                else
                    radEditor.EditModes = Telerik.Web.UI.EditModes.All;

                
                
            }
        }

        public void SetText(string txt)
        {
            radEditor.Content = txt;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            KMPlatform.Entity.User user = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            radEditor.ImageManager.ViewPaths = new string[] { "/ecn.images/Customers/" + user.CustomerID.ToString() + "/images" };
            radEditor.ImageManager.UploadPaths = new string[] { "/ecn.images/Customers/" + user.CustomerID.ToString() + "/images" };
            radEditor.DocumentManager.ViewPaths = new string[] { "/ecn.images/Customers/" + user.CustomerID.ToString() + "/data" };
            radEditor.DocumentManager.UploadPaths = new string[] { "/ecn.images/Customers/" + user.CustomerID.ToString() + "/data" };
        }
    }
}
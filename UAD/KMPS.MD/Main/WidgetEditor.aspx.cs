

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;
using Kalitte.Dashboard.Framework.Utility;

namespace KMPS.MD.Main
{
    public partial class WidgetEditor : KMPS.MD.Main.WebPageHelper
    {
        private WidgetInstance instance = null;
        private UserControl editor = null;

        private IWidgetEditor Editor
        {
            get
            {
                return EditorControl as IWidgetEditor;
            }
        }

        private UserControl EditorControl
        {
            get
            {
                if (editor == null)
                {
                    try
                    {
                        UserControl c = Page.LoadControl(Page.ResolveUrl(Instance.Type.EditorPath)) as UserControl;
                        if (!(c is IWidgetEditor))
                            throw new InvalidOperationException("Editor must implement IWidgetEditor");
                        editor = c;
                    }
                    catch (Exception exc)
                    {
                        throw new ArgumentException(string.Format("Cannot load {0}", Instance.Type.EditorPath), exc);
                    }
                }
                return editor;
            }
        }

        private WidgetInstance Instance
        {
            get
            {
                if (instance == null)
                {
                    string id = Request["ID"];
                    instance = Kalitte.Dashboard.Framework.DashboardFramework.GetWidgetInstance(id);
                }
                return instance;
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Instance.Type.EditorPath))
                    Editor.Edit(Instance.InstanceKey);
            }
        }


        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            if (!string.IsNullOrEmpty(Instance.Type.EditorPath))
                pc.Controls.Add(EditorControl);
        }

        private void Done(string value)
        {
            Dictionary<string, object> pr = new Dictionary<string, object>();
            bool editorResult = true;
            if (!string.IsNullOrEmpty(Instance.Type.EditorPath))
                editorResult = Editor.EndEdit(pr);
            if (editorResult)
            {
                hdnValue.Value = value;
                if (Request.Cookies["wp"] != null && !string.IsNullOrEmpty(Request.Cookies["wp"].Value))
                {
                    pr.Add("WidgetPropertiesUpdated", true);
                    Response.Cookies["wp"].Value = null;
                }
                if (pr.Count == 0)
                    arguments.Value = "null";
                else
                {
                    arguments.Value = Utils.Serialize(pr);
                }

                ScriptManager2.AddScript("javascript:load();");
            }
        }


        protected void btnOk_Click(object sender, EventArgs e)
        {
            Done("done");
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            Done("apply");
        }


        protected void ctnEditWidget_Click(object sender, EventArgs e)
        {
            WidgetEditor1.DataBind(Instance, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WidgetEditor1.DataBind(Instance, string.IsNullOrEmpty(Instance.Type.EditorPath));
            }
        }

        protected void WidgetEditor1_SaveDone(object sender, EventArgs e)
        {
            Response.Cookies["wp"].Value = "1";
        }

    }

}
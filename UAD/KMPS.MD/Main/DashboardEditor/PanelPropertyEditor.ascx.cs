using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Dashboard.Framework;
using Kalitte.Dashboard.Framework.Types;

namespace KMPS.MD.Main.DashboardEditor
{
    public partial class PanelPropertyEditor : System.Web.UI.UserControl
    {
        public PanelInstance Instance { get; set; }

        private void FillIcons(DropDownList list)
        {
            foreach (string item in Enum.GetNames(typeof(WidgetIcon)))
                list.Items.Add(new ListItem(item, item));
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            FillIcons(ctlDashboardIcon);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void DataBind()
        {
            BindInstance();
            base.DataBind();
        }

        private void BindInstance()
        {
            ctlDashboardIcon.SelectedValue = Instance.Icon.ToString();
            ctlDashboardWidth.Text = Instance.Width.HasValue ? Instance.Width.Value.ToString() : "";
            ctlDashboardHeight.Text = Instance.Height.HasValue ? Instance.Height.Value.ToString() : "";
            ctlBodyStyle.Text = Instance.BodyStyle;
            ctlBodyCssClass.Text = Instance.BodyCssClass;
            ctlStyleSpec.Text = Instance.StyleSpec;
            ctlBaseCls.Text = Instance.BaseCls;
            ctlCls.Text = Instance.Cls;
            ctlCollapsedCls.Text = Instance.CollapsedCls;
            ctlIconCls.Text = Instance.IconCls;
            ctlBorder.Checked = Instance.Border;
            ctlBodyBorder.Checked = Instance.BodyBorder;
            ctlCollapsible.Checked = Instance.Collapsible;
            ctlHideCollapseTool.Checked = Instance.HideCollapseTool;
            ctlAutoWidth.Checked = Instance.AutoWidth;
            ctlAutoHeight.Checked = Instance.AutoHeight;
            ctlCtCls.Text = Instance.CtCls;
            ctlEnabled.Checked = Instance.Enabled;
            ctlDisabledClass.Text = Instance.DisabledClass;
            ctlFrame.Checked = Instance.Frame;
            ctlDashboardHeader.Checked = Instance.Header;
            ctlPadding.Text = Instance.Padding.ToString();
            ctlPaddingSummary.Text = Instance.PaddingSummary;
            ctlShim.Checked = Instance.Shim;
            ctlTitleCollapse.Checked = Instance.TitleCollapse;
            ctlUnstyled.Checked = Instance.Unstyled;
            ctlDragable.Checked = Instance.Dragable;
            ctlAutoScroll.Checked = Instance.AutoScroll;
            ctlFitLayout.Checked = Instance.FitLayout;
            ctlStretch.Checked = Instance.Stretch;
            ctlEnabled.Checked = Instance.Enabled;
        }

        public void EndEdit()
        {
            Instance.Icon = (WidgetIcon)Enum.Parse(typeof(WidgetIcon), ctlDashboardIcon.SelectedValue);
            if (string.IsNullOrEmpty(ctlDashboardWidth.Text))
                Instance.Width = null;
            else Instance.Width = int.Parse(ctlDashboardWidth.Text);
            if (string.IsNullOrEmpty(ctlDashboardHeight.Text))
                Instance.Height = null;
            else Instance.Height = int.Parse(ctlDashboardHeight.Text);

            Instance.BodyStyle = ctlBodyStyle.Text;
            Instance.BodyCssClass = ctlBodyCssClass.Text;
            Instance.StyleSpec = ctlStyleSpec.Text;
            Instance.BaseCls = ctlBaseCls.Text;
            Instance.Cls = ctlCls.Text;
            Instance.CollapsedCls = ctlCollapsedCls.Text;
            Instance.IconCls = ctlIconCls.Text;
            Instance.Border = ctlBorder.Checked;
            Instance.BodyBorder = ctlBodyBorder.Checked;
            Instance.Collapsible = ctlCollapsible.Checked;
            Instance.HideCollapseTool = ctlHideCollapseTool.Checked;
            Instance.AutoWidth = ctlAutoWidth.Checked;
            Instance.AutoHeight = ctlAutoHeight.Checked;
            Instance.CtCls = ctlCtCls.Text;
            Instance.Enabled = ctlEnabled.Checked;
            Instance.DisabledClass = ctlDisabledClass.Text;
            Instance.Frame = ctlFrame.Checked;
            Instance.Header = ctlDashboardHeader.Checked;

            if (string.IsNullOrEmpty(ctlPadding.Text))
                Instance.Padding = 5;
            else Instance.Padding = int.Parse(ctlPadding.Text);
            Instance.PaddingSummary = ctlPaddingSummary.Text;
            Instance.Shim = ctlShim.Checked;
            Instance.TitleCollapse = ctlTitleCollapse.Checked;
            Instance.Unstyled = ctlUnstyled.Checked;
            Instance.Dragable = ctlDragable.Checked;
            Instance.AutoScroll = ctlAutoScroll.Checked;
            Instance.FitLayout = ctlFitLayout.Checked;
            Instance.Stretch = ctlStretch.Checked;
            Instance.Enabled = ctlEnabled.Checked;
        }
    }
}
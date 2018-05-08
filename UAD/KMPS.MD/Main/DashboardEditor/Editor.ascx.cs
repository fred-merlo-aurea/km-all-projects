

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;
using System.Threading;
using System.Web.UI.HtmlControls;
using Kalitte.Dashboard.Framework.Providers;
using System.Resources;
using System.Reflection;
using System.Globalization;
using KMPS.MD.Main.DashboardEditor.App_LocalResources;


namespace KMPS.MD.Main.DashboardEditor
{
    public partial class Editor : System.Web.UI.UserControl, IWidgetEditor
    {
        object initialDashboard = null;

        protected override void OnError(EventArgs e)
        {
            Exception exc = Server.GetLastError();
            Server.ClearError();
            DoError(exc.Message);
        }



        private void BindDashboardColumns(DashboardSectionInstance row)
        {
            DashboardColumnDiv.Visible = row != null;
            if (DashboardColumnDiv.Visible)
            {
                ColumnsGrid.DataSource = row.Columns;
                ColumnsGrid.DataBind();
                ColumnsGrid.SelectedIndex = 0;
                ColumnsGrid_SelectedIndexChanged(this, EventArgs.Empty);
                //ctlColumnsLabel.Text = row.Title;
                //if (string.IsNullOrEmpty(row.Title))
                //    ctlColumnsLabel.Text = AppResource.EmptyColumnLabelText;
                //else ctlColumnsLabel.Text = string.Format(AppResource.SelectedColumnLabelText, row.Title);
            }
            ctlcolumnsUpdatePanel.Update();
        }

        private void BindDashboardAuthorization(DashboardInstance instance)
        {
            DashboardAuthDiv.Visible = instance != null;
            if (DashboardAuthDiv.Visible)
            {
                ctlAuthGrid.DataSource = instance.AuthorizationInfo;
                ctlAuthGrid.DataBind();
            }
        }

        private void BindDashboards(object key)
        {
            BindDashboards(key, -1);
        }
        private void BindDashboards(object key, int selectedIndex)
        {
            var list = (DashboardFramework.GetDashboards().OrderBy(p=>p.Title).OrderBy(p=>p.DisplayOrder).OrderBy(p=>p.Group).OrderBy(p=>p.GroupDisplayOrder)).ToList();
            DashboardGrid.DataSource = list;
            DashboardGrid.DataBind();
            if (key != null)
            {
                DashboardInstance instance = list.SingleOrDefault(p => p.InstanceKey.ToString() == key.ToString());
                if (instance == null)
                    return;
                DashboardGrid.SelectedIndex = list.IndexOf(instance);
                BindDashboardRows(instance);
                
                BindDashboardAuthorization(instance);
                ShowDashboard(instance);
            }
            else
            {
                if (selectedIndex >= 0)
                {
                    DashboardGrid.SelectedIndex = selectedIndex;
                    ShowDashboard(list[selectedIndex]);
                    BindDashboardRows(list[selectedIndex]);
                    BindDashboardAuthorization(list[selectedIndex]);
                }
                else if (list.Count > 0)
                {
                    DashboardGrid.SelectedIndex = 0;
                    ShowDashboard(list[0]);
                    BindDashboardRows(list[0]);
                    BindDashboardAuthorization(list[0]);
                }
                else
                {
                    BindDashboardColumns(null);
                    BindDashboardAuthorization(null);
                }
            }
        }

        private void BindDashboardRows(DashboardInstance instance)
        {
            if (instance.Rows.Count == 0)
            {
                instance.CreateDefaultRows();
                DashboardFramework.UpdateDashboard(instance);
            }
            RowsGrid.DataSource = instance.Rows;
            RowsGrid.DataBind();
            RowsGrid.SelectedIndex = 0;
            RowsGrid_SelectedIndexChanged(this, EventArgs.Empty);
            ctlRowsUpdatePanel.Update();
            BindDashboardColumns(instance.Rows[0]);
            
        }

        private void ShowDashboard(DashboardInstance entity)
        {
            ctlDashboardTitle.Text = entity.Title;
            ctlDashboardDescription.Text = entity.Description;
            ctlDashboardSharing.SelectedValue = entity.ShareType.ToString();
            ctlDashboardGroup.Text = entity.Group;
            ctlGroupDispOrder.Text = entity.GroupDisplayOrder.HasValue ? entity.GroupDisplayOrder.Value.ToString() : "";
            ctlDashboardOrder.Text = entity.DisplayOrder.HasValue ? entity.DisplayOrder.ToString() : "";
            ctlCurrentDashboardLabel.Text = entity.Title;
            //if (!string.IsNullOrEmpty(entity.Title))
            //    ctlRowLabel.Text = string.Format(AppResource.SelectedRowLabelText, entity.Title);
            //else
            //{
            //    ctlRowLabel.Text = AppResource.EmptySelectedRowLabelText;
            //}
            ctlDashboardTag.Text = entity.UserTag;
            ctLID.Text = entity.InstanceKey.ToString();
            ctlDisplayMode.SelectedValue = entity.ViewMode.ToString();
            ctlUrl.Text = entity.Url;
            ctlDashboardPanel.Instance = entity;
            ctlDashboardPanel.DataBind();
        }

        private void BindDashboards()
        {
            BindDashboards(initialDashboard);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindDashboards();
            }
        }



        #region IWidgetEditor Members

        public void Edit(object instanceKey)
        {
            if (instanceKey != null)
            {
                initialDashboard = instanceKey;
            }
        }

        public bool EndEdit(Dictionary<string, object> arguments)
        {
            if (ViewState["ColumnsDirty"] != null)
                arguments["ColumnsDirty"] = true;
            return true;
        }

        #endregion


        protected void DashboardGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            object instanceKey = DashboardGrid.SelectedDataKey.Value;
            DashboardInstance entity = DashboardFramework.GetDashboard(instanceKey);
            ShowDashboard(entity);
            BindDashboardRows(entity);
        }

        protected void ctlDashboardCreate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                DashboardInstance instance = new DashboardInstance();
                instance.InstanceKey = Guid.NewGuid();
                instance.Username = Thread.CurrentPrincipal.Identity.Name;
                GetInstanceFromUser(instance, true);
                instance.ViewMode = DashboardViewMode.CurrentPage;
                instance.Url = string.Empty;
                DashboardFramework.CreateDashboard(instance);
                BindDashboards(instance.InstanceKey);
            }
        }

        private void GetInstanceFromUser(DashboardInstance instance, bool fillLayout)
        {
            instance.Title = ctlDashboardTitle.Text;            
            instance.Description = ctlDashboardDescription.Text;
            instance.ShareType = (DashboardShareType)Enum.Parse(typeof(DashboardShareType), ctlDashboardSharing.SelectedValue);
            instance.Group = ctlDashboardGroup.Text;
            if (!string.IsNullOrEmpty(ctlGroupDispOrder.Text))
                instance.GroupDisplayOrder = int.Parse(ctlGroupDispOrder.Text);
            else instance.GroupDisplayOrder = null;
            if (!string.IsNullOrEmpty(ctlDashboardOrder.Text))
                instance.DisplayOrder = int.Parse(ctlDashboardOrder.Text);
            else instance.DisplayOrder = null;
            instance.UserTag = ctlDashboardTag.Text;
            instance.Url = ctlUrl.Text;
            instance.ViewMode = (DashboardViewMode)Enum.Parse(typeof(DashboardViewMode), ctlDisplayMode.SelectedValue);

            if (fillLayout)
            {
                DashboardSectionInstance section1 = new DashboardSectionInstance(instance.InstanceKey, 0, Guid.NewGuid());
                DashboardSectionInstance section2 = new DashboardSectionInstance(instance.InstanceKey, 1, Guid.NewGuid());

                section1.Columns.Add(new DashboardColumn(section1.InstanceKey, 0, 40, null, ""));
                section1.Columns.Add(new DashboardColumn(section1.InstanceKey, 1, 40, null, ""));
                section1.Columns.Add(new DashboardColumn(section1.InstanceKey, 2, 20, null, ""));

                section2.Columns.Add(new DashboardColumn(section2.InstanceKey, 0, 50, null, ""));
                section2.Columns.Add(new DashboardColumn(section2.InstanceKey, 1, 50, null, ""));

                instance.Rows.Add(section1);
                instance.Rows.Add(section2);
            }


        }

        protected void ctlDashboardUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (DashboardGrid.SelectedDataKey == null)
                    return;
                object instanceKey = DashboardGrid.SelectedDataKey.Value;
                int selectedIndex = DashboardGrid.SelectedIndex;
                if (instanceKey != null)
                {
                    DashboardInstance instance = DashboardFramework.GetDashboard(instanceKey);
                    GetInstanceFromUser(instance, false);
                    ctlDashboardPanel.Instance = instance;
                    ctlDashboardPanel.EndEdit();
                    DashboardFramework.UpdateDashboard(instance);
                    BindDashboards(null, selectedIndex);
                }
            }

        }

        protected void ctlDashboardDelete_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (DashboardGrid.SelectedDataKey == null)
                    return;
                object instanceKey = DashboardGrid.SelectedDataKey.Value;
                if (instanceKey != null)
                {
                    DashboardInstance instance = DashboardFramework.GetDashboard(instanceKey);
                    DashboardFramework.DeleteDashboard(instance);
                    BindDashboards();
                }
            }
        }

        protected void DashboardColumnCreate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && DashboardGrid.SelectedDataKey != null && RowsGrid.SelectedDataKey != null)
            {
                object instanceKey = DashboardGrid.SelectedDataKey.Value;
                object rowInstanceKey = RowsGrid.SelectedDataKey.Value;
                if (instanceKey != null)
                {
                    DashboardInstance instance = DashboardFramework.GetDashboard(instanceKey);
                    int? colPadding = null;
                    if (!string.IsNullOrEmpty(ctlColPadding.Text))
                        colPadding = int.Parse(ctlColPadding.Text);

                    DashboardSectionInstance row = instance.Rows.SingleOrDefault(p => p.InstanceKey.ToString() == rowInstanceKey.ToString());
                    DashboardColumn col = new DashboardColumn(instance.InstanceKey, row.Columns.Count, int.Parse(ctlColWithBox.Text), colPadding, ctlColStyle.Text);
                    if (row != null)
                    {

                        row.Columns.Add(col);
                    }
                    DashboardFramework.UpdateDashboard(instance);
                    BindDashboardColumns(row);
                    ColumnsGrid.SelectedIndex = row.Columns.IndexOf(col);
                    ViewState["ColumnsDirty"] = true;
                }
            }
        }

        protected void ColumnsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            object instanceKey = DashboardGrid.SelectedDataKey.Value;
            object rowInstanceKey = RowsGrid.SelectedDataKey.Value;

            DashboardInstance entity = DashboardFramework.GetDashboard(instanceKey);
            DashboardSectionInstance row = entity.Rows.SingleOrDefault(p => p.InstanceKey.ToString() == rowInstanceKey.ToString());

            int colKey = int.Parse(ColumnsGrid.SelectedDataKey.Value.ToString());
            DashboardColumn col = row.Columns.SingleOrDefault(p => p.ColumnOrder == colKey);
            ctlColWithBox.Text = col.WidthInPercent.ToString();
            ctlColStyle.Text = col.StyleSpec;
            ctlColPadding.Text = col.Padding.HasValue ? col.Padding.ToString() : "";
            ctlColResize.Checked = col.Settings.ColumnResizableSettings.IsResizable;
            ctlSaveResize.Checked = col.Settings.ColumnResizableSettings.Save;
        }

        protected void DashboardColumnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && DashboardGrid.SelectedDataKey != null && RowsGrid.SelectedDataKey != null)
            {
                object instanceKey = DashboardGrid.SelectedDataKey.Value;
                object rowInstanceKey = RowsGrid.SelectedDataKey.Value;

                if (ColumnsGrid.SelectedDataKey != null && instanceKey != null && rowInstanceKey != null)
                {
                    

                    int colKey = int.Parse(ColumnsGrid.SelectedDataKey.Value.ToString());
                    DashboardInstance instance = DashboardFramework.GetDashboard(instanceKey);
                    DashboardSectionInstance row = instance.Rows.SingleOrDefault(p => p.InstanceKey.ToString() == rowInstanceKey.ToString());
                    DashboardColumn col = row.Columns.SingleOrDefault(p => p.ColumnOrder == colKey);
                    col.WidthInPercent = int.Parse(ctlColWithBox.Text);
                    if (string.IsNullOrEmpty(ctlColPadding.Text))
                        col.Padding = null;
                    else col.Padding = int.Parse(ctlColPadding.Text);
                    col.StyleSpec = ctlColStyle.Text;

                    col.Settings.ColumnResizableSettings.IsResizable = ctlColResize.Checked;
                    col.Settings.ColumnResizableSettings.Save = ctlSaveResize.Checked;

                    DashboardFramework.UpdateDashboard(instance);
                    BindDashboardColumns(row);
                    ColumnsGrid.SelectedIndex = row.Columns.IndexOf(col);
                    ViewState["ColumnsDirty"] = true;
                }
            }
        }

        protected void DashboardColumnCreateDelete_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                object instanceKey = DashboardGrid.SelectedDataKey.Value;
                object rowInstanceKey = RowsGrid.SelectedDataKey.Value;

                if (ColumnsGrid.SelectedDataKey != null && instanceKey != null && rowInstanceKey != null)
                {
                    int colKey = int.Parse(ColumnsGrid.SelectedDataKey.Value.ToString());
                    DashboardInstance instance = DashboardFramework.GetDashboard(instanceKey);
                    DashboardSectionInstance row = instance.Rows.SingleOrDefault(p => p.InstanceKey.ToString() == rowInstanceKey.ToString());

                    DashboardColumn col = row.Columns.SingleOrDefault(p => p.ColumnOrder == colKey);
                    if (col != row.Columns[row.Columns.Count - 1])
                        DoError("YouCanOnlyDeleteLastColum");
                    else if (row.Columns.Count == 1)
                        DoError("CannotDeleteDefaultColum");
                    else
                    {
                        row.Columns.Remove(col);
                        DashboardFramework.DeleteWidgetsByColumn(instance.InstanceKey, row.InstanceKey, col.ColumnOrder.ToString());
                        DashboardFramework.UpdateDashboard(instance);
                        ViewState["ColumnsDirty"] = true;
                        BindDashboardColumns(row);
                    }
                }
            }
        }

        private void DoError(string msg)
        {
            errLabel.Text = msg;
        }



        protected void ctlAuthGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            object instanceKey = DashboardGrid.SelectedDataKey.Value;
            DashboardInstance instance = DashboardFramework.GetDashboard(instanceKey);

            string key = ctlAuthGrid.SelectedDataKey.Value.ToString();
            DashboardAuthorizationInfo info = instance.AuthorizationInfo.SingleOrDefault(p => p.Role == key);
            ctlRoleName.Text = info.Role;
            ctlCanEdit.Checked = info.CanEdit;
            ctlCanSee.Checked = info.CanView;

        }

        protected void ctlCreateAuthBtn_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                object instanceKey = DashboardGrid.SelectedDataKey.Value;
                if (instanceKey != null)
                {
                    DashboardInstance instance = DashboardFramework.GetDashboard(instanceKey);
                    instance.AuthorizationInfo.Add(new DashboardAuthorizationInfo(ctlRoleName.Text, ctlCanSee.Checked, ctlCanEdit.Checked));
                    DashboardFramework.UpdateDashboard(instance);
                    BindDashboardAuthorization(instance);
                    ViewState["AuthDirty"] = true;
                }
            }
        }

        protected void ctlUpdateAuthBtn_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                object instanceKey = DashboardGrid.SelectedDataKey.Value;
                if (ctlAuthGrid.SelectedDataKey != null && instanceKey != null)
                {
                    string key = ctlAuthGrid.SelectedDataKey.Value.ToString();
                    Kalitte.Dashboard.Framework.Types.DashboardInstance instance = DashboardFramework.GetDashboard(instanceKey);

                    DashboardAuthorizationInfo info = instance.AuthorizationInfo.SingleOrDefault(p => p.Role == key);
                    info.CanView = ctlCanSee.Checked;
                    info.CanEdit = ctlCanEdit.Checked;
                    info.Role = ctlRoleName.Text;

                    DashboardFramework.UpdateDashboard(instance);
                    BindDashboardAuthorization(instance);
                    ViewState["AuthDirty"] = true;
                }
            }
        }

        protected void ctlDeleteAuthBtn_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                object instanceKey = DashboardGrid.SelectedDataKey.Value;
                if (ctlAuthGrid.SelectedDataKey != null && instanceKey != null)
                {
                    string key = ctlAuthGrid.SelectedDataKey.Value.ToString();
                    DashboardInstance instance = DashboardFramework.GetDashboard(instanceKey);
                    DashboardAuthorizationInfo info = instance.AuthorizationInfo.SingleOrDefault(p => p.Role == key);
                    instance.AuthorizationInfo.Remove(info);
                    DashboardFramework.UpdateDashboard(instance);
                    ViewState["AuthDirty"] = true;
                    BindDashboardAuthorization(instance);
                }
            }
        }

        protected void RowsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            object instanceKey = DashboardGrid.SelectedDataKey.Value;
            object rowInstanceKey = RowsGrid.SelectedDataKey.Value;

            DashboardInstance entity = DashboardFramework.GetDashboard(instanceKey);
            DashboardSectionInstance row = entity.Rows.SingleOrDefault(p => p.InstanceKey.ToString() == rowInstanceKey.ToString());
            ctlRowTitle.Text = row.Title;
            ctlRowPanelEditor.Instance = row;
            ctlRowPanelEditor.DataBind();
            BindDashboardColumns(row);
        }

        protected void DashboardRowCreate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                object instanceKey = DashboardGrid.SelectedDataKey.Value;
                if (instanceKey != null)
                {
                    DashboardInstance instance = DashboardFramework.GetDashboard(instanceKey);
                    DashboardSectionInstance row = new DashboardSectionInstance(instance.InstanceKey, instance.Rows.Count, Guid.NewGuid());

                    row.Title = ctlRowTitle.Text;
                    ctlRowPanelEditor.Instance = row;
                    ctlRowPanelEditor.EndEdit();
                    row.CreateDefaultColumns();
                    instance.Rows.Add(row);

                    DashboardFramework.UpdateDashboard(instance);
                    BindDashboardRows(instance);
                    RowsGrid.SelectedIndex = instance.Rows.IndexOf(row);
                    BindDashboardColumns(row);
                    ViewState["RowsDirty"] = true;
                }
            }
        }

        protected void DashboardRowUpdate_Click(object sender, EventArgs e)
        {

            if (Page.IsValid && DashboardGrid.SelectedDataKey != null && RowsGrid.SelectedDataKey != null)
            {
                object instanceKey = DashboardGrid.SelectedDataKey.Value;
                object rowInstanceKey = RowsGrid.SelectedDataKey.Value;
                if (instanceKey != null && rowInstanceKey != null)
                {
                    DashboardInstance instance = DashboardFramework.GetDashboard(instanceKey);
                    DashboardSectionInstance row = instance.Rows.SingleOrDefault(p => p.InstanceKey.ToString() == rowInstanceKey.ToString());

                    row.Title = ctlRowTitle.Text;
                    ctlRowPanelEditor.Instance = row;
                    ctlRowPanelEditor.EndEdit();

                    DashboardFramework.UpdateDashboard(instance);
                    BindDashboardRows(instance);
                    RowsGrid.SelectedIndex = instance.Rows.IndexOf(row);
                    ViewState["RowsDirty"] = true;
                }
            }
        }

        protected void DashboardRowDelete_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                object instanceKey = DashboardGrid.SelectedDataKey.Value;
                object rowInstanceKey = RowsGrid.SelectedDataKey.Value;
                if (instanceKey != null && rowInstanceKey != null)
                {
                    DashboardInstance instance = DashboardFramework.GetDashboard(instanceKey);
                    DashboardSectionInstance row = instance.Rows.SingleOrDefault(p => p.InstanceKey.ToString() == rowInstanceKey.ToString());
                    instance.Rows.Remove(row);
                    for (int i = 0; i < instance.Rows.Count; i++)
                        instance.Rows[i].RowOrder = i;
                    DashboardFramework.DeleteWidgetsBySection(instance.InstanceKey, row.InstanceKey.ToString());
                    DashboardFramework.UpdateDashboard(instance);
                    BindDashboardRows(instance);
                    ViewState["RowsDirty"] = true;
                }
            }
        }


    }
}

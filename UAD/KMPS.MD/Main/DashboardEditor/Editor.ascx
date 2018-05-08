<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="KMPS.MD.Main.DashboardEditor.Editor" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Src="PanelPropertyEditor.ascx" TagName="PanelPropertyEditor" TagPrefix="uc2" %>
<div class="container">
    
    <div class="body nopad noborder">
        <asp:GridView ID="DashboardGrid" runat="server" AutoGenerateColumns="False" DataKeyNames="InstanceKey"
            OnSelectedIndexChanged="DashboardGrid_SelectedIndexChanged" Width="100%" meta:resourcekey="DashboardGridResource1" Visible="false">
            <EmptyDataTemplate>
                <asp:Label ID="Label1" runat="server" Text="There are no dashboards to display. You can create a dashboard and add widgets."
                    meta:resourcekey="Label1Resource1"></asp:Label>
            </EmptyDataTemplate>
            <Columns>
                <asp:CommandField ShowSelectButton="True" meta:resourcekey="CommandFieldResource1" />
                <asp:BoundField DataField="Title" HeaderText="Title" meta:resourcekey="BoundFieldResource1" />
                <asp:BoundField DataField="Group" HeaderText="Group" meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="GroupDisplayOrder" HeaderText="GroupDisplayOrder" meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField DataField="DisplayOrder" HeaderText="ItemDisplayOrder" meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField DataField="CreateDate" HeaderText="CreateDate" meta:resourcekey="BoundFieldResource5" />
            </Columns>
            <SelectedRowStyle BorderStyle="None" />
        </asp:GridView>
    </div>
</div>
  <asp:Label ID="errLabel" runat="server" EnableViewState="False" Font-Bold="True"
                ForeColor="#CC3300" meta:resourcekey="errLabelResource1"></asp:Label>

        <asp:Panel ID="Panel1" runat="server" Visible="false">
       
            <div >
                <h1>
                    <asp:Literal ID="ctlCurrentDashboardLabel" runat="server" Text="Dashboard Properties"
                        meta:resourcekey="ctlCurrentDashboardLabelResource1"></asp:Literal>
                </h1>
                <div class="body">
                    <table style="width:100%">
                        <tr>
                            <td colspan="3" style="width: 50%">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 125px">
                                <asp:Label ID="Label2" runat="server" Text="ID" meta:resourcekey="Label2Resource1"></asp:Label>
                            </td>
                            <td style="width: 75%">
                                <asp:Label ID="ctLID" runat="server" meta:resourcekey="ctLIDResource1"></asp:Label>
                            </td>
                            <td style="width: 25px; text-align: center">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 125px">
                                <asp:Label ID="Label3" runat="server" Text="Title" meta:resourcekey="Label3Resource1"></asp:Label>
                            </td>
                            <td style="width: 75%">
                                <asp:TextBox ID="ctlDashboardTitle" runat="server" ValidationGroup="Dashboard" Width="100%"
                                    meta:resourcekey="ctlDashboardTitleResource1"></asp:TextBox>
                            </td>
                            <td style="width: 25px; text-align: center">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ctlDashboardTitle"
                                    ErrorMessage="*" SetFocusOnError="True" ValidationGroup="Dashboard" meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Description" meta:resourcekey="Label4Resource1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="ctlDashboardDescription" runat="server" Rows="3" TextMode="MultiLine"
                                    ValidationGroup="Dashboard" Width="100%" meta:resourcekey="ctlDashboardDescriptionResource1"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Sharing" meta:resourcekey="Label5Resource1"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ctlDashboardSharing" runat="server" Width="100%" meta:resourcekey="ctlDashboardSharingResource1">
                                    <asp:ListItem Text="Dont' Share" Value="None" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                    <asp:ListItem Text="Share with my workgroup" Value="Workgroup" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                    <asp:ListItem Text="Share with everybody" Value="AllUsers" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Group Name" meta:resourcekey="Label6Resource1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="ctlDashboardGroup" runat="server" ValidationGroup="Dashboard" Width="100%"
                                    meta:resourcekey="ctlDashboardGroupResource1"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Group Order" meta:resourcekey="Label7Resource1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="ctlGroupDispOrder" runat="server" ValidationGroup="Dashboard" Width="100%"
                                    meta:resourcekey="ctlGroupDispOrderResource1"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="Display Order" meta:resourcekey="Label8Resource1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="ctlDashboardOrder" runat="server" ValidationGroup="Dashboard" Width="100%"
                                    meta:resourcekey="ctlDashboardOrderResource1"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Tag" meta:resourcekey="Label9Resource1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="ctlDashboardTag" runat="server" ValidationGroup="Dashboard" Width="100%"
                                    meta:resourcekey="ctlDashboardTagResource1"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Url" meta:resourcekey="Label10Resource1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="ctlUrl" runat="server" ValidationGroup="Dashboard" Width="100%"
                                    meta:resourcekey="ctlUrlResource1"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="View Mode" meta:resourcekey="Label11Resource1"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ctlDisplayMode" runat="server" Width="100%" meta:resourcekey="ctlDisplayModeResource1">
                                    <asp:ListItem Text="CurrentPage" Value="CurrentPage" meta:resourcekey="ListItemResource4"></asp:ListItem>
                                    <asp:ListItem Text="CurrentControl" Value="CurrentControl" meta:resourcekey="ListItemResource5"></asp:ListItem>
                                    <asp:ListItem Text="OnNewPage" Value="OnNewPage" meta:resourcekey="ListItemResource6"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 100%">
                                <uc2:PanelPropertyEditor ID="ctlDashboardPanel" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="commands">
                                    <asp:Button ID="ctlDashboardCreate" runat="server" OnClick="ctlDashboardCreate_Click"
                                        Text="Create" ValidationGroup="Dashboard" meta:resourcekey="ctlDashboardCreateResource1" />
                                    <span lang="tr">&nbsp;</span><asp:Button ID="ctlDashboardUpdate" runat="server" Text="Update"
                                        OnClick="ctlDashboardUpdate_Click" ValidationGroup="Dashboard" 
                                        meta:resourcekey="ctlDashboardUpdateResource1" Visible="False" />
                                    <span lang="tr">&nbsp;</span><asp:Button ID="ctlDashboardDelete" runat="server" Text="Delete"
                                        OnClick="ctlDashboardDelete_Click" meta:resourcekey="ctlDashboardDeleteResource1" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            
            <div >
            <h1>
                <asp:Literal ID="Literal1" runat="server" Text="Dashboard Authorization" meta:resourcekey="Literal1Resource1"></asp:Literal>
            </h1>
            <div class="body">
                <div id="DashboardAuthDiv" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td valign="top">
                                <table>
                                    <tr>
                                        <td style="width: 50%">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 75px" nowrap="nowrap">
                                                        <asp:Label ID="Label12" runat="server" Text="Role:" meta:resourcekey="Label12Resource1"></asp:Label>
                                                    </td>
                                                    <td style="width: 60%">
                                                        <asp:TextBox ID="ctlRoleName" Width="100%" runat="server" meta:resourcekey="ctlRoleNameResource1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ctlRoleName"
                                                            ErrorMessage="*" SetFocusOnError="True" ValidationGroup="DashboardAuth" meta:resourcekey="RequiredFieldValidator3Resource1"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label13" runat="server" Text="Can View ? :" meta:resourcekey="Label13Resource1"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="ctlCanSee" runat="server" meta:resourcekey="ctlCanSeeResource1" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label14" runat="server" Text="Can Edit ? :" meta:resourcekey="Label14Resource1"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="ctlCanEdit" runat="server" meta:resourcekey="ctlCanEditResource1" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div class="commands">
                                                            <asp:Button ID="ctlCreateAuthBtn" runat="server" Text="Create" ValidationGroup="DashboardAuth"
                                                                OnClick="ctlCreateAuthBtn_Click" meta:resourcekey="ctlCreateAuthBtnResource1" />
                                                            <asp:Button ID="ctlUpdateAuthBtn" runat="server" Text="Update" ValidationGroup="DashboardAuth"
                                                                OnClick="ctlUpdateAuthBtn_Click" meta:resourcekey="ctlUpdateAuthBtnResource1" />
                                                            <asp:Button ID="ctlDeleteAuthBtn" runat="server" Text="Delete" OnClick="ctlDeleteAuthBtn_Click"
                                                                meta:resourcekey="ctlDeleteAuthBtnResource1" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <asp:GridView ID="ctlAuthGrid" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="True"
                                    DataKeyNames="Role" Width="100%" OnSelectedIndexChanged="ctlAuthGrid_SelectedIndexChanged"
                                    meta:resourcekey="ctlAuthGridResource1">
                                    <Columns>
                                        <asp:BoundField DataField="Role" HeaderText="Role" meta:resourcekey="BoundFieldResource6" />
                                        <asp:CheckBoxField DataField="CanView" HeaderText="View" meta:resourcekey="CheckBoxFieldResource1" />
                                        <asp:CheckBoxField DataField="CanEdit" HeaderText="Edit" meta:resourcekey="CheckBoxFieldResource2" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>           
        </asp:Panel>
       
   
        <asp:UpdatePanel ID="ctlRowsUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
            <div class="container">
                <h1>
                  &nbsp  <asp:Label ID="RowLabel" runat="server" Text="Dashboard Rows (Sections)" 
                        Font-Size="X-Large"></asp:Label>
                </h1>
                <div class="body">
                    <table style="width: 50%">
                        <tr>
                            <td valign="top">
                                <asp:GridView ID="RowsGrid" runat="server" AutoGenerateColumns="False" DataKeyNames="InstanceKey"
                                    Width="100%" OnSelectedIndexChanged="RowsGrid_SelectedIndexChanged" meta:resourcekey="RowsGridResource1">
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" meta:resourcekey="CommandFieldResource1" />
                                        <asp:BoundField DataField="RowOrder" HeaderText="#" 
                                            meta:resourcekey="BoundFieldResource7" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Title" HeaderText="Title" 
                                            meta:resourcekey="BoundFieldResource8" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <table>
                                    <tr>
                                        <td style="width: 50%">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 75px" nowrap="nowrap">
                                                        <asp:Label ID="Label15" runat="server" Text="Title:" 
                                                            meta:resourcekey="Label15Resource1" Visible="False"></asp:Label>
                                                    </td>
                                                    <td style="width: 60%">
                                                        <asp:TextBox ID="ctlRowTitle" runat="server" 
                                                            meta:resourcekey="ctlRowTitleResource1" Visible="False"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="width: 100%">
                                                        <uc2:PanelPropertyEditor ID="ctlRowPanelEditor" runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div class="commands">
                                                            <asp:Button ID="ctlCreateRow" runat="server" Text="Create" ValidationGroup="DashboardRow"
                                                                OnClick="DashboardRowCreate_Click" meta:resourcekey="ctlCreateRowResource1" />
                                                            &nbsp;<asp:Button ID="ctlUpdateRow" runat="server" Text="Update" ValidationGroup="DashboardRow"
                                                                OnClick="DashboardRowUpdate_Click" 
                                                                meta:resourcekey="ctlUpdateRowResource1" Visible="False" />
                                                            &nbsp;<asp:Button ID="ctlDeleteRow" runat="server" Text="Delete" OnClick="DashboardRowDelete_Click"
                                                                meta:resourcekey="ctlDeleteRowResource1" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            </ContentTemplate></asp:UpdatePanel>

        <asp:UpdatePanel ID="ctlcolumnsUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
            <div class="container">
                <h1>
                 &nbsp    <asp:Label ID="ColumnsLabel" runat="server" Text="Selected Section Columns"
                        Font-Size="X-Large"></asp:Label>
                </h1>
                <div class="body">
                    <div id="DashboardColumnDiv" runat="server">

                        <table style="width: 100%">
                            <tr>
                                <td valign="top">
                                    <table>
                                        <tr>
                                            <td style="width: 50%">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 75px" nowrap="nowrap">
                                                            <asp:Label ID="Label16" runat="server" Text="ColumnWidth (%):" meta:resourcekey="Label16Resource1"></asp:Label>
                                                        </td>
                                                        <td style="width: 60%">
                                                            <asp:TextBox ID="ctlColWithBox" Width="35px" runat="server" 
                                                                meta:resourcekey="ctlColWithBoxResource1" ViewStateMode="Disabled"></asp:TextBox>
                                                            <asp:Button ID="DashboardColumnUpdate" runat="server" 
                                                                meta:resourcekey="DashboardColumnUpdateResource1" 
                                                                OnClick="DashboardColumnUpdate_Click" Text="Update" 
                                                                ValidationGroup="DashboardColumn" />
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ctlColWithBox"
                                                                ErrorMessage="*" SetFocusOnError="True" ValidationGroup="DashboardColumn" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            <asp:GridView ID="ColumnsGrid" runat="server" AutoGenerateColumns="False" 
                                                                DataKeyNames="ColumnOrder" meta:resourcekey="ColumnsGridResource1" 
                                                                OnSelectedIndexChanged="ColumnsGrid_SelectedIndexChanged" Width="100%" 
                                                                ViewStateMode="Enabled">
                                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <Columns>
                                                                    <asp:CommandField meta:resourcekey="CommandFieldResource1" 
                                                                        ShowSelectButton="True" />
                                                                    <asp:BoundField DataField="ColumnOrder" HeaderText="#" 
                                                                        meta:resourcekey="BoundFieldResource9" >
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="WidthInPercent" HeaderText="%" 
                                                                        meta:resourcekey="BoundFieldResource10" >
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            <asp:Button ID="DashboardColumnCreate" runat="server" 
                                                                meta:resourcekey="DashboardColumnCreateResource1" 
                                                                OnClick="DashboardColumnCreate_Click" Text="Create" 
                                                                ValidationGroup="DashboardColumn" />
                                                            &nbsp;<asp:Button ID="DashboardColumnCreateDelete" runat="server" 
                                                                meta:resourcekey="DashboardColumnCreateDeleteResource1" 
                                                                OnClick="DashboardColumnCreateDelete_Click" Text="Delete" />
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label17" runat="server" meta:resourcekey="Label17Resource1" 
                                                                Text="Column Padding :" Visible="False"></asp:Label>
                                                            <asp:Label ID="Label18" runat="server" meta:resourcekey="Label18Resource1" 
                                                                Text="Column Style:" Visible="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="ctlColPadding" runat="server" 
                                                                meta:resourcekey="ctlColPaddingResource1" Visible="False" Width="35px"></asp:TextBox>
                                                            <asp:TextBox ID="ctlColStyle" runat="server" 
                                                                meta:resourcekey="ctlColStyleResource1" Rows="3" TextMode="MultiLine" 
                                                                Visible="False" Width="100%"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="ctlIsColResize" runat="server" 
                                                                meta:resourcekey="ctlIsColResizeResource" Text="User can resize ?" 
                                                                Visible="False"></asp:Label>
                                                            <asp:CheckBox ID="ctlColResize" runat="server" Visible="False" />
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <div class="commands">
                                                                <asp:Label ID="ctlIsSaveColResize" runat="server" 
                                                                    meta:resourcekey="ctlIsSaveColResizeResource" Text="Save Resize:" 
                                                                    Visible="False"></asp:Label>
                                                                <asp:CheckBox ID="ctlSaveResize" runat="server" Visible="False" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    &nbsp;</td>
                            </tr>
                        </table>


                        
                    </div>
                </div>
            </div>
            </ContentTemplate></asp:UpdatePanel>
   

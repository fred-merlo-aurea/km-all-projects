<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="FTPScheduleSetup.aspx.cs" Inherits="KMPS.MD.Main.FTPScheduleSetup" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div class="UpdateProgress" style="position: absolute; z-index: 10; color: black;
                font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center;
                width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft);
                top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="Images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>  
            <asp:GridView ID="gvScheduledExports" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                EnableModelValidation="True" DataKeyNames="SchedID">
                <Columns>
                    <asp:BoundField DataField="SchedName" HeaderText="Name" SortExpression="SchedName">
                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FormatType" HeaderText="Format" SortExpression="FormatType">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastUser" HeaderText="EditedBy" SortExpression="LastUser"                        >
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastDate" HeaderText="EditedDate" SortExpression="LastDate" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Active" SortExpression="IsActive"
                        ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# (Boolean.Parse(Eval("IsActive").ToString())) ? "Yes" : "No"%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                        FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("SchedID")%>'
                                OnCommand="lnkEdit_Command"><img src="../Images/ic-edit.gif" alt=""/></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                        FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# Eval("SchedID")%>'
                                OnCommand="lnkDelete_Command"><img src="../Images/icon-delete.gif" alt=""/></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="600" align="center">
                    <tr>
                        <td id="errorTop">
                        </td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                            alt="" />
                                    </td>
                                    <td valign="middle" align="left" width="80%" height="100%">
                                        <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="errorBottom">
                        </td>
                    </tr>
                </table>
                <br />
            </div> 
            <table border="0" cellpadding="5" cellspacing="3" width="100%" align="center">
                <tr>
                    <td align="center" style="font-size: 15px; vertical-align: top;">
                        <table border="0" cellpadding="5" cellspacing="3" width="100%" align="center">
                            <tr>
                                <td align="right" style="font-size: 15px; vertical-align: top; width: 150px">
                                    <b>Schedule Name</b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSchedName" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                    </td>
                    <td align="center" style="font-size: 15px; vertical-align: top;">
                        <table border="0" cellpadding="5" cellspacing="3" width="100%" align="center">
                            <tr>
                                <td align="right" style="font-size: 15px; vertical-align: top; width: 150px">
                                    <b>Format</b>
                                </td>
                                <td align="left">                                
                                    <asp:DropDownList ID="ddlFormat" runat="server" Width="100%">
                                        <asp:ListItem Selected="True">CSV</asp:ListItem>
                                        <asp:ListItem>TAB Delimeted</asp:ListItem>
                                    </asp:DropDownList>                                
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="font-size: 15px; vertical-align: bottom;">
                        <table border="0" cellpadding="5" cellspacing="3" width="100%" align="center">
                            <tr>
                                <td align="right" style="font-size: 15px; vertical-align: top; width: 150px">
                                    <b>Export Fields</b>
                                </td>
                                <td valign="top" align="left">
                                    <asp:ListBox ID="lbExport" runat="server" Rows="15" SelectionMode="Multiple" Font-Size="x-small"
                                        Font-Names="Arial" Width="100%" Style="text-transform: uppercase">
                                        <asp:ListItem Text='EMAIL' Value='s.EMAIL'></asp:ListItem>
                                        <asp:ListItem Text='FIRSTNAME' Value='s.FNAME'></asp:ListItem>
                                        <asp:ListItem Text='LASTNAME' Value='s.LNAME'></asp:ListItem>
                                        <asp:ListItem Text='COMPANY' Value='s.COMPANY'></asp:ListItem>
                                        <asp:ListItem Text='TITLE' Value='s.TITLE'></asp:ListItem>
                                        <asp:ListItem Text='ADDRESS' Value='s.ADDRESS'></asp:ListItem>
                                        <asp:ListItem Text='MAILSTOP' Value='s.MAILSTOP'></asp:ListItem>
                                        <asp:ListItem Text='CITY' Value='s.CITY'></asp:ListItem>
                                        <asp:ListItem Text='STATE' Value='s.STATE'></asp:ListItem>
                                        <asp:ListItem Text='ZIP' Value='s.ZIP'></asp:ListItem>
                                        <asp:ListItem Text='PLUS4' Value='s.PLUS4'></asp:ListItem>
                                        <asp:ListItem Text='COUNTRY' Value='s.COUNTRY'></asp:ListItem>
                                        <asp:ListItem Text='FORZIP' Value='s.FORZIP'></asp:ListItem>
                                        <asp:ListItem Text='PHONE' Value='s.PHONE'></asp:ListItem>
                                        <asp:ListItem Text='MOBILE' Value='s.MOBILE'></asp:ListItem>
                                        <asp:ListItem Text='FAX' Value='s.FAX'></asp:ListItem>
                                        <asp:ListItem Text='CategoryID' Value='s.CategoryID'></asp:ListItem>
                                        <asp:ListItem Text='TransactionID' Value='s.TransactionID'></asp:ListItem>
                                        <asp:ListItem Text='Pubids' Value='s.pubids'></asp:ListItem>
                                        <asp:ListItem Text='Demo31' Value='s.Demo31'></asp:ListItem>
                                        <asp:ListItem Text='Demo32' Value='s.Demo32'></asp:ListItem>
                                        <asp:ListItem Text='Demo33' Value='s.Demo33'></asp:ListItem>
                                        <asp:ListItem Text='Demo34' Value='s.Demo34'></asp:ListItem>
                                        <asp:ListItem Text='Demo35' Value='s.Demo35'></asp:ListItem>
                                        <asp:ListItem Text='Demo36' Value='s.Demo36'></asp:ListItem>
                                    </asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    
                    <td>
                    </td>
                    <td align="left" style="font-size: 15px; vertical-align: top;">
                        <table border="0" cellpadding="5" cellspacing="3" width="100%" align="center">
                            <tr>
                                <td align="right" style="font-size: 15px; vertical-align: top; width: 150px">
                                    <b>FTP Host</b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtHost" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr>
                                <td align="right" style="font-size: 15px; vertical-align: top; width: 150px">
                                    <b>FTP User</b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtUser" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="font-size: 15px; vertical-align: top; width: 150px">
                                    <b>FTP Password</b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPass" runat="server" Width="100%" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan=2>                                
                                </td>
                            </tr>
                            <tr>
                                <td colspan=2>                                
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="font-size: 15px; vertical-align: middle; width: 400px">
                                    <asp:Label ID="lblTestResult" runat="server" Text=""></asp:Label>
                                </td>
                                <td align=right>                                
                                    <asp:Button ID="btnTestNow" runat="server" Text="Test Now" 
                                        onclick="btnTestNow_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="font-size: 15px; vertical-align: bottom;">
                        <b>Available Filters</b>
                    </td>
                    <td>
                    </td>
                    <td align="center" style="font-size: 15px; vertical-align: bottom;">
                        <b>Selected Filters</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ListBox ID="lstSourceFields" runat="server" Rows="20" Style="text-transform: uppercase;"
                            DataValueField="FilterID" DataTextField="Name" SelectionMode="Multiple" Font-Size="X-Small"
                            Font-Names="Arial" Width="400px"></asp:ListBox>
                    </td>
                    <td align="center">
                        <asp:Button ID="btnAdd" runat="server" Text=">>" CssClass="button" OnClick="btnAdd_Click" />
                        <br />
                        <br />
                        <asp:Button ID="btnremove" runat="server" CssClass="button" Text="<<" OnClick="btnremove_Click" />
                    </td>
                    <td>
                        <asp:ListBox ID="lstDestFields" runat="server" Rows="20" Style="text-transform: uppercase"
                            DataValueField="FilterID" DataTextField="Name" SelectionMode="Multiple" Font-Size="X-Small"
                            Font-Names="Arial" Width="400px"></asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table border="0" cellpadding="5" cellspacing="3" width="100%" align="center">
                            <tr>
                                <td align="right" style="font-size: 15px; vertical-align: top; width: 150px">
                                    <asp:RadioButtonList ID="rblScheduleType" runat="server" Font-Bold="True" 
                                        onselectedindexchanged="rblScheduleType_SelectedIndexChanged">
                                        <asp:ListItem Selected="True">Daily</asp:ListItem>
                                        <asp:ListItem>Monthly</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td align="left">
                                    <asp:Panel ID="pnlDaily" runat="server" >
                                        <table border="0" cellpadding="5" cellspacing="3" width="100%" align="center">
                                            <tr>
                                                <td align="right" style="font-size: 15px; vertical-align: top; width: 100px">
                                                    <asp:CheckBox ID="cbSun" runat="server" Text="Sunday"></asp:CheckBox>
                                                </td>
                                                <td align="right" style="font-size: 15px; vertical-align: top; width: 100px">
                                                    <asp:CheckBox ID="cbMon" runat="server" Text="Monday"></asp:CheckBox>
                                                </td>
                                                <td align="right" style="font-size: 15px; vertical-align: top; width: 100px">
                                                    <asp:CheckBox ID="cbTue" runat="server" Text="Tuesday"></asp:CheckBox>
                                                </td>
                                                <td align="right" style="font-size: 15px; vertical-align: top; width: 100px">
                                                    <asp:CheckBox ID="cbWed" runat="server" Text="Wednesday"></asp:CheckBox>
                                                </td>
                                                <td align="right" style="font-size: 15px; vertical-align: top; width: 100px">
                                                    <asp:CheckBox ID="cbThurs" runat="server" Text="Thursday"></asp:CheckBox>
                                                </td>
                                                <td align="right" style="font-size: 15px; vertical-align: top; width: 100px">
                                                    <asp:CheckBox ID="cbFri" runat="server" Text="Friday"></asp:CheckBox>
                                                </td>
                                                <td align="right" style="font-size: 15px; vertical-align: top; width: 100px">
                                                    <asp:CheckBox ID="cbSat" runat="server" Text="Saturday"></asp:CheckBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlMonthly" runat="server" >
                                        <table border="0" cellpadding="5" cellspacing="3" width="100%" align="center">
                                            <tr>
                                                <td align="right" style="font-size: 15px; vertical-align: top; width: 100px">
                                                    <asp:TextBox ID="txtMonthlyDate" Width="70" CssClass="formfield" MaxLength="10"
                                                        runat="server">
                                                    </asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1"
                                                        runat="server" CssClass="MyCalendar" TargetControlID="txtMonthlyDate" Format="MM/dd/yyyy">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="button"
                            ValidationGroup="save" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                            CssClass="button" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

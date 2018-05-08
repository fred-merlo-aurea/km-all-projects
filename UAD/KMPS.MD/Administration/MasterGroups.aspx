<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true"
    CodeBehind="MasterGroups.aspx.cs" Inherits="KMPS.MDAdmin.MasterGroups" %>

<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">

        function ValidateDelete() {
            if (!confirm('Are you sure you want to delete Mastergroup. It will delete Mastergroup, MasterCodesheet and  all mapping for the Mastergroup?')) return false;
            return true;
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="90%" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="images/errorEx.jpg" runat="server"
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
                        <td id="errorBottom"></td>
                    </tr>
                </table>
                <br />
            </div>
            <asp:TextBox ID="txtSearch" runat="server"  Width="250px"></asp:TextBox>&nbsp;&nbsp;
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click" />&nbsp;
            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="button" OnClick="btnReset_Click" />
            <br />   <br />  
            <asp:GridView ID="gvMasterGroups" runat="server" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" EnableModelValidation="True" OnRowCommand="gvMasterGroups_RowCommand" OnRowDeleting="gvMasterGroups_RowDeleting"
                DataKeyNames="MasterGroupID" OnSorting="gvMasterGroups_Sorting" OnPageIndexChanging="gvMasterGroups_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="DisplayName" HeaderText="Display Name" SortExpression="DisplayName"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="23%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Is Active" SortExpression="IsActive" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate><%# (Boolean.Parse(Eval("IsActive").ToString())) ? "Yes" : "No"%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Enable SubReporting" SortExpression="EnableSubReporting" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><%# (Boolean.Parse(Eval("EnableSubReporting").ToString())) ? "Yes" : "No"%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Enable Searching" SortExpression="EnableSearching" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><%# (Boolean.Parse(Eval("EnableSearching").ToString())) ? "Yes" : "No"%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Enable AdhocSearch" SortExpression="EnableAdhocSearch" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate><%# (Boolean.Parse(Eval("EnableAdhocSearch").ToString())) ? "Yes" : "No"%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                        FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("MasterGroupID")%>'
                                OnCommand="lnkEdit_Command"><img src="../Images/ic-edit.gif" alt="" style="border:none;" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                        ItemStyle-HorizontalAlign="center"
                        HeaderStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("MasterGroupID")%>' OnClientClick="return ValidateDelete();"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:HyperLinkField Text="Responses" DataNavigateUrlFormatString="MasterCodeSheet.aspx?MasterGroupID={0}"
                        DataNavigateUrlFields="MasterGroupID">
                        <HeaderStyle Width="5%" />
                    </asp:HyperLinkField>
                </Columns>
            </asp:GridView>
            <br />

            <table cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td>
                        <asp:Label ID="lblMasterGroup" runat="server" Text="Add MasterGroup"></asp:Label></td>
                </tr>
                <tr>
                    <td>Display Name</td>
                    <td>
                        <asp:HiddenField ID="hfMasterGroupID" runat="server" Value="0" />
                        <asp:HiddenField ID="hfSortOrder" runat="server" Value="0" />
                        <asp:TextBox ID="txtDisplayName" runat="server" Width="250px" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtDisplayName"
                            ErrorMessage="*" ValidationGroup="save"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revDisplayName" runat="server" ValidationGroup="save"
                        ErrorMessage="<br/>Not a valid display name" ControlToValidate="txtDisplayName" ForeColor="Red"
                        ValidationExpression="[^(),]*" Display="Dynamic"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>Name</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqDescription" runat="server" ControlToValidate="txtName"
                            ErrorMessage="*" ValidationGroup="save"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="save"
                        ErrorMessage="<br/>Not a valid name" ControlToValidate="txtName" ForeColor="Red"
                        ValidationExpression="[^(),]*" Display="Dynamic"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>Active</td>
                    <td>
                        <asp:DropDownList ID="ddlActive" runat="server">
                            <asp:ListItem Value="True">Yes</asp:ListItem>
                            <asp:ListItem Value="False">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>SubReporting</td>
                    <td>
                        <asp:DropDownList ID="ddlSubReporting" runat="server">
                            <asp:ListItem Value="True">Yes</asp:ListItem>
                            <asp:ListItem Value="False">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Searching</td>
                    <td>
                        <asp:DropDownList ID="ddlSearching" runat="server">
                            <asp:ListItem Value="True">Yes</asp:ListItem>
                            <asp:ListItem Value="False">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Adhoc Search</td>
                    <td>
                        <asp:DropDownList ID="ddlAdhocSearch" runat="server">
                            <asp:ListItem Value="True">Yes</asp:ListItem>
                            <asp:ListItem Value="False">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button" ValidationGroup="save" />
                        <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                            CssClass="button" CausesValidation="False" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

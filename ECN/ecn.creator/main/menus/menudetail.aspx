<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.creator.menusmanager.menueditor" CodeBehind="menudetail.aspx.cs" MasterPageFile="~/Creator.Master" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
        <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
            <tbody>
                <tr>
                    <td class="tableHeader" align='right'>MenuCode&nbsp;</td>
                    <td>
                        <asp:TextBox runat="Server" ID="MenuCode" Visible="True" EnableViewState="True"></asp:TextBox>
                </tr>
                <tr>
                    <td class="tableHeader" align='right'>&nbsp;MenuName&nbsp;</td>
                    <td>
                        <asp:TextBox EnableViewState="true" ID="MenuName" runat="Server" size="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" align='right'>Target Page&nbsp;</td>
                    <td>
                        <asp:DropDownList runat="Server" ID="MenuTarget" Visible="True" EnableViewState="True"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" align='right'>SortOrder&nbsp;</td>
                    <td>
                        <asp:TextBox ID="SortOrder" runat="Server" size="3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" align='right'>MenuTypeCode&nbsp;</td>
                    <td>
                        <asp:DropDownList runat="Server" ID="MenuTypeCode" Visible="True" EnableViewState="True"
                            OnSelectedIndexChanged="LoadParentMenuDD" AutoPostBack>
                        </asp:DropDownList>
                </tr>
                <tr>
                    <td class="tableHeader" align='right' width="130">ParentID&nbsp;</td>
                    <td>
                        <asp:DropDownList runat="Server" ID="ParentID" Visible="True" EnableViewState="True"></asp:DropDownList>
                    </td>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr size="1" color='#000000'>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="tableHeader" colspan="2" align="middle">
                        <asp:TextBox EnableViewState="true" Visible="false" ID="MenuID" runat="Server"></asp:TextBox>
                        <asp:Button ID="SaveButton" OnClick="CreateMenu" Visible="true" Text="Create" class="formbutton"
                            runat="Server" />
                        <asp:Button ID="UpdateButton" OnClick="UpdateMenu" Visible="false" Text="Update"
                            class="formbutton" runat="Server" />
                    </td>
                </tr>
            </tbody>
        </table>
</asp:Content>


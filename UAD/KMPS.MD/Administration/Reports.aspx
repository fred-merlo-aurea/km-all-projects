<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="KMPS.MD.Administration.Reports" %>

<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" >
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>
    <div style="text-align: right">
<%--        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="Images/progress.gif" runat="server" />
                Processing....
            </ProgressTemplate>
        </asp:UpdateProgress>--%>
    </div>
<%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <table cellpadding="5" cellspacing="5" border="1px">
                <tr>
                    <td align="right" valign="top">
                        <asp:Label ID="Label1" runat="Server" Text="Reports: "></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="ddlReports" runat="server">
                            <asp:ListItem Selected="True">Products</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="Server" Text="Order By: "></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="ddlOrderBy" runat="server">
                            <asp:ListItem Selected="True" Value="PRODUCT">By Product</asp:ListItem>
                            <asp:ListItem Value="MASTERGROUP">By MasterGroup</asp:ListItem>
                        </asp:DropDownList>

                        <asp:Button ID="btnCreateReport" runat="server" Text="Download" 
                            onclick="btnCreateReport_Click" />
                    </td>
                </tr>
            </table>
<%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
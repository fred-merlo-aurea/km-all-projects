<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="SecurityAccessError.aspx.cs" Inherits="KMPS.MD.SecurityAccessError" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <center>
        <table id="Table1" style="width: 800px; height: 96px" cellspacing="1" cellpadding="1" border="1">
            <tr>
                <td style="height: 23px" align="center">
                    <asp:Label ID="lblMsgTitle" runat="Server" Font-Bold="True" Font-Names="Verdana"
                        Font-Size="13px" ForeColor="Black">
					    &nbsp;You are not Authorized to view this Page.<br /></asp:Label></td>
            </tr>
        </table>
    </center>
</asp:Content>

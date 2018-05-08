<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true"
    CodeBehind="PublicationsImport.aspx.cs" Inherits="KMPS.MDAdmin.PublicationsImport" %>

<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divError" runat="Server" visible="false">
        <table cellspacing="0" cellpadding="0" width="674" align="center">
            <tr>
                <td id="errorTop">
                </td>
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
                <td id="errorBottom">
                </td>
            </tr>
        </table>
        <br />
    </div>
    <table cellpadding="5" cellspacing="5" border="1px">
        <tr>
            <td align="right" valign="top">
                <asp:Label ID="Label1" runat="Server" Text="File: "></asp:Label>
            </td>
            <td valign="top">
                <asp:FileUpload ID="FileUpload1" runat="server"/>
            </td>
            <td>
                <asp:Button ID="btnCheckFile" runat="server" Text="Check File" 
                    onclick="btnCheckFile_Click" />
            </td>
            <td valign="top">
                <asp:Button ID="btnUpload" runat="server" Text="Upload" 
                    onclick="btnUpload_Click"/>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" colspan="4">
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

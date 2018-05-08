<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="PublicationsSetupCopy.aspx.cs" Inherits="KMPS.MD.Administration.PublicationsSetupCopy" %>
<%@ MasterType VirtualPath="Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: right">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="Images/progress.gif" runat="server" />
                Processing....
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
                        <asp:Label ID="Label1" runat="Server" Text="From: "></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="ddlFrom" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourcePub" 
                            DataTextField="pubname" DataValueField="pubid">
                        </asp:DropDownList>                        
                    </td>
                    <td align="right" valign="top">
                        <asp:Label ID="Label2" runat="Server" Text="To: "></asp:Label>
                    </td>
                    <td align="right" valign="top">                
                        <asp:DropDownList ID="ddlTo" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourcePub2"
                            DataTextField="pubname" DataValueField="pubid">
                        </asp:DropDownList>                        
                    </td>
                    <td valign="top">
                        <asp:Button ID="btnSubmit" runat="server" Text="Copy" 
                            onclick="btnSubmit_Click" OnClientClick="return confirm('Copy these product values?')"/>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top" colspan="5">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:SqlDataSource ID="SqlDataSourcePub" runat="server" 
                SelectCommand="select pubid, pubname from pubs order by pubname asc">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourcePub2" runat="server" 
                SelectCommand="select pubid, pubname from pubs where (pubid != @PubID) order by pubname asc">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlFrom" Name="PubID" PropertyName="SelectedValue"
                        Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
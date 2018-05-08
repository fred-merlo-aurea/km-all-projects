<%@ Page Language="c#" Inherits="ecn.communicator.main.content.content_main" CodeBehind="default.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Src="~/main/ECNWizard/Content/contentExplorer.ascx" TagName="contentExplorer" TagPrefix="uc1" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="Default.js"></script>
    <link href="DefaultStyleSheet.css" rel="stylesheet" />
    <link href="../Font/SourceSansPro/KMPlatform.css" rel="Stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
        <table cellspacing="0" cellpadding="0" width="674" align="center">
            <tr>
                <td id="errorTop">
                </td>
            </tr>
            <tr>
                <td id="errorMiddle">
                    <table height="67" width="80%">
                        <tr>
                            <td valign="top" align="center" width="20%">
                                <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
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
    </asp:PlaceHolder>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10" Visible="true" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                            <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                                <div>
                                    <center>
                                        <br/>
                                        <br/>
                                        <b>Processing...</b><br/>
                                        <br/>
                                        <img src="http://images.ecn5.com/images/loading.gif" alt=""/><br/>
                                        <br/>
                                        <br/>
                                        <br/>
                                    </center>
                                </div>
                            </asp:Panel>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <uc1:contentExplorer ID="contentExplorer1" runat="server"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
      
      
    </table>
</asp:Content>
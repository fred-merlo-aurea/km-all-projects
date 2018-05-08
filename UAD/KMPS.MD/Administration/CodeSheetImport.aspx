<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="CodesheetImport.aspx.cs" Inherits="KMPS.MD.Administration.CodesheetImport" %>

<%@ MasterType VirtualPath="Site.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">

        function btnUploadClick() {
            document.getElementById('InprogressDiv').style.visibility = "visible";
        }

    </script>

    <div style="text-align: right">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="Images/progress.gif" runat="server" />
                Processing....
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div id="divError" runat="Server" visible="false">
        <table cellspacing="0" cellpadding="0" width="80%" align="center"">
            <tr>
                <td id="errorTop"></td>
            </tr>
            <tr>
                <td id="errorMiddle">
                    <table>
                        <tr>
                            <td valign="top" align="center" width="20%">
                                <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                    alt="" />
                            </td>
                            <td valign="middle" align="left" width="100%" height="100%">
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
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
        <ContentTemplate>
            <table cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td><b>Import and Mapping</b></td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="drpImport" runat="server" AutoPostBack="True" Width="300px">
                            <asp:ListItem Value="Codesheet Import and Mapping">Codesheet Import and Mapping</asp:ListItem>
                            <asp:ListItem Value="MasterGroup Import">MasterGroup Import</asp:ListItem>
                            <asp:ListItem Value="Product Import">Product Import</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RoundedCornersExtender ID="RoundedCornersExtender4" runat="server" TargetControlID="pnlRoundUploadFile"
                            Radius="6" Corners="All" />
                        <asp:ModalPopupExtender ID="mdlPopupTxt" runat="server" TargetControlID="hdnField"
                            PopupControlID="pnlUploadFile" CancelControlID="btnClose" BackgroundCssClass="modalBackground" />
                        <asp:Panel ID="pnlUploadFile" runat="server" Style="display: none"
                            CssClass="modalPopup">
                            <asp:Panel ID="pnlRoundUploadFile" runat="server" Width="600" CssClass="modalPopup2">
                                <div align="center" style="text-align: center; height: 175px; padding: 10px 10px 10px 10px;">

                                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD">
                                        <tr style="background-color: #5783BD;">
                                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold" colspan="3">Upload Txt File
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 5px; font-size: 12px; font-weight: bold">
                                                <asp:Label ID="UploadTxtFileLabel" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input id="FileSelector" type="file" name="FileSelector" runat="server" style="width: 400px;" />
                                                <br />
                                                <span class="highLightOne">(TXT Files ONLY)</span>
                                                <br />
                                                <div id="InprogressDiv" style="visibility: hidden; font-style: oblique">
                                                    Uploading...
                                                </div>
                                                <br />
                                                <asp:Button ID="btnClose" Text="Cancel" CssClass="button" runat="server" CausesValidation="false" />
                                                <asp:Button ID="btnUpload" runat="server" Text="Load File" CssClass="button" OnClientClick="btnUploadClick(); return true;" OnClick="btnUpload_Click" CausesValidation="false"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </asp:Panel>

                        <asp:Button ID="btnUploadFile" runat="server" CssClass="button" Text="Upload TXT" OnClick="btnUploadFile_Click" CausesValidation="false" />
                        <asp:HiddenField ID="hdnField" runat="server" />
                    </td>
                </tr>
            </table>
            <br />
            <div id="divMessage" runat="server" visible="false">
                <table cellspacing="0" cellpadding="0" width="30%">
                    <tr>
                        <td id="Td1"></td>
                    </tr>
                    <tr>
                        <td id="Td2">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img2" style="padding: 0 0 0 15px;" src="images/checkmark.gif" runat="server"
                                            alt="" />
                                    </td>
                                    <td valign="middle" align="left" width="80%" height="100%">
                                        <asp:Label ID="lblMessage" runat="Server" CssClass="Menu"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="Td3"></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


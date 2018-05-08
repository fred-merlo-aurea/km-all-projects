<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RecordViewPDFSetup.aspx.cs" Inherits="KMPS.MD.Tools.RecordViewPDFSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style type="text/css">
        .popitmenu {
            position: absolute;
            background-color: white;
            border: 1px solid black;
            font: normal 12px Verdana;
            line-height: 18px;
            z-index: 100;
            visibility: hidden;
        }

            .popitmenu a {
                text-decoration: none;
                padding-left: 6px;
                color: #5783BD;
                display: block;
                font-weight: bold;
            }

                .popitmenu a:hover {
                    /*hover background color*/
                    background-color: #FF7F00;
                    color: #ffffff;
                }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ModalWindow {
            border: solid 1px#c0c0c0;
            background: #ffffff;
            padding: 0px 10px 10px 10px;
            position: absolute;
            top: -1000px;
        }

        .modalPopup {
            background-color: transparent;
            padding: 1em 6px;
        }

        .modalPopup2 {
            background-color: #ffffff;
            width: 270px;
            vertical-align: top;
        }
    </style>
    <script type="text/javascript">

        function btnUploadClick() {
            document.getElementById('InprogressDiv').style.visibility = "visible";
        }

    </script>

    <div style="text-align: right">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="../images/loading.gif" runat="server" />
                Processing....
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
        <ContentTemplate>
            <center>
                <div style="width: 90%; text-align: left; padding-left: 10px;">
                    <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                        <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                            <div style="float: left;">
                                <asp:Label ID="lblpnlHeader" runat="Server">PDF setup</asp:Label>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel"
                        Height="100%" BorderWidth="1">
                        <div id="divError" runat="Server" visible="false">
                            <table cellspacing="0" cellpadding="0" width="674" align="center">
                                <tr>
                                    <td id="errorMiddle">
                                        <table width="80%">
                                            <tr>
                                                <td valign="top" align="center" width="20%">
                                                    <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                                        alt="" />
                                                </td>
                                                <td valign="middle" align="left" width="80%" height="100%">
                                                    <asp:Label ID="lblErrorMessage" runat="Server" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table cellpadding="5" cellspacing="5" border="0">
                            <tr>
                                <td colspan="2"><b>Upload Company Logo</b></td>
                            </tr>
                            <tr>
                                <td align="right" valign="top"><b>Image</b>
                                </td>
                                <td>
                                    <input id="FileSelector" type="file" name="FileSelector" runat="server" style="width: 400px;" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnUpload" runat="server" Text="Upload Logo" CssClass="button" OnClick="btnUpload_Click" CausesValidation="false" UseSubmitBehavior="False"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Image ID="imglogo" runat="server" Visible="false"></asp:Image>
                                    <asp:HiddenField ID="hfConfigID" Value="0" runat="server" />
                                    <asp:HiddenField ID="hfImage" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="Panel1" runat="server">
                            <table cellpadding="5" cellspacing="5" border="0">
                                <tr>
                                    <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                        <b>Available Dimensions</b>
                                    </td>
                                    <td></td>
                                    <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                        <b>Selected Dimensions</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ListBox ID="lstSourceFields" runat="server" Rows="10" Style="text-transform: uppercase;"
                                            SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="350px" Height="300px"
                                            EnableViewState="True"></asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAdd" runat="server" Text=">>" CssClass="button" OnClick="btnAdd_Click" UseSubmitBehavior="False" />
                                        <br>
                                        <br>
                                        <asp:Button ID="btnremove" runat="server" CssClass="button" OnClick="btnRemove_Click"
                                            Text="<<" UseSubmitBehavior="False" />
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstDestFields" runat="server" Rows="10" Style="text-transform: uppercase;"
                                            SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="350px" Height="300px"
                                            DataTextField="DisplayName"
                                            DataValueField="MasterGroupID"></asp:ListBox>

                                    </td>
                                    <td>
                                        <asp:Button ID="btnUp" runat="server" Text="Move Up" CssClass="button" OnClick="btnUp_Click" UseSubmitBehavior="False" />
                                        <br>
                                        <br>
                                        <asp:Button ID="btnDown" runat="server" CssClass="button" OnClick="btndown_Click"
                                            Text="Move Down" UseSubmitBehavior="False" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <br />
                        <asp:Panel ID="pnlAdhoc" runat="server">
                            <table cellpadding="5" cellspacing="5" border="0">
                                <tr>
                                    <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                        <b>Available Adhoc</b>
                                    </td>
                                    <td></td>
                                    <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                        <b>Selected Adhoc</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ListBox ID="lstAdhocSourceFields" runat="server" Rows="10" Style="text-transform: uppercase;"
                                            SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="350px" Height="300px"
                                            EnableViewState="True"></asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAdhocAdd" runat="server" Text=">>" CssClass="button" OnClick="btnAdhocAdd_Click" UseSubmitBehavior="False" />
                                        <br>
                                        <br>
                                        <asp:Button ID="btnAdhocRemove" runat="server" CssClass="button" OnClick="btnAdhocRemove_Click"
                                            Text="<<" UseSubmitBehavior="False" />
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstAdhocDestFields" runat="server" Rows="10" Style="text-transform: uppercase;"
                                            SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="350px" Height="300px"
                                            DataTextField="CustomField"
                                            DataValueField="SubscriptionsExtensionMapperID"></asp:ListBox>

                                    </td>
                                    <td>
                                        <asp:Button ID="btnAdhocUp" runat="server" Text="Move Up" CssClass="button" OnClick="btnAdhocUp_Click" UseSubmitBehavior="False" />
                                        <br>
                                        <br>
                                        <asp:Button ID="btnAdhocDown" runat="server" CssClass="button" OnClick="btnAdhocDown_Click"
                                            Text="Move Down" UseSubmitBehavior="False" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <table cellpadding="5" cellspacing="5" border="0">
                            <tr>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" UseSubmitBehavior="False" />
                                    &nbsp;&nbsp; 
                    <asp:Button ID="btnCancel" runat="server" CssClass="button" OnClick="btnCancel_Click" Text="Cancel" UseSubmitBehavior="False" /></td>
                            </tr>
                        </table>

                    </asp:Panel>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

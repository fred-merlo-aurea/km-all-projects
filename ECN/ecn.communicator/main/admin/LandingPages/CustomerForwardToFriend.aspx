<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="CustomerForwardToFriend.aspx.cs" Inherits="ecn.communicator.main.admin.landingpages.CustomerForwardToFriend" ValidateRequest="false" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         function disableButton() {
             var button = document.getElementById('<%=btnPreview.ClientID %>');
            if (button.className != "ECN-Button-Medium-disable") {
                button.className = "ECN-Button-Medium-disable";
                button.disabled = true;
            }
        }
    </script>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopup {
            background-color: white;
            border-width: 3px;
            border-style: solid;
            border-color: white;
            padding: 3px;
            overflow: auto;
        }

        .modalPopupHtmlPreview {
            position: fixed;
            width: 75%;
            height: 50%;
            overflow: auto;
            background-color: #e6e7e8;
            border: 2px solid black;
            padding: 20px 20px 20px 0px;
        }


        .overlay {
            position: fixed;
            z-index: 99;
            top: 0px;
            left: 0px;
            background-color: gray;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=70);
            opacity: 0.70;
            -moz-opacity: 0.70;
        }

        .loader {
            z-index: 100;
            position: fixed;
            width: 120px;
            margin-left: -60px;
            background-color: #F4F3E1;
            font-size: x-small;
            color: black;
            border: solid 2px Black;
            top: 40%;
            left: 50%;
        }

        .styled-select {
            width: 240px;
            background: transparent;
            height: 28px;
            overflow: hidden;
            border: 1px solid #ccc;
        }

        .styled-text {
            width: 240px;
            height: 28px;
            overflow: hidden;
            border: 1px solid #ccc;
        }

        .styled-text-multiline {
            height: 100px;
            overflow: auto;
            border: 1px solid #ccc;
        }

        .reorderStyle {
            list-style-type: disc;
            font: Verdana;
            font-size: 12px;
        }

        .reorderStyle li {
            list-style-type: none;
            padding-bottom: 1em;
        }

        .ddlStyle {
            width: 400px;
            margin: 8px 20px;
        }

        .htmlPreviewTable {
            margin-top: 50px;
            padding: 25px;
            width: 75%;
        }

        .htmlPreviewStyle {
            color: Black;
            font-size: 11pt;
            font-family: 'Segoe UI';
            text-decoration: none;
            display: block;
            padding: 8px 20px;
            margin: 0;
            font-weight: 600;
        }
        .ECN-Button-Medium-disable
        {
            -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background-color: #ededed;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            border-radius: 4px;
            border: 2px solid #dcdcdc;
            display: inline-block;
            color: #aaaaaa;
            font-family: arial;
            font-size: 10px;
            font-weight: bold;
            padding: 4px 18px;
            text-decoration: none;
            text-shadow: 1px 1px 0px #ffffff;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="Panel3" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel4" CssClass="loader" runat="server">
                    <div>
                        <center>
                            <br />
                            <br />
                            <b>Processing...</b><br />
                            <br />
                            <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                            <br />
                            <br />
                            <br />
                        </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
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
                        <td id="errorBottom"></td>
                    </tr>
                </table>
            </asp:PlaceHolder>

           <asp:Panel runat="server" ID="pnlNoAccess">
                <div style="padding-top: 150px; padding-bottom: 150px; text-align: center; font-size: large;">
                    <asp:Label ID="Label1" runat="server" Text="Customer override disabled. Please contact your Basechannel Administrator"></asp:Label>
                </div>
            </asp:Panel>

            <asp:Panel runat="server" ID="pnlSettings">
                 <br />
                <table style="width:100%;">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblHeading" runat="server" Text="Forward to Friend Page Settings" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:40%;">Do you want to override BaseChannel settings?
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rblBasechannelOverride" runat="server" RepeatDirection="Horizontal" onchange="disableButton()">
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem Selected="True">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="width:100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblHeader" runat="server" Text="Header" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtHeader" runat="server" TextMode="MultiLine" CssClass="styled-text-multiline" Width="100%" onkeyup="disableButton()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="width:100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFooter" runat="server" Text="Footer" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFooter" runat="server" TextMode="MultiLine" CssClass="styled-text-multiline" Width="100%" onkeyup="disableButton()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top" align="center">
                        <td >
                            <br />
                            <asp:Button ID="btnSave" runat="server" Text="Save" onClick="btnSave_Click" class="ECN-Button-Medium" />
                        </td>
                        <td>
                            <br />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview"  class="ECN-Button-Medium" />
                        </td>

                    </tr>
                     <tr>
                        <td colspan="4" style="text-align: center">
                            <asp:Label ID="lblCustomerOverrideWarning" runat="server" Style="font-weight: bold; align-items: center"></asp:Label>
                        </td>
                        <tr>
                            <td colspan="4" style="text-align: center">
                                <asp:Label ID="lblSentBlastsWarning" runat="server" Style="font-weight: bold; align-items: center"></asp:Label>
                            </td>
                        </tr>
                    </tr>
                </table>

            </asp:Panel>
            </ContentTemplate>
         </asp:UpdatePanel>

</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ECN_SF_Integration.aspx.cs" Inherits="ecn.communicator.main.Salesforce.basechannelsetup" MasterPageFile="~/MasterPages/Communicator.Master" ValidateRequest="false"%>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
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
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
          
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

            <asp:Panel runat="server" ID="pnlNoAccess" Visible="false">
                <div style="padding-top: 150px; padding-bottom: 150px; text-align: center; font-size: large;">
                       You do not have access to this page. If this is an emergency, please call your Account Manager
                        at (866) 844-6275
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlSettings" CssClass="label"  Visible="true">
                <br />
                <table width="100%">
                    
                     <asp:Panel runat="server" ID="pnlInactive" Visible="false">
                         <tr>
                        <td colspan="2">
                             <div style="padding-top:50px;padding-bottom:20px;text-align:center;font-size:large;">
             <asp:Label ID="Label3" runat="server" Text="Your account is NOT setup with Salesforce integration."></asp:Label></div>
                      
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <br />
                            <br />
                                 <table>
                                     <tr>
                                         <td>
                                               <asp:Label ID="Label4" runat="server" Text="Consumer Key"></asp:Label>
                                         </td>
                                         <td>
                                             <asp:TextBox ID="txtConsumerKey" runat="server"></asp:TextBox>
                                         </td>
                                     </tr>
                                      <tr>
                                         <td>
                                             <asp:Label ID="Label5" runat="server" Text="Consumer Secret"></asp:Label>
                                         </td>
                                         <td>
                                             <asp:TextBox ID="txtConsumerSecret" runat="server"></asp:TextBox>
                                         </td>
                                     </tr>
                                      <tr>
                                         <td colspan="2" align="center">
                                             <br />
                                              <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Authorize"></asp:Button>
                                         </td>
                                     </tr>
                                 </table>
                        </td>
                    </tr>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlActive" Visible="false">
                        <tr>
                            <td colspan="2" valign="middle">
                                 <div style="padding-top:150px;padding-bottom:150px;text-align:center;font-size:large;">
             <asp:Label ID="Label1" runat="server" Text="Your account is setup with Salesforce integration."></asp:Label>
        <asp:HyperLink ID="hlAddReport" runat="server" NavigateUrl="/ecn.communicator/main/ecnwizard/wizardsetup.aspx?campaignItemType=salesforce" Font-Size="Large">Click Here</asp:HyperLink><asp:Label ID="Label2" runat="server" Text=" to schedule a Campaign Item."></asp:Label>        
        </div>

                            </td>
                        </tr>
                    </asp:Panel>
                </table>
            </asp:Panel>

</asp:Content>

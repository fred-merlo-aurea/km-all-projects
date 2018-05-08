<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="ManageCampaignItems.aspx.cs" Inherits="ecn.communicator.main.blasts.ManageCampaignItems" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/ecn.communicator/MasterPages/ECN_Controls.css" />
    
    <script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />
    <script type="text/javascript">
        if (window.attachEvent) { window.attachEvent('onload', pageloaded); }
        else if (window.addEventListener) { window.addEventListener('load', pageloaded, false); }
        else { document.addEventListener('load', pageloaded, false); }

        function pageloaded() {
            $('.subject').each(function () {
                var initialString = $(this).html();

                try {
                    if (initialString.indexOf('<img') < 0) {
                        initialString = initialString.replace(/'/g, "\\'");
                        initialString = initialString.replace(/\r?\n|\r/g, ' ');
                        initialString = twemoji.parse(eval("\'" + initialString + "\'"), { size: "16x16" });
                    }
                }
                catch (err) {

                }
                //if (!initialString.includes('<img')) {
                //    initialString = initialString.substr(0, 30);
                //}

                var regSplit = new RegExp("(<img.*?\/?>)");

                var imgSplit = new Array();

                imgSplit = initialString.split(regSplit);
                var textFullSplit = new Array();

                for (var i = 0; i < imgSplit.length; i++) {
                    var current = imgSplit[i];
                    if (current.indexOf('<img') >= 0) {
                        //currentindex is image add to finalSplit
                        textFullSplit.push(current);
                    }
                    else {
                        //currentindex is plain text, loop through each char and add to final split
                        for (var j = 0; j < current.length; j++) {
                            textFullSplit.push(current.charAt(j));
                        }
                    }
                }

                var finalText = "";

                if (initialString.length > 0) {
                    if (textFullSplit.length > 30) {
                        for (var i = 0; i < 30; i++) {
                            finalText = finalText.concat(textFullSplit[i]);
                        }
                        finalText = finalText + '...';
                    }
                    else {
                        finalText = initialString;
                    }

                }
                else {
                    finalText = initialString;
                }


                $(this).html(finalText);
            })
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="uplMaster" UpdateMode="Conditional">
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Panel ID="pnlContent" runat="server">
        <table style="width: 100%;">
            <tr>
                <td style="padding: 10px;">
                    <asp:Label ID="lblCampaignName" CssClass="ECN-Label-Heading-Large" runat="server" />&nbsp;
                </td>
            </tr>

            <tr>
                <td>
                    <table style="width: 100%; padding-bottom: 10px;">
                        <tr>
                            <td style="width: 22%; font-size: 14px;">Campaign Item Name to Search
                            </td>
                            <td style="width: 18%; padding-left: 10px; text-align: left;">
                                <asp:TextBox ID="txtCampaignItemName" runat="server" />
                            </td>
                            <td style="width: 45%; text-align: right;">
                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" />
                            </td>
                            <td style="width: 15%; padding-left: 10px; text-align: left;">
                                <asp:Button ID="btnClearSearch" runat="server" OnClick="btnClearSearch_Click" Text="Clear Search" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="pnlGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <ecnCustom:ecnGridView ID="gvCampaignItems" CssClass="ECN-GridView" Width="100%" AutoGenerateColumns="false" PageSize="10" OnRowDataBound="gvCampaignItems_RowDataBound" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="Campaign Item" DataField="CampaignItemName" />
                                    <asp:BoundField HeaderText="Subject" ItemStyle-CssClass="subject" DataField="EmailSubject" />
                                    <asp:BoundField HeaderText="Group" DataField="GroupName" />
                                    <asp:BoundField HeaderText="Sent Time" DataField="SendTime" />
                                    <asp:BoundField HeaderText="Sends" DataField="SendTotal" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="Type" DataField="BlastType" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Actions">
                                        <ItemTemplate>
                                            <ul class="ECN-InfoLinks" style="padding-left: 0px; padding-top: 10px; padding-right: 0px; display: inline;">
                                                <li style="padding-top: 0px; text-align: center; padding-bottom: 0px; padding-left: 10px;">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/ecn-icon-gear-small.png" />
                                                    <ul style="width: 240px; left: 10px; text-align: left;">
                                                        <li>
                                                            <asp:LinkButton ID="lbEditCampaignItemName" runat="server" Text="Edit Campaign Item Name" CssClass="aspBtn" OnClick="lbEditCampaignItemName_Click" /></li>
                                                        <li id="liMove" runat="server">
                                                            <asp:LinkButton ID="lbMoveCampaignItem" runat="server" Text="Move Campaign Item" CssClass="aspBtn" OnClick="lbMoveCampaignItem_Click" /></li>
                                                    </ul>
                                                </li>
                                            </ul>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </ecnCustom:ecnGridView>
                            <asp:Panel ID="pnlPager" Width="100%" runat="server">
                                <table cellpadding="0" border="0" width="100%">
                                    <tr>
                                        <td align="left" class="label" width="31%">Total Records:
                                                <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                                        </td>
                                        <td align="left" class="label" width="25%">Show Rows:
                                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" CssClass="formfield" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">

                                                    <asp:ListItem Value="5" />
                                                    <asp:ListItem Value="10" />
                                                    <asp:ListItem Value="15" />
                                                    <asp:ListItem Value="20" />
                                                    <asp:ListItem Value="30" />
                                                    <asp:ListItem Value="40" />
                                                    <asp:ListItem Value="50" />
                                                    <asp:ListItem Value="100" />
                                                </asp:DropDownList>
                                        </td>
                                        <td align="right" class="label" width="44%">Page
                                                <asp:TextBox ID="txtGoToPage" runat="server" AutoPostBack="true" class="formtextfield" Width="30px" OnTextChanged="txtGoToPage_TextChanged" />

                                            of
                                                <asp:Label ID="lblTotalNumberOfPages" runat="server" CssClass="label" />
                                            &nbsp;
                                                <asp:Button ID="btnPrevious" runat="server" CommandName="Page" ToolTip="Previous Page"
                                                    CommandArgument="Prev" class="formbuttonsmall" Text="<< Previous" OnClick="btnPrevious_Click" />
                                            <asp:Button ID="btnNext" runat="server" CommandName="Page" ToolTip="Next Page"
                                                CommandArgument="Next" class="formbuttonsmall" Text="Next >>" OnClick="btnNext_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Button ID="hfEditCampaignItem" runat="server" style="display:none;" />
    <ajaxToolkit:ModalPopupExtender ID="mpeEditCampaignItem" runat="server" TargetControlID="hfEditCampaignItem" BackgroundCssClass="modalBackground" PopupControlID="upEditCampaignItem" />
    <asp:UpdatePanel ID="upEditCampaignItem" runat="server" UpdateMode="Conditional">
        
        <ContentTemplate>

            <asp:Panel ID="pnlEditCampaignItem" Width="300px" Height="100px" runat="server">
                <table style="background-color: white; padding: 5px; border-radius: 5px; width: 100%; height: 100%;">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblEditHeading" runat="server" CssClass="ECN-Label-Heading-Large" Text="Edit Campaign Item Name" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblName" runat="server" Text="Campaign Item Name:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 50%; text-align: center;">
                                        <asp:Button ID="btnSaveCampaign" runat="server" OnClick="btnSaveCampaign_Click" Text="Save" />
                                    </td>
                                    <td style="width: 50%; text-align: center;">
                                        <asp:Button ID="btnCancelEdit" runat="server" OnClick="btnCancelEdit_Click" Text="Cancel" />
                                    </td>
                                </tr>
                            </table>
                    </tr>
                </table>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="hfMoveCampaignItem" runat="server" style="display:none;" />
    <ajaxToolkit:ModalPopupExtender ID="mpeMoveCampaignItem" runat="server" TargetControlID="hfMoveCampaignItem" BackgroundCssClass="modalBackground" PopupControlID="upMoveCampaignItem" />
    <asp:UpdatePanel ID="upMoveCampaignItem" runat="server" UpdateMode="Conditional">
        
        <ContentTemplate>

            <asp:Panel ID="pnlMoveCampaignItem" runat="server" Height="100px" Width="200px">
                <table style="width: 100%; height: 100%; background-color: white;">
                    <tr>
                        <td>
                            <asp:Label ID="lblMoveCampaignItemHeading" CssClass="ECN-Label-Heading-Large" runat="server" Text="Move Campaign Item" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 40%;">
                                        <asp:Label ID="lblCampaign" runat="server" Text="Campaigns" />
                                    </td>
                                    <td style="width: 60%;">
                                        <asp:DropDownList ID="ddlCampaigns" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 50%; text-align: center;">
                                        <asp:Button ID="btnMoveCampaigmItem" runat="server" Text="Move" OnClick="btnMoveCampaigmItem_Click" />
                                    </td>
                                    <td style="width: 50%; text-align: center;">
                                        <asp:Button ID="btnCancelMoveCampaignItem" runat="server" Text="Cancel" OnClick="btnCancelMoveCampaignItem_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

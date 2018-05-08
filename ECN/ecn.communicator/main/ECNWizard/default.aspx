<%@ Page Language="c#" Inherits="ecn.communicator.main.ECNWizard._default" CodeBehind="default.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>

<%@ Register Src="~/main/ECNWizard/OtherControls/AddTemplate.ascx" TagName="addTemplate" TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/Controls/WizardSentCampaigns.ascx" TagName="SentCampaigns" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .urbangreymenu {
            width: 150px; /*width of menu*/
        }

            .urbangreymenu .headerbar {
                font: bold 13px Verdana;
                color: white;
                background: #606060 url(media/arrowstop.gif) no-repeat 8px 6px; /*last 2 values are the x and y coordinates of bullet image*/
                margin-bottom: 0; /*bottom spacing between header and rest of content*/
                text-transform: uppercase;
                padding: 2px 0 7px 31px; /*31px is left indentation of header text*/
            }

            .urbangreymenu ul {
                list-style-type: none;
                margin: 0;
                padding: 0;
                margin-bottom: 0; /*bottom spacing between each UL and rest of content*/
            }

                .urbangreymenu ul li {
                    padding-bottom: 2px; /*bottom spacing between menu items*/
                }

                    .urbangreymenu ul li a {
                        font: normal 12px Arial;
                        color: black;
                        background: #E9E9E9;
                        display: block;
                        padding: 5px 0;
                        line-height: 17px;
                        padding-left: 8px; /*link text is indented 8px*/
                        text-decoration: none;
                    }

                        .urbangreymenu ul li a:visited {
                            color: black;
                        }

                        .urbangreymenu ul li a:hover {
                            /*hover state CSS*/
                            color: Orange;
                            background: Grey;
                        }

        .accordionHeader {
            border: 1px solid #2F4F4F;
            background-color: Gray;
            font-family: Arial, Sans-Serif;
            color: white;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }

        .accordionHeaderSelected {
            border: 1px solid #2F4F4F;
            background-color: Gray;
            font-family: Arial, Sans-Serif;
            font-weight: bold;
            color: white;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopup {
            background-color: #D0D0D0;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
        }

        .style1 {
            width: 100%;
        }

        .buttonMedium {
            width: 135px;
            background: url(buttonMedium.gif) no-repeat left top;
            border: 0;
            font-weight: bold;
            color: #ffffff;
            height: 20px;
            cursor: pointer;
            padding-top: 2px;
        }

        .TransparentGrayBackground {
            position: fixed;
            top: 0;
            left: 0;
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
            height: 100%;
            width: 100%;
            min-height: 100%;
            min-width: 100%;
        }

        .overlay {
            position: fixed;
            z-index: 50;
            top: 0px;
            left: 0px;
            background-color: gray;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=70);
            opacity: 0.70;
            -moz-opacity: 0.70;
        }

        * html .overlay {
            position: absolute;
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
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

        * html .loader {
            position: absolute;
            margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
        }

        .ajax__tab_custom .ajax__tab_header {
            font-family: "Helvetica Neue", Arial, Sans-Serif;
            font-size: 14px;
            font-weight: bold;
            display: block;
            border-bottom: solid 1px #d7d7d7;
            padding-bottom: 4px;
        }

            .ajax__tab_custom .ajax__tab_header:after {
                clear: both;
            }

            .ajax__tab_custom .ajax__tab_header:before, .ajax__tab_custom .ajax__tab_header:after {
                content: "";
                display: table;
            }

            .ajax__tab_custom .ajax__tab_header .ajax__tab_outer {
                border-color: #222;
                color: #222;
                padding-left: 10px;
                margin-right: 3px;
                border: solid 1px #d7d7d7;
                background-color: #d7d7d7;
            }

            .ajax__tab_custom .ajax__tab_header .ajax__tab_inner {
                border-color: #666;
                color: #666;
                padding: 3px 10px 2px 0px;
            }

        .ajax__tab_custom .ajax__tab_hover .ajax__tab_outer {
            background-color: orange;
        }

        .ajax__tab_custom .ajax__tab_hover .ajax__tab_inner {
            color: #fff;
        }

        .ajax__tab_custom .ajax__tab_active .ajax__tab_outer {
            border-bottom-color: #ffffff;
            background-color: orange;
        }

        .ajax__tab_custom .ajax__tab_active .ajax__tab_inner {
            color: #000;
            border-color: #333;
        }

        .ajax__tab_custom .ajax__tab_body {
            font-family: verdana,tahoma,helvetica;
            font-size: 10pt;
            background-color: #fff;
            border-top-width: 0;
            border: solid 1px #d7d7d7;
            border-top-color: #ffffff;
            min-height: 500px;
        }
    </style>

    <script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
    <script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
    <link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />

    <script type="text/javascript">

        if (window.attachEvent) { window.attachEvent('onload', pageloaded); }
        else if (window.addEventListener) { window.addEventListener('load', pageloaded, false); }
        else { document.addEventListener('load', pageloaded, false); }

        function pageloaded() {
            $('.subjectdefault').each(function () {
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

        //$(document).ready(function () {
        //    $('.subject').each(function(){
        //        var initialString = $(this).html();

        //        initialString = twemoji.parse(eval("'" + initialString + "'"), { size: "16x16" });

        //        if (!initialString.includes('<img')) {
        //            initialString = initialString.substr(0, 30);
        //        }

        //        $(this).html(initialString);
        //    })});

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="update1" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upMainProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upMainProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional">
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
            <br />
            <table cellpadding="0" cellspacing="0" width="100%" border='0'>
                <tr valign="top">
                    <td align="right">
                        <div style="align: right; width: 100%">
                            <asp:HiddenField ID="hfRedirectURL" Value="" runat="server" />
                            <img src="../../images/ic-RegularBlast.png" />&nbsp;<asp:LinkButton ID="lnkQuickTestBlast" runat="server"  OnClick="lnkQuickTestBlast_Click" CssClass="EcnBlackLink">Quick Test Blast</asp:LinkButton></li>&nbsp;&nbsp;&nbsp;
                            <img src="../../images/ic-RegularBlast.png" />&nbsp;<asp:LinkButton ID="lnkRegular" runat="server" OnClick="lnkRegular_Click" CssClass="EcnBlackLink">Create Regular Blast</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            <img src="../../images/ic-ABBlast.png" />&nbsp;<asp:LinkButton ID="lnkAB" runat="server" OnClick="lnkAB_Click" ToolTip="" CssClass="EcnBlackLink">Create AB Blast</asp:LinkButton></li>&nbsp;&nbsp;&nbsp;
                            <img src="../../images/ic-ChampionBlast.png" />&nbsp;<asp:LinkButton ID="lnkChampion" runat="server" OnClick="lnkChampion_Click" CssClass="EcnBlackLink">Create Champion Blast</asp:LinkButton></li>&nbsp;&nbsp;&nbsp;
                            <a href="/ecn.communicator/main/blasts/BlastCalendarView.aspx" style="text-decoration: none">
                                <img src="../../images/icon-calendar.gif" />&nbsp;<span class="EcnBlackLink">Calendar View</span></a>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td width="100%" style="margin-left: 960px">
                        <br />
                        <ajaxToolkit:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="1"
                            CssClass="ajax__tab_custom" Style="text-align: left" OnActiveTabChanged="TabContainer_ActiveTabChanged">
                            <ajaxToolkit:TabPanel ID="TabSaved" runat="server">
                                <HeaderTemplate>
                                    <img src="/ecn.images/images/saved_message.gif" alt="Saved Campaigns" />&nbsp;<span style="color: black">Saved Campaigns</span>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <br />
                                    <ecnCustom:ecnGridView ID="gvSaved" runat="server" Width="100%" CssClass="grid"
                                        AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="gvSaved_Command"
                                        OnRowDataBound="gvSaved_RowDataBound" OnPageIndexChanging="gvSaved_PageIndexChanging"
                                        DataKeyNames="CampaignItemID" PageSize="15" ShowEmptyTable="True">

                                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                        <Columns>
                                            <asp:BoundField DataField="CampaignItemName" HeaderText="Campaign Item">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Subject">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" CssClass="subjectdefault" runat="server" Text='<%# Eval("EmailSubject") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" CssClass="subjectdefault" runat="server" Text='<%# Eval("EmailSubject") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="GroupName" HeaderText="Group">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CampaignItemType" HeaderText="Type">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UpdatedDate" HeaderText="Last Updated">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="15%" Wrap="False" />
                                            </asp:BoundField>
                                            <asp:HyperLinkField HeaderText="Quick Test Blast" Text="<img src=/ecn.communicator/images/ic-RegularBlast.png alt='Quick Test Blast' border='0'>"
                                                DataNavigateUrlFields="CampaignItemID" DataNavigateUrlFormatString="~/main/ecnwizard/quicktestblast.aspx?campaignitemid={0}">
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:HyperLinkField>
                                            <asp:TemplateField HeaderText="Copy">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.communicator/images/icon-copy.png alt='Copy CampaignItem' height='16 width='16' border='0'&gt;"
                                                        CausesValidation="false" ID="CopyCampaignItemBtn" CommandName="CopyCampaignItem"
                                                        OnClientClick="return confirm('Are you sure you want to Copy this CampaignItem?');"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CampaignItemID") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:HyperLinkField HeaderText="Edit" Text="<img src=/ecn.images/images/icon-edits1.gif alt='Edit CampaignItem' border='0'>"
                                                DataNavigateUrlFields="CampaignItemID,CampaignItemType" DataNavigateUrlFormatString="~/main/ecnwizard/wizardsetup.aspx?CampaignItemID={0}&campaignItemType={1}">
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:HyperLinkField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete CampaignItem' border='0'&gt;"
                                                        CausesValidation="false" ID="DeleteCampaignItemBtn" CommandName="DeleteCampaignItem"
                                                        OnClientClick="return confirm('Are you sure you want to delete this CampaignItem?');"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CampaignItemID") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <AlternatingRowStyle CssClass="gridaltrow" />
                                        <PagerTemplate>
                                            <table cellpadding="0" border="0" width="100%">
                                                <tr>
                                                    <td align="left" class="label" width="31%">Total Records:
                                                        <asp:Label ID="gvSaved_lblTotalRecords" runat="server" Text="" />
                                                    </td>
                                                    <td align="left" class="label" width="25%">Show Rows:
                                                        <asp:DropDownList ID="gvSaved_ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="gvSaved_SelectedIndexChanged"
                                                            CssClass="formfield">
                                                            <asp:ListItem Value="5" />
                                                            <asp:ListItem Value="10" />
                                                            <asp:ListItem Value="15" />
                                                            <asp:ListItem Value="20" />
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td align="right" class="label" width="100%">Page
                                                        <asp:TextBox ID="gvSaved_txtGoToPage" runat="server" AutoPostBack="true" OnTextChanged="gvSaved_TextChanged"
                                                            class="formtextfield" Width="30px" />
                                                        of
                                                        <asp:Label ID="gvSaved_lblTotalNumberOfPages" runat="server" CssClass="label" />
                                                        &nbsp;
                                                        <asp:Button ID="btnPrevious" runat="server" CommandName="Page" ToolTip="Previous Page"
                                                            CommandArgument="Prev" class="formbuttonsmall" Text="<< Previous" />
                                                        <asp:Button ID="btnNext" runat="server" CommandName="Page" ToolTip="Next Page" CommandArgument="Next"
                                                            class="formbuttonsmall" Text="Next >>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </PagerTemplate>
                                    </ecnCustom:ecnGridView>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabScheduled" runat="server" HeaderText="Scheduled">
                                <HeaderTemplate>
                                    <img src="/ecn.images/images/scheduled_emails.gif" class="headerImage" alt="Scheduled Campaigns" />&nbsp;<span style="color: black">Scheduled Campaigns</span>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <br />
                                    <ecnCustom:ecnGridView ID="gvPending" runat="server" Width="100%" CssClass="grid"
                                        AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="gvPending_Command"
                                        OnRowDataBound="gvPending_RowDataBound" OnPageIndexChanging="gvPending_PageIndexChanging"
                                        DataKeyNames="CampaignItemID" PageSize="15" ShowEmptyTable="True">
                                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                        <Columns>
                                            <asp:BoundField DataField="CampaignItemName" HeaderText="Campaign Item">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Subject">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" CssClass="subjectdefault" runat="server" Text='<%# Eval("EmailSubject") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" CssClass="subjectdefault" runat="server" Text='<%# Eval("EmailSubject") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="GroupName" HeaderText="Group">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CampaignItemType" HeaderText="Type">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Sendtime" HeaderText="Scheduled Time">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="7%" Wrap="False" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnEdit" ImageUrl="/ecn.images/images/icon-edits1.gif" runat="server" OnClick="imgbtnEdit_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:HyperLinkField HeaderText="Edit" Text="<img src=/ecn.images/images/icon-edits1.gif alt='Edit CampaignItem' border='0'>"
                                                DataNavigateUrlFields="CampaignItemID,CampaignItemType" DataNavigateUrlFormatString="~/main/ecnwizard/wizardsetup.aspx?CampaignItemID={0}&campaignItemType={1}">
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:HyperLinkField>--%>
                                            <asp:TemplateField HeaderText="Cancel">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ImageUrl="/ecn.images/images/icon-cancel.png" CausesValidation="false" ID="CancelCampaignItemBtn"
                                                        OnClick="CancelCampaignItemBtn_Click"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CampaignItemID") %>'></asp:ImageButton>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="Server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                                        CausesValidation="false" ID="DeleteCampaignItemBtn"
                                                        OnClick="DeleteCampaignItemBtn_Click"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CampaignItemID") %>'></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCampaignItemID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CampaignItemID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnBlastDetails" CommandName="BlastDetails" runat="server">+Details</asp:LinkButton>
                                                    </td> </tr>
                                                    <asp:Panel ID="pnlBlastReport" runat="Server" Visible="false">
                                                        <tr valign="top" style="top: 10px;">
                                                            <td colspan="8" align="left">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td width="2%"></td>
                                                                        <td width="96%">
                                                                            <br />
                                                                            <asp:Label ID="lblBlastReports" runat="server" Text="Blast Reports" Font-Bold="true"></asp:Label>
                                                                            <br />
                                                                            <ecnCustom:ecnGridView ID="gvBlastDetails" CssClass="grid" runat="Server" HorizontalAlign="Center"
                                                                                AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvBlastDetails_RowDataBound">
                                                                                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="[BlastID] EmailSubject" HeaderStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-HorizontalAlign="left">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblCampaignItemID" CssClass="subjectdefault" runat="server" Text='<%# "[" + Eval("BlastID") + "] " + Eval("EmailSubject")  %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:HyperLinkField HeaderText="Web View" Text="<img src='/ecn.images/images/icon-preview.gif' alt='Preview Message for the Blast' border='0'>"
                                                                                        Target="_blank" DataNavigateUrlFields="BlastID" DataNavigateUrlFormatString="~/main/blasts/preview.aspx?BlastID={0}"
                                                                                        ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:HyperLinkField>
                                                                                    <asp:TemplateField HeaderText="" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-HorizontalAlign="left">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblBlastID" runat="server" Text='<%#Eval("BlastID")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <AlternatingRowStyle CssClass="gridaltrow" />
                                                                            </ecnCustom:ecnGridView>
                                                                            <br />
                                                                        </td>
                                                                        <td width="2%"></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <AlternatingRowStyle CssClass="gridaltrow" />
                                        <PagerTemplate>
                                            <table cellpadding="0" border="0" width="100%">
                                                <tr>
                                                    <td align="left" class="label" width="31%">Total Records:
                                                        <asp:Label ID="gvPending_lblTotalRecords" runat="server" Text="" />
                                                    </td>
                                                    <td align="left" class="label" width="25%">Show Rows:
                                                        <asp:DropDownList ID="gvPending_ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="gvPending_SelectedIndexChanged"
                                                            CssClass="formfield">
                                                            <asp:ListItem Value="5" />
                                                            <asp:ListItem Value="10" />
                                                            <asp:ListItem Value="15" />
                                                            <asp:ListItem Value="20" />
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td align="right" class="label" width="100%">Page
                                                        <asp:TextBox ID="gvPending_txtGoToPage" runat="server" AutoPostBack="true" OnTextChanged="gvPending_TextChanged"
                                                            class="formtextfield" Width="30px" />
                                                        of
                                                        <asp:Label ID="gvPending_lblTotalNumberOfPages" runat="server" CssClass="label" />
                                                        &nbsp;
                                                        <asp:Button ID="btnPrevious" runat="server" CommandName="Page" ToolTip="Previous Page"
                                                            CommandArgument="Prev" class="formbuttonsmall" Text="<< Previous" />
                                                        <asp:Button ID="btnNext" runat="server" CommandName="Page" ToolTip="Next Page" CommandArgument="Next"
                                                            class="formbuttonsmall" Text="Next >>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </PagerTemplate>
                                    </ecnCustom:ecnGridView>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabSent" runat="server" HeaderText="Sent" Visible="true"
                                ScrollBars="Auto">
                                <HeaderTemplate>
                                    <img src="/ecn.images/images/sent_emails.gif" class="headerImage" alt="Sent Campaigns" />&nbsp;<span style="color: black">Sent Campaigns</span>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <br />
                                    <uc2:SentCampaigns ID="SentCampaigns" runat="server" />
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabActive" runat="server" HeaderText="Active" Visible="true"
                                ScrollBars="Auto">
                                <HeaderTemplate>
                                    <img src="/ecn.images/images/sends_report.gif" class="headerImage" alt="Active Campaigns" />&nbsp;<span style="color: black">Active Campaigns</span>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <br />
                                    <ecnCustom:ecnGridView ID="gvActive" runat="server" Width="100%" CssClass="grid"
                                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gvActive_PageIndexChanging"
                                        DataKeyNames="CampaignItemID" PageSize="15" ShowEmptyTable="True">
                                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                        <Columns>
                                            <asp:BoundField DataField="CampaignItemName" HeaderText="Campaign Item">
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EmailSubject" ItemStyle-CssClass="subjectdefault" HeaderText="Subject">
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="GroupName" HeaderText="Group">
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CampaignItemType" HeaderText="Type">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UpdatedDate" HeaderText="Last Updated">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="7%" Wrap="False" />
                                            </asp:BoundField>
                                        </Columns>
                                        <AlternatingRowStyle CssClass="gridaltrow" />
                                        <PagerTemplate>
                                            <table cellpadding="0" border="0" width="100%">
                                                <tr>
                                                    <td align="left" class="label" width="31%">Total Records:
                                                        <asp:Label ID="gvPending_lblTotalRecords" runat="server" Text="" />
                                                    </td>
                                                    <td align="left" class="label" width="25%">Show Rows:
                                                        <asp:DropDownList ID="gvPending_ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="gvPending_SelectedIndexChanged"
                                                            CssClass="formfield">
                                                            <asp:ListItem Value="5" />
                                                            <asp:ListItem Value="10" />
                                                            <asp:ListItem Value="15" />
                                                            <asp:ListItem Value="20" />
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td align="right" class="label" width="100%">Page
                                                        <asp:TextBox ID="gvPending_txtGoToPage" runat="server" AutoPostBack="true" OnTextChanged="gvPending_TextChanged"
                                                            class="formtextfield" Width="30px" />
                                                        of
                                                        <asp:Label ID="gvPending_lblTotalNumberOfPages" runat="server" CssClass="label" />
                                                        &nbsp;
                                                        <asp:Button ID="btnPrevious" runat="server" CommandName="Page" ToolTip="Previous Page"
                                                            CommandArgument="Prev" class="formbuttonsmall" Text="<< Previous" />
                                                        <asp:Button ID="btnNext" runat="server" CommandName="Page" ToolTip="Next Page" CommandArgument="Next"
                                                            class="formbuttonsmall" Text="Next >>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </PagerTemplate>
                                    </ecnCustom:ecnGridView>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                        </ajaxToolkit:TabContainer>
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="hfCancelPopup" runat="server" style="display:none;" />
    <ajaxToolkit:ModalPopupExtender ID="mpeCancel" PopupControlID="pnlCancel" runat="server" BackgroundCssClass="modalBackground" TargetControlID="hfCancelPopup" />
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="pnlCancel" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upTemplatesProgressCancel" CssClass="overlay" runat="server">
                <asp:Panel ID="upTemplatesProgressCancel2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="pnlCancel" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table style="width: 300px; height: 100px; background-color: white; padding: 10px;">
                <tr>
                    <td colspan="2">
                        <asp:Label Font-Size="Small" ID="lblCancelMessage" runat="server" />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr style="padding-top: 15px;">
                    <td style="text-align: center; width: 50%;">
                        <asp:Button ID="btnOKCancel" runat="server" Text="Continue" OnClick="btnOKCancel_Click" />
                    </td>
                    <td style="text-align: center; width: 50%;">
                        <asp:Button ID="btnCancelCancel" runat="server" Text="Cancel" OnClick="btnCancelCancel_Click" />
                    </td>

                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="hfDeletePopup" runat="server" style="display:none;"/>
    <ajaxToolkit:ModalPopupExtender ID="mpeDelete" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlDelete" TargetControlID="hfDeletePopup" />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="pnlDelete" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upTemplatesProgressDelete" CssClass="overlay" runat="server">
                <asp:Panel ID="upTemplatesProgressDelete2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="pnlDelete" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table style="width: 300px; height: 100px; background-color: white; padding: 10px;">
                <tr>
                    <td colspan="2">
                        <asp:Label Font-Size="Small" ID="lblDeleteMessage" runat="server" />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr style="padding-top: 15px;">
                    <td style="text-align: center; width: 50%;">
                        <asp:Button ID="btnDeleteOK" runat="server" Text="Continue" OnClick="btnDeleteOK_Click" />
                    </td>
                    <td style="text-align: center; width: 50%;">
                        <asp:Button ID="btnDeleteCancel" runat="server" Text="Cancel" OnClick="btnDeleteCancel_Click" />
                    </td>

                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="hfEditAB" runat="server" style="display:none;" />
    <ajaxToolkit:ModalPopupExtender ID="mpeEditAB" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlEditAB" TargetControlID="hfEditAB" />
    <asp:Panel ID="pnlEditAB" Width="300px" Height="300px" runat="server">
        <table style="width: 100%; background-color: white; padding: 10px;">
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblEditABMessage" runat="server" Font-Size="Small" Text="Are you sure you want to edit this CampaignItem? If there is a Champion blast already set up for this AB, it will be automatically deleted." />
                </td>
            </tr>
            <tr>
                <td style="text-align: center; width: 50%;">
                    <asp:Button ID="btnOkEditAB" runat="server" Text="Continue" OnClick="btnOkEditAB_Click" />
                </td>
                <td style="text-align: center; width: 50%;">
                    <asp:Button ID="btnCancelEditAB" runat="server" Text="Cancel" OnClick="btnCancelEditAB_Click" />
                </td>

            </tr>
        </table>
    </asp:Panel>

    <asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="modalPopupTemplates" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="pnlTemplates" TargetControlID="btnShowPopup1">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlTemplates" CssClass="modalPopup">
        <asp:UpdateProgress ID="upTemplatesProgress" runat="server" DisplayAfter="10" Visible="true"
            AssociatedUpdatePanelID="upTemplates" DynamicLayout="true">
            <ProgressTemplate>
                <asp:Panel ID="upTemplatesProgressP1" CssClass="overlay" runat="server">
                    <asp:Panel ID="upTemplatesProgressP2" CssClass="loader" runat="server">
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
        <asp:UpdatePanel ID="upTemplates" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>Do you want to Pre-populate the wizard with a Template?
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblTemplate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblTemplate_SelectedIndexChanged" RepeatDirection="Horizontal">
                                <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                <asp:ListItem Value="No" Text="No" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>

                    </tr>
                </table>
                <asp:Panel ID="pnlTemplate" runat="server" Visible="false">
                    <table width="100%">
                        <tr>
                            <td align="right">Template
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="drpCampaignItemTemplate" runat="server" CssClass="styled-select">
                                </asp:DropDownList>
                                &nbsp;&nbsp;
                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-add.gif alt='Add New Campaign Item Template' border='0'&gt;" CausesValidation="false" ID="lnkNewTemplate" OnClick="lnkNewTemplate_Click"></asp:LinkButton>
                            </td>
                        </tr>
                </asp:Panel>
                <table align="center" class="style1">
                    <tr>
                        <td style="text-align: right">
                            <asp:Button runat="server" Text="Submit" ID="btnTemplatesSubmit" CssClass="formfield"
                                OnClick="btnTemplates_Submit"></asp:Button>
                        </td>
                        <td style="text-align: left">
                            <asp:Button runat="server" Text="Cancel" ID="btnTemplatesCancel" OnClick="btnTemplates_Cancel" CssClass="formfield"></asp:Button>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>


    <asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="modalPopupAddTemplates" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="pnlAddTemplates" TargetControlID="btnShowPopup2">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlAddTemplates" CssClass="modalPopup">
        <asp:UpdateProgress ID="upAddTemplatesProgress" runat="server" DisplayAfter="10" Visible="true"
            AssociatedUpdatePanelID="upAddTemplates" DynamicLayout="true">
            <ProgressTemplate>
                <asp:Panel ID="upAddTemplatesProgressP1" CssClass="overlay" runat="server">
                    <asp:Panel ID="upAddTemplatesProgressP2" CssClass="loader" runat="server">
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
        <asp:UpdatePanel ID="upAddTemplates" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <table bgcolor="white">
                    <tr>
                        <td>
                            <uc1:addTemplate ID="addTemplate1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <asp:Button runat="server" Text="Submit" ID="btnAddTemplateSave" CssClass="formfield"
                                OnClick="addTemplateSave_Click"></asp:Button>
                            &nbsp; &nbsp;
                             <asp:Button runat="server" Text="Cancel" ID="btnAddTemplateClose" CssClass="formfield"
                                 OnClick="btnAddTemplateClose_Click"></asp:Button>
                        </td>
                        <td style="text-align: center"></td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:Button ID="btnCIMACheck" runat="server" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="mpeMACheck" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="pnlMACheck" TargetControlID="btnCIMACheck">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlMACheck" runat="server" CssClass="modalPopup" Height="100px" Style="background-color: white;" Width="300px">
        <asp:UpdateProgress ID="upMACheckProgress" runat="server" DisplayAfter="10" Visible="true"
            AssociatedUpdatePanelID="upMACheck" DynamicLayout="true">
            <ProgressTemplate>
                <asp:Panel ID="upMACheckProgressP1" CssClass="overlay" runat="server">
                    <asp:Panel ID="upMACheckProgressP2" CssClass="loader" runat="server">
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
        <asp:UpdatePanel ID="upMACheck" runat="server" UpdateMode="Conditional" style="height: 100%;">
            <ContentTemplate>

                <table bgcolor="white" style="height: 100%; width: 100%;">
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            <img style="padding: 0 0 0 2px;" src="/ecn.images/images/warningEx.jpg" alt="">
                        </td>
                        <td style="width: 80%; padding-left: 5px;">
                            <table style="height: 100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMAText" Text="This Campaign Item is tied to one or more published Marketing Automations. Are you sure you want to edit?" runat="server" />
                                    </td>
                                </tr>

                            </table>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:Button runat="server" Text="Continue" ID="btnMACheckContinue" CssClass="formfield"
                                OnClick="btnMACheckContinue_Click"></asp:Button>
                            &nbsp; &nbsp;
                                         <asp:Button runat="server" Text="Cancel" ID="btnMACheckCancel" CssClass="formfield"
                                             OnClick="btnMACheckCancel_Click"></asp:Button>
                        </td>

                    </tr>

                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>

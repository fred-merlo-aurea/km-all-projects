<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardSentCampaigns.ascx.cs"
    Inherits="ecn.communicator.main.ECNWizard.Controls.WizardSentCampaigns" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />

<script type="text/javascript">
    function pageLoad(sender, args) {
        $('.subject').each(function () {
            var initialString = $(this).html();


            initialString = initialString.replace(/'/g, "\\'");
            initialString = initialString.replace(/\r?\n|\r/g, ' ');

            initialString = twemoji.parse(eval("\'" + initialString + "\'"), { size: "16x16" });

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


        });
    }
</script>
<style type="text/css">
    .errorClass {
        border: 1px solid red;
    }
</style>
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
    <br />
</asp:PlaceHolder>

<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td class="label" valign="middle" align="left">Search By</td>
        <td class="label" valign="middle" align="left">Search Text</td>
        <td class="label" valign="middle" align="left">From</td>
        <td class="label" valign="middle" align="left">To</td>
        <td class="label" valign="middle" align="left">User</td>
        <td class="label" valign="middle" align="left">Campaign</td>
        <td class="label" valign="middle" align="left">Blast Type</td>
        <td class="label" valign="middle" align="left">&nbsp;
        </td>
    </tr>
    <tr>
        <td class="label" valign="middle" align="left">
            <asp:DropDownList ID="drpSearchCriteria" runat="Server" AutoPostBack="false" CssClass="formfield">
                <asp:ListItem Value="--Select--" Selected="True">--Select--</asp:ListItem>
                <asp:ListItem Value="Subject">Email Subject</asp:ListItem>
                <asp:ListItem Value="CampaignItem">Campaign Item</asp:ListItem>
                <asp:ListItem Value="Message">Message Name</asp:ListItem>
                <asp:ListItem Value="Group">Group Name</asp:ListItem>
                <asp:ListItem Value="BlastID">Blast ID</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td class="label" valign="middle" align="left">
            <asp:TextBox class="formtextfield" ID="txtSearch" runat="server"></asp:TextBox>
        </td>
        <td class="label" valign="middle" align="left">
            <asp:TextBox class="formtextfield" runat="server" ID="txtFrom" Width="60px"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFrom"
                Format="MM/dd/yyyy">
            </ajaxToolkit:CalendarExtender>
        </td>
        <td class="label" valign="middle" align="left">
            <asp:TextBox class="formtextfield" runat="server" ID="txtTo" Width="60px"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtTo"
                Format="MM/dd/yyyy">
            </ajaxToolkit:CalendarExtender>
        </td>
        <td class="label" valign="middle" align="left">
            <asp:DropDownList ID="drpSentUser" runat="Server" DataTextField="UserName" DataValueField="UserID"
                CssClass="formfield" Width="120px">
            </asp:DropDownList>
        </td>
        <td class="label" valign="middle" align="left">
            <asp:DropDownList class="formfield" ID="drpCampaign" runat="Server" DataTextField="CampaignName"
                DataValueField="CampaignID" Visible="true" Width="120px">
            </asp:DropDownList>
        </td>
        <td class="label" valign="middle" align="left">
            <asp:DropDownList ID="drpBlastType" runat="Server" CssClass="formfield">
                <asp:ListItem Value="false" Selected="True">Live Blasts</asp:ListItem>
                <asp:ListItem Value="true">Test Blasts</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td class="label" valign="middle" align="left">
            <asp:Button class="formbuttonsmall" ID="btnSearch" OnClick="btnSearch_Click" runat="Server"
                EnableViewState="False" Width="50" Visible="true" Text="Search"></asp:Button>
        </td>
    </tr>
    <tr>
        <td colspan="8">
            <hr />
        </td>
    </tr>
</table>
<ecnCustom:ecnGridView ID="gvSent" runat="Server" Width="100%" CssClass="grid" AutoGenerateColumns="False"
    AllowPaging="True" OnRowCommand="gvSent_Command" OnRowDataBound="gvSent_RowDataBound"
    OnPageIndexChanging="gvSent_PageIndexChanging" DataKeyNames="CampaignItemID"
    PageSize="15" ShowEmptyTable="True">
    <HeaderStyle CssClass="gridheader"></HeaderStyle>
    <FooterStyle CssClass="tableHeader1"></FooterStyle>
    <Columns>
        <asp:TemplateField Visible="false">
            <ItemTemplate>
                <asp:Label ID="lblCampaignItemID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CampaignItemID") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="CampaignItemName" HeaderText="Campaign Item" ItemStyle-Width="20%"
            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

            <ItemStyle HorizontalAlign="Left" Width="20%"></ItemStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Subject">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox1" runat="server" CssClass="subject" Text='<%# Eval("EmailSubject") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label2" runat="server" CssClass="subject" Text='<%# Eval("EmailSubject") %>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle HorizontalAlign="Left" Width="20%" />
        </asp:TemplateField>
        <asp:BoundField DataField="GroupName" HeaderText="Group" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Left"
            ItemStyle-HorizontalAlign="Left">
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

            <ItemStyle HorizontalAlign="Left" Width="20%"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="SendTime" HeaderText="SendTime" ItemStyle-Width="17%"
            ItemStyle-HorizontalAlign="Center">
            <ItemStyle HorizontalAlign="Center" Width="17%"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="SendTotal" HeaderText="Sends" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
            <ItemStyle HorizontalAlign="Center" Width="8%"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="CampaignItemType" HeaderText="Type" ItemStyle-HorizontalAlign="Center"
            ItemStyle-Width="10%">
            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
        </asp:BoundField>
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
        <asp:TemplateField HeaderText="Reports" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
            ItemStyle-Width="5%">
            <ItemTemplate>
                <asp:HyperLink ID="hlreporting" runat="server" Text="<img src='/ecn.images/images/icon-reports.gif' alt='View Blast Reporting' border='0'>"
                    NavigateUrl='<%# "~/main/blasts/reports.aspx?CampaignItemID=" + Eval("CampaignItemID") %>' Visible='<%# CheckCampaginItemReportVisible()%>'></asp:HyperLink>
            </ItemTemplate>

            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

            <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField Visible="true">
            <ItemTemplate>
                <asp:LinkButton ID="lnkbtnBlastDetails" CommandName="BlastDetails" runat="server">+Details</asp:LinkButton>
                </td> </tr>
                <asp:Panel ID="pnlBlastReport" runat="Server" Visible="false">
                    <tr valign="top" style="top: 10px;">
                        <td colspan="9" align="left">
                            <table width="100%">
                                <tr>
                                    <td width="2%"></td>
                                    <td width="96%">
                                        <br />
                                        <asp:Label ID="lblBlastReports" runat="server" Text="Blast Reports" Font-Bold="true"></asp:Label>
                                        <br />
                                        <ecnCustom:ecnGridView ID="gvBlastDetails" CssClass="grid" runat="Server" HorizontalAlign="Center"
                                            AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvBlastDetails_RowDataBound" OnRowCommand="gvBlastDetails_Command">
                                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                            <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="[BlastID] Email Subject" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" CssClass="subject" Text='<%# "[" + Eval("BlastID") + "] " + Eval("EmailSubject")  %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="GroupName" HeaderText="Group Name" SortExpression="GroupID"
                                                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundField>
                                                <asp:BoundField DataField="SendTotal" HeaderText="Sends" SortExpression="SendTotal"
                                                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundField>
                                                <asp:HyperLinkField HeaderText="Web View" Text="<img src='/ecn.images/images/icon-preview.gif' alt='Preview Campaign for the Blast' border='0'>"
                                                    Target="_blank" DataNavigateUrlFields="BlastID" DataNavigateUrlFormatString="~/main/blasts/preview.aspx?BlastID={0}"
                                                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:HyperLinkField>
                                                <asp:TemplateField HeaderText="Email View" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEmailPreview" runat="server" ImageUrl="~/images/icon-emailpreview.png"
                                                            CausesValidation="false" Visible='<%# CheckEmailPreviewVisible( Eval("TestBlast").ToString(), Eval("HasEmailPreview").ToString())%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reports" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="HyperLink1" runat="server" Text="<img src='/ecn.images/images/icon-reports.gif' alt='View Blast Reporting' border='0'>"
                                                            NavigateUrl='<%# "~/main/blasts/reports.aspx?blastID=" + Eval("BlastID") %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:HyperLinkField HeaderText="PDF" Text="<img src='/ecn.images/images/icon-pdf.gif' alt='Download report in PDF format' border='0'>"
                                                    DataNavigateUrlFields="BlastID" DataNavigateUrlFormatString="~/main/blasts/rpt_BlastReport.aspx?BlastID={0}"
                                                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:HyperLinkField>
                                                <asp:HyperLinkField HeaderText="Excel" Text="<img width='16px' src='/ecn.images/images/icon-excel.gif' alt='Download report in Excel format' border='0'>"
                                                    DataNavigateUrlFields="BlastID" DataNavigateUrlFormatString="~/main/blasts/rpt_BlastReport.aspx?BlastID={0}&ExportType=Excel"
                                                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:HyperLinkField>
                                                <asp:TemplateField HeaderText="" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkbtnResend" CommandName="ResendTestBlast" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BlastID") %>'>Resend</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
                    <asp:Label ID="gvSent_lblTotalRecords" runat="server" Text="" />
                </td>
                <td align="left" class="label" width="25%">Show Rows:
                    <asp:DropDownList ID="gvSent_ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="gvSent_SelectedIndexChanged"
                        CssClass="formfield">
                        <asp:ListItem Value="5" />
                        <asp:ListItem Value="10" />
                        <asp:ListItem Value="15" />
                        <asp:ListItem Value="20" />
                    </asp:DropDownList>
                </td>
                <td align="right" class="label" width="44%">Page
                    <asp:TextBox ID="gvSent_txtGoToPage" runat="server" AutoPostBack="true" OnTextChanged="gvSent_TextChanged"
                        class="formtextfield" Width="30px" />
                    of
                    <asp:Label ID="gvSent_lblTotalNumberOfPages" runat="server" CssClass="label" />
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


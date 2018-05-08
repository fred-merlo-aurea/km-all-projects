<%@ Control Language="c#" Inherits="ecn.communicator.main.ECNWizard.Controls.WizardSchedule"
    CodeBehind="WizardSchedule.ascx.cs" AutoEventWireup="true" %>

<%@ Register Src="../../blasts/BlastScheduler.ascx" TagName="BlastScheduler" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ Register Src="~/main/ECNWizard/Group/testgroupExplorer.ascx" TagName="testgroupExplorer" TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/Group/groupsLookup.ascx" TagName="groupsLookup" TagPrefix="uc1" %>


<style type="text/css">
    td {
        border-style: none;
    }

    .LabelHeading {
        color: DimGray;
        font-size: medium;
        font-weight: bold;
    }

    fieldset {
        -webkit-border-radius: 8px;
        -moz-border-radius: 8px;
        border-radius: 8px;
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

    .modalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }

    .modalPopupFull {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        width: 100%;
        height: 100%;
        overflow: auto;
    }

    .modalPopup {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        overflow: auto;
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

    .modalPopupScheduleReport {
        position: fixed;
        width: 1000px;
        height: 100%;
        overflow: auto;
        background-color: #e6e7e8;
        border: 2px solid black;
        padding: 20px 20px 20px 0px;
    }

    .modalTextBox {
        width: 250px;
    }

    .btnScheduleReport {
        font-weight: bold;
        width: 280px;
    }

    lblScheduleReport {
        width: 85%;
        align-self: center;
    }
</style>
<script type="text/javascript">
    function getobj(id) {
        if (document.all && !document.getElementById)
            obj = eval('document.all.' + id);
        else if (document.layers)
            obj = eval('document.' + id);
        else if (document.getElementById)
            obj = document.getElementById(id);

        return obj;
    }

    function replyTo_focus() {
        document.BlastForm.ReplyTo.value = document.BlastForm.EmailFrom.value;
    }

    function PopulateData() {
        document.getElementById('<%= btPopupLoad.ClientID %>').click();
    }

</script>

<%--<asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10" Visible="true"
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
</asp:UpdateProgress>--%>
<asp:Panel ID="UpdatePanel1" runat="server">
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
    <asp:Panel ID="pnlBlastOptions" runat="server">
        <div class="section" id="content">
            <table cellspacing="0" cellpadding="2" width="100%" border="0" style="padding-left: 40px">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Label1" runat="server" Text="Tracking Options" CssClass="LabelHeading"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="20%">&nbsp;
                    </td>
                    <td align="left" width="80%">
                        <table>
                            <tr valign="middle">
                                <td>
                                    <asp:CheckBox ID="chkboxGoogleAnalytics" runat="server" Text="Enable Google Analytics Tracking" OnCheckedChanged="chkboxGoogleAnalytics_CheckedChanged" AutoPostBack="true" />
                                    <asp:ImageButton ID="imgGoogleAnalyticsSettings" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgGoogleAnalyticsSettings_Click" Visible="true" />

                                </td>
                                <td align="left"></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnlGoogleAnalytics" runat="server" Visible="false">
                                        <br />
                                        <table style="padding-left: 30px">
                                            <tr>
                                                <td>Campaign Source</td>
                                                <td>
                                                    <asp:DropDownList ID="drpCampaignSource" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCampaignSource_SelectedIndexChanged" CssClass="styled-select">
                                                        <asp:ListItem Text="KnowledgeMarketing" Value="1" Selected="True" />
                                                        <asp:ListItem Text="CustomValue" Value="6" />
                                                        </asp:DropDownList>*
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCampaignSource" runat="server" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Campaign Medium
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drpCampaignMedium" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCampaignMedium_SelectedIndexChanged" CssClass="styled-select">
                                                    </asp:DropDownList>*&nbsp; &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCampaignMedium" runat="server" Visible="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Campaign Term
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drpCampaignTerm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCampaignTerm_SelectedIndexChanged" CssClass="styled-select">
                                                    </asp:DropDownList>&nbsp; &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCampaignTerm" runat="server" Visible="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Campaign Content
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drpCampaignContent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCampaignContent_SelectedIndexChanged" CssClass="styled-select">
                                                    </asp:DropDownList>&nbsp; &nbsp;
                                                </td>
                                                <td>
                                         <asp:TextBox ID="txtCampaignContent" runat="server" Visible="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Campaign Name
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drpCampaignName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCampaignName_SelectedIndexChanged" CssClass="styled-select">
                                                    </asp:DropDownList>*&nbsp; &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCampaignName" runat="server" Visible="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="20%">&nbsp;

                    </td>
                    <td align="left" width="80%">
                        <table>
                            <tr style="vertical-align: middle;">
                                <td>
                                    <asp:CheckBox ID="chkboxOmnitureTracking" runat="server" Text="Enable Omniture Tracking" OnCheckedChanged="chkboxOmnitureTracking_CheckedChanged" AutoPostBack="true" />
                                    <asp:ImageButton ID="imgbtnOmnitureSettings" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgbtnOmnitureSettings_Click" Visible="true" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlOmniture" runat="server" Visible="false">
                                                    <br />
                                                    <table style="padding-left: 30px;">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblOmniture1" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOmniture1" runat="server" OnSelectedIndexChanged="ddlOmniture1_SelectedIndexChanged" CssClass="styled-select" AutoPostBack="true" />&nbsp; &nbsp;
                                                                    <asp:TextBox ID="txtOmniture1" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblOmniture2" runat="server" />

                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOmniture2" runat="server" OnSelectedIndexChanged="ddlOmniture2_SelectedIndexChanged" CssClass="styled-select" AutoPostBack="true"></asp:DropDownList>&nbsp; &nbsp;
                                                                    <asp:TextBox ID="txtOmniture2" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblOmniture3" runat="server" />

                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOmniture3" runat="server" OnSelectedIndexChanged="ddlOmniture3_SelectedIndexChanged" CssClass="styled-select" AutoPostBack="true"></asp:DropDownList>&nbsp; &nbsp;
                                                                    <asp:TextBox ID="txtOmniture3" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblOmniture4" runat="server" />

                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOmniture4" runat="server" OnSelectedIndexChanged="ddlOmniture4_SelectedIndexChanged" CssClass="styled-select" AutoPostBack="true"></asp:DropDownList>&nbsp; &nbsp;
                                                                    <asp:TextBox ID="txtOmniture4" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblOmniture5" runat="server" />

                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOmniture5" runat="server" OnSelectedIndexChanged="ddlOmniture5_SelectedIndexChanged" CssClass="styled-select" AutoPostBack="true"></asp:DropDownList>&nbsp; &nbsp;
                                                                    <asp:TextBox ID="txtOmniture5" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblOmniture6" runat="server" />

                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOmniture6" runat="server" OnSelectedIndexChanged="ddlOmniture6_SelectedIndexChanged" CssClass="styled-select" AutoPostBack="true"></asp:DropDownList>&nbsp; &nbsp;
                                                                    <asp:TextBox ID="txtOmniture6" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblOmniture7" runat="server" />

                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOmniture7" runat="server" OnSelectedIndexChanged="ddlOmniture7_SelectedIndexChanged" CssClass="styled-select" AutoPostBack="true"></asp:DropDownList>&nbsp; &nbsp;
                                                                    <asp:TextBox ID="txtOmniture7" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblOmniture8" runat="server" />

                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOmniture8" runat="server" OnSelectedIndexChanged="ddlOmniture8_SelectedIndexChanged" CssClass="styled-select" AutoPostBack="true"></asp:DropDownList>&nbsp; &nbsp;
                                                                    <asp:TextBox ID="txtOmniture8" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblOmniture9" runat="server" />

                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOmniture9" runat="server" OnSelectedIndexChanged="ddlOmniture9_SelectedIndexChanged" CssClass="styled-select" AutoPostBack="true"></asp:DropDownList>&nbsp; &nbsp;
                                                                    <asp:TextBox ID="txtOmniture9" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblOmniture10" runat="server" />

                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOmniture10" runat="server" OnSelectedIndexChanged="ddlOmniture10_SelectedIndexChanged" CssClass="styled-select" AutoPostBack="true"></asp:DropDownList>&nbsp; &nbsp;
                                                                    <asp:TextBox ID="txtOmniture10" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="20%">&nbsp;
                    </td>
                    <td align="left" width="80%">
                        <asp:CheckBox ID="chkboxConvTracking" runat="server" Text="Enable ECN Conversion Tracking" />
                        <asp:ImageButton ID="imgbtnConvTrackingSettings" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgbtnConvTrackingSettings_Click" Visible="true" />

                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Label2" runat="server" Text="Opt-out Preference" CssClass="LabelHeading"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="20%">&nbsp;
                    </td>
                    <td align="left" width="80%">
                        <asp:CheckBox ID="chkOptOutMasterSuppression" runat="server" Text="Add to Master Suppression" OnCheckedChanged="chkOptOutMasterSuppression_CheckedChanged" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td align="left" width="20%">&nbsp;
                    </td>
                    <td align="left" width="80%">
                        <table>
                            <tr valign="middle">
                                <td>
                                    <asp:CheckBox ID="chkOptOutSpecificGroup" runat="server" Text="Opt-out from Specific Groups" OnCheckedChanged="chkOptOutSpecificGroup_CheckedChanged" AutoPostBack="true" />
                                </td>
                                <td align="left"></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="pnlOptOutSpecificGroups" runat="server" UpdateMode="Conditional" Visible="false">
                                        <ContentTemplate>
                                            <br />
                                            <table style="padding-left: 30px">
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lnkSelectOptOutGroups" runat="server" OnClick="lnkSelectOptOutGroups_Click">Click Here</asp:LinkButton>
                                                        to Select Groups for Opt-out
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="gvOptOutGroups" runat="server" OnRowCommand="gvOptOutGroups_RowCommand" GridLines="None"
                                                            AllowSorting="false" AutoGenerateColumns="false" Width="100%" AllowPaging="false" ShowFooter="false" ShowHeader="false" DataKeyNames="CampaignItemOptOutID">
                                                            <Columns>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="CampaignItemOptOutID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CampaignItemOptOutID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGroupName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.GroupName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                                                            CommandName="CampaignItemOptOutGroupDelete" OnClientClick="return confirm('Are you sure you want to delete this Group from the Opt-out list?')"
                                                                            CausesValidation="false" CommandArgument='<%#Eval("CampaignItemOptOutID")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblCacheBuster" Text="Google &reg; Image Cache Buster" runat="server" CssClass="LabelHeading" />
                    </td>
                </tr>
                <tr>
                    <td align="left" width="20%">&nbsp;
                    </td>
                    <td align="left" width="80%">
                        <asp:CheckBox ID="chkCacheBuster" runat="server" Checked="true" Text="Enable Cache Buster" AutoPostBack="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblScheduleReport" Text="Schedule Report" runat="server" CssClass="LabelHeading" />
                    </td>
                </tr>
                <tr>
                    <td align="left" width="20%"></td>
                    <td align="left" width="80%">
                        <asp:CheckBox ID="chkScheduleReport" runat="server" Checked="false" Text="Schedule Reports" OnCheckedChanged="chkScheduleReport_CheckedChanged" AutoPostBack="true" />
                        <asp:Button ID="btnEditScheduleReport" runat="server" OnClick="btnEditScheduleReport_Click" Text="Edit" class="ECN-Button-Medium" Visible="false" Enabled="true" />
                    </td>
                </tr>

            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlTestOptions" runat="server">
        <div>
            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td colspan="2" align="center">
                        <asp:PlaceHolder ID="phMessage" runat="server" Visible="false">
                            <table cellspacing="0" cellpadding="0" width="674" align="center">
                                <tr>
                                    <td id="successTop"></td>
                                </tr>
                                <tr>
                                    <td id="successMiddle">
                                        <table height="67" width="80%">
                                            <tr>
                                                <td valign="top" align="center" width="20%">
                                                    <img src="/ecn.images/images/checkmark.gif">
                                                </td>
                                                <td valign="middle" align="left" width="80%" height="100%">
                                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="successBottom"></td>
                                </tr>
                            </table>
                        </asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td width="5%">&nbsp;
                    </td>
                    <td>
                        <asp:RadioButton ID="rbTestNew" AutoPostBack="true" CssClass="formLabel" GroupName="grptestSelect"
                            Text="Create New Group &nbsp;<span class='highLightOne'>(10 Recipients or Less. One Email address per line)</span>"
                            runat="Server" OnCheckedChanged="rbTestNew_CheckedChanged"></asp:RadioButton>
                    </td>
                </tr>
                <asp:PlaceHolder ID="phTestNew" Visible="false" runat="server">
                    <tr>
                        <td width="5%">&nbsp;
                        </td>
                        <td style="padding-left: 30px" colspan="3">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="label10" align="left" width="15%">
                                        <strong>Save in &nbsp;Folder:&nbsp;</strong>
                                    </td>
                                    <td class="label10" align="left" width="85%">
                                        <asp:DropDownList ID="drpFolder" runat="server" CssClass="styled-select" Width="250px"
                                            EnableViewState="true" DataTextField="FolderName" DataValueField="FolderID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label10" align="left">
                                        <strong>Group&nbsp;Name:&nbsp;</strong>
                                    </td>
                                    <td class="label10" align="left">
                                        <asp:TextBox ID="txtGroupName" runat="server" CssClass="styled-text" Width="250" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label10" align="left">
                                        <strong>Email Address:&nbsp;</strong>
                                    </td>
                                    <td class="label10" align="left">
                                        <asp:TextBox ID="txtEmailAddress" runat="server" EnableViewState="true" Rows="5"
                                            Columns="58" TextMode="multiline" CssClass="label10"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </asp:PlaceHolder>
                <tr>
                    <td width="5%">&nbsp;
                    </td>
                    <td>
                        <asp:RadioButton ID="rbTestExisting" AutoPostBack="true" CssClass="formLabel" GroupName="grptestSelect"
                            Text="Use Existing Group" Checked="true" runat="Server" OnCheckedChanged="rbTestExisting_CheckedChanged"></asp:RadioButton>
                    </td>
                </tr>
                <asp:PlaceHolder ID="phTestExisting" Visible="true" runat="server">
                    <tr>
                        <td width="5%">&nbsp;
                        </td>
                        <td style="padding-left: 30px" colspan="3">
                            <uc1:testgroupExplorer ID="testgroupExplorer1" runat="server" />
                        </td>
                    </tr>
                </asp:PlaceHolder>
                <tr>
                    <td width="5%">&nbsp;
                    </td>
                    <td><br />
                        <asp:Label ID="Label6" Text="Google &reg; Image Cache Buster" runat="server" CssClass="LabelHeading" />
                    </td>
                </tr>
                <tr>
                    <td align="left" width="5%">&nbsp;
                    </td>
                    <td align="left" style="padding-left:200px;" >
                        <asp:CheckBox ID="chkGoogleTestBuster" runat="server" Checked="true" Text="Enable Cache Buster" AutoPostBack="false" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

    <div class="section">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-left: 40px">
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label3" runat="server" Text="Schedule Blast" CssClass="LabelHeading"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <uc1:BlastScheduler ID="BlastScheduler1" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="left" width="20%">&nbsp;
                </td>
                <td  align="left" width="80%">
                    <asp:CheckBox ID="chkGoToChampion" runat="server" Visible="false" Text="Schedule Champion" />
                </td>
            </tr>
            <tr>
                <td align="left" width="20%">&nbsp;
                </td>
                <td align="left" width="80%">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSchedule_Click_CheckMA" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hfLinkTrackingDomain" runat="server" Value="1" />

</asp:Panel>

<asp:UpdatePanel ID="upgroupExplorer" runat="server" UpdateMode="Always">
    <ContentTemplate>
    <uc1:groupsLookup ID="ctrlgroupsLookup1" runat="server" Visible="false" />
    </ContentTemplate>
</asp:UpdatePanel>


<asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="modalPopupLinkTrackingDomain" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlLinkTrackingDomain" TargetControlID="btnShowPopup1">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlLinkTrackingDomain" CssClass="modalPopup">
    <asp:UpdateProgress ID="upLinkTrackingDomain" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel2" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upLinkTrackingDomainP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upLinkTrackingDomainP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <br />
            <asp:Label ID="lblLinkTrackingDomain" runat="server" Text="Link Tracking Settings" Font-Bold="true" Font-Size="Medium"></asp:Label>
            <br />
            <asp:PlaceHolder ID="PlaceHolder1" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td>
                            <table height="67" width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                    </td>
                                    <td valign="middle" align="left" width="80%" height="100%">
                                        <asp:Label ID="lblLinkTrackingDomain_Error" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>
            <br />
            <asp:RadioButtonList ID="rblLinkTracking" runat="server" OnSelectedIndexChanged="rblLinkTracking_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal">
                <asp:ListItem Text="Track All Domains" Value="all" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Track Specific Domains" Value="specific"></asp:ListItem>
            </asp:RadioButtonList>
            <asp:Panel ID="pnlDomainList" runat="server">
                <table width="100%">
                    <tr>
                        <td>Domain Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtDomainName" runat="server"></asp:TextBox>&nbsp;
                &nbsp; 
                  <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-add.gif alt='Add Domain' border='0'&gt;"
                      CausesValidation="false" ID="btnLinkTrackingDomainAdd" OnClick="btnLinkTrackingDomainAdd_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Label ID="lblDomainError" ForeColor="Red" Font-Size="Small" Visible="false" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <ecnCustom:ecnGridView ID="gvLinkTrackingDomains" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                                Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="LinkTrackingDomainID" CssClass="grid"
                                OnRowCommand="gvLinkTrackingDomains_RowCommand">
                                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLinkTrackingDomainID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.LinkTrackingDomainID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="Domain" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDomain" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Domain") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                                CommandName="LinkTrackingDomain" OnClientClick="return confirm('Are you sure, you want to delete tracking for this Domain?')"
                                                CausesValidation="false" CommandArgument='<%#Eval("LinkTrackingDomainID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </ecnCustom:ecnGridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: right">
                        <asp:Button runat="server" Text="Save" ID="btnSave_LinkTrackingDomain" CssClass="formfield"
                            OnClick="LinkTrackingDomain_Save"></asp:Button>
                    </td>
                    <td style="text-align: left">
                        <asp:Button runat="server" Text="Close" ID="btnClose_LinkTrackingDomain" OnClick="LinkTrackingDomain_Close" CssClass="formfield"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>


<asp:Button ID="btnShowScheduleReportPopup" runat="server" Style="display: none" />
<ajax:ModalPopupExtender ID="modalPopupScheduleReport" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlScheduleReport" TargetControlID="btnShowScheduleReportPopup">
</ajax:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlScheduleReport" CssClass="modalPopupScheduleReport" Height="100%">
    <asp:UpdatePanel ID="updpnlScheduleReport" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:Button ID="btPopupLoad" runat="server" Text="Load"
                CssClass="button" OnClick="btnEditScheduleReport_Click" Style="display: none;" />
            <h2 align="center"><span>Create Schedule for Report</span></h2>

            <hr width="85%" />

            <div align="center" style="margin: 25px 50px 25px 50px">
                <table width="95%" cellspacing="5" align="center">
                    <tr>
                        <td colspan="4" align="center">
                           
                            <asp:CheckBox ID="chkEmailBlastReport" runat="server" Style="font-size: 20px;" Text="Email Campaign Item Report" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:CheckBox ID="chkFtpExport" runat="server" Style="font-size: 20px;" OnCheckedChanged="chkFtpExport_CheckedChanged" AutoPostBack="true" Text="Schedule FTP Export" />
                        </td>
                    </tr>
                    <asp:Panel ID="pnlFtp" runat="server" Visible="false">
                        <tr>
                            <td colspan="2" align="right" width="55%">
                                <asp:Label runat="server"> Statistics to Export</asp:Label>
                            </td>
                            <td colspan="2" align="left" width="45%">
                                <asp:ListBox ID="lbFtpExports" runat="server" SelectionMode="Multiple">
                                    <asp:ListItem Value="sends">Sends</asp:ListItem>
                                    <asp:ListItem Value="opens">Opens</asp:ListItem>
                                    <asp:ListItem Value="unopened">Unopened</asp:ListItem>
                                    <asp:ListItem Value="clicks">Clicks</asp:ListItem>
                                    <asp:ListItem Value="no-clicks">No-clicks</asp:ListItem>
                                    <asp:ListItem Value="bounces">Bounces</asp:ListItem>
                                    <asp:ListItem Value="resends">Resends</asp:ListItem>
                                    <asp:ListItem Value="forwards">Forwards</asp:ListItem>
                                    <asp:ListItem Value="unsubscribes">Unsubscribes</asp:ListItem>
                                    <asp:ListItem Value="suppressed">Suppressed</asp:ListItem>
                                </asp:ListBox>
                                <asp:RequiredFieldValidator ID="rfvFtpExports" runat="server" ControlToValidate="lbFtpExports" ErrorMessage="*" ValidationGroup="schedule" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <asp:Label ID="lblFtpDetails" runat="server" Text="FTP Details" Font-Size="Large"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblFTPURL" Text="FTP URL:" runat="server" CssClass="label" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtFtpUrl" runat="server" CssClass="styled-text" />
                                <asp:RequiredFieldValidator ID="rfvFtpUrl" runat="server" ControlToValidate="txtFtpUrl" ErrorMessage="*" ValidationGroup="schedule" Enabled="false" />
                                <asp:RegularExpressionValidator ID="revFTPURL" runat="server" ValidationExpression="^(ftp|ftps|sftp)://.+$" ControlToValidate="txtFtpUrl" ErrorMessage="Invalid URL" ForeColor="Red" />
                            </td>
                            <td align="right">
                                <asp:Label ID="lblFormat" runat="server" Text="Export to:" CssClass="label" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFormat" runat="server" CssClass="styled-text">
                                    <asp:ListItem Text="EXCEL [.xls]" Value=".xls" Selected="True" />
                                    <asp:ListItem Text="XML [.xml]" Value=".xml" />
                                    <asp:ListItem Text="CSV [.csv]" Value=".csv" />
                                    <asp:ListItem Text="TXT [.txt]" Value=".txt" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblFtpUsername" Text="Username:" runat="server" CssClass="label" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtFtpUsername" runat="server" CssClass="styled-text" />
                                <asp:RequiredFieldValidator ID="rfvFtpUsername" runat="server" ControlToValidate="txtFtpUsername" ErrorMessage="*" ValidationGroup="schedule" Enabled="false" />
                            </td>
                            <td align="right">
                                <asp:Label ID="lblFtpPassword" Text="Password:" runat="server" CssClass="label" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtFtpPassword" runat="server" CssClass="styled-text" />
                                <asp:RequiredFieldValidator ID="rfvFtpPassword" runat="server" ControlToValidate="txtFtpPassword" ErrorMessage="*" ValidationGroup="schedule" Enabled="false" />
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="left">
                            <asp:Label ID="Label4" runat="server" Text="Envelope" Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="15%">From Email :
                        </td>
                        <td align="left" width="35%">
                            <asp:TextBox ID="txtFromEmail" runat="server" CssClass="styled-text"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFromEmail" runat="server" ControlToValidate="txtFromEmail" ErrorMessage="*" ValidationGroup="schedule" />
                        </td>
                        <td align="right" width="15%">To Email :
                        </td>
                        <td align="left" width="35%">
                            <asp:TextBox ID="txtToEmail" runat="server" CssClass="styled-text"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvToEmail" runat="server" ControlToValidate="txtToEmail" ErrorMessage="*" ValidationGroup="schedule" />
                        </td>
                    </tr>

                    <tr>
                        <td align="right">Subject :
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="styled-text"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" ErrorMessage="*" ValidationGroup="schedule" />
                        </td>
                        <td align="right">From Name :
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtFromName" runat="server" CssClass="styled-text" OnTextChanged="txtFromName_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFromName" runat="server" ControlToValidate="txtFromName" ErrorMessage="*" ValidationGroup="schedule" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblAddCc" runat="server" align="right" Text="Cc:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddCc" runat="server" CssClass="styled-text"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">Separate emails with commas when entering a list. [5 Max]</td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="left">
                            <asp:Label ID="Label5" runat="server" Text="Schedule Report" Font-Size="Large"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td align="right">Date :</td>
                        <td>
                            <asp:TextBox ID="txtReportDate" runat="server" CssClass="styled-text" MaxLength="10"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtReportDate"
                                Format="MM/dd/yyyy">
                            </ajaxToolkit:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvtxtReportDate" runat="server" ControlToValidate="txtReportDate" ValidationGroup="schedule"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td align="right">Send Time :</td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlReportTime" runat="server" CssClass="styled-text">
                                <asp:ListItem Value="0:00:00">0:00:00</asp:ListItem>
                                <asp:ListItem Value="1:00:00">1:00:00</asp:ListItem>
                                <asp:ListItem Value="2:00:00">2:00:00</asp:ListItem>
                                <asp:ListItem Value="3:00:00">3:00:00</asp:ListItem>
                                <asp:ListItem Value="4:00:00">4:00:00</asp:ListItem>
                                <asp:ListItem Value="5:00:00">5:00:00</asp:ListItem>
                                <asp:ListItem Value="6:00:00">6:00:00</asp:ListItem>
                                <asp:ListItem Value="7:00:00">7:00:00</asp:ListItem>
                                <asp:ListItem Value="8:00:00">8:00:00</asp:ListItem>
                                <asp:ListItem Value="9:00:00">9:00:00</asp:ListItem>
                                <asp:ListItem Value="10:00:00">10:00:00</asp:ListItem>
                                <asp:ListItem Value="11:00:00">11:00:00</asp:ListItem>
                                <asp:ListItem Value="12:00:00">12:00:00</asp:ListItem>
                                <asp:ListItem Value="13:00:00">13:00:00</asp:ListItem>
                                <asp:ListItem Value="14:00:00">14:00:00</asp:ListItem>
                                <asp:ListItem Value="15:00:00">15:00:00</asp:ListItem>
                                <asp:ListItem Value="16:00:00">16:00:00</asp:ListItem>
                                <asp:ListItem Value="17:00:00">17:00:00</asp:ListItem>
                                <asp:ListItem Value="18:00:00">18:00:00</asp:ListItem>
                                <asp:ListItem Value="19:00:00">19:00:00</asp:ListItem>
                                <asp:ListItem Value="20:00:00">20:00:00</asp:ListItem>
                                <asp:ListItem Value="21:00:00">21:00:00</asp:ListItem>
                                <asp:ListItem Value="22:00:00">22:00:00</asp:ListItem>
                                <asp:ListItem Value="23:00:00">23:00:00</asp:ListItem>
                            </asp:DropDownList>&nbsp;&nbsp;CST

                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <div runat="server" id="divScheduleReportErrorMessage"></div>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnSaveReportDetails" runat="server" OnClick="btnSaveReportDetails_Click" Visible="true" Text="Save" CausesValidation="true" ValidationGroup="schedule" Style="width: 60px; margin-left: 350px; margin-right: 9px" />
                <asp:Button ID="btnCancelDetailEdits" runat="server" OnClick="btnCancelDetailEdits_Click" Text="Cancel" Visible="true" align="center" Style="width: 60px; margin-left: 9px; margin-right: 350px;" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:Button ID="btnMACheck" runat="server" Style="display:none;" />
<ajax:ModalPopupExtender ID="mpeMACheck" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlMACheck" TargetControlID="btnMACheck" ></ajax:ModalPopupExtender>
<asp:Panel ID="pnlMACheck" runat="server" CssClass="modalPopup" Height="200px" Width="350px">
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
    <asp:UpdatePanel ID="upMACheck" runat="server" UpdateMode="Conditional" style="height:100%;">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnMACheckContinue" />
        </Triggers>
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
                                        <asp:Label ID="lblMAText" Text="" runat="server" />
                                    </td>
                                </tr>

                            </table>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:Button runat="server" Text="Continue" ID="btnMACheckContinue" CssClass="formfield"
                                 OnClick="btnSchedule_Click"></asp:Button>
                            &nbsp; &nbsp;
                             <asp:Button runat="server" Text="Cancel" ID="btnMACheckCancel" CssClass="formfield"
                                   OnClick="btnMACheckCancel_Click"></asp:Button>
                        </td>
                        <td style="text-align: center"></td>
                    </tr>

                </table>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>


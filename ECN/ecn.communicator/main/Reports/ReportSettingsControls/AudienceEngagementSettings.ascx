<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AudienceEngagementSettings.ascx.cs" Inherits="ecn.communicator.main.Reports.ReportSettingsControls.AudienceEngagementSettings" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/main/ECNWizard/Group/groupsLookup.ascx" TagName="groupsLookup" TagPrefix="uc1" %>
<style type="text/css">
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

    .modalPopupLayoutExplorer {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: white;
        padding: 3px;
        overflow: auto;
    }

    .modalPopupGroupExplorer {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: white;
        padding: 3px;
        overflow: auto;
    }

    .modalPopupImport {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        height: 60%;
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

    fieldset {
        -webkit-border-radius: 8px;
        -moz-border-radius: 8px;
        border-radius: 8px;
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
</style>
<br />
<asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="upMain" DynamicLayout="false">
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
<asp:UpdatePanel ID="upMain" runat="server">
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Group" CssClass="label"></asp:Label>
                    <asp:ImageButton ID="imgSelectGroup" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgSelectGroup_Click" Visible="true" />

                </td>
                <td>
                    <asp:Label ID="lblSelectGroupName" runat="server" Text="-No Group Selected-" Font-Size="Small"></asp:Label>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Click Percentage" CssClass="label"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtClickPercentage" runat="server"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbClickPerc" runat="server" TargetControlID="txtClickPercentage" FilterType="Numbers" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hfSelectGroupID" runat="server" Value="0" />
        <uc1:groupsLookup ID="ctrlgroupsLookup1" runat="server" Visible="false" />
    </ContentTemplate>
</asp:UpdatePanel>

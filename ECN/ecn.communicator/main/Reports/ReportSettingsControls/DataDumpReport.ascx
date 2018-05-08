<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataDumpReport.ascx.cs" Inherits="ecn.communicator.main.Reports.ReportSettingsControls.DataDumpReport" %>
<%@ Register Src="~/main/ECNWizard/Group/groupsExplorer.ascx" TagName="groupsExplorer" TagPrefix="uc1" %>
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
                <td width="35%" align='right'>
                    <asp:Label ID="lblGroupList" CssClass="label" runat="server" Text="Group / List" />
                    <asp:ImageButton ID="imgSelectGroup" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" CausesValidation="false" OnClick="imgSelectGroup_Click" Visible="true" />
                    <asp:HiddenField ID="hfGroupSelectionMode" Value="" runat="server" />
                </td>
                <td width="65%"><b>Up to 1 year of statistics history is available.</b></td>
            </tr>
            <tr>
                <td align='right'></td>
                <td>
                    
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblSelected" CssClass="label" runat="server" Text="Selected Groups" />
                </td>
                <td>
                    <asp:Label ID="lblNoGroups" CssClass="label" runat="server" Text="No groups selected" />
                    <asp:GridView ID="gvSelectedGroups" runat="server" GridLines="None" DataKeyNames="GroupID" OnRowCommand="gvSelectedGroups_RowCommand" OnRowDataBound="gvSelectedGroups_RowDataBound" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="GroupName" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnDeleteGroup" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="deletegroup" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFTPURL" Text="FTP URL" runat="server" CssClass="label" />
                </td>
                <td>
                    <asp:TextBox ID="txtFTPURL" runat="server" />
                    <asp:RegularExpressionValidator ID="revFTPURL" runat="server" ValidationExpression="^(ftp|ftps|sftp)://.+$" ControlToValidate="txtFTPURL" ErrorMessage="Invalid URL" ForeColor="Red" />
                    <asp:RequiredFieldValidator ID="rfvURL" runat="server" ControlToValidate="txtFTPURL" ErrorMessage="Required" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFTPUsername" Text="Username" runat="server" CssClass="label" />
                </td>
                <td>
                    <asp:TextBox ID="txtFTPUsername" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvURLname" runat="server" ControlToValidate="txtFTPUsername" ErrorMessage="Required" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFTPPassword" Text="Password" runat="server" CssClass="label" />
                </td>
                <td>
                    <asp:TextBox ID="txtFTPPassword" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvURLpassword" runat="server" ControlToValidate="txtFTPPassword" ErrorMessage="Required" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />

<ajaxToolkit:ModalPopupExtender ID="modalPopupGroupExplorer" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlgroupExplorer" TargetControlID="btnShowPopup2">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlgroupExplorer" CssClass="modalPopupGroupExplorer">
    <asp:UpdateProgress ID="upgroupExplorerProgress" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upgroupExplorer" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upgroupExplorerProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upgroupExplorerProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upgroupExplorer" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <table bgcolor="white">
                <tr style="background-color: #5783BD;">
                    <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Group Explorer
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc1:groupsexplorer id="groupExplorer1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Button runat="server" Text="Close" CausesValidation="false" ID="btngroupExplorer" CssClass="aspBtn1"
                            OnClick="groupExplorer_Hide"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

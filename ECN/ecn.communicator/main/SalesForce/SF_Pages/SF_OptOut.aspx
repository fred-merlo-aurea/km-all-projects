<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="SF_OptOut.aspx.cs" Inherits="ecn.communicator.main.Salesforce.SF_Pages.SF_OptOut" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            setScrollPosSF();

        }
        function saveScrollPosSF() {
            document.getElementById("<% =hfSFscrollPos.ClientID %>").value =
                     document.getElementById("<% =divOptOut.ClientID %>").scrollTop;
        }
        function setScrollPosSF() {
            document.getElementById("<% =divOptOut.ClientID %>").scrollTop =
                     document.getElementById("<% =hfSFscrollPos.ClientID %>").value;
        }


    </script>
    <asp:UpdatePanel ID="upOptOut" runat="server">
        <ContentTemplate>


            <input type="hidden" id="hfSFscrollPos" name="SFscrollPos" value="0" runat="server" />
            <asp:Panel ID="pnlOptOut" runat="server">
                <table style="width: 100%; padding-top: 20px;">
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="lblHeader" Text="Sync Opt-out" CssClass="ECN-PageHeading" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; text-align: center;">

                            <table style="text-align: center;">
                                <tr>
                                    <td style="background-color: #4b87bc; width: 20px;"></td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbGreen" runat="server" Text=" = opted-out of email in SF and ECN"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #939597; width: 20px;"></td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbYellow" runat="server" Text=" = opted-out of email in either SF or ECN"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #e6e7e8; width: 20px;"></td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbOrange" runat="server" Text=" = not opted-out of email"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: left;">

                            <asp:UpdatePanel ID="upECNGroup" runat="server" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="ddlECNFolder" />
                                    <asp:PostBackTrigger ControlID="ddlECNGroups" />
                                    <asp:PostBackTrigger ControlID="btnSyncOptOut" />
                                    <asp:PostBackTrigger ControlID="btnSyncSelected" />
                                </Triggers>
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFolder" Text="Folder:" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlECNFolder" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlECNFolder_SelectedIndexChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblGroup" runat="server" Text="Group:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlECNGroups" Width="200px" OnSelectedIndexChanged="ddlECNGroups_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table style="width: 100%; margin-top: 20px;">
                                                    <tr>
                                                        <td style="width: 40%; text-align: left;">
                                                            <asp:Label ID="lblFilter" Text="Show emails that are:" runat="server" />
                                                        </td>
                                                        <td style="width: 60%; text-align: left;">
                                                            <asp:DropDownList ID="ddlFilter" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                                                                <asp:ListItem Text="Show All" Value="all" Selected="True" />
                                                                <asp:ListItem Text="Opted-out of both" Value="both" />
                                                                <asp:ListItem Text="Opted-out of SF" Value="sf" />
                                                                <asp:ListItem Text="Opted-out of ECN" Value="ecn" />
                                                                <asp:ListItem Text="Not opted-out" Value="none" />
                                                            </asp:DropDownList>
                                                        </td>

                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center;">
                                                <table style="width: 100%; margin-top: 20px;">
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <asp:Button ID="btnSyncOptOut" runat="server" OnClick="btnSyncOptOut_Click" Text="Sync All" CssClass="formbutton" />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:Button ID="btnSyncSelected" runat="server" Text="Sync Selected" CssClass="formbutton" OnClick="btnSyncSelected_Click" />
                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </table>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 20%; border-top: 1px solid #808080;"></td>
                        <td style="text-align: center; border-top: 1px solid #808080; padding-top: 10px; width: 60%;">

                            <div id="divOptOut" runat="server" visible="false" onscroll="saveScrollPosSF()" style="overflow: auto; height: 600px; text-align: center;">
                                <asp:GridView ID="gvOptOut" runat="server" AllowSorting="true" DataKeyNames="SFId" CssClass="km_grid" Width="100%" AlternatingItemStyle-CssClass="km_gridAltColor" OnRowDataBound="gvOptOut_RowDataBound" OnSorting="gvOptOut_Sorting" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SF opt-out" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSFOptOutAll" OnCheckedChanged="chkSFOptOutAll_CheckedChanged" Text="SF Opt-out" AutoPostBack="true" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSFOptOut" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SF_Type" SortExpression="type" HeaderText="SF object type" />
                                        <asp:BoundField DataField="Email" SortExpression="email" HeaderText="Email" />
                                        <asp:TemplateField HeaderText="ECN opt-out" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkECNOptOutAll" OnCheckedChanged="chkECNOptOutAll_CheckedChanged" Text="ECN Opt-out" AutoPostBack="true" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkECNOptOut" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <AU:PagerBuilder ID="EmailsPager" HorizontalAlign="Left" runat="Server" Width="100%" PageSize="30" ControlToPage="gvOptOut"
                                    OnIndexChanged="EmailsPager_IndexChanged">
                                    <PagerStyle CssClass="tableContent"></PagerStyle>
                                </AU:PagerBuilder>
                                <asp:HiddenField ID="hfSortDirection" Value="Ascending" runat="server" />
                            </div>
                        </td>
                        <td style="width: 20%; border-top: 1px solid #808080;"></td>

                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlResults" runat="server" Width="250px" Height="150px" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2px" CssClass="popupbody">
                <table style="height: 100%; width: 100%;">
                    <tr>
                        <td style="text-align: center;">
                            <asp:Label ID="lblResults" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnResultsClose" runat="server" CssClass="formbutton" Text="Close" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button ID="hfResults" runat="server" style="display:none;" />
            <ajaxToolkit:ModalPopupExtender ID="mpeResults" BackgroundCssClass="modalBackground" PopupControlID="pnlResults" TargetControlID="hfResults" CancelControlID="btnResultsClose" runat="server"></ajaxToolkit:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    <KM:Message ID="kmMessage" runat="server" />
</asp:Content>

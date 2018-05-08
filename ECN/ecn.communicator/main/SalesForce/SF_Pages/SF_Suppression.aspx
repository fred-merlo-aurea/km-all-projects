<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="SF_Suppression.aspx.cs" Inherits="ecn.communicator.main.Salesforce.SF_Pages.SF_Suppression1" %>

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
                     document.getElementById("<% =divSuppression.ClientID %>").scrollTop;
        }
        function setScrollPosSF() {
            document.getElementById("<% =divSuppression.ClientID %>").scrollTop =
                     document.getElementById("<% =hfSFscrollPos.ClientID %>").value;
        }


    </script>
    <input type="hidden" id="hfSFscrollPos" name="SFscrollPos" value="0" runat="server" />
    <asp:Panel ID="pnlSuppression" Width="100%" runat="server">
        <table style="width:100%;padding-top:20px;">
            <tr>
                <td style="text-align:left;">
                    <asp:Label ID="lblHeader" Text="Sync Master Suppression" runat="server" CssClass="ECN-PageHeading" />
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top;width:30%;">

                    <table style="text-align: center;">
                        <tr>
                            <td style="background-color: #4b87bc; width: 20px;"></td>
                            <td style="text-align: left;">
                                <asp:Label ID="lbGreen" runat="server" Text=" = master suppressed in SF and ECN"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color: #939597; width: 20px;"></td>
                            <td style="text-align: left;">
                                <asp:Label ID="lbYellow" runat="server" Text=" = master suppressed in either SF or ECN"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color: #e6e7e8; width: 20px;"></td>
                            <td style="text-align: left;">
                                <asp:Label ID="lbOrange" runat="server" Text=" = not master suppressed "></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:40%;">
                    <table style="width:100%;border-left: 1px solid #808080; border-right: 1px solid #808080;">
                        <tr>
                            <td colspan="2">
                                <KM:Search ID="kmSearch" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btnGetQuery" runat="server" CssClass="formbutton" Text="SEARCH" OnClick="btnGetQuery_Click" />
                            </td>
                            <td style="text-align: center;">
                                <asp:Button ID="btnReset" runat="server" CssClass="formbutton" Text="RESET" OnClick="btnReset_Click" />
                            </td>
                        </tr>

                    </table>

                </td>
                <td style="width:30%; vertical-align:top;">
                    <table style="height:100%;">
                        <tr style="height:20%;">
                            <td>
                                <asp:Label ID="lblFilter" Text="Show emails that are:" runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFilter" Width="160px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                                    <asp:ListItem Text="All" Value="all" Selected="True" />
                                    <asp:ListItem Text="Suppressed in SF" Value="sf" />
                                    <asp:ListItem Text="Suppressed in ECN" Value="ecn" />
                                    <asp:ListItem Text="Suppressed in both" Value="both" />
                                    <asp:ListItem Text="Not suppressed" Value="none" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height:80%;">
                            <td colspan="2" style="vertical-align:middle;text-align:center;padding-top:15px;">
                                 <asp:Button ID="btnSyncSuppression" runat="server" Text="Sync All" OnClick="btnSyncSuppression_Click" CssClass="formbutton" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="5" style="text-align:left;">
                    <table style="border-bottom:1px solid #808080;border-top:1px solid #808080; width:100%;">
                        <tr>
                            <td class="label" valign="middle" align="left">Search By
                            </td>
                            <td class="label" valign="middle" align="left">Search Text
                            </td>
                            <td class="label" valign="middle" align="left">From
                            </td>
                            <td class="label" valign="middle" align="left">To
                            </td>
                            <td class="label" valign="middle" align="left">User
                            </td>
                            <td class="label" valign="middle" align="left">Campaign
                            </td>
                            <td class="label" valign="middle" align="left">Blast Type
                            </td>
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
                                    <asp:ListItem Value="BlastID">BlastID</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="label" valign="middle" align="left">
                                <asp:TextBox class="formtextfield" ID="txtSearch" runat="server"></asp:TextBox>
                            </td>
                            <td class="label" valign="middle" align="left">
                                <asp:TextBox class="formtextfield" runat="server" ID="txtFrom" Width="50px"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFrom"
                                    Format="MM/dd/yyyy">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td class="label" valign="middle" align="left">
                                <asp:TextBox class="formtextfield" runat="server" ID="txtTo" Width="50px"></asp:TextBox>
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
                            <td class="label" valign="middle" align="left">
                               Campaign Item
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlECNCampaign" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlECNCampaign_SelectedIndexChanged" />
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>

            <tr>
                <td  colspan="5" >
                    <table style="width:100%;">
                        <tr>

                            <td style="text-align:center;">
                                <asp:Button ID="btnSyncSelected" runat="server" Visible="false" Text="Sync Selected" OnClick="btnSyncSelected_Click" CssClass="formbutton" />
                            </td>
                        </tr>

                    </table>
                    
                </td>

            </tr>
            <tr><td colspan="5">
                <table style="width:100%; text-align:center;">
                    <tr>
                        <td style="width:25%;">

                        </td>
                        <td>
                            <div id="divSuppression" runat="server" onscroll="saveScrollPosSF()" style="overflow: auto; height: 500px;">
                        <asp:GridView ID="gvSuppression" runat="server" Visible="false" Width="98%" CssClass="km_grid" AllowSorting="true" OnSorting="gvSuppression_Sorting" AlternatingItemStyle-CssClass="km_gridAltColor" OnRowDataBound="gvSuppression_RowDataBound" DataKeyNames="SFId" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="SF Supp" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSFSupp" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Email" HeaderStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="ECN Supp" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkECNSupp" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                        </td>
                        <td style="width:25%;">

                        </td>
                    </tr>
                </table>
                    

                </td>

            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlResults" runat="server" Width="200px" Height="150px" CssClass="popupbody">
        <table style="height:100%; width:100%;">
            <tr>
                <td>
                    <asp:Label ID="lblREsults" Font-Bold="true" Text="Sync Results" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSFResults" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblECNResults" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="text-align:center;">
                    <asp:Button ID="btnResultsClose" runat="server" Text="Close" />
                </td>
            </tr>
        </table>

        </asp:Panel>
     <asp:Button ID="hfResults" runat="server" style="display:none;" />
            <ajaxToolkit:ModalPopupExtender ID="mpeResults" BackgroundCssClass="modalBackground" TargetControlID="hfResults" runat="server" CancelControlID="btnResultsClose" PopupControlID="pnlResults"></ajaxToolkit:ModalPopupExtender>
    <KM:Message ID="kmMessage" runat="server" />
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="SF_Accounts.aspx.cs" Inherits="ecn.communicator.main.Salesforce.SF_Pages.SF_Accounts" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            setScrollPosSF();
            setScrollPosECN();
        }
        function saveScrollPosSF() {
            document.getElementById("<% =hfSFscrollPos.ClientID %>").value =
                     document.getElementById("<% =divSFAccounts.ClientID %>").scrollTop;
        }
        function setScrollPosSF() {
            document.getElementById("<% =divSFAccounts.ClientID %>").scrollTop =
                     document.getElementById("<% =hfSFscrollPos.ClientID %>").value;
        }

        function saveScrollPosECN() {
            document.getElementById("<% =hfECNscrollPos.ClientID %>").value =
                      document.getElementById("<% =divECNAccounts.ClientID %>").scrollTop;
        }
        function setScrollPosECN() {
            document.getElementById("<% =divECNAccounts.ClientID %>").scrollTop =
                     document.getElementById("<% =hfECNscrollPos.ClientID %>").value;
        }
    </script>
    <KM:Message ID="kmMsg" runat="server" />
    <asp:UpdatePanel ID="updateAccounts" runat="server">
        <ContentTemplate>
            <input type="hidden" id="hfSFscrollPos" name="SFscrollPos" value="0" runat="server" />
            <input type="hidden" id="hfECNscrollPos" name="ECNscrollPos" value="0" runat="server" />
            <asp:Panel ID="pnlGrids" runat="server" Visible="false">
                <table style="width: 100%; margin-right: 10px; padding-top: 20px;">
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="lblHeader" Text="Sync Accounts" CssClass="ECN-PageHeading" runat="server" />
                        </td>
                    </tr>
                    <tr id="trLegend">
                        <td style="text-align: left; width: 30%; vertical-align: top;">
                            <table style="text-align: center;">
                                <tr>
                                    <td style="background-color: #4b87bc; width: 20px;"></td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbGreen" runat="server" Text=" = account is in SF and ECN and data is the same" CssClass="label_gray"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #939597; width: 20px;"></td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbYellow" runat="server" Text=" = account is in SF and ECN but data is different" CssClass="label_gray"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #e6e7e8; width: 20px;"></td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbOrange" runat="server" Text=" = account is ONLY in SF or ECN" CssClass="label_gray"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: left; width: 35%; vertical-align: top;" rowspan="2">
                            <asp:UpdatePanel ID="upKMSearch" runat="server" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSearch" />
                                    <asp:PostBackTrigger ControlID="btnSearchReset" />
                                </Triggers>
                                <ContentTemplate>
                                    <table style="border-left: 1px solid #808080; border-right: 1px solid #808080;">
                                        <tr>
                                            <td colspan="2">
                                                <KM:Search ID="kmSearch" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:Button ID="btnSearch" runat="server" Text="SEARCH" CausesValidation="false" OnClick="btnSearch_Click" CssClass="formbutton" />
                                            </td>
                                            <td style="text-align: center;">
                                                <asp:Button ID="btnSearchReset" runat="server" Text="RESET" CausesValidation="false" OnClick="btnSearchReset_Click" CssClass="formbutton" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>

                        <td style="text-align: left; width: 35%; vertical-align: bottom;">
                            <asp:UpdatePanel ID="upFilter" runat="server" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="ddlFilter" />
                                    <asp:PostBackTrigger ControlID="ddlBaseChannel" />
                                    <asp:PostBackTrigger ControlID="ddlProductLine" />
                                </Triggers>
                                <ContentTemplate>
                                    <table>

                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBaseChannel" runat="server" Text="Base Channel" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBaseChannel" Width="200px" runat="server" OnSelectedIndexChanged="ddlBaseChannel_SelectedIndexChanged" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblProductLine" runat="server" Text="Product Line" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlProductLine" Width="200px" OnSelectedIndexChanged="ddlProductLine_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFilter" runat="server" Text="Show Accounts that are:" CssClass="label_gray_bold" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlFilter" runat="server" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged" AutoPostBack="true">
                                                    
                                                    <asp:ListItem Text="Show All" Value="All" Selected="True" />
                                                    <asp:ListItem Text="Only in SF accounts" Value="OnlySF" />
                                                    <asp:ListItem Text="Only in ECN accounts" Value="OnlyECN" />
                                                    <asp:ListItem Text="In both but diff data" Value="DiffData" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; border-top: 1px solid #808080; padding-top: 5px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblSFAccounts" Text="Salesforce" Font-Size="Large" runat="server" />
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="lblECNAccounts" Text="ECN" Font-Size="Large" runat="server" />
                        </td>
                    </tr>
                    <tr id="trGrids">
                        <td id="tcSF" style="width: 45%; text-align: left;">
                            <div id="divSFAccounts" runat="server" style="overflow: auto; height: 500px;">
                                <asp:GridView ID="gvSFAccounts" runat="server" DataKeyNames="Id" AutoGenerateColumns="false" AllowSorting="true"
                                    CssClass="km_grid" AlternatingItemStyle-CssClass="km_gridAltColor" SelectedItemStyle-BackColor="#3399ff"
                                    OnRowDataBound="gvSFAccounts_RowDataBound" OnSorting="gvSFAccounts_Sorting">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="cbSFselectALL" runat="server" AutoPostBack="true" CausesValidation="false"
                                                    OnCheckedChanged="cbSFselectALL_CheckedChanged" ToolTip="Select All" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbSFselect" runat="server" AutoPostBack="true" Text="" CausesValidation="false" OnCheckedChanged="cbSFselect_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Name" HeaderText="Account" SortExpression="Name" />
                                        <asp:BoundField DataField="BillingCity" HeaderText="City" SortExpression="BillingCity" />
                                        <asp:BoundField DataField="BillingState" HeaderText="State" SortExpression="BillingState" />
                                        <asp:BoundField DataField="BillingPostalCode" HeaderText="Zip" SortExpression="BillingPostalCode" />
                                        <asp:BoundField DataField="Industry" HeaderText="Industry" SortExpression="Industry" />
                                        <asp:BoundField DataField="AnnualRevenue" HeaderText="$$" SortExpression="AnnualRevenue" />
                                    </Columns>
                                </asp:GridView>
                                <asp:HiddenField ID="hfSF_SortDirection" runat="server" Value="Ascending" />
                            </div>
                        </td>
                        <td id="tcOptions" style="vertical-align: top; text-align: center; width: 5%">
                            <asp:Panel ID="pnlOptions" runat="server">
                                <table style="width: 100%">
                                    
                                    <tr>
                                        <td style="text-align: center">
                                            <!--Move record to SF < -->
                                            <asp:ImageButton ID="imgbtnECNToSF" runat="server" ToolTip="Insert account in SF" Visible="false" Width="30px" Height="30px" ImageUrl="http://images.ecn5.com/images/ArrowLeft_Orange.png" OnClick="imgbtnECNToSF_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center">
                                            <!--Look at fields that are different -->
                                            <asp:ImageButton ID="imgbtnCompare" runat="server" ToolTip="Compare data" Visible="false" Width="30px" Height="30px" ImageUrl="http://images.ecn5.com/images/SearchButton.jpg" OnClick="imgbtnCompare_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td id="tcECN" style="width: 45%; text-align: left;">
                            <div id="divECNAccounts" runat="server" style="overflow: auto; height: 500px;">
                                <asp:GridView ID="gvECNAccounts" runat="server" DataKeyNames="CustomerId" AutoGenerateColumns="false" AllowSorting="true"
                                    CssClass="km_grid" AlternatingItemStyle-CssClass="km_gridAltColor" SelectedItemStyle-BackColor="#4b87bc"
                                    OnRowDataBound="gvECNAccounts_RowDataBound" OnSorting="gvECNAccounts_Sorting">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="cbECNselectALL" runat="server" AutoPostBack="true" CausesValidation="false"
                                                    OnCheckedChanged="cbECNselectALL_CheckedChanged" ToolTip="Select All" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbECNselect" runat="server" AutoPostBack="true" Text="" CausesValidation="false" OnCheckedChanged="cbECNselect_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName" />
                                        <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
                                        <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
                                        <asp:BoundField DataField="Zip" HeaderText="Zip" SortExpression="Zip" />
                                    </Columns>
                                </asp:GridView>
                                <asp:HiddenField ID="hfECN_SortDirection" runat="server" Value="Ascending" />
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="pnlCompare" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2px" runat="server" Width="800px" Height="400px" CssClass="popupbody">
                <div>
                    <table style="width: 100%; border-collapse: collapse;">
                        <tr style="background-image: url(~/images/Gradient_DarkBlue.jpg);">
                            <td style="text-align: left; padding-left: 20px;">
                                <div>
                                    <h2>
                                        <asp:Label ID="lbSFInfo" Text="SF Data" runat="server" />
                                    </h2>
                                </div>
                            </td>
                            <td style="text-align: right; padding-right: 20px;">
                                <div>
                                    <h2>
                                        <asp:Label ID="lbECNInfo" Text="ECN Data" runat="server" />
                                    </h2>
                                </div>
                            </td>
                        </tr>
                        <tr style="border-top-color: orange; border-top-style: solid; border-top-width: 5px;">
                            <td colspan="2">
                                <table style="padding-top: 5px; background-color: #e6e7e8; width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSFName" runat="server" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblName" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblECNName" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSFAddress" runat="server" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblAddress" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblECNAddress" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSFCity" runat="server" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblCity" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblECNCity" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSFState" runat="server" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblState" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblECNState" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSFZip" runat="server" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblZip" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblECNZip" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSFCountry" runat="server" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblCountry" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblECNCountry" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSFPhone" runat="server" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblPhone" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblECNPhone" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSFFax" runat="server" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblFax" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblECNFax" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="padding-top: 15px; padding-left: 15px; padding-bottom: 15px; text-align: center;">
                                            <asp:HiddenField ID="hfECNCustomerID" runat="server" />
                                            <asp:HiddenField ID="hfSFAccountID" runat="server" />
                                            <asp:Button ID="btnSyncData" runat="server" Text="Sync Data" CausesValidation="false" OnClick="btnSyncData_Click" CssClass="formbutton" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnCompareClose" Text="Close" runat="server" CssClass="formbutton" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:Button ID="hfCompare" runat="server" style="display:none;" />
            <ajaxToolkit:ModalPopupExtender ID="mpeCompare" BackgroundCssClass="modalBackground" PopupControlID="pnlCompare" TargetControlID="hfCompare" CancelControlID="btnCompareClose" runat="server"></ajaxToolkit:ModalPopupExtender>

            <asp:Panel ID="pnlResults" Width="200px" Height="100px" runat="server" CssClass="popupbody">
                <table style="height: 100%; width: 100%;">
                    <tr>
                        <td style="text-align: center;">
                            <asp:Label ID="lblMessage" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnResultsClose" Text="Close" CssClass="formbutton" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button ID="hfResults" runat="server" style="display:none;"/>
            <ajaxToolkit:ModalPopupExtender ID="mpeResults" BackgroundCssClass="modalBackground" TargetControlID="hfResults" runat="server" CancelControlID="btnResultsClose" PopupControlID="pnlResults"></ajaxToolkit:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

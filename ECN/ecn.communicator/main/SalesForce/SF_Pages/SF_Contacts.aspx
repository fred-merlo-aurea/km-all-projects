<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="SF_Contacts.aspx.cs" Inherits="ecn.communicator.main.Salesforce.SF_Pages.SF_Contacts" %>

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
                     document.getElementById("<% =divSFContacts.ClientID %>").scrollTop;
        }
        function setScrollPosSF() {
            document.getElementById("<% =divSFContacts.ClientID %>").scrollTop =
                     document.getElementById("<% =hfSFscrollPos.ClientID %>").value;
        }

        function saveScrollPosECN() {
            document.getElementById("<% =hfECNscrollPos.ClientID %>").value =
                      document.getElementById("<% =divECNContacts.ClientID %>").scrollTop;
        }
        function setScrollPosECN() {
            document.getElementById("<% =divECNContacts.ClientID %>").scrollTop =
                     document.getElementById("<% =hfECNscrollPos.ClientID %>").value;
        }
    </script>
    <KM:Message ID="kmMsg" runat="server" />
    <asp:UpdatePanel ID="upContacts" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <input type="hidden" id="hfSFscrollPos" name="SFscrollPos" value="0" runat="server" />
            <input type="hidden" id="hfECNscrollPos" name="ECNscrollPos" value="0" runat="server" />
            <asp:Panel ID="pnlContact" runat="server">
                <table style="padding-top: 20px;width:100%;">
                    <tr>
                        <td style="text-align:left;">
                            <asp:Label ID="lblHeader" Text="Sync Contacts" runat="server" CssClass="ECN-PageHeading" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; vertical-align: top;">
                            <table style="text-align: center;">
                                <tr>
                                    <td style="background-color: #4b87bc; width: 20px;"></td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbGreen" runat="server" Text=" = contact is in SF and ECN and data is the same"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #939597; width: 20px;"></td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbYellow" runat="server" Text=" = contact is in SF and ECN but data is different"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #e6e7e8; width: 20px;"></td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbOrange" runat="server" Text=" = contact is ONLY in SF or ECN"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            
                        </td>
                        <td style="vertical-align: top;">
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
                        <td style="vertical-align: top;">
                            <asp:UpdatePanel ID="upECNGroup" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="rblECNGroup" />
                                    <asp:PostBackTrigger ControlID="ddlECNGroup" />
                                    <asp:PostBackTrigger ControlID="ddlECNFolder" />
                                </Triggers>
                                <ContentTemplate>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rblECNGroup" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblECNGroup_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                    <asp:ListItem Text="Existing" Selected="true" Value="existing" />
                                                    <asp:ListItem Text="New" Value="new" />
                                                </asp:RadioButtonList>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFolder" runat="server" Text="Folder:" />

                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlECNFolder" Style="max-width: 200px;" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlECNFolder_SelectedIndexChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblECNGroup" Text="Group:" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlECNGroup" Style="max-width: 200px;" OnSelectedIndexChanged="ddlECNGroup_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                                                <asp:TextBox ID="txtECNGroup" Visible="false" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <br />
                            <asp:UpdatePanel ID="upFilter" runat="server" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="ddlFilter" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Label ID="lblFilter" runat="server" Text="Show contacts that are:" />
                                    <asp:DropDownList ID="ddlFilter" runat="server" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged" AutoPostBack="true">
                                        
                                        <asp:ListItem Text="Show All" Value="All" Selected="True" />
                                        <asp:ListItem Text="Only in SF contacts" Value="OnlySF" />
                                        <asp:ListItem Text="Only in ECN contacts" Value="OnlyECN" />
                                        <asp:ListItem Text="In both but diff data" Value="DiffData" />
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table style="border-top: 1px solid #808080; width: 100%; padding-top: 5px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblSFContacts" Visible="false" Font-Size="Large" Text="Salesforce" runat="server" />
                        </td>
                        <td />
                        <td>
                            <asp:Label ID="lblECNContacts" Visible="false" Font-Size="Large" Text="ECN" runat="server" />
                        </td>
                    </tr>
                    <tr>

                        <td style="text-align: center; width: 45%;">

                            <div id="divSFContacts" runat="server" style="overflow: auto; height: 500px;" onscroll="saveScrollPosSF()">
                                <asp:UpdatePanel ID="upSFContacts" runat="server" ChildrenAsTriggers="true">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="gvSFContacts" />
                                    </Triggers>
                                    <ContentTemplate>

                                        <asp:GridView ID="gvSFContacts" AllowSorting="true" CssClass="km_grid" OnRowDataBound="dgSFContacts_RowDataBound"
                                            AlternatingItemStyle-CssClass="km_gridAltColor" RowStyle-Height="25" Width="95%" SelectedItemStyle-BackColor="#3399ff" DataKeyNames="Id" AutoGenerateColumns="false" OnSorting="gvSFContacts_Sorting" runat="server">

                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSFSelectAll" OnCheckedChanged="chkSFSelectAll_CheckedChanged" AutoPostBack="true" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSFSelect" OnCheckedChanged="chkSFSelect_CheckedChanged" AutoPostBack="true" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Email" ItemStyle-Wrap="true" SortExpression="Email" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" DataField="Email" />
                                                <asp:BoundField HeaderText="First Name" SortExpression="FirstName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" DataField="FirstName" />
                                                <asp:BoundField HeaderText="Last Name" SortExpression="LastName" DataField="LastName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="State" SortExpression="MailingState" DataField="MailingState" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                                <asp:TemplateField HeaderText="Owner" SortExpression="OwnerId" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSFOwner" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td style="width: 10%; vertical-align: top;">
                            <asp:UpdatePanel ID="pnlOptions" UpdateMode="Conditional" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="imgbtnSFToECN" />
                                    <asp:PostBackTrigger ControlID="imgbtnECNToSF" />
                                    <asp:PostBackTrigger ControlID="imgbtnCompare" />
                                </Triggers>
                                <ContentTemplate>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="text-align: center;">
                                                <!--Move record to ECN > -->
                                                
                                                <asp:ImageButton ID="imgbtnSFToECN" ToolTip="Insert contact in ECN" runat="server" Visible="false" Width="30px" Height="30px" ImageUrl="http://images.ecn5.com/images/ArrowRight_Orange.png" OnClick="imgbtnSFToECN_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <!--Move record to SF < -->
                                                <asp:ImageButton ID="imgbtnECNToSF" runat="server" ToolTip="Insert contact in SF" Visible="false" Width="30px" Height="30px" ImageUrl="http://images.ecn5.com/images/ArrowLeft_Orange.png" OnClick="imgbtnECNToSF_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <!--Look at fields that are different -->
                                                <asp:ImageButton ID="imgbtnCompare" runat="server" ToolTip="Compare data" Visible="false" Width="30px" Height="30px" ImageUrl="http://images.ecn5.com/images/SearchButton.jpg" OnClick="imgbtnCompare_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="text-align: center; width: 45%;">
                            <div id="divECNContacts" runat="server" style="overflow: auto; height: 500px;" onscroll="saveScrollPosECN()">
                                <asp:UpdatePanel ID="upECNContacts" runat="server" ChildrenAsTriggers="true">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="gvECNContacts" />
                                    </Triggers>
                                    <ContentTemplate>

                                        <asp:GridView ID="gvECNContacts" AllowSorting="true" CssClass="km_grid" OnSorting="gvECNContacts_Sorting"
                                            AlternatingItemStyle-CssClass="km_gridAltColor" RowStyle-Height="25" SelectedItemStyle-BackColor="#3399ff" DataKeyNames="EmailId" AutoGenerateColumns="false" OnRowDataBound="dgECNContacts_RowDataBound" runat="server">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkECNSelectAll" OnCheckedChanged="chkECNSelectAll_CheckedChanged" AutoPostBack="true" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkECNSelect" OnCheckedChanged="chkECNSelect_CheckedChanged" AutoPostBack="true" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Email" SortExpression="Email" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" DataField="EmailAddress" />
                                                <asp:BoundField HeaderText="First Name" SortExpression="FirstName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" DataField="FirstName" />
                                                <asp:BoundField HeaderText="Last Name" SortExpression="LastName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" DataField="LastName" />
                                                <asp:BoundField HeaderText="State" SortExpression="State" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" DataField="State" />
                                            </Columns>
                                        </asp:GridView>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                        </td>
                    </tr>

                </table>
            </asp:Panel>
            <asp:Panel ID="pnlCompare" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2px" runat="server" Width="800px" CssClass="popupbody">
                <div style="text-align: center;">
                    <table style="width: 100%; height: 100%; border-collapse: collapse;">
                        <tr style="background-image: url(http://images.ecn5.com/images/Gradient_DarkBlue.jpg); height: 10%;">
                            <td id="tdSFInfo" runat="server" style="text-align: left; padding-left: 20px;">
                                <asp:Label ID="lbSFInfo" Font-Bold="true" ForeColor="White" Font-Size="Large" Text="SF Data" runat="server" />

                            </td>

                            <td style="text-align: right; padding-right: 20px;">
                                <asp:Label ID="Label3" Font-Bold="true" ForeColor="White" Font-Size="Large" Text="ECN Data" runat="server" />
                            </td>
                        </tr>
                        <tr style="border-top-color: orange; border-top-style: solid; border-top-width: 5px; height: 90%;">
                            <td colspan="2">
                                <table style="padding-top: 5px; background-color: #e6e7e8; border-top-width: 5px; width: 100%; height: 100%;">
                                    <tr>
                                        <td style="width:10%; text-align:left;">
                                            <asp:Label ID="lblFirstName" Text="First Name:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblSFFirstName" runat="server" />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:RadioButtonList ID="rblFirstName" CellSpacing="0" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblECNFirstName" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:10%; text-align:left;">
                                            <asp:Label ID="Label2" Text="Last Name:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">

                                            <asp:Label ID="lblSFLastName" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:RadioButtonList ID="rblLastName" CellSpacing="0" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblECNLastName" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:10%; text-align:left;">
                                            <asp:Label ID="Label5" Text="Email:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">

                                            <asp:Label ID="lblSFEmail" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:RadioButtonList ID="rblEmail" CellSpacing="0" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblECNEmail" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:10%; text-align:left;">
                                            <asp:Label ID="Label7" Text="Address:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">

                                            <asp:Label ID="lblSFAddress" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:RadioButtonList ID="rblAddress" CellSpacing="0" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblECNAddress" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:10%; text-align:left;">
                                            <asp:Label ID="Label9" Text="City:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">

                                            <asp:Label ID="lblSFCity" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:RadioButtonList ID="rblCity" CellSpacing="0" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblECNCity" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:10%; text-align:left;">
                                            <asp:Label ID="Label11" Text="State:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">

                                            <asp:Label ID="lblSFState" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:RadioButtonList ID="rblState" CellSpacing="0" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblECNState" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:10%; text-align:left;">
                                            <asp:Label ID="Label13" Text="Zip:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">

                                            <asp:Label ID="lblSFZip" runat="server" />

                                        </td>
                                        <td style="text-align: center;">
                                            <asp:RadioButtonList ID="rblZip" CellSpacing="0" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblECNZip" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:10%; text-align:left;">
                                            <asp:Label ID="Label15" Text="Phone:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">

                                            <asp:Label ID="lblSFPhone" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:RadioButtonList ID="rblPhone" CellSpacing="0" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblECNPhone" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:10%; text-align:left;">
                                            <asp:Label ID="Label17" Text="Mobile Phone:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">

                                            <asp:Label ID="lblSFCellPhone" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:RadioButtonList ID="rblCellPhone" CellSpacing="0" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF"></asp:ListItem>
                                                <asp:ListItem Text="Take ECN" Value="ECN"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblECNCellPhone" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding: 15px; text-align: center">
                                            <asp:HiddenField ID="hfECNContactID" runat="server" />
                                            <asp:HiddenField ID="hfSFLeadID" runat="server" />
                                            <asp:Button ID="btnSyncData" runat="server" CausesValidation="false" CssClass="formbutton" OnClick="btnSyncData_Click" Text="Sync Data" />
                                        </td>
                                        <td colspan="2" style="padding: 15px; text-align: center">
                                            <asp:Button ID="btnCompareClose" runat="server" CssClass="formbutton" Text="Close" />
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

            <asp:Panel ID="pnlResults" runat="server" CssClass="popupbody">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="MessageLabel" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataGrid ID="ResultsGrid" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True"
                                AutoGenerateColumns="False" Width="400px">
                                <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="Action" HeaderText="Description" ItemStyle-Width="75%"
                                        HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Totals" HeaderText="Totals" ItemStyle-Width="25%" HeaderStyle-Width="25%"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnResultsClose" Text="Close" CssClass="formbutton" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button ID="hfResults" runat="server" style="display:none;" />
            <ajaxToolkit:ModalPopupExtender ID="mpeResults" BackgroundCssClass="modalBackground" TargetControlID="hfResults" runat="server" CancelControlID="btnResultsClose" PopupControlID="pnlResults"></ajaxToolkit:ModalPopupExtender>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

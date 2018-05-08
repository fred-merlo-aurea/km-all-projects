<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="SF_Leads.aspx.cs" Inherits="ecn.communicator.main.Salesforce.SF_Pages.SF_Leads" %>

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
                     document.getElementById("<% =divSFLeads.ClientID %>").scrollTop;
        }
        function setScrollPosSF() {
            var div = document.getElementById("<% =divSFLeads.ClientID %>");
            if (div) {
                document.getElementById("<% =divSFLeads.ClientID %>").scrollTop =
                         document.getElementById("<% =hfSFscrollPos.ClientID %>").value;
            }
        }

        function saveScrollPosECN() {
            document.getElementById("<% =hfECNscrollPos.ClientID %>").value =
                      document.getElementById("<% =divECNLeads.ClientID %>").scrollTop;
        }
        function setScrollPosECN() {
            var div = document.getElementById("<% =divECNLeads.ClientID %>");
            if (div) {
                document.getElementById("<% =divECNLeads.ClientID %>").scrollTop =
                         document.getElementById("<% =hfECNscrollPos.ClientID %>").value;
            }
        }
    </script>
    <KM:Message ID="kmMsg" runat="server" />
    <asp:UpdatePanel ID="upLeads" runat="server" UpdateMode="Always">
        <ContentTemplate>


            <input type="hidden" id="hfSFscrollPos" name="SFscrollPos" value="0" runat="server" />
            <input type="hidden" id="hfECNscrollPos" name="ECNscrollPos" value="0" runat="server" />
            <asp:Panel ID="pnlLeads" runat="server">
                <table style="border-bottom: 1px solid #808080; width: 100%; padding-top: 20px;">
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="lblHeader" Text="Sync Leads" runat="server" CssClass="ECN-PageHeading" />
                        </td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td style="text-align: left;">

                            <table style="text-align: center;">
                                <tr>
                                    <td style="background-color: #4b87bc; width: 20px;"></td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbGreen" runat="server" Text=" = lead is in SF and ECN and data is the same"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #939597; width: 20px;"></td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbYellow" runat="server" Text=" = lead is in SF and ECN but data is different"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #e6e7e8; width: 20px;"></td>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lbOrange" runat="server" Text=" = lead is ONLY in SF or ECN"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>

                        <td style="text-align: left; vertical-align: top;" rowspan="2">
                            <asp:UpdatePanel ID="upKMSearch" runat="server" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnGetQuery" />
                                    <asp:PostBackTrigger ControlID="btnResetSearch" />
                                    <asp:PostBackTrigger ControlID="ddlSFTags" />
                                </Triggers>
                                <ContentTemplate>
                                    <table style="border-left: 1px solid #808080; border-right: 1px solid #808080;">
                                        <tr>
                                            <td style="vertical-align: middle; padding-bottom: 15px;">
                                                <asp:Label ID="lblFilterTags" runat="server" Text="Filter by Tag" />

                                                <asp:DropDownList ID="ddlSFTags" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSFTags_SelectedIndexChanged" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <KM:Search ID="kmSearch" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:Button ID="btnGetQuery" Text="SEARCH" CausesValidation="false" CssClass="formbutton" runat="server" OnClick="btnGetQuery_Click" />
                                            </td>
                                            <td style="text-align: center;">
                                                <asp:Button ID="btnResetSearch" Text="RESET" CausesValidation="false" CssClass="formbutton" runat="server" OnClick="btnResetSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <%--                            <GS:BoxPanel ID="bpSearch" runat="server" Title="SF SEARCH" HorizontalAlign="Left" ScrollBars="Vertical" Height="150px">
                                <asp:GridView ID="gvSearchBy" runat="server" AutoGenerateColumns="false" AllowSorting="false"
                                    CssClass="km_grid" AlternatingItemStyle-CssClass="km_gridAltColor" SelectedItemStyle-BackColor="#3399ff">
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbSearchBy" runat="server" AutoPostBack="false" Text="" CausesValidation="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Name" HeaderText="Search By" />
                                        <asp:TemplateField HeaderText="Value">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbSearchByValue" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="false" OnClick="btnSearch_Click" CssClass="grayGradientButton" />
                                <asp:Button ID="btnSearchReset" runat="server" Text="Reset" CausesValidation="false" OnClick="btnSearchReset_Click" CssClass="grayGradientButton" />
                            </GS:BoxPanel>--%>
                            
                        </td>
                        <td style="vertical-align: top;" rowspan="2">

                            <asp:UpdatePanel ID="upECNGroups" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="rblECNGroup" />
                                    <asp:PostBackTrigger ControlID="ddlECNGroup" />
                                    <asp:PostBackTrigger ControlID="ddlECNFolder" />
                                    <asp:PostBackTrigger ControlID="ddlFilter" />
                                </Triggers>
                                <ContentTemplate>
                                    <table style="width:100%;">
                                        <tr>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rblECNGroup" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblECNGroup_SelectedIndexChanged" runat="server" AutoPostBack="true">
                                                    <asp:ListItem Text="Existing" Selected="True" Value="existing" />
                                                    <asp:ListItem Text="New" Value="new" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td >
                                                <asp:Label ID="lblECNFolder" Text="Folder:" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlECNFolder" Style="max-width: 200px;" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlECNFolder_SelectedIndexChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblECNGroup" runat="server" Text="Group:" />
                                            </td>
                                            <td >

                                                <asp:DropDownList ID="ddlECNGroup" Style="max-width: 200px;" OnSelectedIndexChanged="ddlECNGroup_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                                                <asp:TextBox ID="txtNewGroup" Visible="false" runat="server" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding-top: 30px;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Text="Show leads that are:" />
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:DropDownList ID="ddlFilter" runat="server" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Text="Show All" Value="All" Selected="True" />
                                                                <asp:ListItem Text="Only in SF leads" Value="OnlySF" />
                                                                <asp:ListItem Text="Only in ECN leads" Value="OnlyECN" />
                                                                <asp:ListItem Text="In both but diff data" Value="DiffData" />
                                                            </asp:DropDownList>
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
                <table style="padding-top: 5px; width:100%;">
                    <tr>
                        <td>
                            <asp:Label ID="lblSFLeads" Text="Salesforce" Font-Size="Large" Visible="false" runat="server" />
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="lblECNLeads" Text="ECN" Font-Size="Large" Visible="false" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:45%;text-align:center;">
                            <div id="divSFLeads" runat="server" visible="false" style="overflow: auto; height: 500px;" onscroll="saveScrollPosSF()">
                                <asp:UpdatePanel ID="upSFLeads" runat="server" ChildrenAsTriggers="true">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="dgSFLeads" />
                                    </Triggers>
                                    <ContentTemplate>

                                        <asp:GridView ID="dgSFLeads" CssClass="km_grid" OnRowDataBound="dgSFLeads_RowDataBound" AutoGenerateColumns="false" AllowSorting="true"
                                            AlternatingItemStyle-CssClass="km_gridAltColor" RowStyle-Height="25" SelectedItemStyle-BackColor="#3399ff" Width="93%" OnSorting="dgSFLeads_Sorting" DataKeyNames="Id" runat="server">

                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSFSelectAll" OnCheckedChanged="chkSFSelectAll_CheckedChanged" AutoPostBack="true" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSFSelect" OnCheckedChanged="chkSFSelectRow_CheckedChanged" AutoPostBack="true" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Email" />
                                                <asp:BoundField SortExpression="FirstName" DataField="FirstName" HeaderText="FirstName" />
                                                <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" />
                                                <asp:BoundField DataField="Company" HeaderText="Company" SortExpression="Company" />
                                                <asp:TemplateField HeaderText="Owner" SortExpression="OwnerId" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSFLeadOwner" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>


                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                        </td>
                        <td style="width: 10%; vertical-align: top; text-align: center;">
                            <asp:UpdatePanel ID="pnlOptions" ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server">
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
                                                <asp:ImageButton ID="imgbtnSFToECN" ToolTip="Insert Lead in ECN" runat="server" Visible="false" Width="30px" Height="30px" ImageUrl="http://images.ecn5.com/images/ArrowRight_Orange.png" OnClick="imgbtnSFToECN_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <!--Move record to SF < -->
                                                <asp:ImageButton ID="imgbtnECNToSF" runat="server" ToolTip="Insert Lead in SF" Visible="false" Width="30px" Height="30px" ImageUrl="http://images.ecn5.com/images/ArrowLeft_Orange.png" OnClick="imgbtnECNToSF_Click" />
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
                        <td style="width: 45%;">
                            <div id="divECNLeads" runat="server" visible="false" style="overflow: auto; height: 500px;" onscroll="saveScrollPosECN()">
                                <asp:UpdatePanel ID="upECNLeads" runat="server" ChildrenAsTriggers="true">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="dgECNLeads" />
                                    </Triggers>
                                    <ContentTemplate>

                                        <asp:GridView ID="dgECNLeads" CssClass="km_grid" OnRowDataBound="dgECNLeads_RowDataBound" AutoGenerateColumns="false" Width="93%"
                                            AlternatingItemStyle-CssClass="gridAltColor" RowStyle-Height="25" SelectedItemStyle-BackColor="#3399ff" AllowSorting="true" OnSorting="dgECNLeads_Sorting" DataKeyNames="EmailId" runat="server">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkECNSelectAll" OnCheckedChanged="chkECNSelectAll_CheckedChanged" AutoPostBack="true" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkECNSelect" OnCheckedChanged="chkECNSelectRow_CheckedChanged" AutoPostBack="true" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="EmailAddress" HeaderText="Email" SortExpression="Email" />
                                                <asp:BoundField SortExpression="FirstName" DataField="FirstName" HeaderText="FirstName" />
                                                <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" />

                                            </Columns>
                                        </asp:GridView>


                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>

                </table>
            </asp:Panel>
            <asp:Panel ID="pnlCompare" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2px" runat="server" Width="800px" Height="400px" CssClass="popupbody">
                <div style="height: 100%;">
                    <table style="width: 100%; height: 100%; border-collapse: collapse;">
                        <tr style="height: 10%; background-image: url(http://images.ecn5.com/images/Gradient_DarkBlue.jpg);">

                            <td style="text-align: left; padding-left: 20px;">
                                <asp:Label ID="lbSFInfo" Font-Bold="true" ForeColor="White" Font-Size="Large" Text="SF Data" runat="server" />
                            </td>

                            <td style="text-align: right; padding-right: 20px;">
                                <asp:Label ID="Label3" Font-Bold="true" ForeColor="White" Font-Size="Large" Text="ECN Data" runat="server" />
                            </td>

                        </tr>
                        <tr style="height: 90%; border-top-color: orange; border-top-style: solid; border-top-width: 5px;">
                            <td colspan="2">
                                <table style="padding-top: 5px; background-color: #e6e7e8; width: 100%; height: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFirstName" Text="First Name:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblSFFirstName" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
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
                                        <td>
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
                                        <td>
                                            <asp:Label ID="Label4" Text="Email:" runat="server" />
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
                                        <td>
                                            <asp:Label ID="Label5" Text="Address:" runat="server" />
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
                                        <td>
                                            <asp:Label ID="Label6" Text="City:" runat="server" />
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
                                        <td>
                                            <asp:Label ID="Label7" Text="State:" runat="server" />
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
                                        <td>
                                            <asp:Label ID="Label8" Text="Zip:" runat="server" />
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
                                        <td>
                                            <asp:Label ID="label" Text="Country:" runat="server" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblSFCountry" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:RadioButtonList ID="rblCountry" CellSpacing="0" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Take SF" Value="SF" />
                                                <asp:ListItem Text="Take ECN" Value="ECN" />
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="lblECNCountry" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label9" Text="Phone:" runat="server" />
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
                                        <td>
                                            <asp:Label ID="Label10" Text="Mobile Phone:" runat="server" />
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

                                        <td colspan="2" style="padding-top: 15px; text-align: center">
                                            <asp:HiddenField ID="hfECNCustomerID" runat="server" />
                                            <asp:HiddenField ID="hfSFAccountID" runat="server" />
                                            <asp:Button ID="btnSyncData" runat="server" Text="Sync Data" CssClass="formbutton" CausesValidation="false" OnClick="btnSyncData_Click" />
                                        </td>

                                        <td colspan="2" style="padding-top: 15px; text-align: center">
                                            <asp:Button ID="btnCompareClose" CssClass="formbutton" Text="Close" runat="server" />
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
                        <td style="text-align: center;">
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

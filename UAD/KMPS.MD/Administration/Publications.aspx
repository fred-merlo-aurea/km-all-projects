<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true"
    CodeBehind="Publications.aspx.cs" Inherits="KMPS.MDAdmin.WebForm1" %>

<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">

        function ValidateDelete() {
            if (!confirm('Are you sure you want to delete Product. It will delete Responsegroup, Codesheet and all mapping for that product?')) return false;

            return true;
        }

    </script>


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="Images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divMessage" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="600" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img2" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                            alt="" />
                                    </td>
                                    <td valign="middle" align="left" width="80%" height="100%">
                                        <asp:Label ID="lblMessage" runat="Server"></asp:Label>
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
            </div>
            <asp:TextBox ID="txtSearch" runat="server" Width="250px"></asp:TextBox>&nbsp;&nbsp;
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click"  />&nbsp;
            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="button" OnClick="btnReset_Click" /><br /><br />            
            <asp:GridView ID="gvPub" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" OnSorting="gvPub_Sorting"
                DataKeyNames="PubID" OnRowDeleting="gvPub_RowDeleting" OnRowCommand="gvPub_RowCommand" OnPageIndexChanging="gvPub_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="PubName" HeaderText="Name" SortExpression="PubName">
                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PubCode" HeaderText="Code" SortExpression="PubCode">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SortOrder" HeaderText="SortOrder" SortExpression="SortOrder" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PubTypeDisplayName" HeaderText="Type" SortExpression="PubTypeDisplayName">
                        <HeaderStyle HorizontalAlign="Left" Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Score" HeaderText="Score" SortExpression="Score" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Enable Searching" SortExpression="EnableSearching"
                        ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# (Boolean.Parse(Eval("EnableSearching").ToString())) ? "Yes" : "No"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                        FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("PubID")%>'
                                OnCommand="lnkEdit_Command"><img src="Images/ic-edit.gif" alt="" style="border:none;" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                        FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("PubID")%>' OnClientClick="return ValidateDelete();"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:HyperLinkField Text="Responses" DataNavigateUrlFormatString="CodeSheet.aspx?PubID={0}"
                        DataNavigateUrlFields="PubID">
                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField Text="Response Groups" DataNavigateUrlFormatString="ResponseGroup.aspx?PubID={0}"
                        DataNavigateUrlFields="PubID">
                        <HeaderStyle Width="10%" />
                    </asp:HyperLinkField>
                </Columns>
            </asp:GridView>
            <br />
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="600" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                            alt="" />
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
            </div>
            <table cellspacing="2" cellpadding="2" width="100%" align="left" border="0">
                <tr>
                    <td>
                        <table cellspacing="2" cellpadding="2" width="100%" align="left" border="0">
                            <tr>
                                <td width="12%">Name 
                                </td>
                                <td>
                                    <asp:HiddenField ID="hfPubID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hfCreatedUserID" runat="server" />
                                    <asp:HiddenField ID="hfCreatedDate" runat="server" />
                                    <asp:TextBox ID="txtName" runat="server" Text="" Width="250px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName"
                                        ErrorMessage="*" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Code 
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCode" runat="server" Text="" Width="250px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqCode" runat="server" ControlToValidate="txtCode"
                                        ErrorMessage="*" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Type 
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlType" runat="server" DataSourceID="SqlDataSourcePubTypes"
                                        DataTextField="PubTypeDisplayName" DataValueField="PubTypeID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Searching 
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbSearching" runat="server" Checked="True" />
                                </td>
                            </tr>
                            <tr>
                                <td>Score 
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpScore" runat="server">
                                        <asp:ListItem Value="0">0</asp:ListItem>
                                        <asp:ListItem Value="1" Selected="True">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                        <asp:ListItem Value="6">6</asp:ListItem>
                                        <asp:ListItem Value="7">7</asp:ListItem>
                                        <asp:ListItem Value="8">8</asp:ListItem>
                                        <asp:ListItem Value="9">9</asp:ListItem>
                                        <asp:ListItem Value="10">10</asp:ListItem>
                                        <asp:ListItem Value="11">11</asp:ListItem>
                                        <asp:ListItem Value="12">12</asp:ListItem>
                                        <asp:ListItem Value="13">13</asp:ListItem>
                                        <asp:ListItem Value="14">14</asp:ListItem>
                                        <asp:ListItem Value="15">15</asp:ListItem>
                                        <asp:ListItem Value="16">16</asp:ListItem>
                                        <asp:ListItem Value="17">17</asp:ListItem>
                                        <asp:ListItem Value="18">18</asp:ListItem>
                                        <asp:ListItem Value="19">19</asp:ListItem>
                                        <asp:ListItem Value="20">20</asp:ListItem>
                                        <asp:ListItem Value="21">21</asp:ListItem>
                                        <asp:ListItem Value="22">22</asp:ListItem>
                                        <asp:ListItem Value="23">23</asp:ListItem>
                                        <asp:ListItem Value="24">24</asp:ListItem>
                                        <asp:ListItem Value="25">25</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>HasPaidRecords 
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbHasPaidRecords" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Active 
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbIsActive" runat="server" Checked="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>IsUAD 
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbIsUAD" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>IsCirc 
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbIsCirc" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>UseSubGen 
                                </td>
                                <td>
                                    <asp:CheckBox ID="UseSubGen" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Frequency 
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpFrequency" runat="server" DataTextField="FrequencyName" DataValueField="FrequencyID"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Audit Period Start Date 
                                </td>
                                <td>
                                    <asp:TextBox ID="txtYearStartDate" runat="server" Text="" Width="50px"></asp:TextBox>
                                    <asp:ImageButton
                                        ID="ibYearStarDatePicker" runat="server" ImageUrl="~/Images/icon-calendar.gif" ImageAlign="Bottom" />
                                    <ajaxToolkit:CalendarExtender
                                        ID="CalendarExtender11" runat="server" CssClass="MyCalendar" TargetControlID="txtYearStartDate"
                                        Format="MM/dd" PopupButtonID="ibYearStarDatePicker">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Enter valid MM/DD format"
                                        ValidationExpression='^(0[1-9]|1[012])/([012]\d|3[01])$' ControlToValidate="txtYearStartDate" ForeColor="red"
                                        ValidationGroup="save" Display="Dynamic"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Audit Period End Date 
                                </td>
                                <td>
                                    <asp:TextBox ID="txtYearEndDate" runat="server" Text="" Width="50px"></asp:TextBox>
                                    <asp:ImageButton
                                        ID="ibYearEndDatePicker" runat="server" ImageUrl="~/Images/icon-calendar.gif" ImageAlign="Bottom" />
                                    <ajaxToolkit:CalendarExtender
                                        ID="CalendarExtender1" runat="server" CssClass="MyCalendar" TargetControlID="txtYearEndDate"
                                        Format="MM/dd" PopupButtonID="ibYearEndDatePicker">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Enter valid MM/DD format"
                                        ValidationExpression='^(0[1-9]|1[012])/([012]\d|3[01])$' ControlToValidate="txtYearEndDate" ForeColor="red"
                                        ValidationGroup="save" Display="Dynamic"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="border: 1px solid #CCCCCC">
                        <table cellspacing="5" cellpadding="5" width="75%" align="left" border="0">
                            <tr>
                                <td>Customers :
                                    <asp:DropDownList ID="drpClients" runat="server" CssClass="formfield" DataTextField="ClientName"
                                        DataValueField="ClientID" AutoPostBack="True" Width="200px" OnSelectedIndexChanged="drpClients_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                    <b>Available Groups</b>
                                </td>
                                <td></td>
                                <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                    <b>Selected Groups</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstSourceFields" runat="server" Rows="20" Style="text-transform: uppercase;"
                                        DataValueField="GroupID" DataTextField="GroupName" SelectionMode="Multiple" Font-Size="X-Small"
                                        Font-Names="Arial" Width="400px"></asp:ListBox>
                                </td>
                                <td align="center">
                                    <asp:Button ID="btnAdd" runat="server" Text=">>" CssClass="button" OnClick="btnAdd_Click" />
                                    <br />
                                    <br />
                                    <asp:Button ID="btnremove" runat="server" CssClass="button" Text="<<" OnClick="btnremove_Click" />
                                </td>
                                <td>
                                    <asp:ListBox ID="lstDestFields" runat="server" Rows="20" Style="text-transform: uppercase"
                                        DataValueField="GroupID" DataTextField="GroupName" SelectionMode="Multiple" Font-Size="X-Small"
                                        Font-Names="Arial" Width="400px"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="border: 1px solid #CCCCCC">
                        <table cellspacing="5" cellpadding="5" width="75%" align="left" border="0">
                            <tr>
                                <td><b>Brand Setup</b>
                                </td>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                    <b>Available Brands</b>
                                </td>
                                <td></td>
                                <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                    <b>Selected Brands</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstAvailableBrands" runat="server" Rows="20" Style="text-transform: uppercase;"
                                        DataValueField="BrandID" DataTextField="BrandName" SelectionMode="Multiple" Font-Size="X-Small"
                                        Font-Names="Arial" Width="400px"></asp:ListBox>
                                </td>
                                <td align="center">
                                    <asp:Button ID="btnAddBrand" runat="server" Text=">>" CssClass="button" OnClick="btnAddBrand_Click" />
                                    <br />
                                    <br />
                                    <asp:Button ID="btnRemoveBrand" runat="server" CssClass="button" Text="<<" OnClick="btnRemoveBrand_Click" />
                                </td>
                                <td>
                                    <asp:ListBox ID="lstSelectedBrands" runat="server" Rows="20" Style="text-transform: uppercase"
                                        DataValueField="BrandID" DataTextField="BrandName" SelectionMode="Multiple" Font-Size="X-Small"
                                        Font-Names="Arial" Width="400px"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="button"
                            ValidationGroup="save" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                            CssClass="button" />
                    </td>
                </tr>
            </table>
            <asp:SqlDataSource ID="SqlDataSourcePubTypes" runat="server" SelectCommand="select PubTypeID, PubTypeDisplayName from PubTypes where IsActive = 1"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

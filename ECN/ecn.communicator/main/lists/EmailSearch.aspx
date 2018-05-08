<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="EmailSearch.aspx.cs" Inherits="ecn.communicator.main.lists.EmailSearch" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
        <table cellspacing="0" cellpadding="0" width="674" align="center">
            <tr>
                <td id="errorTop">
                </td>
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
                <td id="errorBottom">
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <br/>
    <div>
        <label id="searchTypeTitle" runat="server"></label>
    </div><br/>
    <div>
        <label style="font-weight: bold">email address</label>
         <asp:DropDownList class="formfield" ID="FilterType" runat="Server" Visible="true" EnableViewState="False" Width="125">
            <asp:ListItem Value="like"> contains </asp:ListItem>
            <asp:ListItem Value="equals"> equals </asp:ListItem>
            <asp:ListItem Value="starts"> starts with </asp:ListItem>
            <asp:ListItem Value="ends"> ends with </asp:ListItem>
        </asp:DropDownList>
        <asp:textbox runat="server" ID="searchTerm"></asp:textbox>
        <asp:Button runat="server" Text="Get Results" OnClick="GetResults_Click"/>
    </div><br/>
    <div>
        <label style="font-weight: bold">Export this view to</label>
        <asp:DropDownList runat="server" ID="ddlExportType">
            <asp:ListItem Value="XLS">EXCEL [.xls]</asp:ListItem>
            <asp:ListItem Value="CSV">CSV [.csv]</asp:ListItem>
            <asp:ListItem Value="TXT">TXT [.txt]</asp:ListItem>
        </asp:DropDownList>
        <asp:Button runat="server" Text="Export" OnClick="ExportReport_Click"/>
    </div><br/>
    <div>
        <asp:DataGrid ID="ResultsGrid" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" Width="100%" Visible="true" OnSortCommand="SortCommand">
            <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
            <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            <Columns>
                <asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address" SortExpression="EmailAddress"/>
                <asp:BoundColumn DataField="BaseChannelName" HeaderText="Base Channel Name" SortExpression="BaseChannelName"/>
                <asp:BoundColumn DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName"/>
                <asp:BoundColumn DataField="GroupName" HeaderText="Group Name" SortExpression="GroupName"/>
                <asp:BoundColumn DataField="Subscribe" HeaderText="Subscribe" SortExpression="Subscribe"/>
                <asp:BoundColumn DataField="DateCreated" HeaderText="Date Added" SortExpression="DateCreated"/>
                <asp:BoundColumn DataField="DateModified" HeaderText="Date Modified" SortExpression="DateModified"/>
            </Columns>
        </asp:DataGrid>
        
        <div id="pageDiv" runat="server" style="display: none">
            <table cellpadding="0" border="0" width="100%">
                <tr>
                    <td align="left" class="label" width="31%">Total Records:
                        <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                    </td>
                    <td align="left" class="label" width="25%">Show Rows:
                        <asp:DropDownList ID="ddlPageSizeGroup" runat="server" AutoPostBack="true"  CssClass="formfield" OnSelectedIndexChanged="ddlPageSizeGroup_SelectedIndexChanged">
                            <asp:ListItem Value="5" />
                            <asp:ListItem Value="10" />
                            <asp:ListItem Value="15" />
                            <asp:ListItem Value="20" />
                        </asp:DropDownList>
                    </td>
                    <td align="right" class="label" width="44%">Page
                        <asp:TextBox ID="txtGoToPageGroup" runat="server" AutoPostBack="true"  class="formtextfield" Width="30px" OnTextChanged="GoToPageGroup_TextChanged" />of
                        <asp:Label ID="lblTotalNumberOfPagesGroup" runat="server" CssClass="label" />&nbsp;
                        <asp:Button ID="btnPreviousGroup" runat="server" ToolTip="Previous Page" class="formbuttonsmall" Text="<< Previous" OnClick="btnPrevious_Click"/>
                        <asp:Button ID="btnNextGroup" runat="server" ToolTip="Next Page" class="formbuttonsmall" Text="Next >>" OnClick="btnNext_Click"/>
                    </td>
                </tr>
            </table>    
        </div>
    </div>
    
    <div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" ShowRefreshButton="false"/>
    </div>

</asp:Content>

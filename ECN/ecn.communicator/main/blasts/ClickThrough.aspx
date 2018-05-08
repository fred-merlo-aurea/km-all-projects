<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="ClickThrough.aspx.cs" Inherits="ecn.communicator.main.blasts.ClickThrough" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 10px 0px; padding-top: 0px"
        align="center">
        <table width="100%" cellspacing="0" cellpadding="0" border='0'>
            <tr>
                <td valign="bottom" style="width:100%;text-align:right;" align="right">
                    <table cellspacing="0" cellpadding="0" border='0' width="100%">
                        <tr>
                            <td width="100%" align='right'  class="homeButton" style="height: 40px;text-align:right;width:100%;" valign="top">
                                <asp:LinkButton ID="btnHome" runat="Server" style="float:right;" Text="<span>Report Summary Page</span>"
                                    OnClick="btnHome_Click"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width:100%;text-align:right;">
                    <asp:Panel ID="DownloadPanel" runat="Server" Visible="true" CssClass="tableHeader"
                            Height="35px" Style="margin-top: 10px; text-align: right; width: 95%">
                            <table style="width:100%;">
                                <tr>
                                    
                                    
                                    <td style="text-align:right;width:100%;">
                                        <asp:Button class="formbuttonsmall" ID="DownloadEmailsButton"  OnClick="DownloadEmailsButton_Click"
                                            runat="Server" Text="Download" Visible="true" Enabled="true"></asp:Button>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="dgClickThrough" runat="Server" PageSize="50" Width="95%" AllowSorting="true" AutoGenerateColumns="False"
                        CssClass="gridWizard">
                        <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                        <AlternatingRowStyle CssClass="gridaltrowWizard" />
                        <Columns>
                            <asp:BoundField DataField="EmailAddress" ItemStyle-HorizontalAlign="Left" HeaderText="Email Address" />
                        </Columns>
                    </asp:GridView>
                    <AU:PagerBuilder ID="ClicksPager" runat="Server" Width="95%" PageSize="50" ControlToPage="dgClickThrough"
                            OnIndexChanged="ClicksPager_IndexChanged">
                            <PagerStyle CssClass="gridpager"></PagerStyle>
                        </AU:PagerBuilder>
                    <%--<asp:Panel ID="pnlPager" Width="100%" runat="server">
                        <table cellpadding="0" border="0" width="95%">
                                        <tr>
                                            <td align="left" class="label" width="31%">
                                                Total Records:
                                                <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                                            </td>
                                          <td align="left" class="label" width="25%">
                                                Show Rows:
                                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" CssClass="formfield" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                                    
                                                    <asp:ListItem Value="5" />
                                                    <asp:ListItem Value="10" />
                                                    <asp:ListItem Value="15" />
                                                    <asp:ListItem Value="20" />
                                                    <asp:ListItem Value="30" />
                                                    <asp:ListItem Value="40" />
                                                    <asp:ListItem Value="50" Selected="True" />
                                                    <asp:ListItem Value="100" />
                                                </asp:DropDownList>
                                            </td>
                                            <td align="right" class="label" width="44%">
                                                Page
                                                <asp:TextBox ID="txtGoToPage" runat="server" AutoPostBack="true" class="formtextfield" Width="30px" OnTextChanged="txtGoToPage_TextChanged" />
                                                    
                                                of
                                                <asp:Label ID="lblTotalNumberOfPages" runat="server" CssClass="label" />
                                                &nbsp;
                                                <asp:Button ID="btnPrevious" runat="server" CommandName="Page" ToolTip="Previous Page"
                                                    CommandArgument="Prev" class="formbuttonsmall" Text="<< Previous" OnClick="btnPrevious_Click"/>
                                                <asp:Button ID="btnNext" runat="server" CommandName="Page" ToolTip="Next Page"
                                                    CommandArgument="Next" class="formbuttonsmall" Text="Next >>" OnClick="btnNext_Click" />
                                            </td>
                                        </tr>
                                    </table>
                    </asp:Panel>--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

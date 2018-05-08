<%@ Page Language="c#" CodeBehind="domainTrackerUsers.aspx.cs" Inherits="ecn.communicator.main.lists.domainTrackerUsers" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel='stylesheet' href="../../MasterPages/ECN_MainMenu.css" type="text/css" />
    <link rel='stylesheet' href="../../MasterPages/ECN_Controls.css" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
        <table cellspacing="0" cellpadding="0" width="674" align="center">
            <tr>
                <td id="errorTop"></td>
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
                <td id="errorBottom"></td>
            </tr>
        </table>
    </asp:PlaceHolder><br />
    <asp:Label ID="lblDomainName" runat="server" Text="Label" Visible="true" Font-Size="Medium" Font-Bold="true"></asp:Label>
    <br /><br />
    <ecnCustom:ecnGridView ID="gvDomainTrackingUsers" runat="Server" Width="100%" CssClass="grid" AutoGenerateColumns="False"
    AllowPaging="True" OnRowCommand="gvDomainTrackingUsers_RowCommand" OnRowDataBound="gvDomainTrackingUsers_RowDataBound"
    OnPageIndexChanging="gvDomainTrackingUsers_PageIndexChanging" DataKeyNames="ProfileID"
    PageSize="15">
    <HeaderStyle CssClass="gridheader"></HeaderStyle>
    <FooterStyle CssClass="tableHeader1"></FooterStyle>
    <Columns>
        <asp:TemplateField Visible="false">
            <ItemTemplate>
                <asp:Label ID="lblProfileID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ProfileID") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" ItemStyle-Width="95%"
            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
        <asp:TemplateField Visible="true" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:LinkButton ID="lnkbtnProfile" CommandName="ProfileDetails" runat="server">+Details</asp:LinkButton>
                </td> </tr>
                <asp:Panel ID="pnlProfileReport" runat="Server" Visible="false">
                    <tr valign="top" style="top: 10px;">
                        <td colspan="2" align="left">
                            <table width="100%">
                                <tr>
                                    <td width="2%"></td>
                                    <td width="96%">
                                        <br />
                                        <ajaxToolkit:TabContainer ID="TabContainer1" runat="server">
                                            <ajaxToolkit:TabPanel ID="TabStandard" runat="server">
                                                <HeaderTemplate>
                                                    Standard Data Points
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <ecnCustom:ecnGridView ID="gvStandardDataPoints" runat="server" AllowSorting="false" AutoGenerateColumns="false"  CssClass="grid" 
                                                        Width="100%" AllowPaging="false" ShowFooter="false">
                                                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                             <Columns>
                                                                <asp:TemplateField HeaderText="PageURL" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPageURL" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PageURL") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="IPAddress" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIPAddress" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IPAddress") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="OS" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOS" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                  <asp:TemplateField HeaderText="Browser" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBrowser" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Browser") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="TimeStamp" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTimeStamp" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TimeStamp") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 </Columns>
                                                         </ecnCustom:ecnGridView>
                                                </ContentTemplate>
                                            </ajaxToolkit:TabPanel>
                                            <ajaxToolkit:TabPanel ID="TabAdditional" runat="server">
                                                <HeaderTemplate>
                                                    Additional Data Points
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                     <ecnCustom:ecnGridView ID="gvAdditionalDataPoints" runat="server" AllowSorting="false" AutoGenerateColumns="true"  CssClass="grid" 
                                                        Width="100%" AllowPaging="false" ShowFooter="false" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                    </ecnCustom:ecnGridView>
                                                </ContentTemplate>
                                            </ajaxToolkit:TabPanel>
                                        </ajaxToolkit:TabContainer>
                                        <br />
                                      
                                        <br />
                                    </td>
                                    <td width="2%"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <AlternatingRowStyle CssClass="gridaltrow" />
    <PagerTemplate>
        <table cellpadding="0" border="0" width="100%">
            <tr>
                <td align="left" class="label" width="31%">Total Records:
                    <asp:Label ID="gvDomainTrackingUsers_lblTotalRecords" runat="server" Text="" />
                </td>
                <td align="left" class="label" width="25%">Show Rows:
                    <asp:DropDownList ID="gvDomainTrackingUsers_ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="gvDomainTrackingUsers_ddlPageSize_SelectedIndexChanged"
                        CssClass="formfield">
                        <asp:ListItem Value="5" />
                        <asp:ListItem Value="10" />
                        <asp:ListItem Value="15" />
                        <asp:ListItem Value="20" />
                    </asp:DropDownList>
                </td>
                <td align="right" class="label" width="44%">Page
                    <asp:TextBox ID="gvDomainTrackingUsers_txtGoToPage" runat="server" AutoPostBack="true" OnTextChanged="gvDomainTrackingUsers_txtGoToPage_TextChanged"
                        class="formtextfield" Width="30px" />
                    of
                    <asp:Label ID="gvDomainTrackingUsers_lblTotalNumberOfPages" runat="server" CssClass="label" />
                    &nbsp;
                    <asp:Button ID="btnPrevious" runat="server" CommandName="Page" ToolTip="Previous Page"
                        CommandArgument="Prev" class="formbuttonsmall" Text="<< Previous" />
                    <asp:Button ID="btnNext" runat="server" CommandName="Page" ToolTip="Next Page" CommandArgument="Next"
                        class="formbuttonsmall" Text="Next >>" />
                </td>
            </tr>
        </table>
    </PagerTemplate>
</ecnCustom:ecnGridView>
</asp:Content>

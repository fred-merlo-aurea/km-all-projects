<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinkSource.aspx.cs" Inherits="ecn.communicator.main.content.LinkSource"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
<br />
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
    <br />
    <table width="100%" border='0' cellpadding="0" cellspacing="0">
        <tr>
            <td class="PageTitle" style="height: 19px" align="left">
                Link Owner List
            </td>
            <td class="PageTitle" style="height: 19px"  align="left">
                Add/Edit Link Owner
            </td>
        </tr>
        <tr>
            <td width="50%" valign="top" class="PageTitle">
                <asp:GridView ID="gvLinkOwner" AllowSorting="True" AllowPaging="True"
                    runat="server" DataKeyNames="LinkOwnerIndexID" AutoGenerateColumns="False" 
                    Width="100%" OnRowCommand="gvLinkOwner_RowCommand"
                    CssClass="grid" OnSorting="gvLinkOwner_Sorting" 
                    EmptyDataText="No Records Found." PageSize="20" 
                    onpageindexchanging="gvLinkOwner_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="LinkOwnerName" SortExpression="LinkOwnerName" HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                        </asp:BoundField>
                        <asp:BoundField DataField="LinkOwnerCode" SortExpression="LinkOwnerCode" HeaderText="Code" ItemStyle-HorizontalAlign="Left">
                        </asp:BoundField>
                        <asp:BoundField DataField="ContactFirstName" SortExpression="ContactFirstName" HeaderText="First Name" ItemStyle-HorizontalAlign="Left">
                        </asp:BoundField>
                        <asp:BoundField DataField="ContactLastName" SortExpression="ContactLastName" HeaderText="Last Name" ItemStyle-HorizontalAlign="Left">
                        </asp:BoundField>
                        <asp:BoundField DataField="IsActive" SortExpression="IsActive" HeaderText="Active"></asp:BoundField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>                
                                <asp:ImageButton runat="server" ID="editButton" ImageUrl="/ecn.images/images/icon-edits1.gif" CommandName="EditLinkOwnerName"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>                
                                <asp:ImageButton runat="server" ID="deleteButton" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="DeleteLinkOwnerName"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right"></PagerStyle>
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                </asp:GridView>
                <br />
            </td>
            <td width="50%" valign="top" class="PageTitle">
                <asp:DetailsView AutoGenerateRows="False" DataKeyNames="LinkOwnerIndexID" CssClass="grid" HeaderText="Link Owner Details" ID="dvLinkOwner"
                    runat="server" OnModeChanging="dvLinkOwner_ModeChanging"
                    Width="100%" DefaultMode="Insert" OnItemInserting="dvLinkOwner_ItemInserting"
                    OnItemUpdating="dvLinkOwner_ItemUpdating">
                    <CommandRowStyle HorizontalAlign="Right" Font-Bold="True"></CommandRowStyle>
                    <EditRowStyle HorizontalAlign="Left"></EditRowStyle>
                    <Fields>
                        <asp:BoundField ReadOnly="True" DataField="LinkOwnerIndexID" Visible="False" SortExpression="LinkOwnerIndexID"
                            HeaderText="LinkOwnerIndexID"></asp:BoundField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblOwnerName" Text='<%# Eval("LinkOwnerName") %>' runat="Server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="LinkOwnerName" Text='<%# Bind("LinkOwnerName") %>' runat="Server"
                                    Width="200px" CssClass="formfield" />
                                <asp:RequiredFieldValidator ErrorMessage="< required" ControlToValidate="LinkOwnerName"
                                    ID="RequiredFieldValidator1" runat="server" ValidationGroup="Owner"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Code">
                            <ItemTemplate>
                                <asp:Label ID="lblOwnerCode" Text='<%# Eval("LinkOwnerCode") %>' runat="Server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="LinkOwnerCode" Text='<%# Bind("LinkOwnerCode") %>' runat="Server"
                                    Width="100px" CssClass="formfield" />
                                <asp:RequiredFieldValidator ErrorMessage="< required" ControlToValidate="LinkOwnerName"
                                    ID="rfv2" runat="server" ValidationGroup="Owner"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Contact First Name">
                            <ItemTemplate>
                                <asp:Label ID="lblContactFirstName" Text='<%# Eval("ContactFirstName") %>' runat="Server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="ContactFirstName" Text='<%# Bind("ContactFirstName") %>' runat="Server"
                                    Width="200px" CssClass="formfield" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Contact Last Name">
                            <ItemTemplate>
                                <asp:Label ID="lblContactLastName" Text='<%# Eval("ContactLastName") %>' runat="Server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="ContactLastName" Text='<%# Bind("ContactLastName") %>' runat="Server"
                                    Width="200px" CssClass="formfield" />
                            </EditItemTemplate>
                        </asp:TemplateField>


                       <asp:TemplateField HeaderText="Phone">
                            <ItemTemplate>
                                <asp:Label ID="lblContactPhone" Text='<%# Eval("ContactPhone") %>' runat="Server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="ContactPhone" Text='<%# Bind("ContactPhone") %>' runat="Server"
                                    Width="200px" CssClass="formfield" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:Label ID="lblContactEmail" Text='<%# Eval("ContactEmail") %>' runat="Server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="ContactEmail" Text='<%# Bind("ContactEmail") %>' runat="Server"
                                    Width="200px" CssClass="formfield" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Address">
                            <ItemTemplate>
                                <asp:Label ID="lblAddress" Text='<%# Eval("Address") %>' runat="Server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="Address" Text='<%# Bind("Address") %>' runat="Server"
                                    Width="200px" CssClass="formfield" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="City">
                            <ItemTemplate>
                                <asp:Label ID="lblCity" Text='<%# Eval("City") %>' runat="Server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="City" Text='<%# Bind("City") %>' runat="Server"
                                    Width="200px" CssClass="formfield" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                      <asp:TemplateField HeaderText="State">
                            <ItemTemplate>
                                <asp:Label ID="lblState" Text='<%# Eval("State") %>' runat="Server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="State" Text='<%# Bind("State") %>' runat="Server"
                                    Width="200px" CssClass="formfield" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblSTatus" Text='<%# Eval("IsActive") %>' runat="Server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RadioButtonList ID="IsActive" runat="server" RepeatDirection="horizontal" CssClass="label10"
                                    SelectedValue='<%# Convert.ToBoolean(Eval("IsActive")) %>'>
                                    <asp:ListItem Value="True" Text="Active"></asp:ListItem>
                                    <asp:ListItem Value="False" Text="InActive"></asp:ListItem>
                                </asp:RadioButtonList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowInsertButton="True" ShowEditButton="True" ValidationGroup="Owner">
                        </asp:CommandField>
                    </Fields>
                    <FieldHeaderStyle Font-Bold="True"></FieldHeaderStyle>
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                </asp:DetailsView>
            </td>
        </tr>
        <tr>
            <td colspan="2" height="5" style="padding-top: 10px; padding-bottom: 10px;">
                <hr/>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="PageTitle"  align="left">
                Link Type List
            </td>
        </tr>
        <tr>
            <td class="PageTitle">
                <asp:GridView ID="gvLinkType" AllowSorting="True" AllowPaging="True"
                    runat="server" DataKeyNames="CodeID" AutoGenerateColumns="False" Width="100%" OnRowCommand="gvLinkType_RowCommand"
                    CssClass="grid" OnSelectedIndexChaning="gvLinkType_SelectedIndexChanging" OnSorting="gvLinkType_Sorting"
                    EmptyDataText="No Records Found." PageSize="20" onpageindexchanging="gvLinkType_PageIndexChanging">
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right"></PagerStyle>
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField DataField="CodeValue" HeaderText="Link Type" SortExpression="CodeValue"  ItemStyle-HorizontalAlign="Left" />
                          <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>                
                                <asp:ImageButton runat="server" ID="editButton" ImageUrl="/ecn.images/images/icon-edits1.gif" CommandName="EditLinkType"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>                
                                <asp:ImageButton runat="server" ID="deleteButton" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="DeleteLinkType"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td valign="top" class="PageTitle">
                <asp:DetailsView AutoGenerateRows="false" DataKeyNames="CodeID" CssClass="grid"
                    HeaderText="Link Type Details" ID="dvLinkType" runat="server" OnModeChanging="dvLinkType_ModeChanging"
                    Width="100%" DefaultMode="Insert" OnItemInserting="dvLinkType_ItemInserting"
                    OnItemUpdating="dvLinkType_ItemUpdating">
                    <CommandRowStyle Font-Bold="True"></CommandRowStyle>
                    <FooterStyle ForeColor="#F7F6F3" BackColor="#5D7B9D" Font-Bold="True" Height="25px">
                    </FooterStyle>
                    <FieldHeaderStyle Font-Bold="True"></FieldHeaderStyle>
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <Fields>
                        <asp:BoundField DataField="CodeID" HeaderText="CodeID" ReadOnly="True" SortExpression="CodeID" Visible="false" />
                        <asp:TemplateField HeaderText="Link Type"  ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblCodeValue" Text='<%# Eval("CodeValue") %>' runat="Server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="CodeValue" Text='<%# Bind("CodeValue") %>' runat="Server" Width="200px"
                                    CssClass="formfield" />
                                <asp:RequiredFieldValidator ErrorMessage="< required" ControlToValidate="CodeValue"
                                    ID="rfv3" runat="server" ValidationGroup="linkType"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" ShowInsertButton="True" ValidationGroup="linkType"  ItemStyle-HorizontalAlign="Right" />
                    </Fields>
                </asp:DetailsView>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

<%@ Page Language="c#" Inherits="ecn.communicator.channelsmanager.templates" CodeBehind="templates.aspx.cs"
    MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
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
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td class="tableHeader">
                <div style="float: right">
                    <label class="TableHeader">Category:  </label><asp:dropdownlist ID="ddlCategoryFilter" DataValueField="TemplateID" DataTextField="Category" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategoryFilter_IndexChanged"></asp:dropdownlist>    
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdTemplates" AllowPaging="false" CssClass="grid" Width="100%"
                    AutoGenerateColumns="false" runat="server" GridLines="None" DataKeyNames="TemplateID"
                    AllowSorting="false" OnRowDeleting="grdTemplates_RowDeleting" OnPageIndexChanging="grdTemplates_PageIndexChanging"
                    OnRowCommand="grdTemplates_RowCommand">
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Image" ItemStyle-Width="4%">
                            <ItemTemplate>
                                <img src='<%# Eval("Thumb") %>' border='0' height="50" align="middle" alt=""/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Description">
                            <ItemTemplate>
                               <asp:Label Text='<%# Eval("Descr") %>'  runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TemplateStyleCode" HeaderText="Template Type" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="middle" ItemStyle-Width="13%" />
                        <asp:BoundField DataField="SlotsTotal" HeaderText="Slots" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="middle" />
                        <asp:BoundField DataField="IsActive" HeaderText="Active" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="middle" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit" ItemStyle-Width="4%">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlEdit" runat="server" NavigateUrl='<%# Eval("ChannelID",Eval("TemplateID", "templateseditor.aspx?TemplateID={0}") + "&ChannelID={0}") %>'
                                    Text="<center><img src=/ecn.images/images/icon-edits1.gif border='0' alt='Edit Template'></center>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnDelete" CommandArgument='<%# Eval("TemplateID") %>' CommandName="Delete"
                                    OnClientClick="return confirm('Are you Sure?\nSelected Template will be permanently deleted.');"
                                    runat="server"><img src="/ecn.images/images/icon-delete1.gif" border='0' alt='Delete Template'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>

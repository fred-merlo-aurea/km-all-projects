<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true"
    CodeBehind="PublisherList.aspx.cs" Inherits="KMPS_JF_Setup.Publisher.PublisherList"
    Title="KMPS Form Builder - Publisher List" %>

<%@ Register TagPrefix="JF" Namespace="KMPS_JF_Objects.Controls" Assembly="KMPS_JF_Objects" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpPubList" runat="server">
        <ContentTemplate>
            <JF:BoxPanel ID="BoxPanel2" runat="server" Width="100%" Title="Create new Publications">
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 50%;">
                            <b>Customer :</b> &nbsp;&nbsp;<asp:DropDownList ID="ddlCustomer" runat="server" Width="200px"
                                AppendDataBoundItems="true" DataSourceID="SqlDataSourcePCustomerConnect" DataTextField="CustomerName"
                                DataValueField="CustomerId" AutoPostBack="true">
                                <asp:ListItem Text="ALL " Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSourcePCustomerConnect" runat="server" ConnectionString="<%$ ConnectionStrings:ecn5_accounts %>"
                                SelectCommandType="Text"></asp:SqlDataSource>
                        </td>
                        <td style="text-align: right; width: 50%;">
                            <asp:Button ID="btnAdd" Text="New Publication" CssClass="buttonMedium" runat="server"
                                OnClick="btnAdd_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                            <asp:GridView ID="grdPublisherList" DataSourceID="SqlDataSourcePListConnect" runat="server"
                                AllowSorting="true" AutoGenerateColumns="false" DataKeyNames="PubId" Width="100%"
                                PageSize="20" AllowPaging="true">
                                <Columns>
                                    <asp:BoundField DataField="PubName" HeaderText="Pub Name" ReadOnly="true" SortExpression="PubName"
                                        ItemStyle-Width="20%" />
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="UDF" ItemStyle-HorizontalAlign="center"
                                        ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <div class="button">
                                                <asp:HyperLink ID="hyplnkUDF" runat="server" Text="UDF" NavigateUrl='<%# String.Format("Pub_UDF.aspx?PubId={0}&PubName={1}", Eval("PubID"),Eval("PubName")) %>'></asp:HyperLink>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Category" ItemStyle-HorizontalAlign="center"
                                        ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <div class="button">
                                                <asp:HyperLink ID="hyplnkCategories" runat="server" Text="Category" NavigateUrl='<%# String.Format("Pub_Categories.aspx?PubId={0}&PubName={1}", Eval("PubID"),Eval("PubName")) %>'></asp:HyperLink></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Newsletters"
                                        ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <div class="button">
                                                <asp:HyperLink ID="hyplnkNewsLetters" runat="server" Text="Newsletters" NavigateUrl='<%# String.Format("Pub_Newsletters.aspx?PubId={0}&PubName={1}", Eval("PubID"),Eval("PubName")) %>'></asp:HyperLink></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Pubs" ItemStyle-HorizontalAlign="center"
                                        ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <div class="button">
                                                <asp:HyperLink ID="hyplnkPubs" runat="server" Text="Pubs" NavigateUrl='<%# String.Format("Pub_RelatedPubs.aspx?PubId={0}&PubName={1}", Eval("PubID"),Eval("PubName")) %>'></asp:HyperLink></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Events" ItemStyle-HorizontalAlign="center"
                                        ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <div class="button">
                                                <asp:HyperLink ID="hyplnkEvents" runat="server" Text="Events" NavigateUrl='<%# String.Format("Pub_Events.aspx?PubId={0}&PubName={1}", Eval("PubID"),Eval("PubName")) %>'></asp:HyperLink></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Pages" ItemStyle-HorizontalAlign="center"
                                        ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <div class="button">
                                                <asp:HyperLink ID="hyplnkPages" runat="server" Text="Pages" NavigateUrl='<%# String.Format("Pub_CustomPage.aspx?PubId={0}&PubName={1}", Eval("PubID"),Eval("PubName")) %>'></asp:HyperLink></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Forms" ItemStyle-HorizontalAlign="center"
                                        ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <div class="button">
                                                <asp:HyperLink ID="hyplnkForms" runat="server" Text="Forms" NavigateUrl='<%# String.Format("Pub_Forms.aspx?PubId={0}&PubName={1}", Eval("PubID"),Eval("PubName")) %>'></asp:HyperLink></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Auto Subs" ItemStyle-HorizontalAlign="center"
                                        ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <div class="button">
                                                <asp:HyperLink ID="hyplnkAutoForms" runat="server" Text="Auto Subs" NavigateUrl='<%# String.Format("Pub_AutoSubscription.aspx?PubId={0}&PubName={1}", Eval("PubID"),Eval("PubName")) %>'></asp:HyperLink></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderText="Edit" ItemStyle-HorizontalAlign="center"
                                        ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink2" runat="server" ImageUrl="~/images/icon-edit.gif" NavigateUrl='<%# String.Format("PublisherAdd.aspx?PubId={0}", Eval("PubID")) %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSourcePListConnect" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                SelectCommand="select * from publications where ECNcustomerID in ( @CustomerID )"
                                SelectCommandType="Text" OnSelecting="SqlDataSourcePListConnect_Selecting"></asp:SqlDataSource>
                        </td>
                    </tr>
                </table>
            </JF:BoxPanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

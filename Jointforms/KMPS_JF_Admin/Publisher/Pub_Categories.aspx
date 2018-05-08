<%@ Page MasterPageFile="~/MasterPages/Site.master" Language="C#" AutoEventWireup="true"
    CodeBehind="Pub_Categories.aspx.cs" Inherits="KMPS_JF_Setup.Publisher.Pub_Categories"
    Title="KMPS Form Builder - Categories" %>

<%@ Register Src="~/Publisher/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="lftMenu" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpPubList" runat="server">
        <ContentTemplate>
            <JF:BoxPanel ID="BoxPanel2" runat="server" Width="100%" Title="Manage Publication Categories">
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20%; vertical-align: top;">
                            <lftMenu:LeftMenu ID="LeftMenu" runat="server" CurrentMenu="CATEGORY" />
                        </td>
                        <td style="width: 2%;">
                            &nbsp;
                        </td>
                        <td style="width: 78%; vertical-align: top;">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdPublisherCategory" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                            Width="100%" AllowPaging="true" DataKeyNames="CategoryId" DataSourceID="SqlDataSourcePCategoryConnect"
                                            OnRowCommand="grdPublisherCategory_RowCommand">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategoryID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CategoryID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CategoryName" HeaderText="Category Name" ReadOnly="true"
                                                    SortExpression="CategoryName" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="true"
                                                    SortExpression="Description" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="30%" HeaderText="Type" SortExpression="CategoryType">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategoryType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CategoryType").ToString().ToUpper()=="F"?"Field":"Newsletter" %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnCategoryEdit" runat="server" ImageUrl="~/images/icon-edit.gif"
                                                            CommandName="CategoryEdit" CausesValidation="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnCategoryDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                            CommandName="CategoryDelete" OnClientClick="return confirm('Are you sure, you want to delete this record?')"
                                                            CausesValidation="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSourcePCategoryConnect" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                            SelectCommand="sp_PublishersCategory" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:QueryStringParameter QueryStringField="PubID" Name="PubID" Type="Int32" DefaultValue="0" />
                                                <asp:Parameter Name="CategoryID" Type="Int32" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="txtCategoryName" PropertyName="Text" Name="CategoryName"
                                                    DefaultValue="0" ConvertEmptyStringToNull="false" />
                                                <asp:ControlParameter ControlID="txtDescription" PropertyName="Text" Name="CategoryDesc"
                                                    DefaultValue="0" ConvertEmptyStringToNull="false" />
                                                <asp:ControlParameter ControlID="ddlCategoryType" PropertyName="SelectedValue" Name="CategoryType"
                                                    DefaultValue="0" ConvertEmptyStringToNull="false" />
                                                <asp:Parameter Name="AddedBy" Type="String" DefaultValue="0" ConvertEmptyStringToNull="false" />
                                                <asp:Parameter Name="ModifiedBy" Type="String" DefaultValue="0" ConvertEmptyStringToNull="false" />
                                                <asp:Parameter Name="iMod" Type="Int32" DefaultValue="4" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; text-align: left">
                                        <JF:BoxPanel ID="BoxPanel1" runat="server" Width="100%" Title="Add Category">
                                            <table style="width: 90%;" border="0px" cellpadding="5" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: left; width: 30%">
                                                        Name
                                                    </td>
                                                    <td style="text-align: left; width: 70%">
                                                        <asp:TextBox ID="txtCategoryName" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqCategoryName" runat="server" ControlToValidate="txtCategoryName"
                                                            ErrorMessage="*" Font-Bold="false"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Description
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqDescription" runat="server" ControlToValidate="txtDescription"
                                                            ErrorMessage="*" Font-Bold="false"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Category Type
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCategoryType" runat="server" Width="200px">
                                                            <asp:ListItem Text="Newsletter" Value="N" Selected="true"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
                                                        <asp:HiddenField ID="hfldCategoryID" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="text-align: left">
                                                        <asp:Button ID="btnAdd" runat="server" Text="SAVE" OnClick="btnAdd_Click" CssClass="button" />&nbsp;&nbsp;
                                                        <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                                                            CausesValidation="false" CssClass="button" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </JF:BoxPanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </JF:BoxPanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

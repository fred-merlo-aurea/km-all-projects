<%@ Page MasterPageFile="~/MasterPages/Site.master" Language="C#" AutoEventWireup="true"
    CodeBehind="Pub_Forms.aspx.cs" Inherits="KMPS_JF_Setup.Publisher.Pub_Forms" Title="KMPS Form Builder - Publication Forms" %>

<%@ Register Src="~/Publisher/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="lftMenu" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpPubList" runat="server">
        <ContentTemplate>
            <JF:BoxPanel ID="BoxPanel2" runat="server" Width="100%" Title="Create new Forms">
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20%; vertical-align: top;">
                            <lftMenu:LeftMenu ID="LeftMenu" runat="server" CurrentMenu="FORMS" />
                        </td>
                        <td style="width: 2%;">
                            &nbsp;
                        </td>
                        <td style="width: 78%; vertical-align: top;">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdPublisherForms" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                            Width="100%" AllowPaging="true" DataKeyNames="PFID" DataSourceID="SQLDataSourceForm"
                                            OnRowCommand="grdPublisherForms_RowCommand">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFormID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PFID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="FormName" HeaderText="Form Name" ReadOnly="true" SortExpression="FormName"
                                                    ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="true"
                                                    SortExpression="Description" ItemStyle-Width="40%" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:HyperLinkField HeaderText="Edit" DataTextField="PFID" DataTextFormatString="&lt;img src='../images/icon-edit.gif' border='0' /&gt;"
                                                    DataNavigateUrlFormatString="Pub_FormsCreate.aspx?pubid={0}&PFID={1}" DataNavigateUrlFields="PubID,PFID"
                                                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="5%" />
                                                <asp:HyperLinkField HeaderText="Preview" DataTextField="PubID" DataTextFormatString="&lt;img src='../images/icon-preview.gif' border='0' /&gt;"
                                                    DataNavigateUrlFormatString="http://eforms.kmpsgroup.com/jointforms/forms/subscription.aspx?pubid={0}&mode=preview"
                                                    DataNavigateUrlFields="PubID" Target="_blank" ItemStyle-HorizontalAlign="center"
                                                    HeaderStyle-HorizontalAlign="center" ItemStyle-Width="5%" />
                                                 <asp:TemplateField HeaderText="Copy" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnFormCopy" runat="server" ImageUrl="~/Images/icon-copy.png" Width="16" Height="16"
                                                            CommandArgument='<%# DataBinder.Eval(Container,"DataItem.PFID") %>' CommandName="FormCopy"
                                                            OnClientClick="return confirm('Are you sure, you want to copy this Form?')"
                                                            CausesValidation="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnFormDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                            CommandArgument='<%# DataBinder.Eval(Container,"DataItem.PFID") %>' CommandName="FormDelete"
                                                            OnClientClick="return confirm('Are you sure, you want to delete this Form?')"
                                                            CausesValidation="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                        Link 1:
                                        <asp:HyperLink ID="hLink1" runat="server" Target="_blank"></asp:HyperLink><br />
                                        <br />
                                        Link 2:
                                        <asp:HyperLink ID="hLink2" runat="server" Target="_blank"></asp:HyperLink><br />
                                        <br />
                                        <asp:SqlDataSource ID="SQLDataSourceForm" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                            SelectCommand="select * from pubforms where pubID=@pubID" SelectCommandType="Text">
                                            <SelectParameters>
                                                <asp:QueryStringParameter DefaultValue="-1" Name="PubID" QueryStringField="PubId"
                                                    Type="Int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Button runat="server" Text="Create New Form" ID="btnCreate" OnClick="btnCreate_Click"
                                            CssClass="buttonMedium" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
                                        <asp:HiddenField ID="hfldFormId" runat="server" />
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

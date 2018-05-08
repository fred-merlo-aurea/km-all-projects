<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="PaidPub.main.eNewsletter.Category" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
   <div class="contentheader">
        Category
    </div>
    <br />
    <div class="boxcontent padding20">
        <asp:GridView ID="gveNewsLetter" DataSourceID="dsrceNewsLetter" AllowSorting="True"
            AllowPaging="True" runat="server" DataKeyNames="CategoryID" AutoGenerateColumns="False"
            Width="90%" EmptyDataText="No eNewsLetter Found." CellPadding="4" ForeColor="#333333"
            GridLines="both">
            <RowStyle ForeColor="#333333" BackColor="#F7F6F3" />
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Category Name" SortExpression="Name">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                </asp:BoundField>
                <asp:BoundField DataField="Desc" HeaderText="Description" SortExpression="Desc">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                </asp:BoundField>
                <asp:HyperLinkField HeaderText="Edit" DataTextField="CategoryID" DataTextFormatString="&lt;img src='../../images/btn_edit.png' border='0' /&gt;"
                    DataNavigateUrlFormatString="categoryadd.aspx?CategoryID={0}" DataNavigateUrlFields="CategoryID"
                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%" HeaderStyle-Width="5%"  ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="Server" CommandName="Delete" AlternateText="Delete eNewsletter Group"
                            ImageUrl="~/images/btn_delete.png" OnClientClick="return confirm('Are you sure you want to delete?')" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#00477F" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:SqlDataSource ID="dsrceNewsLetter" runat="server" SelectCommand="select * from ecn_misc..CANON_PAIDPUB_eNewsLetter_Category where customerID = @customerID order by name"
            DeleteCommand="DELETE FROM CANON_PAIDPUB_eNewsLetter_Category WHERE CategoryID = @CategoryID"
            ConnectionString="<%$ ConnectionStrings:conn_misc %>">
            <SelectParameters>
                <asp:SessionParameter Name="CustomerID" SessionField="CustomerID" />
            </SelectParameters>
            </asp:SqlDataSource>
    </div>
</asp:Content>

<%@ Page Language="C#"  MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="PaidPub.main.Pricing._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
   <div class="contentheader">
        Pricing
    </div>
    <br />
    <div class="boxcontent padding20">
        <asp:GridView ID="gvPricing" DataSourceID="dsrcPricing" AllowSorting="True"
            AllowPaging="True" runat="server" DataKeyNames="PriceID" AutoGenerateColumns="False"
            Width="90%" EmptyDataText="No Records Found." CellPadding="4" ForeColor="#333333"
            GridLines="both">
            <RowStyle ForeColor="#333333" BackColor="#F7F6F3" />
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                </asp:BoundField>
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                </asp:BoundField>
                <asp:HyperLinkField HeaderText="Edit" DataTextField="PriceID" DataTextFormatString="&lt;img src='../../images/btn_edit.png' border='0' /&gt;"
                    DataNavigateUrlFormatString="add.aspx?PriceID={0}" DataNavigateUrlFields="PriceID"
                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%" HeaderStyle-Width="5%"  ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="Server" CommandName="Delete" AlternateText="Delete Pricing Group"
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
        <asp:SqlDataSource ID="dsrcPricing" runat="server" SelectCommand="select * from ecn_misc..CANON_PAIDPUB_Pricing where  customerID = @customerID order by [name]"
            DeleteCommand="DELETE FROM CANON_PAIDPUB_Pricing WHERE PriceID = @PriceID"
            ConnectionString="<%$ ConnectionStrings:conn_misc %>">
            <SelectParameters>
                <asp:SessionParameter Name="CustomerID" SessionField="CustomerID" />
            </SelectParameters>
            </asp:SqlDataSource>
    </div>
</asp:Content>

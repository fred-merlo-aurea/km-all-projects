<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs"
    Inherits="PaidPub.main.Promotions._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="contentheader">
        Promotions
    </div>
    <br />
    <div class="boxcontent padding20">
        <asp:GridView ID="gvPromotions" DataSourceID="dsrcPromotions" AllowSorting="True"
            AllowPaging="True" runat="server" DataKeyNames="PromotionID" AutoGenerateColumns="False"
            Width="80%" EmptyDataText="No Promotions Found." CellPadding="4" ForeColor="#333333"
            GridLines="both">
            <RowStyle ForeColor="#333333" BackColor="#F7F6F3" />
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Promotion Name" SortExpression="Name">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Width="40%" />
                </asp:BoundField>
                <asp:BoundField DataField="Code" HeaderText="Promotion Code" SortExpression="Code">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                </asp:BoundField>
                <asp:BoundField DataField="Discount" HeaderText="Discount %" SortExpression="Discount"
                    DataFormatString="{0:F2}%">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" HeaderStyle-Width="10%"
                    ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("IsActive"))?"Yes":"No" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField HeaderText="Edit" DataTextField="promotionID" DataTextFormatString="&lt;img src='../../images/btn_edit.png' border='0' /&gt;"
                    DataNavigateUrlFormatString="add.aspx?PromotionID={0}" DataNavigateUrlFields="promotionID"
                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="10%" HeaderStyle-Width="10%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="Server" CommandName="Delete" AlternateText="Delete Category"
                            ImageUrl="~/images/btn_delete.png" OnClientClick="return confirm('Are you sure you want to delete?')" />
                    </ItemTemplate>
                    <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="10%"  HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#00477F" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:SqlDataSource ID="dsrcPromotions" runat="server" SelectCommand="select * from CANON_PAIDPUB_Promotions where customerID = @customerID order by Name"
            DeleteCommand="DELETE FROM CANON_PAIDPUB_Promotions WHERE PromotionID = @PromotionID"
            ConnectionString="<%$ ConnectionStrings:conn_misc %>">
            <SelectParameters>
                <asp:SessionParameter Name="CustomerID" SessionField="CustomerID" />
            </SelectParameters>
            </asp:SqlDataSource>
    </div>
</asp:Content>

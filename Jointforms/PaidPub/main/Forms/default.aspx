<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs"
    Inherits="PaidPub.main.Forms._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="contentheader">
        Form
    </div>
    <br />
    <div class="boxcontent padding20">
        <asp:GridView ID="gvForm" DataSourceID="dsrcForm" AllowSorting="True" AllowPaging="True"
            runat="server" DataKeyNames="FormID" AutoGenerateColumns="False" Width="80%"
            EmptyDataText="No Subscription Forms." CellPadding="4" ForeColor="#333333" GridLines="both">
            <RowStyle ForeColor="#333333" BackColor="#F7F6F3" />
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Newsletter Name" SortExpression="Name">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Width="85%" />
                </asp:BoundField>
                <asp:HyperLinkField HeaderText="Preview" DataTextField="Code" DataTextFormatString="&lt;img src='../../images/btn_html.png' border='0' /&gt;"
                    DataNavigateUrlFormatString="Preview.aspx?Code={0}" DataNavigateUrlFields="Code"
                    Target="_blank" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"
                    ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                <asp:HyperLinkField HeaderText="Edit" DataTextField="FormID" DataTextFormatString="&lt;img src='../../images/btn_edit.png' border='0' /&gt;"
                    DataNavigateUrlFormatString="add.aspx?FormID={0}" DataNavigateUrlFields="FormID"
                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle"
                    HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%" HeaderStyle-Width="5%"
                    ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="Server" CommandName="Delete" AlternateText="Delete Form"
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
        <asp:SqlDataSource ID="dsrcForm" runat="server" SelectCommand="select * from ecn_misc..CANON_PAIDPUB_Forms where customerID = @customerID order by name asc"
            DeleteCommand="DELETE FROM CANON_PAIDPUB_Forms WHERE FormID = @FormID" ConnectionString="<%$ ConnectionStrings:conn_misc %>">
            <SelectParameters>
                <asp:SessionParameter Name="CustomerID" SessionField="CustomerID" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>

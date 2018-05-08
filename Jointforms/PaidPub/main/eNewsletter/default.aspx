<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs"
    Inherits="PaidPub.main.eNewsletter._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
   <div class="contentheader">
        eNewsletter
    </div>
    <br />
    <div class="boxcontent padding20">
        <asp:GridView ID="gveNewsLetter" DataSourceID="dsrceNewsLetter" AllowSorting="True"
            AllowPaging="True" runat="server" DataKeyNames="GroupID" AutoGenerateColumns="False"
            Width="90%" EmptyDataText="No eNewsLetter Found." CellPadding="4" ForeColor="#333333"
            GridLines="both">
            <RowStyle ForeColor="#333333" BackColor="#F7F6F3" />
            <Columns>
                <asp:BoundField DataField="GroupName" HeaderText="Newsletter Name" SortExpression="GroupName">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="GroupDescription" HeaderText="Description" SortExpression="GroupDescription">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                </asp:BoundField>
                <asp:BoundField DataField="CategoryName" HeaderText="CategoryName" SortExpression="CategoryName">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="Frequency" HeaderText="Frequency" SortExpression="Frequency">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                </asp:BoundField>                
                <asp:HyperLinkField HeaderText="Edit" DataTextField="GroupID" DataTextFormatString="&lt;img src='../../images/btn_edit.png' border='0' /&gt;"
                    DataNavigateUrlFormatString="add.aspx?GroupID={0}" DataNavigateUrlFields="GroupID"
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
        <asp:SqlDataSource ID="dsrceNewsLetter" runat="server" SelectCommand="select *, ec.name as categoryname from ecn_misc..CANON_PAIDPUB_eNewsLetters n join ecn5_communicator..groups g on n.groupID = g.groupID left outer join  ecn_misc..CANON_PAIDPUB_Frequency f on n.frequencyID = f.frequencyID left outer join CANON_PAIDPUB_eNewsletter_Category ec on ec.categoryID = n.categoryID where n.customerID = @customerID order by ec.name"
            DeleteCommand="DELETE FROM CANON_PAIDPUB_eNewsLetters WHERE GroupID = @GroupID"
            ConnectionString="<%$ ConnectionStrings:conn_misc %>">
            <SelectParameters>
                <asp:SessionParameter Name="CustomerID" SessionField="CustomerID" />
            </SelectParameters>
            </asp:SqlDataSource>
    </div>
</asp:Content>

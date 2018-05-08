<%@ Control Language="c#" Inherits="ecn.creator.includes.templates" Codebehind="templates.ascx.cs" %>
<asp:DataList ID="templaterepeater" runat="server" CellSpacing="2" CellPadding="2"
    GridLines="Both" RepeatColumns="6" RepeatLayout="Table" BorderWidth="0" OnItemCommand="DoItemSelect"
    SelectedItemStyle-BackColor="lightgrey" SelectedItemStyle-VerticalAlign="Top"
    SelectedItemStyle-HorizontalAlign="Center" SelectedItemStyle-BorderWidth="1"
    SelectedItemStyle-BorderColor="black" SelectedItemStyle-Font-Size="xxsmall" ItemStyle-VerticalAlign="Top"
    ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
    <ItemTemplate>
        <img src="<%#DataBinder.Eval(Container.DataItem,"TemplateImage")%>"><br>
        <br>
        <asp:Button CommandName="Select" runat="server" ID="Select" CssClass="FormButton"
            Text='Select'></asp:Button><br>
        <asp:TextBox runat="server" ID="TemplateID" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "TemplateID") %>'></asp:TextBox>
    </ItemTemplate>
    <SelectedItemTemplate>
        <img src="<%#DataBinder.Eval(Container.DataItem,"TemplateImage")%>"><br>
        <br>
        <%#DataBinder.Eval(Container.DataItem,"TemplateName")%>
        <br>
        <asp:TextBox runat="server" ID="TemplateID" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "TemplateID") %>'></asp:TextBox>
        <asp:TextBox runat="server" ID="SlotsTotal" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "SlotsTotal") %>'></asp:TextBox>
    </SelectedItemTemplate>
</asp:DataList>

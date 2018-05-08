<%@ Control Language="c#" Inherits="ecn.communicator.includes.templates" Codebehind="templates.ascx.cs" %>
<asp:DataList ID="templaterepeater" runat="server" CellSpacing="2" CellPadding="2"
    GridLines="None" RepeatDirection="Horizontal" RepeatColumns="6" RepeatLayout="Table"  BorderWidth="0" OnItemCommand="DoItemSelect"
    SelectedItemStyle-BackColor="Transparent" SelectedItemStyle-VerticalAlign="Top"
    SelectedItemStyle-HorizontalAlign="Center" SelectedItemStyle-BorderWidth="2"
    SelectedItemStyle-BorderColor="Gray"  SelectedItemStyle-BorderStyle="Dashed" SelectedItemStyle-Font-Size="xxsmall" ItemStyle-VerticalAlign="Top"
    ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
    <ItemTemplate>
        <img src="<%#DataBinder.Eval(Container.DataItem,"TemplateImage")%>"><br>
        <br>
        <asp:LinkButton CommandName="Select" runat="server" ID="Select" CssClass="FormButton"
            Text='Select'></asp:LinkButton><br>
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

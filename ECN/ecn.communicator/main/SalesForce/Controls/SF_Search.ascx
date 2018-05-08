<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SF_Search.ascx.cs" Inherits="ecn.communicator.main.Salesforce.Controls.SF_Search" %>

<asp:UpdatePanel ID="upSearch" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <Triggers>
        <asp:PostBackTrigger ControlID="gvSearch" />
    </Triggers>
    <ContentTemplate>

        <div style="overflow: auto; height: 100px;">
            <asp:GridView ID="gvSearch" runat="server" OnRowDataBound="gvSearch_RowDataBound" AutoGenerateColumns="false" AllowSorting="false" CssClass="km_grid">
                <Columns>

                    <asp:TemplateField HeaderText="Field">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlField" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlField_SelectedIndexChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Criteria">
                        <ItemTemplate>
                            <asp:TextBox ID="txtSearch" runat="server" Width="100px" />
                            <asp:DropDownList ID="ddlBoolean" runat="server" Width="100px" Visible="false">
                                <asp:ListItem Text="True" Value="true" Selected="True" />
                                <asp:ListItem Text="False" Value="false" />
                            </asp:DropDownList>
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Logic">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlLogic" OnSelectedIndexChanged="ddlLogic_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                <asp:ListItem Text="SELECT" Selected="True" Value="-1" />
                                <asp:ListItem Text="AND" Value="and" />
                                <asp:ListItem Text="OR" Value="or" />
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

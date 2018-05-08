<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true"
    CodeBehind="PublicationTypes.aspx.cs" Inherits="KMPS.MDAdmin.PublicationTypes" %>

<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">

        function ValidateDelete() {
            if (!confirm('Are you sure you want to delete?')) return false;

            if (!confirm('Are you sure you want to delete Publication Type. It will delete all mapping for the Publication Type?')) return false;

            return true;
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvPublicationTypes" runat="server" AllowPaging="True" AllowSorting="True" OnSorting="gvPublicationTypes_Sorting"
                AutoGenerateColumns="False" 
                DataKeyNames="PubTypeID" OnRowCommand="gvPublicationTypes_RowCommand"  OnPageIndexChanging="gvPublicationTypes_PageIndexChanging" OnRowDeleting="gvPublicationTypes_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="PubTypeDisplayName" HeaderText="Display Name" SortExpression="PubTypeDisplayName"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="55%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SortOrder" HeaderText="Sort Order" SortExpression="SortOrder"
                        HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" >
                     </asp:BoundField>
                    <asp:TemplateField HeaderText="Is Active" SortExpression="IsActive" HeaderStyle-HorizontalAlign="center"  ItemStyle-HorizontalAlign="Center" > 
                        <ItemTemplate><%# (Boolean.Parse(Eval("IsActive").ToString())) ? "Yes" : "No"%></ItemTemplate> 
                    </asp:TemplateField>
                    <asp:ButtonField HeaderStyle-Width="5%" ItemStyle-Width="5%" ButtonType="Link"
                        Text="<img src='Images/ic-edit.gif' style='border:none;'>" CommandName="Select"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                    <asp:TemplateField 
                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("PubTypeID")%>' OnClientClick="return ValidateDelete();" CausesValidation="false"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop">
                        </td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="images/errorEx.jpg" runat="server"
                                            alt="" />
                                    </td>
                                    <td valign="middle" align="left" width="80%" height="100%">
                                        <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="errorBottom">
                        </td>
                    </tr>
                </table>
                <br />
            </div>
            <asp:SqlDataSource ID="SqlDataSourcePublicationTypeConnect" runat="server" 
                SelectCommand="select [PubTypeID], [PubTypeDisplayName], [IsActive] from [PubTypes] WHERE  [PubTypeID] = @PubTypeID"                
                UpdateCommand="UPDATE [PubTypes] SET [PubTypeDisplayName] = @PubTypeDisplayName, [IsActive] = @IsActive WHERE [PubTypeID] = @PubTypeID">
                <SelectParameters>
                    <asp:ControlParameter ControlID="gvPublicationTypes" Name="PubTypeID" 
                        PropertyName="SelectedValue" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="PubTypeDisplayName" />
                    <asp:Parameter Name="IsActive" />
                    <asp:Parameter Name="PubTypeID" />
                </UpdateParameters>
            </asp:SqlDataSource>
            <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                    <div style="float: left;">
                        <asp:Label ID="lblpnlHeader" runat="Server">Add Publication Type</asp:Label></div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel"
                Height="145px" BorderWidth="1">
                <table cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td align="right">
                            <asp:TextBox ID="txtPubTypeID" runat="server" Visible="false" Text="0"></asp:TextBox>
                            Display Name :
                        </td>
                        <td>
                            <asp:TextBox ID="txtPubTypeDisplayName" runat="server" Width="148px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqPubTypeDisplayName" runat="server" ControlToValidate="txtPubTypeDisplayName"
                                ErrorMessage="*"></asp:RequiredFieldValidator>                            
                        </td>
                    </tr> 
                    <tr>
                        <td>
                            Sort Order :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSortingOrder" runat="server" CssClass="formfield" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Is Active :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlActive" runat="server" SelectedValue='<%# Bind("IsActive") %>'>   
                                <asp:ListItem Value="True">Yes</asp:ListItem>   
                                <asp:ListItem Value="False">No</asp:ListItem>   
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button" />
                            <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click" CssClass="button" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

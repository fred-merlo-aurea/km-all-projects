<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true"
    CodeBehind="ResponseGroup.aspx.cs" Inherits="KMPS.MDAdmin.ResponseGroup" %>

<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">

        function ValidateDelete() {

            if (!confirm('Are you sure you want to delete Response Group. It will delete Responsegroup, Codesheet and all mapping for that Responsegroup?')) return false;
            return true;
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: right">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="Images/progress.gif" runat="server" />
                Processing....
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="90%" align="center">
                    <tr>
                        <td id="errorTop"></td>
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
                        <td id="errorBottom"></td>
                    </tr>
                </table>
            </div>
            Product :
            <asp:DropDownList ID="drpPubs" runat="server" AutoPostBack="true"
                DataTextField="PubName" DataValueField="PubID"
                OnSelectedIndexChanged="drpPubs_SelectedIndexChanged" Width="300px">
            </asp:DropDownList>
            &nbsp;&nbsp;
            <br />
            <br />
            <asp:GridView ID="gvGroup" runat="server" AutoGenerateColumns="False" DataKeyNames="ResponseGroupID"
                EnableModelValidation="True" AllowSorting="True" OnSorting="gvGroup_Sorting"
                AllowPaging="True" OnPageIndexChanging="gvGroup_PageIndexChanging" OnRowCommand="gvGroup_RowCommand" OnRowDeleting="gvGroup_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="ResponseGroupName" HeaderText="Group" SortExpression="ResponseGroupName"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DisplayName" HeaderText="Display Name" SortExpression="DisplayName"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Multiple Value" SortExpression="IsMultipleValue" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate><%# (Boolean.Parse(Eval("IsMultipleValue").ToString())) ? "Yes" : "No"%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Required" SortExpression="IsRequired" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate><%# (Boolean.Parse(Eval("IsRequired").ToString())) ? "Yes" : "No"%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" SortExpression="IsActive" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate><%# (Boolean.Parse(Eval("IsActive").ToString())) ? "Yes" : "No"%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField HeaderStyle-Width="5%" ItemStyle-Width="5%" ButtonType="Link"
                        Text="<img src='Images/ic-edit.gif' style='border:none;'>" CommandName="Select"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                    <asp:TemplateField
                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ResponseGroupID")%>' OnClientClick="return ValidateDelete();"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:HyperLinkField Text="Responses" DataNavigateUrlFormatString="CodeSheet.aspx?PubID={0}&ResponseGroupID={1}"
                        DataNavigateUrlFields="PubID,ResponseGroupID">
                        <HeaderStyle Width="5%" />
                    </asp:HyperLinkField>
                </Columns>
            </asp:GridView>
            <br />

            <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                    <div style="float: left;">
                        <asp:Label ID="lblpnlHeader" runat="Server">Add Response Group</asp:Label>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel"
                Height="100%" BorderWidth="1">
                <table cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td align="right">
                            <asp:HiddenField ID="hfResponseGroupID" runat="server" Value="0" />
                            <asp:TextBox ID="txtPubID" runat="server" Visible="false"></asp:TextBox>
                            Group :
                        </td>
                        <td>
                            <asp:TextBox ID="txtResponseGroupName" runat="server" Width="148px" ValidationGroup="save"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqResponseGroupName" runat="server" ControlToValidate="txtResponseGroupName"
                                ErrorMessage="*" ValidationGroup="save"  ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revResponseGroupName" runat="server" ValidationGroup="save"
                            ErrorMessage="<br/>Not a valid group" ControlToValidate="txtResponseGroupName" ForeColor="Red"
                            ValidationExpression="[^(),]*" Display="Dynamic"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Display Name :
                        </td>
                        <td>
                            <asp:TextBox ID="txtDisplayName" runat="server" Width="148px" ValidationGroup="save"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="save"
                            ErrorMessage="<br/>Not a valid display name" ControlToValidate="txtDisplayName" ForeColor="Red"
                            ValidationExpression="[^(),]*" Display="Dynamic"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            MultipleValue 
                        </td>
                        <td>
                            <asp:CheckBox ID="cbIsMultipleValue" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Required 
                        </td>
                        <td>
                            <asp:CheckBox ID="cbIsRequired" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Active 
                        </td>
                        <td>
                            <asp:CheckBox ID="cbIsActive" runat="server" Checked="true" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            KM Product 
                        </td>
                        <td>
                            <asp:DropDownList ID="drpResponseGroupType" runat="server" DataTextField="DisplayName" DataValueField="CodeID"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpResponseGroupType"
                                ErrorMessage="*" ValidationGroup="save" InitialValue="" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button" ValidationGroup="save" />
                            <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click" CssClass="button" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

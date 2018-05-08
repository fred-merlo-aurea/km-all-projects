<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="PubCustomField.aspx.cs" Inherits="KMPS.MD.Administration.PubCustomField" %>

<%@ MasterType VirtualPath="Site.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="gvPubCustomFields" />
        </Triggers>
        <ContentTemplate>
            <div style="text-align: right">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <asp:Image ID="Image1" ImageUrl="Images/progress.gif" runat="server" />
                        Processing....
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="80%" align="center">
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
                <br />
            </div>
            Product: &nbsp;&nbsp;
            <telerik:RadDropDownList ID="rddlProducts" runat="server" DropDownHeight="200px" Width="200px"
                DefaultMessage="Select a product" DropDownWidth="200px"
                DataValueField="PubID" DataTextField="PubName" OnSelectedIndexChanged="rddlProducts_SelectedIndexChanged" AutoPostBack="true">
            </telerik:RadDropDownList>
            <asp:RequiredFieldValidator ID="rfvProducts" runat="server" ControlToValidate="rddlProducts"
                ErrorMessage="*" Font-Bold="false" ValidationGroup="save" ForeColor="Red"></asp:RequiredFieldValidator>
            <br /><br />
            <asp:GridView ID="gvPubCustomFields" runat="server" AutoGenerateColumns="False" DataKeyNames="PubSubscriptionsExtensionMapperId"
                EnableModelValidation="True" AllowSorting="True" OnRowDataBound="gvPubCustomFields_RowDataBound" OnRowDeleting="gvPubCustomFields_RowDeleting"
                AllowPaging="True" OnRowCommand="gvPubCustomFields_RowCommand" RowStyle-BackColor="#EBEBEB" OnPageIndexChanging="gvPubCustomFields_PageIndexChanging" AlternatingRowStyle-BackColor="White" AlternatingRowStyle-ForeColor="#284775" OnSorting="gvPubCustomFields_Sorting" >
                <Columns>
                    <asp:BoundField DataField="CustomField" HeaderText="Name" SortExpression="CustomField"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomFieldDataType" HeaderText="Data Type"
                        ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:CheckBoxField DataField="Active" HeaderText="Active"
                        ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                    </asp:CheckBoxField>
                    <asp:ButtonField HeaderStyle-Width="10%" ItemStyle-Width="10%" ButtonType="Link"
                        Text="<img src='Images/ic-edit.gif' style='border:none;'>" CommandName="Select"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Edit" />
                    <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                        ItemStyle-HorizontalAlign="center"
                        HeaderStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("PubSubscriptionsExtensionMapperId") + "|" + Eval("PubID") %>' OnClientClick="return confirm('Are you sure you want to delete Adhoc?');"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                    <div style="float: left;">
                        <asp:Label ID="lblpnlHeader" runat="Server">Add Custom Fields</asp:Label>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel" Height="100%" BorderWidth="1">
                <table cellspacing="5" cellpadding="5" border="0">
                    <tr>
                        <td align="right">Custom Field Name :
                        </td>
                        <td>
                            <asp:HiddenField ID="hfMapperID" runat="server" Value="0" />
                            <asp:HiddenField ID="hfCreatedUserID" runat="server" />
                            <asp:HiddenField ID="hfCreatedDate" runat="server" />
                            <asp:TextBox ID="txtName" runat="server" Width="148px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqfldName" runat="server" ControlToValidate="txtName"
                                ErrorMessage="*" Font-Bold="false" ValidationGroup="save" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Data Type :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDataType" runat="server">
                                <asp:ListItem Text="String" Value="varchar"></asp:ListItem>
                                <asp:ListItem Text="Int" Value="int"></asp:ListItem>
                                <asp:ListItem Text="Float" Value="float"></asp:ListItem>
                                <asp:ListItem Text="SmallDateTime" Value="smalldatetime"></asp:ListItem>
                                <asp:ListItem Text="DateTime" Value="datetime"></asp:ListItem>
                                <asp:ListItem Text="Bit" Value="bit"></asp:ListItem>
                            </asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                        <td align="right">Active :
                        </td>
                        <td>
                            <asp:CheckBox ID="chkActive" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button" ValidationGroup="save" />
                            <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                                CssClass="button" />
                        </td>
                    </tr>

                    <tr>
                        <td>&nbsp;
                        </td>
                        <td align="center">
                            <asp:Label ID="lblMessage" Visible="false" runat="Server"></asp:Label></td>
                    </tr>

                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

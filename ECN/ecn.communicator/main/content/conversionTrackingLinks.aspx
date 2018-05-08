<%@ Page Language="c#" Inherits="ecn.communicator.contentmanager.conversionTrackingLinks"
    CodeBehind="conversionTrackingLinks.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="msglabel" Visible="false" CssClass="errormsg" Style="width: 750px;
        text-align: left;" runat="Server"></asp:Label>
   <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                                <table cellspacing="0" cellpadding="0" width="674" align="center">
                                    <tr>
                                        <td id="errorTop">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="errorMiddle">
                                            <table height="67" width="80%">
                                                <tr>
                                                    <td valign="top" align="center" width="20%">
                                                        <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
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
    </asp:PlaceHolder>
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'
        align="left">
        <tr>
            <td class="tableHeader" valign="bottom" align='right'>
                <asp:Button ID="UpdateLinksBTN" runat="Server" Text="Check for New Tracking Links"
                    CssClass="formbuttonsmall" OnClick="UpdateLinksBTN_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top" align="left">
                <asp:DataList ID="ConversionTrackingLinksList" runat="Server" DataKeyField="LinkID"
                    GridLines="both" CellPadding="2" BackColor="#FFFFFF">
                    <HeaderTemplate>
                        <tr class="tableHeader1">
                            <td width="15%">
                                Link Name
                            </td>
                            <td width="50%" style="max-width:50%;">
                                Link URL
                            </td>
                            <!--<td width="15%">Link URL Parameters</td>-->
                            <td width="7%" align="center">
                                Active
                            </td>
                            <td width="7%" align="center">
                                Order
                            </td>
                            <td width="11%" align="center">
                            </td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td width="15%" class="tableContent" title='<%# DataBinder.Eval(Container.DataItem, "LinkID")%>'>
                                <%# DataBinder.Eval(Container.DataItem, "LinkName") %>
                            </td>
                            <td width="50%" class="tableContent">
                                <%# DataBinder.Eval(Container.DataItem, "LinkURL") %>
                            </td>
                            <td width="7%" class="tableContent" align="center">
                                <%# DataBinder.Eval(Container.DataItem, "IsActive")  %>
                            </td>
                            <td width="7%" class="tableContent" align="center">
                                <%# DataBinder.Eval(Container.DataItem, "SortOrder") %></asp:Label>
                            </td>
                            <td width="11%" class="tableContent" bgcolor="#eeeeee" align='right'>
                                <asp:LinkButton ID="ConversionLinkEdit" runat="Server" Text="Edit" CommandName="Edit"
                                    CausesValidation="false"></asp:LinkButton>
                                <asp:LinkButton ID="ConversionLinkDelete" runat="Server" Text="Delete" CommandName="Delete"
                                    CausesValidation="false"></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <tr>
                            <td width="15%">
                                <asp:TextBox ID="Edit_LinkNameTXT" runat="Server" Width="150px" class="formfield"></asp:TextBox>
                            </td>
                            <td width="50%">
                                <asp:TextBox ID="Edit_LinkURLTXT" runat="Server" Width="400px" class="formfield"></asp:TextBox>
                            </td>
                            <td width="7%" align="center">
                                <asp:CheckBox ID="Edit_IsActiveCHKBX" runat="Server"></asp:CheckBox>
                            </td>
                            <td width="7%" align="center">
                                <asp:TextBox ID="Edit_SortOrderTXT" runat="Server" Width="26px" class="formfield"
                                    MaxLength="2"></asp:TextBox>
                            </td>
                            <td width="11%" align='right'>
                                <asp:LinkButton runat="Server" Text="Update" CommandName="Update" CausesValidation="false"
                                    ID="UpdateLinkBTN"></asp:LinkButton>
                                <asp:LinkButton runat="Server" Text="Cancel" CommandName="Cancel" CausesValidation="false"
                                    ID="CancelLinkBTN"></asp:LinkButton>
                            </td>
                        </tr>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <tr>
                            <td width="15%" bgcolor="#eeeeee">
                                <asp:TextBox ID="Add_LinkNameTXT" runat="Server" Width="150px" class="formfield"></asp:TextBox>
                            </td>
                            <td width="50%" bgcolor="#eeeeee">
                                <asp:TextBox ID="Add_LinkURLTXT" runat="Server" Width="400px" class="formfield"></asp:TextBox>
                            </td>
                            <td width="7%" align="center" bgcolor="#eeeeee">
                                <asp:CheckBox ID="Add_IsActiveCHKBX" runat="Server"></asp:CheckBox>
                            </td>
                            <td width="7%" align="center" bgcolor="#eeeeee">
                                <asp:TextBox ID="Add_SortOrderTXT" runat="Server" Width="26px" MaxLength="2" class="formfield"></asp:TextBox>
                            </td>
                            <td width="11%" align='right' bgcolor="#eeeeee">
                                <asp:LinkButton ID="AddLinkBTN" Text="Add" OnClick="ConversionLinkAdd_Click" runat="Server"></asp:LinkButton>
                            </td>
                        </tr>
                    </FooterTemplate>
                </asp:DataList>
            </td>
        </tr>
    </table>
</asp:Content>

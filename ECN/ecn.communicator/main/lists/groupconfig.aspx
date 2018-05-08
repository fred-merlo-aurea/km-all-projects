<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.groupconfig"
    CodeBehind="groupconfig.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />

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
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" border='0'>
        <tbody>
            <tr align="left">
                <td class="tableHeader" align='left' height="16">
                    &nbsp;<span class="label">ShortName&nbsp;</span>
                </td>
                <td valign="top" height="16" align="left">
                    <asp:TextBox ID="txtShortName" runat="server"></asp:TextBox>
                </td>
                <td>
                        &nbsp; &nbsp;<asp:CheckBox ID="isPublicChkbox" runat="Server" Enabled="true"></asp:CheckBox>Is Public &nbsp;
                </td>
                  <td valign="top" height="16" align="left">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click"/>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                      <ecnCustom:ecnGridView  ID="gvGroupConfig" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                            Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="GroupConfigID"  CssClass="grid" 
                            OnRowCommand="gvGroupConfig_RowCommand">
                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false" HeaderText="Field Values" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGroupConfigID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.GroupConfigID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>    
                                      <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="true" HeaderText="User Defined Fields " HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ShortName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>      
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="true" HeaderText="IsPublic" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsPublic" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.IsPublic") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                               
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnGroupConfigDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                                CommandName="GroupConfigDelete" OnClientClick="return confirm('Are you sure, you want to delete this UDF?')"
                                                CausesValidation="false" CommandArgument='<%#Eval("GroupConfigID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                  </ecnCustom:ecnGridView>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

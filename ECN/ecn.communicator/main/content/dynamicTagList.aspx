<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dynamicTagList.aspx.cs" Inherits="ecn.communicator.main.content.dynamicTagList" MasterPageFile="~/MasterPages/Communicator.Master" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br /> <br />
    <table width="100%">
        <tr>
            <td align="left">
                 &nbsp; 
            </td>
            <td align="right">
                <asp:Button ID="btnManageRules" runat="server" Text="Manage Rules"  class="formbuttonsmall" OnClick="btnManageRules_Click" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnAddDynamicTag" runat="server" Text="Add Dynamic Tag"  class="formbuttonsmall" OnClick="btnAddDynamicTag_Click" />
            </td>
        </tr>

    </table>
   
   <br />
    <ecnCustom:ecnGridView ID="gvDynamicTag" runat="server" AllowSorting="false" AutoGenerateColumns="false"  CssClass="grid" 
                            Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="DynamicTagID"
                            OnRowCommand="gvDynamicTag_RowCommand"><HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDynamicTagID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.DynamicTagID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="70%" HeaderText="Tag" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                               <asp:Label ID="lblDynamicTag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Tag") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Created Date">
                                        <ItemTemplate>
                                               <asp:Label ID="lblCreatedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CreatedDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                           <a href='DynamicTagedit.aspx?DynamicTagID=<%# DataBinder.Eval(Container, "DataItem.DynamicTagID") %>'>
                                                <center><img src="/ecn.images/images/icon-edits1.gif" alt='Edit Filter' border='0'></center>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                                CommandName="DynamicTagDelete" OnClientClick="return confirm('Are you sure you want to delete this Dynamic Tag?')"
                                                CausesValidation="false" CommandArgument='<%#Eval("DynamicTagID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
        
                                <AlternatingRowStyle CssClass="gridaltrow" />
           </ecnCustom:ecnGridView>
    <br />
</asp:Content>
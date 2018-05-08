<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ruleList.aspx.cs" Inherits="ecn.communicator.main.content.ruleList" MasterPageFile="~/MasterPages/Communicator.Master" %>
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
                <asp:Button ID="btnAddRule" runat="server" Text="Add New Rule"  class="formbuttonsmall" OnClick="btnAddRule_Click" />
            </td>
        </tr>

    </table>
   
   <br />
    <ecnCustom:ecnGridView ID="gvRule" runat="server" AllowSorting="false" AutoGenerateColumns="false"  CssClass="grid" 
                            Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="RuleID"
                            OnRowCommand="gvRule_RowCommand"><HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRuleID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.RuleID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="70%" HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                               <asp:Label ID="lblRuleName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.RuleName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Created Date" >
                                        <ItemTemplate>
                                         <asp:Label ID="lblCreatedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CreatedDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                           <a href='RuleEdit.aspx?RuleID=<%# DataBinder.Eval(Container, "DataItem.RuleID") %>'>
                                                <center><img src="/ecn.images/images/icon-edits1.gif" alt='Edit Filter' border='0'></center>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                                CommandName="RuleDelete" OnClientClick="return confirm('Are you sure you want to delete this rule?')"
                                                CausesValidation="false" CommandArgument='<%#Eval("RuleID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
        
                                <AlternatingRowStyle CssClass="gridaltrow" />
           </ecnCustom:ecnGridView>
    <br />
</asp:Content>
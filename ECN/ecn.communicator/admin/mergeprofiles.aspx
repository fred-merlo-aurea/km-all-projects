<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mergeprofiles.aspx.cs" Inherits="ecn.communicator.admin.mergeprofiles" MasterPageFile="~/MasterPages/Communicator.Master"%>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

     <br />
    <table>
        <tr>
             <td>
                Base Channel
            </td>
             <td>
                 <asp:DropDownList ID="drpBaseChannel" runat="server"></asp:DropDownList>
            </td>
            <td>
                From Email Address
            </td>
            <td>
                <asp:TextBox ID="txtFromEmailAddress" runat="server"></asp:TextBox>
            </td>
             <td>
                To Email Address
            </td>
            <td>
                <asp:TextBox ID="txtToEmailAddress" runat="server"></asp:TextBox>
            </td>
             <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"/>
            </td>
        </tr>
         <tr valign="top">
              <td colspan="2">
                <asp:Label ID="lblMergeAll" runat="server" Text="Accounts Processed"></asp:Label><br />
              <ecnCustom:ecnGridView  ID="gvAllCustomer" runat="server" AllowSorting="false" AutoGenerateColumns="false" ShowEmptyTable="false"
                            Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="CustomerID"  CssClass="grid" >
                  <EmptyDataTemplate>
                      <asp:Label ID="lblCustomer" runat="server" Text="No customer accounts found"></asp:Label>
                  </EmptyDataTemplate>
                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <Columns>
                                      <asp:TemplateField ItemStyle-HorizontalAlign="Left" Visible="true" HeaderText="CustomerName" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Customer") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>    
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" Visible="false" HeaderText="CustomerID" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CustomerID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                </Columns>
                  </ecnCustom:ecnGridView>
           </td>
             <td colspan="2">
                 <asp:Label ID="lblMergeAuto" runat="server" Text="Accounts Updated"></asp:Label><br />
              <ecnCustom:ecnGridView  ID="gvReplaceCustomer" runat="server" AllowSorting="false" AutoGenerateColumns="false" ShowEmptyTable="false"
                            Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="CustomerID"  CssClass="grid" >
                  <EmptyDataTemplate>
                      <asp:Label ID="lblCustomer" runat="server" Text="No customer accounts were updated"></asp:Label>
                  </EmptyDataTemplate>
                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <Columns>
                                      <asp:TemplateField ItemStyle-HorizontalAlign="Left" Visible="true" HeaderText="CustomerName" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Customer") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>    
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" Visible="false" HeaderText="CustomerID" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CustomerID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                </Columns>
                  </ecnCustom:ecnGridView>
           </td>
           <td colspan="3">
                <asp:Label ID="lblMergeManual" runat="server" Text="Please Review"></asp:Label><br />
              <ecnCustom:ecnGridView  ID="gvMergeCustomer" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                            Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="CustomerID"  CssClass="grid" ShowEmptyTable="false"
                            OnRowCommand="gvMergeCustomer_RowCommand">
                    <EmptyDataTemplate>
                      <asp:Label ID="lblCustomer" runat="server" Text="No customer accounts to review"></asp:Label>
                  </EmptyDataTemplate>
                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <Columns>
                                      <asp:TemplateField ItemStyle-HorizontalAlign="Left" Visible="true" HeaderText="CustomerName" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Customer") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>    
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" Visible="false" HeaderText="CustomerID" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CustomerID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>    
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" Visible="false" HeaderText="OldEmailID" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOldEmailID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.OldEmailID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>    
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" Visible="false" HeaderText="NewEmailID" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNewEmailID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.NewEmailID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                         
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnMergeProfile" runat="server" ImageUrl="/ecn.images/images/icon-edit1.gif"
                                                CommandName="Merge" CausesValidation="false" CommandArgument='<%#Eval("CustomerID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                  </ecnCustom:ecnGridView>
           </td>
        </tr>
        <tr>
           
        </tr>
    </table>
</asp:Content>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlastFieldsConfig.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.OtherControls.BlastFieldsConfig" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<table>
    <tr>        
        <td>
            Field Name
        </td>
        <td>
            <asp:TextBox ID="txtBlastFieldsCustomName" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblBlastFieldsNameMessage" runat="server" Font-Size="Small" ForeColor="Red" Visible="false" />
        </td>
    </tr>
    <tr>
        <td>
            Please Enter a Value
        </td>
         <td>
              <asp:TextBox ID="txtBlastFieldsValue" runat="server"></asp:TextBox>&nbsp;&nbsp;
               <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-add.gif alt='Add Value' border='0'&gt;"
                                                                    CausesValidation="false" ID="btnAddBlastFieldsValue"  OnClick="btnAddBlastFieldsValue_Click"></asp:LinkButton>
        </td>
    </tr>
    <tr>
            <td colspan="2">
                <ecnCustom:ecnGridView  ID="gvBlastFieldsValue" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                            Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="BlastFieldsValueID"  CssClass="grid" 
                            OnRowCommand="gvBlastFieldsValue_RowCommand">
                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="true" HeaderText="Field Values" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Value") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                   
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                                CommandName="ValueDelete" OnClientClick="return confirm('Are you sure, you want to delete this Value?')"
                                                CausesValidation="false" CommandArgument='<%#Eval("BlastFieldsValueID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                  </ecnCustom:ecnGridView>
            </td>
    </tr>
</table>
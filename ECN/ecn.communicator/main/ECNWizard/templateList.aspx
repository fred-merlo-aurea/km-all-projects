<%@ Page Language="c#" Inherits="ecn.communicator.main.ECNWizard.templateList" CodeBehind="templateList.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <br /> <br />
    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
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
                        <td id="errorBottom"></td>
                    </tr>
                </table>
            </asp:PlaceHolder>
    <table width="100%">
        <tr>
            <td align="left">
                 &nbsp;
            </td>
            <td align="right">
                <asp:DropDownList ID="ddlArchiveFilter" OnSelectedIndexChanged="ddlArchiveFilter_SelectedIndexChanged" AutoPostBack="true" runat="server">
                    <asp:ListItem Text="Active" Value="active" Selected="True" />
                    <asp:ListItem Text="All" Value="all" />
                    <asp:ListItem Text="Archived" Value="archived" />
                </asp:DropDownList>
                &nbsp;
                <asp:Button ID="btnAdd" runat="server" Text="Add"  class="formbuttonsmall" OnClick="btnAdd_Click" />
            </td>
        </tr>

    </table> <br />
     <ecnCustom:ecnGridView ID="gvCampaignItemTemplate" runat="server" AllowSorting="false" AutoGenerateColumns="false"  CssClass="grid" 
                            Width="100%" AllowPaging="false" OnRowDataBound="gvCampaignItemTemplate_RowDataBound" ShowFooter="false" DataKeyNames="CampaignItemTemplateID"
                            OnRowCommand="gvCampaignItemTemplate_RowCommand"><HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCampaignItemTemplateID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CampaignItemTemplateID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="TemplateName">
                                        <ItemTemplate>
                                               <asp:Label ID="lblTemplateName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TemplateName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="CreatedDate">
                                        <ItemTemplate>
                                               <asp:Label ID="lblCreatedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.CreatedDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                       <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="UpdatedDate">
                                        <ItemTemplate>
                                               <asp:Label ID="lblUpdatedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.UpdatedDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                           <a href='templateEdit.aspx?CampaignItemTemplateID=<%# DataBinder.Eval(Container.DataItem, "CampaignItemTemplateID") %>'>
                                                <center><img src="/ecn.images/images/icon-edits1.gif" alt='Edit Filter' border='0'></center>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                                CommandName="CampaignItemTemplateDelete" OnClientClick="return confirm('Are you sure, you want to delete this CampaignItem Template?')"
                                                CausesValidation="false" CommandArgument='<%#Eval("CampaignItemTemplateID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Archived" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsArchived" runat="server" OnCheckedChanged="chkIsArchived_CheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

           </ecnCustom:ecnGridView>
</asp:Content>

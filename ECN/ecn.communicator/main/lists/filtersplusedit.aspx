<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="filtersplusedit.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" Inherits="ecn.communicator.listsmanager.filtersplusedit1" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
    
<%@ Register src="~/main/ECNWizard/Group/filters.ascx" tagname="filters" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <uc1:filters ID="filters1" runat="server" />
 <table width="100%">
    <tr>
                    <td class="tableHeader" align="center">
                        &nbsp;
                        <asp:Button ID="btnReturn" runat="server" Text="Return to Filters list" class="formbuttonsmall"
                            Width="180px" OnClick="btnReturn_Click" />
                        &nbsp;
                    </td>
    </tr>
 </table>

 </asp:Content>

<%@ Page Language="c#" Inherits="ecn.communicator.main.ECNWizard.templateEdit" CodeBehind="templateEdit.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>
<%@ Register Src="~/main/ECNWizard/OtherControls/AddTemplate.ascx" TagName="addTemplate" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width:100%">
        <tr>
            <td>
                  <uc1:addTemplate ID="addTemplate1" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center">
                   <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
            </td>
        </tr>

    </table>   
</asp:Content>

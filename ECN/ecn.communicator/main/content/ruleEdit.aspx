<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ruleEdit.aspx.cs" Inherits="ecn.communicator.main.content.ruleEdit" MasterPageFile="~/MasterPages/Communicator.Master"%>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register src="~/main/ECNWizard/OtherControls/ruleEditor.ascx" tagname="ruleEditor" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <link rel='stylesheet' href="../../MasterPages/ECN_MainMenu.css" type="text/css" />
    <link rel='stylesheet' href="../../MasterPages/ECN_Controls.css" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <table width="100%">
    <tr>
        <td>
          <uc1:ruleEditor ID="ruleEditor1" runat="server" />
        </td>  
    </tr>
    <tr align="center" >
        <td>
            <br />
            <asp:Button ID="btnSave" OnClick="btnSave_Click" Visible="true" Text="Save" class="ECN-Button-Medium" runat="Server"/>
        </td>
    </tr>
</table>   

</asp:Content>
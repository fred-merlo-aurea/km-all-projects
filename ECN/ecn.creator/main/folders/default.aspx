<%@ Page Language="c#" Inherits="ecn.creator.folders.folderseditor" CodeBehind="default.aspx.cs" MasterPageFile="~/Creator.Master" %>

<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>
<%@ Register TagPrefix="ecn" TagName="FolderSystem" Src="../../includes/folderSystem.ascx" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function deleteFolder(theID) {
            if (confirm('Are you Sure?\n Selected Folder will be permanently deleted.')) {
                window.location = "folderseditor.aspx?folderID=" + theID + "&action=delete";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
        <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="450" border='0'>
            <tbody>
                <tr>
                    <td class="tableHeader" width="140">Folder Type:&nbsp;</td>
                    <td>
                        <asp:DropDownList class="formfield" ID="FolderTypeDR" runat="Server" Visible="true"
                            EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="FolderTypeDR_SelectedIndexChanged">
                            <asp:ListItem Selected="true" Value="GRP">List Group Folders</asp:ListItem>
                            <asp:ListItem Value="CNT">List Content / Page Folders</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" width="140">Folder Name:&nbsp;</td>
                    <td align="left">
                        <asp:TextBox class="formfield" ID="txtFolderName" Width="168px" runat="Server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="FolderNameValidator" runat="Server" Display="Dynamic"
                            ControlToValidate="txtFolderName" ErrorMessage="* required" CssClass="errormsg"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" nowrap width="140">Folder Description:</td>
                    <td>
                        <asp:TextBox class="formfield" ID="txtFolderDesc" runat="Server" Enabled="true" TextMode="multiline"
                            Rows="3" Columns="51" Wrap="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan='3' align='right' class="tableHeader">
                        <asp:Button class="formfield" ID="DetailsButton" runat="Server" Text="Folder Details"
                            CausesValidation="False" Enabled="False" OnClick="DetailsButton_Click"></asp:Button>
                        &nbsp;&nbsp;|&nbsp;&nbsp;
                    <asp:Button class="formfield" ID="AddButton" runat="Server" Text="Add" OnClick="AddButton_Click"></asp:Button>
                        &nbsp;
                    <asp:Button class="formfield" ID="EditButton" runat="Server" Text="Edit" Enabled="False"
                        OnClick="EditButton_Click"></asp:Button>
                        &nbsp;
                    <asp:Button class="formfield" ID="DeleteButton" runat="Server" Text="Delete" Enabled="False"
                        CausesValidation="False" OnClick="DeleteButton_Click"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" align="center" colspan="2">
                        <asp:Label ID="FolderError" runat="Server" CssClass="errormsg" Visible="false" />
                        <br />
                        <hr size="1" color='#000000' width="100%">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align='right'>
                        <ecn:FolderSystem ID="FolderControl" runat="Server" Width="467" Height="105" CssClass="formfield"></ecn:FolderSystem>
                    </td>
                </tr>
            </tbody>
        </table>
</asp:Content>


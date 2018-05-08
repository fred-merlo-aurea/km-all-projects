<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.communicator.contentmanager.MessageTypeSort"
    CodeBehind="MessageTypeSort.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript">
        window.setTimeout("window.open('timeout.htm','Timeout', 'left=100,top=100,height=250,width=300,resizable=no,scrollbar=no,status=no' )", 1500000);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <table cellspacing="5" cellpadding="5" border="0">
                <tr>
                    <td valign="top" align="right" class="label">
                        Message Types :
                    </td>
                    <td>
                        <asp:ListBox ID="lstSourceFields" runat="server" Rows="10" Style="text-transform: uppercase;"
                            SelectionMode="Multiple" CssClass="formfield" Width="200px" DataTextField="Name"
                            DataValueField="MessageTypeID"></asp:ListBox>
                    </td>
                    <td>
                        <td>
                            <asp:Button ID="btnUp" runat="server" Text="Move Up" CssClass="button" OnClick="btnUp_Click"
                                Width="100px" />
                            <br>
                            <br>
                            <asp:Button ID="btnDown" runat="server" CssClass="button" OnClick="btndown_Click"
                                Text="Move Down" Width="100px" />
                        </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td align="center">
                        <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button"
                            Width="80px" />
                        <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                            Width="80px" CssClass="button" />
                    </td>
                </tr>
            </table>
</asp:Content>

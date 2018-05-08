<%@ Page Language="C#" ValidateRequest="false" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="PaidPub.main.Forms.Add" %>

<%@ Register TagPrefix="FredCK" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
   
    <script type="text/javascript">

        function moveItem(i) {
            var listObj;
            var selIndex;
            var selValu;
            var selText;

            listObj = document.getElementById("ctl00_Content_lstSelectedNewsLetters");
            selIndex = listObj.selectedIndex;
            
            if (selIndex + i < 0 || selIndex + i > listObj.options.length - 1) { return; }

            selValue = listObj.options[selIndex].value;
            selText = listObj.options[selIndex].text;

            listObj.options[selIndex].value = listObj.options[selIndex + i].value;
            listObj.options[selIndex].text = listObj.options[selIndex + i].text;
            
            listObj.options[selIndex + i].value = selValue;
            listObj.options[selIndex + i].text = selText;

            listObj.selectedIndex = selIndex + i;

        } 
    </script>

    <div class="contentheader">
        Subscription Forms
    </div>
    <br />
    <div class="padding20">
        <div class="box">
            <div class="boxheader">
                Add/Edit Subscription Forms</div>
            <div class="boxcontent">
                <table cellpadding="5" cellspacing="0" border="0">
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left">
                            Form Name :
                        </td>
                        <td width="50%" align="left">
                            <asp:TextBox ID="txtformname" runat="server" Width="400" MaxLength="50"></asp:TextBox>
                        </td>
                        <td width="20%" align="left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtformname"
                                ErrorMessage="&gt;&gt; requried" Font-Bold="True" Font-Size="X-Small"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left">
                            Status :
                        </td>
                        <td width="50%" align="left">
                            <asp:RadioButtonList ID="rbStatus" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="1">Active</asp:ListItem>
                                <asp:ListItem Value="0">Inactive</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td width="20%" align="left">
                            &nbsp;
                        </td>
                    </tr>
                     <tr>
                        <td width="20%" align="left">
                            Form Type :
                        </td>
                        <td width="50%" align="left">
                            <asp:RadioButtonList ID="rbFormType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="0">Paid</asp:ListItem>
                                <asp:ListItem Value="1">Trial</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td width="20%" align="left">
                            &nbsp;
                        </td>
                    </tr>
                    <asp:Panel ID="pnlURL" runat="server" Visible="false">
                        <tr>
                            <td width="20%" align="left" valign="top">
                                Form URL :
                            </td>
                            <td width="50%" align="left">
                                <asp:TextBox ID="txtURL" ReadOnly="true" runat="server" Width="400"></asp:TextBox>
                            </td>
                            <td width="20%" align="left">
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td width="20%" align="left" valign="top">
                            Header :
                        </td>
                        <td width="50%" align="left">
                            <FredCK:FCKeditor ID="HeaderHTML" runat="server" BasePath="/ecn.editor/" ToolbarSet="Basic"
                                Width="790" Height="200">
                            </FredCK:FCKeditor>
                        </td>
                        <td width="20%" align="left">
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left" valign="top">
                            Step 1 Description :
                        </td>
                        <td width="50%" align="left">
                            <FredCK:FCKeditor ID="DescHTML" runat="server" BasePath="/ecn.editor/" ToolbarSet="Basic"
                                Width="790" Height="200">
                            </FredCK:FCKeditor>
                        </td>
                        <td width="20%" align="left">
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left" valign="top">
                            eNewletter Description :
                        </td>
                        <td width="50%" align="left">
                            <FredCK:FCKeditor ID="newsletterHTML" runat="server" BasePath="/ecn.editor/" ToolbarSet="Basic"
                                Width="790" Height="200">
                            </FredCK:FCKeditor>
                        </td>
                        <td width="20%" align="left">
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left" valign="top">
                            NewsLetters :
                        </td>
                        <td colspan="2" align="left">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table cellpadding="5" cellspacing="0" border="0">
                                       <tr>
                                            <td valign="middle" width="40%">
                                            <b>Available Newsletters</b>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td valign="middle" width="40%">
                                            <b>Selected Newsletters</b>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" width="40%">
                                                <asp:ListBox ID="lstNewsletters" SelectionMode="Multiple" runat="server" Width="300"
                                                    Rows="10"></asp:ListBox>
                                            </td>
                                            <td valign="middle" align="middle" width="10%">
                                                <asp:Button ID="btnadd" Text=" >> " runat="server" OnClick="btnadd_Click" CausesValidation="false" /><br />
                                                <br />
                                                <asp:Button ID="btndelete" Text=" << " runat="server" OnClick="btndelete_Click" CausesValidation="false" />
                                            </td>
                                            <td valign="middle" width="40%">
                                                <asp:ListBox ID="lstSelectedNewsLetters" SelectionMode="Multiple" runat="server" Width="300"
                                                    Rows="10"></asp:ListBox>
                                            </td>
                                            <td valign="middle" width="10%" align="middle">
                                               <asp:imagebutton ID="btnmoveup" ImageUrl="~/images/arrowup.gif" runat="server" 
                                                    onclick="btnmoveup_Click" BorderWidth="1px"  />
                                                <br />
                                                <br />
                                                <asp:imagebutton ID="btnmovedown" ImageUrl="~/images/arrowdown.gif" 
                                                    runat="server" onclick="btnmovedown_Click" BorderWidth="1px"/>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left" valign="top">
                            Footer :
                        </td>
                        <td width="50%" align="left">
                            <FredCK:FCKeditor ID="FooterHTML" runat="server" BasePath="/ecn.editor/" ToolbarSet="Basic"
                                Width="790" Height="200">
                            </FredCK:FCKeditor>
                        </td>
                        <td width="20%" align="left">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            <br />
                            <asp:Label ID="lblErrorMessage" runat="server" Visible="false" CssClass="error"></asp:Label>
                            <br />
                            <asp:Button CssClass="blackButton" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click">
                            </asp:Button>&nbsp;
                            <asp:Button CssClass="blackButton" ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                OnClick="btnCancel_Click"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

<%@ Control Language="c#" Inherits="ecn.communicator.main.SMSWizard.Controls.ContentInfo" Codebehind="ContentInfo.ascx.cs" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:PlaceHolder ID="plCreate" Visible="false" runat="server">
    <div class="section">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td class="formLabel">
                    Select one of the content options below.</td>
            </tr>
            <tr>
                <td width="100%" class="formLabel">
                    <asp:RadioButton ID="rbNewContent" GroupName="grpSelect" Text="Create New Content"
                        runat="Server" AutoPostBack="true" Checked="True" CssClass="expandAccent" OnCheckedChanged="rbNewContent_CheckedChanged">
                    </asp:RadioButton></td>
            </tr>
            <asp:PlaceHolder ID="plNewContent" Visible="true" runat="server">
                <tr>
                    <td style="padding-left: 30px">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td class="formLabel" align="left" width="15%">
                                    Save&nbsp;in&nbsp;Folder:&nbsp;</td>
                                <td class="dataOne" align="left" width="85%">
                                    <asp:DropDownList ID="drpFolder1" CssClass="label10" runat="server" DataTextField="FolderName"
                                        DataValueField="FolderID" EnableViewState="true">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="formLabel" align="left">
                                    Content Title:&nbsp;</td>
                                <td class="dataOne" align="left">
                                    <asp:TextBox ID="txtContentTitle" CssClass="label10" runat="server" Width="200" MaxLength="50"></asp:TextBox>&nbsp;
                                    <asp:RequiredFieldValidator ID="rfvtxtContentTitle" runat="server" Font-Size="xx-small"
                                        ControlToValidate="txtContentTitle" ErrorMessage="« required" Font-Italic="True"
                                        Font-Bold="True"></asp:RequiredFieldValidator></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </asp:PlaceHolder>
        </table>
    </div>
</asp:PlaceHolder>
<div class="section">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td width="100%" class="formLabel">
                <asp:RadioButton ID="rbExistingContent" CssClass="expandAccent" AutoPostBack="true"
                    runat="Server" Text="Use Existing Content" GroupName="grpSelect" OnCheckedChanged="rbExistingContent_CheckedChanged">
                </asp:RadioButton></td>
        </tr>
        <asp:PlaceHolder ID="plExistingContent" runat="server" Visible="false">
            <tr>
                <td style="padding-left: 30px">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="formLabel" align="left" width="15%">
                                Select&nbsp;Folder:&nbsp;</td>
                            <td class="dataOne" align="left" width="85%">
                                <asp:DropDownList ID="drpFolder" AutoPostBack="true" CssClass="label10" runat="server"
                                    DataTextField="FolderName" DataValueField="FolderID" EnableViewState="true" OnSelectedIndexChanged="drpFolder_SelectedIndexChanged">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="formLabel" align="left">
                                Select&nbsp;Content:&nbsp;</td>
                            <td class="dataOne" align="left">
                                <asp:DropDownList ID="drpContent" AutoPostBack="true" CssClass="label10" runat="server"
                                    OnSelectedIndexChanged="drpContent_SelectedIndexChanged">
                                </asp:DropDownList>&nbsp;
                                <asp:RequiredFieldValidator ID="rfvdrpContent" runat="server" Font-Size="xx-small"
                                    ControlToValidate="drpContent" ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </asp:PlaceHolder>
    </table>
</div>
<div class="section">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">     
        <tr>
            <td class="headingTwo" valign="top" colspan="2" style="padding-top: 10px;">
                Message:&nbsp;</td>
        </tr>
        
      
        <tr>
            <td class="tableContent" colspan="2" align="center">
                <br/>
                <asp:TextBox ID="ContentText" CssClass="formfield" runat="server" EnableViewState="true"
                    Width="790" TextMode="multiline" Columns="126" Rows="15"></asp:TextBox><br/>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
	runat="server" ControlToValidate="ContentText" 
	ErrorMessage="* Max Length: 145 Characters"
 SetFocusOnError="true" ValidationExpression="^[\s\S]{0,145}$">
	</asp:RegularExpressionValidator>      <br/>
                <asp:Label ID="Label1" runat="server" CssClass="formLabel" Text="Max Length: 145 Characters"></asp:Label>
                     </td>
        </tr>
          <tr>
            <td class="headingTwo" valign="top" colspan="2" style="padding-top: 10px;">
               Auto Welcome Message:&nbsp;</td>
        </tr>
          <tr>
            <td class="tableContent" colspan="2" align="center">
                <br/>
                <asp:TextBox ID="WelcomeText" CssClass="formfield" runat="server" EnableViewState="true"
                    Width="790" TextMode="multiline" Columns="126" Rows="15"></asp:TextBox><br/>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" 
	runat="server" ControlToValidate="WelcomeText" 
	ErrorMessage="* Max Length: 145 Characters"
 SetFocusOnError="true" ValidationExpression="^[\s\S]{0,145}$">
	</asp:RegularExpressionValidator>      <br/>
                <asp:Label ID="Label2" runat="server" CssClass="formLabel" Text="Max Length: 145 Characters"></asp:Label>
                     </td>
        </tr>
        
    </table>
</div>

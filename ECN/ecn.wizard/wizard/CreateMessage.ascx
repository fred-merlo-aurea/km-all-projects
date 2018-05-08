<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="CreateMessage.ascx.cs"
    Inherits="ecn.wizard.wizard.CreateMessage" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<script language="javascript">
	function SelectedLogo(HTMLfilectrl)
	{
		file = HTMLfilectrl.value;
		if (file == '')
		{
			document.getElementById("ECNWizard_HeaderImg").value = "";
		} 
		else
		{
			while (file.indexOf("\\") != -1)
				file = file.slice(file.indexOf("\\") + 1);

			if (!ValidateExtension(file))
				alert("Please only upload image files that end in types:  " + (extArray.join("  ")));

			document.getElementById("ECNWizard_HeaderImg").value = file;
		}
	}
	
	function ValidateExtension(file)
	{
		extArray = new Array(".gif", ".jpg", ".png");

		allowSubmit = false;
			
		ext = file.slice(file.indexOf(".")).toLowerCase();
		
		for (var i = 0; i < extArray.length; i++) 
		{
			if (extArray[i] == ext) { allowSubmit = true; break; }
		}
		
		return allowSubmit;
	}
	
</script>

<!--content-->
<div style="padding-right: 20px; font-size: 12px; padding-bottom: 10px; padding-top: 10px;">
    <div>
        Please complete all the provided fields before you continue on to step 3.<br>
        *All Fields are required with the exception of the Phone Number.</div>
    <br>
    <div>
        <asp:Label ID="lblMsg" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label></div>
    <div class="dashed_lines1" style="padding-right: 0px; padding-bottom: 0px; padding-top: 10px">
        <strong>Please enter a message name.</strong></div>
    <div style="padding-right: 0px; padding-left: 40px; padding-bottom: 0px; padding-top: 10px">
        *Save Message As:<br>
        <asp:TextBox ID="msgTitle" CssClass="blue_border_box" runat="server"></asp:TextBox><asp:RequiredFieldValidator
            ID="RequiredFieldValidator1" runat="server" ErrorMessage="«Required" ControlToValidate="msgTitle"></asp:RequiredFieldValidator></div>
    <div class="dashed_lines1" style="padding-right: 0px; padding-top: 10px; p: 0px">
        <strong>Email Information (these will appear in the recepient’s email folder)</strong></div>
    <asp:Panel ID="pnlfromaddress" runat="server">
        <div style="padding-right: 0px; padding-left: 40px; padding-bottom: 0px; padding-top: 10px">
            *From Email Address:<br>
            <asp:TextBox ID="email" runat="server" CssClass="blue_border_box"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="email"
                ErrorMessage="«Required"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="email"
                ErrorMessage="«Not Valid" ValidationExpression=".*@.*\..*"></asp:RegularExpressionValidator></div>
    </asp:Panel>
    <div style="padding-right: 0px; padding-left: 40px; padding-bottom: 0px; padding-top: 10px">
        *From Name:<br>
        <asp:TextBox ID="name" CssClass="blue_border_box" runat="server"></asp:TextBox><asp:RequiredFieldValidator
            ID="RequiredFieldValidator3" runat="server" ErrorMessage="«Required" ControlToValidate="name"></asp:RequiredFieldValidator></div>
    <div style="padding-right: 0px; padding-left: 40px; padding-bottom: 0px; padding-top: 10px">
        *Email Subject:<br>
        <asp:TextBox ID="emailSubject" CssClass="blue_border_box" runat="server"></asp:TextBox><asp:RequiredFieldValidator
            ID="RequiredFieldValidator4" runat="server" ErrorMessage="«Required" ControlToValidate="emailSubject"></asp:RequiredFieldValidator></div>
    <div class="dashed_lines1" style="padding-right: 0px; padding-bottom: 0px; padding-top: 10px">
        <strong>Content Information (this info. will appear in the text of the message)</strong></div>
    <div style="padding-right: 0px; padding-left: 20px; padding-bottom: 0px; padding-top: 10px">
        <strong>Salutation</strong></div>
    <div style="padding-right: 0px; padding-left: 40px; padding-bottom: 0px; padding-top: 10px">
        <input type="radio" checked name="salutation" id="rblFirst" runat="server" value="firstname">&nbsp;First
        Name Only&nbsp;&nbsp;<input type="radio" name="salutation" id="rblFirstLast" runat="server"
            value="firstlast">&nbsp;First and Last Name</div>
    <div style="padding-right: 0px; padding-left: 20px; padding-bottom: 0px; padding-top: 10px">
        <strong>Footer Information</strong></div>
    <div style="padding-right: 0px; padding-left: 40px; padding-bottom: 0px; padding-top: 10px">
        *From Name:<br>
        <asp:TextBox ID="footerName" CssClass="blue_border_box" runat="server"></asp:TextBox><asp:RequiredFieldValidator
            ID="RequiredFieldValidator5" runat="server" ErrorMessage="«Required" ControlToValidate="footerName"></asp:RequiredFieldValidator></div>
    <div style="padding-right: 0px; padding-left: 40px; padding-bottom: 0px; padding-top: 10px">
        *Title:<br>
        <asp:TextBox ID="footerTitle" CssClass="blue_border_box" runat="server"></asp:TextBox><asp:RequiredFieldValidator
            ID="RequiredFieldValidator6" runat="server" ErrorMessage="«Required" ControlToValidate="footerTitle"></asp:RequiredFieldValidator></div>
    <div style="padding-right: 0px; padding-left: 40px; padding-bottom: 0px; padding-top: 10px">
        *Company Name:<br>
        <asp:TextBox ID="footerCompany" CssClass="blue_border_box" runat="server"></asp:TextBox><asp:RequiredFieldValidator
            ID="RequiredFieldValidator7" runat="server" ErrorMessage="«Required" ControlToValidate="footerCompany"></asp:RequiredFieldValidator></div>
    <div style="padding-right: 0px; padding-left: 40px; padding-bottom: 0px; padding-top: 10px">
        Phone Number (optional):<br>
        <asp:TextBox ID="footerPhone" CssClass="blue_border_box" runat="server"></asp:TextBox></div>
    <asp:Panel ID="pnlCustomHeader" Visible="False" runat="server">
        <div style="padding-right: 0px; padding-left: 20px; padding-bottom: 0px; padding-top: 10px">
            <strong>Header Information</strong></div>
        <div style="padding-right: 0px; padding-left: 40px; font-weight: bold; font-size: 12px;
            padding-bottom: 0px; color: red; padding-top: 10px">
            * For better results, your image should not be more than 650px (pixels) wide.
        </div>
        <div style="padding-right: 0px; padding-left: 40px; padding-bottom: 0px; padding-top: 10px">
            Upload New Logo:&nbsp;<font style="font-size: 10px; padding-bottom: 0px; color: red">(Image
                will be uploaded when you click continue button.)</font><br>
            <input id="fHeaderImg" type="file" onchange="SelectedLogo(this);" name="fHeaderImg"
                runat="server">&nbsp;
        </div>
        <div style="padding-right: 0px; padding-left: 40px; padding-bottom: 0px; padding-top: 10px">
            Selected Logo:<br>
            <input id="HeaderImg" type="text" size="40" name="HeaderImg" runat="server"></div>
    </asp:Panel>
    <div class="dashed_lines1" style="padding-right: 0px; font-size: 14px; padding-bottom: 0px;
        padding-top: 10px">
        <table cellspacing="0" cellpadding="0" width="100%">
            <tr>
                <td align="left">
                    <strong>Advanced Editing <span style="color: red">(Optional)</span></strong></td>
                <td align="right">
                    <asp:ImageButton ID="ibtnEdit" runat="server" ImageUrl="../images/btn_edit_message.gif"
                        CausesValidation="False"></asp:ImageButton></td>
            </tr>
        </table>
    </div>
    <div style="padding-right: 70px; padding-left: 40px; padding-bottom: 10px; padding-top: 10px">
        You can choose to edit or replace the existing text from the template with a message
        of your own.</div>
</div>
<!--eof content-->
</TD></TR>
<!--text editor row-->
<asp:Panel ID="pEditor" runat="server">
    <tr>
        <td colspan="2">
            <div style="padding-right: 0px; padding-left: 0px; font-size: 12px; padding-bottom: 10px;
                padding-top: 10px" align="center">
                <div class="dashed_lines1" style="width: 650px" align="left">
                    <strong>Editing Instructions:</strong></div>
                <br>
                <div class="dashed_lines1" style="padding-left: 20px; padding-bottom: 10px; width: 650px"
                    align="left">
                    -&nbsp;Use the editor below to modify the text of the message.<br>
                    -&nbsp;Type your revisions just as you would in any text editor.<br>
                    -&nbsp;Put your cursor on the icons to determine their function.<br>
                    -&nbsp;To insert a link (for example, to a page on your website), type in a name
                    for the link, highlight the name, and click on the <sub>
                        <img src="/ecn.editor/editor/images/button_link.gif"></sub>
                    icon. In the box that appears, use the drop down to select the URL type, type in
                    the URL and click on save.<br>
                    -&nbsp;To delete a link, highlight the link and click the <sub>
                        <img src="/ecn.editor/editor/images/button_unlink.gif"></sub>
                    icon
                    <br>
                </div>
                <div style="width: 650px" align="left">
                    <img src="images/img_editor_header.gif"></div>
                <!--editor box-->
                  <CKEditor:CKEditorControl ID="contents" runat="server" Skin="kama" Height="450" Width="650"  BasePath="/ecn.editor/ckeditor/"></CKEditor:CKEditorControl>
 
            </div>
        </td>
    </tr>
</asp:Panel>

<%@ Control language="c#" Inherits="ecn.editor.ckeditor.controls.uploader" Codebehind="uploader.ascx.cs" %>
<style>
ol li
{
	margin-bottom:12px;
}
</style>
<table border="0" cellpadding="0" cellspacing="0" style="margin:10px 0;">
	<tr>
		<TD class=tableContent align=left width="50%">
    	<asp:Panel ID="HelpPanel" Runat="server" Visible=False>
      			<FIELDSET style="padding:10px;margin:0 0 0 10px;background:#fff;">
      			<oL style="padding:10px 20px;margin:0;">
      				<li>Click <strong>Choose File</strong> to select your image file.</li>
					<li>Select the image file from the File Dialog and click <strong>Open</strong>.</li>
                    <li>Click the <strong>Add</strong> button to add the image file to the file pool.<br />        
                		[optional] - Repeat steps 1-3 to add multiple files to the file pool.<br /> 
                		[optional] - To remove a file from the pool, select the file from the list of files & click <strong>Remove</strong>.</li>
                    <li>Select the <strong>Folder</strong> you want to Upload the file[s] to from the drop down menu or leave as ROOT.</li>
                    <li>Click the <strong>Upload</strong> Button to complete the upload process.<br /></li>
               </oL>
      		</FIELDSET> 
		</asp:Panel></TD>
		<td align="center">
        	<div style="width:274px;text-align:right;"><INPUT class="formfield" id="FindFile" style="WIDTH: 274px; HEIGHT: 22px" type="file" size="26"
				runat="server" NAME="FindFile"></div>
		
			<asp:listbox id="FilesListBox" runat="server" style="height:100px;width:274px;margin:10px 0;" Font-Size="XX-Small"></asp:listbox><br />
		
			<asp:button id="AddFile" runat="server" CssClass="formbuttonsmall" Text="Add" CausesValidation="False" Width="75px" onclick="AddFile_Click"></asp:button>
			<asp:button id="RemvFile" runat="server" CausesValidation="False" CssClass="formbuttonsmall"
				Text="Remove" Width="75px" onclick="RemvFile_Click"></asp:button> <br />
                
            <asp:Panel ID="FoldersPanel" Runat="server" style="text-align:center;margin-top:10px;" Visible=False>
            	<asp:DropDownList id=ImgFoldersDR Runat="server" Width="274px" CssClass="formfield" DataValueField="DirID" DataTextField="DirName" align="center"></asp:DropDownList>
			</asp:Panel><br />
	
	<asp:button id="Upload" runat="server" CssClass="formbuttonsmall" Text="Upload" CausesValidation="False" Width="75px" onclick="Upload_ServerClick"></asp:button>
    <div class="errormsg" style="text-align:center;">
			<asp:label id="MessageLabel" CssClass="whiteBG" runat="server" Height="25px"></asp:label>
			<asp:TextBox ID="uploadpath" CssClass="whiteBG" Runat="server" Visible="False"></asp:TextBox>
	</div>
		</td>
	</tr>
</table>


<%@ Control Language="c#" Inherits="ecn.communicator.includes.uploader" Codebehind="uploader.ascx.cs" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <asp:Panel ID="HelpPanel" runat="server" Visible="False">
            <td class="tableContent" align="left" width="50%" rowspan="5" style="padding-left: 5px">
                <FIELDSET style="padding:10px;margin:0 0 0 10px;background:#fff;">
      			<oL style="padding:10px 20px;margin:0;">
      				<li>Click <strong>Choose File</strong> to select your image file.</li>
					<li>Select the image file from the File Dialog and click <strong>Open</strong>.</li>
                    <li>Click the <strong>Add</strong> button to add the image file to the file pool.<br />        
                		[Optional] - Repeat steps 1-3 to add multiple files to the file pool.<br /> 
                		[Optional] - To remove a file from the pool, select the file from the list of files & click <strong>Remove</strong>.</li>
                    <li>Select the <strong>Folder</strong> you want to Upload the file[s] to from the drop down menu or leave as ROOT.</li>
                    <li>Click the <strong>Upload</strong> Button to complete the upload process.<br /></li>
                      <br/>
                </oL>
      		</FIELDSET> 
            </td>
        </asp:Panel>
        <td align="center">
            <input class="formfield" id="FindFile" style="width: 274px; height: 22px" type="file"
                size="26" runat="server" name="FindFile">
        </td>
    </tr>
    <tr>
        <td align="center" valign="bottom" style="padding-top: 2px">
            <asp:ListBox ID="FilesListBox" runat="server" Height="100px" Width="274px" Font-Size="XX-Small">
            </asp:ListBox>
        </td>
    </tr>
    <tr>
        <td align="center" style="padding-left: 122px">
            <asp:Button ID="AddFile" runat="server" CssClass="formbuttonsmall" Text="Add" CausesValidation="False"
                Width="75px" OnClick="AddFile_Click"></asp:Button>
            <asp:Button ID="RemvFile" runat="server" CausesValidation="False" CssClass="formbuttonsmall"
                Text="Remove" Width="75px" OnClick="RemvFile_Click"></asp:Button>
        </td>
    </tr>
    <asp:Panel ID="FoldersPanel" runat="server" Visible="False">
        <tr>
            <td height="30" valign="bottom" align="center" >
                <asp:DropDownList  ID="ImgFoldersDR" runat="server" DataTextField="DirName"
                    DataValueField="DirID" Width="274px" CssClass="formfield">
                </asp:DropDownList>
            </td>
        </tr>
    </asp:Panel>
    <tr>
        <td align="center" style="padding-left: 202px; padding-top: 3px">
            <input class="formbuttonsmall" id="Upload" type="submit" causesvalidation="False"
                value="Upload" runat="server" onserverclick="Upload_ServerClick" style="width: 75px">
        </td>
        <td>
        </td>
    </tr>
</table>

   
       <div class="errormsg" style="text-align:center;">
			<asp:label id="MessageLabel" CssClass="whiteBG" runat="server" Height="25px"></asp:label>
			<asp:TextBox ID="uploadpath" CssClass="whiteBG" Runat="server" Visible="False"></asp:TextBox>
	</div>
   

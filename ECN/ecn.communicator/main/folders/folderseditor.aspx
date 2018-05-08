<%@ Page Language="c#" Inherits="ecn.communicator.foldermanager.folderseditor" Codebehind="folderseditor.aspx.cs"  MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="/ecn.accounts/js/overlib/overlib.js"></script>

<script type="text/javascript"> djConfig = { isDebug: false }; </script>

<style type="text/css">
#FolderType img {position:relative;top:2px; left:-3px;}

#divPage
{
	display:none;
}

.modalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }
.modalPopup
{
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;       
}
 .TransparentGrayBackground
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
        height: 100%;
        width: 100%;
        min-height: 100%;
        min-width: 100%;
    }
    .overlay
    {
        position: fixed;
        z-index: 99;
        top: 0px;
        left: 0px;
        background-color: gray;
        width: 100%;
        height: 100%;
        filter: Alpha(Opacity=70);
        opacity: 0.70;
        -moz-opacity: 0.70;
    }
    * html .overlay
    {
        position: absolute;
        height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
        width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
    }
    .loader
    {
        z-index: 100;
        position: fixed;
        width: 120px;
        margin-left: -60px;
        background-color: #F4F3E1;
        font-size: x-small;
        color: black;
        border: solid 2px Black;
        top: 40%;
        left: 50%;
    }
    * html .loader
    {
        position: absolute;
        margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
    }
</style>

<script type="text/javascript">
function alternate()
{
	srcTable = document.getElementById("<%= FoldersList.ClientID %>");
	var rows = srcTable.getElementsByTagName("tr");   
	for(i = 0; i < rows.length; i++){
		if(i % 2 == 0){
       		rows[i].className = "even";
     	}else{
       		rows[i].className = "odd";
     }
	 	if(i == 0){
       		rows[i].className = "first";
     	}
	}
}

</script>


<script type="text/javascript">alternate();</script>

<script type="text/javascript" src="http://www.ecn5.com/ecn.accounts/js/browser.js"></script>

<script type="text/javascript">
    if (BrowserDetect.browser == "Safari") {
        if (document.getElementById("fldrName")) {
            if (document.getElementById("fldrDesc")) {
                document.getElementById("fldrName").style.position = "static";
                document.getElementById("fldrDesc").style.position = "static";
            }
        }
    }

	function deleteImgFolderConfirm(){
		if (confirm('Are you Sure?\nSelected Folder and the Images will be permanently deleted.')) {
			return true;
		}else{
			return false;
		}
	}

	function deleteCntGrpFolderConfirm(){
		if (confirm('Are you Sure?\nSelected Folder will be permanently deleted.')) {
			return true;
		}else{
			return false;
		}
	}

	function isNameValid(folderType)	{
		var fldrType	= folderType;
		var nameStr	= document.getElementById("fldrName").value;
		var desc		= document.getElementById("fldrDesc").value;		
		if(	nameStr.length > 20){
			alert('Folder Name cannot exceed 20 Characters in length');
			return false;			
		}if(	nameStr.length < 1){
			alert('Folder Name is blank');
			return false;			
		}else if((nameStr.toLowerCase() == 'root') || (nameStr.toLowerCase() == 'archived groups') || (nameStr.toLowerCase() == 'master suppression')){
			alert('Folder Name you have entered is \'reserved\' for ECN. Please use a different name');
			return false;	
		}else {
			var regexp			= /^[a-zA-Z0-9_\s]*$/;
			var matchArray = nameStr.match(regexp); // is the format ok?
			if (matchArray == null) {
				alert('Folder Name cannot have any special Characters. Underscore \'_\' is allowed.');
				return false;
			}
			
			if(folderType == 'IMG'){
				document.addNewSubFolder.action = "foldersAddHandler.aspx?fType="+folderType+"&fName="+nameStr;			
			}else {
				document.addNewSubFolder.action = "foldersAddHandler.aspx?fID="+folderID+"&fType="+folderType+"&fName="+nameStr+"&fDesc="+desc;			
			}
			document.addNewSubFolder.submit();
			cancelDlg();
		}
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="upMain" DynamicLayout="true">
    <ProgressTemplate>
        <asp:Panel ID="upMainProgressP1" CssClass="overlay" runat="server">
            <asp:Panel ID="upMainProgressP2" CssClass="loader" runat="server">
                <div>
                    <center>
                    <br />
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                    <br />
                    <br />
                    <br />
                    </center>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="upMain" runat="server">
    <ContentTemplate>
  <asp:PlaceHolder ID="phErrorMain" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="Td1">
                        </td>
                    </tr>
                    <tr>
                        <td id="Td2">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                    </td>
                                    <td valign="middle" align="left" width="80%" height="100%">
                                        <asp:Label ID="lblErrorMessageMain" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="Td3">
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>
    <table cellpadding="0" cellspacing="0" border='0' width="100%" style="margin: 0 0 10px 0;">
        <tr>
            <td style="padding: 10px;">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="gradient">
                            <table border='0' cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="formLabel" style="padding: 0 5px;">
                                        <strong>Manage Folder Type:</strong>
                                    </td>
                                    <td>
                                        <asp:radiobuttonlist id="FolderType" runat="Server" repeatdirection="horizontal"
                                            cssclass="formLabel" autopostback="true">
										<asp:ListItem value="IMG">&nbsp;&nbsp;<img src="/ecn.images/images/manageImagesFolders.gif" />Images</asp:ListItem>			
									</asp:radiobuttonlist>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="greySidesB offWhite"  align="left">
                            <div style="padding: 10px;">
                                <div style="width: 100%;">
                                    <span class="headingOne">
                                        <asp:label id="FolderListHeading" runat="Server"></asp:label>
                                    </span>
                                    <!-- start new design-->
                                    <asp:datalist id="FoldersList" runat="Server" style="background: #fff" width="100%"
                                        datakeyfield="FolderID" cellpadding="0" border='0' OnItemDataBound="FoldersList_ItemDataBound1" cssclass="gridWizard">
							<HeaderTemplate>
                            Folder Name</td>
                        <td width="15%" align="center">
                            Description</td>
                        <td width="15%" align="center">
                            Add Sub-Folder</td>
                        <td width="13%" align="center">
                            Date Created</td>
                        <td width="5%" align="center">
                            <% if (FolderType.SelectedValue.Equals("GRP"))
                               { %>
                            Groups
                            <% }
                               else %>
                            <% if (FolderType.SelectedValue.Equals("CNT"))
                               { %>
                            Contents
                            <% }
                               else %>
                            <% if (FolderType.SelectedValue.Equals("IMG"))
                               { %>
                            Images
                            <% } %>
                        </td>
                        <td width="5%">
                        </td>
                        <td width="5%">
                            </HeaderTemplate>
                            <itemtemplate>
                                <%# DataBinder.Eval(Container.DataItem, "FolderName") %>
                        </td>
                        <td align="center">
                            <asp:label id="FolderDescLinkBtn" runat="Server" text="&lt;img src=/ecn.images/images/folderDescription.gif border='0'&gt;"
                                causesvalidation="false" style="cursor: pointer; margin: 0; padding: 0;"></asp:label>
                            <asp:textbox id="HDNFolderDescTxtBx" runat="Server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "FolderDescription") %>'></asp:textbox>
                        </td>
                        <td align="center">
                            <asp:linkbutton id="AddSubFolderLinkBtn" style="padding: 0; margin: 0; border: none;"
                                runat="Server" commandname="Add" commandargument='<%# DataBinder.Eval(Container.DataItem, "FolderID") + "|" +  DataBinder.Eval(Container.DataItem, "ParentID") %>' causesvalidation="false" text="<img src=/ecn.images/images/addSubFolder.gif border='0'>"></asp:linkbutton>
                        </td>
                        <td align="center">
                            <%# DataBinder.Eval(Container.DataItem, "DateCreated") %>
                        </td>
                        <td align="center">
                            <asp:label id="FolderItemsLbl" runat="Server" text='<%# DataBinder.Eval(Container.DataItem, "Items") %>'></asp:label>
                        </td>
                        <td align="center" nowrap>
                            <asp:textbox id="HDNSystemFolder" runat="Server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "SystemFolder") %>'></asp:textbox>
                            <asp:textbox id="HDNFolderType" runat="Server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "FolderType") %>'></asp:textbox>
                            <asp:textbox id="HDNParentID" runat="Server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "ParentID") %>'></asp:textbox>
                            <asp:linkbutton id="FolderEdit" runat="Server" text=<%# ( (DataBinder.Eval(Container.DataItem, "SystemFolder").Equals("Y")) || (DataBinder.Eval(Container.DataItem, "FolderType").Equals("IMG")) )? "":"<img src=/ecn.images/images/editFolder.gif alt='Edit Folder details' border='0'>" %>
                                commandname="Edit" causesvalidation="false"></asp:linkbutton>
                        </td>
                        <td align="center" nowrap>
                            <asp:linkbutton id="FolderDelete" runat="Server" style="padding: 0; margin: 0; border: none;
                                cursor: pointer" text=<%# (DataBinder.Eval(Container.DataItem, "SystemFolder").Equals("Y")) ? "":"<img src=/ecn.images/images/icon-delete1.gif alt='Delete Folder' style='padding:0;margin:0;' border='0'>" %>
                                commandname="Delete" causesvalidation="false"></asp:linkbutton>
                        </td>
                        </ItemTemplate>
                        <edititemtemplate>
									<asp:TextBox id="Edit_FolderName" runat="Server" class="formfield" ></asp:TextBox>
            </td>
            <td valign="bottom">
                <asp:textbox id="Edit_FolderDesc" runat="Server" class="formfield" textmode="multiline"
                    rows="3" style="width: 130px; height: 40px;"></asp:textbox>
            </td>
            <td align="center" valign="middle">
            </td>
            <td align="center" valign="middle">
                <%# DataBinder.Eval(Container.DataItem, "DateCreated") %>
            </td>
            <td align="center" valign="middle">
                <%# DataBinder.Eval(Container.DataItem, "items") %>
            </td>
            <td nowrap align="center" valign="middle">
                <asp:linkbutton runat="Server" text="&lt;img src=/ecn.images/images/icon-save.gif alt='Save Folder Changes' border='0'&gt;"
                    commandname="Update" causesvalidation="false" id="UpdateProfileFieldBTN"></asp:linkbutton>
                <br />
                <asp:linkbutton runat="Server" text="&lt;img src=/ecn.images/images/icon-cancel.gif alt='Cancel Folder Changes' border='0'&gt;"
                    commandname="Cancel" causesvalidation="false" id="CancelProfileFieldBTN"></asp:linkbutton>
            </td>
            <td>
                &nbsp;</td>
            </EditItemTemplate> </asp:datalist>
            </div></div> </td>
        </tr>
    </table>
    </td> </tr> </table>
        <asp:Label ID="lblFolderID" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblCurrentDirectory" runat="server" Text="" Visible="false"></asp:Label>
     </ContentTemplate>  
</asp:UpdatePanel>
    
<asp:Button ID="btnShowPopup3" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="modalPopupAddFolder" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlAddFolder" TargetControlID="btnShowPopup3">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlAddFolder" CssClass="modalPopup">
<asp:UpdateProgress ID="upAddFolderProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="UpdatePanel3" DynamicLayout="true">
    <ProgressTemplate>
        <asp:Panel ID="upAddFolderProgressP1" CssClass="overlay" runat="server">
            <asp:Panel ID="upAddFolderProgressP2" CssClass="loader" runat="server">
                <div>
                    <center>
                    <br />
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                    <br />
                    <br />
                    <br />
                    </center>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ProgressTemplate>
</asp:UpdateProgress>
   <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
        <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop">
                        </td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                    </td>
                                    <td valign="middle" align="left" width="80%" height="100%">
                                        <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="errorBottom">
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>
        <table align="center" class="style1">
            <tr>
                <td>
                    Folder Name
                </td>
                <td>
                    <asp:TextBox ID="txtFolderNameSave" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Folder Description
                </td>
                <td>
                    <asp:TextBox ID="txtFolderDescriptionSave" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td style="text-align: right" >
               
            </td>
            <td style="text-align: left">
             <asp:Button runat="server" Text="Save" ID="btnAddFolderSave" CssClass="formfield"
                    OnClick="btnAddFolder_Save"></asp:Button> &nbsp;&nbsp;&nbsp;
                <asp:Button runat="server" Text="Close" ID="btnAddFolderClose"  OnClick="btnAddFolder_Close" CssClass="formfield"></asp:Button>
            </td>
            </tr>
        </table>
     </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
</asp:Content>


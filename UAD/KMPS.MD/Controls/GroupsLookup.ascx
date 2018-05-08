<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupsLookup.ascx.cs" Inherits="KMPS.MD.Controls.GroupsLookup" %>
<%@ Register TagPrefix="ecn" TagName="FolderSys" Src="~/Controls/FolderSystem.ascx" %>

<style type="text/css">
    .modalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
        
    }

    .modalPopupFull {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        width: 100%;
        height: 100%;
        overflow: auto;
    }

    .modalPopupLayoutExplorer {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: white;
        padding: 3px;
        overflow: auto;
    }

    .modalPopupGroupExplorer {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: white;
        padding: 3px;
        overflow: auto;
        z-index: 10002 !important;
    }

    .TransparentGrayBackground {
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

    .mdloverlay {
        position: fixed;
        z-index: 100002 !important;
        top: 0px;
        left: 0px;
        background-color: gray;
        width: 100%;
        height: 100%;
        filter: Alpha(Opacity=70);
        opacity: 0.70;
        -moz-opacity: 0.70;
    }

    * html .mdloverlay {
        position: absolute;
        height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
        width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
    }

    .mdlloader {
        z-index: 100002 !important;
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

    * html .mdlloader {
        position: absolute;
        margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
    }

    .modalPopupCal {
        background-color: transparent;
        padding: 1em 6px;
        z-index: 10003 !important;
    }

    .modalPopup2Cal {
        background-color: #ffffff;
        width: 270px;
        vertical-align: top;
        z-index: 10003 !important;
    }

</style>
        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
        <ajaxToolkit:ModalPopupExtender ID="mpeNewGroup" runat="server" TargetControlID="btnShowPopup"
            PopupControlID="pnlPopupDimensions2" BackgroundCssClass="modalBackground" />
        <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender8" runat="server" BehaviorID="RoundedCornersBehavior8"
            TargetControlID="pnlCalendar" Radius="6" Corners="All" />
        <asp:Panel ID="pnlPopupDimensions2" runat="server" Width="400px" CssClass="modalPopupCal">
            <asp:Panel ID="pnlCalendar" runat="server" Width="400px" CssClass="modalPopup2Cal">
                <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD; height: auto">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Add Group
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" width="100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblErrGroup" runat="server" Text="" Visible = "false" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label" valign="middle" align="left" width="100%">
                                            Group Name : &nbsp;
                                            <asp:TextBox ID="txtGroupName" runat="server" MaxLength="50"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtGroupName" ErrorMessage="Group Name has invalid characters. Please enter a valid Group Name." ValidationExpression="([A-Z]|[a-z]|[0-9]|[\s])+" ValidationGroup="groupAdd" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvtxtGroupName" runat="server" Font-Size="xx-small"
                                                ControlToValidate="txtGroupName" ErrorMessage=" * required"
                                                Font-Bold="True" ForeColor="red" ValidationGroup="groupAdd"></asp:RequiredFieldValidator>
                                        </td>
                                     </tr>
                                    <tr>
                                        <td width="100%" height="40" align="center">
                                            <asp:Button runat="server" Text="Save Group" ID="btnAddGroup" class="button" ValidationGroup="groupAdd"
                                                OnClick="btnAddGroup_Click"></asp:Button>
                                            <asp:Button runat="server" Text="Cancel" ID="btnCancelGroup" class="button" OnClick="btnCancelGroup_Click"
                                                ></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </asp:Panel>

        <asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
        <ajaxToolkit:ModalPopupExtender ID="modalPopupGroupExplorer" runat="server" BackgroundCssClass="modalBackground"
            PopupControlID="pnlgroupExplorer" TargetControlID="btnShowPopup2">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel runat="server" ID="pnlgroupExplorer" CssClass="modalPopupGroupExplorer">
        <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
            AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
            <ProgressTemplate>
                <div class="TransparentGrayBackground">
                </div>
                <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 1000000; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="../images/loading.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <Triggers> 
            <asp:PostBackTrigger ControlID="btngroupExplorer" />
            <asp:PostBackTrigger ControlID="GroupsGrid" />
        </Triggers>
        <ContentTemplate>
            <table style="background-color:white;width:800px">
                <tr style="background-color: #5783BD;">
                    <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold" align="center">Group Explorer
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                            <table cellspacing="0" cellpadding="0" width="674" align="center">
                                <tr>
                                    <td id="errorTop"></td>
                                </tr>
                                <tr>
                                    <td id="errorMiddle">
                                        <table width="80%">
                                            <tr>
                                                <td valign="top" align="center" width="20%">
                                                    <img style="padding: 0 0 0 15px;" src="../images/ic-error.gif">
                                                </td>
                                                <td valign="middle" align="left" width="80%" height="100%">
                                                    <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="errorBottom"></td>
                                </tr>
                            </table>
                        </asp:PlaceHolder>
                        <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0' bgcolor="#ffffff">
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" border='0' width="100%">
                                        <tr>
                                            <td align="left">
                                                <asp:Panel ID="FilterPanel" runat="Server">
                                                    <asp:DropDownList class="formfield" ID="SearchGrpsDR" runat="Server"
                                                        EnableViewState="False" Width="125">
                                                        <asp:ListItem Value="starts"> value starting with </asp:ListItem>
                                                        <asp:ListItem Value="like"> value containing any </asp:ListItem>
                                                        <asp:ListItem Value="ends"> value ending with </asp:ListItem>
                                                        <asp:ListItem Value="equals"> value equals </asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;
                                                    <asp:CheckBox ID="chkAllFolders" runat="server" Text="All Folders" AutoPostBack="false" />
                                                    &nbsp;
                                                    <asp:TextBox class="formtextfield" ID="SearchCriteria" runat="Server" EnableViewState="False"
                                                        Columns="16"></asp:TextBox>&nbsp;
                                                    <asp:Button class="button" ID="SearchButton"
                                                        runat="Server" EnableViewState="False" CausesValidation="false" OnClick="SearchButton_Click" Text="Search"></asp:Button>&nbsp;&nbsp;
                                                <div style="float:right">
                                                    <asp:Button ID="btnAddNewGroup" Text="Add Group" runat="server" CssClass="button" OnClick="btnAddNewGroup_Click" Visible="false" />
                                                </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <hr />
                                 </td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="width: 100%;" cellspacing="0" cellpadding="0" border="0">
                                        <tr valign="top">
                                            <td width="12%" valign="top">
                                                <table class="grid" cellspacing="0" cellpadding="4" border="0" style="width: 100%; border-collapse: collapse;">
                                                    <tr class="gridheader">
                                                        <th align="left" scope="col">
                                                            Customers/Folders
                                                        </th>
                                                    </tr>
                                                </table>
                                                <div style="overflow: auto; width: 300px; height: 400px; border: solid 1px #CCCCCC; border-top: none;">
                                                    <ecn:FolderSys ID="GroupFolder" runat="Server"></ecn:FolderSys>
                                                    <br />
                                                </div>
                                            </td>
                                            <td width="1%">&nbsp;
                                            </td>
                                            <td style="border: solid 1px #CCCCCC; border-top: none;" valign="top">
                                                <div style="overflow: auto; width: 100%; height: 400px;">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="hfFolderID" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hfCustomerID" runat="server" Value="0"  />
                                                    <asp:GridView ID="GroupsGrid" runat="Server" Width="100%" CssClass="grid"
                                                    AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="GroupsGrid_Command"
                                                    OnPageIndexChanging="GroupsGrid_PageIndexChanging" DataKeyNames="GroupID" PageSize="10"
                                                    OnRowDataBound="GroupsGrid_RowDataBound">
                                                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="GroupName" HeaderText="Group Name" ItemStyle-Width="65%"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="Server" Text="&lt;img src='../Images/icon-add.gif' alt='Select Group' border='0'&gt;"
                                                                    CausesValidation="false" ID="SelectGroupBtn" CommandName="SelectGroup" CommandArgument='<%# Eval("GroupID") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle CssClass="gridaltrow" />
                                                </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlPager" runat="server" Visible="false">
                                                    <table cellpadding="0" border="0" width="100%">
                                                        <tr>
                                                            <td class="label" style="width:23%;">
                                                                
                                                                    Total Records:
                                                <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                                                                </td>
                                                            <td class="label" style="width:30%;">
                                                                
                                                                    Show Rows:
                                                                    
                                                                    <asp:DropDownList ID="ddlPageSizeContent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="GroupGrid_SelectedIndexChanged" CssClass="formfield">
                                                                        <asp:ListItem Value="5" />
                                                                        <asp:ListItem Value="10" />
                                                                        <asp:ListItem Value="15" />
                                                                        <asp:ListItem Value="20" />
                                                                    </asp:DropDownList>
                                                                    
                                                                </td>
                                                            <td class="label" style="width:47%;">
                                                                
                                                                    Page
                                                <asp:TextBox ID="txtGoToPageContent" runat="server" AutoPostBack="true" OnTextChanged="GoToPageContent_TextChanged" class="formtextfield" Width="30px" />
                                                                    of
                                                <asp:Label ID="lblTotalNumberOfPagesGroup" runat="server" CssClass="label" />
                                                                    
                                                <asp:Button ID="btnPreviousGroup" runat="server" CausesValidation="false" ToolTip="Previous Page" OnClick="btnPreviousGroup_Click" class="formbuttonsmall" Text="<< Prev" />
                                                                    <asp:Button ID="btnNextGroup" runat="server" CausesValidation="false" ToolTip="Next Page" OnClick="btnNextGroup_Click" class="formbuttonsmall" Text="Next >>" />
                                                                

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Button runat="server" Text="Close" CausesValidation="false" ID="btngroupExplorer" CssClass="button"
                            OnClick="groupExplorer_Hide"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        </asp:UpdatePanel>
        </asp:Panel>
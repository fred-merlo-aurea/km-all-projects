<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="groupsLookup.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Group.groupsLookup" %>
<%@ Register TagPrefix="ecn" TagName="FolderSys" Src="~/includes/folderSystem.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>

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

</style>
        <asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />

        <ajaxToolkit:ModalPopupExtender ID="modalPopupGroupExplorer" runat="server" BackgroundCssClass="modalBackground"
            PopupControlID="pnlgroupExplorer" TargetControlID="btnShowPopup2">
        </ajaxToolkit:ModalPopupExtender>

        <asp:Panel runat="server" ID="pnlgroupExplorer" CssClass="modalPopupGroupExplorer">
        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <Triggers> 
            <asp:PostBackTrigger ControlID="btngroupExplorer" />
            <asp:PostBackTrigger ControlID="GroupsGrid" />
        </Triggers>
        <ContentTemplate>
            <table style="background-color:white;width:600px">
                <tr style="background-color: #5783BD;">
                    <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Group Explorer
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
                                    <td id="errorBottom"></td>
                                </tr>
                            </table>
                        </asp:PlaceHolder>
                        <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0' bgcolor="#ffffff">
                            <tr>
                                <td valign="top">
                                    <table cellpadding="0" cellspacing="0" border='0' width="100%">
                                        <tr>
                                            <td align="right">
                                                <asp:Panel ID="FilterPanel" runat="Server">
                                                    <asp:DropDownList class="formfield" ID="SearchTypeDR" runat="Server" Visible="true"
                                                        EnableViewState="False" Width="100">
                                                        <asp:ListItem Value="Group" Selected="True">Group</asp:ListItem>
                                                        <asp:ListItem Value="Profile">Profile in Group</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;
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
                                                    <asp:DropDownList ID="ddlArchiveFilter" runat="server">
                                                        <asp:ListItem Text="Active" Value="active" Selected="True" />
                                                        <asp:ListItem Text="Archived" Value="archived" />
                                                        <asp:ListItem Text="All" Value="all" />
                                                    </asp:DropDownList>
                                <asp:Button class="formbuttonsmall" ID="SearchButton"
                                    runat="Server" EnableViewState="False" Width="75" CausesValidation="false" OnClick="SearchButton_Click" Text="Search"></asp:Button>
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

                                                            <asp:HyperLink ID="hlFolders" runat="server" Text="Folders" NavigateUrl="~/main/folders/folderseditor.aspx" />
                                                        </th>
                                                    </tr>
                                                </table>
                                                <div style="overflow: auto; width: 200px; height: 400px; border: solid 1px #CCCCCC; border-top: none;">
                                                    <ecn:FolderSys ID="GroupFolder" runat="Server"></ecn:FolderSys>
                                                    <br />
                                                </div>
                                            </td>
                                            <td width="1%">&nbsp;
                                            </td>
                                            <td style="border: solid 1px #CCCCCC; border-top: none;" valign="top">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td>
                                                            <ecnCustom:ecnGridView ID="GroupsGrid" runat="Server" Width="100%" CssClass="grid"
                                                    AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="GroupsGrid_Command"
                                                    OnPageIndexChanging="GroupsGrid_PageIndexChanging" DataKeyNames="GroupID" PageSize="15"
                                                    OnRowDataBound="GroupsGrid_RowDataBound">
                                                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField  HeaderText="Folder Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFolderName" runat="server" />
                                                            </ItemTemplate>
                                                            </asp:TemplateField>
                                                        <asp:BoundField DataField="GroupName" HeaderText="Group Name" ItemStyle-Width="65%"
                                                            ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/add_icon.jpg alt='Select Group' border='0'&gt;"
                                                                    CausesValidation="false" ID="SelectGroupBtn" CommandName="SelectGroup" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "GroupID") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle CssClass="gridaltrow" />
                                                    
                                                </ecnCustom:ecnGridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlPager" runat="server" Visible="false">
                                                    <table cellpadding="0" border="0" width="100%">
                                                        <tr>
                                                            <td class="label" style="width:33%;">
                                                                
                                                                    Total Records:
                                                <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                                                                </td>
                                                            <td class="label" style="width:33%;">
                                                                
                                                                    Show Rows:
                                                                    
                                                                    <asp:DropDownList ID="ddlPageSizeContent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="GroupGrid_SelectedIndexChanged" CssClass="formfield">
                                                                        <asp:ListItem Value="5" />
                                                                        <asp:ListItem Value="10" />
                                                                        <asp:ListItem Value="15" />
                                                                        <asp:ListItem Value="20" />
                                                                    </asp:DropDownList>
                                                                    
                                                                </td>
                                                            <td class="label" style="width:34%;">
                                                                
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
                        <asp:Button runat="server" Text="Close" CausesValidation="false" ID="btngroupExplorer" CssClass="aspBtn1"
                            OnClick="groupExplorer_Hide"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        </asp:UpdatePanel>
        </asp:Panel>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dynamicTagEdit.aspx.cs" Inherits="ecn.communicator.main.content.dynamicTagEdit" MasterPageFile="~/MasterPages/Communicator.Master"%>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/main/ECNWizard/Content/contentExplorer.ascx" TagName="contentExplorer" TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/Content/contentEditor.ascx" TagName="contentEditor" TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/OtherControls/ruleEditor.ascx" TagName="ruleEditor" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel='stylesheet' href="../../MasterPages/ECN_MainMenu.css" type="text/css" />
    <link rel='stylesheet' href="../../MasterPages/ECN_Controls.css" type="text/css" />
    <style type="text/css">
    .modalPopup
    {
        background-color: Gray;
        overflow: auto;
    }
    .modalPopupContentExplorer
    {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: white;
        padding: 3px;
        overflow: auto;
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
    .reorderStyle
    {
	    list-style-type:disc;
	    font:Verdana;
	    font-size:12px;
    }
    .reorderStyle li
    {
	    list-style-type:none;
        padding-bottom: 1em;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="UpdatePanel1ProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="UpdatePanel1ProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <br />
    <br />   <br /> 
    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop">
                        </td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table height="67" width="80%">
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
    <table cellpadding="3" cellspacing="0">
          <tr valign="middle">
            <td colspan="7" align="left">     
                    &nbsp;
            </td>
        </tr>
        <tr valign="middle">
            <td align="right">
                <asp:Label ID="Label9" runat="server" Text="Tag" Font-Size="Small" />
            </td>
            <td align="left">
                 <asp:TextBox ID="txtTag" runat="server" MaxLength="50" CssClass="ECN-TextBox-Medium"></asp:TextBox>
            </td>
            <td></td>
            <td align="right">
                <asp:Label ID="Label8" runat="server" Text="Content" Font-Size="Small" />
            </td>                   
            <td align="left">
                 <asp:TextBox ID="txtContentTag" runat="server" CssClass="ECN-TextBox-Medium" ReadOnly="true" Text="-No Content Selected-"></asp:TextBox>
                 <asp:HiddenField ID="hfTagContentID" runat="server" Value="0"/>
            </td>
            <td align="left" colspan="2">
                <ul class="ECN-InfoLinks" style="padding-left: 0px;padding-top:10px;" >
                    <li>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/ecn-icon-gear-small.png" />
                        <ul>                                                                                                                                                                              
                            <li><asp:LinkButton ID="btnNewContentTag" runat="server" Text="New Content"  CssClass="aspBtn"  OnClick="btnNewContentTag_Click"/> </li>
                            <li><asp:LinkButton ID="btnExistingContentTag" runat="server" Text="Existing Content"  CssClass="aspBtn"  OnClick="btnExistingContentTag_Click"/> </li>
                        </ul>
                    </li>
                </ul>
                    <asp:HiddenField ID="hfContentEditorFromTag" runat="server" Value=""/>                      
                    <asp:HiddenField ID="hfContentExplorerFromTag" runat="server" Value=""/>
            </td> 
            <td></td>
        </tr>
        <tr valign="middle" align="left">     
            <td colspan="7">       <br />   <br />            
                <asp:Label ID="lblHeadingAddRule" runat="server" Text="Rules" Font-Bold="true" Font-Size="Medium"></asp:Label>  <br /> 
            </td>
        </tr>
        <tr valign="middle">
            <td align="right">
                <asp:Label ID="Label5" runat="server" Text="Rule" Font-Size="Small" />
            </td>
            <td align="left">
                 <asp:TextBox ID="txtRule" runat="server" CssClass="ECN-TextBox-Medium" ReadOnly="true" Text="-No Rule Selected-"></asp:TextBox>
                 <asp:HiddenField ID="hfRuleID" runat="server"  Value="0"/>
            </td>
            <td  align="left" >
                  <ul class="ECN-InfoLinks"  style="padding-left: 0px;padding-top:10px;padding-right: 0px;">
                    <li>
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/ecn-icon-gear-small.png" />
                        <ul>                                                                                                                                                                              
                            <li><asp:LinkButton ID="btnNewRule" runat="server" Text="New Rule"  CssClass="aspBtn"  OnClick="btnNewRule_Click"/></li>
                            <li><asp:LinkButton ID="btnExistingRule" runat="server" Text="Existing Rule"  CssClass="aspBtn"  OnClick="btnExistingRule_Click"/></li>
                        </ul>
                    </li>
                </ul>
            </td>
            <td align="right">
                <asp:Label ID="Label7" runat="server" Text="Content" Font-Size="Small" />
            </td>                                
            <td align="left">
                 <asp:TextBox ID="txtContentRule" runat="server" CssClass="ECN-TextBox-Medium" ReadOnly="true" Text="-No Content Selected-"></asp:TextBox>
                        <asp:HiddenField ID="hfRuleContentID" runat="server"  Value="0"/>
            </td> 
            <td align="left">
                <ul class="ECN-InfoLinks"  style="padding-left: 0px;padding-top:10px;">
                    <li>
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/images/ecn-icon-gear-small.png" />
                        <ul>                                                                                                                                                                              
                            <li><asp:LinkButton ID="btnNewContentRule" runat="server" Text="New Content"  CssClass="aspBtn"  OnClick="btnNewContentRule_Click"/></li>
                            <li><asp:LinkButton ID="btnExistingContentRule" runat="server" Text="Existing Content"  CssClass="aspBtn"  OnClick="btnExistingContentRule_Click"/></li>
                        </ul>
                    </li>
                </ul>
            </td> 
            <td align="center">
                <asp:Button ID="btnAddRule" runat="server" Text="Add" OnClick="btnAddRule_Click" CssClass="ECN-Button-Small"/>
            </td>
        </tr>
    </table> 
    <br />
    <table width="100%">
        <tr>
            <td align="left">                
                 <asp:HiddenField ID="hfRuleCount" runat="server"  Value="0"/>
                <asp:ReorderList ID="rolRules" runat="server" AllowReorder="true" DragHandleAlignment="Left" 
                    ShowInsertItem="false" DataKeyField="DynamicTagRuleID" SortOrderField="Priority"  OnDeleteCommand="rolRules_DeleteCommand" 
                     CssClass="reorderStyle" OnItemReorder="rolRules_ItemReorder" PostBackOnReorder="true">
                    <EmptyListTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                     <asp:Label ID="lblHeadingExistingRules" runat="server" Text="There are no existing Rules for this Dynamic Tag" Font-Bold="true" Font-Size="Small"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </EmptyListTemplate> 
                     <ReorderTemplate>
                        <div style="height: 40px; border: dashed 2px orange; background-color: transparent; cursor:move">                           
                        </div>         
                    </ReorderTemplate>
                    <DragHandleTemplate>
                        <div style="height: 40px; width:20px; border: solid 2px darkgrey; background-color: darkgrey; cursor:move">                           
                        </div>
                    </DragHandleTemplate>                   
                    <ItemTemplate>
                        <table  style="background-color: transparent; border: solid 2px darkgrey;height:40px;" width="100%" cellspacing="3" cellpadding="0">
                            <tr>
                                 <td width="10%" align="right">
                                      <asp:Label ID="lblDynamicTagRuleID" runat="server" Font-Size="Small" Visible="false" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("DynamicTagRuleID"))) %>' />
                               
                                     <asp:Label ID="Label3" runat="server" Text="Rule:" Font-Size="Small" />
                                </td>
                                 <td width="20%" align="left">
                                     <asp:Label ID="Label4" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("RuleName"))) %>'  Font-Size="Small" />
                                </td>
                                 <td width="10%" align="right">
                                     <a href='contentpreview.aspx?ContentID=<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("ContentID"))) %>' target="_blank">
                                        <img src="/ecn.images/images/icon-preview-HTML.gif" alt='Edit Content' border='0'>
                                    </a>
                                </td>
                                 <td width="20%" align="left">
                                     <asp:Label ID="Label6" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("ContentTitle"))) %>' Font-Size="Small"  />
                                    
                                </td>
                                 <td width="10%" align="right">
                                     <asp:Label ID="Label5" runat="server" Text="Priority:" Font-Size="Small" />
                                </td>
                                 <td width="10%" align="left">
                                    <asp:Label ID="Label1" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("Priority"))) %>' Font-Size="Small"  />
                                </td>
                                <td width="10%">
                                    <asp:ImageButton ID="imgbtnRuleEdit" runat="server" ImageUrl="/ecn.images/images/icon-edits1.gif" CausesValidation="false"  OnClick="imgbtnRuleEdit_Click1" CommandArgument='<%#Eval("RuleID")%>' />
                                </td>
                                <td align="center" width="10%">
                                     <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                                CommandName="Delete" OnClientClick="return confirm('Are you sure you want to remove this Rule?')"
                                                CausesValidation="false" CommandArgument='<%#Eval("DynamicTagRuleID")%>' />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:ReorderList>
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:Button ID="btnTagSave" runat="server" Text="Save" OnClick="btnTagSave_Click" CssClass="ECN-Button-Medium"/>
            </td>
        </tr>
    </table> <br /> <br />
    </ContentTemplate>
       
</asp:UpdatePanel>


    
<asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
<asp:ModalPopupExtender ID="modalPopupcontentExplorer" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlcontentExplorer" TargetControlID="btnShowPopup2">
</asp:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlcontentExplorer" CssClass="modalPopupcontentExplorer">
    <asp:UpdateProgress ID="upcontentExplorerProgress" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upcontentExplorer" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upcontentExplorerProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upcontentExplorerProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upcontentExplorer" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <table bgcolor="white">
                <tr style="background-color: #5783BD;">
                    <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold" colspan="2">Content Explorer
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc1:contentExplorer ID="contentExplorer1" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Button runat="server" Text="Close" ID="btncontentExplorer" CssClass="ECN-Button-Small"
                            OnClick="btncontentExplorer_Click"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

    
<asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
<asp:ModalPopupExtender ID="modalPopupCreateContent" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlCreateContent" TargetControlID="btnShowPopup1">
</asp:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlCreateContent" CssClass="modalPopupCreateContent">
    <asp:UpdateProgress ID="upContentEditorProgress" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upContentExplorer" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upContentEditorProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upContentEditorProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upContentEditor" runat="server">
        <ContentTemplate>
            <uc1:contentEditor ID="contentEditor1" runat="server" />
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: right">
                        <asp:Button runat="server" Text="Save" ID="btnCreateContent"  CssClass="ECN-Button-Small"
                            OnClick="CreateContent_Save"></asp:Button>
                    </td>
                    <td style="text-align: left">
                        <asp:Button runat="server" Text="Close" ID="btnClose"  CssClass="ECN-Button-Small" OnClick="CreateContent_Close">
                        </asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

        
<asp:Button ID="Button1" runat="server" Style="display: none" />
<asp:ModalPopupExtender ID="modalPopupCreateRule" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlRuleEditor" TargetControlID="Button1">
</asp:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlRuleEditor" Width="900px" CssClass="modalPopup">
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upRuleEditor" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upRuleEditorProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upRuleEditorProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upRuleEditor" runat="server">
        <ContentTemplate ><br />
            <uc1:ruleEditor ID="RuleEditor1" runat="server" />
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: right">
                        <asp:Button runat="server" Text="Save" ID="btnRuleEditorSave"  CssClass="ECN-Button-Small"
                            OnClick="btnRuleEditorSave_Click"></asp:Button>
                    </td>
                    <td style="text-align: left">
                        <asp:Button runat="server" Text="Close" ID="btnRuleEditorClose"  CssClass="ECN-Button-Small" OnClick="btnRuleEditorClose_Click">
                        </asp:Button>
                    </td>
                </tr>
            </table><br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:Button ID="Button2" runat="server" Style="display: none" />
<asp:ModalPopupExtender ID="modalPopupExistingRule" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlRuleExisting" TargetControlID="Button2">
</asp:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlRuleExisting" CssClass="modalPopup">
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upRuleExisting" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upRuleExistingProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upRuleExistingProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upRuleExisting" runat="server">
        <ContentTemplate>    <br />       
            <table align="center" cellspacing="3">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Existing Rule" Font-Size="Small" />
                    </td>
                    <td>
                        <asp:DropDownList ID="drpRule" DataValueField="RuleID" DataTextField="RuleName" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Button runat="server" Text="Ok" ID="btnExitingRuleOk"  CssClass="ECN-Button-Small"
                            OnClick="btnExitingRuleOk_Click"></asp:Button>
                    </td>
                    <td style="text-align: left">
                        <asp:Button runat="server" Text="Cancel" ID="btnExistingRuleClose"  CssClass="ECN-Button-Small" OnClick="btnExistingRuleClose_Click">
                        </asp:Button>
                    </td>
                </tr>
            </table><br /><br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

</asp:Content>   

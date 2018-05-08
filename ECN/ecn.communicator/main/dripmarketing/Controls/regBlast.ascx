<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="regBlast.ascx.cs" Inherits="ecn.communicator.blastsmanager.regBlast" %>
<%@ Register Src="../../blasts/BlastScheduler.ascx" TagName="BlastScheduler" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/main/ECNWizard/Group/groupExplorer.ascx" TagName="groupExplorer"
    TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/Content/layoutExplorer.ascx" TagName="layoutExplorer"
    TagPrefix="uc1" %>
<script type="text/javascript">
    function replyTo_focus() {
        try {
            getobj('<%=txtReplyTo.ClientID%>').value = getobj('<%=txtEmailFrom.ClientID%>').value
        }
        catch (e)
                    { }
    };
</script>
<style type="text/css">
    .modalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }
    .modalPopupFull
    {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        width: 100%;
        height: 100%;
        overflow: auto;
    }
    .modalPopupLayoutExplorer
    {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: white;
        padding: 3px;
        overflow: auto;
    }
    .modalPopupGroupExplorer
    {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: white;
        padding: 3px;
        overflow: auto;
    }
    .modalPopupImport
    {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        height: 60%;
        overflow: auto;
    }
    .buttonMedium
    {
        width: 135px;
        background: url(buttonMedium.gif) no-repeat left top;
        border: 0;
        font-weight: bold;
        color: #ffffff;
        height: 20px;
        cursor: pointer;
        padding-top: 2px;
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
<br />
<asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="upMain" DynamicLayout="false">
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
        <table width="100%">
            <tr valign="top">
                <td align="left" colspan="2">
                    <asp:Label ID="Label2" runat="server" Text="Details" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr valign="top">
                <td align="left" width="20%" class="label" style="padding-left: 50px;">
                   <asp:Button runat="server" Text="Groups" ID="btnGroupConfigure" CssClass="aspBtn1" OnClick="lnkGroupConfigure_Click">
                    </asp:Button>
                </td>
                <td align="left" width="80%" class="label"  style="padding-left: 20px;">
                    <asp:Label ID="lblGroups" runat="server" Text="-No Group Selected-" Font-Bold="true"></asp:Label>
                    <asp:GridView ID="gvSelectedGroups" runat="server" AutoGenerateColumns="false" Font-Size="Small" font-Bold="true" GridLines="None" ShowHeader="false" >
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
                                Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GroupID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" >
                                <ItemTemplate>
                                    <asp:Label ID="lblGroupName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GroupName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                     <asp:GridView ID="gvSuppressed" runat="server"  AutoGenerateColumns="false" GridLines="None" Font-Size="Small" font-Bold="true" ShowHeader="false" >
                        <Columns>
                            <asp:TemplateField HeaderText="GroupID" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
                              Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GroupID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Group Name" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblGroupName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GroupName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                </td>
            </tr>
            <tr valign="top">
                <td align="left" width="20%" class="label" style="padding-left: 50px;">
                 <asp:Button runat="server" Text="Message" ID="btnMessageConfigure" CssClass="aspBtn1" OnClick="lnkMessageConfigure_Click">
                    </asp:Button>
                </td>
                <td align="left" width="80%" class="label"  style="padding-left: 20px;">
                    <asp:Label ID="lblLayoutName" runat="server" Text="-No Message Selected-"  Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblLayoutID" runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr valign="top">
                <td align="left" colspan="2">
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="Envelope" Font-Size="Large"></asp:Label>
                </td>
            </tr valign="top">
            <tr align="left">
                <td align="left" colspan="2" style="padding-left: 50px;">
                    <table width="100%">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                        <td colspan="2" class="label">
                                            <b>
                                                <asp:Label ID="dyanmicFieldsLbl" Visible="false" runat="server">Dynamic Personalization Fields [optional]:</asp:Label></b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align='left' class="label">
                                            From Email&nbsp;
                                        </td>
                                        <td nowrap="nowrap" align="left">
                                            <asp:TextBox class="styled-text" ID="txtEmailFrom" runat="Server" Columns="40" ValidationGroup="formValidation">
                                            </asp:TextBox>
                                            <asp:DropDownList class="styled-select" ID="drpEmailFrom" runat="Server" Visible="false"
                                                AutoPostBack="true" OnSelectedIndexChanged="drpEmailFrom_OnSelectedIndexChanged"
                                                Width="225px">
                                            </asp:DropDownList>
                                            <asp:Button ID="btnChangeEnvelope" runat="server" Text="Change" OnClick="btnChangeEnvelope_onclick"
                                               CssClass="aspBtn1" />
                                        </td>
                                        <td valign="middle" align='left'>
                                            <asp:Label ID="dyanmicEmailFromLbl" Visible="false" runat="server"> From Email&nbsp;</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dyanmicEmailFrom" runat="server" Width="200px" Visible="false"
                                                class="styled-select" DataTextField="ShortNameText" DataValueField="ShortNameValue">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align='left' class="label">
                                            Reply To&nbsp;
                                        </td>
                                        <td nowrap="nowrap" align="left">
                                            <asp:TextBox class="styled-text" ID="txtReplyTo" runat="Server" Columns="40" ValidationGroup="formValidation"></asp:TextBox><asp:DropDownList
                                                class="styled-select" ID="drpReplyTo" runat="Server" Visible="false" AutoPostBack="true"
                                                OnSelectedIndexChanged="drpReplyTo_OnSelectedIndexChanged" Width="225px">
                                            </asp:DropDownList>
                                        </td>
                                        <td valign="middle" align='left'>
                                            <asp:Label ID="dyanmicReplyToEmailLbl" Visible="false" runat="server">Reply To&nbsp;</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dyanmicReplyToEmail" runat="server" Width="200px" Visible="false"
                                                class="styled-select" DataTextField="ShortNameText" DataValueField="ShortNameValue">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align='left' class="label">
                                            From Name&nbsp;
                                        </td>
                                        <td nowrap="nowrap" align="left">
                                            <asp:TextBox class="styled-text" ID="txtEmailFromName" runat="Server" Columns="40"></asp:TextBox><asp:DropDownList
                                                class="styled-select" ID="drpEmailFromName" runat="Server" Visible="false" AutoPostBack="true"
                                                OnSelectedIndexChanged="drpEmailFromName_OnSelectedIndexChanged" Width="225px">
                                            </asp:DropDownList>
                                        </td>
                                        <td valign="middle" align='left'>
                                            <asp:Label ID="dyanmicEmailFromNameLbl" Visible="false" runat="server"> From Name&nbsp;</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dyanmicEmailFromName" runat="server" Width="200px" Visible="false"
                                                class="styled-select" DataTextField="ShortNameText" DataValueField="ShortNameValue">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" class="label">
                                            Subject&nbsp;
                                        </td>
                                        <td nowrap="nowrap" align="left">
                                            <asp:TextBox class="styled-text" ID="txtSubject" runat="Server" Columns="40"></asp:TextBox>
                                        </td>
                                        <td valign="middle" align='left'>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Label ID="Label3" runat="server" Text="Schedule" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr align="center">
                <td colspan="2" style="padding-left: 50px;">
                    <br />
                    <uc1:BlastScheduler ID="BlastScheduler1" runat="server" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
<asp:ModalPopupExtender ID="modalPopupGroupExplorer" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlgroupExplorer" TargetControlID="btnShowPopup2">
</asp:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlgroupExplorer" CssClass="modalPopupGroupExplorer">
    <asp:UpdateProgress ID="upgroupExplorerProgress" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upgroupExplorer" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upgroupExplorerProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upgroupExplorerProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upgroupExplorer" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
        <table  bgcolor="white">
                <tr style="background-color: #5783BD;">
                    <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold" colspan="2">
                                Group Explorer
                    </td>
                </tr>
                <tr>
                    <td>                        
                        <uc1:groupExplorer ID="groupExplorer1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Button runat="server" Text="Close" ID="btngroupExplorer" CssClass="aspBtn1"
                            OnClick="groupExplorer_Hide"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
<asp:ModalPopupExtender ID="modalPopupLayoutExplorer" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnllayoutExplorer" TargetControlID="btnShowPopup1">
</asp:ModalPopupExtender>
<asp:Panel runat="server" ID="pnllayoutExplorer" CssClass="modalPopupLayoutExplorer">
    <asp:UpdateProgress ID="uplayoutExplorerProgress" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="uplayoutExplorer" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="uplayoutExplorerProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="uplayoutExplorerProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="uplayoutExplorer" runat="server">
        <ContentTemplate>
         <table bgcolor="white">
                <tr style="background-color: #5783BD;">
                    <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold" colspan="2">
                                Message Explorer
                    </td>
                </tr>
                <tr>
                    <td>                  
                    <uc1:layoutExplorer ID="layoutExplorer1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Button runat="server" Text="Close" ID="Button1" CssClass="aspBtn1"
                            OnClick="layoutExplorer_Hide"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

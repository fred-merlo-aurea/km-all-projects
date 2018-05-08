<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.addressloader.addressloader" CodeBehind="addressloader.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>
<%@ Register Src="~/main/ECNWizard/Group/groupsExplorer.ascx" TagName="groupsExplorer" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="upMain" ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="AddButton"/>
        </Triggers>
        <ContentTemplate>
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
            <br />
            <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border="0">
                <tbody>
                <tr>
                    <td class="tableHeader" valign="top" align="right" height="16" style="width: 125px;">
                        &nbsp;<span class="label">Group / List&nbsp;</span>
                    </td>
                    <%--<td valign="top" height="16" align="left">
                    <asp:DropDownList Style="width: 250px" CssClass="formfield" ID="GroupID" EnableViewState="true"
                        runat="Server" DataTextField="GroupName" OnSelectedIndexChanged="GroupID_SelectedIndexChanged" AutoPostBack="true" DataValueField="GroupID" class="formfield">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvGroupID" runat="server" ControlToValidate="GroupID"
                        Error                    
                    Message="&laquo; required" ForeColor="" CssClass="errormsg"></asp:RequiredFieldValidator>
                </td>--%>

                    <td>
                        <div style="padding-bottom: 8px">
                            <asp:RadioButton ID="rbGroupChoice1" runat="server" Text="" GroupName="GroupChoice" AutoPostBack="true" OnCheckedChanged="GroupChoice_CheckedChanged" Checked="True"/>                        
                            <asp:HiddenField ID="hfGroupSelectionMode" runat="server" Value="None"/>
                            <asp:HiddenField ID="hfSelectGroupID" runat="server" Value="0"/>
                            <asp:ImageButton ID="imgSelectGroup" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgSelectGroup_Click" Visible="true"/>
                            <asp:Label ID="lblSelectGroupName" runat="server" Text="-No Group Selected-" Font-Size="Small"></asp:Label>
                        </div>
                        <div style="padding-bottom: 8px">
                            <asp:RadioButton ID="rbGroupChoice2" runat="server" Text="All Groups" GroupName="GroupChoice" AutoPostBack="true" OnCheckedChanged="GroupChoice_CheckedChanged"/>
                        </div>
                        <div style="padding-bottom: 8px">
                            <asp:RadioButton ID="rbGroupChoice3" runat="server" Text="Master Suppression Group" GroupName="GroupChoice" AutoPostBack="true" OnCheckedChanged="GroupChoice_CheckedChanged"/>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" valign="top" align="right">
                        <span class="label">&nbsp;SubscribeType&nbsp;</span>
                    </td>
                    <td valign="top" align="left">
                        <asp:DropDownList Style="width: 135px" ID="SubscribeTypeCode" EnableViewState="true"
                                          runat="Server" DataTextField="CodeName" DataValueField="CodeValue" class="formfield">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" valign="top" align="right">
                        <span class="label">&nbsp;FormatType&nbsp;</span>
                    </td>
                    <td valign="top" align="left">
                        <asp:DropDownList Style="width: 135px" ID="FormatTypeCode" EnableViewState="true"
                                          runat="Server" DataTextField="CodeName" DataValueField="CodeValue" class="formfield">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:RadioButtonList CssClass="tableContent" RepeatColumns="2" RepeatLayout="flow"
                                             EnableViewState="false" Visible="false" ID="HandleDuplicates" DataTextField="display"
                                             DataValueField="value" runat="Server">
                            <asp:ListItem id="I" runat="Server" Value="Insert" Selected="true" enableviewstate="false"/>
                            <asp:ListItem id="U" runat="Server" Value="Update" enableviewstate="false"/>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" valign="top" align="right">
                        <span class="label">&nbsp;Addresses&nbsp;</span>
                    </td>
                    <td valign="top" align="left">
                        <asp:TextBox ID="Addresses" runat="Server" EnableViewState="true" TextMode="multiline"
                                     Columns="55" Rows="10" class="formfield" Width="400px">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" align="right" valign="top">
                        <span class="label">&nbsp;Results&nbsp;</span>
                    </td>
                    <td valign="top">
                        <asp:Label ID="MessageLabel" runat="Server" Visible="false"></asp:Label>
                        <asp:DataGrid ID="ResultsGrid" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True"
                                      AutoGenerateColumns="False" OnItemCommand="ResultsGrid_ItemCommand"  OnItemDataBound="ResultsGrid_ItemDataBound" Width="400px" Visible="false">
                            <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <Columns>
                                <asp:BoundColumn DataField="Action" HeaderText="Description" ItemStyle-Width="75%"
                                                 HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Totals" ItemStyle-Width="25%" HeaderStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkTotals" Text='<%# DataBinder.Eval(Container.DataItem, "Totals") %>' runat="server" CommandName="DownloadEmails" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ActionCode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" colspan="3" align="left" style="padding-left: 310px;">
                        <asp:TextBox EnableViewState="true" Visible="false" ID="LayoutID" runat="Server"></asp:TextBox>
                        <asp:Button ID="AddButton" OnClick="AddEmails" runat="Server" Text="Add" class="formbutton"
                                    Visible="true"/>
                    </td>
                </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="btnShowPopup2" runat="server" Style="display: none"/>
    <ajaxToolkit:ModalPopupExtender ID="modalPopupGroupExplorer" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlgroupExplorer" TargetControlID="btnShowPopup2"/>
    <asp:Panel runat="server" ID="pnlgroupExplorer" CssClass="modalPopupGroupExplorer">
        <asp:UpdateProgress ID="upgroupExplorerProgress" runat="server" DisplayAfter="10"
                            Visible="true" AssociatedUpdatePanelID="upgroupExplorer" DynamicLayout="false">
            <ProgressTemplate>
                <asp:Panel ID="upgroupExplorerProgressP1" CssClass="overlay" runat="server">
                    <asp:Panel ID="upgroupExplorerProgressP2" CssClass="loader" runat="server">
                        <div>
                            <center>
                                <br/>
                                <br/>
                                <b>Processing...</b><br/>
                                <br/>
                                <img src="http://images.ecn5.com/images/loading.gif" alt=""/><br/>
                                <br/>
                                <br/>
                                <br/>
                            </center>
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="upgroupExplorer" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <table bgcolor="white">
                    <tr style="background-color: #5783BD;">
                        <td style="color: #ffffff; font-size: 20px; font-weight: bold; padding: 5px;">
                            Group Explorer
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:groupsExplorer ID="groupExplorer1" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <asp:Button runat="server" Text="Close" ID="btngroupExplorer" CssClass="aspBtn1" OnClick="groupExplorer_Hide"></asp:Button>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
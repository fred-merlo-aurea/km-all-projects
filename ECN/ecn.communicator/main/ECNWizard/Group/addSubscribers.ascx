<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addSubscribers.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Group.newGroup_add" %>
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
 <table>
                <tr>
                    <td  style="padding-left: 30px; padding-right: 30px">
                     

                        <table cellspacing="0" cellpadding="4" width="100%" border="0" style="padding-left: 30px">
                         <asp:Panel ID="pnlSubscriberType" runat="Server" Visible="true">
                            <tr>
                                <td class="tableHeader" align="left" width="15%">
                                    Type:
                                </td>
                                <td class="dataOne" align="left" width="85%">
                                    <asp:RadioButtonList ID="rblistType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblistType_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="Email">Email Address</asp:ListItem>
                                     <asp:ListItem Value="Mobile">Mobile Number</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            </asp:Panel>
                             <asp:Panel ID="pnlTypeCode" runat="Server" Visible="true">
                              <tr>
                                    <td class="tableHeader" valign="top" align='right'>
                                        <span class="formLabel">&nbsp;SubscribeType&nbsp;</span>
                                    </td>
                                    <td valign="top" align="left">
                                        <asp:DropDownList Style="width: 135px" ID="SubscribeTypeCode" EnableViewState="true"
                                            runat="Server" DataTextField="CodeName" DataValueField="CodeValue" class="formfield">
                                        </asp:DropDownList>
                                    </td>
                            </tr>
                            <tr>
                                    <td class="tableHeader" valign="top" align='right'>
                                        <span class="formLabel">&nbsp;FormatType&nbsp;</span>
                                    </td>
                                    <td valign="top" align="left">
                                        <asp:DropDownList Style="width: 135px" ID="FormatTypeCode" EnableViewState="true"
                                            runat="Server" DataTextField="CodeName" DataValueField="CodeValue" class="formfield">
                                        </asp:DropDownList>
                                    </td>
                            </tr>
                            </asp:Panel>                          
                            <tr>
                                <td class="formLabel" valign="top" align="left" width="15%">
                                    <asp:Label ID="lblType" runat="server" Text="Email Address"></asp:Label></td>
                                <td class="dataOne" align="left" width="85%">
                                    <asp:TextBox ID="txtEmailAddress" CssClass="dataOne" runat="server" EnableViewState="true"
                                        Rows="10" Columns="75" TextMode="multiline"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="tableHeader" align='right' valign="top">
                                </td>
                                <td valign="top">
                                    <asp:DataGrid ID="ResultsGrid" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True"
                                        AutoGenerateColumns="False" Width="400px" Visible="false">
                                        <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                                        <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="Action" HeaderText="Description" ItemStyle-Width="75%"
                                                HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Totals" HeaderText="Totals" ItemStyle-Width="25%" HeaderStyle-Width="25%">
                                            </asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                        </table>
                   
                    </td>
                </tr>
</table>
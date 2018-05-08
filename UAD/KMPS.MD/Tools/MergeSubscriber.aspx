<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="MergeSubscriber.aspx.cs" Inherits="KMPS.MD.Tools.MergeSubscriber" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript">
        function showdetails(id) {

            var cont = document.getElementById('Div' + id);
            if (cont.style.display == 'block') {
                cont.style.display = 'none';
                document.getElementById('Img' + id).src = '../Images/plus.gif';
            }
            else {
                cont.style.display = 'block';
                document.getElementById('Img' + id).src = '../Images/minus.gif';
            }
        }

    </script>
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 1000009; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="../images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSelect" />
            <asp:AsyncPostBackTrigger ControlID="btnReset" />
        </Triggers>
        <ContentTemplate>
            <center>
                <div id="divError" runat="Server" visible="false">
                    <table cellspacing="0" cellpadding="0" width="674" align="center">
                        <tr>
                            <td id="errorMiddle">
                                <table width="80%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/ic-error.gif" runat="server"
                                                alt="" />
                                        </td>
                                        <td valign="middle" align="left" width="80%" height="100%">
                                            <asp:Label ID="lblErrorMessage" runat="Server" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divMessage" runat="server" visible="false">
                    <table cellspacing="0" cellpadding="0" width="60%">
                        <tr>
                            <td id="Td1"></td>
                        </tr>
                        <tr>
                            <td id="Td2">
                                <table width="80%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <img id="Img2" style="padding: 0 0 0 15px;" src="~/images/checkmark.gif" runat="server"
                                                alt="" />
                                        </td>
                                        <td valign="middle" align="left" width="80%" height="100%">
                                            <asp:Label ID="lblMessage" runat="Server" CssClass="Menu"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="Td3"></td>
                        </tr>
                    </table>
                </div>
                <br />
                <div style="width: 90%; text-align: left; padding-left: 10px;">
                    <table border="0" cellpadding="0" cellspacing="15" width="100%" align="center">
                        <tr>
                            <td width="3%" align="right"><b>IGrpNo</b>&nbsp;&nbsp;</td>
                            <td width="28%">
                                <asp:TextBox ID="tbIGrp_No1" runat="server" Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvIGrp_No1" runat="server" ControlToValidate="tbIGrp_No1"
                                    ErrorMessage="*" Font-Bold="false" ValidationGroup="Select" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid IGRP_No"
                                    ValidationExpression='^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$' ControlToValidate="tbIGrp_No1"
                                    ValidationGroup="Select" ForeColor="Red">*</asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td width="3%" align="right"><b>IGrpNo</b>&nbsp;&nbsp;</td>
                            <td width="28%">
                                <asp:TextBox ID="tbIGrp_No2" runat="server" Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvIGrp_No2" runat="server" ControlToValidate="tbIGrp_No2"
                                    ErrorMessage="*" Font-Bold="false" ValidationGroup="Select" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid IGrpNo"
                                    ValidationExpression='^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$' ControlToValidate="tbIGrp_No2"
                                    ValidationGroup="Select" ForeColor="Red">*</asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnSelect" runat="server" Text="Select" OnClick="btnSelect_Click" CssClass="button" ValidationGroup="Select" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="button" CausesValidation="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:HiddenField ID="hfSubscriptionID1" runat="server" />
                                <asp:HiddenField ID="hfSubscriptionID2" runat="server" />
                                <telerik:RadGrid AutoGenerateColumns="False" ID="rgSubscriptionDetails" runat="server" Visible="false" Width="90%" OnItemDataBound="rgSubscriptionDetails_ItemDataBound">
                                    <MasterTableView DataKeyNames="Item2, Item3" Width="100%" ShowHeadersWhenNoRecords="false" NoMasterRecordsText="No Records" ShowFooter="True">
                                        <HeaderStyle Font-Bold="true" Font-Size="13px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                        <FooterStyle Font-Bold="true" Font-Size="13px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                        <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                        <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="15%" UniqueName="Item1">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFieldName" runat="server" Text='<%# Eval("Item1")%>' Font-Bold="True"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text="Consensus Profile Section"></asp:Label>
                                                </HeaderTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="42%" UniqueName="Item2">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFieldValue1" runat="server" Text='<%# Eval("Item2")%>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text="Please select IGroup Number to keep on Merged Subscriber"></asp:Label><br />
                                                    <br />
                                                    <asp:CheckBox ID="cbID1" runat="server" />
                                                    <asp:Label ID="lbltbIGrp_No1" runat="server" Text='<%# tbIGrp_No1.Text%>'></asp:Label>
                                                    <asp:HiddenField ID="hfID1" runat="server" Value='<%# hfSubscriptionID1.Value%>' />
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="43%" UniqueName="Item3">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFieldValue2" runat="server" Text='<%# Eval("Item3")%>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <br />
                                                    <asp:CheckBox ID="cbID2" runat="server" />
                                                    <asp:Label ID="lbltbIGrp_No2" runat="server" Text='<%# tbIGrp_No2.Text%>'></asp:Label>
                                                    <asp:HiddenField ID="hfID2" runat="server" Value='<%# hfSubscriptionID2.Value%>' />
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                <br />
                                <telerik:RadGrid AutoGenerateColumns="False" ID="rgProducts" runat="server" Visible="false" Width="90%" OnItemDataBound="rgProducts_ItemDataBound">
                                    <MasterTableView DataKeyNames="PubID1, PubID2" Width="100%" ShowHeadersWhenNoRecords="True" NoMasterRecordsText="" AllowAutomaticUpdates="false">
                                        <HeaderStyle Font-Bold="true" Font-Size="13px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                        <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                        <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Products &nbsp; &nbsp; For all duplicate products below, select the ones that you would like on the final merged subscriber" HeaderStyle-Font-Bold="true" HeaderStyle-Width="50px" UniqueName="PubName1">
                                                <ItemTemplate>
                                                    <table width="100%" border="1">
                                                        <tr>
                                                            <td width="16%">
                                                                <a onclick='return void(0);' href="<%# DataBinder.Eval(Container,"RowIndex","javascript:showdetails('{0}')") %>">
                                                                    <img src="../Images/plus.gif" alt="Add" style="border: none; vertical-align: middle" id='<%# DataBinder.Eval(Container,"RowIndex","Img{0}") %>' /></a>

                                                            </td>
                                                            <td width="42%">
                                                                <asp:CheckBox ID="cbID1" runat="server" Visible='<%# Eval("PubName1") == string.Empty ||  Eval("PubName1") == null  ? false : true %>' />
                                                                <asp:Label ID="lblPubName1" runat="server" Text='<%# Eval("PubName1")%>' Font-Bold="true"></asp:Label>
                                                                <asp:HiddenField ID="hfPubSubscriptionID1" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hfPubID1" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hfIsCirc1" runat="server" Value='<%# Eval("IsCirc1")%>' />
                                                            </td>
                                                            <td width="42%">
                                                                <asp:CheckBox ID="cbID2" runat="server" Visible='<%# Eval("PubName2") == string.Empty || Eval("PubName2") == null ? false : true %>' />
                                                                <asp:Label ID="lblPubName2" runat="server" Text='<%# Eval("PubName2")%>' Font-Bold="true"></asp:Label>
                                                                <asp:HiddenField ID="hfPubSubscriptionID2" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hfPubID2" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hfIsCirc2" runat="server" Value='<%# Eval("IsCirc2")%>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <div id='<%# DataBinder.Eval(Container,"RowIndex","Div{0}") %>' style="display: none">
                                                                    <telerik:RadGrid AutoGenerateColumns="False" ID="rgPubSubscriptionDetails" runat="server" Visible="true" Width="100%">
                                                                        <MasterTableView DataKeyNames="Item2" Width="100%" ShowHeadersWhenNoRecords="false" NoMasterRecordsText="" AllowAutomaticUpdates="false">
                                                                            <HeaderStyle Font-Bold="true" Font-Size="13px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                            <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                            <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                            <Columns>
                                                                                <telerik:GridBoundColumn HeaderText="" HeaderStyle-Width="16%" DataField="Item1" ItemStyle-Font-Bold="true"></telerik:GridBoundColumn>
                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="42%" UniqueName="Select">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Item2")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="42%" UniqueName="Select">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Item3")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>
                                                                            </Columns>
                                                                        </MasterTableView>
                                                                    </telerik:RadGrid>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnMerge" runat="server" Text="Merge" OnClick="btnMerge_Click" OnClientClick="return confirm('Are you sure you want to merge this subscriber? It will delete the data from the unselected checkboxes!!')" CssClass="button" CausesValidation="false" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

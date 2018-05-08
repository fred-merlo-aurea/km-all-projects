<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master"
    AutoEventWireup="True" CodeBehind="BlastEnvelopes.aspx.cs" Inherits="ecn.communicator.main.blasts.BlastEnvelopes" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="update1" DynamicLayout="true">
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
    <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
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
                        <td id="errorBottom"></td>
                    </tr>
                </table>
                </br>
            </asp:PlaceHolder>
            <ecnCustom:ecnGridView ID="gvEnvelope" runat="server" Width="100%" CssClass="grid"
                AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="gvEnvelope_Command"
                OnPageIndexChanging="gvEnvelope_PageIndexChanging" DataKeyNames="BlastEnvelopeID"
                PageSize="15">
                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                <Columns>
                    <asp:BoundField DataField="FromName" HeaderText="From Name" ItemStyle-Width="35%"
                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="FromEmail" HeaderText="From Email" ItemStyle-Width="35%"
                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="UpdatedDate" HeaderText="Last Updated" ItemStyle-Width="20%"
                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                        <ItemStyle Width="5%"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Blast Envelope' border='0'&gt;"
                                CausesValidation="false" ID="EditBlastEnvelopeBtn" CommandName="EditBlastEnvelope"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BlastEnvelopeID") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                        <ItemStyle Width="5%"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Blast Envelope' border='0'&gt;"
                                CausesValidation="false" ID="DeleteBlastEnvelopeBtn" CommandName="DeleteBlastEnvelope"
                                OnClientClick="return confirm('Are you sure you want to delete this Blast Envelope?');"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BlastEnvelopeID") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </ecnCustom:ecnGridView>
            <br />
            <asp:Panel ID="pnlAdd" runat="server">
                <table id="layoutWrapper" cellspacing="1" cellpadding="3" width="100%" border='0'>
                    <tbody>
                        <tr>
                            <td class="tableHeader" valign="middle" align='right' width="10%">From Name
                            </td>
                            <td align="left">
                                <asp:Label ID="lblBlastEnvelopeID" runat="Server" Text="0" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtFromName" runat="Server" Width="150px" class="formfield" ValidationGroup="newBlastEnvelope"></asp:TextBox>
                                &nbsp;<font size="-2" face='verdana' color="#000000">
                                    <asp:RequiredFieldValidator ID="rfvFromname" runat="Server" ErrorMessage="<< required"
                                        Font-Names="arial" Font-Size="xx-small" ControlToValidate="txtFromName" ValidationGroup="newBlastEnvelope"></asp:RequiredFieldValidator>
                                </font>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableHeader" valign="middle" align='right' height="41">From Email
                            </td>
                            <td height="41" valign="middle" align="left">
                                <asp:TextBox ID="txtFromEmail" runat="Server" Width="400px" class="formfield"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtFromEmail"
                                    runat="server" Font-Size="xx-small" ControlToValidate="txtFromEmail" ErrorMessage="« required"
                                    Font-Italic="True" Font-Bold="True" ValidationGroup="newBlastEnvelope"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                        ID="revtxtReplyTo" runat="server" Font-Size="xx-small" ControlToValidate="txtFromEmail"
                                        ErrorMessage="« Not Valid" Font-Italic="True" Font-Bold="True" ValidationExpression=".*@.*\..*" ValidationGroup="newBlastEnvelope"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableHeader" valign="top" align="center" colspan="2">
                                <asp:Button class="formbutton" ID="btnSave" runat="Server" Text="Save" OnClick="btnSave_Click"
                                    ValidationGroup="newBlastEnvelope"></asp:Button>&nbsp;&nbsp;
                            <asp:Button class="formbutton" ID="btnCancel" runat="Server" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

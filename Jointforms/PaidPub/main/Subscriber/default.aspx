<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs"
    Inherits="PaidPub.main.Subscriber._default" %>

<%@ MasterType VirtualPath="~/Site1.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="contentheader">
        Subscribers
    </div>
    <br />
    <div class="padding20">
        <div class="box">
            <div class="boxheader">
                View Subscribers</div>
            <div class="boxcontent">
                <table cellpadding="5" cellspacing="0" border="0">
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left" valign="middle">
                            Email Address :
                        </td>
                        <td width="50%" align="left" valign="middle">
                            <asp:TextBox ID="txtEmailAddress" runat="server" Width="200" MaxLength="50"></asp:TextBox>&nbsp;&nbsp;<asp:Button
                                CssClass="blackButton" ID="btnSave" runat="server" Text="Submit" OnClick="btnSubmit_Click">
                            </asp:Button>&nbsp;
                        </td>
                        <td width="20%" align="left" valign="middle">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmailAddress"
                                ErrorMessage="&gt;&gt; requried" Font-Bold="True" Font-Italic="False" Font-Size="X-Small"
                                Font-Strikeout="False" Font-Underline="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            <asp:Label ID="lblErrorMessage" runat="server" Visible="false" CssClass="error"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <asp:Panel ID="pnlCurrentSubscriptions" runat="server" Visible="false">
                        <tr>
                            <td colspan="3">
                                <div class="eNewsHeader">
                                    Current Subscriptions :</div>
                                <br>
                                <asp:GridView ID="gvSubscribed" runat="server" DataKeyNames="groupID" AutoGenerateColumns="False"
                                    Width="80%" PageSize="20" CellPadding="4" BackColor="White" BorderColor="#CC9966"
                                    BorderStyle="None" BorderWidth="1px" OnRowCommand="gvSubscribed_RowCommand">
                                    <RowStyle BackColor="White" ForeColor="#330099" />
                                    <Columns>
                                        <asp:BoundField DataField="groupname" HeaderText="Newsletter">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="50%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Datavalue" HeaderText="Subscription Type">
                                            <HeaderStyle HorizontalAlign="center" />
                                            <ItemStyle HorizontalAlign="center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:CommandField ButtonType="Button" ShowSelectButton="true" SelectText="Change" />
                                    </Columns>
                                    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                    <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <br />
                                <asp:Panel ID="pnlChange" runat="server" Visible="false" Width="80%">
                                    <div class="box">
                                        <div class="boxheader">
                                            Change Subscription</div>
                                        <div class="boxcontent">
                                            <table cellpadding="5" cellspacing="0" border="0">
                                                <tr>
                                                    <td colspan="3">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="30%" align="left">
                                                        <b>Newsletter :</b>
                                                    </td>
                                                    <td width="70%" align="left">
                                                        <asp:Label ID="lblgroupID" runat="server" Visible="false" />
                                                        <asp:Label ID="lbleNewsletter" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <b>Current Type :</b>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblcsType" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <b>Change to :</b>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="drpsubscriptiontype" runat="server" Width="100" OnSelectedIndexChanged="drpsubscriptionOnChanged"
                                                            AutoPostBack="true" />
                                                    </td>
                                                </tr>
                                                <asp:Panel ID="pnlStartDate" runat="server" Visible="false">
                                                    <tr>
                                                        <td align="left">
                                                            <b>Subscription Option :</b>
                                                        </td>
                                                        <td align="left">
                                                            <asp:RadioButtonList ID="rbYears" runat="server" CellSpacing="5" CellPadding="5"
                                                                RepeatColumns="3" RepeatDirection="Horizontal" AutoPostBack="True">
                                                                <asp:ListItem Text="1 Year" Value="1" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="2 Years" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="3 Years" Value="3"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlTrialperiod" runat="server" Visible="false">
                                                <tr>
                                                        <td align="left">
                                                            <b>Trial Period :</b>
                                                        </td>
                                                        <td align="left">
                                                            1 week.
                                                        </td>
                                                    </tr>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlAmountPaid" runat="server" Visible="false">
                                                    <tr>
                                                        <td align="left">
                                                            <b>Amount Paid :</b>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtAmountPaid" runat="server" Width="100" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <b>Promo Code :</b>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtPromoCode" runat="server" Width="100" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <b>Payment Method:</b>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="drpPaymentMethod" runat="server" Width="100">
                                                                <asp:ListItem Text="CHECK" Value="CHECK"></asp:ListItem>
                                                                <asp:ListItem Text="CREDIT CARD" Value="CREDIT"></asp:ListItem>
                                                                <asp:ListItem Text="PO" Value="PO"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlPassword" runat="server" Visible="false">
                                                    <tr>
                                                        <td align="left">
                                                            <b>Password:</b>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtPassword" runat="server" Width="100" />
                                                        </td>
                                                    </tr>
                                                </asp:Panel>
                                                <tr>
                                                    <td colspan="3">
                                                        <br />
                                                        <asp:Button CssClass="blackButton" ID="btnSaveStatus" runat="server" Text="Save"
                                                            OnClick="btnSubmitStatus_Click"></asp:Button>&nbsp;
                                                        <asp:Button CssClass="blackButton" ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"  OnClick="btnCancel_Click">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

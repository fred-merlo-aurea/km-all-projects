<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Adjustments.aspx.cs"
    Inherits="PaidPub.main.Subscriber.Adjustments" %>

<%@ MasterType VirtualPath="~/Site1.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnCancel" />
        </Triggers>
        <ContentTemplate>
            <div class="contentheader">
                Adjustments
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
                                </td>
                            </tr>
                            <tr>
                                <td width="20%" align="left" valign="middle">
                                    Email Address :
                                </td>
                                <td width="50%" align="left" valign="middle">
                                    <asp:TextBox ID="txtEmailAddress" runat="server" Width="200" MaxLength="50"></asp:TextBox>&nbsp;&nbsp;<asp:Button
                                        CssClass="blackButton" ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click">
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
                                            <b>Current Subscriptions :</b></div>
                                        <br />
                                        <asp:DataList ID="dlSubscribed" runat="server" AllowPaging="false" AllowSorting="false"
                                            Width="100%" AutoGenerateColumns="false" DataKeyField="EntryID" GridLines="Both"
                                            OnItemDataBound="dlSubscribed_ItemDataBound">
                                            <HeaderTemplate>
                                                <table cellspacing="0" cellpadding="4" rules="all" border="0" style="background-color: White;
                                                    border-color: #CC9966; border-width: 1px; border-style: None; width: 100%; border-collapse: collapse;">
                                                    <tr style="color: #FFFFCC; background-color: #990000; font-weight: bold;">
                                                        <th align="left" width="35%" nowrap="nowrap">
                                                            Newsletter
                                                        </th>
                                                        <th align='center' width="10%" nowrap="nowrap">
                                                            Start date
                                                        </th>
                                                        <th align='center' width="10%" nowrap="nowrap">
                                                            End Date
                                                        </th>
                                                        <th align='center' width="10%" nowrap="nowrap">
                                                            Amount
                                                        </th>
                                                        <th align='center' width="10%" nowrap="nowrap">
                                                            Earned
                                                        </th>
                                                        <th align='center' width="10%" nowrap="nowrap">
                                                            Deferred
                                                        </th>
                                                        <th align='center' width="10%" nowrap="nowrap">
                                                            PaidorFree
                                                        </th>
                                                        <th align='center' width="10%" nowrap="nowrap">
                                                            Total Paid
                                                        </th>
                                                        <th align='center' width="5%" nowrap="nowrap">
                                                        </th>
                                                    </tr>
                                            </HeaderTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <tr style="background-color: #eeeeee; color: #000000;">
                                                    <td align="left" width="35%">
                                                        <b>
                                                            <%# DataBinder.Eval(Container.DataItem, "groupname")%></b>
                                                    </td>
                                                    <td align="center" width="10%">
                                                        <%# DataBinder.Eval(Container.DataItem, "startdate")%>
                                                    </td>
                                                    <td align="center" width="10%">
                                                        <%# DataBinder.Eval(Container.DataItem, "enddate")%>
                                                    </td>
                                                    <td align="center" width="10%">
                                                        <asp:Label ID="lblAmountPaid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amountpaid")%>'></asp:Label>
                                                    </td>
                                                    <td align="center" width="10%">
                                                        <%# DataBinder.Eval(Container.DataItem, "earnedamount")%>
                                                    </td>
                                                    <td align="center" width="10%">
                                                        <%# DataBinder.Eval(Container.DataItem, "Deferredamount")%>
                                                    </td>
                                                    <td align="center" width="10%">
                                                        <%# DataBinder.Eval(Container.DataItem, "PaidorFree")%>
                                                    </td>
                                                     <td align="center" width="10%">
                                                        <asp:Label ID="lblTotalPaid" runat="server"></asp:Label>
                                                    </td>
                                                    <td align="center" width="5%">
                                                        <asp:Button ID="btnAddAdjustment" runat="server" Text="Add" OnCommand="btnAddAdjustment_Command"
                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EntryID")%>' Visible='<%# DataBinder.Eval(Container.DataItem, "PaidorFree").ToString().ToUpper()=="PAID"?true:false%>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="9" align="center" style="padding-top: 5px; padding-bottom: 5px">
                                                        <div style="width: 95%; padding-top: 5px; padding-bottom: 10px;">
                                                            <asp:GridView ID="gvAdjustments" runat="server" DataKeyNames="entryID" AutoGenerateColumns="False"
                                                                Width="90%" AllowPaging="false" AllowSorting="false" CellPadding="2" BackColor="#444444"
                                                                BorderColor="#990000" BorderStyle="Dotted" BorderWidth="1px">
                                                                <RowStyle BackColor="White" ForeColor="#000000" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="AdjDate" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}">
                                                                        <HeaderStyle HorizontalAlign="center" />
                                                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AdjType" HeaderText="Type">
                                                                        <HeaderStyle HorizontalAlign="center" />
                                                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AdjAmount" HeaderText="Amount">
                                                                        <HeaderStyle HorizontalAlign="center" />
                                                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AdjExpDate" HeaderText="Exp Date">
                                                                        <HeaderStyle HorizontalAlign="center" />
                                                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AdjDesc" HeaderText="Description">
                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                        <ItemStyle HorizontalAlign="left" Width="40%" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                                                <HeaderStyle BackColor="#eeeeee" Font-Bold="True" ForeColor="#000000" />
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <br />
                                        <asp:Panel ID="pnlAddAdjustment" runat="server" Visible="false" Width="80%">
                                            <div class="box">
                                                <div class="boxheader">
                                                    Add Adjustments</div>
                                                <div class="boxcontent">
                                                    <table cellpadding="5" cellspacing="0" border="0">
                                                        <tr>
                                                            <td align="left">
                                                                <b>Newsletter :</b>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                                <asp:Label ID="lblNewsletterName" runat="server" Visible="true" />
                                                                <asp:Label ID="lblGroupID" runat="server" Visible="false" />
                                                                <asp:Label ID="lblAdjEntryID" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <b>Adjustment Type :</b>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="drpAdjustmentType" runat="server" Width="200" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="drpAdjustmentType_SelectedIndexChanged">
                                                                    <asp:ListItem Text="--- Select Adjustment Type ---" Value=""></asp:ListItem>
                                                                    <asp:ListItem Text="Discounts" Value="DISCOUNT"></asp:ListItem>
                                                                    <asp:ListItem Text="Expiration" Value="EXPIRATION"></asp:ListItem>
                                                                    <asp:ListItem Text="Cancel" Value="CANCEL"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpAdjustmentType"
                                                                    ErrorMessage="&gt;&gt; requried" Font-Bold="True" Font-Italic="False" Font-Size="X-Small"
                                                                    InitialValue="" Font-Strikeout="False" Font-Underline="False"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <asp:Panel ID="pnlDiscounts" Visible="false" runat="server">
                                                            <tr>
                                                                <td align="left">
                                                                    <b>Amount :</b>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAmount" runat="server" Width="100" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAmount"
                                                                        ErrorMessage="&gt;&gt; requried" Font-Bold="True" Font-Italic="False" Font-Size="X-Small"
                                                                        Font-Strikeout="False" Font-Underline="False"></asp:RequiredFieldValidator>
                                                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtAmount"
                                                                        Type="Currency" ErrorMessage=">> Invalid Amount (Should be between 1 and 10000)"
                                                                        Font-Size="X-Small" Font-Bold="True" MaximumValue="10000" MinimumValue="1"></asp:RangeValidator>
                                                                </td>
                                                            </tr>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlExpiration" Visible="false" runat="server">
                                                            <tr>
                                                                <td align="left">
                                                                    <b>New Expiration Date :</b>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtExpDate" runat="server" Width="100" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtExpDate"
                                                                        ErrorMessage="&gt;&gt; requried" Font-Bold="True" Font-Italic="False" Font-Size="X-Small"
                                                                        Font-Strikeout="False" Font-Underline="False"></asp:RequiredFieldValidator>
                                                                    <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtExpDate"
                                                                        Type="Date" ErrorMessage=">> Invalid Date" Font-Size="X-Small" Font-Bold="True"
                                                                        MaximumValue="12/31/2099" MinimumValue="01/01/2000"></asp:RangeValidator>
                                                                </td>
                                                            </tr>
                                                        </asp:Panel>
                                                        <tr>
                                                            <td align="left">
                                                                <b>Description :</b>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDesc" runat="server" Width="250" MaxLength="50" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
                                                                    AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
                                                                    <ProgressTemplate>
                                                                        <sub>
                                                                            <img id="Img1" border="0" src="http://goldensupply.ecn5.com/images/animated-loading-orange.gif" /></sub>&nbsp;<font
                                                                                size="2" color="#cc0000"><b>processing.......</b></font>
                                                                    </ProgressTemplate>
                                                                </asp:UpdateProgress>
                                                            </td>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <br />
                                                                    <asp:Button CssClass="blackButton" ID="btnSaveStatus" runat="server" Text="Save"
                                                                        OnClick="btnSave_Click"></asp:Button>&nbsp;
                                                                    <asp:Button CssClass="blackButton" ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                                                        OnClick="btnCancel_Click"></asp:Button>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

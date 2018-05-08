<%@ Reference Control="~/includes/ContactEditor.ascx" %>
<%@ Reference Control="~/includes/TestingAccount.ascx" %>
<%@ Reference Control="~/includes/ContactEditor2.ascx" %>
<%@ Reference Control="~/includes/UserInfoCollector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ContactEditor2" Src="../includes/ContactEditor2.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TestingAccount" Src="TestingAccount.ascx" %>
<%@ Control Language="c#" Inherits="ecn.accounts.includes.QuoteViewer" Codebehind="QuoteViewer.ascx.cs" %>
<%@ Register TagPrefix="uc1" TagName="UserInfoCollector" Src="UserInfoCollector.ascx" %>
<%@ Register TagPrefix="uc1" TagName="QuoteOptionSelector" Src="QuoteOptionSelector.ascx" %>
<style type="text/css" media="screen">@import url(/ecn.images/images/styles.css ); 
	</style>
<table class="tableContent" id="tblQuote" width="100%" border="0">
    <tr>
        <td colspan="2">
            <table cellspacing="1" width="100%" border="0">
                <tr>
                    <td>
                        <div id="contentText">
                            <asp:Literal ID="ltlDearCustomer" runat="server"></asp:Literal></div>
                    </td>
                    <td style="border-left: black 1px solid" valign="top" align="center">
                        <uc1:TestingAccount ID="testingLogin" runat="server"></uc1:TestingAccount>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="2">
            <asp:DataGrid ID="dgdQuoteItems" runat="server" AutoGenerateColumns="False" CellPadding="10"
                CssClass="grid" Width="100%">
                <AlternatingItemStyle CssClass="gridaltrow" />
                <ItemStyle Height="17" VerticalAlign="top"></ItemStyle>
                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                <Columns>
                    <asp:BoundColumn DataField="Quantity" HeaderText="Qty"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Name" HeaderText="Item"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Description" HeaderText="Description"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Frequency" HeaderText="Frequency" ItemStyle-HorizontalAlign="center"
                        HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Rate" HeaderText="Rate" ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                    <asp:BoundColumn DataField="ItemPrice" HeaderText="Total" ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                </Columns>
            </asp:DataGrid></td>
    </tr>
    <tr>
        <td height="10"></td>
    </tr>
    <tr>
        <td colspan="2">
            <table style="border-right: black 1px solid; padding-right: 5px; border-top: black 1px solid;
                padding-left: 5px; font-size: smaller; padding-bottom: 5px; border-left: black 1px solid;
                padding-top: 5px; border-bottom: black 1px solid; font-family: Arial" cellspacing="0"
                cellpadding="0" width="100%">
                <tr class="gridheader">
                    <td align="right" width="254"><strong>&nbsp; </strong></td>
                    <td align="right" width="175"><strong>One Time Fees</strong></td>
                    <td align="right" width="175"><strong>Monthly Fees</strong></td>
                    <td align="right" width="175"><strong>Quarterly Fees</strong></td>
                    <td align="right" width="175"><strong>Annual Fees</strong></td>
                </tr>
                <tr class="gridaltrow">
                    <td class="tableHeader" align="right"><strong>Total:</strong>&nbsp;</td>
                    <td class="tableHeader" align="right">
                        <asp:Label ID="lblOneTimeFees" runat="server"></asp:Label></td>
                    <td class="tableHeader" align="right">
                        <asp:Label ID="lblMonthlyFees" runat="server"></asp:Label></td>
                    <td class="tableHeader" align="right">
                        <asp:Label ID="lblQuarterlyFees" runat="server"></asp:Label></td>
                    <td class="tableHeader" align="right">
                        <asp:Label ID="lblAnnualFees" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="tableHeader" style="border-right: black 1px solid; width: 254px; border-bottom: black 1px solid"
                        align="right"><b>
                            <asp:Label ID="lblDiscount" runat="server">Discount</asp:Label></b></td>
                    <td class="tableContent" style="border-right: black 1px solid; color: red; border-bottom: black 1px solid"
                        align="right">
                        <asp:Label ID="lblOneTimeSaving" runat="server"></asp:Label></td>
                    <td class="tableContent" style="border-right: black 1px solid; color: red; border-bottom: black 1px solid"
                        align="right">
                        <asp:Label ID="lblMonthlySaving" runat="server"></asp:Label></td>
                    <td class="tableContent" style="border-right: black 1px solid; color: red; border-bottom: black 1px solid"
                        align="right">
                        <asp:Label ID="lblQuarterlySaving" runat="server"></asp:Label></td>
                    <td class="tableContent" style="color: red; border-bottom: black 1px solid" align="right">
                        <asp:Label ID="lblAnnualSaving" runat="server"></asp:Label></td>
                </tr>
                <tr bgcolor="#f4f4f4">
                    <td class="tableHeader" style="border-right: black 1px solid; width: 254px" align="right"
                        bgcolor="#ffffff"><b>
                            <asp:Label ID="lblNetAmount" runat="server">Net Amount</asp:Label></b></td>
                    <td class="tableContent" style="border-right: black 1px solid" align="right">
                        <asp:Label ID="lblOneTimeNetAmount" runat="server"></asp:Label></td>
                    <td class="tableContent" style="border-right: black 1px solid" align="right">
                        <asp:Label ID="lblMonthlyNetAmount" runat="server"></asp:Label></td>
                    <td class="tableContent" style="border-right: black 1px solid" align="right">
                        <asp:Label ID="lblQuarterlyNetAmount" runat="server"></asp:Label></td>
                    <td class="tableContent" align="right">
                        <asp:Label ID="lblAnnualNetAmount" runat="server"></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td height="15"></td>
    </tr>
    <tr>
        <td align="left" valign="top" rowspan="2">
            <table align="left" class="grid">
                <tr class="gridheader">
                    <td align="left">1. Company Information</td>
                </tr>
                <tr>
                    <td align="center" valign="top" class="gridaltrow">
                        <uc1:ContactEditor2 ID="Contact2" runat="server"></uc1:ContactEditor2>
                    </td>
                </tr>
            </table>
        </td>
        <asp:PlaceHolder ID="phOtherInfo"  runat="Server" Visible="true"    >
            <td valign="top">
                <table align="left" class="grid">
                    <tr class="gridheader">
                        <td align="left">2. Login Information</td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:UserInfoCollector ID="UserEditor" runat="server"></uc1:UserInfoCollector>
                        </td>
                    </tr>
                    <tr class="gridheader">
                        <td align="left">3. Activation</td>
                    </tr>
                    <tr>
                        <td valign="top" class="tableContent">Activate my license after
                            <asp:DropDownList ID="ddlStartDate" runat="server" class="formfield">
                                <asp:ListItem Value="0" Selected="True">Today</asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="16">16</asp:ListItem>
                                <asp:ListItem Value="17">17</asp:ListItem>
                                <asp:ListItem Value="18">18</asp:ListItem>
                                <asp:ListItem Value="19">19</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="21">21</asp:ListItem>
                                <asp:ListItem Value="22">22</asp:ListItem>
                                <asp:ListItem Value="23">23</asp:ListItem>
                                <asp:ListItem Value="24">24</asp:ListItem>
                                <asp:ListItem Value="25">25</asp:ListItem>
                                <asp:ListItem Value="26">26</asp:ListItem>
                                <asp:ListItem Value="27">27</asp:ListItem>
                                <asp:ListItem Value="28">28</asp:ListItem>
                                <asp:ListItem Value="29">29</asp:ListItem>
                                <asp:ListItem Value="30">30</asp:ListItem>
                            </asp:DropDownList>
                            days.<br>
                            <br>
                            <asp:CheckBox ID="chkAgree2" runat="server" Text="" TextAlign="Left"></asp:CheckBox>
                            <asp:Label ID="lblTerms" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblTermsConditions" runat="server">I have read and accept the terms and conditions stated in the <a href="http://www.knowledgemarketing.com/terms.php" target="_blank">KMLLC Services Agreement.</a></asp:Label>
                            <!--<asp:HyperLink id="hlAgreement" runat="server" NavigateUrl="http://www.knowledgemarketing.com/terms.php">KMLLC Services Agreement.</asp:HyperLink>-->
                        </td>
                    </tr>
                </table>
            </td>
        </asp:PlaceHolder>
    </tr>
</table>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>

<%@ Page Language="c#" Inherits="ecn.accounts.main.reports.AccountIntensity" CodeBehind="AccountIntensity.aspx.cs"
    MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="idMain" width="100%" cellspacing="0" cellpadding="0" border='0' align="center">
        <tr>
            <td>
                <cr:CrystalReportViewer ID="crv" runat="Server" Width="350px" Height="50px" ViewStateMode="Disabled"
                    Visible="false"></cr:CrystalReportViewer>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" cellspacing="0" cellpadding="0" border='0'>
                    <tr class="gradient">
                        <td width="50%" valign="middle" style="border: 1px #A4A2A3 solid; border-right: none;
                            font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;">
                            &nbsp;Account&nbsp;Intensity&nbsp;
                        </td>
                        <td width="50%" align='right' valign="middle" style="border: 1px #A4A2A3 solid; border-left: none;
                            font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;">
                            Download as&nbsp;:&nbsp;
                            <asp:ImageButton ID="lnkToPDF" runat="Server" ImageUrl="/ecn.images/images/icon-pdf.gif"
                                CausesValidation="false"></asp:ImageButton>
                            &nbsp;
                            <asp:ImageButton ID="lnktoExl" runat="Server" ImageUrl="/ecn.images/images/icon-xls.gif"
                                CausesValidation="false"></asp:ImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="offWhite borderSides" valign="top">
                            <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                                style="margin: 10px 0px">
                                <tr>
                                    <td width="30%" align='right'>
                                        <b>Channel&nbsp;:&nbsp;</b>
                                    </td>
                                    <td width="70%"  align="left">
                                        <asp:DropDownList ID="drpChannel" runat="Server" CssClass="formlabel" Width="200px"
                                            AutoPostBack="True" OnSelectedIndexChanged="drpChannel_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right' valign="top">
                                        <b>Customer&nbsp;:&nbsp;</b>
                                    </td>
                                    <td  align="left">
                                        <asp:DropDownList ID="drpCustomer" runat="Server" CssClass="formlabel" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right' valign="top">
                                        <b>Customer&nbsp;Type&nbsp;:&nbsp;</b>
                                    </td>
                                    <td  align="left">
                                        <asp:DropDownList ID="drpCustomerType" runat="Server" CssClass="formlabel" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="offWhite borderSides" valign="top">
                            <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                                style="margin: 10px 0px">
                                <tr>
                                    <td width="30%" align='right'>
                                        <b>Account Executive&nbsp;:&nbsp;</b>
                                    </td>
                                    <td width="70%"  align="left">
                                        <asp:DropDownList ID="drpAE" runat="Server" CssClass="formlabel" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='right'>
                                        <b>Account Manager&nbsp;:&nbsp;</b>
                                    </td>
                                    <td  align="left">
                                        <asp:DropDownList ID="drpAM" runat="Server" CssClass="formlabel" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="offWhite borderSides" colspan="2" align="center">
                            <asp:Button ID="btnSubmit" runat="Server" Text="Show Report" CssClass="formfield"
                                OnClick="btnSubmit_Click"></asp:Button>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="offWhite borderSides" align="center">
                            <asp:DataGrid ID="dgCustomers" runat="Server" CssClass="gridWizard" AutoGenerateColumns="False"
                                HorizontalAlign="center" Width="95%">
                                <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                                <AlternatingItemStyle HorizontalAlign="left" CssClass="gridaltrowWizard" />
                                <Columns>
                                    <asp:BoundColumn ItemStyle-Width="18%" DataField="basechannelName" HeaderText="Channel">
                                    </asp:BoundColumn>
                                    <asp:HyperLinkColumn ItemStyle-Width="20%" HeaderText="Customer" DataTextField="CustomerName"
                                        DataNavigateUrlFormatString="../customers/customerdetail.aspx?CustomerID={0}"
                                        DataNavigateUrlField="customerID"></asp:HyperLinkColumn>
                                    <asp:BoundColumn ItemStyle-Width="20%" DataField="CustomerType" HeaderText="Customer Type">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn ItemStyle-Width="8%" DataField="Createddate" HeaderText="Created"
                                        DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn ItemStyle-Width="5%" DataField="ActiveFlag" HeaderText="Active"
                                        HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                    <asp:BoundColumn ItemStyle-Width="15%" DataField="AMName" HeaderText="Manager"></asp:BoundColumn>
                                    <asp:BoundColumn ItemStyle-Width="8%" DataField="YTD" HeaderText="YTD<br />Email"
                                        HeaderStyle-HorizontalAlign='right' ItemStyle-HorizontalAlign='right'></asp:BoundColumn>
                                    <asp:BoundColumn ItemStyle-Width="6%" DataField="IsStrategic" HeaderText="Strategic"
                                        HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                    <asp:BoundColumn ItemStyle-Width="6%" DataField="Email" HeaderText="Email" HeaderStyle-HorizontalAlign="center"
                                        ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                    <asp:BoundColumn ItemStyle-Width="7%" DataField="Survey" HeaderText="Survey" HeaderStyle-HorizontalAlign="center"
                                        ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                    <asp:BoundColumn ItemStyle-Width="7%" DataField="DE" HeaderText="DE" HeaderStyle-HorizontalAlign="center"
                                        ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                    <asp:BoundColumn ItemStyle-Width="8%" DataField="vband" HeaderText="Email<br />vband"
                                        HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                    <asp:BoundColumn ItemStyle-Width="8%" DataField="tier" HeaderText="Tier" HeaderStyle-HorizontalAlign="center"
                                        ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="offWhite borderSides" align="center">
                            <AU:PagerBuilder ID="CustomersPager" runat="Server" Visible="false" ControlToPage="dgCustomers"
                                PageSize="25" Width="95%" OnIndexChanged="CustomersPager_IndexChanged">
                                <pagerstyle cssclass="gridpager"></pagerstyle>
                            </AU:PagerBuilder>
                        </td>
                    </tr>
                    <tr colspan="2">
                        <td class="gradient" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

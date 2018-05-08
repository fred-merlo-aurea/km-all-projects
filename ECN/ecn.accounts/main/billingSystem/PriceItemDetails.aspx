<%@ Page Language="c#" Inherits="ecn.accounts.main.billingSystem.PriceItemDetails"
    CodeBehind="PriceItemDetails.aspx.cs" Title="QuoteOpionDetails" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="uc1" TagName="PriceItemEditor" Src="../../includes/PriceItemEditor.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tableContent" width="100%" border='0'>
        <tr>
            <td class="tableHeader" align="right" width="15%" style="height: 16px">
                Base Channel:
            </td>
            <td width="85%" style="height: 16px" align="left">
                <asp:DropDownList ID="ddlChannels" runat="Server" AutoPostBack="True" Width="112px"
                    OnSelectedIndexChanged="ddlChannels_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" align="left" colspan="10">
                <hr width="100%" color="#000000" size="1">
            </td>
        </tr>
        <tr>
            <td colspan="2" width="100%" align="left">
                <p>
                    <uc1:PriceItemEditor ID="TechAccessPriceItemEditor" PanelTitle="Annual Tech Access"
                        runat="Server"></uc1:PriceItemEditor>
                </p>
            </td>
            <p>
            </p>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" width="100%" align="left">
                <p>
                    <uc1:PriceItemEditor ID="EmailUsagePriceItemEditor" PanelTitle="Email Usage" runat="Server">
                    </uc1:PriceItemEditor>
                </p>
            </td>
            <p>
            </p>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" width="100%" align="left">
                <p>
                    <uc1:PriceItemEditor ID="OptionPriceItemEditor" PanelTitle="Options" runat="Server">
                    </uc1:PriceItemEditor>
                </p>
            </td>
            <p>
            </p>
        </tr>
    </table>
</asp:Content>

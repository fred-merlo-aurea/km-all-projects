<%@ Page Language="c#" Inherits="ecn.accounts.customersmanager.LicenseEdit" CodeBehind="LicenseEdit.aspx.cs" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveDateTime" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
<br />
<asp:panel id="pnlViewEdit" runat="server" visible="true">
<table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>       
    <tbody> 
            <tr>
                <td colspan="2">
                    <br />
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
                        <br />
                        <br />
                    </asp:PlaceHolder>
                </td>
            </tr>           
            <tr>
            <td width="15%"></td>
                <td class="tableHeader" align="left">
                    &nbsp;Customer License</td>
            </tr>
            <tr>
                <td class="tableHeader"  align='right'>
                    Customer&nbsp;</td>
                <td align="left">
                    <asp:label id="lblCustomerName" runat="server" text=""></asp:label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    Quantity&nbsp;</td>
                <td align="left">
                    <asp:textbox id="tbQuantity" runat="Server" columns="10"  CssClass="formfield"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    &nbsp;Add Date&nbsp;</td>
                <td valign="top" class="tableContent" align="left">
                    <AU:ActiveDateTime runat="Server" ID="adtAddDate" BaseStyle-Font-Name="Tahoma" BaseStyle-Font-Bold="true"
                        DateStyle-BackColor="Lavender" TimeStyle-BackColor="Linen" Format="MONTH;/;DAY;/;YEAR;">
                    </AU:ActiveDateTime>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    &nbsp;Expiration Date&nbsp;</td>
                <td valign="top" class="tableContent" align="left">
                    <AU:ActiveDateTime runat="Server" ID="adtExpirationDate" BaseStyle-Font-Name="Tahoma"
                        BaseStyle-Font-Bold="true" DateStyle-BackColor="Lavender" TimeStyle-BackColor="Linen"
                        Format="MONTH;/;DAY;/;YEAR;">
                    </AU:ActiveDateTime>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    &nbsp;</td>
                <td class="tableHeader"  align="left">
                    <asp:button class="formbutton" id="btnUpdate" onclick="btnUpdate_Click" runat="Server"
                        visible="true" text="Update"></asp:button>
                    <asp:button class="formbutton" id="btnCancel" onclick="Cancel" runat="Server"
                        visible="true" text="Cancel"></asp:button>
                </td>
            </tr>
        </TBODY>
    </table>_
</asp:panel>
<asp:panel id="pnlError" runat="server" visible="false">
            <asp:label id="lblError" Runat="server" Font-Bold="True" ForeColor="red"></asp:label>
		</asp:panel>
</asp:content>

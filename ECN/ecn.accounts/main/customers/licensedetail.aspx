<%@ Page Language="c#" Inherits="ecn.accounts.customersmanager.licensedetail" CodeBehind="licensedetail.aspx.cs" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveDateTime" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server"> 
<table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
  <tbody>
            <tr>
                <td class="tableHeader" colspan="2"  align="left">
                    &nbsp;Customer License</td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                    Base Channel&nbsp;</td>
                <td align="left">
                    <asp:dropdownlist enableviewstate="true" id="BaseChannelList" runat="Server" datavaluefield="BaseChannelID"
                        datatextfield="BaseChannelName" autopostback="true" class="formfield" visible="true"
                        onselectedindexchanged="LoadChannelDDfromBaseChannel"></asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                    Channel&nbsp;</td>
                <td align="left">
                    <asp:dropdownlist id="ChannelList" runat="Server" datavaluefield="channelID" datatextfield="channelName"
                        class="formfield" autopostback="true" onselectedindexchanged="LoadCustomersDDfromChannels"></asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                    Customer&nbsp;</td>
                <td align="left">
                    <asp:dropdownlist id="CustomerID" runat="Server" datavaluefield="CustomerID" datatextfield="CustomerName"
                        class="formfield"></asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                    Type&nbsp;</td>
                <td align="left">
                    <asp:dropdownlist id="LicenseTypeCode" runat="Server" datavaluefield="CodeValue"
                        datatextfield="CodeName" class="formfield"></asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                    Quantity&nbsp;</td>
                <td align="left">
                    <asp:textbox id="Quantity" runat="Server" columns="10"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                    Used&nbsp;</td>
                <td align="left">
                    <asp:textbox id="Used" runat="Server" columns="10"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="top">
                    &nbsp;Add Date&nbsp;</td>
                <td valign="top" class="tableContent" align="left">
                    <AU:ActiveDateTime runat="Server" ID="AddDate" BaseStyle-Font-Name="Tahoma" BaseStyle-Font-Bold="true"
                        DateStyle-BackColor="Lavender" TimeStyle-BackColor="Linen" Format="MONTH;/;DAY;/;YEAR;">
                    </AU:ActiveDateTime>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="top">
                    &nbsp;Expiration Date&nbsp;</td>
                <td valign="top" class="tableContent" align="left">
                    <AU:ActiveDateTime runat="Server" ID="ExpirationDate" BaseStyle-Font-Name="Tahoma"
                        BaseStyle-Font-Bold="true" DateStyle-BackColor="Lavender" TimeStyle-BackColor="Linen"
                        Format="MONTH;/;DAY;/;YEAR;">
                    </AU:ActiveDateTime>
                </td>
            </tr>
            <tr>
            <td></td>
                <td class="tableHeader" align="left" colspan='2'>
                    <asp:textbox id="CLID" runat="Server" visible="false" enableviewstate="true"></asp:textbox>
                    <asp:button class="formbutton" id="SaveButton" onclick="CreateLicense" runat="Server"
                        visible="true" text="Create"></asp:button>
                    <asp:button class="formbutton" id="UpdateButton" onclick="UpdateLicense" runat="Server"
                        visible="false" text="Update"></asp:button>
                </td>
            </tr>
    </TBODY>
 </table>
</asp:content>

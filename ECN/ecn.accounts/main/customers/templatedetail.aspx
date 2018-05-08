<%@ Page Language="c#" Inherits="ecn.accounts.customersmanager.templatedetail" ValidateRequest="false"
    CodeBehind="templatedetail.aspx.cs" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveDateTime" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'
        align="left">
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
            <td></td>
                <td class="tableHeader" style="padding-top: 10px;" align="left">
                    Customer Template
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                    Customer&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlCustomerID" runat="Server" DataValueField="CustomerID" DataTextField="CustomerName"
                        class="formfield">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                    Choose Template&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlTemplateTypeCode" runat="Server" DataValueField="CodeValue"
                        DataTextField="CodeName" class="formfield" AutoPostBack="True" OnSelectedIndexChanged="TemplateTypeCode_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="middle">
                    Template&nbsp;<br />
                    Description&nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="tbTemplateDescription" runat="Server" Columns="90" Rows="4" TextMode="multiline"
                        class="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="middle">
                    Header&nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="tbHeaderSource" runat="Server" Columns="90" Rows="7" TextMode="multiline"
                        class="formfield"></asp:TextBox>
                </td>
            </tr>
            <asp:TextBox ID="tbFooterSource" runat="Server" Columns="60" Rows="7" TextMode="multiline"
                Visible="false"></asp:TextBox>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                    Active&nbsp;
                </td>
                <td align="left">
                    <asp:CheckBox ID="cbActiveFlag" runat="Server" />
                </td>
            </tr>
            </tr>
            <tr>
                <td class="tableHeader" align="center" colspan='3'>
                    <asp:Button class="formbutton" ID="btnSave" OnClick="btnSave_Click" runat="Server"
                        Visible="true" Text="Create"></asp:Button>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

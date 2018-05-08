<%@ Page Language="c#" Inherits="ecn.accounts.main.Digital.ConvertDE" CodeBehind="ConvertDE.aspx.cs" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="0" cellpadding="5" width="800" border='0'>
        <tr>
            <td class="tableHeader" align="left" width="20%">
                Channel :&nbsp;</td>
            <td class="tableHeader" align="left">
                <asp:label id="lblChannel" runat="Server"></asp:label>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" align="left">
                Customer :&nbsp;</td>
            <td class="tableHeader" align="left">
                <asp:label id="lblCustomer" runat="Server"></asp:label>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top">
                Publication :&nbsp;</td>
            <td class="tableHeader">
                <asp:label id="lblPublication" runat="Server"></asp:label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top" width="20%">
                Edition&nbsp;Name :&nbsp;</td>
            <td>
                <asp:textbox id="txtEditionName" runat="Server" cssclass="formfield" columns="40"></asp:textbox>
                <asp:requiredfieldvalidator id="rfv1" runat="Server" font-size="xx-small" controltovalidate="txtEditionName"
                    errormessage=">> required" font-italic="True" font-bold="True"></asp:requiredfieldvalidator>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top" align="left">
                Edition Type :</td>
            <td>
                <asp:dropdownlist id="drpEditionType" runat="Server" cssclass="formfield" width="125px">
					<asp:ListItem value="" Selected="True">----- Select Type -----</asp:ListItem>
					<asp:ListItem value="1Flyer">1 Page Flyer</asp:ListItem>
					<asp:ListItem value="2Flyer">2 Page Flyer</asp:ListItem>
					<asp:ListItem value="Catalog">Catalog</asp:ListItem>
					<asp:ListItem value="Magazine">Magazine</asp:ListItem>
				</asp:dropdownlist>
                <asp:requiredfieldvalidator id="rfvType" runat="Server" font-size="xx-small" controltovalidate="drpEditionType"
                    errormessage=">> required" font-italic="True" font-bold="True"></asp:requiredfieldvalidator>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top" align="left">
                Status:</td>
            <td>
                <asp:dropdownlist id="drpStatus" runat="Server" cssclass="formfield" width="100px">
					<asp:ListItem value="Active" Selected="True">Active</asp:ListItem>
					<asp:ListItem value="InActive">InActive</asp:ListItem>
					<asp:ListItem value="Archieve">Archieve</asp:ListItem>
				</asp:dropdownlist>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top">
                Activation Date :</td>
            <td>
                <asp:textbox id="txtActivationDate" runat="Server" cssclass="formfield" columns="20"></asp:textbox>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top">
                De-Activation Date :</td>
            <td>
                <asp:textbox id="txtDeActivationDate" runat="Server" cssclass="formfield" columns="20"></asp:textbox>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top">
                Uploaded PDF File :&nbsp;</td>
            <td class="label10">
                <asp:label id="lblFileName" cssclass="label" runat="Server"></asp:label>
                &nbsp;[<asp:hyperlink id="hlPDFlink" runat="Server"></asp:hyperlink>]</td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top">
                PDF File :</td>
            <td>
                <input class="blue_border_box" id="FileUpload1" type="file" size="40" name="FileUpload1"
                    runat="Server">
                <asp:regularexpressionvalidator id="rev2" runat="Server" font-bold="True" font-italic="True"
                    errormessage="Only PDF is allowed!" controltovalidate="FileUpload1" font-size="xx-small"
                    validationexpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.PDF)$"></asp:regularexpressionvalidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:label id="lblErrorMessage" runat="Server" forecolor="Red" font-size="x-small"
                    visible="False"></asp:label>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" align="center" colspan="2">
                <br />
                <asp:button class="formbutton" id="SaveButton" runat="Server" visible="true" text="Convert To Digital Edition"
                    onclick="SaveButton_Click"></asp:button>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" colspan="2">
                &nbsp;</td>
        </tr>
    </table>
</asp:content>

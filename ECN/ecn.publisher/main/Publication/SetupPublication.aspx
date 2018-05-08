<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetupPublication.aspx.cs"
    Inherits="ecn.publisher.main.Publication.SetupPublication" MasterPageFile="~/MasterPages/Publisher.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Publisher.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin: 10px 0; padding: 0;" align="center">
        <table width="100%" cellspacing="0" cellpadding="0" border='0'>
            <tr>
                <td valign="bottom" align="left" style="width: 1018px">
                    <table cellspacing="0" cellpadding="0" border='0'>
                        <tr>
                            <td class="steps tabNav" valign="bottom">
                                <asp:ImageButton ID="ibStep1" runat="Server" CommandArgument="1" CausesValidation="True"
                                    OnCommand="ibStep_Command"></asp:ImageButton>
                            </td>
                            <td class="steps tabNav" valign="bottom">
                                <asp:ImageButton ID="ibStep2" runat="Server" CommandArgument="2" CausesValidation="True"
                                    OnCommand="ibStep_Command"></asp:ImageButton>
                            </td>
                            <td class="steps tabNav" valign="bottom">
                                <asp:ImageButton ID="ibStep3" runat="Server" CommandArgument="3" CausesValidation="True"
                                    OnCommand="ibStep_Command"></asp:ImageButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gradient buttonPad" align='right' valign="middle" style="width: 1018px">
                    <ul class="surveyNav">
                        <!-- items are in reverse order because they're floated right -->
                        <li>
                            <asp:LinkButton ID="lbtnNext1" CssClass="btnbgGreen" runat="Server" Text="Next&nbsp;&raquo;" OnClick="lbtnNext_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbtnPrevious1" CssClass="btnbgGray" runat="Server" CausesValidation="False" Text="&laquo;&nbsp;Previous"
                                OnClick="lbtnPrevious_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbtnCancel1" CssClass="btnbgRed" runat="Server" CausesValidation="False" Text="Cancel"
                                OnClick="lbtnCancel_Click"></asp:LinkButton>
                        </li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td class="greyOutSide offWhite center label" style="width: 1018px">
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
                <td class=" greyOutSide offWhite center label" style="width: 1018px">
                    <asp:Panel ID="pnl1" runat="server" Visible="true">
                        <div class="section">
                            <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                <tr>
                                    <td valign="middle" align="center" width="5%">
                                        <img src="/ecn.images/images/sendMyCampaign.gif" />
                                    </td>
                                    <td class="headingOne" width="95%">
                                        Publication Details&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="layoutWrapper" cellspacing="0" cellpadding="5" width="800" border="0">
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right" width="150">
                                        <font color="red">*</font> Publication Name :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblGroupID" runat="Server" visible="false" Text="0" />
                                        <asp:TextBox ID="tbPublicationName" runat="Server" Columns="40" CssClass="label10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv1" runat="Server" Font-Bold="True" Font-Italic="True"
                                            ErrorMessage=">> required" ControlToValidate="tbPublicationName" Font-Size="xx-small"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                        <font color="red">*</font> Publication Type :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlPublicationType" runat="Server" Width="125px" CssClass="label10">
                                            <asp:ListItem Value="" Selected="True">----- Select Type -----</asp:ListItem>
                                            <asp:ListItem Value="1Flyer">1 Page Flyer</asp:ListItem>
                                            <asp:ListItem Value="2Flyer">2 Page Flyer</asp:ListItem>
                                            <asp:ListItem Value="AnnualReport">Annual Report</asp:ListItem>
                                            <asp:ListItem Value="Brochure">Brochure</asp:ListItem>
                                            <asp:ListItem Value="Catalog">Catalog</asp:ListItem>
                                            <asp:ListItem Value="Journal">Journal</asp:ListItem>
                                            <asp:ListItem Value="Magazine">Magazine</asp:ListItem>
                                            <asp:ListItem Value="Newspaper">Newspaper</asp:ListItem>
                                            <asp:ListItem Value="Newsletter">Newsletter</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlPublicationType" runat="Server" Font-Bold="True"
                                            Font-Italic="True" ErrorMessage=">> required" ControlToValidate="ddlPublicationType"
                                            Font-Size="xx-small"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                        Category :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlCategory" runat="Server" Width="200" CssClass="label10" DataTextField="CategoryName" DataValueField="CategoryID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                        Frequency :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlFrequency" runat="Server" Width="200" CssClass="label10"  DataTextField="FrequencyName" DataValueField="FrequencyID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                        Publication&nbsp;URL :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="tbPublicationAlias" runat="Server" Columns="20" CssClass="label10"
                                            MaxLength="50"></asp:TextBox>
                                        <font class="formLabel">.ecndigitaledition.com</font> &nbsp;&nbsp;<asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" Font-Bold="True" Font-Italic="True"
                                            ErrorMessage=">> Invalid Alias (only numbers and Text are allowed)" Font-Size="XX-Small"
                                            ControlToValidate="tbPublicationAlias" ValidationExpression="[a-zA-Z0-9]*" Font-Overline="False"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                        Circulation :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="tbCirculation" runat="Server" Columns="20" CssClass="label10" MaxLength="10"></asp:TextBox>&nbsp;<font
                                            class="formLabel">use only [0-9]</font> &nbsp;&nbsp;<asp:RegularExpressionValidator
                                                ID="rfvtbCirculation" runat="server" Font-Bold="True" Font-Italic="True" ErrorMessage=">> Only numbers allowed"
                                                Font-Size="XX-Small" ControlToValidate="tbCirculation" ValidationExpression="[0-9]*"
                                                Font-Overline="False"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px" class="formLabel" valign="middle" align="right">
                                        Active :
                                    </td>
                                    <td class="tableContent" align="left">
                                        <asp:CheckBox ID="cbActive" runat="Server" Checked="true"></asp:CheckBox>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnl2" runat="server" Visible="true">
                        <table border="0" cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td width="5%">
                                </td>
                                <td width="95%">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="middle">
                                    <img src="/ecn.images/images/sendMyCampaign.gif">
                                </td>
                                <td class="headingOne">
                                    Subscribe Options
                                </td>
                            </tr>
                            <asp:Panel ID="pnltemp" runat="server" Visible="false">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="3" valign="bottom">
                                        <asp:RadioButton ID="rbSubOptn1" CssClass="formLabel" GroupName="grpSubscribe" Text="Require readers to complete registration form to view the publication."
                                            runat="Server"></asp:RadioButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="3" valign="bottom">
                                        <asp:RadioButton ID="rbSubOptn2" CssClass="formLabel" GroupName="grpSubscribe" Text="Require readers email address to view the publication."
                                            runat="Server"></asp:RadioButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="3" valign="bottom">
                                        <asp:RadioButton ID="rbSubOptn3" CssClass="formLabel" GroupName="grpSubscribe" Text="Require readers email address & Zip Code to view publication."
                                            runat="Server"></asp:RadioButton>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3" valign="bottom">
                                    <asp:RadioButton ID="rbSubOptn4" CssClass="formLabel" GroupName="grpSubscribe" Text="Enable Subscribe option in Digital Edition."
                                        runat="Server" Checked="true"></asp:RadioButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3" valign="bottom">
                                    <asp:RadioButton ID="rbSubOptn5" CssClass="formLabel" GroupName="grpSubscribe" Text="Enable Subscribe option and redirect to subscribe form"
                                        runat="Server"></asp:RadioButton>&nbsp;<asp:TextBox ID="tbSubscriptionFormLink"
                                            runat="Server" CssClass="label10" Columns="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3" valign="bottom">
                                    <asp:RadioButton ID="rbSubOptn6" CssClass="formLabel" GroupName="grpSubscribe" Text="Disable Subscribe option"
                                        runat="Server"></asp:RadioButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnl3" runat="server" Visible="true">
                        <table cellspacing="0" cellpadding="5" width="100%" border="0">
                            <tr>
                                <td align="center" valign="middle" width="5%">
                                    <img src="/ecn.images/images/sendMyCampaign.gif">
                                </td>
                                <td class="headingOne" width="95%">
                                    Select Publication Logo
                                </td>
                            </tr>
                        </table>
                        <div class="section">
                            <table cellspacing="0" cellpadding="5" width="800" border='0'>
                                <tr>
                                    <td class="formLabel" valign="middle" align="right" style="padding-right: 20px" width="150">
                                        Current Logo:&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:Image runat="server" ID="imgLogo" ImageUrl="http://www.ecndigitaledition.com/images/km-logo-plate.gif">
                                        </asp:Image>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" valign="middle" align="right" style="padding-right: 20px">
                                        Upload New Logo:&nbsp;
                                    </td>
                                    <td align="left">
                                        <input class="dataOne" id="fBrowse" type="file" name="fBrowse" runat="server">&nbsp;<span
                                            class="highLightOne">(use only JPG & GIF image and size should be 140 x 34 pixles)</span>
                                        <asp:Label ID="lblogoURL" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" valign="middle" align="right" style="padding-right: 20px">
                                        Logo Link:&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="tbLogoLink" runat="Server" CssClass="label10" Columns="100"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="section">
                            <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                <tr>
                                    <td width="5%">
                                    </td>
                                    <td width="95%">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="middle">
                                        <img src="/ecn.images/images/sendMyCampaign.gif">
                                    </td>
                                    <td class="headingOne">
                                        Contact Information displayed in Digital Edition:
                                    </td>
                                </tr>
                            </table>
                            <table cellspacing="0" cellpadding="5" width="800" border='0'>
                                <tr>
                                    <td class="formLabel" valign="middle" align="right" style="padding-right: 20px" width="150">
                                        Email Address :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="tbContactEmail" runat="Server" CssClass="label10" Columns="40"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" valign="middle" align="right" style="padding-right: 20px">
                                        Phone :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="tbContactPhone" runat="Server" CssClass="label10" Columns="40"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" valign="middle" align="right" style="padding-right: 20px">
                                        Address :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="tbContactAddress1" runat="Server" CssClass="label10" Columns="40"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" valign="middle" align="right" style="padding-right: 20px">
                                        City,State &amp; Zip :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="tbContactAddress2" runat="Server" CssClass="label10" Columns="40"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" valign="middle" align="right" style="padding-right: 20px">
                                        <b>(or)&nbsp;</b>
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" valign="middle" align="right" style="padding-right: 20px">
                                        Contact Us Link :&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="tbContactFormLink" runat="Server" CssClass="label10" Columns="100"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td class="gradient buttonPad" valign="middle" align='right' style="width: 1018px">
                    <ul class="surveyNav">
                        <!-- items are in reverse order because they're floated right -->
                        <li>
                            <asp:LinkButton ID="lbtnNext2" CssClass="btnbgGreen" runat="Server" Text="Next&nbsp;&raquo;" OnClick="lbtnNext_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbtnPrevious2" CssClass="btnbgGray" runat="Server" CausesValidation="False" Text="&laquo;&nbsp;Previous"
                                OnClick="lbtnPrevious_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbtnCancel2" CssClass="btnbgRed" runat="Server" CausesValidation="False" Text="Cancel"
                                OnClick="lbtnCancel_Click"></asp:LinkButton>
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>

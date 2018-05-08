<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs"
    Inherits="PaidPub.main.Pricing.Add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="contentheader">
        Pricing
    </div>
    <br />
    <div class="padding20">
        <div class="box">
            <div class="boxheader">
                Add/Edit Pricing</div>
            <div class="boxcontent">
                <table cellpadding="5" cellspacing="0" border="0">
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left">
                            Name :
                        </td>
                        <td width="50%" align="left">
                            <asp:TextBox ID="txtName" runat="server" Width="250" MaxLength="100"></asp:TextBox>
                        </td>
                        <td width="20%" align="left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                ErrorMessage="&gt;&gt; requried" Font-Bold="True" Font-Italic="False" Font-Size="X-Small"
                                Font-Strikeout="False" Font-Underline="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left">
                            Description :
                        </td>
                        <td width="50%" align="left">
                            <asp:TextBox ID="txtDescription" runat="server" Width="250" MaxLength="200"></asp:TextBox>
                        </td>
                        <td width="20%" align="left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" nowrap colspan="3">
                            Rates for
                            <asp:DropDownList ID="drpCombo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCombo_SelectedIndexChanged">
                                <asp:ListItem Text="1" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10+" Value="10"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;Newsletter(s)
                            <br />
                            <br />
                            <asp:RadioButton ID="rbWith" runat="server" GroupName="group1" Text="for" AutoPostBack="true"
                                OnCheckedChanged="rbWith_CheckedChanged" />&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="drpNewsletter2"
                                    runat="server">
                                </asp:DropDownList>
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="drpNewsletter2"
                                ErrorMessage="&gt;&gt; requried" Font-Bold="True" Font-Italic="False" Font-Size="X-Small"
                                Font-Strikeout="False" Font-Underline="False" InitialValue=""></asp:RequiredFieldValidator><br />
                            <asp:RadioButton ID="rbWithout" runat="server" GroupName="group1" Text="default pricing excluding "
                                AutoPostBack="true" OnCheckedChanged="rbWithout_CheckedChanged" />&nbsp;&nbsp;&nbsp;<asp:DropDownList
                                    ID="drpNewsletter3" runat="server">
                                </asp:DropDownList>
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="drpNewsletter3"
                                ErrorMessage="&gt;&gt; requried" Font-Bold="True" Font-Italic="False" Font-Size="X-Small"
                                Font-Strikeout="False" Font-Underline="False" InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <asp:Panel ID="pnlPricing" Visible="false" runat="server">
                        <tr>
                            <td colspan="3" align="left">
                                <b>Newsletter Pricing</b>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table cellpadding="5" cellspacing="0" border="0" width="70%">
                                    <thead>
                                        <tr>
                                            <td width="20%">
                                                Year(s)
                                            </td>
                                            <td width="40%">
                                                Regular Rate
                                            </td>
                                            <td width="40%">
                                                Actual Rate
                                            </td>
                                        </tr>
                                    </thead>
                                    <tr>
                                        <td align="left">
                                            1 year
                                        </td>
                                        <td align="left">
                                            $<asp:TextBox ID="txtRegRate1" runat="server" Width="50"></asp:TextBox>&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtRegRate1"
                                                ErrorMessage="*" Font-Bold="True" Font-Italic="False" Font-Size="X-Small" Font-Strikeout="False"
                                                Font-Underline="False"></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="left">
                                            $<asp:TextBox ID="txtActualRate1" runat="server" Width="50"></asp:TextBox>&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtActualRate1"
                                                ErrorMessage="*" Font-Bold="True" Font-Italic="False" Font-Size="X-Small" Font-Strikeout="False"
                                                Font-Underline="False"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            2 years
                                        </td>
                                        <td align="left">
                                            $<asp:TextBox ID="txtRegRate2" runat="server" Width="50"></asp:TextBox>&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtRegRate2"
                                                ErrorMessage="*" Font-Bold="True" Font-Italic="False" Font-Size="X-Small" Font-Strikeout="False"
                                                Font-Underline="False"></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="left">
                                            $<asp:TextBox ID="txtActualRate2" runat="server" Width="50"></asp:TextBox>&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtActualRate2"
                                                ErrorMessage="*" Font-Bold="True" Font-Italic="False" Font-Size="X-Small" Font-Strikeout="False"
                                                Font-Underline="False"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            3 years
                                        </td>
                                        <td align="left">
                                            $<asp:TextBox ID="txtRegRate3" runat="server" Width="50"></asp:TextBox>&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtRegRate3"
                                                ErrorMessage="*" Font-Bold="True" Font-Italic="False" Font-Size="X-Small" Font-Strikeout="False"
                                                Font-Underline="False"></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="left">
                                            $<asp:TextBox ID="txtActualRate3" runat="server" Width="50"></asp:TextBox>&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtActualRate3"
                                                ErrorMessage="*" Font-Bold="True" Font-Italic="False" Font-Size="X-Small" Font-Strikeout="False"
                                                Font-Underline="False"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%" align="left">
                                Addl Discount :
                            </td>
                            <td width="50%" align="left">
                                <asp:TextBox ID="txtAddlDiscount" runat="server" Width="50" MaxLength="5"></asp:TextBox>%
                            </td>
                            <td width="20%" align="left">
                                &nbsp;
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td colspan="3" align="left">
                            <br />
                            <asp:Label ID="lblErrorMessage" runat="server" Visible="false" CssClass="error"></asp:Label>
                            <br />
                            <asp:Button CssClass="blackButton" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click">
                            </asp:Button>&nbsp;
                            <asp:Button CssClass="blackButton" ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                OnClick="btnCancel_Click"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

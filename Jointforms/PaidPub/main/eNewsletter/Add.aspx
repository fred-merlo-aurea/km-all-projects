<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs"
    Inherits="PaidPub.main.eNewsletter.Add" %>
<%@ MasterType VirtualPath="~/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="contentheader">
        eNewsletters
    </div>
    <br />
    <div class="padding20">
        <div class="box">
            <div class="boxheader">
                Add/Edit eNewsletter</div>
            <div class="boxcontent">
                <table cellpadding="5" cellspacing="0" border="0">
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left">
                            Newsletter Name :
                        </td>
                        <td width="50%" align="left">
                            <asp:TextBox ID="txtName" runat="server" Width="200" MaxLength="50"></asp:TextBox>
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
                            <asp:TextBox ID="txtDescription" runat="server" Width="200" MaxLength="200"></asp:TextBox>
                        </td>
                        <td width="20%" align="left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left">
                            Frequency :
                        </td>
                        <td width="50%" align="left">
                            <asp:DropDownList ID="drpFrequency" runat="server" Width="200">
                            </asp:DropDownList>
                        </td>
                        <td width="20%" align="left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpFrequency"
                                ErrorMessage="&gt;&gt; requried" Font-Bold="True" Font-Italic="False" Font-Size="X-Small"
                                InitialValue="0" Font-Strikeout="False" Font-Underline="False"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                     <tr>
                        <td width="20%" align="left">
                            Category :
                        </td>
                        <td width="50%" align="left">
                            <asp:DropDownList ID="drpCategory" runat="server" Width="200">
                            </asp:DropDownList>
                        </td>
                        <td width="20%" align="left">
                            &nbsp;
                        </td>
                    </tr>
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

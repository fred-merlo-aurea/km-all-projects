<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs"
    Inherits="PaidPub.main.Promotions.Add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="contentheader">
        Promotions
    </div>
    <br />
    <div class="padding20">
        <div class="box">
            <div class="boxheader">
                Add/Edit Promotions</div>
            <div class="boxcontent">
                <table cellpadding="5" cellspacing="0" border="0">
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left">
                            Promotion Name :
                        </td>
                        <td width="50%" align="left">
                            <asp:TextBox ID="txtPromotionName" runat="server" Width="200"></asp:TextBox>
                        </td>
                        <td width="20%" align="left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPromotionName"
                                ErrorMessage="&gt;&gt; requried" Font-Bold="True" Font-Size="X-Small"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left">
                            Promotion Description :
                        </td>
                        <td width="50%" align="left">
                            <asp:TextBox ID="txtDesc" runat="server" Width="200" MaxLength="200"></asp:TextBox>
                        </td>
                        <td width="20%" align="left">&nbsp;
                        </td>
                    </tr>
                     <tr>
                        <td width="20%" align="left">
                            Promotion Code :
                        </td>
                        <td width="50%" align="left">
                            <asp:TextBox ID="txtPromotionCode" runat="server" Width="50" MaxLength="10"></asp:TextBox>
                        </td>
                        <td width="20%" align="left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPromotionCode"
                                ErrorMessage="&gt;&gt; requried" Font-Bold="True" Font-Size="X-Small"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left">
                            Discount % :
                        </td>
                        <td width="50%" align="left">
                            <asp:TextBox ID="txtDiscount" runat="server" Width="50" MaxLength="5"></asp:TextBox>
                        </td>
                        <td width="20%" align="left">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" align="left">
                            Status :
                        </td>
                        <td width="50%" align="left">
                            
                            <asp:RadioButtonList ID="rbStatus" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="1">Active</asp:ListItem>
                                <asp:ListItem Value="0">Inactive</asp:ListItem>
                            </asp:RadioButtonList>
                            
                        </td>
                        <td width="20%" align="left">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                        <br />
                            <asp:Label ID="lblErrorMessage" runat="server" Visible="false" CssClass="error"></asp:Label>
                            <br />
                            <asp:Button CssClass="blackButton" ID="btnSave" runat="server" Text="Save" 
                                onclick="btnSave_Click"></asp:Button>&nbsp;
                            <asp:Button CssClass="blackButton" ID="btnCancel" runat="server" Text="Cancel" 
                                CausesValidation="false" onclick="btnCancel_Click"></asp:Button>
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WattWebService_TEST._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body
        {
            font-family: Arial;
            font-size: small;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Label ID="lblErrorMessage" runat="server" Font-Bold="true" Font-Size="Small"
        ForeColor="Red"></asp:Label>
    <asp:Panel ID="pnlAddProfile" runat="server">
        <fieldset>
            <legend>UDF</legend>
            <table>
                <tr>
                    <td width="25%">
                        Access Key:
                    </td>
                    <td width="75%">
                        <asp:TextBox ID="tbAccessKey" runat="server" Text="2B4E4F20-B642-457D-8407-DB82F1BDC401"
                            Width="300" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        PubCode:
                    </td>
                    <td width="75%">
                        <asp:DropDownList ID="drpPubCodes" runat="server">
                            <asp:ListItem Text="WATTAGNET" Value="WATTAGNET" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="PETI" Value="PETI"></asp:ListItem>
                            <asp:ListItem Text="CFDM" Value="CFDM"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnGetUDF" runat="server" Text="Get UDF" OnClick="btnGetUDF_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:DataGrid ID="dgUDFs" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundColumn DataField="shortname" HeaderText="UDF Name"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
        </fieldset>
        <br />
        <fieldset>
            <legend>Add UDF</legend>
            <asp:Panel ID="pnlCreateUDF" runat="server">
                <table>
                    <tr>
                        <td width="25%">
                            UDF Name:
                        </td>
                        <td width="75%">
                            <asp:TextBox Width="200" ID="tbNewUdfName" runat="server"></asp:TextBox>&nbsp;<asp:Button
                                ID="btnNewUDF" runat="server" OnClick="btnNewUDF_Click" Text="Create UDF" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Result:
                            <asp:Label ID="lblResult1" runat="server" Font-Bold="true" Font-Size="X-Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </fieldset>
        <br />
        <br />
        <fieldset>
            <legend>Get Subscriber</legend>
            <asp:Panel ID="Panel1" runat="server">
                <table>
                    <tr>
                        <td width="25%">
                            Emailaddress:
                        </td>
                        <td width="75%">
                            <asp:TextBox Width="200" ID="txtGetEmailaddress" runat="server"></asp:TextBox>&nbsp;<asp:Button
                                ID="btnGetSubscriber" runat="server" OnClick="btnGetSubscriber_Click" Text="GetSubscriber" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Result:
                            <asp:Label ID="lblGerSubscriberResult" runat="server" Font-Bold="true" Font-Size="X-Small"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </fieldset>
        <fieldset>
            <legend>Add Subscriber</legend>
            <table>
                <tr>
                    <td width="25%">
                        Ektron User Name (<b>PrimaryKey</b>):
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbUserName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Email:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbEmail" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Birthday:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbBirthDay" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        First Name:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbFirstName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Last Name:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbLastName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fullname:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbFullname" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Company:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbCompanyName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        AddressLine1:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbAddress1" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        AddressLine2:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbAddress2" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        City:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbCity" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        State:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbState" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        PostalCode:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbPostalCode" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Country:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbCountry" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Phone:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbPhone" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mobile:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbMobile" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fax:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbFax" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Title:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbTitle" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Occupation:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbOccupation" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Gender:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbGender" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Income:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbIncome" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Password:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbPassword" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Age:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbAge" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Website:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbWebsite" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        NonUsProvince:
                    </td>
                    <td width="75%">
                        <asp:RadioButtonList ID="rbNonUSProvince" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Industry:
                    </td>
                    <td width="75%">
                        <asp:TextBox Width="200" ID="tbIndustry" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        SponsoredLinks:
                    </td>
                    <td width="75%">
                        <asp:RadioButtonList ID="rbSponsoredLinks" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        SideRoads:
                    </td>
                    <td width="75%">
                        <asp:RadioButtonList ID="rbSideRoads" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        VideoAlert:
                    </td>
                    <td width="75%">
                        <asp:RadioButtonList ID="rbVideoAlert" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend>Newsletters</legend>
                            <table>
                                <tr>
                                    <td>
                                        Agribusiness:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbAgribusiness" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        DesignTips:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbDesignTips" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        EquipmentTips:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbEquipmentTips" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Feed_ENews:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbFeed_ENews" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr><tr>
                                    <td>
                                        IA_Ciberboletin:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbIA_Ciberboletin" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr><tr>
                                    <td>
                                        Management_Tips:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbManagement_Tips" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr><tr>
                                    <td>
                                        Pet_ENews:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbPet_ENews" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr><tr>
                                    <td>
                                        Petfood_Nutrition:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbPetfood_Nutrition" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr><tr>
                                    <td>
                                        Pig_ENews:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbPig_ENews" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr><tr>
                                    <td>
                                        Shoptips:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbShoptips" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr><tr>
                                    <td>
                                        TipSheet:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbTipSheet" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr><tr>
                                    <td>
                                        WPOU_ENews:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbWPOU_ENews" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr><tr>
                                    <td>
                                        PoultryUpdate:
                                    </td>
                                    <td width="75%">
                                        <asp:RadioButtonList ID="rbPoultryUpdate" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Value="N" Selected="True" Text="No"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnAddProfile" runat="server" OnClick="btnAddProfile_Click" Text="Add Profile" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Result:<asp:Label ID="lblResult2" runat="server" Font-Bold="true" Font-Size="X-Small"
                            ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <br />
    <br />
    </form>
</body>
</html>

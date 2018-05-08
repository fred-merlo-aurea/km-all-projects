<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="ECN_SF_Integration_old.aspx.cs" Inherits="ecn.communicator.main.Salesforce.ECN_SF_Integration" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
.popupbody
{
    /*background:#fffff url(images/blank.gif) repeat-x top;*/
    z-index: 101;
    background-color: #FFFFFF;
    font-family: calibri, trebuchet ms, myriad, tahoma, verdana;
    font-size: 16px;
}

.modalBackground
{
    background-color: Gray;
    filter: alpha(opacity=70);
    opacity: 0.7;
}

.ECN-Label-Heading-Large
{
    font-size: large;
    font-family: Arial, Verdana, Helvetica, sans-serif;
    color: slategray;
    font-weight: 600;
    padding:0px;
    text-align:left;
}
.ECN-Label-Heading-Large:hover
{
    font-size: large;
    font-family: Arial, Verdana, Helvetica, sans-serif;
    color: slategray;
    font-weight: 600;
    cursor:pointer;
    text-align:left;
}

.ECN-PageHeading
{
    font-family: Arial, Verdana, Helvetica, sans-serif;
    font-size: x-large;
    color: slategray;
    font-weight: 100;
}
 .ECN-Button-Heading-Medium-
        {
	        font-size:medium;
	        font-family: 'Segoe UI' ,Arial;
	        color: slategray;
            font-weight: 600;
        }
 .ECN-Button-Heading-Medium:hover
        {
	        font-size:medium;
	        font-family: 'Segoe UI' ,Arial;
	        color: slategray;
            font-weight: 600;
            cursor:pointer;
        }


</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
    <div style="text-align: center;">
        <asp:Label ID="lblSyncHeading" runat="server" Text="Sync" CssClass="PageHeading" Font-Bold="true" Font-Size="XX-Large"></asp:Label>
        <asp:Label ID="lblImportHeading" runat="server" Text="Import" CssClass="PageHeading" Font-Bold="true" Font-Size="XX-Large"></asp:Label>
    </div>
    <br />

    <KM:Message ID="kmMsg" runat="server" />

    <table style="width: 100%; height: 450px; border: thin; border-color: lightgray;" border="1">
        <tr style="vertical-align: top;">

            <td style="border-top: none; border-left: none; border-bottom: none; border-right: none; padding-left: 30px; text-align: left;">
                <br />
                <table style="width:100%;">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlHome" Width="100%" Height="100%" Visible="false" runat="server">
                                <table style="width:100%;height:100%;">
                                    <tr style="height: 120px;">
                                        <td style="width:10%; text-align:center;">
                                            <asp:Image ID="imgImportHome" ImageUrl="http://images.ecn5.com/images/ImportOrange.jpg" runat="server" />
                                        </td>
                                        <td style="width:40%;">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td style="text-align:left;">
                                                        <asp:Button ID="btnImportHome" CssClass="ECN-Label-Heading-Large" runat="server" BackColor="Transparent" BorderStyle="None"  Text="Import" OnClick="btnImportHome_Click" />
                                                    </td>
                                                </tr>
                                                <tr style="text-align:left;">
                                                    <td style="text-align:left;">
                                                        <asp:Label ID="lblImportHomr" Font-Size="Medium" Text="Import your Salesforce campaign members into an existing or new ECN group" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width:10%; text-align:center;">
                                            <img src="http://images.ecn5.com/images/SForceButton.jpg" alt="" />
                                        </td>
                                        <td style="width:40%;">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnLogIn" Text="Login" CssClass="ECN-Label-Heading-Large" OnClick="btnSF_Login_Click" BackColor="Transparent" BorderStyle="None" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblLogIn" Font-Size="Medium" Text="Log in to Salesforce to get started" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        
                                    </tr>
                                    <tr style="height:120px;">
                                        <td style="width:10%; text-align:center;">
                                            <asp:Image ID="imgSyncHome" ImageUrl="http://images.ecn5.com/images/Sync.jpg" runat="server"/>
                                        </td>
                                        <td style="width:40%;">
                                            <table style="text-align:left;width:100%;">
                                                <tr style="text-align:left;">
                                                    <td style="text-align:left;">
                                                        <asp:Button ID="btnSyncHome" CssClass="ECN-Label-Heading-Large" BackColor="Transparent" BorderStyle="None"  Text="Sync" runat="server" OnClick="btnSyncHome_Click" />
                                                    </td>
                                                </tr>
                                                <tr style="text-align:left;">
                                                    <td style="text-align:left;">
                                                        <asp:Label ID="lblSyncHome" Font-Size="Medium" Text="Sync your contacts, leads, master suppression list, and email opt-out lists between Salesforce and ECN" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlImport" Width="100%" Height="100%" Visible="false" runat="server">
                                <table runat="server"  style="width:100%;" ID="tblImport">
                                    
                                    <tr>
                                        <td style="text-align:center; width:10%;">
                                            <asp:Image ID="imgImportCampaignMember" ImageUrl="http://images.ecn5.com/images/CampaignMembers.jpg" runat="server" />
                                        </td>
                                        <td style="width:40%;">
                                            <table style="text-align:left; width:100%;">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnImportSFCampaign" runat="server" OnClick="btnImportSFCampaign_Click" CssClass="ECN-Label-Heading-Large" Text="Salesforce Campaign Members" BackColor="Transparent" BorderStyle="None" />
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </td>
                                        <td style="width:10%;">

                                        </td>
                                        <td style="width:40%;">

                                        </td>
                                    </tr></table></asp:Panel>
                            <asp:Panel ID="pnlSync" Width="100%" Height="100%" Visible="false" runat="server">
                                <table id="tblSync" style="width:100%;" runat="server">
                                    <tr style="height:80px;">
                                        <td style="width:10%;text-align:center;">
                                            <asp:Image ID="imgSyncContacts" ImageUrl="http://images.ecn5.com/images/Contacts.jpg" runat="server" />
                                        </td><td style="text-align:left;">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSyncContacts" OnClick="btnSyncContacts_Click" CssClass="ECN-Label-Heading-Large" Text="Contacts" runat="server" BorderStyle="None" BackColor="Transparent" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSyncContacts" Text="" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td><td style="width:10%;text-align:center;">
                                            <asp:Image ID="imgSyncLeads" runat="server" ImageUrl="http://images.ecn5.com/images/Leads.jpg" />
                                        </td><td style="text-align:left;">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Button ID="btnSyncLeads" OnClick="btnSyncLeads_Click" CssClass="ECN-Label-Heading-Large" Text="Leads" runat="server" BorderStyle="None" BackColor="Transparent" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lblSyncLeads" Text="" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>

                                        </td></tr><tr style="height:80px;">
                                            <td style="width:10%;text-align:center;">
                                            <asp:Image ID="imgSyncOptOut" ImageUrl="http://images.ecn5.com/images/EmailOptOut.jpg" runat="server" />
                                        </td><td style="text-align:left;">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Button ID="btnSyncOptOut" OnClick="btnSyncOptOut_Click" CssClass="ECN-Label-Heading-Large" Text="Opt-out" runat="server" BorderStyle="None" BackColor="Transparent" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lblSyncOptOut" Text="" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>
                                        <td style="width:10%;text-align:center;">
                                            <asp:Image ID="imgSyncSuppression" runat="server" ImageUrl="http://images.ecn5.com/images/MasterSupress.jpg" />
                                        </td><td style="text-align:left;width:40%">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSyncSuppression" Width="200px" OnClick="btnSyncSuppression_Click" CssClass="ECN-Label-Heading-Large" Text="Master Suppression" runat="server" BorderStyle="None" BackColor="Transparent" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSyncSuppression" Text="" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>

                                        </td></tr><tr style="height:80px;">
                                        
                                            <td style="width:10%;text-align:center;">
                                            <asp:Image ID="imgExportECNActivity" runat="server" ImageUrl="http://images.ecn5.com/images/EmailActivity.jpg" />
                                        </td><td style="text-align:left;">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Button ID="btnExportECNActivity" OnClick="btnExportECNActivity_Click" CssClass="ECN-Label-Heading-Large" runat="server" Text="Email Activity" BackColor="Transparent" BorderStyle="None" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lblExportECNActivity" Text="" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>
                                            <td style="width:10%;text-align:center;" id="tcAccountsImg" runat="server">
                                            <asp:Image ID="imgSyncAccounts" runat="server" ImageUrl="http://images.ecn5.com/images/Accounts.jpg" />
                                        </td><td id="tcAccountsBtn" runat="server" style="text-align:left;width:40%;">
                                            <table style="width:100%">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSyncAccounts" OnClick="btnSyncAccounts_Click" CssClass="ECN-Label-Heading-Large" Text="Accounts" runat="server" BorderStyle="None" BackColor="Transparent" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSyncAccounts" runat="server" Text="" />
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>
                                                  </tr></table></asp:Panel><asp:Panel ID="pnlInstructions" Visible="false" runat="server">

                                <asp:Literal ID="litInstructions" runat="server" Mode="PassThrough" Text="On this page is where you can put instructions and examples for the Salesforce/ECN integration features"></asp:Literal><table style="margin-top: 20px;">
                                    <tr>
                                        <td>
                                            <p style="font-size: 18px; color: #045C94;">If you are unable to update/insert records to Salesforce, check the information in ECN.  Last name, email address, and company are required fields.</p></td></tr></table></asp:Panel></td></tr></table></td></tr></table></asp:Content>
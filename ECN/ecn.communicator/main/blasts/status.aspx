<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.status" CodeBehind="status.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="d" Namespace="Donavon.Web.UI.WebControls" Assembly="Donavon.Web.UI.WebControls.ProgressBar" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="JavaScript" type="text/javascript">
        //configure refresh interval (in seconds)
        var countDownInterval = 10;

        var countDownTime = countDownInterval + 1;
        function countDown() {
            countDownTime--;
            if (countDownTime <= 0) {
                countDownTime = countDownInterval;
                clearTimeout(counter)
                window.location.reload()
                return
            }
            $("#divCounter").html("Next <a href='javascript:window.location.reload()'>refresh</a>  in <b>" + countDownTime + " </b> seconds");
            counter = setTimeout("countDown()", 1000);
        }

        function startit() {
            $("#divCounter").html("Next <a href='javascript:window.location.reload()'>refresh</a>  in <b>" + countDownTime + " </b> seconds");
            countDown()
        }

        if (document.all || document.getElementById)
            startit()
        else
            window.onload = startit
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="3" width="100%" border='0'>
        <tbody>
            <tr>
                <td class="tableHeader" colspan="2" align="left">
                    &nbsp;Reports
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right' width="200">
                    &nbsp;Email Subject:
                </td>
                <td class="tableContent"  align="left">
                    <asp:Label ID="EmailSubject" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Email From:
                </td>
                <td class="tableContent"  align="left">
                    <asp:Label ID="EmailFrom" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Group To:
                </td>
                <td class="tableContent"  align="left">
                    <asp:HyperLink ID="GroupTo" runat="Server"></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Filter used:
                </td>
                <td class="tableContent"  align="left">
                    <asp:GridView ID="gvFilters" runat="server" GridLines="None" AutoGenerateColumns="false" OnRowDataBound="gvFilters_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlFilterName" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lblNoFilter" ForeColor="DarkRed" Text="No Filters Used" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Campaign:
                </td>
                <td class="tableContent"  align="left">
                    <asp:HyperLink ID="Campaign" runat="Server"></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Start Time:
                </td>
                <td class="tableContent"  align="left">
                    <asp:Label ID="SendTime" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Send Progress:
                </td>
                <td class="tableContent"  align="left">
                    <d:ProgressBar ID="SuccessBar" runat="Server" Width="200px" Height="15px" BarColor="ForestGreen">
                    </d:ProgressBar>
                    &nbsp;<asp:Label ID="Successful" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="35">
                </td>
                <td class="tableHeader" align='right'>
                    &nbsp;Estimated Remaining:
                </td>
                <td class="tableContent"  align="left">
                    <asp:Label ID="Remaining" runat="Server"></asp:Label>
                </td>
            </tr>
             <tr>
                <td colspan="3" align="center">
                    <div class="tableHeader" id="divCounter"></div>
                </td>
               
            </tr>


        </tbody>
    </table>
    <br />
</asp:Content>

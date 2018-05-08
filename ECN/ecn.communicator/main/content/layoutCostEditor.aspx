<%@ Page Language="c#" Inherits="ecn.communicator.contentmanager.layoutCostEditor" Codebehind="layoutCostEditor.aspx.cs" %>

<style type="text/css">@import url( /ecn.images/images/stylesheet.css ); 
	</style>
<form id="Form1" method="post" runat="Server">
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
    </asp:PlaceHolder>
    <table style="font-family: Arial, Helvetica, sans-serif;" cellspacing="1" cellpadding="1"
        border='0'>
        <tr>
            <td class="tableHeader" valign="top" align="center" colspan='3' height="30">
                <asp:label class="errorMsg" id="MessageLabel" runat="Server"></asp:label>
            </td>
        </tr>
        <tr>
            <td class="headingOne" valign="top" align="center" colspan='3' height="30">
                Message ROI Cost details:
            </td>
        </tr>
        <tr>
            <td width="240" align='right' class="formLabel">
                <b>Setup Cost:</b><br />
                <span class="highLightOne">[Email message setup Cost]</span></td>
            <td width="15" align='right' valign="top" class="tableContent">
                &nbsp;$</td>
            <td width="50" valign="top">
                <asp:textbox id="SetupCostTxtBx" runat="Server" cssclass="formfield" size="10">0.00</asp:textbox>
            </td>
            <td width="250" style="font-size: 10px;" valign="middle">
                <asp:rangevalidator id="SetupCost_Rangevalidator" runat="Server" text="Setup Cost: Only values 0 - 9999"
                    enableclientscript="true" maximumvalue="9999.00" minimumvalue="0.00" controltovalidate="SetupCostTxtBx"
                    type="Double"></asp:rangevalidator>
            </td>
        </tr>
        <tr>
            <td height="5" colspan='3'>
                &nbsp;</td>
        </tr>
        <tr>
            <td width="240" align='right' class="formLabel">
                <b>Outbound Cost:</b><br />
                <span class="highLightOne">[Outbound Email message Cost ($0.025 / email)]</span>
            </td>
            <td width="15" align='right' valign="top" class="tableContent">
                &nbsp;$</td>
            <td width="50" valign="top">
                <asp:textbox id="OutboundCostTxtBx" runat="Server" cssclass="formfield" size="10">0.00</asp:textbox>
            </td>
            <td width="250" style="font-size: 10px;">
                <asp:rangevalidator id="OutboundCost_Rangevalidator" runat="Server" text="Outbound Cost: Only values 0 - 9999"
                    enableclientscript="true" maximumvalue="9999.00" minimumvalue="0.00" controltovalidate="OutboundCostTxtBx"
                    type="Double"></asp:rangevalidator>
            </td>
        </tr>
        <tr>
            <td height="5" colspan='3'>
                &nbsp;</td>
        </tr>
        <tr>
            <td width="240" align='right' class="formLabel">
                <b>Inbound Cost:</b><br />
                <span class="highLightOne">[Inbound Email message Cost ($0.025 / email)]</span>
            </td>
            <td width="15" align='right' valign="top" class="tableContent">
                &nbsp;$</td>
            <td width="50" valign="top">
                <asp:textbox id="InboundCostTxtBx" runat="Server" cssclass="formfield" size="10">0.00</asp:textbox>
            </td>
            <td width="250" style="font-size: 10px;">
                <asp:rangevalidator id="InboundCost_Rangevalidator" runat="Server" text="Inbound Cost: Only values 0 - 9999"
                    enableclientscript="true" maximumvalue="9999.00" minimumvalue="0.00" controltovalidate="InboundCostTxtBx"
                    type="Double"></asp:rangevalidator>
            </td>
        </tr>
        <tr>
            <td height="5" colspan='3'>
                &nbsp;</td>
        </tr>
        <tr>
            <td width="240" align='right' class="formLabel">
                <b>Design Cost:</b><br />
                <span class="highLightOne">[Email message designing Cost]</span>
            </td>
            <td width="15" align='right' valign="top" class=" tableContent">
                &nbsp;$</td>
            <td width="50" valign="top">
                <asp:textbox id="DesignCostTxtBx" runat="Server" cssclass="formfield" size="10">0.00</asp:textbox>
            </td>
            <td width="250" style="font-size: 10px;">
                <asp:rangevalidator id="DesignCost_Rangevalidator" runat="Server" text="Design Cost: Only values 0 - 9999"
                    enableclientscript="true" maximumvalue="9999.00" minimumvalue="0.00" controltovalidate="DesignCostTxtBx"
                    type="Double"></asp:rangevalidator>
            </td>
        </tr>
        <tr>
            <td height="5" colspan='3'>
                &nbsp;</td>
        </tr>
        <tr>
            <td width="240" align='right' class="formLabel">
                <b>Other Cost:</b><br />
                <span class="highLightOne">[Any other Email message Cost]</span>
            </td>
            <td width="15" align='right' valign="top" class="tableContent">
                &nbsp;$</td>
            <td width="50" valign="top">
                <asp:textbox id="OtherCostTxtBx" runat="Server" cssclass="formfield" size="10">0.00</asp:textbox>
            </td>
            <td width="250" style="font-size: 10px;">
                <asp:rangevalidator id="OtherCost_Rangevalidator" runat="Server" text="Other Cost: Only values 0 - 9999"
                    enableclientscript="true" maximumvalue="9999.00" minimumvalue="0.00" controltovalidate="OtherCostTxtBx"
                    type="Double"></asp:rangevalidator>
            </td>
        </tr>
        <tr>
            <td height="5" colspan='3'>
                &nbsp;</td>
        </tr>
        <tr>
            <td width="240" height="35" align='right' valign="bottom" class="tableContent">
                <asp:button id="SaveButton" runat="Server" text="Save Cost Info" class="formbutton"
                    onclick="SaveButton_Click"></asp:button>
            </td>
        </tr>
    </table>
</form>

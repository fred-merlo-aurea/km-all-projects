<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardContent_Champion.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Controls.WizardContent_Champion" %>


<script language="javascript" type="text/javascript">
    function getobj(id) {
        //alert('In the method!!');
        if (document.all && !document.getElementById)
            obj = eval('document.all.' + id);
        else if (document.layers)
            obj = eval('document.' + id);
        else if (document.getElementById)
            obj = document.getElementById(id);

        return obj;
    }

    
</script>

<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />

<script type="text/javascript" >
    function pageLoad(sender, args)
    {
        $('.subject').each(function(){
            var initialString = $(this).html();
            initialString = initialString.replace(/'/g, "\\'");
            initialString = initialString.replace(/\r?\n|\r/g, ' ');
            initialString = twemoji.parse(eval("'" + initialString + "'"));
            $(this).html(initialString);
});
    }
</script>

<asp:UpdateProgress ID="UpdateProgress2" runat="server" Visible="true"
    AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
    <ProgressTemplate>
        <asp:Panel ID="Panel3" CssClass="overlay" runat="server">
            <asp:Panel ID="Panel4" CssClass="loader" runat="server">
                <div>
                    <center>
                        <br />
                        <br />
                        <b>Processing...</b><br />
                        <br />
                        <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                        <br />
                        <br />
                        <br />
                    </center>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ProgressTemplate>
</asp:UpdateProgress>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="section bottomDiv" style="padding-left: 30px; padding-right: 30px">
            <fieldset>
                <legend>
                    <table>
                        <tr>
                            <td>Sample Details
                            </td>
                        </tr>
                    </table>
                </legend>
                <table cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td class="label" valign="middle" align='left' width="150" style="padding-top: 3px;">
                            <b>A/B Sample Blasts:</b>&nbsp;
                        </td>
                        <td align="left" style="padding-top: 3px; ">
                            <asp:DropDownList class="formfield" ID="AbSampleBlast" runat="Server" AutoPostBack="true"
                                DataValueField="SampleID" DataTextField="SampleName" OnSelectedIndexChanged="LoadSampleValues"
                                EnableViewState="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label" valign="middle" align='left' width="150" style="padding-top: 3px; ">
                            <b>Group:</b>&nbsp;
                        </td>
                        <td align="left" style="padding-top:3px;">
                            <asp:Label runat="server" ID="lblGroup"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="label" valign="middle" align='left' width="150" style="padding-top: 3px; padding-bottom:">
                            <b>Filter:</b>&nbsp;
                        </td>
                        <td align="left" style="padding-top: 3px;">
                            <asp:GridView ID="gvFilters" runat="server" ShowHeader="false" AutoGenerateColumns="false" OnRowDataBound="gvFilters_RowDataBound" GridLines="None">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFilterName" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Label runat="server" ID="lblFilter"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="label" valign="middle" align='left' width="150">
                            <b>Suppression Groups:</b>&nbsp;
                        </td>
                        <td align="left" style="padding-top: 3px;">
                            <asp:Label runat="server" ID="lblSuppressionGroups"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="label" valign="middle" align="left" width="150">
                            <b>Suppression Group Filters:</b>&nbsp;
                        </td>
                        <td align="left" style="padding-top: 3px;">
                            <asp:Label runat="server" ID="lblSuppGroupFilters" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label" valign="middle" align='left' width="150" style="padding-top: 3px;">
                            <b>Chose winner by:</b>&nbsp;
                        </td>
                        <td align="left" style="padding-top: 3px;">
                            <asp:Label runat="server" ID="lblABWinnerType"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="label" colspan="2" style="vertical-align: middle;">
                            <table style="width:100%;">
                                <tr>
                                    <td style="text-align:left;padding-left:30px;">
                                        <b>Send Winner To People Who:</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;padding-left:50px; padding-top: 3px; ">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkAorB" runat="server" Text="Did not receive A or B" Checked="true" AutoPostBack="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkLosingCampaign" runat="server" Text="Received Losing Campaign" AutoPostBack="true" Checked="true" OnCheckedChanged="chkLosingCampaign_CheckedChanged" />
                                                    <table style="padding-left:20px;">
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButtonList ID="rblLosingAction" Visible="true" runat="server">
                                                                    <asp:ListItem Text="Delivered" Value="delivered" Selected="True" />
                                                                    <asp:ListItem Text="Opened" Value="opened" />
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                        </td>

                    </tr>
                    <tr>
                        <td class="label" valign="top" align='left' width="150" style="padding-top: 3px;">
                            <b>Envelope:</b>&nbsp;
                        </td>
                        <td align="left" style="padding-top: 3px;">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <asp:Repeater ID="rptrEnvelope" runat="server">
                                        <ItemTemplate>
                                            <td>
                                                Email Subject: <asp:Label ID="lblEmailSubject" CssClass="subject" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EmailSubject") %>' /><br />
                                                <br />
                                                From Email: &nbsp; <%# DataBinder.Eval(Container.DataItem, "FromEmail") %>
                                                <br /><br />
                                                Reply To: &nbsp; <%# DataBinder.Eval(Container.DataItem, "ReplyTo") %>
                                                <br /><br />
                                                From Name: &nbsp; <%# DataBinder.Eval(Container.DataItem, "FromName") %>
                                                <br />
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </table>
                            
                        </td>
                       
                    </tr>
                    <tr>
                        <td class="label" valign="top" align='left' width="150" style="padding-top: 3px;">
                            <b>Samples:</b>&nbsp;
                        </td>
                        <td align="left" style="padding-top: 3px;">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <asp:Repeater ID="rptrSample" runat="server">
                                        <ItemTemplate>
                                            <td valign="middle" align="left" width="25%" >
                                                Delivered:
                                                            <%# DataBinder.Eval(Container.DataItem, "Delivered")%><br />
                                                <br />
                                                Total Sent:
                                                            <%# DataBinder.Eval(Container.DataItem, "SendTotal")%><br />
                                                <br />
                                                Total Bounce:
                                                            <%# DataBinder.Eval(Container.DataItem, "BounceTotal")%><br />
                                                <br />
                                                Total Opened:
                                                            <%# DataBinder.Eval(Container.DataItem, "OpenTotal")%><br />
                                                <br />
                                                Total Clicked: &nbsp;
                                                            <%# DataBinder.Eval(Container.DataItem, "ClickTotal")%><br />
                                                <br />
                                                Open Percentage: &nbsp;
                                                            <%# String.Format("{0:0.##}{1}",DataBinder.Eval(Container.DataItem, "OpenPercent"), "%")%><br />
                                                <br />
                                                Click Percentage: &nbsp;
                                                            <%# String.Format("{0:0.##}{1}", DataBinder.Eval( Container.DataItem, "ClickPercent"), "%")%><br />
                                                <br />
                                                
                                                <b><font color="red">
                                                    <%# DataBinder.Eval(Container.DataItem, "Winner").ToString().ToLower() == "true" ? "Current Winner" : ""%>
                                                </font></b>&nbsp;
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr>
                                    <td colspan="2" style="font-weight:bold;text-align:left;">
                                        Note: Winner will be determined when the Champion Blast is sent.
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </fieldset>
            
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

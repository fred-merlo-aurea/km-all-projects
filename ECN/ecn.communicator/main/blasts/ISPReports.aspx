<%@ Page Language="c#" Inherits="ecn.communicator.main.blasts.ISPReports" CodeBehind="ISPReports.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="gradient">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="border-left: 1px #A4A2A3 solid; border-bottom: 1px #A4A2A3 solid; border-right: 1px #A4A2A3 solid;
                padding: 10px;">
                <table id="layoutWrapper" cellspacing="1" cellpadding="3" width="100%" border='0'>
                    <tr>
                        <td class="tableHeader" colspan='3'>
                            &nbsp; &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="tableHeader" valign="top" align='right' width="35">
                        </td>
                        <td class="tableHeader" valign="top" align='right' width="135">
                            &nbsp;Email Subject:
                        </td>
                        <td class="tableContent" width="400"  align="left">
                            <asp:label id="EmailSubject" runat="Server"></asp:label>
                        </td>
                        <td class="tableHeader" align='right'>
                            &nbsp;Message:
                        </td>
                        <td class="tableContent blastLinksTwo"  align="left">
                            <asp:hyperlink id="Campaign" runat="Server"></asp:hyperlink>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableHeader" valign="top" align='right' width="35">
                        </td>
                        <td class="tableHeader" align='right'>
                            &nbsp;Email From:
                        </td>
                        <td class="tableContent"  align="left">
                            <asp:label id="EmailFrom" runat="Server"></asp:label>
                        </td>
                        <td align='right' class="tableHeader">
                            Send Time:</td>
                        <td class="tableContent"  align="left">
                            <asp:label id="SendTime" runat="Server"></asp:label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableHeader" valign="top" align='right' width="35">
                        </td>
                        <td class="tableHeader" align='right'>
                            &nbsp;Group To:
                        </td>
                        <td class="tableContent blastLinksTwo"  align="left">
                            <asp:hyperlink id="GroupTo" runat="Server"></asp:hyperlink>
                        </td>
                        <td align='right' class="tableHeader">
                            Finish Time:</td>
                        <td class="tableContent"  align="left">
                            <asp:label id="FinishTime" runat="Server"></asp:label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableHeader" valign="top" align='right' width="35">
                        </td>
                        <td class="tableHeader" align='right'>
                            &nbsp;Filter used:
                        </td>
                        <td class="tableContent blastLinksTwo"  align="left">
                            <asp:GridView ID="gvFilters" runat="server" GridLines="None" OnRowDataBound="gvFilters_RowDataBound" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlFilterName" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                            <asp:hyperlink id="Filter" runat="Server"></asp:hyperlink>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableHeader" valign="top" align='right' width="35">
                        </td>
                        <td class="tableHeader" align='right'>
                            &nbsp;Select ISP:
                        </td>
                        <td class="tableHeader" valign="middle"  align="left">
                            <asp:listbox cssclass="tableContent" id="lstISP" runat="Server" width="250px" selectionmode="Multiple"
                                rows="5">
                                <asp:ListItem value="AOL.COM">AOL.COM</asp:ListItem>
                                <asp:ListItem value="CS.COM">CS.COM</asp:ListItem>
                                <asp:ListItem value="Comcast.COM">COMCAST.COM</asp:ListItem>
                                <asp:ListItem value="Comcast.NET">COMCAST.NET</asp:ListItem>
                                <asp:ListItem value="EXCITE.COM">EXCITE.COM</asp:ListItem>
                                <asp:ListItem value="GMAIL.COM">GMAIL.COM</asp:ListItem>
                                <asp:ListItem value="HOTMAIL.COM">HOTMAIL.COM</asp:ListItem>
                                <asp:ListItem value="MSN.COM">MSN.COM</asp:ListItem>
                                <asp:ListItem value="NETSCAPE.NET">NETSCAPE.NET</asp:ListItem>
                                <asp:ListItem value="YAHOO.COM">YAHOO.COM</asp:ListItem>
                            </asp:listbox>
                            <br />
                            <asp:requiredfieldvalidator id="rfv1" errormessage="Please select an ISP Provider"
                                controltovalidate="lstISP" runat="Server"></asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableHeader" align="center" colspan='3'>
                            <asp:button id="btnReport" runat="Server" text="Submit" onclick="btnReport_Click"></asp:button>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableHeader" valign="top" align="left" colspan="5">
                            <asp:datagrid id="dgReport" runat="Server" width="100%" autogeneratecolumns="False"
                                cssclass="gridWizard">
                              <itemstyle></itemstyle>
                              <headerstyle CssClass="gridheaderWizard"></headerstyle>
                              <footerstyle CssClass="tableHeader1"></footerstyle>
                              <alternatingitemstyle CssClass="gridaltrowWizard"/>  
                              <columns>
                              <asp:BoundColumn ItemStyle-Width="15%" ItemStyle-HorizontalAlign="left" DataField="ISPs" HeaderStyle-HorizontalAlign="left"
							                    HeaderText="ISP"></asp:BoundColumn>
                              <asp:BoundColumn ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" DataField="Sends" HeaderStyle-HorizontalAlign="Center"
							                    HeaderText="Sends"></asp:BoundColumn>
                              <asp:TemplateColumn ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center" HeaderText="Opens">
                                <itemtemplate> <a href="Opens.aspx?blastID=<%# getBlastID().ToString()%>&isp=@<%# DataBinder.Eval(Container.DataItem, "ISPs")%>"><%# DataBinder.Eval(Container.DataItem, "Opens")%></a> </itemtemplate>
                              </asp:TemplateColumn>
                              <asp:TemplateColumn ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center" HeaderText="Clicks">
                                <itemtemplate> <a href="Clicks.aspx?blastID=<%# getBlastID().ToString()%>&isp=@<%# DataBinder.Eval(Container.DataItem, "ISPs")%>"><%# DataBinder.Eval(Container.DataItem, "Clicks")%></a> </itemtemplate>
                              </asp:TemplateColumn>
                              <asp:TemplateColumn ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center" HeaderText="Bounces">
                                <itemtemplate> <a href="Bounces.aspx?blastID=<%# getBlastID().ToString()%>&isp=@<%# DataBinder.Eval(Container.DataItem, "ISPs")%>"><%# DataBinder.Eval(Container.DataItem, "Bounces")%></a> </itemtemplate>
                              </asp:TemplateColumn>
                              <asp:TemplateColumn ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center" HeaderText="Unsubscribes">
                                <itemtemplate> <a href="subscribes.aspx?blastID=<%# getBlastID().ToString()%>&isp=@<%# DataBinder.Eval(Container.DataItem, "ISPs")%>"><%# DataBinder.Eval(Container.DataItem, "Unsubscribes")%></a> </itemtemplate>
                              </asp:TemplateColumn>
                              <asp:TemplateColumn ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center" HeaderText="Resends">
                                <itemtemplate> <a href="resends.aspx?blastID=<%# getBlastID().ToString()%>&isp=@<%# DataBinder.Eval(Container.DataItem, "ISPs")%>"><%# DataBinder.Eval(Container.DataItem, "Resends")%></a> </itemtemplate>
                              </asp:TemplateColumn>
                              <asp:TemplateColumn ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center" HeaderText="Forwards">
                                <itemtemplate> <a href="Forwards.aspx?blastID=<%# getBlastID().ToString()%>&isp=@<%# DataBinder.Eval(Container.DataItem, "ISPs")%>"><%# DataBinder.Eval(Container.DataItem, "Forwards")%></a> </itemtemplate>
                              </asp:TemplateColumn>
                              <asp:TemplateColumn ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center" HeaderText="Feedback Loop Unsubscribes">
                                <itemtemplate> <a href="subscribes.aspx?blastID=<%# getBlastID().ToString()%>&code=FEEDBACK_UNSUB&isp=@<%# DataBinder.Eval(Container.DataItem, "ISPs")%>"><%# DataBinder.Eval(Container.DataItem, "feedbackUnsubs")%></a> </itemtemplate>
                              </asp:TemplateColumn>
                              </columns>
                            </asp:datagrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:content>

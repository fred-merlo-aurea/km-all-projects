<%@ Page Language="c#" Inherits="ecn.communicator.blasts.reports.BlastComparisonReport" CodeBehind="BlastComparisonReport.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="ecnChart" Namespace="dotnetCHARTING" Assembly="dotnetCHARTING" %>
<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    	 <style type='text/css'>
        .ui-datepicker-trigger { position: relative; vertical-align:middle; padding-left:5px; }
    </style>
	
	<script type="text/javascript">
	    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
	        $("#<%=fromDateTxt.ClientID%>").datepicker({
                showOn: "button",
                buttonImage: "/ecn.images/images/icon-calendar.gif",
                buttonImageOnly: true,
                buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
            $("#<%=toDateTxt.ClientID%>").datepicker({
                showOn: "button",
                buttonImage: "/ecn.images/images/icon-calendar.gif",
                buttonImageOnly: true,
                buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
        });
    </script>

    <br />
    <table id="title" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td valign="middle" style="padding-right: 5px; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding-top: 0px">&nbsp;Blast Comparison Report
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
        <table cellspacing="0" cellpadding="0" width="674" align="center">
            <tr>
                <td id="errorTop"></td>
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
                <td id="errorBottom"></td>
            </tr>
        </table>
        <br />
    </asp:PlaceHolder>
    <div align="center">
        <br />
        <table bgcolor="#000000" cellpadding="1" cellspacing="0" border="0" width="100%">
            <tr>
                <td bgcolor='#EEEEEE' width="100%" style="border: 1px solid #CCCCCC">
                    <cpanel:DataPanel ID="Datapanel2" Style="z-index: 101" runat="Server" ExpandImageUrl="../expand.gif"
                        CollapseImageUrl="../collapse.gif" CollapseText="Click to hide Chart Preferences"
                        TitleText="Charting Preferences" TitleStyle-Font-Bold="true" ExpandText="Click to display Chart Preferences"
                        Collapsed="False" AllowTitleExpandCollapse="True" Width="100%">
                        <table border='0' cellpadding="3" cellspacing="0" bgcolor="#EEEEEE" width="100%">
                            <tr>
                                <td class="tableHeader" align="left" valign="top" width="14%">Customer:
                                </td>
                                <td width="86%">
                                    <asp:DropDownList ID="customerListBox" runat="Server" DataValueField="customerID"
                                        DataTextField="customerName" AutoPostBack="true" class="formfield" OnSelectedIndexChanged="customerListBox_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader" valign="top">Filter blasts:
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="tablecontent" style="font-weight: bold; font-size: 12px;">Date range&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="fromDateTxt" runat="Server" Columns="10" class="formfield" MaxLength="10" />&nbsp;
                                                            <asp:RegularExpressionValidator ID="revFromDate" runat="server" Display="Dynamic" ValidationExpression="^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$" ErrorMessage="Invalid" ValidationGroup="dateRangeGroup" ControlToValidate="fromDateTxt" />
                                                            &nbsp;
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="ftbeFromDateTXT" runat="server" TargetControlID="fromDateTxt" ValidChars="1234567890/" FilterMode="ValidChars" />

                                                        </td>
                                                        <td class="tablecontent" style="font-weight: bold; padding-left: 10px; font-size: 12px;">to&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="toDateTxt" runat="Server" Columns="10" class="formfield" MaxLength="10" />
                                                            &nbsp;<asp:RegularExpressionValidator ID="revToDate" Display="Dynamic" runat="server" ValidationExpression="^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$" ErrorMessage="Invalid" ValidationGroup="dateRangeGroup" ControlToValidate="toDateTxt" />
                                                            &nbsp;
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="ftbetoDateTxt" runat="server" TargetControlID="toDateTxt" ValidChars="1234567890/" FilterMode="ValidChars" />

                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" width="100%">
                                                <table border="0" cellpadding="0" cellspacing="0" width="90%">
                                                    <tr>
                                                        <td class="tablecontent" style="font-weight: bold; font-size: 12px;">Groups
                                                        </td>
                                                        <td style="width: 5px"></td>
                                                        <td class="tablecontent" style="font-weight: bold; font-size: 12px;">Campaigns
                                                        </td>
                                                        <td style="width: 5px"></td>
                                                        <td class="tablecontent" style="font-weight: bold; font-size: 12px;">User
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="groupsDR" DataTextField="GroupName"
                                                                DataValueField="GroupID" AppendDataBoundItems="true" class="formfield">
                                                                <asp:ListItem Selected="True" Value="0">-- select all --</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 5px"></td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="campaignsDR" DataTextField="CampaignName" DataValueField="CampaignID" AppendDataBoundItems="true"
                                                                class="formfield">
                                                                <asp:ListItem Selected="True" Value="0">-- select all --</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 5px"></td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="usersDR" DataTextField="UserName"
                                                                DataValueField="UserID" AppendDataBoundItems="true" class="formfield">
                                                                <asp:ListItem Selected="True" Value="0">-- select all --</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="bottom" width="10%">
                                                <asp:Button ID="refreshBlastListBtn" runat="Server" Text="Filter Blasts" class="formfield"
                                                    OnClick="refreshBlastListBtn_Click" ValidationGroup="dateRangeGroup" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader" valign="top">Blast list:
                                </td>
                                <td>
                                    <script type="text/javascript" language="javascript">
                                        function onClientSelectedIndexChangingHandler(source, arguments) {
                                            var listbox = document.getElementById('<%=BlastListBox.ClientID%>');
                                            var selectedCount = 0;
                                            for (var index = 0; index < listbox.options.length; index++) {
                                                if (listbox.options[index].selected)
                                                    selectedCount += 1;
                                            }
                                            if (selectedCount <= 10)
                                                arguments.IsValid = true;
                                            else
                                                arguments.IsValid = false;

                                        }
                                    </script>
                                    <asp:ListBox ID="BlastListBox" Rows="6" Width="100%" DataTextField="EmailSubject"
                                        CausesValidation="True" DataValueField="BlastID" SelectionMode="Multiple" runat="Server"
                                        class="formfield" />
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="You can select a maximum of 10 blasts"
                                        ClientValidationFunction="onClientSelectedIndexChangingHandler"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader" valign="top">Report on:
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="ReportOn" runat="Server" class="formfield" RepeatDirection="horizontal"
                                        RepeatColumns="6" RepeatLayout="table" TextAlign="Right" CellSpacing="1" CellPadding="5">
                                        <asp:ListItem Selected="true" Value="open">Opens</asp:ListItem>
                                        <asp:ListItem Selected="true" Value="click">Clicks</asp:ListItem>
                                        <asp:ListItem Value="bounce">Bounces</asp:ListItem>
                                        <asp:ListItem Value="subscribe">Opt-outs</asp:ListItem>
                                        <asp:ListItem Value="complaint">Complaints</asp:ListItem>
                                    </asp:CheckBoxList>
                                    <asp:Button ID="DrawChartButton" runat="Server" class="formfield" OnClick="DrawChart"
                                        Text="Draw Chart" />
                                </td>
                            </tr>
                        </table>
                    </cpanel:DataPanel>
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="graphPNL" runat="Server" Visible="true">
            <table border="0" width="900">
                <tr>
                    <td style="padding-left: 30px; padding-bottom: 5px" class="tableContent">
                        <asp:Label ID="RptHeader" runat="Server" Visible="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Chart ID="chtBlastComparision" runat="server" BackImageTransparentColor="White">
                        </asp:Chart>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="PanelExport" runat="server" Visible="false">
                                    Format:
                                    <asp:DropDownList ID="dropdownExport" runat="server">
                                        <asp:ListItem>PDF</asp:ListItem>
                                        <asp:ListItem>Excel</asp:ListItem>
                                        <asp:ListItem>Word</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:LinkButton ID="ltbnExport" runat="server" OnClick="Export_Click">Export</asp:LinkButton>
                                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" ShowBackButton="False" ShowRefreshButton="False"
                                        Visible="false">
                                        <LocalReport EnableExternalImages="True" ReportPath="main\blasts\Report\rpt_BlastComparision.rdlc">
                                        </LocalReport>
                                    </rsweb:ReportViewer>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="ltbnExport" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvChartData" runat="server" BackColor="White" CssClass="gridWizard"
                            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                            GridLines="Horizontal" Width="100%" Font-Size="X-Small" OnRowDataBound="gvChartData_RowDataBound"
                            AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Blast Details" ItemStyle-Width="50%">
                                    <ItemTemplate>
                                        <asp:Label ID="BlastID" runat="server" Text='<%# Eval("BlastID") %>' Visible="true"
                                            Font-Bold="true"></asp:Label><br />
                                        <asp:Label ID="Subject" runat="server" Text='<%# Eval("EmailSubject") %>' Visible="true"
                                            Font-Size="X-Small" ForeColor="Gray"></asp:Label><br />
                                        <asp:Label ID="TotalSent" runat="server" Text='<%# Eval("TotalSent") %>' Visible="true"
                                            Font-Size="X-Small" ForeColor="Gray"></asp:Label>
                                        <asp:Label ID="SendTime" runat="server" Text='<%# Eval("SendTime") %>' Visible="true"
                                            Font-Size="X-Small" ForeColor="Gray"></asp:Label><br />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Opens">
                                    <ItemTemplate>
                                        <asp:Label ID="OpensPerc" runat="server" Text='<%# Eval("OpensPerc") %>' Visible="true"
                                            Font-Bold="true" Font-Size="X-Small"></asp:Label><br />
                                        <asp:Label ID="Opens" runat="server" Text='<%# Eval("Opens") %>' Visible="true" Font-Size="X-Small"
                                            ForeColor="Gray"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Clicks">
                                    <ItemTemplate>
                                        <asp:Label ID="ClicksPerc" runat="server" Text='<%# Eval("ClicksPerc") %>' Visible="true"
                                            Font-Bold="true" Font-Size="X-Small"></asp:Label>
                                        <br />
                                        <asp:Label ID="Clicks" runat="server" Text='<%# Eval("Clicks") %>' Visible="true"
                                            Font-Size="X-Small" ForeColor="Gray"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bounces">
                                    <ItemTemplate>
                                        <asp:Label ID="BouncesPerc" runat="server" Text='<%# Eval("BouncesPerc") %>' Visible="true"
                                            Font-Bold="true" Font-Size="X-Small"></asp:Label><br />
                                        <asp:Label ID="Bounces" runat="server" Text='<%# Eval("Bounces") %>' Visible="true"
                                            Font-Size="X-Small" ForeColor="Gray"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OptOuts">
                                    <ItemTemplate>
                                        <asp:Label ID="OptOutsPerc" runat="server" Text='<%# Eval("OptOutsPerc") %>' Visible="true"
                                            Font-Bold="true" Font-Size="X-Small"></asp:Label>
                                        <br />
                                        <asp:Label ID="OptOuts" runat="server" Text='<%# Eval("OptOuts") %>' Visible="true"
                                            Font-Size="X-Small" ForeColor="Gray"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Complaints">
                                    <ItemTemplate>
                                        <asp:Label ID="ComplaintsPerc" runat="server" Text='<%# Eval("ComplaintsPerc") %>'
                                            Visible="true" Font-Bold="true" Font-Size="X-Small"></asp:Label>
                                        <br />
                                        <asp:Label ID="Complaints" runat="server" Text='<%# Eval("Complaints") %>' Visible="true"
                                            Font-Size="X-Small" ForeColor="Gray"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                            <HeaderStyle CssClass="gridheaderWizard" />
                            <AlternatingRowStyle CssClass="gridaltrowWizard" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#808080" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#383838" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

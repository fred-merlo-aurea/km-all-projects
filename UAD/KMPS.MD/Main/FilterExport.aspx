<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="FilterExport.aspx.cs" Inherits="KMPS.MD.Main.FilterExport" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="Marketo" TagPrefix="uc" Src="~/Controls/Marketo.ascx" %>
<%@ Register Src="~/Controls/GroupsLookup.ascx" TagName="GroupsLookup" TagPrefix="uc" %>
<%@ Register TagName="DownloadCase" TagPrefix="dc" Src="~/Controls/DownloadCase.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript" language="javascript">
        function toggleSelection(rb) {
            var gv = document.getElementById("<%=grdFilterSegmentationCounts.ClientID%>");
            var rbs = gv.getElementsByTagName("input");

            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        break;
                    }
                }
            }
        }
    </script>
    <style type="text/css">
        #overlay {
            position: fixed;
            z-index: 99;
            top: 0px;
            left: 0px;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=80);
            opacity: 0.80;
            -moz-opacity: 0.80;
        }
    </style>
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div id="overlay">
                <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 10002; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="../images/loading.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DownloadEditCase" EventName="CausePostBack" />
        </Triggers>
        <ContentTemplate>
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/ic-error.gif" runat="server"
                                            alt="" />
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
            </div>
            <center>
                <div style="width: 90%; text-align: left; padding-left: 10px;">
                    <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                        <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                            <div style="float: left;">
                                <asp:Label ID="lblpnlHeader" runat="Server"><b>Filter Export</b></asp:Label>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="Panel4" runat="server" Style="border-color: #5783BD; border-style: none" CssClass="collapsePanel"
                        Height="100%" BorderWidth="1">
                        <asp:Panel ID="Panel2" runat="server" Height="100%">
                            <table cellpadding="5" cellspacing="5" border="0">
                                <tr>
                                    <td align="right">
                                        <b>Filter Name</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFilterName" runat="Server"></asp:Label>
                                        <asp:HiddenField ID="hdFilterID" runat="server" Visible="false" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hdFilterScheduleID" runat="server" Visible="false" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hfViewType" runat="server"  />
                                        <asp:HiddenField ID="hfPubID" runat="server"  />
                                        <asp:HiddenField ID="hfBrandID" runat="server"  Value="0" />
                                    </td>
                                </tr>
                                <asp:PlaceHolder ID="phFSName" runat="server" Visible="false">
                                <tr>
                                    <td align="right">
                                        <b>Filter Segmentation</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFSName" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                                </asp:PlaceHolder>
                                <tr>
                                    <td align="right"><b>Scheduled Export Name</b></td>
                                    <td>
                                        <asp:TextBox ID="txtExportName" Width="200px" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rvtxtFSName" runat="server" ControlToValidate="txtExportName"
                                                ErrorMessage="*" Font-Bold="false" ValidationGroup="Export" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hfFilterSegmentationID" runat="server" Value="0" />
                                    </td>
                                </tr>
                                </tr>
                                <tr>
                                    <td align="right"><b>Scheduled Export Notes</b></td>
                                    <td>
                                        <asp:TextBox ID="txtExportNotes" Width="300px" runat="server" MaxLength="250" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="Panel1" runat="server" Style="padding-left: 30px;" Height="100%" GroupingText="Export Type" Font-Size="Smaller" Width="95%">
                                <table cellpadding="5" cellspacing="5" border="0">
                                    <tr>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" Width="200px" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvExport" runat="server" ControlToValidate="ddlExport"
                                                ErrorMessage="*" Font-Bold="false" ValidationGroup="Export" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                                <asp:PlaceHolder ID="plFTP" runat="server">
                                    <table cellpadding="5" cellspacing="5" border="0">
                                        <tr>
                                            <td align="left" colspan="2">
                                                <b>FTP Info</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlSiteType" runat="server" Width="200px">
                                                    <asp:ListItem Text="FTP" Value="FTP"></asp:ListItem>
                                                    <asp:ListItem Text="FTPS" Value="FTPS"></asp:ListItem>
                                                    <asp:ListItem Text="SFTP" Value="SFTP"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSiteType" ValidationGroup="Export"
                                                    ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">Server
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtServer" runat="server" Width="250px" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtServer" runat="server" ControlToValidate="txtServer" ValidationGroup="Export"
                                                    ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">User Name 
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtUserName" runat="server" Width="250px" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtUserName" runat="server" ControlToValidate="txtUserName" ValidationGroup="Export"
                                                    ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">Password
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPassword" runat="server" Width="250px" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtPassword" runat="server" ControlToValidate="txtPassword" ValidationGroup="Export"
                                                    ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">Folder
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFolder" runat="server" Width="250px" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtFolder" runat="server" ControlToValidate="txtFolder" ValidationGroup="Export"
                                                    ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2">
                                                <b>Export Format </b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">File Name
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFileName" runat="server" Width="250px" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvFileName" runat="server" ControlToValidate="txtFileName" ValidationGroup="Export"
                                                    ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtFileName" ErrorMessage="Please enter a valid File Name" ValidationExpression="([A-Z]|[a-z]|[0-9]|_|-|\|\s)+" ValidationGroup="Export" Display="Dynamic"></asp:RegularExpressionValidator>
                                                <asp:RadioButtonList ID="rblFileNameFormat" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                    <asp:ListItem Text="FileNameDateTime_YYYYMMDD_HHMMSS" Value="FileName_DateTime" Selected="true"></asp:ListItem>
                                                    <asp:ListItem Text="FileName_YYYYMMDD" Value="FileName_Date"></asp:ListItem>
                                                    <asp:ListItem Text="FileName" Value="FileName"></asp:ListItem>
                                                </asp:RadioButtonList>
                                           </td>
                                        </tr>
                                        <tr>
                                            <td align="right">Format
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlExportFormat" runat="server" Width="150px">
                                                    <asp:ListItem Value="CSV">CSV</asp:ListItem>
                                                    <asp:ListItem Value="TXT">TXT</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="cbShowHeader" runat="server" Checked="false" />
                                                &nbsp;Include Query Details Header
                                            </td>
                                        </tr>
                                    </table>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="plECN" runat="server" Visible="false">
                                    <uc:GroupsLookup runat="server" ID="GroupsLookup" Visible="false" FolderType="GRP"></uc:GroupsLookup>
                                    <table cellpadding="5" cellspacing="5" border="0">
                                        <tr>
                                            <td align="right">Group</td>
                                            <td>
                                                <asp:TextBox ID="txtGroupName" runat="server" Width="250px" MaxLength="50" ValidationGroup="Export" ReadOnly="true" ></asp:TextBox> &nbsp;
                                                <asp:ImageButton ID="ImgGroupList" runat="server" ImageUrl="~/Images/ic-lookup.jpg" OnClick="ImgGroupList_Click" ImageAlign="AbsBottom" />
                                                <asp:HiddenField ID="hfGroupID" runat="server"  />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="plMarketo" runat="server" Visible="false">
                                    <uc:Marketo runat="server" ID="Marketo"></uc:Marketo>
                                </asp:PlaceHolder>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="plFilters" runat="server" Style="padding-left: 30px;" Height="100%" GroupingText="Filters" Font-Size="Smaller" Width="95%" Visible="false">
                                <table cellpadding="5" cellspacing="5" border="0" width="100%">
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView ID="grdFilters" runat="server" Width="100%" AutoGenerateColumns="False"
                                                Height="100%" OnRowDataBound="grdFilters_RowDataBound"
                                                ShowFooter="true" AllowPaging="false" DataKeyNames="FilterNo">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Name" HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFilterGroupName" runat="server" Text='<%# Eval("FilterGroupName") %>' Font-Bold="true"></asp:Label>
                                                            <asp:Label ID="lblFilterGroupID" runat="server" Text='<%# Eval("FilterGroupID") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblBrandID" runat="server" Text='<%# Eval("BrandID") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Filters"
                                                        ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:Panel ID="pnlResultHeader" runat="server" Height="28px" CssClass="collapsePanelHeader"
                                                                BackColor="#eeeeee" ForeColor="Black">
                                                                <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                                                    <div style="float: left;">
                                                                    </div>
                                                                    <div style="float: left; margin-left: 20px;">
                                                                        <asp:Label ID="pnlResultLabel" runat="server">(Show Filters...)</asp:Label>
                                                                    </div>
                                                                    <div style="float: right; vertical-align: middle;">
                                                                        <asp:ImageButton ID="pnlResultImage" runat="server" ImageUrl="~/images/expand_blue.jpg"
                                                                            AlternateText="(Show Filters...)" />
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlResultBody" runat="server" CssClass="collapsePanel" Height="0"
                                                                BorderColor="#eeeeee" BorderWidth="2">
                                                                <div style="width: 1150px; overflow-x:auto;">
                                                                <asp:GridView ShowHeader="false" ID="grdFilterValues" Width="100%" runat="server"
                                                                    AutoGenerateColumns="False" GridLines="Both">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="name" HeaderText="Column">
                                                                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="right" Width="20%" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="#" HeaderStyle-Width="3%" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="center"
                                                                            ItemStyle-VerticalAlign="Middle">
                                                                            <ItemTemplate>
                                                                                =
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="#" HeaderStyle-Width="77%" ItemStyle-Width="77%" ItemStyle-HorizontalAlign="left"
                                                                            ItemStyle-VerticalAlign="Middle">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFilterText" runat="server" Text='<%# Eval("Text") %>'></asp:Label>
                                                                                <asp:Label ID="lblAdhocCondition" runat="server" Text='<%# Eval("name").ToString() == "Adhoc" ?  " - " + Eval("SearchCondition") + " - " + Eval("Values") : "" %>'></asp:Label>
                                                                                <asp:Label ID="lblFilterValues" runat="server" Text='<%# Eval("Values") %>' Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblFiltername" runat="server" Text='<%# Eval("name") %>' Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblSearchCondition" runat="server" Text='<%# Eval("SearchCondition") %>' Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblFilterType" runat="server" Text='<%# Eval("FilterType") %>' Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblGroup" runat="server" Text='<%# Eval("Group") %>' Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:CollapsiblePanelExtender ID="cpeDemo3" runat="Server" TargetControlID="pnlResultBody"
                                                                ExpandControlID="pnlResultHeader" CollapseControlID="pnlResultHeader" Collapsed="true"
                                                                TextLabelID="pnlResultLabel" ImageControlID="pnlResultImage" ExpandedText="(Hide Filters...)"
                                                                CollapsedText="(Show Filters...)" ExpandedImage="~/images/collapse_blue.jpg"
                                                                CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Counts" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCounts" runat="server" Text='<%# Eval("Count") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <asp:PlaceHolder ID="plFilterSegmentationsSelect" runat="server" Visible="false">
                                        <tr>
                                            <td colspan="3">
                                                <b>Do you want to create a new segment?</b>&nbsp;
                                                <asp:RadioButtonList ID="rbNewExisitngSegment" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbNewExisitngSegment_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="Yes" Text=" Yes"></asp:ListItem>
                                                    <asp:ListItem Value="No" Text=" No"  Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>                                            
                                            </td>
                                        </tr>
                                    <asp:PlaceHolder ID="plExistingSegment" runat="server">
                                        <tr>
                                            <td  colspan="3">
                                                <asp:GridView ID="grdFilterSegmentationCounts" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    Height="100%" ShowHeader="true" AllowPaging="false" DataKeyNames="FilterViewNo">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Name" HeaderStyle-Width="30%" ItemStyle-Width="30%"
                                                            ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFilterViewName" runat="server" Text='<%# Eval("FilterViewName")%>' Font-Bold="true"></asp:Label>
                                                                <asp:HiddenField ID="hfFilterViewNo" runat="server" Value='<%# Eval("FilterViewNo") %>' Visible="false"></asp:HiddenField>
                                                                <asp:HiddenField ID="hfFilterViewName" runat="server" Value='<%# Eval("FilterViewName") %>' Visible="false"></asp:HiddenField>
                                                                <asp:HiddenField ID="hfFilterDescription" runat="server" Value='<%# Eval("FilterDescription") %>' Visible="false"></asp:HiddenField>
                                                                <asp:HiddenField ID="hfSelectedFilterNo" runat="server" Value='<%# Eval("SelectedFilterNo") %>' Visible="false"></asp:HiddenField>
                                                                <asp:HiddenField ID="hfSuppressedFilterNo" runat="server" Value='<%# Eval("SuppressedFilterNo") %>' Visible="false"></asp:HiddenField>
                                                                <asp:HiddenField ID="hfSelectedFilterOperation" runat="server" Value='<%# Eval("SelectedFilterOperation") %>' Visible="false"></asp:HiddenField>
                                                                <asp:HiddenField ID="hfSuppressedFilterOperation" runat="server" Value='<%# Eval("SuppressedFilterOperation") %>' Visible="false"></asp:HiddenField>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Data Segments Included" DataField="FilterDescription" HeaderStyle-Width="38%"
                                                            ItemStyle-Width="30%" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle"
                                                            FooterStyle-HorizontalAlign="center" FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left" />
                                                        <asp:BoundField HeaderText="Count" DataField="Count" HeaderStyle-Width="20%"
                                                            ItemStyle-Width="20%" ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle"
                                                             HeaderStyle-HorizontalAlign="center" />
                                                        <asp:TemplateField HeaderText="Export" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                                            ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="rbFSSelect" runat="server"  GroupName="select"  onclick="toggleSelection(this);"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder> 
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="PlFiltersSelect" runat="server" Visible="false">
                                    <tr>
                                        <td style="vertical-align: middle" width="30%">
                                            <table cellpadding="5" cellspacing="5" border="0" width="70%">
                                                <tr>
                                                    <td width="5%" style="vertical-align: middle">
                                                        <b>Operation</b>
                                                    </td>
                                                    <td style="vertical-align: middle" width="16%">
                                                        <asp:RadioButtonList ID="rblSelectedFiltersOperation" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="Union" Text=" Union" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="Intersect" Text=" Intersect"></asp:ListItem>
                                                        </asp:RadioButtonList></td>
                                                    <td style="vertical-align: middle">
                                                        <asp:RequiredFieldValidator ID="rvSelectedFiltersOperation" runat="server" ControlToValidate="rblSelectedFiltersOperation" ValidationGroup="Export" Display="Dynamic"
                                                            ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: middle" width="30%">
                                            <table cellpadding="5" cellspacing="5" border="0" width="70%">
                                                <tr>
                                                    <td width="5%" style="vertical-align: middle">
                                                        <b>Operation</b>
                                                    </td>
                                                    <td style="vertical-align: middle" width="16%">
                                                        <asp:RadioButtonList ID="rblSuppressedFiltersOperation" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="Union" Text=" Union" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="Intersect" Text=" Intersect"></asp:ListItem>
                                                        </asp:RadioButtonList></td>
                                                    <td style="vertical-align: middle">
                                                        <asp:RequiredFieldValidator ID="rvSuppressedFiltersOperation" runat="server" ControlToValidate="rblSuppressedFiltersOperation" ValidationGroup="Export" Display="Dynamic"
                                                            ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td align="center"><b>Available Filters - In</b></td>
                                        <td align="center"><b>Available Filters - Not In(Optional)</b></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td  valign="top">
                                            <asp:ListBox ID="lstSelectedFilters" runat="server" Rows="10" 
                                                SelectionMode="Multiple" Width="350px" Height="150px"
                                                EnableViewState="True"></asp:ListBox>
                                            <asp:RequiredFieldValidator ID="rvSelectedFilters" runat="server" ControlToValidate="lstSelectedFilters" Display="static" ValidationGroup="Export"
                                                ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </td>
                                        <td valign="top">
                                            <asp:ListBox ID="lstSuppressedFilters" runat="server" Rows="10" 
                                                SelectionMode="Multiple" Width="350px" Height="150px"></asp:ListBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    </asp:PlaceHolder>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlExportFields" runat="server" Style="padding-left: 30px;" Height="100%" GroupingText="Export Fields" Font-Size="Smaller" Width="95%">
                                <asp:PlaceHolder ID="Pl1" runat="server">
                                    <table cellpadding="5" cellspacing="5" border="0">
                                        <tr>
                                            <td colspan="4">Download Template &nbsp;&nbsp;
                                                <asp:DropDownList ID="drpDownloadTemplate" runat="server" Width="200px" DataTextField="DownloadTemplateName" DataValueField="DownloadTemplateID"
                                                    AutoPostBack="true" OnSelectedIndexChanged="drpDownloadTemplate_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center"><b>Available Fields</b></td>
                                            <td></td>
                                            <td align="center"><b>Selected Fields</b></td>
                                            <td></td>
                                        </tr>
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblFieldsType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rblFieldsType_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Value="Profile" Selected="True">Profile</asp:ListItem>
                                                        <asp:ListItem Value="Demo">Demo</asp:ListItem>
                                                        <asp:ListItem Value="Adhoc">Adhoc</asp:ListItem>
                                                    </asp:RadioButtonList>&nbsp;&nbsp;
                                                </td>
                                                <td></td>
                                                <td><asp:Button ID="btnEditCase" runat="server" CssClass="button" OnClick="btnEditCase_Click" Text="Edit Case" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                            <asp:PlaceHolder ID="phProfileFields" runat="server">
                                                <td>
                                                    <asp:ListBox ID="lstAvailableProfileFields" runat="server" Rows="10"
                                                        SelectionMode="Multiple" Width="350px" Height="300px"
                                                        EnableViewState="True"></asp:ListBox>
                                                </td>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder ID="phDemoFields" runat="server" Visible = "false">
                                                <td>
                                                    <asp:ListBox ID="lstAvailableDemoFields" runat="server" Rows="10"
                                                        SelectionMode="Multiple" Width="350px" Height="300px"
                                                        EnableViewState="True"></asp:ListBox>
                                                </td>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder ID="phAdhocFields" runat="server" Visible ="false">
                                                <td>
                                                    <asp:ListBox ID="lstAvailableAdhocFields" runat="server" Rows="10"
                                                        SelectionMode="Multiple" Width="350px" Height="300px"
                                                        EnableViewState="True"></asp:ListBox>
                                                </td>
                                            </asp:PlaceHolder>
                                            <td>
                                                <asp:Button ID="btnAdd" runat="server" Text=">>" CssClass="button" OnClick="btnAdd_Click" />
                                                <br>
                                                <br>
                                                <asp:Button ID="btnremove" runat="server" CssClass="button" OnClick="btnRemove_Click"
                                                    Text="<<" />
                                            </td>
                                            <td>
                                                <asp:ListBox ID="lstSelectedFields" runat="server" Rows="10"
                                                    SelectionMode="Multiple" Width="350px" Height="300px"
                                                    DataTextField="DisplayName"
                                                    DataValueField="ColumnValue"></asp:ListBox>

                                            </td>
                                            <td>
                                                <asp:Button ID="btnUp" runat="server" Text="Move Up" CssClass="button" OnClick="btnUp_Click" />
                                                <br>
                                                <br>
                                                <asp:Button ID="btnDown" runat="server" CssClass="button" OnClick="btndown_Click"
                                                    Text="Move Down" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4"><asp:Label ID="lblErrSelectedFields" text="" runat="Server" Visible="false" ForeColor="Red"></asp:Label></td>
                                        </tr>
                                    </table>
                                </asp:PlaceHolder>
                                <br />
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="Panel6" runat="server" Style="padding-left: 30px;" Height="100%" GroupingText="Schedule" Font-Size="Smaller" Width="95%">
                                <asp:PlaceHolder ID="Pl3" runat="server">
                                    <table cellpadding="5" cellspacing="5" border="0">
                                        <tr>
                                            <td align="right">Schedule Type 
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlScheduleType" runat="server" AutoPostBack="True" Width="150px" OnSelectedIndexChanged="ddlScheduleType_SelectedIndexChanged">
                                                    <asp:ListItem Value="One-Time">Schedule One-Time</asp:ListItem>
                                                    <asp:ListItem Value="Recurring">Schedule Recurring</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <asp:Panel ID="pnlOneTime" runat="server">
                                            <tr>
                                                <td align="right">Start Date</td>
                                                <td>
                                                    <asp:TextBox ID="txtStartDate" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvtxtStartDate" runat="server" ControlToValidate="txtStartDate" ValidationGroup="Export"
                                                        ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cmpStartDate" runat="server"
                                                        ControlToValidate="txtStartDate"
                                                        ErrorMessage="Please enter a valid date"
                                                        Operator="DataTypeCheck" Type="Date" ValidationGroup="Export"></asp:CompareValidator>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1"
                                                        runat="server" CssClass="MyCalendar" TargetControlID="txtStartDate" Format="MM/dd/yyyy">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">Start Time</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlStartTime" runat="server" Width="100px">
                                                        <asp:ListItem Value="0:00:00">12:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="1:00:00">1:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="2:00:00">2:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="3:00:00">3:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="4:00:00">4:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="5:00:00">5:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="6:00:00">6:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="7:00:00">7:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="8:00:00">8:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="9:00:00">9:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="10:00:00">10:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="11:00:00">11:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="12:00:00">12:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="13:00:00">1:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="14:00:00">2:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="15:00:00">3:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="16:00:00">4:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="17:00:00">5:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="18:00:00">6:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="19:00:00">7:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="20:00:00">8:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="21:00:00">9:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="22:00:00">10:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="23:00:00">11:00 PM</asp:ListItem>
                                                    </asp:DropDownList>&nbsp;&nbsp;CST
                                <asp:RequiredFieldValidator ID="rfvddlStartTime" runat="server" ControlToValidate="ddlStartTime" ValidationGroup="Export"
                                    ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlRecurrence" Visible="false">
                                            <tr>
                                                <td align="right">Recurrence
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlRecurrence" runat="server" AutoPostBack="True" Width="150px" DataTextField="Type" DataValueField="RecurrenceTypeID"
                                                        OnSelectedIndexChanged="ddlRecurrence_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlRecurring" runat="server" Visible="false">
                                            <tr>
                                                <td align="right">Start Date</td>
                                                <td>
                                                    <asp:TextBox ID="txtRecurringStartDate" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                                    <asp:HiddenField ID="hdnDate" runat="server" />
                                                    <asp:RequiredFieldValidator ID="rfvtxtRecurringStartDate" runat="server" ControlToValidate="txtRecurringStartDate" ValidationGroup="Export"
                                                        ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cmpRecStartDate" runat="server"
                                                        ControlToValidate="txtRecurringStartDate"
                                                        ErrorMessage="Please enter a valid date"
                                                        Operator="DataTypeCheck" Type="Date" ValidationGroup="Export"></asp:CompareValidator>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3"
                                                        runat="server" CssClass="MyCalendar" TargetControlID="txtRecurringStartDate" Format="MM/dd/yyyy">
                                                    </ajaxToolkit:CalendarExtender>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">Start Time</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlRecurringStartTime" runat="server" Width="100px">
                                                        <asp:ListItem Value="0:00:00">12:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="1:00:00">1:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="2:00:00">2:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="3:00:00">3:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="4:00:00">4:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="5:00:00">5:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="6:00:00">6:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="7:00:00">7:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="8:00:00">8:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="9:00:00">9:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="10:00:00">10:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="11:00:00">11:00 AM</asp:ListItem>
                                                        <asp:ListItem Value="12:00:00">12:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="13:00:00">1:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="14:00:00">2:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="15:00:00">3:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="16:00:00">4:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="17:00:00">5:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="18:00:00">6:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="19:00:00">7:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="20:00:00">8:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="21:00:00">9:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="22:00:00">10:00 PM</asp:ListItem>
                                                        <asp:ListItem Value="23:00:00">11:00 PM</asp:ListItem>
                                                    </asp:DropDownList>&nbsp;&nbsp;CST
                                <asp:RequiredFieldValidator ID="rfvddlRecurringStartTime" runat="server" ControlToValidate="ddlRecurringStartTime" ValidationGroup="Export"
                                    ErrorMessage=" *" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">End Date</td>
                                                <td>
                                                    <asp:TextBox ID="txtRecurringEndDate" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2"
                                                        runat="server" CssClass="MyCalendar" TargetControlID="txtRecurringEndDate" Format="MM/dd/yyyy">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:CompareValidator ID="cmpRecEndDate" runat="server"
                                                        ErrorMessage="Please enter a valid date"
                                                        ControlToValidate="txtRecurringEndDate"
                                                        Operator="DataTypeCheck" Type="Date" ValidationGroup="Export"></asp:CompareValidator>

                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlDays" Visible="false">
                                            <tr>
                                                <td align="right">Days</td>
                                                <td>
                                                    <asp:CheckBoxList ID="cbDays" runat="server" RepeatDirection="Horizontal" SelectMethod="">
                                                        <asp:ListItem Value="Sunday">Sunday</asp:ListItem>
                                                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                                                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                                                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                                                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                                                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                                                        <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlMonth" Visible="false">
                                            <tr>
                                                <td align="left" valign="middle" width="20%" class="label">What day of each month</td>
                                                <td align="left" valign="middle" width="80%" class="label">
                                                    <asp:TextBox ID="txtMonth" runat="server"></asp:TextBox>
                                                    <asp:RangeValidator ID="rvMonth" runat="server"
                                                        ControlToValidate="txtMonth" MinimumValue="1" Type="Integer" MaximumValue="28" ValidationGroup="Export"
                                                        CssClass="errormsg" Display="Dynamic">&laquo;&laquo Invalid Value></asp:RangeValidator>
                                                    <asp:CheckBox ID="cbLastDay" runat="server" Text=" Last Day" />
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                    </table>
                                </asp:PlaceHolder>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="Panel7" runat="server" Style="padding-left: 30px;" Height="100%" GroupingText="Email Notification" Font-Size="Smaller" Width="95%">
                                <asp:PlaceHolder ID="Pl2" runat="server">
                                    <table cellpadding="5" cellspacing="5" border="0">
                                        <tr>
                                            <td align="right">Email Address
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmailAddress" runat="server" Width="250px" MaxLength="500"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationGroup="Export"
                                                    ErrorMessage="Not a valid email address" ControlToValidate="txtEmailAddress" ValidationExpression="^(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,])*)*$"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <%--                                        <tr>
                                            <td colspan="2">Upon successful export, email will be sent to the above email address(es).</td>
                                        </tr>--%>
                                    </table>
                                </asp:PlaceHolder>
                            </asp:Panel>
                            <br />
                            <table cellpadding="5" cellspacing="5" border="0">
                                <tr>
                                    <td>
                                        <div id="divErrMsg" runat="Server" visible="false">
                                            <font color="red">
                                                <asp:Label ID="lblErrMsg" runat="Server"></asp:Label></font>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" ValidationGroup="Export" />
                                        &nbsp;&nbsp; 
                    <asp:Button ID="btnCancel" runat="server" CssClass="button" OnClick="btnCancel_Click" Text="Cancel" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
        <ContentTemplate>
            <dc:DownloadCase runat="server" ID="DownloadEditCase" Visible="false"></dc:DownloadCase>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


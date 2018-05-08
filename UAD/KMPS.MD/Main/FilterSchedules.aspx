<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="FilterSchedules.aspx.cs" Inherits="KMPS.MD.Main.FilterSchedules" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 100005; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="../images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <div id="divError" runat="Server" visible="false">
                    <table cellspacing="0" cellpadding="0" width="674" align="center">
                        <tr>
                            <td id="errorMiddle">
                                <table width="80%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/ic-error.gif" runat="server"
                                                alt="" />
                                        </td>
                                        <td valign="middle" align="left" width="80%" height="100%">
                                            <asp:Label ID="lblErrorMessage" runat="Server" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
                <table border="0" cellpadding="5" cellspacing="3" width="90%" align="center">

                    <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                        <td valign="middle" align="left"  width="20%">
                            <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                                <b>Brand
                                <asp:Label ID="lblColon" runat="server" Visible="false" Text=":"></asp:Label></b>&nbsp;&nbsp;
                                <asp:Label ID="lblBrandName" runat="server" Visible="false"></asp:Label>
                                <asp:DropDownList ID="drpBrand" runat="server" Font-Names="Arial" Font-Size="x-small" Visible="false"
                                    AutoPostBack="true" Style="text-transform: uppercase" DataTextField="BrandName"
                                    DataValueField="BrandID" Width="150px" OnSelectedIndexChanged="drpBrand_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hfBrandID" runat="server" Value="0" />
                            </asp:Panel>
                        </td>
                        <td  width="25%">
                            <b><asp:RadioButtonList ID="rblListType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rblListType_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="Filters" Selected="True">Filters</asp:ListItem>
                                <asp:ListItem Value="FilterSegmentations">Filter Segmentation</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;&nbsp;</b>
                        </td>
                        <td align="right" width="55%">
                            <b><asp:Label ID="lblSearch" runat="server" Text="Filter Name"></asp:Label></b>&nbsp;
                                <asp:DropDownList ID="drpSearch" runat="server" Font-Names="Arial" Font-Size="x-small"
                                    Width="100">
                                    <asp:ListItem Selected="true" Value="Contains">CONTAINS</asp:ListItem>
                                    <asp:ListItem Value="Equal">EQUAL</asp:ListItem>
                                    <asp:ListItem Value="Start With">START WITH</asp:ListItem>
                                    <asp:ListItem Value="End With">END WITH</asp:ListItem>
                                </asp:DropDownList>&nbsp;
                                <asp:TextBox ID="txtSearch" Width="180px" runat="server"></asp:TextBox>&nbsp;
                            <asp:Button ID="btnSearch" Text="Search" runat="server" CssClass="button" ValidationGroup="search"
                                OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
                <div style="width: 90%; text-align: left; padding-left: 10px;">
                    <asp:PlaceHolder ID="phFilters" runat="server">
                    <asp:GridView ID="gvFilterSchedules" runat="server" AllowPaging="True" AllowSorting="True" OnSorting="gvFilterSchedules_Sorting"
                        AutoGenerateColumns="False" DataKeyNames="FilterID" OnRowCommand="gvFilterSchedules_RowCommand" OnRowCreated="gvFilterSchedules_RowCreated" OnPageIndexChanging="gvFilterSchedules_PageIndexChanging" OnRowDeleting="gvFilterSchedules_OnRowDeleting" OnRowDataBound="gvFilterSchedules_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Brand Name" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Left" SortExpression="BrandName">
                                <ItemTemplate>
                                    <asp:Label ID="lblBrandName" runat="server" Text='<%# Eval("BrandName") == null ? "" : Eval("BrandName").ToString()%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ExportName" HeaderText="Export Name" SortExpression="ExportName"
                                ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Export Notes" SortExpression="ExportNotes" HeaderStyle-Width="5%" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="center"
                                ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Image ID="ImgNotes" runat="server" title='<%# Eval("ExportNotes") %>'  ImageUrl="../Images/ic-note.jpg" style='<%# Eval("ExportNotes") == "" || Eval("ExportNotes") == null? "display:none":"display:inline" %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FilterName" HeaderText="Filter Name" SortExpression="FilterName"
                                ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="16%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Type" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" SortExpression="ExportTypeID" ItemStyle-Width="10%">
                                <ItemTemplate><%# Eval("ExportTypeID").ToString().ToUpper() == "ECN" ? "Email Marketing" : Eval("ExportTypeID").ToString()%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RecurrenceType" HeaderText="Recurrence" SortExpression="RecurrenceType"
                                ItemStyle-HorizontalAlign="center">
                                <HeaderStyle HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate"
                                ItemStyle-HorizontalAlign="center" DataFormatString="{0:MM/dd/yyyy}">
                                <HeaderStyle HorizontalAlign="center" Width="9%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Start Time" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="StartTime">
                                <ItemTemplate>
                                    <asp:Label ID="lblStartTime" runat="server" Width="100px" MaxLength="10" Text='<%# Eval("StartTime")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="EndDate" HeaderText="End Date" SortExpression="EndDate"
                                ItemStyle-HorizontalAlign="center" DataFormatString="{0:MM/dd/yyyy}">
                                <HeaderStyle HorizontalAlign="center" Width="7%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Sun" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunSunday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mon" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunMonday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tue" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunTuesday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Wed" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunWednesday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Thu" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunThursday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fri" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunFriday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sat" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunSaturday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MonthScheduleDay" HeaderText="Month Sch.Day"
                                ItemStyle-HorizontalAlign="center">
                                <HeaderStyle HorizontalAlign="center" Width="6%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Month LastDay" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "3" ? (Boolean.Parse(Eval("MonthLastDay").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderText="Export" Visible="false"
                                ItemStyle-HorizontalAlign="center"
                                HeaderStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkExport" runat="server" CommandName="Export" CommandArgument='<%# Eval("FilterScheduleId")%>'><img src="../Images/export.png" alt="" style="border:none;" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkEdit" NavigateUrl='<%# Eval("FilterScheduleId", "FilterExport.aspx?FilterScheduleId=" + Eval("FilterScheduleId").ToString() + "&FilterID=" +  Eval("FilterId").ToString()) %>'
                                        HeaderText="Edit" runat="server"
                                        Text="&lt;img src='../images/ic-edit.gif' border='0'/&gt;"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField
                                ItemStyle-HorizontalAlign="center"
                                HeaderStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("FilterScheduleId")%>' OnClientClick="return confirm('Are you sure you want to delete?')"><img src="../Images/icon-delete.gif" alt="" style="border:none;"  /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phFilterSegmentations" runat="server" Visible="false">
                    <asp:GridView ID="gvFilterSegmentationSchedules" runat="server" AllowPaging="True" AllowSorting="True" OnSorting="gvFilterSegmentationSchedules_Sorting"
                        AutoGenerateColumns="False" DataKeyNames="FilterID" OnRowCommand="gvFilterSegmentationSchedules_RowCommand" OnRowCreated="gvFilterSegmentationSchedules_RowCreated" OnPageIndexChanging="gvFilterSegmentationSchedules_PageIndexChanging" OnRowDeleting="gvFilterSegmentationSchedules_OnRowDeleting" OnRowDataBound="gvFilterSegmentationSchedules_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Brand Name" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Left" SortExpression="BrandName">
                                <ItemTemplate>
                                    <asp:Label ID="lblBrandName" runat="server" Text='<%# Eval("BrandName") == null ? "" : Eval("BrandName").ToString()%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ExportName" HeaderText="Export Name" SortExpression="ExportName"
                                ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Export Notes" HeaderStyle-Width="5%" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="center" SortExpression="ExportNotes"
                                ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Image ID="ImgNotes" runat="server" title='<%# Eval("ExportNotes") %>'  ImageUrl="../Images/ic-note.jpg" style='<%# Eval("ExportNotes") == "" || Eval("ExportNotes") == null? "display:none":"display:inline" %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FilterName" HeaderText="Filter Name" SortExpression="FilterName"
                                ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FilterSegmentationName" HeaderText="Filter Segmentation" SortExpression="FilterSegmentationName"
                                ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Type" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" SortExpression="ExportTypeID" ItemStyle-Width="10%">
                                <ItemTemplate><%# Eval("ExportTypeID").ToString().ToUpper() == "ECN" ? "Email Marketing" : Eval("ExportTypeID").ToString()%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RecurrenceType" HeaderText="Recurrence" SortExpression="RecurrenceType"
                                ItemStyle-HorizontalAlign="center">
                                <HeaderStyle HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate"
                                ItemStyle-HorizontalAlign="center" DataFormatString="{0:MM/dd/yyyy}">
                                <HeaderStyle HorizontalAlign="center" Width="9%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Start Time" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="StartTime">
                                <ItemTemplate>
                                    <asp:Label ID="lblStartTime" runat="server" Width="100px" MaxLength="10" Text='<%# Eval("StartTime")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="EndDate" HeaderText="End Date" SortExpression="EndDate"
                                ItemStyle-HorizontalAlign="center" DataFormatString="{0:MM/dd/yyyy}">
                                <HeaderStyle HorizontalAlign="center" Width="7%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Sun" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunSunday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mon" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunMonday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tue" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunTuesday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Wed" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunWednesday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Thu" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunThursday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fri" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunFriday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sat" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "2" ? (Boolean.Parse(Eval("RunSaturday").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MonthScheduleDay" HeaderText="Month Sch.Day"
                                ItemStyle-HorizontalAlign="center">
                                <HeaderStyle HorizontalAlign="center" Width="6%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Month LastDay" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate><%# (Eval("RecurrenceTypeID") == null? "0" : Eval("RecurrenceTypeID").ToString()) == "3" ? (Boolean.Parse(Eval("MonthLastDay").ToString()) ? "Yes" : "No") : ""%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderText="Export" Visible="false"
                                ItemStyle-HorizontalAlign="center"
                                HeaderStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkExport" runat="server" CommandName="Export" CommandArgument='<%# Eval("FilterScheduleId")%>'><img src="../Images/export.png" alt="" style="border:none;" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkEdit" NavigateUrl='<%# Eval("FilterScheduleId", "FilterExport.aspx?FilterScheduleId=" + Eval("FilterScheduleId").ToString() + "&FilterID=" +  Eval("FilterId").ToString() + "&FilterSegmentationId=" + Eval("FilterSegmentationId").ToString()) %>'
                                        HeaderText="Edit" runat="server"
                                        Text="&lt;img src='../images/ic-edit.gif' border='0'/&gt;"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField
                                ItemStyle-HorizontalAlign="center"
                                HeaderStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("FilterScheduleId")%>' OnClientClick="return confirm('Are you sure you want to delete?')"><img src="../Images/icon-delete.gif" alt="" style="border:none;"  /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </asp:PlaceHolder>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

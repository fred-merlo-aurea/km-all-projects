<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="FilterList.aspx.cs" Inherits="KMPS.MD.Main.FilterList" %>

<%@ Register TagName="FilterSave" TagPrefix="uc" Src="~/Controls/FilterSavePanel.ascx" %>
<%@ Register TagName="FilterSegmentationSave" TagPrefix="uc" Src="~/Controls/FilterSegmentationSave.ascx" %>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
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
                </div>
                <br />
                <div style="width: 90%; text-align: left;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="20%">
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
                                    <asp:ListItem Value="FilterSegmentation">Filter Segmentation</asp:ListItem>
                                </asp:RadioButtonList>&nbsp;&nbsp;</b>
                            </td>
                            <td align="right" width="55%">
                                <b><asp:Label ID="lblSearch" runat="server" Text="Filter Name or Question Name"></asp:Label></b>&nbsp;
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
                </div>
                <br />
                <div style="width: 90%; text-align: left;">
                    <div style="width: 20%; float: left;">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" style="border: solid 1px #5783BD; background-color: white">
                            <tr style="background-color: #5783BD;">
                                <td style="padding: 5px; font-size: 14px; color: #ffffff; font-weight: bold; text-align: left;">Filters Category
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 5px 5px 5px 5px;">
                                    <telerik:RadTreeView ID="rtvFilterCategory" runat="server" IsExpanded="False" IsSelected="False" Expand="True" OnNodeClick="rtvFilterCategory_OnNodeClick" DataTextField="CategoryName" DataFieldID="FilterCategoryID" DataFieldParentID="ParentID" DataValueField="FilterCategoryID" Height="670px">
                                        <DataBindings>
                                            <telerik:RadTreeNodeBinding></telerik:RadTreeNodeBinding>
                                        </DataBindings>
                                    </telerik:RadTreeView>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 79%; float: right; padding-left: 1px;">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="center">
                                    <asp:PlaceHolder ID="phFilters" runat="server">
                                    <asp:GridView ID="gvFilters" runat="server" AllowPaging="True" AllowSorting="True" OnSorting="gvFilters_Sorting" OnRowDataBound="gvFilters_RowDataBound" ShowHeader="true"
                                        AutoGenerateColumns="False" DataKeyNames="FilterId" OnRowCommand="gvFilters_RowCommand" OnPageIndexChanging="gvFilters_PageIndexChanging" OnRowDeleting="gvFilters_OnRowDeleting" OnRowEditing="grdFilters_RowEditing" BorderColor="#5783BD">
                                        <Columns>
                                            <asp:TemplateField HeaderText="View Type" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" SortExpression="FilterType">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFilterType" runat="server" Text='<%# Eval("FilterType").ToString().Replace("View", " View")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Brand Name" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" SortExpression="BrandName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBrandName" runat="server" Text='<%# Eval("BrandName") == null ? "" : Eval("BrandName").ToString() %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Name" HeaderText="Filter Name" SortExpression="Name"
                                                ItemStyle-HorizontalAlign="Left">
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Notes" SortExpression="Notes" HeaderStyle-Width="5%" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="center"
                                                ItemStyle-HorizontalAlign="center">
                                                <ItemTemplate>
                                                    <asp:Image ID="ImgNotes" runat="server" title='<%# Eval("Notes") %>'  ImageUrl="../Images/ic-note.jpg" style='<%# Eval("Notes") == "" || Eval("Notes") == null? "display:none":"display:inline" %>'  />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PubName" HeaderText="Product" SortExpression="PubName"
                                                ItemStyle-HorizontalAlign="left">
                                                <HeaderStyle HorizontalAlign="left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FilterCategoryName" HeaderText="Filter Category" SortExpression="FilterCategoryName"
                                                ItemStyle-HorizontalAlign="left">
                                                <HeaderStyle HorizontalAlign="left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="QuestionCategoryName" HeaderText="Question Category" SortExpression="QuestionCategoryName" Visible="false"
                                                ItemStyle-HorizontalAlign="left">
                                                <HeaderStyle HorizontalAlign="left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="QuestionName" HeaderText="Question Name" SortExpression="QuestionName"
                                                ItemStyle-HorizontalAlign="left">
                                                <HeaderStyle HorizontalAlign="left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CreatedName" HeaderText="Created By" SortExpression="CreatedName"
                                                ItemStyle-HorizontalAlign="left">
                                                <HeaderStyle HorizontalAlign="left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Created Date" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                                                ItemStyle-HorizontalAlign="center" SortExpression="CreatedDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate").ToString()  %>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Schedule" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                HeaderStyle-HorizontalAlign="center">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lnkSchedule" NavigateUrl='<%# Eval("FilterId", "FilterExport.aspx?FilterId=" + Eval("FilterId").ToString()) %>'
                                                        HeaderText="Schedule" runat="server"
                                                        Text="&lt;img src='../Images/ic-schedule.png' border='0'/&gt;"></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("FilterId")%>'
                                                        CommandName="FilterEdit"> <img src="../Images/ic-edit.gif" alt="" style="border:none;" /></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                ItemStyle-HorizontalAlign="center"
                                                HeaderStyle-HorizontalAlign="center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("FilterId")%>' OnClientClick="return confirm('Are you sure you want to delete?')"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phFilterSegmentations" runat="server" Visible="false">
                                    <asp:GridView ID="gvFilterSegmentations" runat="server" AllowPaging="True" AllowSorting="True" OnSorting="gvFilterSegmentations_Sorting" OnRowDataBound="gvFilterSegmentations_RowDataBound" ShowHeader="true"
                                        AutoGenerateColumns="False" DataKeyNames="FilterSegmentationId" OnRowCommand="gvFilterSegmentations_RowCommand" OnPageIndexChanging="gvFilterSegmentations_PageIndexChanging" OnRowDeleting="gvFilterSegmentations_OnRowDeleting" OnRowEditing="gvFilterSegmentations_RowEditing" BorderColor="#5783BD">
                                        <Columns>
                                            <asp:TemplateField HeaderText="View Type" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" SortExpression="FilterType">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFilterType" runat="server" Text='<%# Eval("FilterType").ToString().Replace("View", " View")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Brand Name" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" SortExpression="BrandName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBrandName" runat="server" Text='<%# Eval("BrandName") == null ? "" : Eval("BrandName").ToString() %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Name" HeaderText="Filter Name" SortExpression="Name"
                                                ItemStyle-HorizontalAlign="Left">
                                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FilterSegmentationName" HeaderText="Filter Segmentation" SortExpression="FilterSegmentationName"
                                                ItemStyle-HorizontalAlign="Left">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Notes" SortExpression="Notes" HeaderStyle-Width="5%" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="center"
                                                ItemStyle-HorizontalAlign="center">
                                                <ItemTemplate>
                                                    <asp:Image ID="ImgNotes" runat="server" title='<%# Eval("Notes") %>'  ImageUrl="../Images/ic-note.jpg" style='<%# Eval("Notes") == "" || Eval("Notes") == null? "display:none":"display:inline" %>'  />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PubName" HeaderText="Product" SortExpression="PubName"
                                                ItemStyle-HorizontalAlign="left">
                                                <HeaderStyle HorizontalAlign="left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FilterCategoryName" HeaderText="Filter Category" SortExpression="FilterCategoryName"
                                                ItemStyle-HorizontalAlign="left">
                                                <HeaderStyle HorizontalAlign="left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CreatedName" HeaderText="Created By" SortExpression="CreatedName"
                                                ItemStyle-HorizontalAlign="left">
                                                <HeaderStyle HorizontalAlign="left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Created Date" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="center"
                                                ItemStyle-HorizontalAlign="center" SortExpression="CreatedDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate").ToString()  %>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Schedule" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                HeaderStyle-HorizontalAlign="center">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lnkSchedule" NavigateUrl='<%# Eval("FilterId", "FilterExport.aspx?FilterSegmentationId=" + Eval("FilterSegmentationId").ToString()) %>'
                                                        HeaderText="Schedule" runat="server"
                                                        Text="&lt;img src='../Images/ic-schedule.png' border='0'/&gt;"></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("FilterSegmentationId")%>'
                                                        CommandName="FilterSegmentationEdit"> <img src="../Images/ic-edit.gif" alt="" style="border:none;" /></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                ItemStyle-HorizontalAlign="center"
                                                HeaderStyle-HorizontalAlign="center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("FilterSegmentationId")%>' OnClientClick="return confirm('Are you sure you want to delete?')"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    </asp:PlaceHolder>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </center>
            <uc:FilterSave runat="server" ID="FilterSave" Visible="false"></uc:FilterSave>
            <uc:FilterSegmentationSave runat="server" ID="FilterSegmentationSave" Visible="false"></uc:FilterSegmentationSave>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

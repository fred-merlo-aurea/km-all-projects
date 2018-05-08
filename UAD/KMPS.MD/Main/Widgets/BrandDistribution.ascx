<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BrandDistribution.ascx.cs" Inherits="KMPS.MD.Main.Widgets.BrandDistribution" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <style>
            .smalllogo {
                height: 50%;
                width: 50%;
            }
        </style>
        <div style="text-align:right"><img src="../Images/icon-DrillI.png" style="border:none" /></div>
        <table cellpadding="5" cellspacing="5" style="width: 100%">
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: center">
                    <telerik:RadDropDownList ID="rddlDimension" runat="server" DropDownHeight="200px" Width="200px"
                        DropDownWidth="200px"
                        DataValueField="MasterGroupID" DataTextField="DisplayName" OnSelectedIndexChanged="rddlDimension_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadDropDownList>
                    
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <div style="font: 16px Arial,Helvetica,sans-serif; width: 100%; text-align: center; padding-top: 10px; padding-bottom: 10px;">Brands</div>
                    <br />
                    <br />
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGridBrand" runat="server" AutoGenerateColumns="False"
                        OnSelectedIndexChanged="RadGridBrand_SelectedIndexChanged" OnPageIndexChanged="RadGridBrand_PageIndexChanged" OnItemDataBound="RadGridBrand_ItemDataBound" OnDataBound="RadGridBrand_DataBound"
                        Width="300px" GridLines="None" AllowMultiRowSelection="false"
                        PageSize="10" AllowPaging="true">
                        <MasterTableView DataKeyNames="BrandID">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Logo" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLogo" runat="server" Text='<%# Eval("Logo", "{0}") %>' Visible="false"></asp:Label>
                                        <asp:Image ID="imgLogo" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="BrandID" UniqueName="BrandID" HeaderText="BrandID"
                                    EmptyDataText="&amp;nbsp;" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="BrandName" UniqueName="BrandName" HeaderText="Brand" ItemStyle-Width="150px"
                                    EmptyDataText="&amp;nbsp;" HeaderStyle-HorizontalAlign="left">
                                    <ItemStyle HorizontalAlign="left" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ProductCounts" UniqueName="ProductCounts" HeaderText="Product Counts" ItemStyle-Width="50px"
                                    EmptyDataText="&amp;nbsp;" HeaderStyle-HorizontalAlign="Right">
                                    <ItemStyle HorizontalAlign="Right" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Counts" UniqueName="Counts" HeaderText="Records" ItemStyle-Width="100px"
                                    EmptyDataText="&amp;nbsp;" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N0}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </telerik:GridBoundColumn>

                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnablePostBackOnRowClick="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>
                        <PagerStyle Mode="NumericPages"></PagerStyle>
                        <HeaderStyle Height="25px"></HeaderStyle>
                    </telerik:RadGrid>
                </td>
                <td style="vertical-align: top">
                    <telerik:RadHtmlChart runat="server" ID="RadHtmlChart1" Width="600" Height="500" Legend-Appearance-Visible="true">
                        <ChartTitle Text="Product Counts">
                        </ChartTitle>
                        <Legend>
                            <Appearance Position="bottom" Visible="false">
                            </Appearance>
                        </Legend>
                        <PlotArea>
                            <Series>
                                <telerik:DonutSeries DataFieldY="PubPercentage" NameField="PubName">
                                    <LabelsAppearance DataFormatString="{0} %" Position="OutsideEnd">
                                    </LabelsAppearance>
                                    <TooltipsAppearance Color="White" ClientTemplate="#=dataItem.PubName# (#=dataItem.Pubcode#): #=dataItem.Counts#"></TooltipsAppearance>
                                </telerik:DonutSeries>
                            </Series>
                            <YAxis>
                            </YAxis>
                        </PlotArea>
                    </telerik:RadHtmlChart>
                    <telerik:RadHtmlChart runat="server" ID="RadHtmlChart2" Width="600" Height="500" Visible="false">
                        <ChartTitle Text="Counts">
                        </ChartTitle>
                        <Legend>
                            <Appearance Position="bottom" Visible="false">
                            </Appearance>
                        </Legend>
                        <PlotArea>
                            <Series>
                                <telerik:DonutSeries DataFieldY="Percentage" NameField="MasterDesc">
                                    <LabelsAppearance DataFormatString="{0} %" Position="OutsideEnd">
                                    </LabelsAppearance>
                                    <TooltipsAppearance Color="White" ClientTemplate="#=dataItem.MasterDesc# (#=dataItem.MasterValue#): #=dataItem.Counts#"></TooltipsAppearance>
                                </telerik:DonutSeries>
                            </Series>
                            <YAxis>
                            </YAxis>
                        </PlotArea>
                    </telerik:RadHtmlChart>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

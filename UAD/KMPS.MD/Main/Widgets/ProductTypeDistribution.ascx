<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductTypeDistribution.ascx.cs" Inherits="KMPS.MD.Main.Widgets.ProductTypeDistribution" %>

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
                        DataValueField="MasterGroupID" DataTextField="DisplayName"  OnSelectedIndexChanged="rddlDimension_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadDropDownList>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <div style="font: 16px Arial,Helvetica,sans-serif; width: 100%; text-align: center; padding-top: 10px; padding-bottom: 10px;">Product Types</div>
                    <br />
                    <br />
                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGridProductType" runat="server" AutoGenerateColumns="False"
                        OnSelectedIndexChanged="RadGridProductType_SelectedIndexChanged" OnNeedDataSource="RadGridProductType_NeedDataSource" OnDataBound="RadGridProductType_DataBound"
                        Width="300px" GridLines="None" AllowMultiRowSelection="false"
                        PageSize="10" AllowPaging="true">
                        <MasterTableView DataKeyNames="PubTypeID">
                            <Columns>
                                <telerik:GridBoundColumn DataField="PubTypeID" UniqueName="PubTypeID" HeaderText="PubTypeID"
                                    EmptyDataText="&amp;nbsp;" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PubTypeDisplayName" UniqueName="PubTypeDisplayName" HeaderText="Product Type" ItemStyle-Width="150px"
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
                    <telerik:RadHtmlChart runat="server" ID="RadHtmlChart1" Width="600" Height="500" Legend-Appearance-Visible="true" >
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
                    <telerik:RadHtmlChart runat="server" ID="RadHtmlChart2" Width="600" Height="500" Visible="false" Legend-Appearance-Visible="true">
                        <ChartTitle Text=" Counts">
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

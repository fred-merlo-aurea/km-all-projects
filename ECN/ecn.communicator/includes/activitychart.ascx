<%@ Register TagPrefix="Web" Assembly="WebChart" Namespace="WebChart" %>
<%@ Control Language="c#" Inherits="ecn.communicator.includes.activitychart" Codebehind="activitychart.ascx.cs" %>
<Web:ChartControl runat="server" ID="ActivityChart" Height="125px" Width="700px"
    ChartPadding="0" BorderStyle="None" BorderWidth="0px" Padding="0" TopPadding="0"
    LeftChartPadding="35" BottomChartPadding="20" TopChartPadding="10" RightChartPadding="10">
    <XTitle ForeColor="Black" StringFormat="Center,Near,Character,LineLimit" Font="Tahoma, 8pt">
    </XTitle>
    <YAxisFont ForeColor="Black" StringFormat="Far,Near,Character,LineLimit" Font="Tahoma, 8pt">
    </YAxisFont>
    <ChartTitle ForeColor="Black" StringFormat="Center,Near,Character,LineLimit" Font="Tahoma, 1pt, style=Bold">
    </ChartTitle>
    <XAxisFont ForeColor="Black" StringFormat="Center,Near,Character,LineLimit" Font="Tahoma, 8pt">
    </XAxisFont>
    <Legend Position="Top" Width="0" Font="Tahoma, 7pt">
        <Border EndCap="Flat" DashStyle="Solid" StartCap="Flat" Color="Transparent" Width="0"
            LineJoin="Miter"></Border>
        <Background CenterPoint="0, 0" Type="Solid" CenterColor="" StartPoint="0, 0" ForeColor="Black"
            LinearGradientMode="Horizontal" WrapMode="Tile" EndPoint="100, 100" Color="White"
            HatchStyle="Shingle"></Background>
    </Legend>
    <Background CenterPoint="0, 0" Type="Solid" CenterColor="" StartPoint="0, 0" ForeColor="Black"
        LinearGradientMode="Horizontal" WrapMode="Tile" EndPoint="100, 100" Color="Transparent"
        HatchStyle="Shingle"></Background>
    <YTitle ForeColor="Black" StringFormat="Center,Near,Character,LineLimit" Font="Tahoma, 8pt">
    </YTitle>
    <Border EndCap="Flat" DashStyle="Solid" StartCap="Flat" Color="Black" Width="0" LineJoin="Miter">
    </Border>
    <PlotBackground CenterPoint="0, 0" Type="Solid" CenterColor="" StartPoint="0, 0"
        ForeColor="Black" LinearGradientMode="Horizontal" WrapMode="Tile" EndPoint="100, 100"
        Color="LightSteelBlue" HatchStyle="Shingle"></PlotBackground>
</Web:ChartControl>

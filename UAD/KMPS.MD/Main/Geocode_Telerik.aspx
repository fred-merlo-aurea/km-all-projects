<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Geocode_Telerik.aspx.cs" Inherits="KMPS.MD.Main.Geocode_Telerik" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript">
        var markerData = [{
            "location": [51.506421, -0.127215],
            "city": "London"
        }, {
            "location": [51.762152, -1.258430],
            "city": "Oxford"
        }];


        function OnInitialize(sender, args) {
            var originalOptions = args.get_options();

            originalOptions.layers[1].dataSource = { data: markerData };

            args.set_options(originalOptions);
        }


    </script>
<%--    <table>
        <tr>
            <td colspan="2" align="left">
                <br />
                <u><b>Enter an address </u></b><br />
                <br />
            </td>
        </tr>
        <tr>
            <td>Street Address
            </td>
            <td align="left">
                <asp:TextBox ID="textboxAddress" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>City
            </td>
            <td align="left">
                <asp:TextBox ID="textboxCity" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>State
            </td>
            <td align="left">
                <asp:DropDownList ID="DropDownListState" runat="server">
                    <asp:ListItem Value="*">-Select-</asp:ListItem>
                    <asp:ListItem Value="AL">Alabama</asp:ListItem>
                    <asp:ListItem Value="AK">Alaska</asp:ListItem>
                    <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                    <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                    <asp:ListItem Value="CA">California</asp:ListItem>
                    <asp:ListItem Value="CO">Colorado</asp:ListItem>
                    <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                    <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                    <asp:ListItem Value="DE">Delaware</asp:ListItem>
                    <asp:ListItem Value="FL">Florida</asp:ListItem>
                    <asp:ListItem Value="GA">Georgia</asp:ListItem>
                    <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                    <asp:ListItem Value="ID">Idaho</asp:ListItem>
                    <asp:ListItem Value="IL">Illinois</asp:ListItem>
                    <asp:ListItem Value="IN">Indiana</asp:ListItem>
                    <asp:ListItem Value="IA">Iowa</asp:ListItem>
                    <asp:ListItem Value="KS">Kansas</asp:ListItem>
                    <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                    <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                    <asp:ListItem Value="ME">Maine</asp:ListItem>
                    <asp:ListItem Value="MD">Maryland</asp:ListItem>
                    <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                    <asp:ListItem Value="MI">Michigan</asp:ListItem>
                    <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                    <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                    <asp:ListItem Value="MO">Missouri</asp:ListItem>
                    <asp:ListItem Value="MT">Montana</asp:ListItem>
                    <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                    <asp:ListItem Value="NV">Nevada</asp:ListItem>
                    <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                    <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                    <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                    <asp:ListItem Value="NY">New York</asp:ListItem>
                    <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                    <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                    <asp:ListItem Value="OH">Ohio</asp:ListItem>
                    <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                    <asp:ListItem Value="OR">Oregon</asp:ListItem>
                    <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                    <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                    <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                    <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                    <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                    <asp:ListItem Value="TX">Texas</asp:ListItem>
                    <asp:ListItem Value="UT">Utah</asp:ListItem>
                    <asp:ListItem Value="VT">Vermont</asp:ListItem>
                    <asp:ListItem Value="VA">Virginia</asp:ListItem>
                    <asp:ListItem Value="WA">Washington</asp:ListItem>
                    <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                    <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                    <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Zip Code
            </td>
            <td align="left">
                <asp:TextBox ID="textboxZip" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>


            <td>Radius
            </td>
            <td align="left">
                <asp:TextBox ID="textboxRadius" runat="server"></asp:TextBox>
                <br />
                <font style="font-size: x-small">Range (0 to 300 miles)</font>
            </td>
        </tr>
        <tr valign="top">
            <td colspan="2" align="center">
                <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Submit_Click" CssClass="button"
                    ValidationGroup="Validate" />
                &nbsp&nbsp
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Label ID="lbMsg" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>--%>
    <br />
    <br />
    <table width="100%">
        <tr>
            <td>
                <asp:HiddenField runat="server" ID="HiddenGeoJSON" />
                <telerik:RadMap runat="server" ID="RadGeoCode" Zoom="2" Width="940" Height="500" CssClass="MyMap" OnItemDataBound="RadMap1_ItemDataBound" AppendDataBoundMarkers="True" EnableViewState="True" MapMouseClick="radMap_MapMouseClick">
                   <%-- <clientevents oninitialize="OnInitialize" />--%>
                    <centersettings latitude="23" longitude="10" />
                    <databindings>
                        <MarkerBinding DataShapeField="Shape" DataTitleField="City" DataLocationLatitudeField="Latitude" DataLocationLongitudeField="Longitude" />
                    </databindings>
                    <layerscollection>
                        <telerik:MapLayer Type="Tile" Subdomains="a,b,c"
                            UrlTemplate="http://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png"
                            Attribution="">
                        </telerik:MapLayer>
                        <telerik:MapLayer Type="Marker" Shape="pinTarget">
                            <TooltipSettings AutoHide="false" Template="#= marker.dataItem.city # <br/> #=  location.lat #, #=  location.lng #"></TooltipSettings>
                        </telerik:MapLayer>
                    </layerscollection>
                </telerik:RadMap>
                <asp:Button ID="Button3" runat="server" Text="Submit" OnClick="Submit_Click" CssClass="button"
                     />
            </td>
        </tr>
        <tr>
            <td align="left">
                <div id="myMap" style="position: relative; width: 100%; height: 750px; cursor: crosshair !important; z-index: 1">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

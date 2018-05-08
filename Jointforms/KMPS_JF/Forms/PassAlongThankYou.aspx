<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PassAlongThankYou.aspx.cs" Inherits="KMPS_JF.Forms.PassAlongThankYou" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ThankYou</title>
    <!-- START Conversion Tracking -->

    <script language="javascript" src="getConversionData.js"></script>

    <script language="Javascript">
	<!--
        ECNstepname = 'ThankYou';
        document.write("<img src='" + getConversionURL() + "' height=1 width=1 border=0>");
	//-->
    </script>

    <!-- END Conversion Tracking -->
    <link href="../CSS/styles.css" rel="stylesheet" type="text/css">
    <div id="divcss" runat="server">
    </div>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container"> 
        <div id="innerContainer">
            <div>
                <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                <asp:Label ID="lblPageDesc" runat="server">
                    <p>
                        <span> 
                            <br /><br /><br />
                            Thank you for sharing the names of your colleagues. We will be providing them with a complimentary <br /> digital copy of <asp:Label ID="lblPubName" Text="" runat="server"/> magazine.                         
                            <br /><br /><br />
                        </span>
                    </p>
                </asp:Label>
                <asp:GridView ID="gvRelatedPubs" runat="server" AllowPaging="false" AllowSorting="false"
                    AutoGenerateColumns="False" CellPadding="10" CellSpacing="5" DataKeyNames="LinkedToPubID"
                    ForeColor="Black" GridLines="None" ShowFooter="false" ShowHeader="false" Width="90%">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="left" ItemStyle-Width="2%" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <b><i><a href='Subscription.aspx?PubID=<%# Eval("LinkedToPubID") %> '>
                                    <img alt='<%# Eval("PubName") %>' src='<%# "../PubLogo/" + Eval("pubLogo") %>' border="0"
                                        style='display: <%# Eval("pubLogo").ToString().Trim()==""?"none":"block" %>' /></a>
                                </i></b>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="left" ItemStyle-Width="98%" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                &nbsp;&nbsp;&nbsp; <b><i><a href='Subscription.aspx?Pubcode=<%# Eval("PubCode") %> '>
                                    <%# Eval("PubName") %>
                                    (<%# Eval("PubCode") %>
                                    )</a> </i></b>
                            </ItemTemplate>
                        </asp:TemplateField> 
                    </Columns>
                </asp:GridView>
                <p>
                    <span class="label"><a class="label" href="Subscription.aspx?Pubcode=<% = PubCode%>">
                        RETURN TO SUBSCRIPTION FORM</a></span>
                    <br />
                </p>
                <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
            </div>
        </div>
    </div>
    </form>
     <script type="text/javascript">
var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
</script>
<script type="text/javascript">
    try
    {
        var pageTracker = _gat._getTracker("UA-10775168-1");
        pageTracker._trackPageview('/thankyou.aspx');
    } catch (err)
    {
    }
    try
    {
        var ECNpageTracker = _gat._getTracker("UA-2081962-10");
        ECNpageTracker._trackPageview();
    } catch (err)
    {
    }
</script>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DESubscribe.aspx.cs" Inherits="ecn.digitaledition.DESubscribe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width='100%' border='0' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF'>
            <tr>
                <td align='center'>
                    <table width='456' border='0' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF'>
                        <tr>
                            <td width='31'>
                                <img height='30' alt='' width='30' src='http://www.ecndigitaledition.com/images/f2f-art_01.gif' />
                            </td>
                            <td>
                                <img height='30' alt='' width='395' src='http://www.ecndigitaledition.com/images/f2f-art_02.gif' />
                            </td>
                            <td width='31'>
                                <img height='30' alt='' width='31' src='http://www.ecndigitaledition.com/images/f2f-art_03.gif' />
                            </td>
                        </tr>
                        <tr>
                            <td height='100%'>
                                <img height='100%' alt='' width='30' src='http://www.ecndigitaledition.com/images/f2f-art_04.gif' />
                            </td>
                            <td valign='top'>
                                <table width='394' height='100%' border='0' cellpadding='0' cellspacing='0' bgcolor='#FFFFFF'>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign='middle' align='center' colspan='3' height='180'>
                                            <asp:HyperLink ID="hl" runat="server"></asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign='middle' align='center' colspan='3'>
                                            <strong><font face='Arial' size='2'>
                                                <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                                            </font></strong>
                                            <br>
                                            <br>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' colspan='3'>
                                            
                                            <font face='Arial' size='2'><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label><br />
                                                <asp:Button ID="btnSubscribe" runat="server" Text="Subscribe" 
                                                onclick="btnSubscribe_Click" />
                                            </font><strong>
                                                <hr color='#999999' size='1' />                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width='20'>
                                            &nbsp;
                                        </td>
                                        <td align='left'>
                                            <font face='Arial' size='2'></font>
                                        </td>
                                        <td width='20'>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align='left'>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td height='100%'>
                                <img height='100%' alt='' width='31' src='http://www.ecndigitaledition.com/images/f2f-art_06.gif' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img height='31' alt='' width='30' src='http://www.ecndigitaledition.com/images/f2f-art_07.gif' />
                            </td>
                            <td>
                                <img height='31' alt='' width='395' src='http://www.ecndigitaledition.com/images/f2f-art_08.gif' />
                            </td>
                            <td>
                                <img height='31' alt='' width='31' src='http://www.ecndigitaledition.com/images/f2f-art_09.gif' />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

    </div>
    </form>
</body>
</html>

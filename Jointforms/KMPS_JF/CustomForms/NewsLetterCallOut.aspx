<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsLetterCallOut.aspx.cs"
    Inherits="KMPS_JF.CustomForms.NewsLetterCallOut" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Editorial Comps Subscription Form </title>
    <link href="../CSS/styles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .errorsummary
        {
            border: 1px solid black;
            color: red;
            margin: 5px 0px;
            padding: 15px;
            background: white url(../images/ic-alert.jpg) no-repeat 5px 50%;
            background-color: white;
        }
        .errorsummary ul
        {
            margin: 5px;
            padding: 10px;
            margin-left: 80px;
            list-style: square;
            color: red;
        }
        .ccblcategory
        {
            font-size: 11pt;
            font-weight: 900;
            padding: 5px 0px 5px 0px;
            text-align: left;
        }
        .Grouppadding
        {
            padding: 5px 3px 5px 0px;
        }
        .addpadding
        {
            padding: 3px 3px 3px 3px;
        }
    </style>
    <div id="divcss">
        <style type='text/css'>
            body
            {
                background-color: #c0c0c0;
                text-align: center;
                padding: 20px 0;
                font-size: 12px;
                font-family: Arial;
            }
            #container
            {
                font-family: Arial;
                text-align: left;
                width: 760px;
                background-color: #FFFFFF;
                margin: 0 auto;
                border: 2px #000 solid;
                min-height: 100%;
                height: auto !important;
                height: 100%;
                width: 800px;
            }
            .Category
            {
                font-family: Arial;
                background-color: #c0c0c0;
                font-size: 12px;
                color: #;
            }
            .label
            {
                font-family: Arial;
                font-size: 12px;
                color: #000000;
                font-weight: normal;
            }
            .labelAnswer
            {
                font-family: Arial;
                font-size: 12px;
                color: #000000;
                font-weight: normal;
            }
        </style>
        <style>
            #question_COMP td
            {
                vertical-align: top;
                border: 0px solid black;
                padding: 1px 1px 1px 1px;
            }
            #question_COMP label
            {
                margin-left: 2px;
            }
            .style1
            {
                height: 23px;
            }
        </style>
    </div>
</head>
<body>
    <form id="Form1" runat="server">   
        <div id="container" runat="server">
        <div id="innerContainer">
            <div>
                <span id="lblPageDesc"></span>
                <br />
                <br />
                <table width="100%">
                    <tr>
                        <td align="right" class="style1">
                            <b>Publication:</b>
                        </td>
                        <td class="style1">
                            <asp:DropDownList ID="drpPublication" runat="server">
                                <asp:ListItem Text="HITN" Value="HITN" />
                                <asp:ListItem Text="HIITN" Value="HIITN" />                                 
                                <asp:ListItem Text="HFNS" Value="HFNS" />
                                <asp:ListItem Text="HPNW" Value="HPNW" />   
                                <asp:ListItem Text="GHIT" Value="GHIT" />  
                                <asp:ListItem Text="PBIZ" Value="PBIZ" />
                                <asp:ListItem Text="MHIMSS" Value="MHIMSS" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Email Address:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmailAddress" runat="server" Width="250px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnSubmit" Text="Submit" Width="100px" runat="server" OnClick="btnSubmit_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>                                             
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailPreviewUsage.aspx.cs"
    Inherits="ecn.communicator.main.blastsmanager.EmailPreviewUsage" MasterPageFile="~/MasterPages/Communicator.Master" %>


<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="ecn" TagName="gallery" Src="../../includes/imageGallery.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />

<script type="text/javascript" >
    
    if (window.attachEvent) { window.attachEvent('onload', pageloaded); }
    else if (window.addEventListener) { window.addEventListener('load', pageloaded, false); }
    else { document.addEventListener('load', pageloaded, false); }

    function pageloaded() {
        $('.subject').each(function () {
            var initialString = $(this).html();
            initialString = initialString.replace(/'/g, "\\'");
            initialString = initialString.replace(/\r?\n|\r/g, ' ');
            initialString = twemoji.parse(eval("\'" + initialString + "\'"), { size: "16x16" });
            $(this).html(initialString);
            //if (!initialString.includes('<img')) {
            //    initialString = initialString.substr(0, 30);
            //}

            //var regSplit = new RegExp("(<img.*?\/?>)");

            //var imgSplit = new Array();

            //imgSplit = initialString.split(regSplit);
            //var textFullSplit = new Array();

            //for (var i = 0; i < imgSplit.length; i++) {
            //    var current = imgSplit[i];
            //    if (current.includes('<img')) {
            //        //currentindex is image add to finalSplit
            //        textFullSplit.push(current);
            //    }
            //    else {
            //        //currentindex is plain text, loop through each char and add to final split
            //        for (var j = 0; j < current.length; j++) {
            //            textFullSplit.push(current.charAt(j));
            //        }
            //    }
            //}

            //var finalText = "";

            //if (initialString.length > 0) {
            //    if (textFullSplit.length > 30) {
            //        for (var i = 0; i < 30; i++) {
            //            finalText = finalText.concat(textFullSplit[i]);
            //        }
            //        finalText = finalText + '...';
            //    }
                
            //}
            //else {
            //    finalText = initialString;
            //}


            //$(this).html(finalText);
        })
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td align='right' valign="top" class="label">
                <b>Customer&nbsp;:&nbsp;</b>
                <asp:DropDownList ID="drpCustomer" runat="Server" CssClass="formlabel" Width="200px">
                </asp:DropDownList>
                &nbsp;&nbsp; <b>Month&nbsp;:&nbsp;</b>
                <asp:DropDownList ID="drpMonth" runat="server" Width="100">
                    <asp:ListItem Value="1">Jan</asp:ListItem>
                    <asp:ListItem Value="2">Feb</asp:ListItem>
                    <asp:ListItem Value="3">Mar</asp:ListItem>
                    <asp:ListItem Value="4">Apr</asp:ListItem>
                    <asp:ListItem Value="5">May</asp:ListItem>
                    <asp:ListItem Value="6">Jun</asp:ListItem>
                    <asp:ListItem Value="7">Jul</asp:ListItem>
                    <asp:ListItem Value="8">Aug</asp:ListItem>
                    <asp:ListItem Value="9">Sep</asp:ListItem>
                    <asp:ListItem Value="10">Oct</asp:ListItem>
                    <asp:ListItem Value="11">Nov</asp:ListItem>
                    <asp:ListItem Value="12">Dec</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp; <b>Year&nbsp;:&nbsp;</b>
                <asp:DropDownList ID="drpYear" runat="server" Width="100">
 <%--                   <asp:ListItem Value="2011">2011</asp:ListItem>
                    <asp:ListItem Value="2012">2012</asp:ListItem>
                    <asp:ListItem Value="2013">2013</asp:ListItem>
                    <asp:ListItem Value="2014">2014</asp:ListItem>
                    <asp:ListItem Value="2015">2015</asp:ListItem>
                    <asp:ListItem Value="2016">2015</asp:ListItem>--%>
                </asp:DropDownList>
                &nbsp;&nbsp;
            <asp:Button class="formbuttonsmall" ID="btnSearch" runat="Server" Width="75" Text="Search"
                OnClick="btnSearch_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td>
                <ecnCustom:ecnGridView ID="grdEmailPreviewUsage" runat="Server" AutoGenerateColumns="False"
                    DataKeyNames="CustomerID" datakeyfield="CustomerID" Style="margin: 7px 0;" Width="100%"
                    CssClass="grid" OnPageIndexChanging="grdEmailPreviewUsage_PageIndexChanging" OnRowCommand="grdEmailPreviewUsage_RowCommand"
                    ShowEmptyTable="true" EmptyTableRowText="No usage to display" 
                    AllowPaging="true" AllowSorting="true" OnSorting="ActiveGrid_Sorting">
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <Columns>
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName" ItemStyle-HorizontalAlign="left"></asp:BoundField>
                        <asp:TemplateField HeaderText="Preview Usage"   HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" SortExpression="Counts">
                            <ItemTemplate>
                                <asp:LinkButton runat="Server" Text='<%# Eval("Counts") %>'
                                    CausesValidation="false" ID="ViewDetails" CommandName="ViewDetails" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CustomerID") %>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass="gridaltrow" />
                </ecnCustom:ecnGridView>
            </td>
        </tr>
        <asp:Panel ID="pnlUsageDetails" runat="server" Visible="false">
        <tr>
            <td class="formLabel" valign="top" align='right' width="47%">Your Email Preview Usage is&nbsp;<span style="color: #ff0000"><asp:Label ID="lblPreviewUsage"
                runat="Server"></asp:Label>&nbsp;</span>out of&nbsp;<asp:Label ID="lblPreviewUsageCanBeUsed"
                    runat="Server"></asp:Label>&nbsp;
            <div id="capacityContainer">
                <div id="capacityBar" runat="Server">
                </div>
                <div id="capacityBarArrow" align='right' runat="Server">
                    <img src="/ecn.images/images/pointer.jpg" border='0'>
                </div>
            </div>
                <span class="formLabel" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 5px 0px 0px; padding-top: 0px; text-align: right">&nbsp;<asp:Label ID="lblPreviewUsageAvailable"
                    runat="Server"></asp:Label>&nbsp; Available</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hfCustomerID"  runat="server"/>
                <ecnCustom:ecnGridView ID="grdUsageDetails" runat="Server" AutoGenerateColumns="False"
                    DataKeyNames="BlastID" datakeyfield="BlastID" Style="margin: 7px 0;" Width="100%"
                    CssClass="grid" OnPageIndexChanging="grdUsageDetails_PageIndexChanging"
                    ShowEmptyTable="true" EmptyTableRowText="No usage to display"
                    AllowPaging="true" AllowSorting="true" OnSorting="grdUsageDetails_Sorting">
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <Columns>
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Width="10%" SortExpression="CustomerName" ItemStyle-HorizontalAlign="left"></asp:BoundField>
                        <asp:BoundField DataField="BlastID" HeaderText="BlastID" ItemStyle-Width="10%" SortExpression="BlastID" ItemStyle-HorizontalAlign="left"></asp:BoundField>
                        <asp:BoundField DataField="EmailSubject" HeaderText="EmailSubject" ItemStyle-CssClass="subject" ItemStyle-HorizontalAlign="left" ItemStyle-Width="50%"
                            SortExpression="EmailSubject"></asp:BoundField>
                        <asp:BoundField DataField="EmailFromName" HeaderText="EmailFromName" ItemStyle-HorizontalAlign="left" ItemStyle-Width="15%"
                            SortExpression="EmailFromName"></asp:BoundField>
                        <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" ItemStyle-Width="15%"
                            SortExpression="DateCreated"></asp:BoundField>
                    </Columns>
                    <AlternatingRowStyle CssClass="gridaltrow" />
                </ecnCustom:ecnGridView>
            </td>
        </tr>
        </asp:Panel>
    </table>
</asp:Content>

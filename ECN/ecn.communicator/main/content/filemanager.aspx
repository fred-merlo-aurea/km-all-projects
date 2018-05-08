<%@ Page Language="c#" Inherits="ecn.communicator.contentmanager.filemanager" CodeBehind="filemanager.aspx.cs"  MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecn" TagName="gallery" Src="../../includes/imageGallery.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td class="formLabel" valign="top" width="53%" align="left">
                <div style="display: none">
                    <asp:label id="StorageDisclaimer" runat="Server"></asp:label>
                </div>
                <p style="padding-right: 15px; padding-left: 15px; padding-bottom: 0px; text-indent: -15px;
                    padding-top: 0px">
                    <img src="/ecn.images/images/capacityDisclamer.gif">&nbsp;<strong>To avoid additional
                        charges, be sure to maintain your storage level under
                        <asp:label id="INFO_StorageCanBeUsedLBL1" runat="Server"></asp:label>
                        &nbsp;MB</strong>
                    <br />
                    Your ECN Account comes standard with
                    <asp:label id="INFO_StorageCanBeUsedLBL2" runat="Server"></asp:label>
                    &nbsp;MB of data storage. Storage capacity includes List and Image files. You may
                    at any time upgrade your data storage in increments of 25 MBs. Please contact your
                    ECN account representative for pricing.
                </p>
            </td>
            <td class="formLabel" valign="top" align='right' width="47%">
                Your file storage size is&nbsp;<span style="color: #ff0000"><asp:label id="StorageUsedLBL"
                    runat="Server"></asp:label>&nbsp;MB</span>&nbsp;out of&nbsp;<asp:label id="StorageCanBeUsedLBL"
                        runat="Server"></asp:label>&nbsp;MB
                <div id="capacityContainer">
                    <div id="capacityBar" runat="Server">
                    </div>
                    <div id="capacityBarArrow" align='right' runat="Server">
                        <img src="/ecn.images/images/pointer.jpg" border='0'></div>
                </div>
                <span class="formLabel" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
                    margin: 5px 0px 0px; padding-top: 0px; text-align: right">&nbsp;<asp:label id="StorageAvailableLBL"
                        runat="Server"></asp:label>&nbsp;MB Available</span>
            </td>
        </tr>
        <tr>
            <td colspan="2" height="10">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <ecn:gallery ID="maingallery" runat="Server" borderWidth="0" imagesPerColumn="5"
                    thumbnailSize="100"></ecn:gallery>
            </td>
        </tr>
    </table>
    <br />
</asp:content>

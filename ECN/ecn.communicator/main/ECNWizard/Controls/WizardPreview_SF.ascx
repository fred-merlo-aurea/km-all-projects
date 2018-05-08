<%@ Control Language="c#" Inherits="ecn.communicator.main.ECNWizard.Controls.WizardPreview_SF"
    CodeBehind="WizardPreview_SF.ascx.cs" %>

<div class="section bottomDiv" style="padding-left: 30px; padding-right: 30px">
    <table width="100%">
        <tr valign="top">
            <td width="50%">
                <fieldset>
                    <legend>
                        <table>
                            <tr>
                                <td>Details
                                </td>
                            </tr>
                        </table>
                    </legend>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblSelectedMessage" runat="server" Text="Message" Font-Size="Large"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr style="padding-left: 30px">
                            <td align="left" class="formLabel">
                                <b>Message Name:</b> &nbsp&nbsp
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                &nbsp&nbsp
                            </td>
                        </tr>
                        <tr style="padding-left: 30px">
                            <td align="left" class="formLabel">

                                <b>Preview:</b> &nbsp&nbsp  
                                <asp:HyperLink ID="hlPreviewHTML" runat="server"><img src="/ecn.images/images/icon-preview-HTML.gif" alt="Preview Message as HTML" border="0"></asp:HyperLink>
                                &nbsp&nbsp
            <asp:HyperLink ID="hlPreviewTEXT" runat="server"><img src="/ecn.images/images/icon-preview-TEXT.gif" alt="Preview Message as TEXT" border="0"></asp:HyperLink>
                                <br />
                                <br />
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSelected" runat="server" Text="Salesforce Campaign" Font-Size="Large"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr style="padding-left: 30px">
                            <td class="formLabel">
                                <table>
                                    <tr>
                                        <td>
                                            <b>Campaign Name:</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSFCampaignName" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="formLabel">
                                <table>
                                    <tr>
                                        <td>
                                            <b>Estimated Sends:</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSFEstimatedSends" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlBlastFields" runat="server">
                                    <asp:Label ID="lblBlastFieldHeader" runat="server" Text="Blast Fields" />
                                    <br />
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBlastField1" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBlastFieldValue1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBlastField2" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBlastFieldValue2" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBlastField3" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBlastFieldValue3" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBlastField4" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBlastFieldValue4" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBlastField5" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBlastFieldValue5" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;" id="tblSocial" runat="server">
                                    <tr>
                                        <td class="formlabel">
                                            <asp:Label ID="lblSocialHeader" runat="server" Text="Social Share" Font-Size="Large" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table id="tblSimple" runat="server">
                                                <tr>
                                                    <td style="vertical-align: top; font-size: 11px;">
                                                        <b>Simple Share:</b>
                                                    </td>
                                                    <td>
                                                        <asp:GridView ID="gvSimpleShare" runat="server" GridLines="None" ShowHeader="false" OnRowDataBound="gvSimpleShare_RowDataBound" AutoGenerateColumns="false">
                                                            <Columns>
                                                                <asp:TemplateField ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSocialMedia" Font-Size="11px" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="80%" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSocialMediaName" Font-Size="11px" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table id="tblSubscriber" runat="server">
                                                <tr>
                                                    <td style="vertical-align: top; font-size: 11px;">
                                                        <b>Subscriber Share:</b>
                                                    </td>
                                                    <td>
                                                        <asp:GridView ID="gvSubscriberShare" runat="server" GridLines="None" ShowHeader="false" AutoGenerateColumns="false" OnRowDataBound="gvSubscriberShare_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSocialMedia" Font-Size="11px" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSocialMediaName" Font-Size="11px" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>

                    </table>


                </fieldset>
            </td>

            <td width="50%">


                <fieldset>
                    <legend>
                        <table>
                            <tr>
                                <td>Envelope
                                </td>
                            </tr>
                        </table>
                    </legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-left: 30px">

                        <tr class="formLabel">
                            <td width="100%">
                                <b>Envelope:</b>
                                <table border="0" cellpadding="1" cellspacing="1" width="100%">
                                    <tr>
                                        <td class="formLabel">From Email:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFromEmail" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formLabel">Reply To:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReplyTo" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formLabel">From Name:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFromName" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formLabel">Subject:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSubject" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>

                        </tr>
                        <tr class="formLabel">
                            <td width="100%">
                                <asp:Panel ID="pnlDynamicPersonalization" runat="server" Visible="false">
                                    <b>Dynamic Personalization:</b>
                                    <table border="0" cellpadding="1" cellspacing="1" width="100%">
                                        <tr>
                                            <td class="formLabel">From Email:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFromEmail_DP" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel">Reply To:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblReplyTo_DP" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel">From Name:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFromName_DP" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel">&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </fieldset>

            </td>

        </tr>

    </table>
</div>

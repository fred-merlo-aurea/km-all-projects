<%@ Control Language="c#" Inherits="ecn.communicator.main.ECNWizard.Controls.WizardPreview"
    CodeBehind="WizardPreview_Regular.ascx.cs" %>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />

<script type="text/javascript">
    function pageLoad(sender, args)
    {
        var initialString = $('#<%= lblSubject.ClientID %>').html();
        initialString = initialString.replace(/'/g, "\\'");
        initialString = initialString.replace(/\r?\n|\r/g, ' ');
        initialString = twemoji.parse(eval("'" + initialString + "'"), { size: "16x16" });

        $('#<%= lblSubject.ClientID %>').html(initialString);

    }
</script>

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
                            <td style="text-align: left;" class="formLabel">
                                <b>Estimated Sends:</b>&nbsp;&nbsp;
                                <asp:Label ID="lblEstimatedSends" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSelected" runat="server" Text="Selected Groups" Font-Size="Large"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr style="padding-left: 30px">
                            <td class="formLabel">

                                <asp:Repeater ID="rpterGroupDetails" runat="server">
                                    <HeaderTemplate>
                                        <table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <b>Group Name:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "GroupName")%>
                                            </td>

                                            <td>&nbsp&nbsp&nbsp  <b>Filter:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "FilterName")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                        <tr style="padding-left: 30px">
                            <td class="formLabel">
                                <asp:Label ID="lblSuppresed" runat="server" Text="Suppressed Groups" Font-Size="Large"></asp:Label>
                                <br />
                                <asp:Repeater ID="rpterSuppression" runat="server">
                                    <HeaderTemplate>
                                        <table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <b>Group Name:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "GroupName")%>
                                            </td>
                                            <td>&nbsp&nbsp&nbsp  <b>Filter:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "FilterName")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <asp:Label ID="lblTransactional" Visible="false" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlBlastFields" runat="server">
                                    <asp:Label ID="lblBlastFieldHeader" runat="server" Text="Blast Fields" Font-Size="Large" />
                                    <br />
                                    <br />
                                    <table>
                                        <tr>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastField1" Font-Bold="true" Text="Field1" runat="server" />
                                            </td>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastFieldValue1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastField2" Font-Bold="true" Text="Field2" runat="server" />
                                            </td>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastFieldValue2" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastField3" Font-Bold="true" Text="Field3" runat="server" />
                                            </td>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastFieldValue3" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastField4" Font-Bold="true" Text="Field4" runat="server" />
                                            </td>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastFieldValue4" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastField5" Font-Bold="true" Text="Field5" runat="server" />
                                            </td>
                                            <td class="formLabel">
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
                                            <table id="tblSimple" style="width: 100%;" runat="server">
                                                <tr>
                                                    <td style="vertical-align: top; font-size: 11px;">
                                                        <b>Simple Share:</b>
                                                    </td>
                                                    <td>
                                                        <asp:GridView ID="gvSimpleShare" Width="100%" runat="server" GridLines="None" ShowHeader="false" OnRowDataBound="gvSimpleShare_RowDataBound" AutoGenerateColumns="false">
                                                            <Columns>
                                                                <asp:TemplateField ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
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
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSocialMedia" Font-Size="11px" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
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

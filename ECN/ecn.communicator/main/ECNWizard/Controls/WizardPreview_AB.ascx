<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardPreview_AB.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Controls.WizardPreview_AB" %>

<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />

<script type="text/javascript">
    function pageLoad(sender, args)
    {
        var initialStringA = $('#<%= lblSubjectA.ClientID %>').html();
        initialStringA = initialStringA.replace(/'/g, "\\'");
        initialStringA = initialStringA.replace(/\r?\n|\r/g, ' ');
        initialStringA = twemoji.parse(eval("'" + initialStringA + "'"), { size: "16x16" });

        $('#<%= lblSubjectA.ClientID %>').html(initialStringA);

        var initialStringB = $('#<%= lblSubjectB.ClientID %>').html();
        initialStringB = initialStringB.replace(/'/g, "\\'");
        initialStringB = initialStringB.replace(/\r?\n|\r/g, ' ');
        initialStringB = twemoji.parse(eval("'" + initialStringB + "'"), { size: "16x16" });

        $('#<%= lblSubjectB.ClientID %>').html(initialStringB);



    }
</script>

<div class="section bottomDiv" style="padding-left: 30px; padding-right: 30px">
    <table width="100%">
        <tr valign="top">
            <td width="100%">
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
                        <tr class="formLabel">
                            <td align="left" style="text-align: left; width: 50%;">

                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblHeadingA" runat="server" Font-Size="Large" Text="Message A" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Message A:</b> &nbsp&nbsp
                                                <asp:Label ID="lblMessageA" runat="server" Text=""></asp:Label>
                                            &nbsp&nbsp
                                        </td>
                                    </tr>
                                    <tr class="formLabel">
                                        <td>
                                            <b>Preview A:</b> &nbsp&nbsp     
                                <asp:HyperLink ID="hlPreviewHTML1" runat="server"><img src="/ecn.images/images/icon-preview-HTML.gif" alt="Preview Message as HTML" border="0"></asp:HyperLink>
                                            &nbsp&nbsp
            <asp:HyperLink ID="hlPreviewTEXT1" runat="server"><img src="/ecn.images/images/icon-preview-TEXT.gif" alt="Preview Message as TEXT" border="0"></asp:HyperLink>
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr class="formLabel">
                                        <td>
                                            <b>Subject A:</b>&nbsp;&nbsp;
                                       
                                            <asp:Label ID="lblSubjectA" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formLabel">
                                        <td>
                                            <b>From Email:</b>&nbsp;&nbsp;
                                            <asp:Label ID="lblFromEmailA" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="formLabel">
                                        <td>
                                            <b>Reply To:</b>&nbsp;&nbsp;
                                            <asp:Label ID="lblReplyToA" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="formLabel">
                                        <td>
                                            <b>From Name:</b>&nbsp;&nbsp;
                                            <asp:Label ID="lblFromNameA" runat="server" />
                                        </td>
                                    </tr>

                                </table>


                            </td>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblHeadingB" runat="server" Font-Size="Large" Text="Message B" />
                                        </td>
                                    </tr>
                                    <tr class="formLabel">
                                        <td align="left">
                                            <b>Message B:</b> &nbsp&nbsp
                                <asp:Label ID="lblMessageB" runat="server" Text=""></asp:Label>
                                            &nbsp&nbsp
                                        </td>
                                    </tr>
                                    <tr class="formLabel">
                                        <td>
                                            <b>Preview B:</b> &nbsp&nbsp  
            <asp:HyperLink ID="hlPreviewHTML2" runat="server"><img src="/ecn.images/images/icon-preview-HTML.gif" alt="Preview Message as HTML" border="0"></asp:HyperLink>
                                            &nbsp&nbsp
            <asp:HyperLink ID="hlPreviewTEXT2" runat="server"><img src="/ecn.images/images/icon-preview-TEXT.gif" alt="Preview Message as TEXT" border="0"></asp:HyperLink><br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr class="formLabel">
                                        <td>
                                            <b>Subject B:</b>&nbsp;&nbsp;
                                        
                                            <asp:Label ID="lblSubjectB" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formLabel">
                                        <td>
                                            <b>From Email:</b>&nbsp;&nbsp;
                                            <asp:Label ID="lblFromEmailB" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="formLabel">
                                        <td>
                                            <b>Reply To:</b>&nbsp;&nbsp;
                                            <asp:Label ID="lblReplyToB" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="formLabel">
                                        <td>
                                            <b>From Name:</b>&nbsp;&nbsp;
                                            <asp:Label ID="lblFromNameB" runat="server" />
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:left;" class="formLabel">
                                <b>Estimated Sends:</b>&nbsp;&nbsp;
                                <asp:Label ID="lblEstimatedSends" runat="server" />
                            </td>
                        </tr>

                    </table>

                    <table>

                        <tr>
                            <td align="left">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td class="formLabel">
                                            <asp:Label ID="lblSelected" runat="server" Text="Selected Groups" Font-Size="Large"></asp:Label>
                                            <br />
                                            <br />
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
                                    <tr>
                                        <td class="formLabel">
                                            <asp:Label ID="lblSuppresed" runat="server" Text="Suppressed Groups" Font-Size="Large"></asp:Label>
                                            <br />
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


                                </table>

                            </td>
                            <td>
                                <asp:Panel ID="pnlBlastFields" runat="server">
                                    <asp:Label ID="lblBlastFieldHeader" runat="server" Text="Blast Fields" Font-Size="Large" />
                                    <br />
                                    <br />
                                    <table>
                                        <tr>
                                            <td class="formLabel" >
                                                <asp:Label ID="lblBlastField1" Font-Bold="true" Text="Field1:" runat="server" />
                                            </td>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastFieldValue1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastField2" Text="Field2:" Font-Bold="true" runat="server" />
                                            </td>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastFieldValue2" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastField3" Text="Field3:" Font-Bold="true" runat="server" />
                                            </td>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastFieldValue3" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastField4" Text="Field4:" Font-Bold="true" runat="server" />
                                            </td>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastFieldValue4" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel">
                                                <asp:Label ID="lblBlastField5" Text="Field5:" Font-Bold="true" runat="server" />
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
                                            <table id="tblSimple" runat="server">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSimpleHeader" runat="server" Text="Simple Share:" />
                                                    </td>
                                                    <td>
                                                        <asp:GridView ID="gvSimpleShare" runat="server" OnRowDataBound="gvSimpleShare_RowDataBound" AutoGenerateColumns="false">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSocialMedia" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSocialMediaName" runat="server" />
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
                                                    <td>
                                                        <asp:Label ID="lblSubscriberShare" runat="server" Text="Subscriber Share:" />
                                                    </td>
                                                    <td>
                                                        <asp:GridView ID="gvSubscriberShare" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvSubscriberShare_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSocialMedia" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSocialMediaName" runat="server" />
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

    </table>
</div>

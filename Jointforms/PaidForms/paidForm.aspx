<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="paidForm.aspx.cs" Inherits="PaidPub.paidForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head lang="en" runat="server">
    <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap CSS -->
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Styles/Site.css?dt=07192017" rel="stylesheet" />

    <style>
        .g-recaptcha {
            margin: 15px auto !important;
            width: auto !important;
            height: auto !important;
            text-align: -webkit-center;
            text-align: -moz-center;
            text-align: -o-center;
            text-align: -ms-center;
        }
    </style>

    <title>Paid Form</title>
    <script src='https://www.google.com/recaptcha/api.js' type="text/javascript"></script>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>

    <script>
        /**
       --https://stackoverflow.com/questions/196859/change-text-box-color-using-required-field-validator-no-extender-controls-pleas
        --https://stackoverflow.com/questions/124682/can-you-have-custom-client-side-javascript-validation-for-standard-asp-net-web-f/125158#125158
    
        * Re-assigns the ASP.NET validation JS function to
        * provide a more flexible approach
        */
        function UpgradeASPNETValidation() {
            if (typeof (Page_ClientValidate) != "undefined") {
                AspValidatorUpdateDisplay = ValidatorUpdateDisplay;
                ValidatorUpdateDisplay = NicerValidatorUpdateDisplay;
                AspValidatorValidate = ValidatorValidate;
                ValidatorValidate = NicerValidatorValidate;

                // Remove the error class on each control group before validating
                // Store a reference to the ClientValidate function
                var origValidate = Page_ClientValidate;
                // Override with our custom version
                Page_ClientValidate = function (validationGroup) {
                    // Clear all the validation classes for this validation group
                    for (var i = 0; i < Page_Validators.length; i++) {
                        if ((typeof (Page_Validators[i].validationGroup) == 'undefined' && !validationGroup) ||
                            Page_Validators[i].validationGroup == validationGroup) {
                            $("#" + Page_Validators[i].controltovalidate).parents('.form-group').each(function () {
                                $(this).removeClass('has-error');
                            });
                        }
                    }
                    // Call the original function
                    return origValidate(validationGroup);
                };
            }
        }

        /**
        * This function is called once for each Field Validator, passing in the 
        * Field Validator span, which has helpful properties 'isvalid' (bool) and
        * 'controltovalidate' (string = id of the input field to validate).
        */
        function NicerValidatorUpdateDisplay(val) {
            // Do the default asp.net display of validation errors (remove if you want)
            AspValidatorUpdateDisplay(val);

            // Add our custom display of validation errors
            // IF we should be paying any attention to this validator at all
            if ((typeof (val.enabled) == "undefined" || val.enabled != false) && IsValidationGroupMatch(val, AspValidatorValidating)) {
                if (!val.isvalid) {
                    // Set css class for invalid controls
                    var t = $('#' + val.controltovalidate).parents('.form-group:first');
                    t.addClass('has-error');
                }
                else {
                    // remove css class for invalid controls
                    var t = $('#' + val.controltovalidate).parents('.form-group:first');
                    t.removeClass('has-error');
                }
            }
        }

        function NicerValidatorValidate(val, validationGroup, event) {
            AspValidatorValidating = validationGroup;
            AspValidatorValidate(val, validationGroup, event);
        }

        // Call UpgradeASPNETValidation after the page has loaded so that it 
        // runs after the standard ASP.NET scripts.
        $(function () {
            UpgradeASPNETValidation();
        });
    </script>


</head>
<body>
    <br />
    <div class="container" style="max-width: 800px; padding: 40px 20px;">
        <form class="form-horizontal" role="form" runat="server">
            <ajaxToolkit:ToolkitScriptManager ID="ajaxScriptManager" runat="server" />

            <div id="banner">
                <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
            </div>
            <br />
            <asp:Panel ID="phError" runat="Server" Visible="false" BorderColor="#A30100" BorderStyle="Solid"
                BorderWidth="2px">
                <div style="padding-left: 65px; padding-right: 5px; padding-top: 15px; padding-bottom: 5px; height: 75px; background-image: url('images/errorEx.jpg'); background-repeat: no-repeat">
                    <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red" Font-Size="small" Font-Bold="true"></asp:Label>
                </div>
            </asp:Panel>

            <%--   <div class="form-group">
                <asp:ValidationSummary ID="valSum"
                    DisplayMode="BulletList"
                    EnableClientScript="true"
                    HeaderText="You must enter a value in the following fields:"
                    runat="server" />
            </div>--%>

            <asp:Panel ID="pnlPaidForm" runat="server" Visible="true">
                <asp:UpdatePanel ID="grdUpdatePanel" runat="server">
                    <ContentTemplate>
                        <div id="pnlTESTMODE" class="row text-center" runat="server" visible="false" style="margin-right: 0px; margin-left: 0px;">
                            <h5 style="background-color: #eeeeee; padding-top: 5px; padding-bottom: 5px; color: red;">Payment Processing environment is set to TEST</h5>
                        </div>
                        <div id="pnlCountry" runat="server" visible="false">
                            <div class="form-group">
                                <label class="control-label col-sm-2 lbl">Country</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList ID="drpCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged" CssClass="form-control input-sm"></asp:DropDownList>
                                </div>
                            </div>

                            <div id="pnlPromoCode" runat="server" visible="false">
                                <div class="form-group">
                                    <div class="control-label col-sm-10 text-center small">
                                        <b>To take advantage of a special promotional offer, please enter offer code, and select “Apply"</b>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-2 lbl">Promo Code</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPromoCode" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Button ID="btnApplyPromoCode" runat="server" Text="Apply" OnClick="btnApplyPromoCode_Click" CssClass="btn btn-xs btn-default active" ValidationGroup="PromoCode" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" id="pnlPromoCodeDetails" runat="server" visible="false">
                                <label class="control-label col-sm-3 lbl">Promo Code Applied</label>
                                <div class="control-label col-sm-3">
                                    <asp:Label Text="N/A" ID="lblPromoCode" CssClass="control-label" runat="server" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:Button ID="btnRemovePromoCode" runat="server" Text="Remove" OnClick="btnRemovePromoCode_Click" CssClass="btn btn-xs btn-default active" ValidationGroup="PromoCode" />
                                </div>
                            </div>
                        </div>

                        <div id="pnlGrid" runat="server">
                            <asp:GridView ID="grdMagazines" runat="server" AutoGenerateColumns="false" HorizontalAlign="Center"
                                ShowFooter="true" GridLines="both" CellPadding="5" CssClass="grid" AllowPaging="false" Width="100%" OnRowDataBound="grdMagazines_RowDataBound">
                                <AlternatingRowStyle CssClass="gridAltColor" HorizontalAlign="center" />
                                <HeaderStyle HorizontalAlign="center" />
                                <FooterStyle CssClass="gridfooter" HorizontalAlign="center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Product" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle"
                                        SortExpression="true" Visible="true">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50%" />
                                        <ItemTemplate>
                                            <div class="form-group">
                                                <div class="col-sm-4">
                                                    <asp:Image ID="lblImageID" runat="server" CssClass="img-responsive img-thumbnail" />
                                                </div>
                                                <div class="col-sm-8">
                                                    <asp:Label ID="lblProductName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductName") %>'
                                                        class="control-label" Visible="true" />
                                                    <asp:Label ID="lblProductDesc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductDesc") %>'
                                                        Visible="false" />
                                                </div>
                                            </div>

                                            <asp:Panel ID="pnlTerm" runat="server" Visible="false">
                                                <div class="form-group">
                                                    <div class="col-sm-4"></div>
                                                    <div class="col-sm-2">
                                                        <label class="control-label lbl">Term</label>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:DropDownList ID="drpTerm" runat="server" AutoPostBack="true" CssClass="form-control input-sm" OnSelectedIndexChanged="drpTerm_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </asp:Panel>

                                            <asp:Panel ID="pnlShippingOption" runat="server" Visible="false">
                                                <div class="form-group">
                                                    <div class="col-sm-4">
                                                        <label class="control-label lbl">Shipping Options</label>
                                                    </div>
                                                    <div class="col-sm-5">
                                                        <asp:RadioButtonList ID="rblShipping" runat="server" CssClass="form-control-no-border input-sm" OnSelectedIndexChanged="rblShipping_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="horizontal">
                                                            <asp:ListItem Selected="True" Value="False">Standard</asp:ListItem>
                                                            <asp:ListItem Value="True">Airmail</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </div>
                                            </asp:Panel>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                                        SortExpression="true" Visible="false">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpQuantity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpQuantity_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Purchase" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-VerticalAlign="Middle" SortExpression="true">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'
                                                Visible="false" />
                                            <asp:Label ID="lblPubcode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Pubcode") %>'
                                                Visible="false" />
                                            <asp:Label ID="lblIsSubscription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"IsSubscription") %>'
                                                Visible="false" />
                                            <asp:Label ID="lblImageName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ImageName") %>'
                                                Visible="false" />
                                            <asp:Label ID="lblGroupID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"GroupID") %>'
                                                Visible="false" />
                                            <asp:Label ID="lblIsBundle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"IsBundle") %>'
                                                Visible="false" />
                                            <asp:Label ID="lblCustomerID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CustomerID") %>'
                                                Visible="false" />
                                            <asp:Label ID="lblPrice" runat="server" Text="" Visible="true"></asp:Label>
                                            <asp:Label ID="lblPriceExpressShipping" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:CheckBox ID="chkPrint" runat="server" AutoPostBack="true" Checked="false" OnCheckedChanged="chBoxPrint_CheckedChanged"
                                                Visible="true" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblFtrText" runat="server" Text="Total:" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                                        ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotal" runat="server" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span>$<asp:Label ID="lblTotalAmount" Text="0.00" runat="server" /></span>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <div class="form-group">
                            <label class="control-label col-sm-6 lbl" style="font-size: x-small;">* Indicates a required field.</label>
                        </div>

                        <h5 style="background-color: #eeeeee; padding: 5px 5px 5px 5px;"><b>Subscriber Information</b></h5>
                        <br />
                        <div class="form-group">
                            <label for="name" class="control-label col-sm-3 lbl">* Email</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtemail" runat="server" MaxLength="100" TabIndex="1" CssClass="form-control input-sm" />
                            </div>
                            <label class="control-label col-sm-1">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" EnableClientScript="true"
                                    runat="server" ControlToValidate="txtemail" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid"
                                    EnableClientScript="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ControlToValidate="txtemail"></asp:RegularExpressionValidator></label>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 lbl">* First Name</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtfirstname" CssClass="form-control input-sm" MaxLength="50" runat="server" TabIndex="2" />
                            </div>
                            <label class="control-label col-sm-1">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                    ControlToValidate="txtfirstname"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 lbl">* Last Name</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtlastname" CssClass="form-control input-sm" MaxLength="50" runat="server" TabIndex="3" />
                            </div>
                            <label class="control-label col-sm-1">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                    ControlToValidate="txtlastname"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 lbl">Company</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtcompany" CssClass="form-control input-sm" MaxLength="75" runat="server" TabIndex="4" />
                            </div>
                            <label class="control-label col-sm-1"></label>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 lbl">* Phone Number</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtphone" CssClass="form-control input-sm" MaxLength="20" runat="server" TabIndex="5" />
                            </div>
                            <label class="control-label col-sm-1">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                    ControlToValidate="txtphone"></asp:RequiredFieldValidator></label>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 lbl">Fax Number</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="fax" CssClass="form-control input-sm" MaxLength="20" runat="server" TabIndex="6" />
                            </div>
                        </div>
                        <br />
                        <div class="row" id="bill_ship_pnl" runat="server">
                            <div class="col-sm-6">
                                <h5 class="text-center" style="background-color: #eeeeee; padding: 5px 5px 5px 5px;"><b>Billing Address</b></h5>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 lbl">* Address</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtBillingAddress" runat="server" CssClass="form-control input-sm" MaxLength="100" TabIndex="7" />
                                    </div>
                                    <label class="control-label col-sm-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" EnableClientScript="true"
                                            ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" ControlToValidate="txtBillingAddress"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 lbl">Address 2</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtBillingAddress2" runat="server" CssClass="form-control input-sm" MaxLength="100" TabIndex="8" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 lbl">* City</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtBillingCity" runat="server" CssClass="form-control input-sm" MaxLength="50" TabIndex="9" />
                                    </div>
                                    <label class="control-label col-sm-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="txtBillingCity"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-group" id="pnlState3" runat="server" visible="false">
                                    <label class="control-label col-sm-4 lbl">* State:</label>
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="drpBillingState" runat="server" CssClass="form-control input-sm" TabIndex="10">
                                        </asp:DropDownList>
                                    </div>
                                    <label class="control-label col-sm-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="drpBillingState"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-group" id="pnlState4" runat="server" visible="false">
                                    <label class="control-label col-sm-4 lbl">State</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtBillingState" runat="server" CssClass="form-control input-sm" MaxLength="50" TabIndex="10" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 lbl">* Zip Code</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtBillingZip" runat="server" CssClass="form-control input-sm" MaxLength="10" TabIndex="11" />
                                    </div>
                                    <label class="control-label col-sm-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="txtBillingZip"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-group">
                                    <div class="center-block">
                                        <asp:Button ID="btnCopyAddres" Text="Copy Address" OnClick="btnCopyAddres_Click" CssClass="btn btn-sm btn-default active center-block" runat="server" CausesValidation="false" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6">
                                <h5 class="text-center" style="background-color: #eeeeee; padding: 5px 5px 5px 5px;"><b>Shipping Address</b></h5>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 lbl">* Address</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtShippingAddress" CssClass="form-control input-sm" MaxLength="100" runat="server" TabIndex="12" />
                                    </div>
                                    <label class="control-label col-sm-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="txtShippingAddress"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 lbl">Address 2</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtShippingAddress2" CssClass="form-control input-sm" MaxLength="100" runat="server" TabIndex="13" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 lbl">* City</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtShippingCity" CssClass="form-control input-sm" MaxLength="50" runat="server" TabIndex="14" />
                                    </div>
                                    <label class="control-label col-sm-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="txtShippingCity"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-group" id="pnlState1" runat="server" visible="false">
                                    <label class="control-label col-sm-4 lbl">* State</label>
                                    <div class="col-sm-6">
                                        <asp:DropDownList CssClass="form-control input-sm" ID="drpShippingState" runat="server" TabIndex="15">
                                        </asp:DropDownList>
                                    </div>
                                    <label class="control-label col-sm-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="drpShippingState"></asp:RequiredFieldValidator></label>
                                </div>
                                <div class="form-group" id="pnlState2" runat="server" visible="false">
                                    <label class="control-label col-sm-4 lbl">State</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtShippingState" CssClass="form-control input-sm" MaxLength="50" runat="server" TabIndex="15" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 lbl">* Zip Code</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtShippingZip" runat="server" CssClass="form-control input-sm" MaxLength="10" TabIndex="16" />
                                    </div>
                                    <label class="control-label col-sm-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                            ControlToValidate="txtShippingZip"></asp:RequiredFieldValidator></label>
                                </div>
                            </div>
                        </div>

                        <div id="pnlCreditCardDetails" runat="server">
                            <h5 style="background-color: #eeeeee; padding: 5px 5px 5px 5px;"><b>Credit Card Details</b></h5>
                            <br />
                            <div class="form-group">
                                <label class="control-label col-sm-3 lbl">* Name on the card</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="user_CardHolderName" runat="server" MaxLength="50" TabIndex="17" CssClass="form-control input-sm" />
                                </div>
                                <label class="control-label col-sm-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" EnableClientScript="true"
                                        ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;" ControlToValidate="user_CardHolderName"></asp:RequiredFieldValidator>
                                </label>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 lbl">* Card Type</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList ID="user_CCType" runat="server" TabIndex="18" CssClass="form-control input-sm">
                                        <asp:ListItem Value=""></asp:ListItem>
                                        <asp:ListItem Value="MasterCard"></asp:ListItem>
                                        <asp:ListItem Value="Visa"></asp:ListItem>
                                        <asp:ListItem Value="Amex"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <label class="control-label col-sm-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                        ControlToValidate="user_CCType"></asp:RequiredFieldValidator>
                                </label>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 lbl">* Card Number</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="user_CCNumber" runat="server" MaxLength="20" TabIndex="19" CssClass="form-control input-sm" />
                                </div>
                                <label class="control-label col-sm-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                        ControlToValidate="user_CCNumber"></asp:RequiredFieldValidator>
                                </label>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 lbl">* Expiration Date</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList ID="user_Exp_Month" runat="server" TabIndex="20" CssClass="form-control input-sm">
                                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="01">Jan</asp:ListItem>
                                        <asp:ListItem Value="02">Feb</asp:ListItem>
                                        <asp:ListItem Value="03">Mar</asp:ListItem>
                                        <asp:ListItem Value="04">Apr</asp:ListItem>
                                        <asp:ListItem Value="05">May</asp:ListItem>
                                        <asp:ListItem Value="06">Jun</asp:ListItem>
                                        <asp:ListItem Value="07">Jul</asp:ListItem>
                                        <asp:ListItem Value="08">Aug</asp:ListItem>
                                        <asp:ListItem Value="09">Sep</asp:ListItem>
                                        <asp:ListItem Value="10">Oct</asp:ListItem>
                                        <asp:ListItem Value="11">Nov</asp:ListItem>
                                        <asp:ListItem Value="12">Dec</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <label class="control-label col-sm-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                        ControlToValidate="user_Exp_Month" Display="Dynamic"></asp:RequiredFieldValidator>
                                </label>
                                <div class="col-sm-2">
                                    <asp:DropDownList
                                        ID="user_Exp_Year" runat="server" TabIndex="21" CssClass="form-control input-sm">
                                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="2017">2017</asp:ListItem>
                                        <asp:ListItem Value="2018">2018</asp:ListItem>
                                        <asp:ListItem Value="2019">2019</asp:ListItem>
                                        <asp:ListItem Value="2020">2020</asp:ListItem>
                                        <asp:ListItem Value="2021">2021</asp:ListItem>
                                        <asp:ListItem Value="2022">2022</asp:ListItem>
                                        <asp:ListItem Value="2023">2023</asp:ListItem>
                                        <asp:ListItem Value="2024">2024</asp:ListItem>
                                        <asp:ListItem Value="2025">2025</asp:ListItem>
                                        <asp:ListItem Value="2026">2026</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <label class="control-label col-sm-1">

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                        ControlToValidate="user_Exp_Year"></asp:RequiredFieldValidator>
                                </label>


                            </div>
                            <div class="form-group">
                                <label for="pwd" class="control-label col-sm-3 lbl">* CVV Code</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="user_CCVerfication" runat="server" TabIndex="22" MaxLength="5" CssClass="form-control" />
                                </div>
                                <label class="control-label col-sm-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="&lt;img src=&quot;images/required_field.jpg&quot;&gt;"
                                        ControlToValidate="user_CCVerfication"></asp:RequiredFieldValidator>
                                </label>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="row">
                    <div class="col-md-6 center-block">
                        <div class="g-recaptcha" data-sitekey="6LdH3AoUAAAAAL-QRSj-PeLIYrbckR_wTd48ub6l"></div>
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col-md-6 center-block">
                        <asp:Button ID="btnSecurePayment" Text="Submit" OnClick="btnSecurePayment_Click" runat="server" CssClass="btn btn-default active center-block" CausesValidation="true" UseSubmitBehavior="false" TabIndex="22" />
                    </div>
                </div>

            </asp:Panel>
            <br />
            <div id="footer">
                <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
            </div>
            <asp:Button ID="btnDupTrans" Style="display: none" runat="server" />
            <ajaxToolkit:ModalPopupExtender ID="mpeDuplicateTrans" TargetControlID="btnDupTrans" PopupControlID="pnlDupeTrans" BackgroundCssClass="modalBackground" CancelControlID="btnCancelTransCache" runat="server">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlDupeTrans" CssClass="popupbody" runat="server">
                <div style="width: 200px; height: 200px; padding: 10px;">
                    <table style="width: 100%; height: 100%;">
                        <tr>
                            <td colspan="2">A payment has already been processed. If you would like to continue with another payment, click Continue and then click the Submit button again.
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%; text-align: center;">
                                <asp:Button ID="btnClearTransCache" Style="padding: 3px; color: #111; font: 12px arial; background-color: #ddd; border: 1px solid; border-color: #aaa; border-radius: 2px;" Text="Continue" runat="server" OnClick="btnClearTransCache_Click" />
                            </td>
                            <td style="width: 50%; text-align: center;">
                                <asp:Button ID="btnCancelTransCache" Style="padding: 3px; color: #111; font: 12px arial; background-color: #ddd; border: 1px solid; border-color: #aaa; border-radius: 2px;" Text="Cancel" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>

            <div id="modalBackgroundDiv" style="display: none; width: 100%; height: 100%; background-color: gray; z-index: 10000; position: fixed; left: 0px; top: 0px; opacity: 0.7;">
            </div>
            <div id="loadingModalBackgroundDiv" style="display: none; width: 100%; height: 100%; background-color: gray; z-index: 10019; position: fixed; left: 0px; top: 0px; opacity: 0.7;">
            </div>
            <div id='loadingPanel' style="display: none; z-index: 10020; position: fixed; left: 50%; top: 50%; display: none; text-align: center; background-color: white; border-radius: 5px; margin: -100px 0 0 -100px;">
                <img src="Images/LoadingIcon.gif" alt="" />
                <br />
                <asp:Label ID="lblProcessingPayment" Style="text-align: center; font-family: Arial; font-size: 12px; color: #8C8989; padding: 5px 5px 5px 5px;" runat="server" Text="Please wait, your payment is processing" />
                <br />
            </div>
            <%--<div id="loadingPanel" style="display: none; z-index: 10020; position: fixed; width: 200px; height: 200px; background-color: white; left: 50%; top: 50%; border-radius: 5px; margin: -100px 0 0 -100px;">
                <div class='uil-default-css' style='margin-left: 25px; margin-top: 20px;'>
                    <img src="Images/LoadingIcon.gif" alt="" />
                </div>
                <asp:Label ID="lblProcessingPayment" Style="text-align: center; font-family: Arial; font-size: 12px; color: #8C8989;padding: 5px 5px 5px 5px;" runat="server" Text="Please wait, your payment is processing" />
            </div>
            --%>
            <div id="loadingPanelIE" style="display: none; z-index: 10020; position: fixed; width: 200px; height: 100px; background-color: white; left: 50%; top: 50%; border-radius: 5px; margin: -100px 0 0 -100px;">


                <div class='uil-default-css' style='margin-left: 25px; margin-top: 20px;'>
                    <asp:Label ID="lblProcessingPaymentIE" Style="text-align: center; font-family: Arial; font-size: 12px; color: #8C8989;" runat="server" Text="Please wait, your payment is processing" />
                </div>


            </div>
        </form>
    </div>
    <br />
</body>


<script language="javascript" type="text/javascript">
    var canSubmit = false;
    // OnClientClick="doubleSubmitCheck()" for btnSecurePayment control.
    $(document).ready(function () {
        $("#btnSecurePayment").click(function (event) {
            if (!canSubmit) {
                if (event.preventDefault) {
                    event.preventDefault();
                    event.stopPropagation();
                } else { event.returnValue = false };

                if (doubleSubmitCheck() == true) {
                    canSubmit = true;
                    event.returnValue = false;
                    setTimeout(function(){$(this).trigger('click')}, 50);
                    return false;
                }
            }
            else {

            }

        });
    });
    function doubleSubmitCheck() {

        var validation = Page_ClientValidate();

        if (validation) {
            $("#loadingModalBackgroundDiv").show();

            if (isBrowserIE()) {
                $("#loadingPanelIE").show();
            }
            else {
                $("#loadingPanel").show();
            }
            return true;
            <%--document.getElementById("<%= btnSecurePayment.ClientID %>").style.Visibility = 'hidden';
            document.getElementById("<%= btnSecurePayment.ClientID %>").style.display = 'none';
            return true;--%>
        }
        else {
            return false;
        }
    }

    function isBrowserIE() {

        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))  // If Internet Explorer, return version number
        {
            return true;
        }
        else  // If another browser, return 0
        {
            return false;
        }

        return false;
    }



    function showLoading() {
        $("#loadingModalBackgroundDiv").show();
        $("#loadingPanel").show();
    }
</script>
</html>

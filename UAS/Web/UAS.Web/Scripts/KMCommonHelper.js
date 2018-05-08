var targetRegionUrl = "/uas.web/CommonMethods/GetRegions";
var targetCountryUrl = "/uas.web/CommonMethods/GetCountries";
var targetProductsUrl = "/uas.web/CommonMethods/GetCircProducts";
var targetProductsNoDefaultUrl = "/uas.web/CommonMethods/GetCircProductsNoDefault";
var targetAddressTypeUrl = "/uas.web/CommonMethods/GetAddressTypes";
var targetDatabaseFileTypesUrl = "/uas.web/CommonMethods/GetDatabaseFileTypes";
var targetDatabaseFileTypesNoDefaultUrl = "/uas.web/CommonMethods/GetDatabaseFileTypesNoDefault";
var targetCircServiceFeaturesUrl = "/uas.web/CommonMethods/GetCircServiceFeatures";
var targetUadServiceFeaturesUrl = "/uas.web/CommonMethods/GetUadServiceFeatures";
var targetUadServiceFeaturesNoDefaultUrl = "/uas.web/CommonMethods/GetUadServiceFeaturesNoDefault";

var targetQDateFormatUrl = "/uas.web/CommonMethods/GetQDateFormats";
var targetSourceFileDelimiterUrl = "/uas.web/CommonMethods/GetSourceFileDelimiters";
var targetYesNoUrl = "/uas.web/CommonMethods/GetYesOrNo";

var targetTransformationTypesUrl = "/uas.web/CommonMethods/GetTransformationTypes";
var targetGetAllProductsForTransformationUrl = "/uas.web/CommonMethods/GetAllProductsForTransformation";
var targetTransformationDelimitersUrl = "/uas.web/CommonMethods/GetTransformationDelimiters";

var commonControlBinder;
var commonAjaxPost;
function CommonControlBinder(option) {

    this.ControlID = option.ControlID;
    this.URL = option.URL;
    this.TextField = option.TextField;
    this.ValueField = option.ValueField;
    this.OptionLabel = option.OptionLabel;
    this.DataType = option.DataType;
    this.SelectedValue = option.SelectedValue;    

    this.BindKDDL = function () {
        $("#" + this.ControlID).kendoDropDownList({
            dataTextField: this.TextField,
            dataValueField: this.ValueField,
            optionLabel: this.OptionLabel,
            value: this.SelectedValue,
            dataSource: {
                transport: {
                    read: {
                        dataType: this.DataType,
                        url: this.URL
                    }
                }
            }
        });
    };
    this.BindKDDLWithSort = function () {
        $("#" + this.ControlID).kendoDropDownList({
            dataTextField: this.TextField,
            dataValueField: this.ValueField,
            optionLabel: this.OptionLabel,
            value: this.SelectedValue,
            dataSource: {
                transport: {
                    read: {
                        dataType: this.DataType,
                        url: this.URL
                    }
                },
                sort: { field: this.TextField, dir: 'asc' }
            }
        });
    };
}

function BindDropDownList(option){
    commonControlBinder = new CommonControlBinder(option);
    if(option.sort){
        commonControlBinder.BindKDDLWithSort();
    } else {
        commonControlBinder.BindKDDL();
    }
    
}

function AjaxPostMethod(option) {
    $.ajax({
            type: option.Type,
            url:  option.URL,
            data: JSON.stringify({ model: option.PostModel }),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response) {
                    $("#" + this.FailureControlID).hide();
                    $("#" + option.TargetControlId).html(response);
                    $("#" + option.TargetControlId).show();
                }
                else //error
                {
                    $("#" + option.FailureControlID).text(response);
                    $("#" + option.TargetControlId).hide();
                    $("#" + option.FailureControlID).show();
                }

            },
            error: function (response) {
                
                    $("#" + this.FailureControlID).text(response);
                    $("#" + this.TargetControlId).hide();
                    $("#" + this.FailureControlID).show();
               

            }
     });

}

function AjaxGetMethod(option) {
    $.ajax
        ({
            url: option.URL,
            contentType: "application/html; charset=utf-8",
            type: option.Type,
            cache: false,
            datatype: option.dataType,
            success: function (response) {
                if (response) {
                    $("#" + option.FailureControlID).hide();
                    $("#" + option.TargetControlId).html(response);
                    $("#" + option.TargetControlId).show();
                }
                else //error
                {
                    $("#" + option.FailureControlID).text(response);
                    $("#" + option.TargetControlId).hide();
                    $("#" + option.FailureControlID).show();
                }
            },
            error: function () {
                $("#" + option.FailureControlID).text(response);
                $("#" + option.TargetControlId).hide();
                $("#" + option.FailureControlID).show();
            }
        })


}


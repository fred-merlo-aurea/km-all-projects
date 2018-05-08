var targetYesNoUrl = "/uad.web.admin/CommonMethods/GetYesOrNo";
var targetProductUrl = "/uad.web.admin/CommonMethods/GetProducts";
var targetSearchCriteriaUrl = "/uad.web.admin/CommonMethods/GetSearchCriteria";
var targetResponseGroupsUrl = "/uad.web.admin/CommonMethods/GetResponseGroups";
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
        $("#"+this.ControlID).kendoDropDownList({
            dataTextField: this.TextField,
            dataValueField: this.ValueField,
            optionLabel: this.OptionLabel,
            value:this.SelectedValue,
            dataSource: {
                transport: {
                    read: {
                        dataType: this.DataType,
                        url: this.URL
                    }
                }
            }
        });

    }
}

function BindDropDownList(option){
    commonControlBinder = new CommonControlBinder(option);
    commonControlBinder.BindKDDL();
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
            cache: !0,
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


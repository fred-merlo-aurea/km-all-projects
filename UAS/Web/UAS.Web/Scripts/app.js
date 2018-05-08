

(function () {
    //'use strict';
    
       
    
    var myapp = angular.module("app", ["kendo.directives", "ngMessages"]);
    var datefilter = 'ctime';
    var directiveID = 'rqrd'; 
    var serviceId = 'dataService';
    var AddEditSubCtrl = 'SubscriberCtrl';

    myapp.run(['$anchorScroll', function ($anchorScroll) {
        $anchorScroll.yOffset = 50;   // always scroll by 50 extra pixels
        

    }]);
    
    myapp.config(function ($httpProvider) {
        $httpProvider.interceptors.push(function ($rootScope) {
            var activeRequests = 0;
              
            return {
                request: function (config) {
                    $rootScope.pending = true;
                    if ($rootScope.pending) {
                        $rootScope.LoadingWindow.show();
                    }
                    activeRequests++;
                    return config;
                },
                response: function (response) {
                    activeRequests--;
                      
                    if(activeRequests === 0) {
                        $rootScope.pending = false;
                        
                    }
                    if ($rootScope.pending) {
                         $rootScope.LoadingWindow.close();
                    }
                    return response;
                }
            }
        });
    })

   

    myapp.filter(datefilter, datetimeFilter);

    myapp.directive(directiveID, requiredValues);

    myapp.directive('responseRepeatDirective', NgRepeatLoading);
    myapp.directive('responseMainDirective', NgRepeatLoadingManager);

    myapp.factory(serviceId, ['$http', '$window', dataService]);

    myapp.controller(AddEditSubCtrl, ['$scope', '$http', '$location', '$window', '$anchorScroll', '$timeout', 'dataService','$q','$rootScope', SubscriberCtrl]);
   
    function NgRepeatLoading() {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                if (scope.$last) {
                    scope.$emit('LastElem');
                }
              }
        }
    }
    function NgRepeatLoadingManager() {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
               
                scope.$on('LastElem', function (event) {
                    scope.CloseLoadingWindow();
                });
            }
        }
    }
    

    function requiredValues(){
        return {
                restrict: 'A',
                require: 'ngModel',
                link: function (scope, element, attr, ngModel) {
                    ngModel.$validators.rqrd = function (modelValue, viewValue) {
                    var value = modelValue || viewValue;
                    return (value != null && value != undefined && value != '' && value>0) ||value=='A'||value=='B'||value=='C' ;
                }
        }
        }
    }

    function datetimeFilter($filter){
         return function (jsonDate) {
             if (jsonDate) {
                 var date = new Date(parseInt(jsonDate.substr(6)));
                 var filterDate = $filter('date')(date, "medium");
                 return filterDate;
             } else {
                 return '';
             }
           
        };
        }

    function dataService($http, $window) {
        delete $http.defaults.headers.common['X-Requested-With'];
        var config = {
                headers: {
                'Content-Type': 'application/json; charset=utf-8'
        }
        };
        var service = {
                loadData: LoadSubscriber,
                searchMatch: MatchSubscriber,
                reLoadData: ReloadSubscriber,
                searchMathces: SearchMatches,
                saveSubscriber: SaveSubscriber,
                showLoadingIcon: LoadWindow,
                hideLoadingIcon: CloseWindow,
                showSuccessMessage: ShowSuccesWindow,
                getLatestUpdatedDateForSubscriber : GetLatestDate
        };
        return service;
        
            function ShowSuccesWindow(options) {
                showMessage(options);
            }

            function LoadWindow() {
                showLoadingWindow();
            }
            function CloseWindow() {

                closeLoadingWindow();
               
                
            }
            function LoadSubscriber() {
               return $http.get($window.baseUrl + '?psid=' + $window.psid + '&pid=' + $window.pid, config);
            }
            function MatchSubscriber(psid, pid, type) {
            return $http.get($window.baseUrl + '?psid=' + psid + '&pid=' + pid + '&type=' + type, config);

        }
            function ReloadSubscriber(psid, pid) {
            return $http.get($window.baseUrl + '?psid=' + psid + '&pid=' + pid, config);

        }
            function SearchMatches(data) {
            return $http({ method: "POST", url: $window.searchMatchUrl, data: { matchsub: data } });

        }
            function SaveSubscriber(model) {
            return $http({ method: "POST", url: $window.saveSubUrl, data: { data: model } });

            }

            function GetLatestDate() {
                return $http.get($window.getlastupdateddatetime + '?psid=' + $window.psid, config);
            }
    }

    function SubscriberCtrl($scope, $http, $location, $window, $anchorScroll, $timeout, dataService,$q ,$rootScope) {
       
        $rootScope.LoadingWindow =  {
             show : function () {
                var options = {  //Options can be customized according to requirement.
                    title: false,
                    modal: true,
                    visible: false,
                    animation: false,
                    width: 200,
                    height: 200
                };
                var container = $("#LoadingWindow");
                var widget = kendo.widgetInstance(container);
                if (widget) {
                    widget.destroy();
                }
                container.kendoWindow(options);
                var window = container.data("kendoWindow");
                window.content("<div class='uil-default-css' style='transform:scale(0.46);'><div style='top:80px;left:93px;width:14px;height:40px;background:#559edb;-webkit-transform:rotate(0deg) translate(0,-60px);transform:rotate(0deg) translate(0,-60px);border-radius:10px;position:absolute;'></div><div style='top:80px;left:93px;width:14px;height:40px;background:#559edb;-webkit-transform:rotate(30deg) translate(0,-60px);transform:rotate(30deg) translate(0,-60px);border-radius:10px;position:absolute;'></div><div style='top:80px;left:93px;width:14px;height:40px;background:#559edb;-webkit-transform:rotate(60deg) translate(0,-60px);transform:rotate(60deg) translate(0,-60px);border-radius:10px;position:absolute;'></div><div style='top:80px;left:93px;width:14px;height:40px;background:#559edb;-webkit-transform:rotate(90deg) translate(0,-60px);transform:rotate(90deg) translate(0,-60px);border-radius:10px;position:absolute;'></div><div style='top:80px;left:93px;width:14px;height:40px;background:#559edb;-webkit-transform:rotate(120deg) translate(0,-60px);transform:rotate(120deg) translate(0,-60px);border-radius:10px;position:absolute;'></div><div style='top:80px;left:93px;width:14px;height:40px;background:#559edb;-webkit-transform:rotate(150deg) translate(0,-60px);transform:rotate(150deg) translate(0,-60px);border-radius:10px;position:absolute;'></div><div style='top:80px;left:93px;width:14px;height:40px;background:#559edb;-webkit-transform:rotate(180deg) translate(0,-60px);transform:rotate(180deg) translate(0,-60px);border-radius:10px;position:absolute;'></div><div style='top:80px;left:93px;width:14px;height:40px;background:#559edb;-webkit-transform:rotate(210deg) translate(0,-60px);transform:rotate(210deg) translate(0,-60px);border-radius:10px;position:absolute;'></div><div style='top:80px;left:93px;width:14px;height:40px;background:#559edb;-webkit-transform:rotate(240deg) translate(0,-60px);transform:rotate(240deg) translate(0,-60px);border-radius:10px;position:absolute;'></div><div style='top:80px;left:93px;width:14px;height:40px;background:#559edb;-webkit-transform:rotate(270deg) translate(0,-60px);transform:rotate(270deg) translate(0,-60px);border-radius:10px;position:absolute;'></div><div style='top:80px;left:93px;width:14px;height:40px;background:#559edb;-webkit-transform:rotate(300deg) translate(0,-60px);transform:rotate(300deg) translate(0,-60px);border-radius:10px;position:absolute;'></div><div style='top:80px;left:93px;width:14px;height:40px;background:#559edb;-webkit-transform:rotate(330deg) translate(0,-60px);transform:rotate(330deg) translate(0,-60px);border-radius:10px;position:absolute;'></div></div><span style='margin-left:30%;text-align:center;font-family:Arial;font-size:12px;color:#8C8989;'>Please wait…</span>");
                window.center().open();

            },
            close : function () {
                setTimeout(function () {
                    var wnd = $("#LoadingWindow").data('kendoWindow');
                    if (wnd) {
                        wnd.close();
                    }
                }, 1000);//It can be adjusted as per your requirements.
            }
        }

        //Initialization of variables Start
        $scope.loadcomplete =false;
        $scope.url = $window.baseUrl;
        $scope.ShowCreditCardDetails = false;
        $scope.ShowCheckDetails = false;
        $scope.ShowCommonDetails = false;
        $scope.firstTimeLoad = true;
        $scope.submitted = false;
        $scope.unSubscribed = false;
        $scope.savefailed = false;
        $scope.reactivealreadyclicked = false;
        $scope.subscrberViewModel = {};
        $scope.subscrberViewModel.ErrorList = {}
        $scope.subscrberViewModel.madeAdHocChange = false;
        $scope.subscrberViewModel.madeResponseChange = false;
        $scope.subscrberViewModel.madePaidChange = false;
        $scope.subscrberViewModel.madePaidBillToChange = false;
        $scope.RequiredResponses = [];
        $scope.CurrentResponseList = [];
        $scope.ProductResponseList = [];
        $scope.transactiontracker = {
                RequalOnlyChange: false,
                RenewalPayment: false,
                CompleteChange: false,
                AddressOnlyChange: false
        };
        $scope.startsWith = function (actual, expected) {
            var lowerStr = (actual + "").toLowerCase().trim();
            return lowerStr.indexOf(expected.toLowerCase().trim()) === 0;
        }
        $scope.LoadMore = function (Question) {
            
            $scope.OpenLoadingWindow();
            Question.ResponseDisplay = Question.ResponseDisplay + 50;
            if (Question.ResponseDisplay >= Question.ResponseCounter) {
                Question.ShowMore = false;
                Question.ResponseDisplay = Question.ResponseCounter;
                $scope.CloseLoadingWindow();
            }
        };
       
        $scope.buttons = ["btnPOKillChecked", "btnPersonKillChecked", "btnOnBehalfKillChecked", "paidPostOffice", "paidExpireCancel", "paidCreditCancel"];
        $scope.OverrideRecordUpdate = false;
        $scope.YesNo = [{ Text: "", Value: "" }, { Text: "Yes", Value: "1" }, { Text: "No", Value: "0" }];
        //End
        $scope.OpenLoadingWindow = function () {
            $rootScope.LoadingWindow.show();
        }
        $scope.CloseLoadingWindow = function () {
               $rootScope.LoadingWindow.close();
        }
        //acroll to  Top if there are any validation error occurs
        $scope.gotoTop = function () {
            var newHash = 'topDiv';
            if ($location.hash() !== newHash) {
                // set the $location.hash to `newHash` and
                // $anchorScroll will automatically scroll to it
                $location.hash(newHash);
            } else {
                // call $anchorScroll() explicitly,
                // since $location.hash hasn't changed
                $anchorScroll();
            }
        };
        //Convert to String to Bool
        $scope.ConvertToBool = function (value) {

            if (value == "1")
                return true;
            else if (value == "0")
                return false;
            else
                return "";
        }
        //Convert To Bool to String 
        $scope.ConvertToString = function (value) {

            if (value == true)
                return "1";
            else if (value == false)
                return "0";
            else
                return "";
        }
        //Handel Error Messages Box
        $scope.showMesssageWindow = function (value, message, id) {
            
            if (value == 'Success_Complete') {
                var options = {
                    type: 'Success',
                    text: message,
                    autoClose: false,
                    action: CloseSubscriber,
                    data: { 'psid': id },
                    IsOpen:false
                }
                dataService.showSuccessMessage(options);
               
            } else if (message!="" && message.length>3) {
                    $("#alertMsg").text(message);
                    $("#alertDiv").show();
            }
            else {
                $("#alertDiv").hide();
            }
            
        }
        //Controls always enabled on page
        $scope.alwaysEnabledCtrls = function () {
                    angular.element("#cancelButton").removeClass("disabledinputs");
                    angular.element("#rstButton").removeClass("disabledinputs");
                    angular.element("#cancelButton1").removeClass("disabledinputs");
                    angular.element("#rstButton1").removeClass("disabledinputs");
                    if ($scope.subscrberViewModel.PubSubscription.IsNewSubscription) {
                        angular.element("#ProductID").data("kendoDropDownList").enable(true);
                        angular.element("#ProductID").removeClass("disabledinputs");
                    }
        }
        $scope.getFilteredRegions = function (countryID) {
            var filteredregions = [];
            if (countryID == 1 || countryID == 2 || countryID == 429) {
                for (var i = 0; i < $scope.subscrberViewModel.entlst.regions.length; i++) {

                    if ($scope.subscrberViewModel.entlst.regions[i].CountryID == countryID) {
                        filteredregions.push({ RegionName: $scope.subscrberViewModel.entlst.regions[i].RegionName, RegionID: $scope.subscrberViewModel.entlst.regions[i].RegionID })
                    }
                }
            }
            else {

                filteredregions.push({ RegionName: 'Foreign State', RegionID: 52 })
            }
            return filteredregions;
        }
         //Load List 
        $scope.loadList = function (isNew) {
            if (isNew) {
                angular.element("#MailPermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    }
                });

                angular.element("#FaxPermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    }
                });

                angular.element("#PhonePermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    }
                });
                angular.element("#OtherProductsPermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    }
                });

                angular.element("#EmailRenewPermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    }
                });

                angular.element("#ThirdPartyPermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    }
                });

                angular.element("#TextPermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    }
                });
                angular.element("#ProductID").kendoDropDownList({
                    dataTextField: "PubCode",
                    dataValueField: "PubID",
                    dataSource: $scope.subscrberViewModel.ProductList,
                    optionLabel: {
                        PubCode: "",
                        PubID: ""
                    },
                    value: $scope.subscrberViewModel.PubSubscription.PubID
                });

                angular.element("#Country").kendoDropDownList({
                    dataTextField: "ShortName",
                    dataValueField: "CountryID",
                    dataSource: $scope.subscrberViewModel.entlst.countryList,
                    optionLabel: {
                        ShortName: "",
                        CountryID: ""
                    }
                });

                angular.element("#State").kendoDropDownList({
                    dataTextField: "RegionName",
                    dataValueField: "RegionID",
                    dataSource: $scope.subscrberViewModel.entlst.regions,
                    optionLabel: {
                        RegionName: "",
                        RegionID: ""
                    },
                    // Task-47706: AMS Web - Adding a new subscriber in AMS Entry and subscriber has a possible name match existing in a UAD product, the State value isn’t pre-populating on entry screen.
                    value: $scope.subscrberViewModel.PubSubscription.RegionID > 0
                        ? $scope.subscrberViewModel.PubSubscription.RegionID
                        : ""
                });

                angular.element("#AddressTypeCodeId").kendoDropDownList({
                    dataTextField: "DisplayName",
                    dataValueField: "CodeId",
                    dataSource: $scope.subscrberViewModel.entlst.addressTypeList,
                    optionLabel: {
                        DisplayName: "",
                        CodeId: ""
                    }
                });

                angular.element("#PubCategoryTypeID").kendoDropDownList({
                    dataTextField: "CategoryCodeTypeName",
                    dataValueField: "CategoryCodeTypeID",
                    dataSource: $scope.subscrberViewModel.entlst.catTypeList,
                    optionLabel: {
                        CategoryCodeTypeName: "",
                        CategoryCodeTypeID: ""
                    }
                });

                angular.element("#PubCategoryCodeID").kendoDropDownList({
                    dataTextField: "CategoryCodeName",
                    dataValueField: "CategoryCodeID",
                    dataSource: $scope.subscrberViewModel.entlst.categoryCodeList,
                    optionLabel: {
                        CategoryCodeName: "",
                        CategoryCodeID: ""
                    }
                });

                angular.element("#subSrcID").kendoDropDownList({
                    dataTextField: "DisplayName",
                    dataValueField: "CodeId",
                    dataSource: $scope.subscrberViewModel.entlst.subscrtypelst,
                    optionLabel: {
                        DisplayName: "",
                        CodeId: ""
                    }
                });

                angular.element("#mediaType").kendoDropDownList({
                    dataTextField: "DisplayName",
                    dataValueField: "CodeValue",
                    dataSource: $scope.subscrberViewModel.entlst.mediaTypeList,
                    optionLabel: {
                        DisplayName: "",
                        CodeValue: ""
                    }
                });

                angular.element("#QSource").kendoDropDownList({
                    dataTextField: "DisplayName",
                    dataValueField: "CodeId",
                    dataSource: $scope.subscrberViewModel.entlst.qSourceList,
                    optionLabel: {
                        DisplayName: "",
                        CodeId: ""
                    }
                });

                angular.element("#Par3C").kendoDropDownList({
                    dataTextField: "DisplayName",
                    dataValueField: "CodeId",
                    dataSource: $scope.subscrberViewModel.entlst.parList,
                    optionLabel: {
                        DisplayName: "",
                        CodeId: ""
                    }
                });

            }
            else {
                angular.element("#MailPermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    },
                    value: $scope.ConvertToString($scope.subscrberViewModel.PubSubscription.MailPermission)
                });

                angular.element("#FaxPermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    },
                    value: $scope.ConvertToString($scope.subscrberViewModel.PubSubscription.FaxPermission)
                });

                angular.element("#PhonePermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    },
                    value: $scope.ConvertToString($scope.subscrberViewModel.PubSubscription.PhonePermission)
                });
                angular.element("#OtherProductsPermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    },
                    value: $scope.ConvertToString($scope.subscrberViewModel.PubSubscription.OtherProductsPermission)
                });

                angular.element("#EmailRenewPermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    },
                    value: $scope.ConvertToString($scope.subscrberViewModel.PubSubscription.EmailRenewPermission)
                });

                angular.element("#ThirdPartyPermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    },
                    value: $scope.ConvertToString($scope.subscrberViewModel.PubSubscription.ThirdPartyPermission)
                });

                angular.element("#TextPermission").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.YesNo,
                    optionLabel: {
                        Text: "",
                        Value: ""
                    },
                    value: $scope.ConvertToString($scope.subscrberViewModel.PubSubscription.TextPermission)
                });
                angular.element("#ProductID").kendoDropDownList({
                    dataTextField: "PubCode",
                    dataValueField: "PubID",
                    dataSource: $scope.subscrberViewModel.ProductList,
                    optionLabel: {
                        PubCode: "",
                        PubID: ""
                    },
                    value: $scope.subscrberViewModel.PubSubscription.PubID
                });

                angular.element("#Country").kendoDropDownList({
                    dataTextField: "ShortName",
                    dataValueField: "CountryID",
                    dataSource: $scope.subscrberViewModel.entlst.countryList,
                    optionLabel: {
                        ShortName: "",
                        CountryID: ""
                    },
                    value: $scope.subscrberViewModel.PubSubscription.CountryID
                });

                angular.element("#State").kendoDropDownList({
                    dataTextField: "RegionName",
                    dataValueField: "RegionID",
                    dataSource: $scope.getFilteredRegions($scope.subscrberViewModel.PubSubscription.CountryID),
                    optionLabel: {
                        RegionName: "",
                        RegionID: ""
                    },
                    value: $scope.subscrberViewModel.PubSubscription.RegionID
                });

                angular.element("#AddressTypeCodeId").kendoDropDownList({
                    dataTextField: "DisplayName",
                    dataValueField: "CodeId",
                    dataSource: $scope.subscrberViewModel.entlst.addressTypeList,
                    optionLabel: {
                        DisplayName: "",
                        CodeId: ""
                    },
                    value: $scope.subscrberViewModel.PubSubscription.AddressTypeCodeId
                });

                angular.element("#PubCategoryTypeID").kendoDropDownList({
                    dataTextField: "CategoryCodeTypeName",
                    dataValueField: "CategoryCodeTypeID",
                    dataSource: $scope.subscrberViewModel.entlst.catTypeList,
                    optionLabel: {
                        CategoryCodeTypeName: "",
                        CategoryCodeTypeID: ""
                    },
                    value: $scope.subscrberViewModel.CategoryCodeTypeID
                });

                angular.element("#PubCategoryCodeID").kendoDropDownList({
                    dataTextField: "CategoryCodeName",
                    dataValueField: "CategoryCodeID",
                    dataSource: $scope.subscrberViewModel.entlst.categoryCodeList,
                    optionLabel: {
                        CategoryCodeName: "",
                        CategoryCodeID: ""
                    },
                    value: $scope.subscrberViewModel.PubSubscription.PubCategoryID
                });

                angular.element("#subSrcID").kendoDropDownList({
                    dataTextField: "DisplayName",
                    dataValueField: "CodeId",
                    dataSource: $scope.subscrberViewModel.entlst.subscrtypelst,
                    optionLabel: {
                        DisplayName: "",
                        CodeId: ""
                    },
                    value: $scope.subscrberViewModel.PubSubscription.SubSrcID
                });

                angular.element("#mediaType").kendoDropDownList({
                    dataTextField: "DisplayName",
                    dataValueField: "CodeValue",
                    dataSource: $scope.subscrberViewModel.entlst.mediaTypeList,
                    optionLabel: {
                        DisplayName: "",
                        CodeValue: ""
                    },
                    value: $scope.subscrberViewModel.PubSubscription.Demo7
                });

                angular.element("#QSource").kendoDropDownList({
                    dataTextField: "DisplayName",
                    dataValueField: "CodeId",
                    dataSource: $scope.subscrberViewModel.entlst.qSourceList,
                    optionLabel: {
                        DisplayName: "",
                        CodeId: ""
                    },
                    value: $scope.subscrberViewModel.PubSubscription.PubQSourceID
                });

                angular.element("#Par3C").kendoDropDownList({
                    dataTextField: "DisplayName",
                    dataValueField: "CodeId",
                    dataSource: $scope.subscrberViewModel.entlst.parList,
                    optionLabel: {
                        DisplayName: "",
                        CodeId: ""
                    },
                    value: $scope.subscrberViewModel.PubSubscription.Par3CID
                });

            }
            
        }
        //Initialises Controls 
        $scope.initialsetup = function () {
            //Set Transaction Text
            angular.element("#TransactionText").text($scope.subscrberViewModel.TransactionName);
           
            var dataNew = new kendo.data.DataSource();
            $scope.gridOptions = {
                    dataSource: dataNew,
                    sortable: true,
                    selectable: true,
                    columns: [
                      { field: "", title: "", width: 100 },
                      { field: "MatchType", title: "Match", width: 100 },
                      { field: "Name", title: "Name", width: 100 },
                      { field: "TITLE", title: "Title", width: 100 },
                      { field: "COMPANY", title: "Comapany", width: 100 },
                      { field: "ADDRESS", title: "Address", width: 180 },
                      { field: "PHONE", title: "Phone", width: 100 },
                      { field: "EMAIL", title: "Email", width: 150 }

            ]
            };

            //Load List
            $scope.loadList($scope.subscrberViewModel.PubSubscription.IsNewSubscription);
            //Show onbelf of box if its having value
            if ($scope.subscrberViewModel.PubSubscription.OnBehalfOf) {
                angular.element("#txtOnBehalf").show();
            }
            else {
                angular.element("#txtOnBehalf").hide();
            }
            if ($scope.subscrberViewModel.PubSubscription.PubCategoryID > 0) {
                $scope.CategoryCodeName = $.grep($scope.subscrberViewModel.entlst.categoryCodeList, function (catcode) {
                    return catcode.CategoryCodeID == $scope.subscrberViewModel.PubSubscription.PubCategoryID;
                })[0].CategoryCodeName;
            }
            else {
                $scope.CategoryCodeName = "";
            }
            if ($scope.subscrberViewModel.PubSubscription.IsPaid) {

                $($("#tabstrip").data("kendoTabStrip").items()[2]).attr("style", "display:visible");
                $($("#tabstrip").data("kendoTabStrip").items()[3]).attr("style", "display:visible");
                $scope.setUpPaid($scope.subscrberViewModel.MySubscriptionPaid.PaymentTypeID);
            }
            else {
                $($("#tabstrip").data("kendoTabStrip").items()[2]).attr("style", "display:none");
                $($("#tabstrip").data("kendoTabStrip").items()[3]).attr("style", "display:none");
            }

            $scope.RequiredResponses = [];
            $scope.CurrentResponseList = [];
            $scope.CurrentDemoDateGroupList = [];

            for (var i = 0 ; i < $scope.subscrberViewModel.QuestionList.length; i++) {

                for (var j = 0; j < $scope.subscrberViewModel.QuestionList[i].SelectedAnswers.length; j++) {

                    $("#Other" + $scope.subscrberViewModel.QuestionList[i].SelectedAnswers[j].ResponseGroupID).show();
                    $("#lblOther" + $scope.subscrberViewModel.QuestionList[i].SelectedAnswers[j].ResponseGroupID).show();
                    
                    $scope.CurrentResponseList.push({
                            CodeSheetID: $scope.subscrberViewModel.QuestionList[i].SelectedAnswers[j].CodeSheetID,
                            PubID: $scope.subscrberViewModel.QuestionList[i].SelectedAnswers[j].PubID,
                            GroupID: $scope.subscrberViewModel.QuestionList[i].SelectedAnswers[j].ResponseGroupID,
                            DateCreated: $scope.subscrberViewModel.QuestionList[i].DateCreated
                    });
                    $scope.CurrentDemoDateGroupList.push($scope.subscrberViewModel.QuestionList[i].SelectedAnswers[j].ResponseGroupID);
            }
                if ($scope.subscrberViewModel.QuestionList[i].IsRequired) {
                    $scope.RequiredResponses.push(
                        {
                                GroupID: $scope.subscrberViewModel.QuestionList[i].GroupID,
                                DisplayName: $scope.subscrberViewModel.QuestionList[i].DisplayName
                    });
            }
        }

            if (!$scope.subscrberViewModel.Enabled) { //
                $scope.DisableEnableSubscriberUI(false);
            }
            else if ($scope.subscrberViewModel.PubSubscription.IsNewSubscription) {
                $scope.DisableEnableSubscriberUI(true);
            }
            else {
                $scope.DisableEnableSubscriberUI(true);
                angular.element("#PubCategoryCodeID").data("kendoDropDownList").enable(false);
                angular.element("#ProductID").data("kendoDropDownList").enable(false);
                angular.element("#Country").data("kendoDropDownList").enable(false);
                angular.element("#PubCategoryTypeID").data("kendoDropDownList").enable(false);
            }
            $scope.alwaysEnabledCtrls();
        }
        //Disable Subscriber UI if Subscriber is Disabled
        $scope.DisableEnableSubscriberUI = function (enabled) {
           
            if (!enabled) {
                angular.element("#mainCtrlDiv :input").addClass("disabledinputs");
                if ($scope.subscrberViewModel.Product.AllowDataEntry == false) {
                    angular.element("#mainCtrlDiv :input").addClass("disabledinputs");
                    $scope.showMesssageWindow("Disabled", "Data entry is currently locked for this product.");
                    angular.element("#reactivateBtn").addClass("disabledinputs");
                   
                }
                else {
                    angular.element("#reactivateBtn").removeClass("disabledinputs");
                    $scope.showMesssageWindow("hide", "");
                }
                angular.element("#reactivateBtn").addClass("btn-orange1");
            }
            else {
               
                angular.element("#mainCtrlDiv :input").removeClass("disabledinputs");
                angular.element("#mainCtrlDiv").find("span").removeClass("disabledinputs");
                angular.element("#mainCtrlDiv").find("button").removeClass("disabledinputs");
                $scope.showMesssageWindow("hide", "");
            }
            if ($scope.subscrberViewModel.PubSubscription.IsInActiveWaveMailing) {
                angular.element("#warningDiv").show();
                angular.element("#warningMsg").text("This subscriber is currently in an active Wave Mailing. Certain field changes will not take effect until the current issue is finalized.");
            }
            else {
                angular.element("#warningDiv").hide();
                angular.element("#warningMsg").text("");
            }
            $scope.toggleControlState(enabled);
            
                
        }
        //Toogle dropdown list state
        $scope.toggleControlState = function (flag) {

            angular.element("#State").data("kendoDropDownList").enable(flag);
            angular.element("#AddressTypeCodeId").data("kendoDropDownList").enable(flag);
            angular.element("#PubCategoryCodeID").data("kendoDropDownList").enable(flag);
            angular.element("#ProductID").data("kendoDropDownList").enable(flag);
            angular.element("#Country").data("kendoDropDownList").enable(flag);
            angular.element("#PubCategoryTypeID").data("kendoDropDownList").enable(flag);
            angular.element("#subSrcID").data("kendoDropDownList").enable(flag);
            angular.element("#mediaType").data("kendoDropDownList").enable(flag);
            angular.element("#MailPermission").data("kendoDropDownList").enable(flag);
            angular.element("#FaxPermission").data("kendoDropDownList").enable(flag);
            angular.element("#PhonePermission").data("kendoDropDownList").enable(flag);
            angular.element("#OtherProductsPermission").data("kendoDropDownList").enable(flag);
            angular.element("#EmailRenewPermission").data("kendoDropDownList").enable(flag);
            angular.element("#ThirdPartyPermission").data("kendoDropDownList").enable(flag);
            angular.element("#TextPermission").data("kendoDropDownList").enable(flag);
            if ($scope.subscrberViewModel.PubSubscription.IsPaid) {
                angular.element("#AddressTypeBillTo").data("kendoDropDownList").enable(flag);
                angular.element("#CountryBillTo").data("kendoDropDownList").enable(flag);
                angular.element("#StateBillTo").data("kendoDropDownList").enable(flag);
            }
           
        }
        //First time Subscriber load
        $scope.firstload = function () {
           
            $scope.loadcomplete = false;
            var promise = dataService.loadData();
            promise.then(function (response) {

                $scope.subscrberViewModel = response.data;
                $scope.initialsetup();
                angular.forEach($scope.subscrberViewModel.ErrorList, function (key, value) {
                    $scope.showMesssageWindow("FailedToLoad", "Failed to retrieve all data.Please contact system administrator.");
                });
                $scope.loadcomplete = true;
                $rootScope.LoadingWindow.close();
            }, function (httpError) {

                $scope.showMesssageWindow("FailedToLoad", "Failed to retrieve data.Please contact system administrator.");
                $rootScope.LoadingWindow.close();

            });
            $scope.firstTimeLoad = false;
        }
       //Reload Subscriber
        $scope.reloadSubscriber = function (psid,pid) {
            //Show loading message while page load.
           
            //dataService.showLoadingIcon();
            $scope.loadcomplete = false;
            dataService.reLoadData(psid, pid).then(function (response) {
               
                $scope.subscrberViewModel = response.data;
                $scope.subscrberViewModel.PubSubscription.PubID = pid;
                $scope.initialsetup();
                $scope.loadcomplete = true;
                $rootScope.LoadingWindow.close();
            }, function (response) {

                $rootScope.LoadingWindow.close();
                $scope.showMesssageWindow("FailedToLoad", "Failed to retrieve data.Please contact system administrator.");

            });

           
        }
        //Start -Do Match for Sequence Number or Profile Attributes like Firstname Lastname and Email
        $scope.SuggestMatches = function () {
                dataService.searchMathces($scope.subscrberViewModel.PubSubscription).then(function (response) {
                    var dataNew = new kendo.data.DataSource({
                            data: response.data
                    });
                    dataNew.fetch(function () {
                        if (dataNew.total() > 0) {
                            var resultGrid = $("#resultGrid").data("kendoGrid");
                            resultGrid.setDataSource(dataNew);
                            $('#myModal').modal('show');
                            $scope.loadcomplete = true;
                            $rootScope.LoadingWindow.close();
                        }
                        else {
                            $scope.loadcomplete = true;
                            $rootScope.LoadingWindow.close();
                        }
                    });
                 }, function (response) {;
                    
                });

        }
        //Copy if Matched profile is found
        $scope.copyMatchedRecordProfile = function (dataItem) {
            //dataService.showLoadingIcon();
            $scope.copyRecordProfile = dataItem;
            var psid = 0;
            if (dataItem.MatchType == "PRODUCT") {
                psid = dataItem.pubsubscriptionID;
            }
            else {
                psid = dataItem.SubscriptionID;
        }

            dataService.searchMatch(psid, $scope.subscrberViewModel.PubSubscription.PubID, dataItem.MatchType)
           .then(function (response) {

               if (response.data == 0) {
                   $scope.showMesssageWindow("NoRecordFound", "No Reocrd found.");
               }
               else {
                   $scope.subscrberViewModel = response.data;
                   $scope.subscrberViewModel.TriggerQualDate = true;
                   $scope.initialsetup();
           }
               $('#myModal').modal('hide');
               $rootScope.LoadingWindow.close();

            }, function (response) {
               $('#myModal').modal('hide');
               $rootScope.LoadingWindow.close();

            });
        }
        //Open matched record if its in same product
        $scope.openMatchedRecord = function () {
            $('#myModal').modal('hide');
        }
        $scope.handleChange = function (dataItem) {

            $scope.SelectedMatchedRecord = dataItem;

        };
        //Reset the current modifiocation to the subscriber record
        $scope.Reset = function () {
            $scope.loadcomplete = false;
            $scope.url = $window.baseUrl;
            $scope.ShowCreditCardDetails = false;
            $scope.ShowCheckDetails = false;
            $scope.ShowCommonDetails = false;
            $scope.firstTimeLoad = true;
            $scope.submitted = false;
            $scope.unSubscribed = false;
            $scope.savefailed = false;
            $scope.reactivealreadyclicked = false;
            $scope.subscrberViewModel = {};
            $scope.subscrberViewModel.ErrorList = {}
            $scope.subscrberViewModel.madeAdHocChange = false;
            $scope.subscrberViewModel.madeResponseChange = false;
            $scope.subscrberViewModel.madePaidChange = false;
            $scope.subscrberViewModel.madePaidBillToChange = false;
            $scope.RequiredResponses = [];
            $scope.CurrentResponseList = [];
            $scope.ProductResponseList = [];
            $scope.transactiontracker = {
                RequalOnlyChange: false,
                RenewalPayment: false,
                CompleteChange: false,
                AddressOnlyChange: false
            };
            $scope.buttons = ["btnPOKillChecked", "btnPersonKillChecked", "btnOnBehalfKillChecked", "paidPostOffice", "paidExpireCancel", "paidCreditCancel"];
            $scope.YesNo = [{ Text: "", Value: "" }, { Text: "Yes", Value: "1" }, { Text: "No", Value: "0" }];
            angular.element("#validations").text("");
            $scope.requiredfields = {};
            $scope.subscrberViewModel.TriggerQualDate = false;
            angular.element("#QualDate").removeClass("highlight");
            angular.element("#mainCtrlDiv :input").removeClass("disabledinputs");
            angular.element("#warningDiv").hide();
            angular.element("#warningMsg").text("");
            $scope.firstload();
        }

        $scope.CheckTrigger = function () {
            if ($scope.subscrberViewModel.TriggerQualDate) {
                angular.element("#QualDate").addClass("highlight");
            }
            else {
                angular.element("#QualDate").removeClass("highlight");
            }
        }
        //Ad Hoc values changed
        $scope.AdHocChanged = function () {
            $scope.subscrberViewModel.madeAdHocChange = true;
        };
        //Property Address Changes
        $scope.AddressChanged = function () {

            $scope.transactiontracker.AddressOnlyChange = true;
            $scope.setSubscribeTransactions();

        }
        //Set Transaction for Response Changes
        $scope.ResponseChanged = function (event, Question, Answer) {
           
            //Check if GroupID is already present in list if not add the GroupID and Description
            if (Question.IsRequired) {
                if (_.findWhere($scope.RequiredResponses, { GroupID: Answer.ResponseGroupID })) {
                    //Do Nothing                       
                }
                else {
                    $scope.RequiredResponses.push({ GroupID: Answer.ResponseGroupID, DisplayName: Question.DisplayName })
                }
            }
            if (event.target.checked) {

                if ($scope.CurrentDemoDateGroupList.indexOf(Question.GroupID) < 0) {
                    $scope.CurrentDemoDateGroupList.push(Question.GroupID);
                }
                if (Answer.IsOther) {
                    Question.ShowOther = true;
                    angular.element("#Other" + Answer.ResponseGroupID).show();

                }
                else {
                    Question.ShowOther = false;
                    Question.OtherValue = "";
                    angular.element("#Other" + Answer.ResponseGroupID).val("");
                    angular.element("#Other" + Answer.ResponseGroupID).hide();
                }
                
                //Check if Multiple answers allowed if Yes then add codesheet, pubid and groupid if not then remove delete object with same groupid
                if (Question.IsMultiple) {
                    
                    //If Multiople allowed add codesheetId in responselist and if already added do nothing
                    if (_.findWhere($scope.CurrentResponseList, { CodeSheetID: Answer.CodeSheetID })) {
                        //Do Nothing since CodesheetID already present in the list

                    }
                    else {
                        Question.DateCreated = null;
                        angular.element("#demodatepicker_" + Question.GroupID).attr("disabled", false);
                        angular.element("#demochecked_" + Question.GroupID).attr("disabled", false);
                        angular.element("#demochecked_" + Question.GroupID).prop("checked", true);
                        $scope.CurrentResponseList.push({ CodeSheetID: Answer.CodeSheetID, PubID: Answer.PubID, GroupID: Answer.ResponseGroupID, DateCreated: Question.DateCreated });
                        
                }

                }
                    //If multiples are not allowed remove the item from list with same groupID
                else {
                    Question.DateCreated = null;
                    angular.element("#demodatepicker_" + Question.GroupID).attr("disabled", false);
                    angular.element("#demochecked_" + Question.GroupID).attr("disabled", false);
                    angular.element("#demochecked_" + Question.GroupID).prop("checked", true);
                    // the name of the box is retrieved using the .attr() method
                    // as it is assumed and expected to be immutable
                    var group = "input:checkbox[name='" + angular.element("#" + event.target.id).attr("name") + "']";
                    // the checked state of the group/box on the other hand will change
                    // and the current value is retrieved using .prop() method
                    $(group).prop("checked", false);
                    angular.element("#"+event.target.id).prop("checked", true);
                    //Remove item with same groupid and
                    for (var i = 0; i < Question.Answers.length; i++) {
                        if (Question.Answers[i].CodeSheetID != Answer.CodeSheetID && Question.Answers[i].IsSelected) {
                            Question.Answers[i].IsSelected = false;
                            angular.element("#responsegroup" + Question.Answers[i].CodeSheetID).prop("checked", false);
                        }
                    }
                     $scope.CurrentResponseList = $scope.CurrentResponseList.filter(function (el) {
                        return el.GroupID !== Answer.ResponseGroupID;
                    });
                    $scope.CurrentResponseList.push({ CodeSheetID: Answer.CodeSheetID, PubID: Answer.PubID, GroupID: Answer.ResponseGroupID, DateCreated: Question.DateCreated });

                }
            }
            else {

                if ($scope.CurrentDemoDateGroupList.indexOf(Question.GroupID) >= 0) {
                    $scope.CurrentDemoDateGroupList.splice($scope.CurrentDemoDateGroupList.indexOf(Question.GroupID),1);
                }
                if (Answer.IsOther) {
                    Question.ShowOther = false;
                    Question.OtherValue = "";
                    angular.element("#Other" + Answer.ResponseGroupID).val("");
                    angular.element("#Other" + Answer.ResponseGroupID).hide();
                }

                if (Question.IsMultiple) {

                        //If Multiople allowed add codesheetId in responselist and if already added remove the code sheet id from list
                        if (_.findWhere($scope.CurrentResponseList, { CodeSheetID: Answer.CodeSheetID })) {
                        $scope.CurrentResponseList = $scope.CurrentResponseList.filter(function (el) {
                            return el.CodeSheetID != Answer.CodeSheetID;
                        });
                        }
                        
                        if (_.findWhere($scope.CurrentResponseList, { GroupID: Question.GroupID })) {
                            Question.DateCreated = null;
                            angular.element("#demodatepicker_" + Question.GroupID).attr("disabled", false);
                            angular.element("#demo_checked" + Question.GroupID).attr("disabled", false);
                            angular.element("#demochecked_" + Question.GroupID).prop("checked", true);
                        }
                        else {
                            Question.DateCreated = null;
                            angular.element("#demodatepicker_" + Question.GroupID).attr("disabled", true);
                            angular.element("#demochecked_" + Question.GroupID).attr("disabled", true);
                            angular.element("#demochecked_" + Question.GroupID).prop("checked", false);
                        }
                      
                }
                //If multiples are not allowed remove the item from list with same groupID
                else {
                    //Disabled the unselected responsegroup  Demo dates
                    Question.DateCreated = null;
                    angular.element("#demodatepicker_" + Question.GroupID).attr("disabled", true);
                    angular.element("#demochecked_" + Question.GroupID).attr("disabled", true);
                    angular.element("#demochecked_" + Question.GroupID).prop("checked", false);
                    angular.element("#" + event.target.id).prop("checked", false);
                    //Remove item with same groupid and
                    $scope.CurrentResponseList = $scope.CurrentResponseList.filter(function (el) {
                        return el.GroupID !== Answer.ResponseGroupID;
                    });
                }

            }
            //Response has been changed
            $scope.subscrberViewModel.madeResponseChange = true;
            $scope.subscrberViewModel.TriggerQualDate = true;
            angular.element("#QualDate").addClass("highlight");
        }
        //IF Other text changed dates checked changed
        $scope.MadeResponseChanged = function () {
            $scope.subscrberViewModel.madeResponseChange = true;
            $scope.subscrberViewModel.TriggerQualDate = true;
            angular.element("#QualDate").addClass("highlight");
        }
        //IF Demo dates checked changed
        $scope.DemoDatesChecked = function (event, Question) {
            $scope.subscrberViewModel.madeResponseChange = true;
            
            var idx = $scope.CurrentDemoDateGroupList.indexOf(Question.GroupID);
           
                if (event.target.checked){
                    if (idx < 0) {
                        $scope.CurrentDemoDateGroupList.push(Question.GroupID);
                    }
                }
                else{
                        $scope.CurrentDemoDateGroupList.splice(idx, 1);
                }
            
           
        }

        //If Demo dates changed
        $scope.DemoDatesChanged = function (Question) {
            angular.element("#demoDatesError" +Question.GroupID).remove();
            var dateDemo = new Date(angular.element("#demodatepicker_" + Question.GroupID).val());
            var qualDate = new Date(angular.element("#QualDate").val());
            //if (dateDemo > qualDate) {
            //    angular.element("#demomessage_" + Question.GroupID).append("<span id='demoDatesError" + Question.GroupID + "'><img src='../Images/Notifications/Error_New.png' style='width:20px;height:20px;'/>Demographic Date cannot be greater than Qualification Date.<span>");
            //    return false;
            //} else {
            //    angular.element("#demoDatesError" + Question.GroupID).remove();
            //    return true;
            //}
            
        }
       
        $scope.ApplyDemoDates = function () {
         
            
            $scope.subscrberViewModel.madeResponseChange = true;
            for (var i = 0; i < $scope.CurrentDemoDateGroupList.length; i++) {
                if ($("#demochecked_" + $scope.CurrentDemoDateGroupList[i]).is(":checked")) {
                    angular.element("#demodatepicker_" + $scope.CurrentDemoDateGroupList[i]).val(angular.element("#QualDate").val());
                    angular.element("#demomessage_" + $scope.CurrentDemoDateGroupList[i]).text("");
                }
                
            }
        }
            
        //Bill To Chnaged
        $scope.PaidBillToChange = function () {
            $scope.subscrberViewModel.madePaidBillToChange = true;
        }

        //Payment Details Changed
        $scope.PaymentDetailsChange = function () {
            $scope.subscrberViewModel.madePaidChange = true;
        }

        //Subscribe or Requalify the subscriber
        $scope.onReactivationClicked = function () {
            if ($scope.reactivealreadyclicked) {
            }
            else {

                $scope.reactivealreadyclicked = true;
                $scope.unSubscribed = false;
                $scope.transactiontracker.RequalOnlyChange = true;
                //Clear on behalf field on reactivation
                $scope.subscrberViewModel.PubSubscription.OnBehalfOf = "";
                angular.element("#txtOnBehalf").val(""); 
                angular.element("#reactivateBtn").removeClass("btn-orange1");
                for (var i = 0; i < $scope.buttons.length; i++) {
                    angular.element("#" + $scope.buttons[i]).addClass("btn-orange1");
                }
                $scope.toggleControlState(true);
                angular.element("#mainCtrlDiv :input").removeClass("disabledinputs");
                angular.element("#cancelButton").removeClass("disabledinputs");
                angular.element("#saveBtn").removeClass("disabledinputs");
                angular.element("#rstButton").removeClass("disabledinputs");
                angular.element("#cancelButton1").removeClass("disabledinputs");
                angular.element("#saveBtn1").removeClass("disabledinputs");
                angular.element("#rstButton1").removeClass("disabledinputs");
                $scope.subscrberViewModel.Enabled = true;
                $scope.subscrberViewModel.TriggerQualDate = true;
                $scope.subscrberViewModel.IsCountryEnabled = true;
                $scope.setSubscribeTransactions();
                $scope.setSubscriptionStatus();
                $scope.CheckTrigger();
        }
        }

        $scope.setSubscribeTransactions = function () {
            var codevalue = 0, typevalue = 0;

            $scope.setPaidOrFreeCategoryType();
            if ($scope.transactiontracker.RenewalPayment && $scope.subscrberViewModel.PubTransaction.IsPaid) {
                codevalue = 40;
                typevalue = 3;
            }
            else if ($scope.transactiontracker.AddressOnlyChange == true && $scope.transactiontracker.RequalOnlyChange == false) {
                if ($scope.subscrberViewModel.PubSubscription.IsPaid == true) {
                    codevalue = 21;
                    typevalue = 3;
                }
                else {
                    codevalue = 21;
                    typevalue = 1;
            }

            }

            else if ($scope.transactiontracker.AddressOnlyChange == true && $scope.transactiontracker.RequalOnlyChange == true) {
                if ($scope.subscrberViewModel.PubSubscription.IsPaid == true) {
                    codevalue = 27;
                    typevalue = 3;
                }
                else {
                    codevalue = 27;
                    typevalue = 1;
            }
            }

            else if ($scope.transactiontracker.RequalOnlyChange == true) {
                if ($scope.subscrberViewModel.PubSubscription.IsPaid == true) {
                    codevalue = 22;
                    typevalue = 3;
                }
                else {
                    codevalue = 22;
                    typevalue = 1;
            }
        }

            var tc = _.first(_.where($scope.subscrberViewModel.entlst.transCodeList, { TransactionCodeValue: codevalue, TransactionCodeTypeID: typevalue }));


            if (tc != null) {
                
                angular.element("#TransactionText").text(tc.TransactionCodeName);
                $scope.subscrberViewModel.TransactionCodeName = tc.TransactionCodeName;
                $scope.subscrberViewModel.PubSubscription.PubTransactionID = tc.TransactionCodeID;
        }


        }

        $scope.setPaidOrFreeCategoryType = function () {

            $scope.transactiontracker.CategoryCodeID = angular.element("#PubCategoryCodeID").val();
            $scope.transactiontracker.CategoryCodeTypeId = angular.element("#PubCategoryTypeID").val();
            var ccType = {};//_.first(_.where($scope.subscrberViewModel.entlst.catTypeList, { CategoryCodeTypeID : $scope.transactiontracker.CategoryCodeTypeId }));

            for (var i = 0; i < $scope.subscrberViewModel.entlst.catTypeList.length ; i++) {
                if ($scope.subscrberViewModel.entlst.catTypeList[i].CategoryCodeTypeID == $scope.transactiontracker.CategoryCodeTypeId) {
                    ccType = $scope.subscrberViewModel.entlst.catTypeList[i];
                    break;
            }

        }
            if ((ccType.CategoryCodeTypeName == "Qualified Free" || ccType.CategoryCodeTypeName == "NonQualified Free")) {
                if ($scope.subscrberViewModel.PubSubscription.IsPaid || $scope.subscrberViewModel.PubSubscription.IsNewSubscription) {
                    var tc = _.first(_.where($scope.subscrberViewModel.entlst.transCodeList, { TransactionCodeValue: 10, TransactionCodeTypeID: 1 }));
                    if (tc) {

                        $scope.subscrberViewModel.PubSubscription.IsPaid = false;
                        $scope.subscrberViewModel.TransactionCodeName = tc.TransactionCodeName;
                        $scope.subscrberViewModel.PubSubscription.PubTransactionID = tc.TransactionCodeID;
                }
            }

            }
            else if ((ccType.CategoryCodeTypeName == "Qualified Paid" || ccType.CategoryCodeTypeName == "NonQualified Paid")) {
                if (!$scope.subscrberViewModel.PubSubscription.IsPaid || $scope.subscrberViewModel.PubSubscription.IsNewSubscription) {
                    var tc = _.first(_.where($scope.subscrberViewModel.entlst.transCodeList, { TransactionCodeValue: 13, TransactionCodeTypeID: 3 }));
                    if (tc) {
                        $scope.subscrberViewModel.PubSubscription.IsPaid = true;
                        $scope.subscrberViewModel.TransactionCodeName = tc.TransactionCodeName;
                        $scope.subscrberViewModel.PubSubscription.PubTransactionID = tc.TransactionCodeID;
                }
            }
        }

            if ($scope.subscrberViewModel.PubSubscription.IsPaid) {

                $scope.setUpPaid($scope.subscrberViewModel.MySubscriptionPaid.PaymentTypeID);
                $("#PaidUnsubscribe").show();
                $("#FreeUnsubscribe").hide();
                $($("#tabstrip").data("kendoTabStrip").items()[2]).attr("style", "display:visible");
                $($("#tabstrip").data("kendoTabStrip").items()[3]).attr("style", "display:visible");
            }
            else {
                $("#PaidUnsubscribe").hide();
                $("#FreeUnsubscribe").show();
                $($("#tabstrip").data("kendoTabStrip").items()[2]).attr("style", "display:none");
                $($("#tabstrip").data("kendoTabStrip").items()[3]).attr("style", "display:none");

        }
            if (ccType.CategoryCodeTypeName == "Qualified Free" || ccType.CategoryCodeTypeName == "Qualified Paid") {
                $scope.subscrberViewModel.AreQuestionsRequired = true;
            }
            else {
                $scope.subscrberViewModel.AreQuestionsRequired = false;
        }
        }

        $scope.setSubscriptionStatus = function () {

            var category = {};
            var transaction = {};
            var catCodeID = angular.element("#PubCategoryCodeID").val();

            for (var i = 0; i < $scope.subscrberViewModel.entlst.categoryCodeList.length ; i++) {
                if ($scope.subscrberViewModel.entlst.categoryCodeList[i].CategoryCodeID == catCodeID) {
                    category = $scope.subscrberViewModel.entlst.categoryCodeList[i];
                    break;
            }

        }
            for (var i = 0; i < $scope.subscrberViewModel.entlst.transCodeList.length ; i++) {
                if ($scope.subscrberViewModel.entlst.transCodeList[i].TransactionCodeID == $scope.subscrberViewModel.PubSubscription.PubTransactionID) {
                    transaction = $scope.subscrberViewModel.entlst.transCodeList[i];
                    break;
            }

        }
            var SubscriptionStatus = "";
            var catValue = category.CategoryCodeValue;
            var catTypeID = category.CategoryCodeTypeID;
            var xactTypeID = transaction.TransactionCodeTypeID;
            if (catValue == 70 || catValue == 71) //Verified Prospect & Unverified Prospect
            {
                if (xactTypeID == 1) //Active
                {
                    SubscriptionStatus = $scope.getSubscriptionStatus("AProsp");
                    $scope.subscrberViewModel.PubSubscription.SubscriptionStatusID = SubscriptionStatus;
                    $scope.subscrberViewModel.PubSubscription.IsActive = true;
                    $scope.subscrberViewModel.PubSubscription.IsSubscribed = true;
                }
                else //InActive
                {

                    SubscriptionStatus = $scope.getSubscriptionStatus("IAProsp");
                    $scope.subscrberViewModel.PubSubscription.SubscriptionStatusID = SubscriptionStatus;
                    $scope.subscrberViewModel.PubSubscription.IsActive = false;
                    $scope.subscrberViewModel.PubSubscription.IsSubscribed = false;
            }
            }
            else if (catTypeID == 1 || catTypeID == 2) //Qualified, Non-Qualified Free
            {
                if (xactTypeID == 1) //Free Active
                {
                    SubscriptionStatus = $scope.getSubscriptionStatus("AFree");
                    $scope.subscrberViewModel.PubSubscription.SubscriptionStatusID = SubscriptionStatus;
                    $scope.subscrberViewModel.PubSubscription.IsActive = true;
                    $scope.subscrberViewModel.PubSubscription.IsSubscribed = true;
                }
                else //Free Inactive
                {
                    SubscriptionStatus = $scope.getSubscriptionStatus("IAFree");
                    $scope.subscrberViewModel.PubSubscription.SubscriptionStatusID = SubscriptionStatus;
                    $scope.subscrberViewModel.PubSubscription.IsActive = false;
                    $scope.subscrberViewModel.PubSubscription.IsSubscribed = false;
            }
            }
            else if (catTypeID == 3 || catTypeID == 4) //Qualified, Non-Qualified Paid
            {
                if (xactTypeID == 3) //Paid Active
                {

                    SubscriptionStatus = $scope.getSubscriptionStatus("APaid");
                    $scope.subscrberViewModel.PubSubscription.SubscriptionStatusID = SubscriptionStatus;
                    $scope.subscrberViewModel.PubSubscription.IsActive = true;
                    $scope.subscrberViewModel.PubSubscription.IsSubscribed = true;
                }
                else //Paid Inactive
                {

                    SubscriptionStatus = $scope.getSubscriptionStatus("IAPaid");
                    $scope.subscrberViewModel.PubSubscription.SubscriptionStatusID = SubscriptionStatus;
                    $scope.subscrberViewModel.PubSubscription.IsActive = false;
                    $scope.subscrberViewModel.PubSubscription.IsSubscribed = false;
            }
        }



        }

        $scope.getSubscriptionStatus = function (value) {

            for (var i = 0; i < $scope.subscrberViewModel.entlst.sstList.length ; i++) {
                if ($scope.subscrberViewModel.entlst.sstList[i].StatusCode == value) {
                    return $scope.subscrberViewModel.entlst.sstList[i].SubscriptionStatusID;

            }

        }


        }

        //Unsubscribe or Inactivate the subscriber
        $scope.unSubscribe = function (event) {
            $scope.reactivealreadyclicked = false;
            $scope.unSubscribed = true;
            $scope.subscrberViewModel.TriggerQualDate = false;
            angular.element("#txtOnBehalf").hide();
            angular.element("#PubCategoryCodeID").data("kendoDropDownList").enable(false);
            angular.element("#PubCategoryTypeID").data("kendoDropDownList").enable(false);
            angular.element("#Country").data("kendoDropDownList").enable(false);


            for (var i = 0; i < $scope.buttons.length; i++) {
                angular.element("#" + $scope.buttons[i]).addClass("btn-orange1");
            }

            angular.element("#" + event.target.id).removeClass("btn-orange1").removeClass("disabledinputs");
            angular.element("#reactivateBtn").addClass("btn-orange1").removeClass("disabledinputs");
            angular.element("#saveBtn").removeClass("disabledinputs");
            angular.element("#saveBtn1").removeClass("disabledinputs");
            var lsittype = "TransCodeList";
            var transcodeval = 0;
            var iskill = false;

            if (event.target.id == "btnPOKillChecked") {

                $scope.subscrberViewModel.btnPOKillChecked = true;
                $scope.subscrberViewModel.btnPersonKillChecked = false;
                $scope.subscrberViewModel.btnOnBehalfKillChecked = false;
                transcodeval = 31;
                iskill = true;
            }
            else if (event.target.id == "btnPersonKillChecked") {

                $scope.subscrberViewModel.btnPOKillChecked = false;
                $scope.subscrberViewModel.btnPersonKillChecked = true;
                $scope.subscrberViewModel.btnOnBehalfKillChecked = false;
                transcodeval = 32;
                iskill = true;
            }
            else if (event.target.id == "btnOnBehalfKillChecked") {

                $scope.subscrberViewModel.btnPOKillChecked = false;
                $scope.subscrberViewModel.btnPersonKillChecked = false;
                $scope.subscrberViewModel.btnOnBehalfKillChecked = true;
                angular.element("#txtOnBehalf").show();
                angular.element("#txtOnBehalf").removeClass("disabledinputs");
                angular.element("#txtOnBehalf").focus();
                transcodeval = 32;
                iskill = true;
                //if (angular.element("#txtOnBehalf").val().trim()=="") {
                //     var options = {
                //    type: 'Confirm',
                //    text: "Please enter On Behalf Request name before saving.",
                //    autoClose: false,
                //    IsOpen:false
                //    }
                //    dataService.showSuccessMessage(options);
                //}
            }
            else if (event.target.id == "paidPostOffice") {

                transcodeval = 61;
                iskill = true;
            }
            else if (event.target.id == "paidExpireCancel") {

                transcodeval = 64;
                iskill = false;
            }
            else if (event.target.id == "paidCreditCancel") {

                transcodeval = 65;
                iskill = false;
        }

            $scope.setUnsubscribeTransactions(transcodeval, iskill);


        }

        $scope.setUnsubscribeTransactions = function (value, truefalse) {

            var obj = _.first(_.where($scope.subscrberViewModel.entlst.transCodeList, { TransactionCodeValue: value, IsKill: truefalse }));
            angular.element("#TransactionText").text(obj.TransactionCodeName);
            $scope.subscrberViewModel.CategoryFreePaidEnabled = false;
            $scope.subscrberViewModel.CategoryCodeEnabled = false;
            $scope.subscrberViewModel.TransactionName = obj.TransactionCodeName;
            $scope.subscrberViewModel.PubSubscription.PubTransactionID = obj.TransactionCodeID;
            $scope.subscrberViewModel.AreQuestionsRequired = false;
            $scope.subscrberViewModel.IsCountryEnabled = false;
            $scope.subscrberViewModel.TriggerQualDate = false;
            $scope.setSubscriptionStatus();



        }

        //Paid Subscriber Setup
        $scope.setUpPaid = function (id) {
            if (id)
                $scope.subscrberViewModel.MySubscriptionPaid.PaymentTypeID = id;
            else
                $scope.subscrberViewModel.MySubscriptionPaid.PaymentTypeID = angular.element("#paymentTypeID").val();

            angular.element("#PhoneCodeBilTo").val(1);

            angular.element("#creditCardType").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.subscrberViewModel.entlst.CreditCardTypeSelectList,
                    optionLabel: {
                            Text: "",
                            Value: ""
            },
                    value: $scope.subscrberViewModel.MySubscriptionPaid.CreditCardTypeID
            });

            angular.element("#creditCardMonth").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.subscrberViewModel.entlst.creditcardMonthSelectList,
                    optionLabel: {
                            Text: "",
                            Value: ""
            },
                    value: parseInt($scope.subscrberViewModel.MySubscriptionPaid.CCExpirationMonth)
            });

            angular.element("#creditCardYear").kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: $scope.subscrberViewModel.entlst.creditcardYearSelectList,
                    optionLabel: {
                            Text: "",
                            Value: ""
            },
                    value: parseInt($scope.subscrberViewModel.MySubscriptionPaid.CCExpirationYear)
            });


            angular.element("#CountryBillTo").kendoDropDownList({
                    dataTextField: "ShortName",
                    dataValueField: "CountryID",
                    dataSource: $scope.subscrberViewModel.entlst.countryList,
                    optionLabel: {
                            ShortName: "",
                            CountryID: ""
            },
                    value: $scope.subscrberViewModel.MyPaidBillTo.CountryID
            });



            angular.element("#StateBillTo").kendoDropDownList({
                    dataTextField: "RegionName",
                    dataValueField: "RegionID",
                    dataSource: $scope.getFilteredRegions($scope.subscrberViewModel.MyPaidBillTo.CountryID),
                    optionLabel: {
                            RegionName: "",
                            RegionID: ""
            },
                    value: $scope.subscrberViewModel.MyPaidBillTo.RegionID
            });

            angular.element("#AddressTypeBillTo").kendoDropDownList({
                dataTextField: "DisplayName",
                dataValueField: "CodeId",
                dataSource: $scope.subscrberViewModel.entlst.addressTypeList,
                optionLabel: {
                    DisplayName: "",
                    CodeId: ""
                },
                value: $scope.subscrberViewModel.MyPaidBillTo.AddressTypeId
            });


            if (id == 10) {
                $scope.ShowCommonDetails = true;
                $scope.ShowCheckDetails = true;
                $scope.ShowCreditCardDetails = false;
            }
            else if (id == 11) {
                $scope.ShowCommonDetails = true;
                $scope.ShowCheckDetails = false;
                $scope.ShowCreditCardDetails = true;
            }
            else {
                $scope.ShowCommonDetails = false;
                $scope.ShowCheckDetails = false;
                $scope.ShowCreditCardDetails = false;
        }

        }


        //Copy Profile Contents
        $scope.CopyProfile = function (){
            if ($scope.subscrberViewModel.CopyProfile) {
                $scope.subscrberViewModel.madePaidBillToChange = true;
                var czr = $scope.GetCountryZipRegion(angular.element("#Country").val(), angular.element("#State").val(), angular.element("#FullZip").val());
                $scope.subscrberViewModel.MyPaidBillTo.PhonePrefix = czr.Country.PhonePrefix,
                $scope.subscrberViewModel.MyPaidBillTo.RegionCode = czr.RegionCode;
                $scope.subscrberViewModel.MyPaidBillTo.Country = czr.Country.ShortName;
                $scope.subscrberViewModel.MyPaidBillTo.ZipCode = czr.ZipCode;
                $scope.subscrberViewModel.MyPaidBillTo.Plus4 = czr.Plus4;
                $scope.subscrberViewModel.MyPaidBillTo.Title = angular.element("#subcriber_txtTitle").val();
                $scope.subscrberViewModel.MyPaidBillTo.FirstName = angular.element("#subcriber_txtFirstName").val();
                $scope.subscrberViewModel.MyPaidBillTo.LastName = angular.element("#subcriber_txtLastName").val();
                $scope.subscrberViewModel.MyPaidBillTo.Address1 = angular.element("#Address1").val();
                $scope.subscrberViewModel.MyPaidBillTo.Address2 = angular.element("#Address2").val();
                $scope.subscrberViewModel.MyPaidBillTo.Address3 = angular.element("#Address3").val();
                $scope.subscrberViewModel.MyPaidBillTo.Mobile = angular.element("#Mobile").val();
                $scope.subscrberViewModel.MyPaidBillTo.City = angular.element("#City").val();
                $scope.subscrberViewModel.MyPaidBillTo.Fax = angular.element("#Fax").val();
                $scope.subscrberViewModel.MyPaidBillTo.Company = angular.element("#Company").val();
                $scope.subscrberViewModel.MyPaidBillTo.Email = angular.element("#Email").val();
                $scope.subscrberViewModel.MyPaidBillTo.Website = angular.element("#Website").val();
                $scope.subscrberViewModel.MyPaidBillTo.Phone = angular.element("#Phone").val();
                $scope.subscrberViewModel.MyPaidBillTo.PhoneExt = angular.element("#PhoneExt").val();
                $scope.subscrberViewModel.MyPaidBillTo.County = angular.element("#County").val();
                $scope.subscrberViewModel.MyPaidBillTo.AddressTypeId = $scope.subscrberViewModel.PubSubscription.AddressTypeCodeId
                $scope.subscrberViewModel.MyPaidBillTo.RegionID = $scope.subscrberViewModel.PubSubscription.RegionID;
                $scope.subscrberViewModel.MyPaidBillTo.CountryID = $scope.subscrberViewModel.PubSubscription.CountryID;
                var countryDropDownlist = $("#CountryBillTo").data("kendoDropDownList");
                // selects item if its text is equal to "test" using predicate function
                countryDropDownlist.select(function (dataItem) {
                    return dataItem.CountryID === $scope.subscrberViewModel.PubSubscription.CountryID;
                });
                var stateDropDownlist = $("#StateBillTo").data("kendoDropDownList");
                stateDropDownlist.select(function (dataItem) {
                    return dataItem.RegionID === $scope.subscrberViewModel.PubSubscription.RegionID;
                });
                var addressTypeDropDownlist = $("#AddressTypeBillTo").data("kendoDropDownList");
                addressTypeDropDownlist.select(function (dataItem) {
                    return dataItem.CodeId === $scope.subscrberViewModel.PubSubscription.AddressTypeCodeId
                });
                
               
            }
            
        };

        

        //Retrive all required questions
        $scope.RequireFields = function () {
            var obj = {}
            var requiredList = [];
            var required = "";
            if ($scope.subscrberViewModel.TriggerQualDate == true) {
                required += "Qualification Date, ";
                requiredList.push({ tabname: "Qualification", fieldMissing: "Please select or check Qualification Date" });
                angular.element("#QualDate").addClass("highlight");

            }
            if (angular.element("#PubCategoryTypeID").val() > 0) {

            }
            else {
                required += "Free/Paid, ";
                requiredList.push({ tabname: "Status", fieldMissing: "Please select Free/Paid categories" });
            }
            if ($scope.subscrberViewModel.PubSubscription.IsSubscribed == true && $scope.subscrberViewModel.PubSubscription.PubCategoryID == 0) {
                required += "Category Type, ";
                requiredList.push({ tabname: "Status", fieldMissing: "Please select Category Type" });
            }
            else {
                $scope.subscrberViewModel.PubSubscription.PubCategoryID = angular.element("#PubCategoryCodeID").val();
            }
            if (angular.element("#mediaType").val()) {

            }
            else {
                required += "Media Type, ";
                requiredList.push({ tabname: "Marketing", fieldMissing: "Please select Media Type" });
            }
            if ($scope.CheckRequired()) {
                if (angular.element("#Par3C").val() != "0" && parseInt(angular.element("#Par3C").val()) > 0) {

                }
                else {
                    required += "Par3C, ";
                    requiredList.push({ tabname: "Qualification", fieldMissing: "Please select Par3C" });
                }
                if (angular.element("#QSource").val() != "0" && parseInt(angular.element("#QSource").val()) > 0) {

                }
                else {
                    required += "Qualification Source, ";
                    requiredList.push({ tabname: "Qualification", fieldMissing: "Please select Qualification Source" });
                }
            }
            
            
            var requQuestions = "";
            if ($scope.subscrberViewModel.AreQuestionsRequired) {
                for (var i = 0; i < $scope.RequiredResponses.length; i++) {
                    if (_.findWhere($scope.CurrentResponseList, { GroupID: $scope.RequiredResponses[i].GroupID })) {
                        //Do Nothing since CodesheetID already present in the list
                        
                    }
                    else {
                        requQuestions += $scope.RequiredResponses[i].DisplayName + ", ";

                }
            }
        }
                    

            if (requQuestions.length>0){
                required = required  + requQuestions;
                requiredList.push({ tabname: "Qualification", fieldMissing: "Please select reponses for following -"+requQuestions });
            }
            else
                required = required + '.'
            obj.required = required;
            obj.requiredList = requiredList;
            return obj;
        }
       
        $scope.OverrideSuscriber = function () {
            $scope.OverrideRecordUpdate = true;
            $scope.SaveSubscriber();
        }

        //Save the subscriber
        $scope.SaveSubscriber = function () {
            $scope.requiredfields = { required: "", requiredList: [{}] };
            $scope.submitted = true;
            var message = "Please update or provide answers/selections for the following Tabs - Fields: "
            //Validations 
            $scope.requiredfields = $scope.RequireFields();
            //Validate Demo dates
            //var validDemodates = true;
            //if ($scope.CurrentResponseList.length > 0 && $scope.subscrberViewModel.madeResponseChange) {
            //    for (var i = 0; i < $scope.CurrentResponseList.length; i++) {
            //        if (!$scope.DemoDatesChanged($scope.CurrentResponseList[i])) {
            //            validDemodates = false;
            //            break;
            //        }
            //    }
            //}
            //Set Statuses
            $scope.setSubscriptionStatus();

            if ($scope.requiredfields.required.length > 1 && !$scope.unSubscribed) {
                angular.element("#validationErrorDiv").show();
                $scope.gotoTop();
                $scope.savefailed = true;
                $scope.submitted = false;
            }
            // else if (!validDemodates) {
            // angular.element("#alertMsg").text("Please check demodates for qualification.");
            // angular.element("#alertDiv").show();
            // $scope.gotoTop();
            //}
            else {

                if ($scope.subscrberViewModel.PubSubscription.IsNewSubscription == false && $scope.subscrberViewModel.btnOnBehalfKillChecked == true && $scope.subscrberViewModel.PubSubscription.OnBehalfOf == "") {
                    $scope.requiredfields.required += "On Behalf Request, ";
                    $scope.requiredfields.requiredList.push({ tabname: "Status", fieldMissing: "Please provide input for On Behalf Request" });
                    angular.element("#validations").text(message);
                    $scope.gotoTop();
                    $scope.savefailed = true;
                    $scope.submitted = false;
                }
                else {

                    if ($scope.subscrberViewModel.PubSubscription.IsNewSubscription) {
                        $scope.Save();
                    }
                    else {
                        dataService.getLatestUpdatedDateForSubscriber().then(function (response) {
                            var currentmodifieddate = new Date(parseInt($scope.subscrberViewModel.PubSubscription.DateCreated.substr(6)));
                            var lastmodifieddate = new Date(parseInt($scope.subscrberViewModel.PubSubscription.DateCreated.substr(6)));
                            if (response.data.length > 6) {
                                lastmodifieddate = new Date(parseInt(response.data.substr(6)));
                                if ($scope.subscrberViewModel.PubSubscription.DateUpdated) {
                                    currentmodifieddate = new Date(parseInt($scope.subscrberViewModel.PubSubscription.DateUpdated.substr(6)));
                                }
                            }
                            if (lastmodifieddate.getTime() == currentmodifieddate.getTime() || $scope.OverrideRecordUpdate) {
                                $scope.Save();
                            }
                            else {
                                dataService.showSuccessMessage({
                                    type: 'Confirm',
                                    text: 'This record has been updated <span style="color:red;">by a different user</span>. <br><b>OK</b> will ovewrite with your changes.<br><b>Cancel</b> will refresh with latest data and updates will need to be made.',
                                    autoClose: false,
                                    IsOpen: false,
                                    cancelaction: angular.element(document.getElementById('mainCtrlDiv')).scope().Reset,
                                    action: angular.element(document.getElementById('mainCtrlDiv')).scope().OverrideSuscriber
                                });
                            }
                        },
                        function (response) {
                            console.log(response);
                        });
                    }


                }

            }
           
        }
        $scope.GetCountryZipRegion = function (countryID, regionID, zipcode) {
            var country = {};
            var zip = "";
            var plus4 = "";
            var state = "";
            if (countryID==0){
                countryID = 1;
            }
            country = _.first(_.where($scope.subscrberViewModel.entlst.countryList, { CountryID: parseInt(countryID) }));
            
            if (zipcode) {
                if (countryID == 1) {
                    zip = zipcode.substring(0, 5);
                    plus4 = zipcode.substring(6, 10);
                }
                else {
                    zip = zipcode;
                }
            }
            else {
                zip = "";
                plus4 = "";
            };
            if (regionID > 0) {
                region = _.first(_.where($scope.subscrberViewModel.entlst.regions, { RegionID: parseInt(regionID) }));
                state = region.RegionCode;
            }
            
            return {
                Country: country,
                RegionCode: state,
                ZipCode: zip,
                Plus4: plus4
            }
        }
        $scope.FormatPhone = function (value) {
          
            if (value) {
                var temp = value.replace("-","");
                return temp.replace("-","")
            }
            else {
                return "";
            }
        }

        $scope.Save = function(){

                $scope.requiredfields = { required: "", requiredList: [{}] };
                angular.element("#alertDiv").hide();
                angular.element("#validationErrorDiv").hide();
                $rootScope.LoadingWindow.show();
                $scope.subscrberViewModel.PubSubscription.MailPermission = $scope.ConvertToBool($scope.subscrberViewModel.PubSubscription.MailPermission);
                $scope.subscrberViewModel.PubSubscription.FaxPermission = $scope.ConvertToBool($scope.subscrberViewModel.PubSubscription.FaxPermission);
                $scope.subscrberViewModel.PubSubscription.PhonePermission = $scope.ConvertToBool($scope.subscrberViewModel.PubSubscription.PhonePermission);
                $scope.subscrberViewModel.PubSubscription.OtherProductsPermission = $scope.ConvertToBool($scope.subscrberViewModel.PubSubscription.OtherProductsPermission);
                $scope.subscrberViewModel.PubSubscription.EmailRenewPermission = $scope.ConvertToBool($scope.subscrberViewModel.PubSubscription.EmailRenewPermission);
                $scope.subscrberViewModel.PubSubscription.ThirdPartyPermission = $scope.ConvertToBool($scope.subscrberViewModel.PubSubscription.ThirdPartyPermission);
                $scope.subscrberViewModel.PubSubscription.TextPermission = $scope.ConvertToBool($scope.subscrberViewModel.PubSubscription.TextPermission);

                //Set Changed Response List
                if ($scope.subscrberViewModel.madeResponseChange)
                {
                    $scope.ProductResponseList = [];
                    if ($scope.CurrentResponseList.length > 0) {
                        for (var i = 0; i < $scope.CurrentResponseList.length; i++) {
                            //Added all latest responses not present in current product map list
                            if ($scope.subscrberViewModel.PubSubscription.IsNewSubscription) {
                                $scope.ProductResponseList.push({
                                    PubSubscriptionID: $scope.subscrberViewModel.PubSubscription.PubSubscriptionID,
                                    SubscriptionID: $scope.subscrberViewModel.PubSubscription.SubscriptionID,
                                    CodeSheetID: $scope.CurrentResponseList[i].CodeSheetID,
                                    ResponseOther: $("#Other" + $scope.CurrentResponseList[i].GroupID).val(),
                                    DateCreated: angular.element("#demodatepicker_" + $scope.CurrentResponseList[i].GroupID).val() ? angular.element("#demodatepicker_" + $scope.CurrentResponseList[i].GroupID).val() : angular.element("#QualDate").val(),
                                    CreatedByUserID: $scope.subscrberViewModel.PubSubscription.CreatedByUserID
                                });
                            }
                            else {
                                $scope.ProductResponseList.push({
                                    PubSubscriptionID: $scope.subscrberViewModel.PubSubscription.PubSubscriptionID,
                                    SubscriptionID: $scope.subscrberViewModel.PubSubscription.SubscriptionID,
                                    CodeSheetID: $scope.CurrentResponseList[i].CodeSheetID,
                                    ResponseOther: $("#Other" + $scope.CurrentResponseList[i].GroupID).val(),
                                    DateCreated: angular.element("#demodatepicker_" + $scope.CurrentResponseList[i].GroupID).val() ? angular.element("#demodatepicker_" + $scope.CurrentResponseList[i].GroupID).val() : angular.element("#QualDate").val(),
                                    CreatedByUserID: $scope.subscrberViewModel.PubSubscription.CreatedByUserID
                                });
                            }
                        }
                    }
                    else {
                        $scope.ProductResponseList = [];
                    }


                }
                if ($scope.subscrberViewModel.PubSubscription.IsPaid) {
                    $scope.subscrberViewModel.madePaidChange = true;
                    $scope.subscrberViewModel.madePaidBillToChange = true;
                    $scope.subscrberViewModel.MySubscriptionPaid.PaymentTypeID = angular.element("#paymentTypeID").val();
                    $scope.subscrberViewModel.MySubscriptionPaid.PaidDate = angular.element("#PaidDate").val();
                    $scope.subscrberViewModel.MySubscriptionPaid.ExpireIssueDate = angular.element("#ExpireIssueDate").val();
                    $scope.subscrberViewModel.MySubscriptionPaid.StartIssueDate = angular.element("#StartIssueDate").val();
                    var czr = $scope.GetCountryZipRegion(angular.element("#CountryBillTo").val(), angular.element("#StateBillTo").val(), angular.element("#ZipCodeBillTo").val());
                    
                    var myPaidBillTo = {
                        PaidBillToID: $scope.subscrberViewModel.MyPaidBillTo.PaidBillToID,
                        SubscriptionPaidID: $scope.subscrberViewModel.MyPaidBillTo.SubscriptionPaidID,
                        PubSubscriptionID: $scope.subscrberViewModel.MyPaidBillTo.PubSubscriptionID,
                        Latitude: $scope.subscrberViewModel.MyPaidBillTo.Latitude,
                        Longitude: $scope.subscrberViewModel.MyPaidBillTo.Longitude,
                        IsAddressValidated: $scope.subscrberViewModel.MyPaidBillTo.IsAddressValidated,
                        AddressValidationDate: $scope.subscrberViewModel.MyPaidBillTo.AddressValidationDate,
                        AddressValidationSource: $scope.subscrberViewModel.MyPaidBillTo.AddressValidationSource,
                        AddressValidationMessage: $scope.subscrberViewModel.MyPaidBillTo.AddressValidationMessage,
                        CarrierRoute: $scope.subscrberViewModel.MyPaidBillTo.CarrierRoute,
                        DateCreated: $scope.subscrberViewModel.MyPaidBillTo.DateCreated,
                        FirstName: angular.element("#FirstNameBillTo").val(),
                        LastName: angular.element("#LastNameBillTo").val(),
                        Company: angular.element("#CompanyBillTo").val(),
                        Title: angular.element("#TitleBillTo").val(),
                        Address1: angular.element("#Address1BillTo").val(),
                        Address2: angular.element("#Address2BillTo").val(),
                        Address3: angular.element("#Address3BillTo").val(),
                        City: angular.element("#CityBillTo").val(),
                        ZipCode: angular.element("#ZipCodeBillTo").val(),
                        Plus4: angular.element("#Plus4BillTo").val(),
                        County: angular.element("#CountyBillTo").val(),
                        Email: angular.element("#EmailBillTo").val(),
                        Phone: $scope.FormatPhone(angular.element("#PhoneBillTo").val()),
                        PhoneExt: angular.element("#PhoneExtBillTo").val(),
                        Fax: $scope.FormatPhone(angular.element("#FaxBillTo").val()),
                        Mobile: $scope.FormatPhone(angular.element("#MobileBillTo").val()),
                        Website: angular.element("#WebsiteBillTo").val(),
                        RegionID: angular.element("#StateBillTo").val(),
                        CountryID: angular.element("#CountryBillTo").val(),
                        AddressTypeId: angular.element("#AddressTypeBillTo").val(),
                        RegionCode: czr.RegionCode,
                        Country:czr.Country.ShortName
                    }
                }
                if ($scope.isDate(angular.element('#QualDate').val())) {
                    $scope.subscrberViewModel.PubSubscription.QualificationDate = angular.element("#QualDate").val();
                    angular.element("#alertMsg").text("");
                    angular.element("#alertDiv").hide();
                
                    var model = {
                    Enabled: $scope.subscrberViewModel.Enabled,
                    MadeAdHocChange: $scope.subscrberViewModel.madeAdHocChange,
                    Saved: $scope.subscrberViewModel.Saved,
                    MadeResponseChange: $scope.subscrberViewModel.madeResponseChange,
                    MadePaidChange: $scope.subscrberViewModel.madePaidChange,
                    MadePaidBillToChange: $scope.subscrberViewModel.madePaidBillToChange,
                    CategoryCodeTypeID: $scope.subscrberViewModel.CategoryCodeTypeID,
                    TransactionName: $scope.subscrberViewModel.TransactionName,
                    SubStatusEnabled: $scope.subscrberViewModel.SubStatusEnabled,
                    TriggerQualDate: $scope.subscrberViewModel.TriggerQualDate,
                    IsCountryEnabled: $scope.subscrberViewModel.IsCountryEnabled,
                    btnPOKillChecked: $scope.subscrberViewModel.btnPOKillChecked,
                    btnPersonKillChecked: $scope.subscrberViewModel.btnPersonKillChecked,
                    btnOnBehalfKillChecked: $scope.subscrberViewModel.btnOnBehalfKillChecked,
                    CategoryFreePaidEnabled: $scope.subscrberViewModel.CategoryFreePaidEnabled,
                    CategoryCodeEnabled: $scope.subscrberViewModel.CategoryCodeEnabled,
                    ReactivateButtonEnabled: $scope.subscrberViewModel.ReactivateButtonEnabled,
                    PubSubscription: $scope.subscrberViewModel.PubSubscription,
                    MyPaidBillTo: myPaidBillTo,
                    MySubscriptionPaid: $scope.subscrberViewModel.MySubscriptionPaid,
                    ProductResponseList: $scope.ProductResponseList
                }


                dataService.saveSubscriber(model).then(function (response) {

                    $scope.OverrideRecordUpdate = false;
                    $rootScope.LoadingWindow.close();
                    $scope.result = response.data;
                    $scope.submitted = true;
                    angular.forEach($scope.result.ErrorList, function (key, value) {
                        if (value == 'Error_EmptyQualDate') {
                            angular.element("#QualDate").addClass("highlight");
                        }
                        $scope.showMesssageWindow(value, key, $scope.result.PubSubscription.PubSubscriptionID);
                    }
                      )

                },
                   function (response) {
                       $rootScope.LoadingWindow.close();
                       $scope.showMesssageWindow("Error", "Failed to save the record.");
                       $scope.submitted = false;

                   });
                } else {
                    angular.element("#alertMsg").text("Please provide valid qualification date.");
                    angular.element("#QualDate").removeClass("highlight");
                    angular.element("#alertDiv").show();
                    $rootScope.LoadingWindow.close();
                    $scope.gotoTop();
                }

                }

        
        $scope.isDate = function(date) {
            return (new Date(date) !== "Invalid Date" && !isNaN(new Date(date)))? true : false;
        }
        //Events Starts

        //Category Type Changes
        $scope.CheckRequired = function () {
            debugger;
            var dropdownlistCategory = angular.element("#PubCategoryTypeID").data("kendoDropDownList");
            if (dropdownlistCategory) {
                if (dropdownlistCategory.text() == "NonQualified Free") {
                    $("#QSourceRequiredStar").hide();
                    $("#Par3CRequiredStar").hide();
                    $("#errorLblQSource").hide();
                    $("#errorLblPar3CID").hide();
                     return false;
                } else {
                    $("#QSourceRequiredStar").show();
                    $("#Par3CRequiredStar").show();
                    if ($("#QSource").val()>0)
                        $("#errorLblQSource").hide();
                    else
                        $("#errorLblQSource").show();
                    if ($("#Par3C").val() > 0)
                        $("#errorLblPar3CID").hide();
                    else
                        $("#errorLblPar3CID").show();
                   
                    return true;
                }
            } else {
                return false;
            }
            
        }

        angular.element("#PubCategoryTypeID").on("change", function () {

           
            var filteredCategoryCodes = [];
            $scope.subscrberViewModel.PubSubscription.PubCategoryID = 0;
            for (var i = 1; i < $scope.subscrberViewModel.entlst.categoryCodeList.length; i++) {

                if ($scope.subscrberViewModel.entlst.categoryCodeList[i].CategoryCodeTypeID == angular.element("#PubCategoryTypeID").val()) {
                    filteredCategoryCodes.push({ CategoryCodeName: $scope.subscrberViewModel.entlst.categoryCodeList[i].CategoryCodeName, CategoryCodeID: $scope.subscrberViewModel.entlst.categoryCodeList[i].CategoryCodeID })
                }
            }
            
            var dropdownlistCategoryCode = angular.element("#PubCategoryCodeID").data("kendoDropDownList");

            dropdownlistCategoryCode.setDataSource(filteredCategoryCodes);

            $scope.setPaidOrFreeCategoryType();

            angular.element("#TransactionText").text($scope.subscrberViewModel.TransactionCodeName);

            $scope.CheckRequired();
        });

        //CategoryCode Changed
        angular.element("#PubCategoryCodeID").on("change", function () {

            var catcodeID = angular.element("#PubCategoryCodeID").val();
            if (catcodeID!='') {
               $scope.CategoryCodeName = $.grep($scope.subscrberViewModel.entlst.categoryCodeList, function (catcode) {
                    return catcode.CategoryCodeID == catcodeID;
               })[0].CategoryCodeName;
                $scope.CategoryCodeValue = $.grep($scope.subscrberViewModel.entlst.categoryCodeList, function (catcode) {
                    return catcode.CategoryCodeID == catcodeID;
                })[0].CategoryCodeValue;

                if ($scope.CategoryCodeValue == 11 || $scope.CategoryCodeValue == 21 || $scope.CategoryCodeValue == 25 || $scope.CategoryCodeValue == 28 || $scope.CategoryCodeValue == 31 ||
                             $scope.CategoryCodeValue == 35 || $scope.CategoryCodeValue == 51 || $scope.CategoryCodeValue == 56 || $scope.CategoryCodeValue == 62) {
                    $scope.subscrberViewModel.IsCopiesEnabled = true;

                }
                else {
                    $scope.subscrberViewModel.IsCopiesEnabled = false;
            }

                $scope.setSubscriptionStatus();
            }
            else {
                $scope.subscrberViewModel.PubSubscription.PubCategoryID = 0;
               
        }

        });

        
        //Product changed
        angular.element("#ProductID").on("change", function () {
           
            $scope.CurrentProduct = _.first(_.where($scope.subscrberViewModel.ProductList, { PubID: parseInt(angular.element("#ProductID").val()) }));
            $scope.subscrberViewModel.Product = $scope.CurrentProduct;
            $scope.subscrberViewModel.PubSubscription.PubID = $scope.CurrentProduct.PubID;
            $scope.subscrberViewModel.PubSubscription.PubCode = $scope.CurrentProduct.PubID.PubCode;
            $scope.subscrberViewModel.PubSubscription.PubName = $scope.CurrentProduct.PubID.PubName;
            $scope.reloadSubscriber($scope.subscrberViewModel.PubSubscription.PubSubscriptionID, angular.element("#ProductID").val());

        });

        //Address Fields Chnaged
        angular.element("#Address1").on("change", function () {

            $scope.AddressChanged();
        });
        angular.element("#Address2").on("change", function () {

            $scope.AddressChanged();
        });
        angular.element("#Address3").on("change", function () {

            $scope.AddressChanged();
        });
        angular.element("#City").on("change", function () {

            $scope.AddressChanged();
        });
        angular.element("#AddressTypeCodeId").on("change", function () {

            $scope.AddressChanged();
        });
        angular.element("#FullZip").on("change", function () {

            $scope.AddressChanged();
        });
        angular.element("#State").on("change", function () {

            $scope.AddressChanged();
        });
        angular.element("#Country").on("change", function (e) {

            $scope.AddressChanged();
            $scope.CurrentCountry = _.first(_.where($scope.subscrberViewModel.entlst.countryList, { CountryID: parseInt(angular.element("#Country").val()) }));

            $scope.subscrberViewModel.PubSubscription.PhoneCode = $scope.CurrentCountry.PhonePrefix;

            //Change Phone Prefix
            angular.element("#PhoneCode").val($scope.CurrentCountry.PhonePrefix);

            //Chnage phone format
            if (angular.element("#Country").val() == 1 || angular.element("#Country").val() == 2) {
                angular.element("#Phone").kendoMaskedTextBox({
                        mask: "000-000-0000",
                        unmaskOnPost: true
                });
                angular.element("#Mobile").kendoMaskedTextBox({
                        mask: "000-000-0000",
                        unmaskOnPost: true
                });
                angular.element("#Fax").kendoMaskedTextBox({
                        mask: "000-000-0000",
                        unmaskOnPost: true
                });
            }
            else {

                angular.element("#Phone").kendoMaskedTextBox({
                        mask: "0000000000",
                        unmaskOnPost: true
                });
                angular.element("#Mobile").kendoMaskedTextBox({
                        mask: "0000000000",
                        unmaskOnPost: true
                });
                angular.element("#Fax").kendoMaskedTextBox({
                        mask: "0000000000",
                        unmaskOnPost: true
                });
        }

            //Change Zip Code Format
            if (angular.element("#Country").val() == 1) {
                angular.element("#FullZip").kendoMaskedTextBox({
                        mask: "00000-0000",
                        unmaskOnPost: true
                });

            }
            else if (angular.element("#Country").val() == 2) {
                angular.element("#FullZip").kendoMaskedTextBox({
                        mask: "000 000",
                        unmaskOnPost: true
                });

            }
            else {
                angular.element("#FullZip").kendoMaskedTextBox({
                        mask: "AAAAAAAAAA",
                        unmaskOnPost: true
                });

        }
            
        var dropdownlist = angular.element("#State").data("kendoDropDownList");
        dropdownlist.setDataSource($scope.getFilteredRegions(angular.element("#Country").val()));

        });

        //Qualification Triggered
        angular.element("#QualDate").on("click", function () {

            angular.element("#QualDate").removeClass("highlight");
            $scope.subscrberViewModel.PubSubscription.QualificationDate = angular.element("#QualDate").val();
            $scope.subscrberViewModel.TriggerQualDate = false;

        })
        angular.element("#rstButton").on("click", function () {
           
            $scope.Reset();
          
        })
        angular.element("#rstButton1").on("click", function () {

            $scope.Reset();

        })
        //Search if Firstname and Lastname is available for new subscriber
        angular.element("#subcriber_txtLastName").on("change", function () {
            if (angular.element("#subcriber_txtLastName") && angular.element("#subcriber_txtFirstName") && $scope.subscrberViewModel.PubSubscription.IsNewSubscription) {
                $scope.SuggestMatches();
            }

        })
        //Open Matched records
        angular.element("#btnSaveMatched").on("click", function () {
            $scope.openMatchedRecord();
        })
        //If Bill to country changed
        angular.element("#CountryBillTo").on("change", function (e) {

            $scope.subscrberViewModel.madePaidChange = true;
            $scope.CurrentCountryBillTo = _.first(_.where($scope.subscrberViewModel.entlst.countryList, { CountryID: parseInt(angular.element("#CountryBillTo").val()) }));

            $scope.subscrberViewModel.MyPaidBillTo.PhoneCode = $scope.CurrentCountryBillTo.PhonePrefix;

            //Change Phone Prefix
            angular.element("#PhoneCodeBilTo").val($scope.CurrentCountryBillTo.PhonePrefix);

            //Chnage phone format
            if (angular.element("#CountryBillTo").val() == 1 || angular.element("#CountryBillTo").val() == 2) {
                angular.element("#PhoneBillTo").kendoMaskedTextBox({
                    mask: "000-000-0000",
                    unmaskOnPost: true
                });
                angular.element("#MobileBillTo").kendoMaskedTextBox({
                    mask: "000-000-0000",
                    unmaskOnPost: true
                });
                angular.element("#FaxBillTo").kendoMaskedTextBox({
                    mask: "000-000-0000",
                    unmaskOnPost: true
                });
            }
            else {

                angular.element("#PhoneBillTo").kendoMaskedTextBox({
                    mask: "0000000000",
                    unmaskOnPost: true
                });
                angular.element("#MobileBillTo").kendoMaskedTextBox({
                    mask: "0000000000",
                    unmaskOnPost: true
                });
                angular.element("#FaxBillTo").kendoMaskedTextBox({
                    mask: "0000000000",
                    unmaskOnPost: true
                });
            }

            //Change Zip Code Format
            if (angular.element("#CountryBillTo").val() == 1) {
                angular.element("#FullZipBillTo").kendoMaskedTextBox({
                    mask: "00000-0000",
                    unmaskOnPost: true
                });

            }
            else if (angular.element("#CountryBillTo").val() == 2) {
                angular.element("#FullZipBillTo").kendoMaskedTextBox({
                    mask: "000 000",
                    unmaskOnPost: true
                });

            }
            else {
                angular.element("#FullZipBillTo").kendoMaskedTextBox({
                    mask: "AAAAAAAAAA",
                    unmaskOnPost: true
                });

            }
           
            var dropdownlist = angular.element("#StateBillTo").data("kendoDropDownList");
            dropdownlist.setDataSource($scope.getFilteredRegions(angular.element("#CountryBillTo").val()));
            
           

        });
        //If payment type changed
        angular.element("#paymentTypeID").on("change", function () {

            $scope.subscrberViewModel.madePaidChange = true;
            $scope.setUpPaid(angular.element("#paymentTypeID").val());
        });

        //Fetch Data for new or Edit
        $scope.firstload();


        }
})();

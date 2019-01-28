var SgFormApp = /** @class */ (function () {
    function SgFormApp() {
        var app = angular.module("SgFormApp", ["ngMessages", "ngAnimate", "ngRoute", "ngSanitize"])
            .directive('input', ['$filter', '$timeout', function ($filter, $timeout) { return new SgInputDirective($filter, $timeout); }])
            .directive("form", ['$compile', '$timeout', function ($compile, $timeout) { return new SgFormsDirective(); }])
            .controller("SgFormController", ['$scope', '$http', SgFormController]);
    }
    return SgFormApp;
}());
var SgFormController = /** @class */ (function () {
    function SgFormController($scope, $http) {
        this.$scope = $scope;
        this.$http = $http;
    }
    return SgFormController;
}());
var SgFormsDirective = /** @class */ (function () {
    function SgFormsDirective() {
        var _this = this;
        this.restrict = 'E';
        this.link = function ($scope, elm, attrs, ctrl) {
            $scope.$watch(attrs.name + ".$error.required.length", function (newValue) {
                SgFormsDirective.ShowFooterStaticPopover($scope[attrs.name].$error, attrs);
            }, true);
            setTimeout(function () {
                _this.FirstRequireFocus(elm); //確保List --> Main 沒有任何Required的focus
            }, 400);
        };
    }
    SgFormsDirective.ShowFooterStaticPopover = function (newValue, attrs) {
        var obj = $("#footer-descript-pop");
        //obj.popover("hide");
        Sort(newValue);
        for (var key in newValue) {
            console.log(key);
            if (key == "required") {
                var validate = newValue[key][0];
                if (typeof validate.$error[key] == "object") { //ng-form
                    var formName = validate.$name;
                    var itemName = validate.$error[key][0].$name;
                    ShowMessage(formName, itemName);
                }
                else {
                    var formName = attrs.name;
                    var itemName = validate.$name;
                    if (attrs.ngForm) {
                        formName = attrs.ngForm; //for ngForm Directive
                    }
                    ShowMessage(formName, itemName);
                }
                break; //only show first type of validation
            }
        }
        HidePopover(newValue);
        function HidePopover(newValue) {
            if (newValue.required == null || newValue.required.length == 0) {
                setTimeout(function () {
                    if (SgFormsDirective.Processing)
                        return;
                    obj.popover("hide");
                }, 100); //主要為控制開啟與關閉要有同一時間的控制ps:100同showMessage的時間,2個要一致,才不會後面蓋前面
            }
        }
        function Sort(newValue) {
            var sortArray = newValue.required;
            if (sortArray == undefined)
                return;
            var requireds = $("[required]");
            sortArray.sort(function (a, b) {
                var aIndex = requireds.index($("[name='" + a.$name + "']"));
                var bIndex = requireds.index($("[name='" + b.$name + "']"));
                //SgFormsDirective.log("a", a.$name + ":" + aIndex);
                //SgFormsDirective.log("b", b.$name + ":" + bIndex);
                if (aIndex == -1 || bIndex == -1)
                    return -999;
                return aIndex - bIndex; //asc
            });
        }
        function ShowMessage(formName, itemName) {
            console.log(itemName);
        }
        function SetFocusColor(obj, color) {
            obj.css("border-bottom-color", color);
            obj.css("border-top-color", color);
            obj.css("border-left-color", color);
            obj.css("border-right-color", color);
        }
    };
    SgFormsDirective.log = function (name, obj) {
        console.log(name + " -- begin");
        console.log(obj);
        console.log(name + " -- end");
        console.log("");
    };
    SgFormsDirective.prototype.FirstRequireFocus = function (form) {
        if ($(form).attr("disabled-required-focus") == "Y" || $("[disabled-required-focus=Y]").length > 0) //20180122修正因Required延遲focus造成的inv_date null錯誤
            return;
        //確保只Focus一次
        if ($(form).attr("auto-focused") != "Y" && $(form).find(":input").length > 0) {
            $(form).attr("auto-focused", "Y");
            var $inputs = $(form).find(":input").not("button");
            if ($inputs.length == 0 || $inputs.filter("[sg_focus_me]").length > 0) //自行focus
                return;
            if ($inputs.filter("[required]").length > 0) {
                $($inputs.filter("[required]")[0]).focus();
            }
            else
                $inputs[0].focus();
        }
    };
    SgFormsDirective.Processing = false;
    return SgFormsDirective;
}());
var SgNgFormDirective = /** @class */ (function () {
    function SgNgFormDirective() {
        this.restrict = 'A';
        this.link = function ($scope, elm, attrs, ctrl) {
            $scope.$watch(attrs.ngForm + ".$error", function (newValue) {
                if (SgFormsDirective.Processing)
                    return;
                SgFormsDirective.ShowFooterStaticPopover(newValue, attrs);
            }, true);
        };
    }
    return SgNgFormDirective;
}());
//# sourceMappingURL=SgForm.js.map
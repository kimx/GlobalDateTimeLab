class HomeApp {
    constructor() {
        var app = angular.module("HomeApp", ["ngMessages", "ngAnimate", "ngRoute", "ngSanitize"])
            .directive('input', ['$filter', '$timeout', function ($filter, $timeout) { return new SgInputDirective($filter, $timeout); }])
            .directive("form", ['$compile', '$timeout', function ($compile, $timeout) { return new SgFormsDirective() }])
            //   .config(["$routeProvider", ($routeProvider) => { this.Config($routeProvider) }])
            .controller("HomeController", ['$scope', '$http', HomeController]);
    }

    //Config($routeProvider: ng.route.IRouteProvider) {
    //    $routeProvider
    //        .when('/Main/:idno?', { templateUrl: "/PUR/PurchaseOrder/Main", controller: "MainController", controllerAs: "vm" })
    //        ;
    //}
}

class HomeController {
    Model: any;
    constructor(private $scope: ng.IScope, private $http: ng.IHttpService) {
        this.Model = {};
        //this.Model.CreateDate = "2019-01-19T00:00:00";
        this.Model.CreateDate = "2019-01-19T00:00:00";
    }

    Send(useUnSpecified: boolean) {
        this.Model.UseUnSpecified = useUnSpecified;
        this.$http.post("/Home/Send", this.Model).then((response) => {
            console.log(response);
            this.Model = response.data;
        });
        //this.Model.CreateDate = "2019-02-19T00:00:00";
    }

    SendAPI(useUnSpecified: boolean) {
        this.Model.UseUnSpecified = useUnSpecified;
        this.$http.post("/api/HomeApi/PostModel", this.Model).then((response) => {
            console.log(response);
            this.Model = response.data;
        });
    }
}

class SgInputDirective implements ng.IDirective {
    constructor(private $filter, private $timeout: ng.ITimeoutService) { }
    require = "ngModel";//必要,ngModel物件才會傳入
    link = (scope, el, attrs, ngModel) => {
        if (attrs.type === "date") {
            //修正用delete值的方式,變空值時無法通過驗證 https://github.com/angular/angular.js/issues/12853 1.5.x fixed
            //el.on('keyup', el.triggerHandler.bind(el, 'input'));
            el.on('mousedown', () => {//20160613 1.5.3 fixed for click "x"  https://github.com/angular/angular.js/issues/14740
                this.$timeout(function () {
                    el.triggerHandler('input');
                }, 150, false);

            });

            //將日期字串轉成date
            //https://github.com/betsol/angular-input-date/blob/master/src/angular-input-date.js
            ngModel.$formatters.push((modelValue) => {//Model -> View
                console.log("modelValue");
                console.log(modelValue);
                if (modelValue) {
                    //var date = new Date(this.$filter('date')(modelValue, "yyyy-MM-dd"));
                    var date = new Date(modelValue);//配合:T00:00:00 不加時間及Z的寫法，才不會在減的時區，日期少了一天
                    //console.log(date);
                    //console.log(date2);
                    return date;
                }
                return modelValue;
            });

            ngModel.$parsers.push((viewValue) => {//View -> Model
                console.log("viewValue");
                console.log(viewValue);
                var dateString = viewValue;
                if (ngModel.$isEmpty(dateString)) {
                    return null;
                }
                dateString = this.$filter('date')(dateString, "yyyy-MM-dd") + "T00:00:00";//Alwasys設為UTC整點 避免null的選擇會取到前一天
                //console.log("dateString");
                //console.log(dateString);
                return dateString;
                //return new Date(dateString);
            });


        }

    }

}

class SgFormsDirective2 implements ng.IDirective {
    constructor() { }
    restrict = 'E';
    static Processing = false;
    link = ($scope, elm, attrs, ctrl) => {
        console.log(attrs.name);
        $scope.$watch(attrs.name + ".$valid", (newValue) => {
            console.log($scope[attrs.name].$error);
        }, true);
        setTimeout(() => {
        }, 400);
    }
}


 class SgFormsDirective implements ng.IDirective {
    constructor() { }
    restrict = 'E';
    static Processing = false;
    link = ($scope, elm, attrs, ctrl) => {
        $scope.$watch(attrs.name + ".$error", (newValue) => {
            SgFormsDirective.ShowFooterStaticPopover(newValue, attrs);

        }, true);
        setTimeout(() => {
            this.FirstRequireFocus(elm);//確保List --> Main 沒有任何Required的focus
        }, 400);
    }

    static ShowFooterStaticPopover(newValue, attrs) {
        var obj: any = $("#footer-descript-pop");
        //obj.popover("hide");
        Sort(newValue);
        for (var key in newValue) {
            if (key == "required") {
                var validate = newValue[key][0];
                if (typeof validate.$error[key] == "object") {//ng-form
                    var formName = validate.$name;
                    var itemName = validate.$error[key][0].$name;
                    ShowMessage(formName, itemName);
                }
                else {
                    var formName = attrs.name;
                    var itemName = validate.$name;
                    if (attrs.ngForm) {
                        formName = attrs.ngForm;//for ngForm Directive
                    }

                    ShowMessage(formName, itemName);
                }
                break;//only show first type of validation
            }

        }
        HidePopover(newValue);

        function HidePopover(newValue) {
            if (newValue.required == null || newValue.required.length == 0) {
                setTimeout(function () {
                    if (SgFormsDirective.Processing)
                        return;
                    obj.popover("hide");
                }, 100);//主要為控制開啟與關閉要有同一時間的控制ps:100同showMessage的時間,2個要一致,才不會後面蓋前面
            }
        }
        function Sort(newValue) {
            var sortArray: Array<any> = newValue.required;
            if (sortArray == undefined)
                return;
            var requireds = $("[required]");
            sortArray.sort((a, b) => {
                var aIndex = requireds.index($("[name='" + a.$name + "']"));
                var bIndex = requireds.index($("[name='" + b.$name + "']"));
                //SgFormsDirective.log("a", a.$name + ":" + aIndex);
                //SgFormsDirective.log("b", b.$name + ":" + bIndex);

                if (aIndex == -1 || bIndex == -1)
                    return -999;
                return aIndex - bIndex;//asc
            });
        }

        function ShowMessage(formName, itemName) {
            var targetId = $("[name='" + itemName + "']").attr("id");
            var ngMessagesSelector = " [ng-messages][form-name=" + formName + "][target-id=" + targetId + "]";
            var ngMessages = $(ngMessagesSelector);
            SgFormsDirective.Processing = true;
            setTimeout(() => {
                SgFormsDirective.Processing = false;
                var msgHtml = $(ngMessages).find("[ng-message=required]").html();
                //SgFormsDirective.log("msgHtml", msgHtml);
                if (msgHtml == "" || msgHtml == undefined) {
                    obj.popover("hide");
                    return;
                }
                //msgHtml = "<a id='footer-descript-pop-focus'><span class='fa fa-hand-pointer-o'> " + msgHtml + "!</span></a>";
                msgHtml = "<a id='footer-descript-pop-focus'><i class='fa fa-hand-pointer-o'></i> <span class='small'>" + msgHtml + "!</span></a>";
                obj.attr("data-content", msgHtml);
                obj.popover("show");
                var itemObj = $("[name='" + itemName + "']:last");
                $("#footer-descript-pop-focus").on("click", function () {
                    SetFocusColor(itemObj, "red");
                    UiHelper.Scroll(itemObj, 100, 10);
                    itemObj.animate({ opacity: 0.2, }, 200, () => {
                        itemObj.animate({ opacity: 1, }, 200, () => {
                            SetFocusColor(itemObj, "#d2d6de");
                            itemObj.focus();
                        }
                        );
                    }
                    );

                });

            }, 100);
        }

        function SetFocusColor(obj, color) {
            obj.css("border-bottom-color", color);
            obj.css("border-top-color", color);
            obj.css("border-left-color", color);
            obj.css("border-right-color", color);
        }
    }

    static log(name, obj) {
        console.log(name + " -- begin");
        console.log(obj);
        console.log(name + " -- end");
        console.log("");

    }

    FirstRequireFocus(form) {
        if ($(form).attr("disabled-required-focus") == "Y" || $("[disabled-required-focus=Y]").length > 0)//20180122修正因Required延遲focus造成的inv_date null錯誤
            return;
        //確保只Focus一次
        if ($(form).attr("auto-focused") != "Y" && $(form).find(":input").length > 0) {
            $(form).attr("auto-focused", "Y");
            var $inputs = $(form).find(":input").not("button");

            if ($inputs.length == 0 || $inputs.filter("[sg_focus_me]").length > 0)//自行focus
                return;

            if ($inputs.filter("[required]").length > 0) {
                $($inputs.filter("[required]")[0]).focus();
            }
            else
                $inputs[0].focus();
        }
    }
}

 class SgNgFormDirective implements ng.IDirective {
    constructor() { }
    restrict = 'A';
    link = ($scope, elm, attrs, ctrl) => {
        $scope.$watch(attrs.ngForm + ".$error", (newValue) => {
            if (SgFormsDirective.Processing)
                return;
            SgFormsDirective.ShowFooterStaticPopover(newValue, attrs);

        }, true);
    }

}
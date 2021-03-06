﻿class HomeApp {
    constructor() {
        var app = angular.module("HomeApp", ["ngMessages", "ngAnimate", "ngRoute", "ngSanitize"])
            .directive('input', ['$filter', '$timeout', function ($filter, $timeout) { return new SgInputDirective($filter, $timeout); }])
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



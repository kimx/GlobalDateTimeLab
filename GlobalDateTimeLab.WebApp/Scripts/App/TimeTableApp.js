var TimeTableApp = /** @class */ (function () {
    function TimeTableApp() {
        var app = angular.module("TimeTableApp", ["ngMessages", "ngAnimate", "ngRoute", "ngSanitize"])
            .directive('input', ['$filter', '$timeout', function ($filter, $timeout) { return new SgInputDirective($filter, $timeout); }])
            .controller("TimeTableController", ['$scope', '$http', TimeTableController]);
    }
    return TimeTableApp;
}());
var TimeTableController = /** @class */ (function () {
    function TimeTableController($scope, $http) {
        this.$scope = $scope;
        this.$http = $http;
        this.Model = {
            TimeZones: [
                { Name: "UTC", Value: 0 },
                { Name: "SouthAfrica", Value: 2 },
                { Name: "Dubayy", Value: 4 },
                { Name: "Taiwan", Value: 8 },
            ],
        };
        //this.Model.CreateDate = "2019-01-19T00:00:00";
        this.Model.CreateDate = "2019-01-19T00:00:00";
    }
    TimeTableController.prototype.Send = function (useUnSpecified) {
    };
    TimeTableController.prototype.GetHours = function () {
        return Array(24);
    };
    return TimeTableController;
}());
//# sourceMappingURL=TimeTableApp.js.map
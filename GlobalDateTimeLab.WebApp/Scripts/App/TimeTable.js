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
        this.Model.CurrentHour = new Date().getUTCHours();
        this.Model.CurrentTimeZone = this.Model.TimeZones[0];
        this.Model.CurrentMinute = 0;
    }
    TimeTableController.prototype.GetHours = function () {
        return Array(24);
    };
    TimeTableController.prototype.CalTimeZoneHour = function (hour, timeZone) {
        var calHour = hour + timeZone.Value;
        if (calHour >= 24)
            calHour = calHour - 24;
        return calHour;
    };
    TimeTableController.prototype.GetCurrentTimeZoneUtcHour = function () {
        var currentTimeZoneUtcHour = this.Model.CurrentHour - this.Model.CurrentTimeZone.Value;
        if (currentTimeZoneUtcHour < 0)
            currentTimeZoneUtcHour = currentTimeZoneUtcHour + 24;
        return currentTimeZoneUtcHour;
    };
    TimeTableController.prototype.CurrentHourClass = function (hour, timeZone) {
        var currentTimeZoneUtcHour = this.GetCurrentTimeZoneUtcHour();
        if (hour == currentTimeZoneUtcHour) {
            return "warning";
        }
        return "";
    };
    TimeTableController.prototype.SelectHour = function (hour, timeZone) {
        this.Model.CurrentHour = this.CalTimeZoneHour(hour, timeZone);
        this.Model.CurrentTimeZone = this.Model.TimeZones.filter(function (f) { return f.Name == timeZone.Name; })[0];
    };
    TimeTableController.prototype.GetCurrentDateTime = function (timeZone) {
        var currentTimeZoneUtcHour = this.GetCurrentTimeZoneUtcHour();
        var currentTimeZoneHour = currentTimeZoneUtcHour + timeZone.Value;
        console.log(currentTimeZoneUtcHour);
        var now = new Date();
        var nowUtc = new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), currentTimeZoneHour, this.Model.CurrentMinute, 0);
        return nowUtc;
    };
    return TimeTableController;
}());
//# sourceMappingURL=TimeTable.js.map
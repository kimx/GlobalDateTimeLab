class TimeTableApp {
    constructor() {
        var app = angular.module("TimeTableApp", ["ngMessages", "ngAnimate", "ngRoute", "ngSanitize"])
            .directive('input', ['$filter', '$timeout', function ($filter, $timeout) { return new SgInputDirective($filter, $timeout); }])
            .controller("TimeTableController", ['$scope', '$http', TimeTableController]);
    }


}

class TimeTableController {
    Model: any;
    constructor(private $scope: ng.IScope, private $http: ng.IHttpService) {
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


    GetHours() {
        return Array(24);
    }

    CalTimeZoneHour(hour: number, timeZone): number {
        var calHour = hour + timeZone.Value;
        if (calHour >= 24)
            calHour = calHour - 24;
        return calHour;
    }

    private GetCurrentTimeZoneUtcHour() {
        var currentTimeZoneUtcHour = this.Model.CurrentHour - this.Model.CurrentTimeZone.Value;
        if (currentTimeZoneUtcHour < 0)
            currentTimeZoneUtcHour = currentTimeZoneUtcHour + 24;
        return currentTimeZoneUtcHour;
    }

    CurrentHourClass(hour: number, timeZone): string {
        var currentTimeZoneUtcHour = this.GetCurrentTimeZoneUtcHour();
        if (hour == currentTimeZoneUtcHour) {
            return "warning";
        }
        return "";
    }

    SelectHour(hour: number, timeZone) {
        this.Model.CurrentHour = this.CalTimeZoneHour(hour, timeZone);
        this.Model.CurrentTimeZone = this.Model.TimeZones.filter(f => f.Name == timeZone.Name)[0];
        
    }

    GetCurrentDateTime(timeZone) {
        var currentTimeZoneUtcHour = this.GetCurrentTimeZoneUtcHour();
        var currentTimeZoneHour = currentTimeZoneUtcHour + timeZone.Value;
        console.log(currentTimeZoneUtcHour);
        var now = new Date();
        var nowUtc = new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), currentTimeZoneHour, this.Model.CurrentMinute, 0);
        return nowUtc;
    }

}



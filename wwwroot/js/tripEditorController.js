// tripEditorController.js
(function(){
    'use strict';

    angular.module("app-trips").controller("tripEditorController", tripEditorController);

    function tripEditorController($routeParams, $http) {
        var vm = this;
        vm.tripName = $routeParams.tripName;
        vm.stops = [];
        vm.errorMessage = "";
        vm.isbusy = true;

        $http.get("/api/trips/" + vm.tripName + "/stops")
             .then(function(response) {
                angular.copy(response.data, vm.stops);
                _showMap(vm.stops);
             }, function(error) {
                 vm.errorMessage = "Failed to load stops";
             })
             .finally(function() { vm.isbusy = false; });    
    }

    function _showMap(stops) {  // 使用 _ 代表此為 inside javascript object scope，不會被外部存取
        if (stops && stops.length > 0) {
            var mapStops = _.map(stops, function(item){
                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };
            });
            //Show Map
            travelMap.createMap({
                stops: mapStops,
                selector: "#map",   // 指定要顯示的位置: html tag id="#map"
                currentStop: 1,
                initialZoom: 3
            });
        }
    }
})();
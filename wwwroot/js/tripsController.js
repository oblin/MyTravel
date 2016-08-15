// tripsController.js
(function () {
    'use strict';

    angular.module("app-trips")
        .controller("tripsController", tripsController);
    tripsController.$inject = ['$http'];
    function tripsController($http) {
        var vm = this;
        vm.trips = [];

        vm.newTrip = {};

        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/trips").then(
            function (response) {
                // success
                angular.copy(response.data, vm.trips);
            },
            function (error) {
                // failure
                vm.errorMessage = "Failed to Load data: " + error;
            }
        ).finally(function () {
            // vm.isBusy = false;
        });

        vm.addTrip = function () {
            vm.isBusy = true;

            $http.post("/api/trips", vm.newTrip).then(
                function (response) {
                    vm.trips.push(response.data);
                    vm.newTrip = {};
                },
                function (error) {
                    vm.errorMessage = "Failed to Load data: " + error;
                }
            ).finally(function () {
                vm.isBusy = false;
            });
        }
    }
})();
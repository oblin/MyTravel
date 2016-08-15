// tripsController.js
(function () {
    'use strict';

    angular.module("app-trips")
        .controller("tripsController", tripsController);

    function tripsController() {
        var vm = this;
        vm.trips = [{
            name: "US Trip",
            created: "2015-01-12"
        }, {
            name: "Japan Trip",
            created: "2015-11-21"
        }];

        vm.newTrip = {};

        vm.addTrip = function () {
            alert(vm.newTrip.name);
        }
    }
})();
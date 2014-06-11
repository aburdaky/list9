(function (window, angular, undefined) {
    var module = angular.module('List9');

    module.controller('MasterController', ['$scope', 'Api', masterController]);

    function masterController($scope, Api) {
        $scope.viewMode = 'Projects';
    }
}(window, angular));

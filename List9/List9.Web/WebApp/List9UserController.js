(function (window, angular, undefined) {
    var module = angular.module('List9.List9Users',['List9.Core']);


   
    module.controller('List9UserController', ['$rootScope', '$scope', 'Api', '$parse', list9Usercontroller])

    function list9Usercontroller($rootscope, $scope, Api) {

        $scope.users = [];
        $scope.deleteMode = false;
        $scope.selectedUser = null;
        $scope.userClicked = userClicked;

        function userClicked(user) {
            $rootScope.$broadcast('USER_SELECTED', user);
        }


        fetchUser = function () {

            Api.Account.query({}, function (data) {
                $scope.users = data;
            }, {})
        };

        fetchUser();
    }


}(window, angular));
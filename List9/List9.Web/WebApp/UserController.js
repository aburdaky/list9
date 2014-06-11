(function (window, angular, undefined) {
    var module = angular.module('List9.Users', [
        'List9.Core'
    ]);

    module.controller("UserController", ['$scope', 'Api', UserController]);

    function UserController($scope, Api) {

        $scope.users = [];

        $scope.selectedUser = null;



        fetchUser = function () {

            Api.User.query({ $expand: 'Tasks' }, function (data) {
                $scope.users = data;
            }, {})
        }



        fetchUser();

        $scope.save = function () {
            Api.User.save($scope.selectedUser, function (
          ) {
                $scope.editMode = false;
                $scope.selectedUser = null;
                alert('User Created Sucessfully');
                fetchUser();


            }, function () { alert('Error Creating User') })
        }
    }

}(window, angular));
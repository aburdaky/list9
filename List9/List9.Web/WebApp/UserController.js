(function (window, angular, undefined) {
    var module = angular.module('List9.Users', [
        'List9.Core'
    ]);

    module.controller("UserController", ['$rootScope', '$scope', 'Api', '$parse', UserController]);

    function UserController($rootScope,$scope, Api) {

        $scope.users = [];
        $scope.deleteMode = false;
        $scope.selectedUser = null;
        $scope.userClicked = userClicked;

        function userClicked(user) {
            $rootScope.$broadcast('USER_SELECTED', user);
        }


        fetchUser = function () {

            Api.User.query({ $expand: 'Tasks' }, function (data) {
                $scope.users = data;
            }, {})
        };



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

        $scope.saveEdit = function (user, $event) {
            var updateable = angular.copy(user);

            if ($event.keyCode === 13) {
                Api.User.update({ id: user.Id }, updateable, function () {
                    alert('User Updated Sucessfully');
                    fetchUser();
                }, function () {
                    alert('Error Editing User');
                    fetchUser();
                })
            }

        }

        $scope.deleteUser = deleteUser

        function deleteUser(u) {
            

            if (confirm('are you Sure you want to delete user "' + u.Name + '"?')) {

                Api.User.delete({ id: u.Id }, function ()  {
                    $scope.deleteMode = false
                    alert('user Deleted Sucessfully');
                    fetchUser();
                }, function () {
                    alert('Error Deleting user');
                    fetchUser();
                })
            }
        }
    }

}(window, angular));
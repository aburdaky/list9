(function (window, angular, undefined) {
    var module = angular.module('List9.Tasks', [
        'List9.Core'
    ]);

    module.controller("TaskController", ['$scope', 'Api', TaskController]);

    function TaskController($scope, Api) {

        $scope.tasks = [];
        $scope.projects = [];
        $scope.users = []
        $scope.taskcategories=[]

        $scope.selectedTask = null;



        fetchTask = function () {

            Api.Task.query({$expand:'Projects,Users,TaskCategories'}, function (data) {
                $scope.tasks = data;
            }, {})
        }


        Api.Project.query({}, function (data) {
            $scope.projects = data;
        }, {})

        Api.User.query({}, function (data) {
            $scope.users = data;
        }, {})

        Api.TaskCategory.query({}, function (data) {
            $scope.taskcategories = data;
        }, {})

        fetchTask();

        $scope.save = function () {

           

            Api.Task.save($scope.selectedTask, function (
          ) {
                $scope.editMode = false;
                $scope.selectedTask = null;
                alert('Task Created Sucessfully');
                fetchTask();


            }, function () { alert('Error Creating Task') })
        }
    }

}(window, angular));
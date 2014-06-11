(function (window, angular, undefined) {
    var module = angular.module('List9.TaskCategories', [
        'List9.Core'
    ]);

    module.controller("TaskCategoryController", ['$scope', 'Api', TaskCategoryController]);

    function TaskCategoryController($scope, Api) {

        $scope.taskcategories = [];

        $scope.selectedTaskCategory = null;



        fetchTaskCategory = function () {

            Api.TaskCategory.query({ $expand: 'Tasks' }, function (data) {
                $scope.taskcategories = data;
            }, {})
        }





        fetchTaskCategory();

        $scope.save = function () {
            Api.TaskCategory.save($scope.selectedTaskCategory, function (
          ) {
                $scope.editMode = false;
                $scope.selectedTaskCategory = null;
                alert('TaskCategory Created Sucessfully');
                fetchTaskCategory();


            }, function () { alert('Error Creating TaskCategory') })
        }
    }

}(window, angular));
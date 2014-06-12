(function (window, angular, undefined) {
    var module = angular.module('List9.TaskCategories', [
        'List9.Core'
    ]);

    module.controller("TaskCategoryController", ['$rootScope', '$scope', 'Api', TaskCategoryController]);

    function TaskCategoryController($rootScope,$scope, Api) {

        $scope.taskcategories = [];

        $scope.selectedTaskCategory = null;
        $scope.categoryClicked = categoryClicked;

        function categoryClicked(category) {
            $rootScope.$broadcast('CATEGORY_SELECTED', category);
        }


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
    

        $scope.saveEdit = function (TaskCategory, $event) {
            var updateable = angular.copy(TaskCategory);

            if ($event.keyCode === 13) {
                Api.TaskCategory.update({ id: TaskCategory.Id }, updateable, function () {
                    alert('Category Updated Sucessfully');
                    fetchTaskCategory();
                }, function () {
                    alert('Error Editing Category');
                    fetchTaskCategory();
                })
            }
        }
    }
}(window, angular));
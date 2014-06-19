(function (window, angular, undefined) {
    var module = angular.module('List9.TaskCategories', [
        'List9.Core'
    ]);

    module.controller("TaskCategoryController", ['$rootScope', '$scope', 'Api', '$parse', TaskCategoryController]);

    function TaskCategoryController($rootScope,$scope, Api) {

        
        $scope.deleteMode = false;
        $scope.selectedTaskCategory = null;
        $scope.categoryClicked = categoryClicked;
        $scope.startDelete = startDelete;
        $scope.projects = []
        $scope.tasks = []
        $scope.alltaskcategories = [];
        $scope.taskcategories = [];

        function categoryClicked(category) {
            $rootScope.$broadcast('CATEGORY_SELECTED', category);
        }


        fetchTaskCategory = function () {

               Api.Account.current({}, function (data) {
                $scope.projects = data.Projects;
                $scope.tasks = data.Projects.reduce(function (tasks, project) { return tasks.concat(project.Tasks); }, []);
                $scope.alltaskcategories = $scope.tasks.reduce(function (taskcategories, task) { return taskcategories.concat(tasks.TaskCategory); }, [])
            });
                                  $.each($scope.alltaskcategories, function (i, el) {
                if ($.inArray(el, $scope.alltaskcategories) === -1) $scope.alltaskcategories.push(el);
            });
           
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
        
        function startDelete(c) {

            c.$deleting = true;

            $('#projectdelete' + c.Id).focus();
        }

        $scope.deletecategory = deletecategory

        function deletecategory(tc) {
          

            if (confirm('are you Sure you want to delete task "' + tc.Name + '"?')) {

                Api.TaskCategory.delete({ id: tc.Id }, function () {
                    $scope.deleteMode = false
                    alert('Task Deleted Sucessfully');
                    fetchTaskCategory();
                }, function () {
                    alert('Error Deleting Task');
                    fetchTaskCategory();
                })
            }
        }
    }
}(window, angular));
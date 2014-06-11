(function (window, angular, undefined) {
    var module = angular.module('List9.Projects', [
        'List9.Core'
    ]);

    module.controller("ProjectsController", ['$scope', 'Api', ProjectsController]);

    function ProjectsController($scope, Api) {

        $scope.projects = [];

        $scope.selectedProject = null;



       fetchProject = function () {

            Api.Project.query({$expand:'Tasks'}, function (data) {
                $scope.projects = data;
            }, {})
        }



        fetchProject();

        $scope.save = function () {
            Api.Project.save($scope.selectedProject, function (
          ) {
                $scope.editMode = false;
                $scope.selectedProject = null;
                alert('Project Created Sucessfully');
                fetchProject();


            }, function () { alert('Error Creating Project') })
        }
    }

}(window, angular));
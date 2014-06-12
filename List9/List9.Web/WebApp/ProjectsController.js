(function (window, angular, undefined) {
    var module = angular.module('List9.Projects', [
        'List9.Core'
    ]);

    module.controller("ProjectsController", ['$rootScope', '$scope', 'Api', ProjectsController]);

    function ProjectsController($rootScope, $scope, Api) {

        $scope.projects = [];

        $scope.selectedProject = null;
        $scope.projectClicked = projectClicked;

        function projectClicked(project) {
            $rootScope.$broadcast('PROJECT_SELECTED', project);
        }


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


        $scope.saveEdit = function (project,$event) {
            var updateable = angular.copy(project);
            
            if ($event.keyCode === 13) {
                Api.Project.update({ id: project.Id }, updateable, function () {
                    alert('Project Updated Sucessfully');
                    fetchProject();
                }, function () {
                    alert('Error Editing Project');
                    fetchProject();
                })
            }

        }
    }

}(window, angular));
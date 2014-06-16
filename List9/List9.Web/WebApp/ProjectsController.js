(function (window, angular, undefined) {
    var module = angular.module('List9.Projects', [
        'List9.Core'
    ]);

    module.controller("ProjectsController", ['$rootScope', '$scope', 'Api', '$parse', ProjectsController]);

    function ProjectsController($rootScope, $scope, Api) {

        $scope.projects = [];
        $scope.deleteMode = false;
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


        $scope.deleteProject = deleteProject

        function deleteProject(p) {
            if (confirm('are you Sure you want to delete project "' + p.Name + '"?')) {
               
                Api.Project.delete({ id:p.Id }, function () {
                    $scope.deleteMode = false;
                    alert('Project Deleted Sucessfully');
                    fetchProject();
                }, function () {
                    alert('Error Deleting project');
                    fetchProject();
                })
            }
        }
    }

}(window, angular));
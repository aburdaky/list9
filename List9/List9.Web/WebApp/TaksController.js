(function (window, angular, undefined) {
    var module = angular.module('List9.Tasks', [
        'List9.Core'
    ]);

   

    module.controller("TaskController", ['$scope','Api', '$user',  TaskController]);

    function TaskController($scope, Api, $user) {

        $scope.tasks = [];
        $scope.projects = [];
        
        $scope.taskcategories = [];
        $scope.filterHeading = 'All';
        $scope.selectedTask = null;
        $scope.editTask = false;
        $scope.canceledit = canceledit;
        $scope.deleteMode = false;
       // $scope.addUsersToProject = addUsersToProject
        //filters
        $scope.user = [];
        $scope.user = $user;
        $scope.$on('PROJECT_SELECTED', onProjectSelected);
        $scope.$on('USER_SELECTED', onUserSelected);
        $scope.$on('CATEGORY_SELECTED', onCategorySelected);
        $scope.$on('FILTERED_BY_DATES', filterTasksByDate);
        $scope.$on('CLEAR_FILTERS',clearFilters)


        

        fetchTask = function () {
            Api.Account.current({}, function (data) {
                $scope.projects = data.Projects;
               
                $scope.tasks = data.Projects.reduce(function (tasks, project) { return tasks.concat(project.Tasks); }, []);
               
            }, {})
        }
       
       

        Api.TaskCategory.query({}, function (data) {
            $scope.taskcategories = data;
        }, {})

        fetchTask();
        
        $scope.save = function () {
          
            $scope.selectedTask.List9UserId = $user.Id;
           
            $scope.selectedTask.Done = false;

            Api.Task.save($scope.selectedTask, function () {
                $scope.editMode = false;
                $scope.selectedTask = null;
                alert('Task Created Sucessfully');
                fetchTask();
            }, function () { alert('Error Creating Task') })
        }

        $scope.saveEdit = function (task,$event) {
            var updateable = angular.copy(task);
            delete updateable.Projects;
            delete updateable.List9Users;
            delete updateable.TaskCategories;
           
            if ($event.keyCode === 13) {
                Api.Task.update({ id: task.Id }, updateable, function () {
                    alert('Task Updated Sucessfully');
                    fetchTask();
                }, function () {
                    alert('Error Editing Task');
                    fetchTask();
                })
            }
            
        }
        $scope.deleteTask=deleteTask

        function deleteTask(t) {
            
           
            if (confirm('are you Sure you want to delete task "' + t.Name + '"?')) {
                
                Api.Task.delete({ id: t.Id }, function () {
                    $scope.deleteMode=false
                    alert('Task Deleted Sucessfully');
                    fetchTask();
                }, function () {
                    alert('Error Deleting Task');
                    fetchTask();
                })
            }
        }

        function canceledit() { $scope.editTask = false };
//Filters
        function onProjectSelected(event, project) {
            $scope.filterHeading = project.Name;

            Api.Task.query({ $expand: 'Projects,TaskCategories', $filter: 'ProjectId eq ' + project.Id }, function (data) {
                $scope.tasks = data;
            }, {})
        }
        function onUserSelected(event, user) {
            $scope.filterHeading = user.Name;

            Api.Task.query({ $expand: 'Projects,TaskCategories', $filter: 'List9UserId eq ' + user.Id }, function (data) {
                $scope.tasks = data;
            }, {})
        }
        function onCategorySelected(event, category) {
            $scope.filterHeading = category.Name;

            Api.Task.query({ $expand: 'Projects,TaskCategories', $filter: 'TaskCategoryId eq ' + category.Id }, function (data) {
                $scope.tasks = data;
            }, {})
        }

        //function addUsersToProject(u,t,$event) {
        //    if ($event.keyCode === 13) {
        //        Api.

        //        { addusermode = false; }
        //    }


        //}

        function filterTasksByDate(event, selectedDates) {


            $scope.filterHeading= 'Date Range: '+new moment(selectedDates.StartDate).format('L')+' to '+new moment(selectedDates.EndDate).format('L')

            Api.Task.query({ $expand: 'Projects,TaskCategories', $filter: 'DateDue gt datetime\'' + selectedDates.StartDate + '\' and DateDue lt datetime\'' + selectedDates.EndDate+'\'' }, function (data) {
                $scope.tasks = data;
            }, {})
        }
        function clearFilters() {
            $scope.filterHeading = 'All'
            fetchTask()
        }
    }

}(window, angular));
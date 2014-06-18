(function (window, angular, undefined) {
    var module = angular.module('List9');

    module.controller('MasterController', ['$rootScope', '$scope', '$http','$user', 'Api', masterController]);

    function masterController($rootScope, $scope, $http,$user ,Api) {


        $scope.currentuser = {};
        $scope.currentuser = $user;
        $scope.viewMode = 'Projects';
        $scope.clearfilters = clearfilters
        $scope.filterTasksByDate = filterTasksByDate;
        $scope.filterDates = {};
        $scope.logOut=logOut

        function filterTasksByDate(filterstring) {



            if (filterstring == 'Today') {
                $scope.filterDates.StartDate = new moment().startOf('day').format()
                $scope.filterDates.EndDate = new moment().endOf('day').format()

            }
            if (filterstring == 'Next 7 Days') {

                $scope.filterDates.StartDate = new moment().startOf('day').format()
                $scope.filterDates.EndDate = new moment().add('days', 6).endOf('day').format()

            } if (filterstring == 'This Month') {

                $scope.filterDates.StartDate = new moment().startOf('month').format()
                $scope.filterDates.EndDate = new moment().endOf('month').format()
            }



            $rootScope.$broadcast('FILTERED_BY_DATES', $scope.filterDates)
        }

        function clearfilters(all) {


            $rootScope.$broadcast('CLEAR_FILTERS')

        }

        function logOut() {

            return window.sessionStorage.removeItem('ACCESS_TOKEN');
            window.location = '/login.html';
           

        }

        
    }
    
       


}(window, angular));

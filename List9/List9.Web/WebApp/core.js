(function (window, angular, undefined) {
    var module = angular.module('List9.Core', [
        'ngResource',
        'ngRoute',
    ]);

    module.provider('Api', apiProvider);

    function apiProvider() {
        var _endpoint;
        var apiProvider = {
            setEndpoint: setEndpoint,
            $get: ['$resource', '$http', getApiService]
        };

        return apiProvider;

        function setEndpoint(value) {
            _endpoint = value;
        }

        function getApiService($resource, $http) {
            var _standardUpdateProcedure = {
                method: 'PUT',
                params: {
                    id: '@Id'
                }
            };

            var project = $resource(_endpoint + '/project/:id', {}, {
                update: _standardUpdateProcedure
            });

            var user = $resource(_endpoint + '/user/:id', {}, {
                update: _standardUpdateProcedure
            });
            var task = $resource(_endpoint + '/task/:id', {}, {
                update: _standardUpdateProcedure
            });
            var taskCategory = $resource(_endpoint + '/taskcategory/:id', {}, {
                update: _standardUpdateProcedure
            });

            var apiService = {
                Project: project,
                User: user,
                Task: task,
                TaskCategory: taskCategory
            }

            return apiService;
        }
    }
}(window, angular));
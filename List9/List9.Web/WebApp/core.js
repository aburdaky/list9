(function (window, angular, undefined) {
    var module = angular.module('List9.Core', [
        'ngResource',
        'ngRoute'
    ]);
    module.constant('StoredAccessToken', {
        get: function () {
            return window.sessionStorage.getItem('ACCESS_TOKEN') || window.localStorage.getItem('ACCESS_TOKEN');
        },
        put: function (token) {
            return window.sessionStorage.setItem('ACCESS_TOKEN', token);
        },
        clear: function () {
            return window.sessionStorage.removeItem('ACCESS_TOKEN');
        },
    });


    module.service('RequestInterceptor', ['$rootScope', '$q', '$window', 'StoredAccessToken', requestInterceptor]);

    function requestInterceptor($rootScope, $q, $window, StoredAccessToken) {
        return {
            request: function (config) {
                config.headers = config.headers || {};
                var token = StoredAccessToken.get();
                if (token) {
                    config.headers.Authorization = "Bearer " + token;
                }
                return config || $q.when(config);
            },
            responseError: function (response) {
                if (response.status === 401) {
                    window.location = "/login.html";
                    return;
                }
                return response || $q.reject(response);
            }
        }
    }

    module.provider('Api', apiProvider);
    module.directive('ngRightClick', ['$parse', rightClickDirective]);
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
            var account = $resource(_endpoint + '/account/:id', {}, {
                update: _standardUpdateProcedure,
                current: {
                    url: _endpoint + '/account/CurrentUser',
                    isArray: false,
                    method: 'GET'
                },
                PostNewUser: {
                    url: _endpoint + '/account/PostNewUser',
                    isArray: false,
                    method: 'POST'
                }
            });

            var apiService = {
                Project: project,
                User: user,
                Task: task,
                TaskCategory: taskCategory,
                Account: account
            }

            return apiService;
        }
    }

    function rightClickDirective($parse) {
        return function (scope, element, attrs) {
            var fn = $parse(attrs.ngRightClick);
            element.bind('contextmenu', function (event) {
                scope.$apply(function () {
                    event.preventDefault();
                    fn(scope, { $event: event });
                });
            });
        };
    }
  
    
}(window, angular));
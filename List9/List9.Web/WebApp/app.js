(function (window, angular, undefined) {
    var module = angular.module('List9', [
        'ngResource',
        'ngRoute',

        'List9.Core',
        'List9.Projects',
        'List9.Users',
        'List9.TaskCategories',
        'List9.Tasks',
        'List9.List9Users'
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
    module.config(['$routeProvider', '$httpProvider', 'ApiProvider', configFunction]);
    
    function configFunction($routeProvider, $httpProvider,  ApiProvider) {
        $routeProvider
            .when('/index', {
                templateUrl: '/index.html',
                controller: 'MasterController'
            })
            .otherwise({
                redirectTo: '/index'
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


        $httpProvider.interceptors.push('RequestInterceptor');

       
        ApiProvider.setEndpoint("http://list9.testpad.e9server.com/api");
        //ApiProvider.setEndpoint("http://localhost:56367/api");
    }

   


}(window, angular));
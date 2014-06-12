(function (window, angular, undefined) {
    var module = angular.module('List9', [
        'ngResource',
        'ngRoute',

        'List9.Core',
        'List9.Projects',
        'List9.Users',
        'List9.TaskCategories',
        'List9.Tasks'
    ]);

    module.config(['$routeProvider', 'ApiProvider', configFunction]);
    
    function configFunction($routeProvider, ApiProvider) {
        $routeProvider
            .when('/index', {
                templateUrl: '/index.html',
                controller: 'MasterController'
            })
            .otherwise({
                redirectTo: '/index'
            });

        ApiProvider.setEndpoint("http://list9.testpad.e9server.com/api");
        //ApiProvider.setEndpoint("http://localhost:56367/api");
    }

   


}(window, angular));
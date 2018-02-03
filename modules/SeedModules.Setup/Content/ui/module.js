define([
    'app.application'
], function (application) {
    'use strict';

    application.requires.push('modules.setup');

    return angular
        .module('modules.setup', [
            'ui.router'
        ])
        .config([
            '$stateProvider',
            '$urlRouterProvider',
            function ($stateProvider, $urlRouterProvider) {
                $urlRouterProvider.otherwise('/setup');

                $stateProvider.state('setup', {
                    url: '/setup',
                    templateUrl: 'SeedModules.Setup/ui/views/form.html',
                    requires: ['SeedModules.Setup/ui/requires']
                });
            }
        ]);
});
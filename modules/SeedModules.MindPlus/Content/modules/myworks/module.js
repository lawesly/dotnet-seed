define(['app/application'], function(application) {
  'use strict';

  application.requires.push('modules.mindPlus.myworks');

  return angular.module('modules.mindPlus.myworks', ['ui.router']).config([
    '$stateProvider',
    '$urlRouterProvider',
    function($stateProvider, $urlRouterProvider) {
      $urlRouterProvider.otherwise('/home/dashboard');

      $stateProvider.state('home', {
        url: '/home',
        templateUrl: '/SeedModules.MindPlus/modules/myworks/views/home.html',
        requires: [
          'SeedModules.AngularUI/modules/requires',
          'SeedModules.MindPlus/modules/myworks/requires'
        ]
      });

      $stateProvider.state('home.dashboard', {
        url: '/dashboard',
        templateUrl:
          '/SeedModules.MindPlus/modules/myworks/views/dashboard.html',
        requires: [
          'SeedModules.AngularUI/modules/requires',
          'SeedModules.MindPlus/modules/myworks/requires'
        ]
      });

      $stateProvider.state('home.works', {
        url: '/works',
        templateUrl:
          '/SeedModules.MindPlus/modules/myworks/components/work/worklist.html',
        requires: [
          'SeedModules.AngularUI/modules/requires',
          'SeedModules.MindPlus/modules/myworks/requires'
        ]
      });

      $stateProvider.state('home.trash', {
        url: '/trash',
        templateUrl: '/SeedModules.MindPlus/modules/myworks/views/trash.html',
        requires: [
          'SeedModules.AngularUI/modules/requires',
          'SeedModules.MindPlus/modules/myworks/requires'
        ]
      });

      // old
      $stateProvider.state('mymind', {
        url: '/mymind',
        templateUrl: '/SeedModules.MindPlus/modules/myworks/views/mymind.html',
        requires: [
          'SeedModules.AngularUI/modules/requires',
          'SeedModules.MindPlus/modules/myworks/requires'
        ]
      });

      $stateProvider.state('mymind.works', {
        url: '/works/{parentid}',
        templateUrl: '/SeedModules.MindPlus/modules/myworks/views/works.html',
        requires: [
          'SeedModules.AngularUI/modules/requires',
          'SeedModules.MindPlus/modules/myworks/requires'
        ]
      });

      $stateProvider.state('workspace', {
        url: '/workspace/{id}',
        templateUrl:
          '/SeedModules.MindPlus/modules/myworks/views/workspace.html',
        requires: [
          'SeedModules.AngularUI/modules/requires',
          'SeedModules.MindPlus/modules/myworks/requires'
        ]
      });

      $stateProvider.state('workspace.list', {
        url: '/list',
        templateUrl:
          '/SeedModules.MindPlus/modules/myworks/views/workItems.html',
        requires: [
          'SeedModules.AngularUI/modules/requires',
          'SeedModules.MindPlus/modules/myworks/requires'
        ]
      });

      $stateProvider.state('workspace.settings', {
        url: '/settings',
        templateUrl:
          '/SeedModules.MindPlus/modules/myworks/views/settings.html',
        requires: [
          'SeedModules.AngularUI/modules/requires',
          'SeedModules.MindPlus/modules/myworks/requires'
        ]
      });

      $stateProvider.state('workspace.tags', {
        url: '/tags',
        templateUrl: '/SeedModules.MindPlus/modules/myworks/views/tags.html',
        requires: [
          'SeedModules.AngularUI/modules/requires',
          'SeedModules.MindPlus/modules/myworks/requires'
        ]
      });
    }
  ]);
});

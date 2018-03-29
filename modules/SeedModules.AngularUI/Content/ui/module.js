define([
  'app/application',
  'SeedModules.AngularUI/ui/configs/httpConfig',
  'SeedModules.AngularUI/ui/configs/location',
  'SeedModules.AngularUI/ui/configs/ngTableDefaults',
  'SeedModules.AngularUI/ui/configs/ngTableTemplates',
  'SeedModules.AngularUI/ui/configs/schemaFormDefaults',
  'SeedModules.AngularUI/ui/configs/schemaForm',
  'SeedModules.AngularUI/ui/configs/form/simplecolor',
  'SeedModules.AngularUI/ui/providers/ngTableDefaultGetData'
], function(application) {
  'use strict';

  application.requires.push('modules.angularui');

  return angular
    .module('modules.angularui', [
      'modules.angularui.configs',
      'modules.angularui.providers'
    ])
    .config([
      '$provide',
      '$appConfig',
      function($provide, $appConfig) {
        var settings = JSON.parse(
          document.getElementById('seed-ui').getAttribute('data-site')
        );
        settings.prefix = settings.prefix ? '/' + settings.prefix : '';

        $appConfig.siteSettings = settings;
      }
    ]);
});

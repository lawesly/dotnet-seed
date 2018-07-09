import angular = require('angular');
import 'app/application';
import 'angular-ui-router';
import 'schema-form-bootstrap';

let instance: ng.IModule = angular.module('modules.admin.boot', [
  'ui.router',
  'schemaForm'
]);

export = instance;

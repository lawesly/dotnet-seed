define(['SeedModules.AngularUI/ui/module'], function(module) {
  'use strict';

  module.factory('SeedModules.AngularUI/ui/factories/schemaFormParams', [
    'SeedModules.AngularUI/ui/configs/schemaFormDefaults',
    function(schemaFormDefaults) {
      var schemaFormParams = function(baseSchema, baseOptions) {
        var self = this;
        var formSchema = {
          type: 'object',
          properties: {},
          required: []
        };

        angular.extend(formSchema, schemaFormDefaults.schema);

        var options = {};

        this.options = function(newOptions) {
          if (angular.isDefined(newOptions)) {
            angular.extend(options, newOptions);
            return self;
          }
          return options;
        };

        this.schema = function(newSchema) {
          if (angular.isDefined(newSchema)) {
            formSchema.type = newSchema.type || 'object';
            formSchema.properties = newSchema.properties || {};
            formSchema.required = newSchema.required || [];
            return self;
          }
          return formSchema;
        };

        this.properties = function(propertiesDefine) {
          var currentSchema = this.schema();
          if (angular.isDefined(propertiesDefine)) {
            currentSchema.properties = propertiesDefine;
            angular.forEach(currentSchema.properties, function(item, key) {
              self.required(key, item.required);
            });
            return self;
          }
          return currentSchema.properties;
        };

        this.property = function(propertyName, propertyDefine) {
          var currentSchema = this.schema();
          if (angular.isDefined(propertyDefine)) {
            currentSchema.properties[propertyName] = propertyDefine;
            self.required(propertyName, propertyDefine.required);
            return self;
          }
          return currentSchema.properties[propertyName];
        };

        this.required = function(propertyName, req) {
          var currentSchema = this.schema();
          var requiredIndex = currentSchema.required.indexOf(propertyName);
          if (req && requiredIndex < 0) {
            currentSchema.required.push(propertyName);
          } else if (requiredIndex >= 0) {
            currentSchema.required.splice(requiredIndex, 1);
          }
          return self;
        };

        this.options(schemaFormDefaults.options);
        this.options(baseOptions);
        this.schema(baseSchema);

        return this;
      };

      return schemaFormParams;
    }
  ]);
});

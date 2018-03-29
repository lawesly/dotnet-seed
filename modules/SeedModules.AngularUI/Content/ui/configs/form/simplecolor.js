define(['SeedModules.AngularUI/ui/configs', 'schema-form-bootstrap'], function(
  configs
) {
  'use strict';

  angular.module('schemaForm').config([
    'schemaFormDecoratorsProvider',
    'schemaFormProvider',
    'sfBuilderProvider',
    'sfPathProvider',
    function(
      schemaFormDecoratorsProvider,
      schemaFormProvider,
      sfBuilderProvider,
      sfPathProvider
    ) {
      var base = '/SeedModules.AngularUI/ui/templates/';

      var simplecolor = function(name, schema, options) {
        if (schema.type === 'string' && schema.format == 'html') {
          var f = schemaFormProvider.stdFormObj(name, schema, options);
          f.key = options.path;
          f.type = 'simplecolor';
          options.lookup[sfPathProvider.stringify(options.path)] = f;
          return f;
        }
      };

      schemaFormProvider.defaults.string.unshift(simplecolor);

      schemaFormDecoratorsProvider.addMapping(
        'bootstrapDecorator',
        'simplecolor',
        base + 'simplecolor.html'
      );

      schemaFormDecoratorsProvider.createDirective(
        'simplecolor',
        base + 'simplecolor.html'
      );
    }
  ]);
});

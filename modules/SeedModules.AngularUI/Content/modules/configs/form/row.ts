import boot = require('SeedModules.AngularUI/modules/boot');
import angular = require('angular');
import { ExtendFormFields } from 'SeedModules.AngularUI/modules/configs/enums/extendFormFields';

var base = '/SeedModules.AngularUI/modules/templates/form/';

class RowConfig {
  static $inject = [
    'schemaFormDecoratorsProvider',
    'schemaFormProvider',
    'sfPathProvider',
    'sfBuilderProvider'
  ];
  constructor(
    schemaFormDecoratorsProvider: AngularUI.SchemaForm.ISchemaFormDecoratorsProvider,
    schemaFormProvider: AngularUI.SchemaForm.ISchemaFormProvider,
    sfPathProvider: AngularUI.SchemaForm.ISfPathProvider,
    sfBuilderProvider: AngularUI.SchemaForm.ISfBuilderProvider
  ) {
    schemaFormDecoratorsProvider.defineAddOn(
      'bootstrapDecorator',
      ExtendFormFields.row,
      base + 'row.html',
      [
        sfBuilderProvider.builders.sfField,
        sfBuilderProvider.builders.ngModelOptions,
        sfBuilderProvider.builders.condition,
        sfBuilderProvider.builders.transclusion
      ]
    );
  }
}

boot.config(RowConfig).run([
  '$templateCache',
  ($templateCache: ng.ITemplateCacheService) => {
    $templateCache.put(
      '/SeedModules.AngularUI/modules/templates/form/row.html',
      '<div class="row" sf-field-transclude="columns"></div>'
    );
  }
]);

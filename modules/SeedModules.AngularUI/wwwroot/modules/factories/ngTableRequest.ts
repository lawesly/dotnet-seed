define(['SeedModules.AngularUI/modules/module'], function(module) {
  'use strict';

  module.factory('SeedModules.AngularUI/modules/factories/ngTableRequest', [
    '$location',
    'SeedModules.AngularUI/modules/factories/ngTableParams',
    'SeedModules.AngularUI/modules/services/requestService',
    function($location, ngTableParams, requestService) {
      function getData(params, requestOptions) {
        var query = $location.search(requestOptions.url);
        var url = requestOptions.url.split(/[&?]/)[0];

        query.page = params.page();
        query.count = params.count();
        var queryArray = [];
        for (var n in query) {
          queryArray.push(n + '=' + query[n]);
        }
        var urlString = [url, queryArray.join('&')].join('?');
        return requestService
          .url(urlString)
          .options(requestOptions)
          .post($.extend({}, requestOptions.data))
          .result.then(
            function(result) {
              if (result && result.total) params.total(result.total);
              return result.list;
            },
            function() {
              return [];
            }
          );
      }

      return function(initOptions) {
        var self = this;
        var options = {};

        this.options = function(newOptions) {
          if (angular.isDefined(newOptions)) {
            angular.extend(options, newOptions);
          }
          return self;
        };

        this.ngTableParams = function(newParams, newSettings) {
          return new ngTableParams(
            newParams,
            $.extend(newSettings, {
              getData: function(params) {
                return getData(params, options);
              }
            })
          );
        };

        this.options(initOptions);

        return this;
      };
    }
  ]);
});

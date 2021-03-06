define(["require", "exports", "SeedModules.AngularUI/modules/module", "SeedModules.AngularUI/js/jquery-form/jquery.form.min"], function (require, exports, mod) {
    "use strict";
    exports.__esModule = true;
    function ajaxFormDirective($q, $modal, $appConfig, httpDataHandler) {
        return {
            restrict: 'AE',
            scope: {
                ajaxForm: '='
            },
            link: function (scope, element, attr, ctrl) {
                scope.ajaxForm = $.extend({
                    type: 'POST',
                    showLoading: true
                }, scope.ajaxForm);
                scope.ajaxForm.submit = function (options) {
                    var defer = $q.defer();
                    var loading = $modal.open({
                        templateUrl: '/SeedModules.AngularUI/modules/views/Loading.html',
                        size: 'sm'
                    });
                    var submitOptions = options
                        ? $.extend(scope.ajaxForm, options)
                        : scope.ajaxForm;
                    submitOptions.url = $appConfig.siteSettings.prefix + submitOptions.url;
                    submitOptions.success = function (responseText, statusText, xhr, form) {
                        httpDataHandler.doResponse({
                            data: '',
                            config: {
                                dataOnly: true,
                                url: $appConfig.siteSettings.prefix + scope.ajaxForm.url
                            }
                        }, defer);
                        loading.close();
                    };
                    submitOptions.error = function (response, statusText, responseText, form) {
                        httpDataHandler.doError({
                            data: '',
                            statusText: responseText,
                            config: {
                                dataOnly: true,
                                url: scope.ajaxForm.url
                            }
                        }, defer);
                        loading.close();
                    };
                    $(element)['ajaxSubmit'](submitOptions);
                    return defer.promise;
                };
            }
        };
    }
    ajaxFormDirective.$inject = [
        '$q',
        '$modal',
        '$appConfig',
        'app/factories/httpDataHandler'
    ];
    mod.directive('ajaxForm', ajaxFormDirective);
});
//# sourceMappingURL=ajaxForm.js.map
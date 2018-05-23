'use strict';
(function (options) {
    var configs = {
        urlArgs: options.urlArgs,
        baseUrl: '/SeedModules.AngularUI/../',
        paths: {},
        shim: {},
        map: options.map
    };
    var references = options['configs'];
    var requires = [];
    for (var name in references) {
        var reference = references[name];
        var referenceType = Object.prototype.toString.call(reference);
        if (referenceType === '[object Object]') {
            configs.paths[name] = reference.path;
            if (reference.shim)
                configs.shim[name] = reference.shim;
            if (reference.required) {
                requires.push(name);
            }
        }
        else if (referenceType === '[object String]') {
            configs.paths[name] = reference;
        }
    }
    if (document.getElementsByTagName('html')[0].getAttribute('data-html-type') ===
        'no-js lte-ie8') {
        for (var path in configs.paths) {
            configs.shim[path] = configs.shim[path] || {};
            configs.shim[path].deps = configs.shim[path].deps || {};
            configs.shim[path].deps = configs.shim[path].deps.concat(options.patchs);
        }
    }
    require.config(configs);
    require(requires.concat(options['requires']), function () {
        angular.element(document).ready(function () {
            angular.bootstrap(document, [options.application]);
            angular
                .element(document)
                .find('html')
                .addClass('ng-app');
        });
    });
})(eval('(' + document.getElementById('seed-ui').getAttribute('data-options') + ')'));
//# sourceMappingURL=startup.js.map
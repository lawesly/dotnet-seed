define(["require", "exports", "angular", "SeedModules.MindPlus/modules/admin/configs/menus"], function (require, exports, angular) {
    "use strict";
    return angular
        .module('modules.mindPlus.admin', [
        'ui.router',
        'modules.mindPlus.admin.configs'
    ])
        .config([
        '$stateProvider',
        function ($stateProvider) {
            $stateProvider.state('admin.mindsettings', {
                url: '/mindsettings',
                title: '文档服务管理',
                templateUrl: '/SeedModules.MindPlus/modules/admin/views/mindsettings.html',
                requires: [
                    'SeedModules.AngularUI/modules/requires',
                    'SeedModules.MindPlus/modules/admin/requires'
                ]
            });
        }
    ]);
});
//# sourceMappingURL=module.js.map
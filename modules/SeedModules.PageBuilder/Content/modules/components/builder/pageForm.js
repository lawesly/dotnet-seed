define(["require", "exports", "SeedModules.PageBuilder/modules/module", "SeedModules.AngularUI/modules/configs/enums/schemaTypes", "SeedModules.AngularUI/modules/configs/enums/defaultFormTypes", "SeedModules.AngularUI/modules/configs/enums/extendFormFields", "rcss!/SeedModules.PageBuilder/css/page-builder.css"], function (require, exports, mod, schemaTypes_1, defaultFormTypes_1, extendFormFields_1) {
    "use strict";
    exports.__esModule = true;
    var PageFormClass = (function () {
        function PageFormClass($scope, $rootScope, $state, $modal) {
            this.$scope = $scope;
            this.$rootScope = $rootScope;
            this.$state = $state;
            this.$modal = $modal;
            $scope.editor = {
                form: [
                    {
                        type: extendFormFields_1.ExtendFormFields.panel,
                        title: 'aaaaaaaaa',
                        theme: 'success',
                        items: [
                            {
                                type: extendFormFields_1.ExtendFormFields.row,
                                items: [
                                    {
                                        type: extendFormFields_1.ExtendFormFields.column,
                                        flex: 6,
                                        items: [
                                            {
                                                key: 'source1.lastName',
                                                type: defaultFormTypes_1.DefaultFormTypes.text,
                                                title: '姓'
                                            }
                                        ]
                                    },
                                    {
                                        type: extendFormFields_1.ExtendFormFields.column,
                                        flex: 6,
                                        items: [
                                            {
                                                key: 'source1.firstName',
                                                type: defaultFormTypes_1.DefaultFormTypes.text,
                                                title: '名'
                                            }
                                        ]
                                    }
                                ]
                            },
                            {
                                key: 'source1.description',
                                type: defaultFormTypes_1.DefaultFormTypes.text,
                                title: '描述'
                            }
                        ]
                    },
                    {
                        type: extendFormFields_1.ExtendFormFields.panel,
                        title: '面板2',
                        theme: 'info',
                        items: [
                            {
                                key: 'category',
                                type: 'select',
                                title: '分类'
                            }
                        ]
                    }
                ],
                schema: {
                    type: schemaTypes_1.SchemaTypes.object,
                    properties: {
                        source1: { type: 'object' }
                    }
                },
                options: {},
                model: {}
            };
            this.$scope.form = {
                form: [
                    {
                        key: 'pagename',
                        title: '页面名称',
                        placeholder: '输入页面名称'
                    }
                ],
                schema: {
                    type: schemaTypes_1.SchemaTypes.object,
                    properties: {
                        pagename: {
                            type: 'string',
                            required: true
                        }
                    }
                },
                options: {},
                model: {}
            };
        }
        PageFormClass.prototype.back = function () {
            this.$state.go('admin.pagebuilder_page');
        };
        PageFormClass.prototype.preview = function () {
            this.$modal.open({
                templateUrl: '/SeedModules.PageBuilder/modules/components/builder/preview.html',
                size: 'lg',
                scope: this.$scope
            });
        };
        PageFormClass.$inject = ['$scope', '$rootScope', '$state', '$modal'];
        return PageFormClass;
    }());
    mod.controller('SeedModules.PageBuilder/modules/components/builder/pageForm', PageFormClass);
});
//# sourceMappingURL=pageForm.js.map
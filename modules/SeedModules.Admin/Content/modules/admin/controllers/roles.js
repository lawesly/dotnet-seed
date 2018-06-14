define(['SeedModules.Admin/modules/admin/module'], function (module) {
    'use strict';
    module.controller('SeedModules.Admin/modules/admin/controllers/roles', [
        '$scope',
        '$modal',
        'app/services/popupService',
        'SeedModules.AngularUI/modules/services/requestService',
        'SeedModules.AngularUI/modules/factories/ngTableRequest',
        'SeedModules.AngularUI/modules/factories/schemaFormParams',
        function ($scope, $modal, popupService, requestService, ngTableRequest, schemaFormParams) {
            $scope.roles = [];
            $scope.currentRole = null;
            $scope.tableParams = null;
            $scope.tableRequest = new ngTableRequest({
                showLoading: false
            });
            $scope.checkedAll = false;
            $scope.checked = {};
            $scope.permissions = [];
            $scope.roleEnables = [];
            $scope.checkAll = function () {
                $scope.checkedAll = !$scope.checkedAll;
                if ($scope.checkedAll) {
                    $.each($scope.tableParams.data, function (idx, item) {
                        $scope.checked[item.id] = true;
                    });
                }
                else {
                    $.each($scope.tableParams.data, function (idx, item) {
                        $scope.checked[item.id] = false;
                    });
                }
            };
            $scope.onCheck = function () {
                $scope.checkedAll = true;
                $.each($scope.tableParams.data, function (idx, item) {
                    if (!$scope.checked[item.id]) {
                        $scope.checkedAll = false;
                        return false;
                    }
                });
            };
            $scope.roleForm = new schemaFormParams().properties({
                rolename: {
                    title: '名称',
                    type: 'string',
                    required: true
                }
            });
            $scope.loadRoles = function () {
                requestService
                    .url('/api/admin/roles')
                    .options({
                    showLoading: false
                })
                    .get()
                    .result.then(function (result) {
                    $scope.roles = result;
                });
            };
            $scope.selectRole = function (role) {
                $scope.currentRole = role;
            };
            $scope.cancelEditing = function () {
                $scope.currentRole = null;
            };
            $scope.create = function () {
                $modal
                    .open({
                    templateUrl: '/SeedModules.AngularUI/modules/views/schemaConfirm.html',
                    size: 'sm',
                    data: {
                        title: '新建角色',
                        formParams: $scope.roleForm,
                        form: ['rolename']
                    }
                })
                    .result.then(function (data) {
                    requestService
                        .url('/api/admin/roles')
                        .post(data)
                        .result.then(function (result) {
                        $scope.loadRoles();
                    });
                });
            };
            $scope.drop = function () {
                popupService.confirm('是否删除角色？').ok(function () {
                    requestService
                        .url('/api/admin/roles/' + $scope.currentRole.id)
                        .drop()
                        .result.then(function (result) {
                        $scope.currentRole = null;
                        $scope.loadRoles();
                    });
                });
            };
            $scope.setName = function (role) {
                $modal
                    .open({
                    templateUrl: '/SeedModules.AngularUI/modules/views/schemaConfirm.html',
                    size: 'sm',
                    data: {
                        title: '设置别名',
                        formParams: new schemaFormParams().properties({
                            displayName: {
                                title: '别名',
                                type: 'string'
                            }
                        }),
                        form: ['displayName']
                    }
                })
                    .result.then(function (data) {
                    requestService
                        .url('/api/admin/roles/' +
                        role.id +
                        '/displayname?name=' +
                        (data.displayName || ''))
                        .patch()
                        .result.then(function (result) {
                        $scope.loadRoles();
                    });
                });
            };
            $scope.addMember = function () {
                if (!$scope.currentRole)
                    return;
                $modal
                    .open({
                    templateUrl: '/SeedModules.Admin/modules/admin/views/members.html',
                    size: 'lg',
                    data: {
                        role: $scope.currentRole
                    }
                })
                    .result.then(function (data) {
                    var postdata = [];
                    $.each(data, function (idx, item) {
                        if (item) {
                            postdata.push(idx);
                        }
                    });
                    if (postdata.length <= 0)
                        return;
                    requestService
                        .url('/api/admin/roles/' + $scope.currentRole.id + '/members')
                        .post({
                        members: postdata
                    })
                        .result.then(function (result) {
                        if ($scope.tableParams) {
                            $scope.tableParams.reload();
                        }
                    });
                });
            };
            $scope.removeMember = function (user) {
                if (!$scope.currentRole)
                    return;
                popupService.confirm('是否删除成员？').ok(function () {
                    var postdata = [];
                    if (user) {
                        postdata.push(user.id);
                    }
                    else {
                        $.each($scope.checked, function (idx, item) {
                            if (item) {
                                postdata.push(idx);
                            }
                        });
                    }
                    if (postdata.length <= 0)
                        return;
                    requestService
                        .url('/api/admin/roles/' + $scope.currentRole.id + '/members')
                        .patch({
                        members: postdata
                    })
                        .result.then(function (result) {
                        if ($scope.tableParams) {
                            $scope.tableParams.reload();
                        }
                    });
                });
            };
            $scope.loadRoleDetails = function (role) {
                $scope.checkedAll = false;
                $scope.checked = {};
                if (!role)
                    return;
                $scope.tableParams = $scope.tableRequest
                    .options({
                    url: '/api/admin/roles/' + role.id + '/members/query'
                })
                    .ngTableParams();
            };
            $scope.loadRolePermissions = function (role) {
                $scope.permissions = [];
                $scope.roleEnables = [];
                if (!role)
                    return;
                requestService
                    .url('/api/admin/roles/' + role.id + '/permission')
                    .options({ showLoading: false })
                    .get()
                    .result.then(function (result) {
                    $scope.permissions = result.permissions;
                    $scope.roleEnables = result.enables;
                });
            };
            $scope.hasPermission = function (per) {
                if ($scope.roleEnables) {
                    return $scope.roleEnables.indexOf(per.name) >= 0;
                }
                else {
                    return false;
                }
            };
            $scope.permissionChanged = function (per) {
                var idx = $scope.roleEnables.indexOf(per.name);
                if (idx >= 0) {
                    $scope.roleEnables.splice(idx, 1);
                }
                else {
                    $scope.roleEnables.push(per.name);
                }
            };
            $scope.isAllPermission = function (defd) {
                var pers = $scope.permissions[defd];
                for (var i = 0; i < pers.length; i++) {
                    if ($scope.roleEnables.indexOf(pers[i].name) < 0) {
                        return false;
                    }
                }
                return true;
            };
            $scope.changeAllPermission = function (defd) {
                var isAll = $scope.isAllPermission(defd);
                var pers = $scope.permissions[defd];
                if (!isAll) {
                    for (var i = 0; i < pers.length; i++) {
                        if ($scope.roleEnables.indexOf(pers[i].name) < 0) {
                            $scope.roleEnables.push(pers[i].name);
                        }
                    }
                }
                else {
                    for (var i = 0; i < pers.length; i++) {
                        var idx = $scope.roleEnables.indexOf(pers[i].name);
                        if (idx >= 0) {
                            $scope.roleEnables.splice(idx, 1);
                        }
                    }
                }
            };
            $scope.saveRolePermission = function () {
                if (!$scope.currentRole)
                    return;
                requestService
                    .url('/api/admin/roles/' + $scope.currentRole.id + '/permission')
                    .put($scope.roleEnables);
            };
            $scope.$watch(function () {
                return $scope.currentRole;
            }, function (val) {
                $scope.loadRoleDetails(val);
                $scope.loadRolePermissions(val);
            });
        }
    ]);
});
//# sourceMappingURL=roles.js.map
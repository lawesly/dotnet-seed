import boot = require('SeedModules.PageBuilder/modules/boot');

class ConfigRouteClass {
  static $inject = ['$stateProvider'];
  constructor($stateProvider: app.configs.IRequireStateProvider) {
    $stateProvider.state('admin.pagebuilder_page', {
      url: '/pagebuilder_page',
      title: '页面管理',
      templateUrl:
        '/SeedModules.PageBuilder/modules/components/builder/page.html',
      requires: [
        'SeedModules.AngularUI/modules/requires',
        'SeedModules.PageBuilder/modules/requires'
      ]
    });

    $stateProvider.state('admin.pagebuilder_pageform', {
      url: '/pagebuilder_pageform/{id}',
      title: '页面编辑',
      templateUrl:
        '/SeedModules.PageBuilder/modules/components/builder/pageForm.html',
      requires: [
        'SeedModules.AngularUI/modules/requires',
        'SeedModules.PageBuilder/modules/requires'
      ]
    });

    $stateProvider.state('admin.pagebuilder_datasource', {
      url: '/pagebuilder_datasource',
      title: '数据源管理',
      templateUrl:
        '/SeedModules.PageBuilder/modules/components/datasource/list.html',
      requires: [
        'SeedModules.AngularUI/modules/requires',
        'SeedModules.PageBuilder/modules/requires'
      ]
    });

    //pagebuilder_dsform
    $stateProvider.state('admin.pagebuilder_dsform', {
      url: '/pagebuilder_dsform',
      title: '数据源编辑',
      templateUrl:
        '/SeedModules.PageBuilder/modules/components/datasource/form.html',
      requires: [
        'SeedModules.AngularUI/modules/requires',
        'SeedModules.PageBuilder/modules/requires'
      ]
    });
  }
}

class RunClass {
  static $inject = ['$state', 'SeedModules.Admin/modules/admin/configs/nav'];
  constructor($state, nav) {
    nav.add({
      text: '定制管理',
      icon: 'fab fa-fort-awesome fa-fw',
      order: 5,
      children: [
        // {
        //   text: '服务设置',
        //   itemClicked: evt => {
        //     $state.go('admin.pagebuilder_serversettings');
        //   }
        // },
        {
          text: '页面管理',
          itemClicked: evt => {
            $state.go('admin.pagebuilder_page');
          }
        },
        {
          text: '数据源管理',
          itemClicked: evt => {
            $state.go('admin.pagebuilder_datasource');
          }
        }
      ]
    });
  }
}

boot.config(ConfigRouteClass).run(RunClass);

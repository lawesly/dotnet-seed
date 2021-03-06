import mod = require('SeedModules.MindPlus/modules/portals/module');
import { SchemaTypes } from 'SeedModules.AngularUI/modules/configs/enums/schemaTypes';
import { DataTypes } from 'SeedModules.AngularUI/modules/configs/enums/dataTypes';
import { DefaultFormTypes } from 'SeedModules.AngularUI/modules/configs/enums/defaultFormTypes';

interface ICurrentScope extends ng.IScope {
  vm: ControllerClass;
  sfform: AngularUI.SchemaForm.ISchemaForm;
}

class ControllerClass {
  regist() {
    this.popupService.information('我就晚上有时间，辣么多功能得一点点来呀');
  }

  static $inject = ['$scope', '$appConfig', 'app/services/popupService'];
  constructor(
    private $scope: ICurrentScope,
    private $appConfig,
    private popupService: app.services.IPopupService
  ) {
    $scope.vm = this;
    $scope.sfform = {
      form: [
        {
          key: 'username',
          placeholder: '请输入用户名/手机号'
        },
        {
          key: 'password',
          type: DefaultFormTypes.password,
          placeholder: '请输入密码'
        },
        {
          key: 'passwordConfirm',
          type: DefaultFormTypes.password,
          placeholder: '请输入确认密码'
        }
      ],
      model: {},
      options: {},
      schema: {
        type: SchemaTypes.object,
        properties: {
          username: {
            type: DataTypes.string,
            title: '用户名',
            required: true
          },
          password: { type: DataTypes.string, title: '密码', required: true },
          passwordConfirm: {
            type: DataTypes.string,
            title: '密码确认',
            required: true
          }
        }
      }
    };
  }
}

mod.controller(
  'SeedModules.MindPlus/modules/portals/controllers/register',
  ControllerClass
);

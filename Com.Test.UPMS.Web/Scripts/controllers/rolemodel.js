/// <reference path="../common.js" />
/// <reference path="../app.js" />

'use strict';
//var app = angular.module('myApp', ['ui.bootstrap']);

var apiUrl = "/admin/RoleModel/";
var apiBUrl = "api/ModelRole/";
app.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.html5Mode(true);
}]);

app.controller('roleModelController', ['$scope', '$filter', '$location', 'apiHelper', function ($scope, $filter, $location, apiHelper) {
    if ($location.search().sid) {
        $scope.SystemId = $location.search().sid;
    }

    $scope.loadRole = function () {
        if ($location.search().sid) {
            $scope.SystemId = $location.search().sid;
        }
        var params = {
            SystemId: $scope.SystemId
        };
        apiHelper.callService(apiUrl + "GetRoleModel", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.dataRoleList = data.data.list;
            } else {
                showMsg.error("提示", data.error);
            }
        });
    }

    $scope.setRole = function (id) {
        $scope.rid = id; //SetModelButtonInfo
        if ($location.search().sid) {
            $scope.SystemId = $location.search().sid;
        }

        var params = {
            SystemId: $scope.SystemId, RoleId: $scope.rid
        };
        apiHelper.callService(apiUrl + "SetModelButtonInfo", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.dataModelButtonList = data.data.list;
            } else {
                showMsg.error("提示", data.error);
            }
        });
    }

    $scope.loadModelInfo = function () {
        if ($location.search().sid) {
            $scope.SystemId = $location.search().sid;
        }
        var params = {
            SystemId: $scope.SystemId
        };
        apiHelper.callService(apiUrl + "GetModelInfo", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.dataModelList = data.data.list;
            } else {
                showMsg.error("提示", data.error);
            }
        });
    }

    $scope.zidingyi = {};

    $scope.bindbutton = function () {
        if ($location.search().sid) {
            $scope.SystemId = $location.search().sid;
        }

        var params = {
            SystemId: $scope.SystemId
        };
        apiHelper.callService(apiUrl + "GetModelButtonInfo", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.dataModelButtonList = data.data.list;
            } else {
                showMsg.error("提示", data.error);
            }
        });
    }
    $scope.updateMB = function (SystemId, ModelId, ButtonId, Checked) {
        var params = { SystemId: SystemId, ModelId: ModelId, ButtonId: ButtonId, RoleId: $scope.rid, Checked: Checked };
        apiHelper.callService(apiUrl + "UpdateRM", "POST", params).then(function (data) {
            if (!data.hasError) {
                if (data.data > 0) {
                    showMsg.success("提示", "成功更新！");
                } else {
                    showMsg.error("提示", "更新失败！");
                }
                // $scope.reload();
            } else {
                showMsg.error("提示", data.error);
            }
        });
    };

    $scope.saveselectedcheck = function () {
        $scope.dataModelButtonListCheck = $filter('filter')($scope.dataModelButtonList, { Checked: true });
        var obj = $scope.dataModelButtonListCheck;

        var params = { ModelButtonList: $filter('filter')($scope.dataModelButtonList, { Checked: true }), RoleId: $scope.rid, SystemId: $scope.SystemId };
        apiHelper.callService(apiUrl + "UpdateRoleModel", "POST", params).then(function (data) {
            if (!data.hasError) {
                if (data.data > 0) {
                    showMsg.success("提示", "成功更新！");
                } else {
                    showMsg.error("提示", "更新失败！");
                }
                // $scope.reload();
            } else {
                showMsg.error("提示", data.error);
            }
        });
    }

    $scope.bindbutton();
}]);
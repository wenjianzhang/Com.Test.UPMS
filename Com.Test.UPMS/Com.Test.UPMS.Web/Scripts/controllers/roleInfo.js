/// <reference path="../common.js" />
/// <reference path="../app.js" />

'use strict';
//var app = angular.module('myApp', ['ui.bootstrap']);

var apiUrl = "/admin/roleInfo/";

app.controller('roleInfoController', ['$scope', 'apiHelper', function ($scope, apiHelper) {
    $scope.paginationConf = {
        currentPage: 1,
        totalItems: 100,
        itemsPerPage: 10,
        pagesLength: 10,
        perPageOptions: [10, 20, 50, 100, 200, 500],
        onChange: function () {
            $scope.load($scope.paginationConf.itemsPerPage, $scope.paginationConf.currentPage);
        }
    };

    $scope.roleInfo = {
        Creater: "",
        IsDel: 0,
        Sort: "9999",
        Updater: "",
        RoleId: "",
        RoleName: "",
        RoleCode: "",
        RoleIcon: "",
        ClearData: function () {
            roleInfo.Creater = "";
            roleInfo.IsDel = 0;
            roleInfo.Sort = "9999";
            roleInfo.Updater = "";
            roleInfo.RoleId = "";
            roleInfo.RoleName = "";
            roleInfo.RoleCode = "";
            roleInfo.RoleIcon = "";
        }
    };

    $scope.ClearData = function () {
        $scope.roleInfo.Creater = "";
        $scope.roleInfo.IsDel = 0;
        $scope.roleInfo.Sort = "9999";
        $scope.roleInfo.Updater = "";
        $scope.roleInfo.RoleId = "";
        $scope.roleInfo.RoleName = "";
        $scope.roleInfo.RoleCode = "";
        $scope.roleInfo.RoleIcon = "";
    };

    //打开Form Bind数据
    $scope.showForm = function (id) {
        if (id) {
            $("#updateForm").show();
            $('#modal-map').modal({ backdrop: 'static', keyboard: false, show: true });
            $('.model-title').text("编辑");
            $('#ID').val(id);
            var params = {
                id: id
            };
            apiHelper.callService(apiUrl + "GetOne", "GET", params).then(function (data) {
                if (!data.hasError) {
                    $scope.roleInfo = data.data;
                } else {
                }
            });
        } else {
            $scope.ClearData();
            $("#updateForm").show();
            $('#modal-map').modal({ backdrop: 'static', keyboard: false, show: true });
            $('.model-title').text("新增");
        }
    }

    //取消
    $scope.cancelClick = function () {
        $("#modal-map").modal('hide');
    }

    //创建或修改数据
    $scope.updateClick = function () {
        if ($scope.roleInfo.RoleId) {
            var params = $scope.roleInfo;
            apiHelper.callService(apiUrl + "Edit", "POST", params).then(function (data) {
                if (!data.hasError) {
                    if (data.data > 0) {
                        showMsg.success("提示", "成功更新" + data.data + "条记录！");
                    } else {
                        showMsg.error("提示", "成功更新" + data.data + "条记录！");
                    }
                    $("#modal-map").modal('hide');
                    $scope.reload();
                } else {
                    showMsg.error("提示", data.error);
                }
            });
        } else {
            var params = $scope.roleInfo;
            apiHelper.callService(apiUrl + "Post", "POST", params).then(function (data) {
                if (!data.hasError) {
                    if (data.data > 0) {
                        showMsg.success("提示", "成功更新" + data.data + "条记录！");
                    } else {
                        showMsg.error("提示", "成功更新" + data.data + "条记录！");
                    }
                    $("#modal-map").modal('hide');
                    $scope.reload();
                } else {
                    showMsg.error("提示", data.error);
                }
            });
        }
    }

    //删除
    $scope.delete = function (id) {
        var params = {
            id: id
        };
        apiHelper.callService(apiUrl + "Delete", "POST", params).then(function (data) {
            if (!data.hasError) {
                if (data.data > 0) {
                    showMsg.success("提示", "成功删除" + data.data + "条记录！");
                } else {
                    showMsg.error("提示", "成功删除" + data.data + "条记录！");
                }
                $scope.reload();
            } else {
                showMsg.error("提示", data.error);
            }
        });
    }

    //基本获取list
    $scope.load = function (pageSize, pageCurrent) {
        var params = {
            pageSize: pageSize, pageCurrent: pageCurrent
        };
        apiHelper.callService(apiUrl + "Get", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.dataRoleList = data.data.list;
                $scope.paginationConf.totalItems = data.data.totalItemCount;
            } else {
            }
        });
    }

    //重新加载
    $scope.reload = function () {
        $scope.load($scope.paginationConf.itemsPerPage, $scope.paginationConf.currentPage);
    }
}]);
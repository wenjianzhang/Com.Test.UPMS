/// <reference path="../common.js" />
/// <reference path="../app.js" />

'use strict';
//var app = angular.module('myApp', ['ui.bootstrap']);

var apiUrl = "/admin/systemInfo/";
var apiUrlRole = "/admin/roleInfo/";

app.controller('systemInfoController', ['$scope', 'apiHelper', function ($scope, apiHelper) {
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

    $scope.systemInfo = {
        Creater: "",
        IsDel: 0,
        Sort: "9999",
        Updater: "",
        SystemId: "",
        SystemName: "",
        SystemCode: "",
        SystemIcon: "",
        SystemRoles: [],
        ClearData: function () {
            systemInfo.Creater = "";
            systemInfo.IsDel = 0;
            systemInfo.Sort = "9999";
            systemInfo.SystemRoles = [];
            systemInfo.Updater = "";
            systemInfo.SystemId = "";
            systemInfo.SystemName = "";
            systemInfo.SystemCode = "";
            systemInfo.SystemIcon = "";
        }
    };

    $scope.ClearData = function () {
        $scope.systemInfo.Creater = "";
        $scope.systemInfo.IsDel = 0;
        $scope.systemInfo.SystemRoles = [];
        $scope.systemInfo.Sort = "9999";
        $scope.systemInfo.Updater = "";
        $scope.systemInfo.SystemId = "";
        $scope.systemInfo.SystemName = "";
        $scope.systemInfo.SystemCode = "";
        $scope.systemInfo.SystemIcon = "";
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
                    $scope.systemInfo = data.data;
                } else {
                    showMsg.error("提示", "数据初始化失败！ message：" + data.error + "");
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
        if ($scope.systemInfo.SystemId) {
            var params = $scope.systemInfo;
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
            var params = $scope.systemInfo;
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
                $scope.dataSystemList = data.data.list;
                $scope.paginationConf.totalItems = data.data.totalItemCount;
            } else {
            }
        });
    }

    //重新加载
    $scope.reload = function () {
        $scope.load($scope.paginationConf.itemsPerPage, $scope.paginationConf.currentPage);
    }

    //加载其他数据
    $scope.getroleinfo = function () {
        //if ($location.search().sid) {
        //    $scope.SystemId = $location.search().sid;
        //}
        var params = {
            pageSize: 10000, pageCurrent: 1//, SystemId: $scope.SystemId
        };
        apiHelper.callService(apiUrlRole + "Get", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.dataRoleSelectList = data.data.list;
            } else {
            }
        });
    }
}]);
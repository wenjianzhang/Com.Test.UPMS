/// <reference path="../common.js" />
/// <reference path="../app.js" />

'use strict';
//var app = angular.module('myApp', ['ui.bootstrap']);

var apiUrl = "/admin/buttoninfo/";
app.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.html5Mode(true);
}]);

//app.controller('buttonInfoController', ['$scope', '$location', 'apiHelper', function ($scope, $location, apiHelper) {
app.controller('buttonInfoController', ['$scope', 'apiHelper', function ($scope, apiHelper) {
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
    $scope.buttons = {
        Inster: false,
        Get: false,
        Update: false,
        Delete: false
    };

    $scope.buttonInfo = {
        Creater: "",
        IsDel: 0,
        Sort: "9999",
        Updater: "",
        ButtonId: "",
        ButtonName: "",
        ButtonCode: "",
        ButtonIcon: "",
        ClearData: function () {
            buttonInfo.Creater = "";
            buttonInfo.IsDel = 0;
            buttonInfo.Sort = "9999";
            buttonInfo.Updater = "";
            buttonInfo.ButtonId = "";
            buttonInfo.ButtonName = "";
            buttonInfo.ButtonCode = "";
            buttonInfo.ButtonIcon = "";
        }
    };

    $scope.ClearData = function () {
        $scope.buttonInfo.Creater = "";
        $scope.buttonInfo.IsDel = 0;
        $scope.buttonInfo.Sort = "9999";
        $scope.buttonInfo.Updater = "";
        $scope.buttonInfo.ButtonId = "";
        $scope.buttonInfo.ButtonName = "";
        $scope.buttonInfo.ButtonCode = "";
        $scope.buttonInfo.ButtonIcon = "";
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
                    $scope.buttonInfo = data.data;
                } else {
                    showMsg.error("提示", data.error);
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
        if ($scope.buttonInfo.ButtonId) {
            var params = $scope.buttonInfo;
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
            var params = $scope.buttonInfo;
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
                $scope.dataButtonList = data.data.list;
                $scope.paginationConf.totalItems = data.data.totalItemCount;
            } else {
                showMsg.error("提示", data.error);
            }
        });
    }

    //重新加载
    $scope.reload = function () {
        $scope.load($scope.paginationConf.itemsPerPage, $scope.paginationConf.currentPage);
    }

    $scope.loadcurrentpagebutton = function () {
        var params = {};
        apiHelper.callService(apiUrl + "GetPageButton", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.buttons = data.data;
            } else {
                showMsg.error("提示", data.error);
            }
        });
    }
}]);
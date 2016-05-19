/// <reference path="../common.js" />
/// <reference path="../app.js" />

'use strict';
//var app = angular.module('myApp', ['ui.bootstrap']);

var apiUrl = "/admin/modelInfo/";

app.controller('modelInfoController', ['$scope', 'apiHelper', function ($scope, apiHelper) {
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

    $scope.modelInfo = {
        Creater: "",
        IsDel: 0,
        Sort: "9999",
        Updater: "",
        ModelId: "",
        PModelId: 0,
        ModelDesc: "",
        ModelName: "",
        ModelCode: "",
        ModelIcon: "",
        ClearData: function () {
            modelInfo.Creater = "";
            modelInfo.IsDel = 0;
            modelInfo.PModelId = 0;
            modelInfo.Sort = "9999";
            modelInfo.Updater = "";
            modelInfo.ModelDesc = "";
            modelInfo.ModelId = "";
            modelInfo.ModelName = "";
            modelInfo.ModelCode = "";
            modelInfo.ModelIcon = "";
        }
    };

    $scope.ClearData = function () {
        $scope.modelInfo.Creater = "";
        $scope.modelInfo.IsDel = 0;
        $scope.modelInfo.PModelId = 0;
        $scope.modelInfo.Sort = "9999";
        $scope.modelInfo.Updater = "";
        $scope.modelInfo.ModelId = "";
        $scope.modelInfo.ModelName = "";
        $scope.modelInfo.ModelCode = "";
        $scope.modelInfo.ModelIcon = "";
        $scope.modelInfo.ModelDesc = "";
    };

    //打开Form Bind数据
    $scope.showForm = function (id) {
        $scope.getmodelinfo();
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
                    $scope.modelInfo = data.data;
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
        if ($scope.modelInfo.ModelId) {
            var params = $scope.modelInfo;
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
            var params = $scope.modelInfo;
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
                $scope.dataModelList = data.data.list;
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
    $scope.getmodelinfo = function () {
        var params = {
            pageSize: 10000, pageCurrent: 1
        };
        apiHelper.callService(apiUrl + "Get", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.dataModelSelectList = data.data.list;
            } else {
            }
        });
    }
}]);
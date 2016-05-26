/// <reference path="../common.js" />
/// <reference path="../app.js" />

'use strict';
//var app = angular.module('myApp', ['ui.bootstrap']);

var apiUrl = "/admin/modelInfo/";
var apiUrlAction = "/admin/buttoninfo/";

app.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.html5Mode(true);
}]);

app.controller('modelInfoController', ['$scope', '$location', 'apiHelper', function ($scope, $location, apiHelper) {
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
        ModelButtons: [],
        SystemId: function () {
            if ($location.search().sid) {
                return $location.search().sid;
            }
            else {
                return 0;
            }
        },
        ClearData: function () {
            modelInfo.Creater = "";
            modelInfo.ModelButtons = [];
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
        $scope.modelInfo.ModelButtons = [];
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
                    showMsg.error("提示", "基础数据初始化失败！");
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
                        showMsg.success("提示", "成功更新第" + data.data + "条记录！");
                    } else {
                        showMsg.error("提示", "更新失败第" + data.data + "条记录！");
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
                        showMsg.success("提示", "成功创建第" + data.data + "条记录！");
                    } else {
                        showMsg.error("提示", "成功创建第" + data.data + "条记录！");
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
        if ($location.search().sid) {
            $scope.SystemId = $location.search().sid;
        }
        var params = {
            pageSize: pageSize, pageCurrent: pageCurrent, SystemId: $scope.SystemId
        };
        apiHelper.callService(apiUrl + "Get", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.dataModelList = data.data.list;
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

    //加载其他数据
    $scope.getmodelinfo = function () {
        if ($location.search().sid) {
            $scope.SystemId = $location.search().sid;
        }
        var params = {
            pageSize: 10000, pageCurrent: 1, SystemId: $scope.SystemId
        };
        apiHelper.callService(apiUrl + "GetList", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.dataModelSelectList = data.data.list;
            } else {
            }
        });
    }

    $scope.getbuttoninfo = function () {
        //if ($location.search().sid) {
        //    $scope.SystemId = $location.search().sid;
        //}
        var params = {
            pageSize: 10000, pageCurrent: 1//, SystemId: $scope.SystemId
        };
        apiHelper.callService(apiUrlAction + "Get", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.dataButtonSelectList = data.data.list;
            } else {
            }
        });
    }
}]);
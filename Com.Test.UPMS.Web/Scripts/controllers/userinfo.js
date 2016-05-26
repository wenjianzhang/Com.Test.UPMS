/// <reference path="../common.js" />
/// <reference path="../app.js" />

'use strict';
//var app = angular.module('myApp', ['ui.bootstrap']);

var apiUrl = "/admin/userinfo/";
var apiUrlByGetRole = "/admin/roleinfo/";

var apiUrlByUserRole = "/admin/userrole/";

app.controller('userInfoController', ['$scope', 'apiHelper', function ($scope, apiHelper) {
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

    $scope.userInfo = {
        CreateDate: "",
        Creater: "",
        IsDel: 0,
        Salt: "",
        Sort: "9999",
        UpdateDate: "",
        Updater: "",
        UserId: "",
        UserName: "",
        UserPassword: "",
        RoleId: "",
        ClearData: function () {
            userInfo.CreateDate = "";
            userInfo.Creater = "";
            userInfo.IsDel = 0;
            userInfo.RoleId = "";
            userInfo.Salt = "";
            userInfo.Sort = "9999";
            userInfo.UpdateDate = "";
            userInfo.Updater = "";
            userInfo.UserId = "";
            userInfo.UserName = "";
            userInfo.UserPassword = "";
        }
    };

    $scope.ClearData = function () {
        $scope.userInfo.CreateDate = "";
        $scope.userInfo.Creater = "";
        $scope.userInfo.IsDel = 0;
        $scope.userInfo.RoleId = "";
        $scope.userInfo.Salt = "";
        $scope.userInfo.Sort = "9999";
        $scope.userInfo.UpdateDate = "";
        $scope.userInfo.Updater = "";
        $scope.userInfo.UserId = "";
        $scope.userInfo.UserName = "";
        $scope.userInfo.UserPassword = "";
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
                    $scope.userInfo = data.data;
                } else {
                    showMsg.error("提示", data.error);
                }
            });
            //加载关系信息
            var paramsUserRole = {
                id: id
            };
            apiHelper.callService(apiUrlByUserRole + "GetOne", "GET", paramsUserRole).then(function (data) {
                if (!data.hasError) {
                    $scope.userInfo.RoleId = data.data.RoleId;
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
        if ($scope.userInfo.UserId) {
            var params = $scope.userInfo;
            //var bl = $scope.updateUserRole("Post", $scope.userInfo.UserId, $scope.userInfo.RoleId)
            //if (bl) {
            apiHelper.callService(apiUrl + "Edit", "POST", params).then(function (data) {
                if (!data.hasError) {
                    if (data.data > 0) {
                        showMsg.success("提示", "成功更新" + data.data + "条记录！");
                    } else {
                        showMsg.error("提示", "更新失败；msg：" + data.message + "！");
                    }
                    $("#modal-map").modal('hide');
                    $scope.reload();
                } else {
                    showMsg.error("提示", data.error);
                }
            });
            //}
            //else {
            //    showMsg.error("提示", "角色更新失败！");
            //}
        } else {
            var params = $scope.userInfo;
            apiHelper.callService(apiUrl + "Post", "POST", params).then(function (data) {
                if (!data.hasError) {
                    if (data.data > 0) {
                        //var bl = $scope.updateUserRole("Post", data.data, $scope.userInfo.RoleId)
                        //if (bl) {
                        showMsg.success("提示", "成功添加" + data.data + "条记录！");
                        //} else {
                        //    showMsg.error("提示", "成功添加用户信息，角色信息配置失败！");
                        //}
                    } else {
                        showMsg.error("提示", "添加失败；msg：" + data.message + "！");
                    }
                    $("#modal-map").modal('hide');
                    $scope.reload();
                } else {
                    showMsg.error("提示", data.error);
                }
            });
        }
    }

    $scope.updateUserRole = function (methed, UserId, RoleId) {
        var paramsUserRole = {
            UserId: UserId,
            RoleId: RoleId
        };
        apiHelper.callService(apiUrlByUserRole + methed, "POST", paramsUserRole).then(function (data) {
            if (!data.hasError) {
                return true;
            } else {
                return false;
            }
        });
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
                $scope.dataUserList = data.data.list;
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
    $scope.getroleinfo = function () {
        var params = {
            pageSize: 10000, pageCurrent: 1
        };
        apiHelper.callService(apiUrlByGetRole + "Get", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.dataRoleSelectList = data.data.list;
            } else {
                showMsg.error("提示", data.error);
            }
        });
    }
}]);
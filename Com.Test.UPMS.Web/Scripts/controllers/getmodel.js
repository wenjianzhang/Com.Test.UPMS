/// <reference path="../common.js" />
/// <reference path="../app.js" />

'use strict';

var apiUrlgetmodel = "/admin/modelInfo/";

app.controller('currentModelController', ['$scope', 'apiHelper', function ($scope, apiHelper) {
    //基本获取list
    $scope.loadcurrentmodel = function () {
        var params = {};
        apiHelper.callService("/admin/modelInfo/GetCurrentModel", "GET", params).then(function (data) {
            if (!data.hasError) {
                $scope.dataCurrentModel = data.data;
            } else {
                showMsg.error("提示", data.error);
            }
        });
    }
}]);
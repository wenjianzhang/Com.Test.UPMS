'use strict';

var app = angular.module('adminAppBase', [
    "ui.bootstrap"
]);

var rootUrl = baseUrl;
app.run(['$rootScope', function ($rootScope) {
}]);

app.filter("jsonDate", function ($filter) {
    return function (input, format) {
        //从字符串 /Date(1448864369815)/ 得到时间戳 1448864369815
        var timestamp = Number(input.replace(/\/Date\((\d+)\)\//, "$1"));
        //转成指定格式
        return $filter("date")(timestamp, format);
    };
});

app.factory('apiHelper', ['$http', '$q', function ($http, $q) {
    return {
        callService: function (url, method, params, data) {
            return $http({
                url: url,
                method: method,
                params: params,
                data: data
            }).then(function (resp) {
                if (!resp.data.HasError) return resp.data;
                else return resp.data;
            });
        },
        deferCallService: function (url, method, params, data) {
            var deferred = $q.defer();
            $http({
                url: url,
                method: method,
                params: params,
                data: data
            }).then(function (resp) {
                if (!resp.data.HasError) {
                    deferred.resolve(resp.data.Data);
                } else {
                    deferred.reject(resp.data);
                }
            });
            return deferred.promise;
        }
    }
}]);
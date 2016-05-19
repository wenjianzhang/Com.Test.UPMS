var showMsg = {
    error: function (title, text) {
        $.gritter.add({
            title: title,
            text: text,
            class_name: 'gritter-error'
        });
    },
    success: function (title, text) {
        $.gritter.add({
            title: title,
            text: text,
            class_name: 'gritter-success'
        });
    },
    msg: function (text) {
        $.gritter.add({
            title: "",
            text: text,
            class_name: 'gritter-info'
        });
    }
}

var Format = {
    Date: function (time) {
        var tmp = /\d+(?=\+)/.exec(time);
        var d = new Date(+tmp);
        return d.getFullYear() + '-' + (+d.getMonth() + 1) + '-' + d.getDate();
    }
}

var Url = {
    GetRequest: function () {
        var url = location.search; //获取url中"?"符后的字串
        var theRequest = new Array();
        var theRequestKey = new Array();
        var result = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
                theRequestKey[i] = [strs[i].split("=")[0]];
            }
        }
        result["data"] = theRequest;

        result["key"] = theRequestKey;
        return result;
    },
    SetRequest: function (k, v) {
        var requestData = Url.GetRequest();
        var strRequest = "?";
        var length = requestData.key.length;
        angular.forEach(requestData.key, function (data, index) {
            if (data != k) {
                strRequest += data + "=" + requestData.data[data];
                strRequest += "&";
            }
            //if (index + 1 != length) {
            //}
        });
        strRequest += k + "=" + v;
        return strRequest;
    }
}
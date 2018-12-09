
/**
* Helper类
* 功能:通用的JS方法
*/
var Helpers = (function () {
    var _helpers = {};

    //测试
    _helpers.sayHi = function (name) {
        alert(name);
    }

    /**
    * 判断是对象是否空
    */
    _helpers.IsNullOrEmpty = function (obj) {
        var bRes = false;
        if (obj == '' || obj == null || obj == undefined)
            bRes = true;

        return bRes;
    }

    /**
    * 删除左右两端的空格
    */
    _helpers.Trim = function (str) {
        return $.trim(str);
    }

    /**
    * ajax处理html字符串
    */
    _helpers.ajax_encode = function (str) {
        str = str.replace(/%/g, "{@bai@}");
        str = str.replace(/ /g, "{@kong@}");
        str = str.replace(/</g, "{@zuojian@}");
        str = str.replace(/>/g, "{@youjian@}");
        str = str.replace(/&/g, "{@and@}");
        str = str.replace(/\"/g, "{@shuang@}");
        str = str.replace(/\'/g, "{@dan@}");
        str = str.replace(/\t/g, "{@tab@}");
        str = str.replace(/\+/g, "{@jia@}");

        return str;
    }

    /**
     * 查找列表中是否存在当前值
     * strKey:关键词
     * lstStr:字符串数组
     */
    _helpers.SearchKey = function (strKey, lstStr) {

        var bRet = false;
        for (i = 0; i < lstStr.length ; i++) {
            if (strKey == lstStr[i]) {
                bRet = true;
                break;
            }
        }

        return bRet;
    }

    /**
    * 获取url参数
    */
    _helpers.getQueryString = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return '';
    }

    /**
    *验证输入是否是正整数   add by rui.li 2014-03-21
    */
    _helpers.isNum = function (s) {
        if (s != null) {
            var r, re;
            re = /\d*/i; //\d表示数字,*表示匹配多个数字
            r = s.match(re);
            return (r == s) ? true : false;
        }
        return false;
    }

    //正整数
    _helpers.isDecimal = function (s) {

        if (s != null) {
            var r, re;
            re = /^\\d+$/; //\d表示数字,*表示匹配多个数字
            r = s.match(re);
            return (r == s) ? true : false;
        }
        return false;
    }

    /**
    * 验证数字是否奇数
    */
    _helpers.isEven = function isEven(value) {
        if (value % 2 == 0)
            return true;
        else
            return false;
    }


    /**
    * 封装jquery Ajax
    * 加入了bootbox
    */
    _helpers.Ajax = function (strUrl) {

        $.ajax({
            type: 'POST',
            url: strUrl,
            data: null,
            success: function (data) {
                bootbox.alert(data.Info, function () {
                    location.reload();
                });
            }, error: function () {
                bootbox.alert("ajax请求失败!");
                location.reload();
            }
        });
    }

    /**
     * 两个参数，一个是cookie的名子，一个是值
     */
    _helpers.SetCookie = function (name, value) {

        var Days = 30; //此 cookie 将被保存 30 天
        var exp = new Date();    //new Date("December 31, 9998");
        exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
        document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
    }

    //取cookies函数        
    _helpers.getCookie = function (name) {

        var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
        if (arr != null) return unescape(arr[2]); return null;

    }

    //删除
    _helpers.delCookie = function (name) {

        var exp = new Date();
        exp.setTime(exp.getTime() - 1);
        var cval = this.getCookie(name);
        if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
    }

    return _helpers;
})();
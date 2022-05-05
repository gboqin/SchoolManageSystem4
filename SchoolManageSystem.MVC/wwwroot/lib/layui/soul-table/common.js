layui.define(['jquery'], function (exports) {
    var $ = layui.jquery;
    var obj = {
        test: function () {
            alert('test from common');
        }
    };
    //输出接口
    exports('common', obj);
});
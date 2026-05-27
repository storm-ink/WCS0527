$.wmsNotify = function () {
    var _obj = this;
    this.getNotify = function () {
        $.post("url", {}, function (data) {
            var bar = _obj.buildBar();
            if (data == null && data.length == 0) {
                $(bar).hide();
            } else {
                $(bar).children().remove();
                $(bar).show();
                for (var i = 0; i < data.length; i++) {
                    $(bar).appendMsg(data.msg);
                }
            }
        }, "json")
        .error(function (err) {
            var bar = _obj.buildBar();
            $(bar).children().remove();
            var msg = err.status + ' ' + err.statusText;
            if (typeof (getResponseErrorText) == 'function') {
                msg = getResponseErrorText(err);
            }
            bar.appendMsg('【'+_obj.getDate('HH时mm分ss秒')+'】服务器消息获取失败：'+msg);
            $(bar).show();
        });
    };

    this.getDate = function (formatStr) {
        var d = new Date();
        var vYear = d.getFullYear();
        var vMon = d.getMonth() + 1;
        var vDay = d.getDate();
        var h = d.getHours();
        var m = d.getMinutes();
        var se = d.getSeconds();
        s = vYear + (vMon < 10 ? "0" + vMon : vMon) + (vDay < 10 ? "0" + vDay : vDay) + (h < 10 ? "0" + h : h) + (m < 10 ? "0" + m : m) + (se < 10 ? "0" + se : se);

        if (formatStr != undefined && formatStr != null && formatStr != '') {
            return formatStr.replace('yyyy', vYear)
                            .replace('MM', vMon < 10 ? "0" + vMon : vMon)
                            .replace('mm', vMon)
                            .replace('DD', (vDay < 10 ? "0" + vDay : vDay))
                            .replace('dd', vDay)
                            .replace('HH', (h < 10 ? "0" + h : h))
                            .replace('hh', h)
                            .replace('ss', se);
        } else {
            return s;
        }
    };

    this.buildBar = function () {
        var bar = $('#wmsNotify_bar');
        if (bar.length == 0) {
            bar = $('<ul id="wmsNotify_bar" style="width:100%;background-color:yellow;height:auto;overflow:auto;display:none;margin:0px auto;padding:0px;list-style:none;max-width:960px;"></ul>"');
            $(document.body).prepend(bar);
        }

        bar.appendMsg = function (msg) {
            $(this).append(
                $('<li>')
                .html(msg)
                .css('list-style','decimal')
                .css('list-style-type','decimal')
                .css('list-style-position','inside')
                .css('float','left')
                .css('padding','5px')
                .css('margin-left','5px')
                );
        };

        return bar;
    };

    this._timer = null;
    this.start = function () {
        this.stop();
        this._timer = setInterval(this.getNotify, 50 * 1000);
    };

    this.stop = function () {
        if (this._timer != null) {
            clearInterval(this._timer);
        }
    };
};

function timeLine(container, startTime, endTime) {
    var that = this;
    var totalSeconds = parseInt((endTime.getTime() - startTime.getTime()) / 1000);
    var secondPixel = 8;
    var width = secondPixel * totalSeconds + 20;
    var heigth = 60;
    var y = heigth - 25;
    var canvasObj = $('<canvas width="' + width + '" height="' + heigth + '"></canvas>');
    $(container).append(canvasObj);

    var timer = null;
    $(canvasObj).draggable(
        {
            axis: "x",
            drag: function (event, ui) {
                //console.info(ui.offset);
                if (ui.offset.left > 0) {
                    cancelDrag();
                }
            },
            start: function (event, ui) {

            },
            stop: function (event, ui) {

                console.info(ui.offset);

                if (that.onChange) {
                    that.onChange(that.getTime(ui.offset.left));
                }
            }
        });

    var canvas = canvasObj[0];

    var ctx = canvas.getContext('2d');
    var startSecond = startTime.getSeconds();

    ctx.font = "12px Arial";

    ctx.beginPath();
    ctx.moveTo(0, y);


    this.onChange = null;
    this.onFinished = null;
    this.StartTime = startTime;
    this.speed = 1;

    function play() {
        var x = $(canvas).offset().left;
        x -= secondPixel;
        $(canvas).offset({ left: x });

        if (that.onChange) {
            that.onChange(that.getTime(x));
        }
        if (Math.abs(x) >= width) {
            that.stop();
            if (that.onFinished) {
                that.onFinished();
                return;
            }
        }
        timer = setTimeout(play, 1 / that.speed * 1000);
    }

    this.start = function () {
        that.stop();
        timer = setTimeout(play, 1 / that.speed * 1000);
    };
    this.stop = function () {
        if (timer) {
            clearTimeout(timer);
        }
    };
    this.getTime = function (x) {
        x = parseInt(Math.abs(x));
        var seconds = (x - x % secondPixel) / secondPixel;
        if (x % secondPixel > 0) {
            seconds += 1;
        }

        return that.StartTime.add('s', seconds);
    }

    this.init = function () {
        $(canvasObj).offset({ left: 0 });
    }

    this.dispose = function () {
        this.stop();
        $(canvasObj).remove();
    };

    for (var i = 0; i < totalSeconds; i++) {
        var time = startTime.add('s', i);
        var currentSecnods = (startSecond + i);
        ctx.lineTo(i * secondPixel, y);
        if (currentSecnods % 60 == 0) {
            ctx.lineTo(i * secondPixel, y - 15);

            ctx.fillText(time.format('hh时mm分'), i * secondPixel - time.format('hh时mm分').length * 3, y - 20);


            ctx.fillText(time.getSeconds(), i * secondPixel - time.getSeconds().toString().length * 5, y + 20);

            //var angle = -290;
            //var x1 = i * secondPixel - 20;
            //var y1 = y + 45;
            //var rx = x1 * Math.cos(-angle) - y1 * Math.sin(-angle);
            //var ry = y1 * Math.cos(-angle) + x1 * Math.sin(-angle);

            //ctx.save();
            //ctx.rotate(angle);
            //// 设置字体内容，以及在画布上的位置
            //ctx.fillText(time.format('hh:mm:ss'), rx, ry);
            //ctx.restore();


        } else if (currentSecnods % 5 == 0) {
            ctx.lineTo(i * secondPixel, y - 5);


            ctx.fillText(time.getSeconds(), i * secondPixel - time.getSeconds().toString().length * 5, y + 20);


        } else {
            ctx.lineTo(i * secondPixel, y - 2);
        }
        ctx.moveTo(i * secondPixel, y);


    }

    ctx.lineWidth = 1; // 设置线宽
    ctx.strokeStyle = "#000000"; // 设置线的颜色
    ctx.stroke(); // 进行线的着色，这时整条线才变得可见

    return that;
}

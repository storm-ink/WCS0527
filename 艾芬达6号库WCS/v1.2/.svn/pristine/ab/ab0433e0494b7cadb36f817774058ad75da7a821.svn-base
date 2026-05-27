var MessageCenter = function (ttsUrl, mcClip) {
    var that = this;
    var currentMsg = null;
    this.ttsUrl = ttsUrl;
    this.messages = new Array();
    var timer = setInterval(timerInterval, 500);

    function timerInterval() {
        if (!currentMsg || !currentMsg.isPlaying) {
            that.next();
        }
    }

    function createAudio(msg) {
        var audio = $('<audio autoplay="autoplay"></audio>');
        $(audio).attr('autoplay', 'autoplay');

        $('body').append(audio);

        return audio;
    };

    function play(msg) {
        clearInterval(timer);
        if (that.palyer) {
            $(that.palyer).remove();
            delete that.palyer;
        }

        that.palyer = createAudio();
        bindPlayEvents();


        timer = 0;
        msg.isPlaying = true;
        currentMsg = msg;
        $(that.palyer).attr('src', that.ttsUrl + '?text=' + encodeURIComponent(msg.text)+'&t='+(new Date()).toString());
        $(that.palyer)[0].msg = msg;
    }

    function bindPlayEvents() {
        var errorEvents = ['error', 'abort', 'stalled', 'ended'];
        //事件参照 http://blog.sina.com.cn/s/blog_51e565eb01018tbp.html
        for (var i = 0; i < errorEvents.length; i++) {
            that.palyer[0].addEventListener(errorEvents[i], function () {
                if (timer && timer != 0) {
                    return;
                }

                var player = this;
                var msg = player.msg;
                msg.isPlaying = false;

                console.log(msg.text + '由于 '+ arguments[0].type +' 播放结束');

                timer = setInterval(timerInterval, 500);

                timerInterval();
            });
        }
    }

    this.next = function () {
        if ((currentMsg && currentMsg.isPlaying) || that.messages.length == 0) {
            return;
        }

        var nextMsg = null;
        if (currentMsg) {
            var index = this.findIndex(currentMsg);
            if (index + 1 >= that.messages.length) {
                return;
            }
            nextMsg = that.messages[index + 1];
        } else {
            nextMsg = that.messages[0];
        }

        if (nextMsg.isPlaying) {
            return;
        }

        play(nextMsg);
    };
    this.isExists = function (text) {
        for (var i = 0; i < that.messages.length; i++) {
            if (that.messages[i].text == text) {
                return true;
            }
        }

        return false;
    }

    this.findIndex = function (msg) {
        for (var i = 0; i < that.messages.length; i++) {
            if (that.messages[i] == msg) {
                return i;
            }
        }

        return -1;
    }
    
    this.createMessage=function(msg) {
        return {
            text: msg,
            createdAt: new Date(),
            playCount: 0,
            isPlaying:false
        };
    }


    this.addMessage = function (msg) {
        if (msg == '') {
            return;
        }

        if (that.isExists(msg)) {
            return;
        }

        var m=that.createMessage(msg);
        that.messages.push(m);
    }

}
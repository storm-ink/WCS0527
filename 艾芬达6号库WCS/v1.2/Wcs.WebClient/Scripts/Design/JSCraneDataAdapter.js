(function () {
    Array.prototype.contains = function (item) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == item) {
                return true;
            }
        }

        return false;
    };

    Kinetic.JSCraneDataAdapter = function (config) {
        this.____init(config);
    };

    Kinetic.JSCraneDataAdapter.prototype = {
        ____init: function (config) {
            var that = this;
            this.className = 'JSCraneDataAdapter';
            Kinetic.Group.call(this, config);

            this.on('add.kinetic', function (evt) {
                that._addListeners(evt.child);
                that._sync();
            });

            if (Kinetic.isDesign) {
                var bodyLayer = new Kinetic.Rect({
                    draggable: false,
                    'fill': '#ccc',
                    'stroke': '#000',
                    'strokeWidth': 1,
                    'width': this.getWidth(),
                    'height': this.getHeight(),
                    'opacity': 0.3,
                    'name': 'body',
                    sceneFunc: function (context) {
                        context.beginPath();
                        context.fillRect(0, 0, this.getWidth(), this.height());
                        context.closePath();
                        context.fillStrokeShape(this);
                    }
                });

                this.add(bodyLayer);
                bodyLayer.moveToBottom();

                //添加文本
                this.add(new Kinetic.Text({
                    fontFamily: 'Calibri',
                    fontSize: 12,
                    padding: 5,
                    lineHeight: 1.2,
                    width: config.width,
                    fill: 'red',
                    align: 'center',
                    name: 'text',
                    text: '堆垛机数据填充器'
                }));
            } else {
                this.start();
            }
        },
        getBody: function () {
            return this.find('.body')[0];
        },
        getText: function () {
            return this.find('.text')[0];
        },
        //将指定的对象转换为 Object
        toJSON: function (v) {
            if (typeof (v) == 'string') {
                r = $.parseJSON(v);
            } else {
                r = v;
            }
            for (var prop in r) {
                if (r[prop] && typeof (r[prop]) == 'string') {
                    if ((this.startWith(r[prop], '\\{') && this.endWith(r[prop], '\}')) || (this.startWith(r[prop], '\\[') && this.endWith(r[prop], '\\]'))) {
                        r[prop] = $.parseJSON(r[prop]);
                    } else {
                        if (r[prop] == "null") {
                            r[prop] = null;
                        }
                    }
                }
            }

            return r;
        },
        //根据名称查找堆垛机对象
        findCrane : function (name) {
            if(!window.stage){
                return [];
            }

            var result=[];
            this.getStage().find('Layer').each(function (layer) {
                layer.getChildren().each(function (crane) {
                    if (Kinetic.JSCraneTypes.contains(crane.getClassName()) && crane.getName() == name) {
                        result.push(crane);
                    }
                });
            });

            return result;
        },
        //开始数据填充
        start: function () {
            if (this._timer && this._timer != 0) {
                console.error("数据填充器已启动，请勿重复操作");
            }

            var that = this;

            this._timer = setInterval(function () {
                (function (adapter) {
                    
                    $.ajax({
                        type: "get",
                        dataType: "jsonp",
                        url: adapter.getUrl(),
                        success: function (data) {
                            if (data && data.JsonString) {
                                data.JsonString = adapter.toJSON(data.JsonString);
                            }

                            if (data && data.JsonString) {
                                for (var i = 0; i < data.JsonString.length; i++) {
                                    var status = data.JsonString[i];
                                    if (status.Name == '') {
                                        continue;
                                    }

                                    var crane = adapter.findCrane(status.Name);
                                    for (var j = 0; j < crane.length; j++) {
                                        crane[j].setStatus(status);
                                    }
                                }
                            }
                        },
                        error: function (x1, x2, x3) {
                            console.error(x1.status);
                        }
                    });

                })(that);
            }, that.getInterval());
        },
        //停止数据填充
        stop: function () {
            if (this._timer && this._timer != 0) {
                clearInterval(this._timer);
                this._timer = 0;
            }
        },
        //重写序列化方法
        toObject: function () {
            var obj = Kinetic.Node.prototype.toObject.call(this);

            obj.children = [];

            return obj;
        },
        _addListeners: function (text) {

        },
        _sync: function () {
            //if (Kinetic.isDesign) {
            //    var text = this.getText();
            //    if (text) {
            //        text.setAttrs({
            //            y: this.getHeight() / 2
            //        });
            //    }
            //}
        }
    };

    Kinetic.Util.extend(Kinetic.JSCraneDataAdapter, Kinetic.Group);
    Kinetic.Factory.addGetterSetter(Kinetic.JSCraneDataAdapter, 'interval', 1000);
    Kinetic.Factory.addGetterSetter(Kinetic.JSCraneDataAdapter, 'url', 'http://localhost:9991/CraneServiceForClient/GetAllStatus');

    Kinetic.Factory.addGetterSetter(Kinetic.JSCraneDataAdapter, 'editor',
    {
        "x": {
            text: 'x',
            getter: 'getX',
            setter: 'x',
            parser: parseInt
        },
        "y": {
            text: 'y',
            getter: 'getY',
            setter: 'y',
            parser: parseInt
        },
        "zIndex": {
            text: '层级',
            getter: 'getZIndex',
            setter: 'setZIndex',
            parser: parseInt
        },
        "interval": {
            text: '周期(毫秒)',
            getter: 'getInterval',
            setter: 'setInterval',
            parser: parseInt
        },
        "url": {
            text: '数据地址',
            getter: 'getUrl',
            setter: 'setUrl'
        }
    });

    Kinetic.Collection.mapMethods(Kinetic.JSCraneDataAdapter);
})();
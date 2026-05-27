(function () {
    Array.prototype.contains = function (item) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == item) {
                return true;
            }
        }

        return false;
    };

    Kinetic.JSShuttleDataAdapter = function (config) {
        this.____init(config);
    };

    Kinetic.JSShuttleDataAdapter.prototype = {
        ____init: function (config) {
            var that = this;
            this.className = 'JSShuttleDataAdapter';
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
                    text: '穿梭车数据填充器'
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
        findShuttles : function () {
            if(!window.stage){
                return [];
            }

            var result=[];
            this.getStage().find('Layer').each(function (layer) {
                layer.getChildren().each(function (shuttle) {
                    //if (Kinetic.JSShuttleTypes.contains(shuttle.getClassName()) && shuttle.getName() == name) {
                    if (Kinetic.JSShuttleTypes.contains(shuttle.getClassName())) {
                        result.push(shuttle);
                    }
                });
            });

            return result;
        },
        //开始数据填充
        start: function () {
            if (window.Playback) {
                console.warn("数据回放模式下无法以启动实时数据填充器.");
                return;
            }

            if (this._timer && this._timer != 0) {
                console.error("数据填充器已启动，请勿重复操作");
            }

            var that = this;

            this._timer = setTimeout(function () {
                (function (adapter) {
                    
                    try {
                        $.ajax({
                            type: "get",
                            dataType: "json",
                            url: adapter.getUrl(),
                            success: function (data) {
                                if (data) {
                                    data = adapter.toJSON(data);
                                }

                                if (data) {
                                    for (var i = 0; i < data.length; i++) {
                                        var status = data[i];
                                        if (status.Name == '') {
                                            continue;
                                        }

                                        var shuttle = adapter.findShuttles();
                                        for (var j = 0; j < shuttle.length; j++) {
                                            shuttle[j].updateCarStatus(status.Name, status);


                                            if (window._messageCenter) {

                                                if (status.State == 8) {
                                                    var msg = status.Name + '处于报警停机状态，原因是' + status.ErrorName;
                                                    window._messageCenter.addMessage(msg);
                                                } else if (status.State == 1) {
                                                    var msg = status.Name + '被切换到了手动模式';
                                                    window._messageCenter.addMessage(msg);
                                                } else if (status.ErrorName != '' && status.ErrorName!=null) {

                                                    var msg = status.Name + status.ErrorName;
                                                    window._messageCenter.addMessage(msg);
                                                }
                                            }


                                        }
                                    }
                                }
                            },
                            error: function (x1, x2, x3) {
                                console.error(x1.status);
                            }
                        });
                    } catch (e) {
                        console.error(e);
                    }
                    adapter._timer = 0;
                    adapter.start();
                })(that);
            }, that.getInterval());
        },
        //停止数据填充
        stop: function () {
            if (this._timer && this._timer != 0) {
                //clearInterval(this._timer);
                clearTimeout(this._timer);
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

    Kinetic.Util.extend(Kinetic.JSShuttleDataAdapter, Kinetic.Group);
    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttleDataAdapter, 'interval', 1000);
    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttleDataAdapter, 'url', '/RailGuidedVehicle/DeviceService/GetAllStatus');

    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttleDataAdapter, 'editor',
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

    Kinetic.Collection.mapMethods(Kinetic.JSShuttleDataAdapter);
})();
(function () {
    Kinetic.JSConveyorDataAdapter = function (config) {
        this.____init(config);
    };

    Kinetic.JSConveyorDataAdapter.prototype = {
        ____init: function (config) {
            var that = this;
            this.className = 'JSConveyorDataAdapter';
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
                    text: '输送线数据填充器'
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
        toJSON : function (v) {
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
        //根据货位设备编码查找货位对象
        findConveyorLocation : function (deviceCode) {
            if(!this.getStage()){
                return [];
            }

            var result=[];
            this.getStage().find('Layer').each(function (layer) {
                layer.getChildren().each(function (item) {
                    if (item.getClassName()=='JSConveyorLocation' && item.getDeviceCode() == deviceCode) {
                        result.push(item);
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
                    //报警状态
                    $.ajax({
                        type: "get",
                        dataType: "jsonp",
                        url: adapter.getAlarmsUrl(),
                        success: function (data) {
                            if (data && data.JsonString) {
                                data.JsonString = adapter.toJSON(data.JsonString);
                            }

                            if (data && data.JsonString) {
                                for (var i = 0; i < data.JsonString.length; i++) {
                                    var alarms = data.JsonString[i];
                                    if (alarms.PosNo == 0) {
                                        continue;
                                    }

                                    var loc = adapter.findConveyorLocation(alarms.PosNo.toString());
                                    for (var j = 0; j < loc.length; j++) {
                                        loc[j].setAlarm(alarms);
                                    }
                                }
                            }
                        },
                        error: function (x1, x2, x3) {
                            console.error(x1.status);
                        }
                    });

                    //状态信息
                    $.ajax({
                        type: "get",
                        dataType: "jsonp",
                        url: adapter.getLocationStatusUrl(),
                        success: function (data) {
                            if (data && data.JsonString) {
                                data.JsonString = adapter.toJSON(data.JsonString);
                            }

                            if (data && data.JsonString) {
                                for (var i = 0; i < data.JsonString.length; i++) {
                                    var state = data.JsonString[i];
                                    if (state.PosNo == 0) {
                                        continue;
                                    }

                                    var loc = adapter.findConveyorLocation(state.PosNo.toString());
                                    for (var j = 0; j < loc.length; j++) {
                                        loc[j].setStatus(state.Status);
                                    }
                                }
                            }
                        },
                        error: function (x1, x2, x3) {
                            console.error(x1.status);
                        }
                    });

                    //任务信息
                    $.ajax({
                        type: "get",
                        dataType: "jsonp",
                        url: adapter.getLocationTasksUrl(),
                        success: function (data) {
                            if (data && data.JsonString) {
                                data.JsonString = adapter.toJSON(data.JsonString);
                            }

                            if (data && data.JsonString) {
                                for (var i = 0; i < data.JsonString.length; i++) {
                                    var task = data.JsonString[i];
                                    if (task.PosNo == 0) {
                                        continue;
                                    }

                                    var loc = adapter.findConveyorLocation(task.PosNo.toString());
                                    for (var j = 0; j < loc.length; j++) {
                                        loc[j].setTask(task);
                                    }
                                }
                            }
                        },
                        error: function (x1, x2, x3) {
                            console.error(x1.status);
                        }
                    });

                    //任务信息
                    $.ajax({
                        type: "get",
                        dataType: "jsonp",
                        url: adapter.getOccupysUrl(),
                        success: function (data) {
                            if (data && data.JsonString) {
                                data.JsonString = adapter.toJSON(data.JsonString);
                            }

                            if (data && data.JsonString) {
                                for (var i = 0; i < data.JsonString.length; i++) {
                                    var occupy = data.JsonString[i];
                                    if (occupy.PosNo == 0) {
                                        continue;
                                    }

                                    var loc = adapter.findConveyorLocation(occupy.PosNo.toString());
                                    for (var j = 0; j < loc.length; j++) {
                                        loc[j].setOccupy(occupy);
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

    Kinetic.Util.extend(Kinetic.JSConveyorDataAdapter, Kinetic.Group);
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorDataAdapter, 'interval', 1000);
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorDataAdapter, 'alarmsUrl', 'http://localhost:9991/ConveyorServiceForClient/GetAlarms?jsoncallback=?&deviceName=' + encodeURIComponent('输送线'));
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorDataAdapter, 'locationStatusUrl', 'http://localhost:9991/ConveyorServiceForClient/GetLocationStatus?jsoncallback=?&deviceName=' + encodeURIComponent('输送线'));
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorDataAdapter, 'locationTasksUrl', 'http://localhost:9991/ConveyorServiceForClient/GetLocationTasks?jsoncallback=?&deviceName=' + encodeURIComponent('输送线'));
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorDataAdapter, 'occupysUrl', 'http://localhost:9991/ConveyorServiceForClient/GetOccupys?jsoncallback=?&deviceName=' + encodeURIComponent('输送线'));

    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorDataAdapter, 'editor',
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
        "alarmsUrl": {
            text: '报警数据地址',
            getter: 'getAlarmsUrl',
            setter: 'setAlarmsUrl'
        },
        "locationStatusUrl": {
            text: '状态数据地址',
            getter: 'getLocationStatusUrl',
            setter: 'setLocationStatusUrl'
        },
        "locationTasksUrl": {
            text: '任务数据地址',
            getter: 'getLocationTasksUrl',
            setter: 'setLocationTasksUrl'
        },
        "occupysUrl": {
            text: '光电数据地址',
            getter: 'getOccupysUrl',
            setter: 'setOccupysUrl'
        }
    });

    Kinetic.Collection.mapMethods(Kinetic.JSConveyorDataAdapter);
})();
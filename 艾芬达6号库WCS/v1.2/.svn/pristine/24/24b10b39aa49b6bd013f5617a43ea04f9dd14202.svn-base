(function () {
    Kinetic.JSShuttle = function (config) {
        this.____init(config);
    };

    if (!Kinetic.JSShuttleTypes) {
        Kinetic.JSShuttleTypes = [];
    }
    Kinetic.JSShuttleTypes.push('JSShuttle');

    Kinetic.JSShuttle.prototype = {
        ____init: function (config) {
            var that = this;
            this.className = 'JSShuttle';
            Kinetic.Group.call(this, config);

            this.on('add.kinetic', function (evt) {
                that._addListeners(evt.child);
                that._sync();
            });

            if (Kinetic.isDesign) {
                var maskLayer = new Kinetic.Shape({
                    draggable: false,
                    'fill': '#ccc',
                    'stroke': '#ccc',
                    'strokeWidth': 1,
                    'width': this.getWidth(),
                    'height': this.getHeight(),
                    'opacity': 0,
                    'name': 'maskLayer',
                    sceneFunc: function (context) {
                        context.beginPath();
                        context.rect(0, 0, this.getWidth(), this.height());
                        context.closePath();
                        context.fillShape(this);
                    }
                });

                this.add(maskLayer);
                maskLayer.moveToBottom();
            }

            //添加轨道层
            this.add(new Kinetic.Shape({
                draggable: false,
                'fill': '#ccc',
                'stroke': '#ccc',
                'width': this.getWidth(),
                'height': this.getHeight(),
                'name': 'railWay',
                sceneFunc: function (context) {
                    var count = that.getRailWayCount();
                    var weight = that.getRailWayWeigth();
                    var padding = that.getRailWayPadding();
                    context.beginPath();
                    if (count == 1) {
                        context.rect((this.getWidth() - weight) / 2, 0, weight, this.getHeight());
                    } else {
                        var interval = (this.getWidth() - padding * 2 - weight) / (count - 1);
                        for (var i = 0; i < count; i++) {
                            var x = padding + interval * i;
                            context.rect(x, 0, weight, this.getHeight());
                        }
                    }
                    context.closePath();
                    context.fillShape(this);
                }
            }));

            //添加车体
            var cars = new Array();
            var carNames = this.getCarNames();
            for (var carIndex = 0; carIndex < carNames.length; carIndex++) {
                var car = new Kinetic.Shape({
                    draggable: false,
                    'fill': '#ccc',
                    'stroke': '#ccc',
                    'width': this.getWidth(),
                    'height': this.getHeight(),
                    'name': 'shuttle',
                    sceneFunc: function (context) {
                        var oldFillValue = this.getFill();
                        var height = that.getShuttleHeight();
                        var width = that.getWidth();
                        var chainSize = that.getChainSize();
                        var chainPadding = that.getChainPadding();
                        var chainCount = that.getChainCount();
                        //var y = (that.getHeight() - height) / 2;
                        var y = 0;
                        var fillColor = oldFillValue;
                        if (this.state && this.state.State && this.statusColors && this.statusColors[this.state.State] != '') {
                            fillColor = this.statusColors[this.state.State];
                        }
                        this.setFill(fillColor);

                        //画车体
                        context.beginPath();
                        context.rect(0, y, width, height);
                        context.closePath();
                        context.fillShape(this);

                        //画链条
                        var toothCount = 0, toothStartX = 0, toothRectWidth = 0;
                        while (toothCount * (chainSize * 2) < (width - chainSize / 2)) {
                            toothCount++;
                        }
                        this.toothCount = toothCount;
                        toothRectWidth = chainSize * toothCount + chainSize * (toothCount - 1);
                        toothStartX = (width - toothRectWidth) / 2;

                        if (this.runningOffset != undefined && this.runningOffset != null) {
                            toothStartX += this.runningOffset;
                        }

                        context.beginPath();
                        for (var i = 0; i < toothCount; i++) {
                            if (chainCount == 1) {
                                var p = { x: toothStartX + i * (chainSize * 2), y: y + height / 2 }
                                context.rect(p.x, p.y, chainSize, chainSize);

                            } else {
                                var interval = (height - chainPadding * 2 - chainSize) / (chainCount - 1);
                                for (var j = 0; j < chainCount; j++) {
                                    var p = { x: toothStartX + i * (chainSize * 2), y: y + chainPadding + interval * j };
                                    context.rect(p.x, p.y, chainSize, chainSize);
                                }
                            }
                        }
                        context.closePath();



                        this.setFill('#000');


                        context.fillShape(this);
                        //context.save();
                        //画货物(此处应该判断状态)
                        //var height = that.getShuttleHeight() - chainPadding * 2;
                        //var width = that.getWidth()*0.7;
                        //context.beginPath();
                        //context.rect((this.getWidth() - width) / 2, y + chainPadding, width, height);
                        //context.closePath();
                        //this.setFill('gray');
                        //context.fillShape(this);

                        if (this.state && this.state.State && (this.state.State == 3 || this.state.State == 5 || this.state.State == 9))
                        {
                            var c_width = width / 2;
                            var c_height = height / 2;
                            context.rect((width - c_width) / 2, (height - c_height) / 2, c_width, c_height);
                            context.fillShape(this);
                        }

                        //context.restore();
                        this.setFill(oldFillValue);
                    }
                });

                //car.on('click', function () {
                //    alert(this.CarName);
                //});
                car.getTaskModeName = function (v) {
                    switch (v) {
                        case 0:
                            return '无任务类型';
                        case 1:
                            return '全自动任务';
                        case 2:
                            return '取货任务';
                        case 3:
                            return '放货任务';
                        case 4:
                            return '有货行走';
                        case 5:
                            return '无货行走';
                        default:
                            return '未知模式';
                    }
                };
                car.getEventName = function (v) {
                    switch (v) {
                        case 0:
                            return '无货无任务';
                        case 1:
                            return '接到任务未运行';
                        case 2:
                            return '行走运行';
                        case 3:
                            return '到达起始点';
                        case 4:
                            return '到达目的地';
                        case 5:
                            return '执行让道任务';
                        case 6:
                            return '自动任务完成';
                        case 7:
                            return '手动报完成';
                        case 8:
                            return '交接货超时';
                        default:
                            return '未知事件';
                    }

                };
                car.statusColors = {
                    "0":"#ccc",//初始化
                    "1":"gray",//手动模式
                    "2": "#ccc",//无货待命
                    "3": "#ccc",//有货待命
                    "4":"green",//无货运行
                    "5": "green",//有货运行
                    "6":"#ccc",//停止
                    "7":"yellow",//手动报完成
                    "8":"red",//报警停机
                    "9":"#ccc",//有货
                };
                car.getStateName = function (v) {
                    switch (v) {
                        case 0:
                            return '初始化';
                        case 1:
                            return '手动模式';
                        case 2:
                            return '无货待命';
                        case 3:
                            return '有货待命';
                        case 4:
                            return '无货运行';
                        case 5:
                            return '有货运行';
                        case 6:
                            return '停止';
                        case 7:
                            return '手动报完成';
                        case 8:
                            return '报警停机';
                        case 9:
                            return '有货';
                        default:
                            return '未知状态';
                    }
                };
                car.CarName = carNames[carIndex];
                car.updateStatus = function (state) {
                    this.state = state;
                    if (!Kinetic.isDesign && $.qtip) {
                        //更新提示内容
                        var qtip = $(this.getTip()).qtip();
                        var thatCar = this;
                        if (qtip && qtip.hasOwnProperty('_id')) {
                            var tipId = qtip._id;
                            var status = state;
                            if (status) {
                                $('#' + tipId + ' label[name]').each(function (i, item) {
                                    if (status.hasOwnProperty($(item).attr('name'))) {
                                        var v = status[$(item).attr('name')];
                                        switch ($(item).attr('name')) {
                                            case "TaskMode":
                                                v = thatCar.getTaskModeName(v);
                                                break;
                                            case "Event":
                                                v = thatCar.getEventName(v);
                                                break;
                                            case "State":
                                                v = thatCar.getStateName(v);
                                                break;

                                        }
                                        $(item).text(v);
                                    } else {
                                        $(item).text('<属性不存在>');
                                    }
                                });

                                var y=that.getStationY(status.CurrentStation);
                                this.setY(y);
                            } else {
                                $('#' + tipId + ' label[name]').each(function (i, item) {
                                    $(item).text('-');
                                });
                            }
                        }
                    }
                    this.draw();
                };


                if (!Kinetic.isDesign && $.qtip) {

                    car.on('click', function () {
                        var layer = this.getLayer();

                        $(this.getTip()).qtip('show');

                    });


                    var tip = $('<div />').qtip({
                        content: {
                            text: function () {
                                return '<div style="line-height:1.8">' +
                                            '<label class="title" style = "width: 80px;display:inline-block;";>名称：</label><label name="Name" class="content">-</label><br />' +
                                            '<label class="title" style = "width: 80px;display:inline-block;";>状态：</label><label name="State" class="content">-</label><br />' +
                                            '<label class="title" style = "width: 80px;display:inline-block;";>事件：</label><label name="Event" class="content">-</label><br />' +
                                            '<label class="title" style = "width: 80px;display:inline-block;";>任务模式：</label><label name="TaskMode" class="content">-</label><br />' +
                                            '<label class="title" style = "width: 80px;display:inline-block;";>是否在站点：</label><label name="AtStation" class="content">-</label><br />' +
                                            '<label class="title" style = "width: 80px;display:inline-block;";>当前站点：</label><label name="CurrentStation" class="content">-</label><br />' +
                                            '<label class="title" style = "width: 80px;display:inline-block;";>位置：</label><label name="Position" class="content">-</label><br />' +
                                            '<label class="title" style = "width: 80px;display:inline-block;";>起始站：</label><label name="FromStation" class="content">-</label><br />' +
                                            '<label class="title" style = "width: 80px;display:inline-block;";>终点站：</label><label name="ToStation" class="content">-</label><br />' +
                                            '<label class="title" style = "width: 80px;display:inline-block;";>故障码：</label><label name="ErrorCode" class="content">-</label> <label name="ErrorName" class="content"></label><br />' +
                                            '<label class="title" style = "width: 80px;display:inline-block;";>任务号：</label><label name="TaskId" class="content">-</label><br />' +
                                        '</div>';
                            },
                            title: {
                                text: car.CarName,
                                button: true
                            }
                        },
                        position: {
                            my: 'center', at: 'center',
                            target: $(window),
                            adjust: {
                                scroll: false // Can be ommited (e.g. default behaviour)
                            }
                        },
                        show: {
                            ready: false,
                            modal: {
                                on: true,
                                blur: false
                            }
                        },
                        style: {
                            width: 250,
                            classes: 'dialogue',
                            tip: false
                        },
                        hide: false,
                        events: {
                            render: function (event, api) {
                                $('button', api.elements.content).click(function (e) {
                                    api.hide(e);
                                });

                                $(this).draggable({
                                    containment: 'window',
                                    handle: api.elements.titlebar
                                });
                            },
                            hide: function (event, api) {
                                //api.destroy();
                            }
                        }
                    });


                }

                car.tip = tip;
                car.getTip= function () {
                    return this.tip;
                },
                cars.push(car);

                if (carIndex == 0) {
                    car.setY(this.getHeight() / (carNames.length - 1) * carIndex);
                } else{
                    car.setY(this.getHeight() / (carNames.length - 1) * carIndex - car.getWidth());
                }
            }

            this.cars = cars;

            for (var i = 0; i < cars.length; i++) {
                this.add(cars[i]);
            }
        },
        getRailWayLayer: function () {
            return this.find('.railWay')[0];
        },
        getMaskLayer: function () {
            return this.find('.maskLayer')[0];
        },
        getShuttleLayer: function () {
            return this.find('.shuttle')[0];
        },
        resize: function () {
            if (this.getRailWayLayer()) {
                this.getRailWayLayer().size(this.getSize());
            }
            if (this.getMaskLayer()) {
                this.getMaskLayer().size(this.getSize());
            }
            this.draw();
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
            var proprities = ["railWayCount", "railWayWeigth", "getRailWayPadding","chainPadding","chainSize","chainCount", "width", "height","stations"];
            for (var i = 0; i < proprities.length; i++) {
                this.on(proprities[i]+'Change.kinetic', function () {
                    this.resize();
                });
            }

            //测试站点用
            this.on("currentStationChange.kinetic", function () {
                if (this.cars.length > 0) {
                    this.updateCarStatus(this.cars[0].CarName, { CurrentStation: this.getCurrentStation() });
                }
            });
            this.on("stationsChange.kinetic", function () {
                if (this.cars.length > 0) {
                    this.updateCarStatus(this.cars[0].CarName, { CurrentStation: this.getCurrentStation() });
                }
            });
        },
        run: function () {
            if (this.runningTimer && this.runningTimer != 0) {
                return;
            }

            var offset = 0;
            var that = this;

            that.runningTimer = setTimeout(function () {
                var shuttle = that.getShuttleLayer();
                if (!shuttle.runningOffset) {
                    shuttle.runningOffset = 0;
                }
                if (shuttle.direction == 'left') {
                    if (shuttle.runningOffset >= -that.getChainSize()) {
                        shuttle.runningOffset -= 0.2;
                    } else {
                        shuttle.runningOffset = that.getChainSize();
                    }
                } else {
                    if (shuttle.runningOffset <= that.getChainSize()) {
                        shuttle.runningOffset += 0.2;
                    } else {
                        shuttle.runningOffset = -that.getChainSize();
                    }
                }

                that.draw();

                if (that.runningTimer != 0) {
                    clearTimeout(that.runningTimer);
                    that.runningTimer = 0;
                }

                that.run.call(that);
                that.stopping = false;
            }, 30);
        },
        turnLeft: function () {
            var shuttle = this.getShuttleLayer();
            shuttle.direction = 'left';
            this.run();
        },
        turnRight: function () {
            var shuttle = this.getShuttleLayer();
            shuttle.direction = 'right';
            this.run();
        },
        getCarNames:function(){
            var names = this.getName();
            if (!names || names == '') {
                names = '一号穿梭车';
            }
            var cars = names.split(',');
            return cars;
        },
        stop: function () {
            if (this.runningTimer) {
                this.stopping = true;
                clearTimeout(this.runningTimer);
            }

            var shuttle = that.getShuttleLayer();
            if (shuttle) {
                shuttle.runningOffset = 0;
                this.draw();
            }
        },
        getStationY: function (station) {
            var stationConfig = this.getStations();
            if (!stationConfig || stationConfig == '') {
                return 0;
            }
            var stations = stationConfig.split(',');
            for (var i = 0; i < stations.length; i++) {
                var sv = stations[i].split(':');
                if (sv.length <= 1) {
                    continue;
                }

                if (sv[0] == station.toString()) {
                    return sv[1];
                }
            }

            return 0;
        },
        updateCarStatus: function (name, state) {
            for (var i = 0; i < this.cars.length; i++) {
                var car = this.cars[i];
                if (car.CarName == name) {
                    car.updateStatus(state);
                }
            }

            this.draw();
        }
    };

    Kinetic.Util.extend(Kinetic.JSShuttle, Kinetic.Group);

    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttle, 'railWayCount', 2);
    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttle, 'railWayWeigth', 3);
    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttle, 'railWayPadding', 10);
    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttle, 'shuttleHeight', 40);
    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttle, 'chainSize', 2);
    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttle, 'chainPadding', 3);
    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttle, 'chainCount', 2);
    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttle, 'stations', '1:0,2:50');
    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttle, 'currentStation', 1);
    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttle, 'editor',
    {
        "name": {
            text: '车辆',
            getter: 'getName',
            setter: 'setName'
        },
        "width": {
            text: '宽',
            getter: 'getWidth',
            setter: 'setWidth',
            parser: parseInt
        },
        "height": {
            text: '高',
            getter: 'getHeight',
            setter: 'setHeight',
            parser: parseInt
        },
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
        "rotate": {
            text: '旋转角度',
            getter: 'getRotation',
            setter: 'setRotation',
            parser: parseFloat
        },
        "railWayWeigth": {
            text: '轨道厚',
            getter: 'getRailWayWeigth',
            setter: 'setRailWayWeigth',
            parser: parseInt
        },
        "railWayCount": {
            text: '轨道数',
            getter: 'getRailWayCount',
            setter: 'setRailWayCount',
            parser: parseInt
        },
        "railWayPadding": {
            text: '轨道边距',
            getter: 'getRailWayPadding',
            setter: 'setRailWayPadding',
            parser: parseInt
        },
        "shuttleHeight": {
            text: '车体高',
            getter: 'getShuttleHeight',
            setter: 'setShuttleHeight',
            parser: parseInt
        },
        "chainSize": {
            text: '链条尺寸',
            getter: 'getChainSize',
            setter: 'setChainSize',
            parser: parseInt
        },
        "chainPadding": {
            text: '链条边距',
            getter: 'getChainPadding',
            setter: 'setChainPadding',
            parser: parseInt
        },
        "chainCount": {
            text: '链条数量',
            getter: 'getChainCount',
            setter: 'setChainCount',
            parser: parseInt
        },
        "stations": {
            text: '站点(站号:y轴)',
            getter: 'getStations',
            setter: 'setStations'
        },
        "currentStation": {
            text: '当前站点(测试)',
            getter: 'getCurrentStation',
            setter: 'setCurrentStation',
            parser: parseInt
        },
    });

    Kinetic.Collection.mapMethods(Kinetic.JSShuttle);
})();
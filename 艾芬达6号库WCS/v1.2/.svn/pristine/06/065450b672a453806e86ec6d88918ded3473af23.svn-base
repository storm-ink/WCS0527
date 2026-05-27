(function () {
    Kinetic.JSShuttle = function (config) {
        this.____init(config);
    };

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
            this.add(new Kinetic.Shape({
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
                    var y = (that.getHeight() - height) / 2;

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
                            context.rect(p.x,p.y, chainSize, chainSize);
                            
                        } else {
                            var interval = (height - chainPadding * 2 - chainSize) / (chainCount - 1);
                            for (var j = 0; j < chainCount; j++) {
                                var p = { x: toothStartX + i * (chainSize * 2), y: y + chainPadding + interval * j };
                                context.rect(p.x,p.y, chainSize, chainSize);
                            }
                        }
                    }
                    context.closePath();

                    this.setFill('#000');
                    context.fillShape(this);

                    //画货物(此处应该判断状态)
                    //var height = that.getShuttleHeight() - chainPadding * 2;
                    //var width = that.getWidth()*0.7;
                    //context.beginPath();
                    //context.rect((this.getWidth() - width) / 2, y + chainPadding, width, height);
                    //context.closePath();
                    //this.setFill('gray');
                    //context.fillShape(this);

                    this.setFill(oldFillValue);
                }
            }));
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
            var proprities = ["railWayCount", "railWayWeigth", "getRailWayPadding","chainPadding","chainSize","chainCount", "width", "height"];
            for (var i = 0; i < proprities.length; i++) {
                this.on(proprities[i]+'Change.kinetic', function () {
                    this.resize();
                });
            }
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
    Kinetic.Factory.addGetterSetter(Kinetic.JSShuttle, 'editor',
    {
        "name": {
            text: '名称',
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
    });

    Kinetic.Collection.mapMethods(Kinetic.JSShuttle);
})();
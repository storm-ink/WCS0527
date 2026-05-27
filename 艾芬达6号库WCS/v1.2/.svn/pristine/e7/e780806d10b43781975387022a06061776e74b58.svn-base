(function () {
    function addAnchor(group, x, y, name, size) {
        var anchor = new Kinetic.Rect({
            x: x,
            y: y,
            stroke: 'red',
            fill: 'red',
            strokeWidth: 1,
            name: name,
            draggable: true,
            dragOnTop: false,
            width: size,
            height: size,
        });

        anchor.on('dragmove', function () {
            update(this);
            this.getLayer().draw();
        });
        anchor.on('mousedown touchstart', function () {
            group.setDraggable(false);
            this.moveToTop();
        });
        anchor.on('dragend', function () {
            group.setDraggable(true);
            this.getLayer().draw();
        });
        // add hover styling
        anchor.on('mouseover', function () {
            var layer = this.getLayer();
            switch (this.name()) {
                case 'topLeft':
                    document.body.style.cursor = 'nw-resize';
                    break;
                case 'topRight':
                    document.body.style.cursor = 'ne-resize';
                    break;
                case 'bottomRight':
                    document.body.style.cursor = 'se-resize';
                    break;
                case 'bottomLeft':
                    document.body.style.cursor = 'sw-resize';
                    break;
            }

            this.setStrokeWidth(4);
            layer.getLayer().draw();
        });
        anchor.on('mouseout', function () {
            var layer = this.getLayer();
            document.body.style.cursor = 'default';
            this.strokeWidth(2);
            this.getLayer().draw();
        });

        group.add(anchor);
    }

    function update(activeAnchor) {
        var group = activeAnchor.getParent();

        var topLeft = group.find('.topLeft')[0];
        var topRight = group.find('.topRight')[0];
        var bottomRight = group.find('.bottomRight')[0];
        var bottomLeft = group.find('.bottomLeft')[0];
        var image = group.getBody();

        var anchorX = activeAnchor.x();
        var anchorY = activeAnchor.y();

        // update anchor positions
        switch (activeAnchor.name()) {
            case 'topLeft':
                topRight.y(anchorY);
                bottomLeft.x(anchorX);
                break;
            case 'topRight':
                topLeft.y(anchorY);
                bottomRight.x(anchorX);
                break;
            case 'bottomRight':
                bottomLeft.y(anchorY);
                topRight.x(anchorX);
                break;
            case 'bottomLeft':
                bottomRight.y(anchorY);
                topLeft.x(anchorX);
                break;
        }

        image.setPosition(topLeft.getPosition());

        var width = topRight.x() - topLeft.x();
        var height = bottomLeft.y() - topLeft.y();
        if (width && height) {
            //image.setSize({ width: width, height: height });
            group.setSize({ width: width, height: height });
        }
    }

    function updateAnchorPosition(group) {
        var topLeft = group.find('.topLeft')[0];
        var topRight = group.find('.topRight')[0];
        var bottomRight = group.find('.bottomRight')[0];
        var bottomLeft = group.find('.bottomLeft')[0];

        var anchorSize = 4;
        var bodyWidth = group.getWidth();
        var bodyHeight = group.getHeight();

        topLeft.setPosition({ x: 0, y: 0 });
        topRight.setPosition({ x: bodyWidth - anchorSize, y: 0 });
        bottomRight.setPosition({ x: bodyWidth - anchorSize, y: bodyHeight - anchorSize });
        bottomLeft.setPosition({ x: 0, y: bodyHeight - anchorSize });
    }


    function addTip(group) {
        var tooltip = new Kinetic.Label({
            opacity: 0.75,
            visible: false,
            listening: false,
            name: 'tooltip'
        });
        tooltip.add(new Kinetic.Tag({
            fill: 'black',
            pointerDirection: 'left',
            pointerWidth: 10,
            pointerHeight: 10,
            lineJoin: 'round',
            shadowColor: 'black',
            shadowBlur: 10,
            shadowOffset: { x: 10, y: 10 },
            shadowOpacity: 0.5
        }));

        tooltip.add(new Kinetic.Text({
            text: '',
            fontFamily: 'Calibri',
            fontSize: 12,
            padding: 5,
            lineHeight: 1.5,
            fill: 'white'
        }));

        group.add(tooltip);

        group.on('mousemove', function () {
            var tooltip = this.find('.tooltip')[0];
            //var mousePos = this.getStage().getPointerPosition();
            var org = this.getPosition();
            org.x += this.getWidth();
            var JSConveyorLocation = tooltip.getParent();

            var text = '货位号:' + JSConveyorLocation.getUserCode();
            switch (JSConveyorLocation.getStatus()) {
                case 0:
                    text += '\n状态:初始化';
                    break;
                case 1:
                    text += '\n状态:报警';
                    break;
                case 2:
                    text += '\n状态:离线';
                    break;
                case 3:
                    text += '\n状态:手动';
                    break;
                case 4:
                    text += '\n状态:停止';
                    break;
                case 5:
                    text += '\n状态:运行';
                    break;
            }

            var occupy = JSConveyorLocation.getOccupy();
            if (occupy && typeof (occupy) == 'object') {
                var occupyText = "光电:";
                if (occupy.FroProPotocell) {
                    occupyText += "前保护,";
                }
                if (occupy.FroPosPotocell) {
                    occupyText += "前到位,";
                }
                if (occupy.FroSloPotocell) {
                    occupyText += "前减速,";
                }
                if (occupy.AftProPotocell) {
                    occupyText += "后保护,";
                }
                if (occupy.AftPosPotocell) {
                    occupyText += "后到位,";
                }
                if (occupy.AftSloPotocell) {
                    occupyText += "后减速,";
                }
                if (occupy.UpPotocell) {
                    occupyText += "后高位,";
                }
                if (occupy.DownPotocell) {
                    occupyText += "后低位,";
                }

                if (occupyText.length > 3) {
                    occupyText = '\n' + occupyText.substring(0, occupyText.length - 1);
                } else {
                    occupyText = "";
                }
                text += occupyText;
            }

            var alarm=JSConveyorLocation.getAlarm();
            if(alarm && typeof(alarm)=='object'){
                var alarmText = "报警:";
                if(alarm.Manual){
                    alarmText+='手动,';
                }
                if(alarm.Isolator){
                    alarmText+='离线,';
                }
                if(alarm.Breaker){
                    alarmText+='电路保护器断开,';
                }
                if(alarm.Photocell){
                    alarmText+='光电异常,';
                }
                if(alarm.RunOvertime){
                    alarmText+='运行超时,';
                }
                if(alarm.OccupyOvertime){
                    alarmText+='占位超时,';
                }
                if(alarm.TaskNoGoods){
                    alarmText+='有任务无货,';
                }
                if(alarm.X_MotorVAF){
                    alarmText+='X轴电机变频器故障,';
                }
                if(alarm.Y_MotorVAF){
                    alarmText+='Y轴电机变频器故障,';
                }
                if(alarm.X_MotorContactor){
                    alarmText+='X轴电机正反转接触器故障,';
                }
                if(alarm.X_MotorBraker){
                    alarmText+='X轴电机正反转接触器故障,';
                }
                if(alarm.Y_MotorContactor){
                    alarmText+='Y轴电机正反转接触器故障,';
                }
                if(alarm.Y_MotorBraker){
                    alarmText+='Y轴电机抱闸接触器故障,';
                }
                if(alarm.Lift_MotorContactor){
                    alarmText+='顶升电机正反转接触器故障,';
                }
                if(alarm.Lift_MotorBraker){
                    alarmText+='顶升电机抱闸接触器故障,';
                }
                if (alarmText.length > 3) {
                    alarmText = '\n' + alarmText.substring(0, alarmText.length - 1);
                } else {
                    alarmText = "";
                }
                text += alarmText;
            }

            tooltip.setRotation(360 - JSConveyorLocation.getRotation());
            tooltip.setX(this.getWidth());

            tooltip.getText().setText(text);

            tooltip.show();

            tooltip.getLayer().draw();
        });

        group.on('mouseout', function () {
            var tooltip = this.find('.tooltip')[0];
            tooltip.hide();
            tooltip.getLayer().draw();
        });
    }

    var ATTR_CHANGE_LIST = ['fontFamily', 'fontSize', 'fontStyle', 'padding', 'lineHeight', 'text'],
        CHANGE_KINETIC = 'Change.kinetic',

     // cached variables
     attrChangeListLen = ATTR_CHANGE_LIST.length;

    Kinetic.JSConveyorLocation = function (config) {
        this.____init(config);
    };

    Kinetic.JSConveyorLocation.prototype = {
        ____init: function (config) {
            var that = this;

            this.className = 'JSConveyorLocation';
            Kinetic.Group.call(this, config);

            this.on('add.kinetic', function (evt) {
                that._addListeners(evt.child);
                that._sync();
            });

            //用于选中
            this.add(new Kinetic.Rect({
                draggable: false,
                'fill': '#fff',
                'stroke': '#fff',
                'strokeWidth': 1,
                'width': this.getWidth(),
                'height': this.getHeight(),
                'opacity': 0,
                'name': 'maskLayer'
            }));

            //添加主体
            this.add(new Kinetic.Shape({
                width: config.width,
                height: config.height,
                fill: '#000',
                stroke: '#000',
                strokeWidth: 2,
                draggable: false,
                name: 'body',
                hitFunc : function (context) {
                    context.beginPath();
                    context.rect(0, 0, this.getWidth(), this.getHeight());
                    context.closePath();
                    context.fill();
                },
                sceneFunc: function (context) {
                    var offset = this.runningOffset;
                    if (!offset) {
                        offset = 0;
                    }

                    var strokeWidth = this.strokeWidth();
                    var width = this.getWidth();
                    var height = this.getHeight();

                    context.beginPath();
                    //格式
                    for (var y = strokeWidth - offset ; y < height; y = y + (strokeWidth + strokeWidth / 2) * 2) {

                        context.moveTo(0, y);
                        context.lineTo(width, y);
                    }
                    context.closePath();
                    context.clearRect(0, 0, width, height);

                    try {
                        if (that.getStatus()!= null && that.getStatus() != undefined) {
                            this.setStroke(that.getStatusColors()[that.getStatus().toString()]);
                        }
                    } catch (e) {
                        console.warn(e.message);
                    }

                    context.fillStrokeShape(this);
                }
            }));

            //添加文本
            this.add(new Kinetic.Text({
                fontFamily: 'Calibri',
                fontSize: 12,
                padding: 5,
                lineHeight: 1.5,
                width: config.width,
                fill: 'red',
                align: 'center',
                name: 'text',
            }));

            if (Kinetic.isDesign) {
                //添加缩放角
                var anchorSize = 4;
                addAnchor(this, 0, 0, 'topLeft', anchorSize);
                addAnchor(this, config.width - anchorSize, 0, 'topRight', anchorSize);
                addAnchor(this, config.width - anchorSize, config.height - anchorSize, 'bottomRight', anchorSize);
                addAnchor(this, 0, config.height - anchorSize, 'bottomLeft', anchorSize);

            }
            //添加方向
            this.add(new Kinetic.Shape({
                sceneFunc: function (context) {

                    var bodyWidth = this.getParent().getWidth();
                    var bodyHeight = this.getParent().getHeight();

                    var width = bodyWidth / 2;
                    var height = bodyHeight / 2;

                    var jtHeight = width / 2;//箭头高
                    var zzWidth = width / 2;//柱体宽

                    context.beginPath();

                    if (this.getParent().getDirection() == 1) {

                        var l = height - jtHeight;
                        var dy = (bodyHeight - height) / 2;//顶点y
                        var dx = bodyWidth / 2;//顶点x

                        context.moveTo(dx, dy);//顶点
                        context.lineTo(bodyWidth / 2 - width / 2, dy + jtHeight);//左上
                        context.lineTo(dx - zzWidth / 2, dy + jtHeight);//左中
                        context.lineTo(dx - zzWidth / 2, dy + jtHeight + l);//左下

                        context.lineTo(dx + zzWidth / 2, dy + jtHeight + l);//右下
                        context.lineTo(dx + zzWidth / 2, dy + jtHeight);//右中
                        context.lineTo(dx + width / 2, dy + jtHeight);//右上
                        context.lineTo(dx, dy);//顶点

                    } else { //2个方向
                        var l = height - jtHeight * 2;
                        var dy = (bodyHeight - height) / 2;//顶点y
                        var dx = bodyWidth / 2;//顶点x

                        var dy1 = dy + jtHeight * 2 + l;
                        var dx1 = dx;

                        context.moveTo(dx, dy);//顶点
                        context.lineTo(bodyWidth / 2 - width / 2, dy + jtHeight);//左上
                        context.lineTo(dx - zzWidth / 2, dy + jtHeight);//左中
                        context.lineTo(dx - zzWidth / 2, dy + jtHeight + l);//左下

                        context.lineTo(bodyWidth / 2 - width / 2, dy + jtHeight + l);//左下2
                        context.lineTo(dx1, dy1);//下顶点
                        context.lineTo(dx + width / 2, dy + jtHeight + l);//左下2

                        context.lineTo(dx + zzWidth / 2, dy + jtHeight + l);//右下
                        context.lineTo(dx + zzWidth / 2, dy + jtHeight);//右中
                        context.lineTo(dx + width / 2, dy + jtHeight);//右上
                        context.lineTo(dx, dy);//顶点

                    }
                    context.closePath();
                    context.fillStrokeShape(this);
                },
                fill: '#00D2FF',
                stroke: 'black',
                strokeWidth: 1,
                name: 'direction'
            }));

            //添加货物
            this.add(new Kinetic.Shape({
                sceneFunc: function (context) {
                    var task = this.getParent().getTask();
                    if (task && task.TaskNo && task.TaskNo != 0) {
                        var bodyWidth = this.getParent().getWidth();
                        var bodyHeight = this.getParent().getHeight();

                        var width = bodyWidth / 2;
                        var height = bodyHeight / 2;
                        context.beginPath();
                        context.rect((bodyWidth - width) / 2, (bodyHeight - height) / 2, width, height);
                        context.closePath();
                        context.fillStrokeShape(this);
                    }
                },
                fill: 'gray',
                stroke: 'gray',
                strokeWidth: 1,
                name:'box'
            }));
            
            this.getDirectionShape().setVisible(this.getShowDirection());
        },
        getText: function () {
            return this.find('.text')[0];
        },
        getBody: function () {
            return this.find('.body')[0];
        },
        getDirectionShape: function () {
            return this.find('.direction')[0];
        },
        //重写序列化方法
        toObject: function () {
            var obj = Kinetic.Node.prototype.toObject.call(this);

            obj.children = [];

            //var children = this.getChildren();
            //var len = children.length;
            //for (var n = 0; n < len; n++) {
            //    var child = children[n];
            //    obj.children.push(child.toObject());
            //}

            return obj;
        },
        run: function () {
            if (this.runningTimer && this.runningTimer != 0) {
                return;
            }

            var offset=0;
            var that = this;

            that.runningTimer = setTimeout(function () {
                var body = that.getBody();
                if (!body.runningOffset) {
                    body.runningOffset = 0;
                }
                var strokeWidth = body.strokeWidth();
                if (body.runningOffset <= strokeWidth+0.5) {
                    body.runningOffset += 0.1;
                } else {
                    body.runningOffset = -strokeWidth-1.5;
                }

                if (!body.oldFill) {
                    body.oldFill = body.getStroke();
                }
                body.stroke('green');
                //body.draw();//.sceneFunc.call(body, body.getContext(), offset);


                body.getLayer().batchDraw();

                if (that.runningTimer != 0) {
                    clearTimeout(that.runningTimer);
                    that.runningTimer = 0;
                }
                if (!that.stopping && that.getStatus() == 5) {
                    that.run.call(that);
                } else {
                    if (body.oldFill) {
                        body.stroke(body.oldFill);
                        body.getLayer().batchDraw();
                    }
                }
                that.stopping = false;
            }, 30);
        },
        stop: function () {
            if (this.runningTimer) {
                this.stopping = true;
                clearTimeout(this.runningTimer);
            }

            var body = this.getBody();
            if (body) {
                body.runningOffset = 0;
                if (body.oldFill) {
                    body.stroke(body.oldFill);
                    body.getLayer().batchDraw();
                }
            }
        },
        _addListeners: function (text) {
            var that = this,
                n;
            var func = function () {
                that._sync();
            };

            // update text data for certain attr changes
            for (n = 0; n < attrChangeListLen; n++) {
                text.on(ATTR_CHANGE_LIST[n] + CHANGE_KINETIC, func);
            }

            this.on('widthChange.kinetic', function () {
                if (this.getBody()) {
                    this.getBody().setWidth(this.getWidth());
                    if (Kinetic.isDesign) {
                        updateAnchorPosition(this);
                    }
                }
            });

            this.on('heightChange.kinetic', function () {
                if (this.getBody()) {
                    this.getBody().setHeight(this.getHeight());
                    if (Kinetic.isDesign) {
                        updateAnchorPosition(this);
                    }
                }
            });

            this.on('directionChange.kinetic', function () {
                if (this.getDirectionShape()) {
                    this.getDirectionShape().drawScene();
                }
            });

            this.on('showDirectionChange.kinetic', function () {
                if (this.getDirectionShape()) {
                    this.getDirectionShape().setVisible(this.getShowDirection());
                }
            });

            this.on('statusChange.kinetic', function () {
                if (this.getStatus() == 5) {
                    this.run();
                } else {
                    this.stop();
                }
            });
        },
        _sync: function () {
            var text = this.getText();
            if (text) {
                text.setAttrs({
                    y:this.getHeight()/2
                });
            }
        }
    };

    Kinetic.Util.extend(Kinetic.JSConveyorLocation, Kinetic.Group);

    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorLocation, 'deviceCode', 0);
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorLocation, 'userCode', '00-000-000');
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorLocation, 'status', 0);
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorLocation, 'direction', 1);
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorLocation, 'showDirection', true);
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorLocation, 'task',
    {
        "TaskNo":0,
        "TUID":0,
        "Str_Rcv_X":false,
        "Fnh_Rcv_X":false,
        "Rqs_Snt":false,
        "Rcv_Rdy":false,
        "Str_Rcv_Y":false,
        "Fnh_Rcv_Y":false
    });
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorLocation, 'occupy',
    {
        "PhocllUseStatus": 0,
        "FroProPotocell": 0,
        "FroPosPotocell": 0,
        "FroSloPotocell": 0,
        "AftProPotocell": 0,
        "AftPosPotocell": 0,
        "AftSloPotocell": 0,
        "UpPotocell": 0,
        "DownPotocell": 0
    });
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorLocation, 'alarm',
    {
        "PosNo": 0,
        "Manual": 0,
        "Isolator": 0,
        "Breaker": 0,
        "Photocell": 0,
        "RunOvertime": 0,
        "OccupyOvertime": 0,
        "TaskNoGoods": 0,
        "MotorUseStatus": 0,
        "X_MotorVAF": 0,
        "Y_MotorVAF": 0,
        "X_MotorContactor": 0,
        "X_MotorBraker": 0,
        "Y_MotorContactor": 0,
        "Y_MotorBraker": 0,
        "Lift_MotorContactor": 0,
        "Lift_MotorBraker": 0,
        "Spare": 0
    });

    //状态颜色
    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorLocation, 'statusColors',
    {
        //初始化
        "0": '#ccc',
        //报警
        "1": 'red',
        //离线
        "2": 'gray',
        //手动
        "3": 'yellow',
        //无货运行
        "4": '#ccc',
        //运行
        "5": 'green'
    });

    Kinetic.Factory.addGetterSetter(Kinetic.JSConveyorLocation, 'editor',
    {
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
        "deviceCode": {
            text: '设备编码',
            getter: 'getDeviceCode',
            setter: 'setDeviceCode'
        },
        "userCode": {
            text: '用户编码',
            getter: 'getUserCode',
            setter: 'setUserCode'
        },
        "rotate": {
            text: '旋转角度',
            getter: 'getRotation',
            setter: 'setRotation',
            parser:parseFloat
        },
        "x": {
            text: 'x',
            getter: 'getX',
            setter: 'x',
            parser:parseInt
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
        "showDirection":{
            text: '是否显示方向',
            getter: 'getShowDirection',
            setter: 'setShowDirection',
            parser: function (v) {
                if(v.toLowerCase()=='true'){
                    return true;
                }

                if(v.toLowerCase()=='false'){
                    return false;
                }

                try {
                    if(parseInt(v)!=0){
                        return true;
                    }
                    else{
                        return false;
                    }
                } catch (e) {
                    
                }
                if (v) {
                    return true;
                } else {
                    return false;
                }
            }
        },
        "direction": {
            text: '方向数量',
            getter: 'getDirection',
            setter: 'setDirection',
            parser: function (v) {
                try {
                    v = parseInt(v);
                } catch (e) {
                    v = 1;
                }
                return v;
            }
        },
        "status": {
            text: '状态',
            getter: 'getStatus',
            setter: 'setStatus',
            parser: parseInt
        }
    });
    
    Kinetic.Collection.mapMethods(Kinetic.JSConveyorLocation);
})();
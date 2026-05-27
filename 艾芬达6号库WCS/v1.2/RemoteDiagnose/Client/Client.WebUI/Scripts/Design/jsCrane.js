(function () {
    if (!Kinetic.JSCraneTypes) {
        Kinetic.JSCraneTypes = [];
    }

    Kinetic.JSCraneTypes.push('JSCrane');

    Kinetic.JSCrane = function (config) {
        this.____init(config);
    };

    Kinetic.JSCrane.prototype = {
        ____init: function (config) {

            //config.hitFunc = function (context) {
            //    context.beginPath();
            //    context.rect(0, 0, this.getWidth(), this.getHeight());
            //    context.closePath();
            //    context.fillStrokeShape(this);
            //};

            var that = this;
            this.className = 'JSCrane';
            Kinetic.Group.call(this, config);

            this.resize();

            this.on('add.kinetic', function (evt) {
                that._addListeners(evt.child);
                that._sync();
            });

            if (Kinetic.isDesign) {
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
            }

            var rackLayer = new Kinetic.Shape({
                draggable: false,
                'fill': 'green',
                'stroke': 'red',
                'strokeWidth': 1,
                'width': this.getWidth(),
                'height': this.getHeight(),
                'opacity': 0.3,
                'name': 'rack',
                sceneFunc: function (context) {

                    //地轨
                    var railWeight = that.getRailWeight();
                    context.beginPath();
                    context.rect(0, (that.getHeight() - railWeight) / 2, that.getWidth(), railWeight);
                    context.closePath();
                    context.fill();

                    var leftHiddenColumns = {};
                    var leftHiddenColumnsValues = that.getLeftHiddenColumns().split(',');
                    for (var i = 0; i < leftHiddenColumnsValues.length; i++) {
                        if (leftHiddenColumnsValues[i] == '') {
                            continue;
                        }

                        leftHiddenColumns[leftHiddenColumnsValues[i]] = 1;
                    }

                    var rightHiddenColumns = {};
                    var rightHiddenColumnsValues = that.getRightHiddenColumns().split(',');
                    for (var i = 0; i < rightHiddenColumnsValues.length; i++) {
                        if (leftHiddenColumnsValues[i] == '') {
                            continue;
                        }

                        rightHiddenColumns[rightHiddenColumnsValues[i]] = 1;
                    }


                    this.columns = [];


                    //左列
                    var leftColumnStartOffsetX = that.getColumnStartOffsetX();
                    var columnDeep = that.getColumnDeep();
                    var columnWidth = that.getColumnWidth();
                    var leftColumnY = columnDeep;

                    for (var i = 0; i < that.getColumnCount() ; i++) {
                        this.columns[i] = { x: leftColumnStartOffsetX + columnWidth * i, y: leftColumnY };

                        if (leftHiddenColumns.hasOwnProperty(i.toString())) {
                            continue;
                        }

                        context.moveTo(leftColumnStartOffsetX + columnWidth * i, leftColumnY);
                        context.lineTo(leftColumnStartOffsetX + columnWidth * i, 0);
                        context.lineTo(leftColumnStartOffsetX + columnWidth * i + columnWidth, 0);
                        context.lineTo(leftColumnStartOffsetX + columnWidth * i + columnWidth, leftColumnY);

                    }

                    //右列
                    var rightColumnStartOffsetX = that.getColumnStartOffsetX();
                    var rightColumnY = that.getHeight() - columnDeep;

                    for (var i = 0; i < that.getColumnCount() ; i++) {
                        if (rightHiddenColumns.hasOwnProperty(i.toString())) {
                            continue;
                        }

                        context.moveTo(rightColumnStartOffsetX + columnWidth * i, rightColumnY);
                        context.lineTo(rightColumnStartOffsetX + columnWidth * i, rightColumnY + columnDeep);
                        context.lineTo(rightColumnStartOffsetX + columnWidth * i + columnWidth, rightColumnY + columnDeep);
                        context.lineTo(rightColumnStartOffsetX + columnWidth * i + columnWidth, rightColumnY);

                    }

                    context.stroke(this);

                }
            });

            this.add(rackLayer);

            //机体
            var craneDevice = new Kinetic.Shape({
                draggable: false,
                'fill': '#ccc',
                'stroke': '#ccc',
                'strokeWidth': 1,
                'width': this.getCraneWidth(),
                'height': this.getHeight(),
                'name': 'craneDevice',
                'y': 0,
                sceneFunc: function (context) {
                    var forkPosition = that.getColumnPosition();
                    var forkHeight = 2;//货叉突出2px
                    var craneHeight = that.getLanewayWidth() - 2 * 2 - forkHeight * 2;//机体厚度等于巷道宽去掉两侧2px的空隙
                    var forkWidth = that.getColumnWidth(); //货叉宽与货加列宽一致

                    var startX = forkPosition.x - (this.getWidth() - forkWidth) / 2;
                    var startY = that.getColumnDeep() + forkHeight + 2;

                    var forkScale = 1;
                    var forkHorizontalPosition = 0;
                    if (that.getStatus()) {
                        forkHorizontalPosition = that.getStatus().ForkHorizontalPosition;
                        switch (forkHorizontalPosition) {
                            case 2:
                            case 4:
                                forkScale = 0.5;
                                break;
                            case 3:
                            case 5:
                                forkScale = 0.3;
                                break;
                            case 6:
                            case 8:
                                forkScale = 0.3;
                                break;
                            case 7:
                            case 9:
                                forkScale = 0;
                                break;
                            default:
                                forkScale = 1;
                        }
                    }

                    context.beginPath();

                    context.moveTo(startX, startY);
                    context.lineTo(startX + (this.getWidth() - forkWidth) / 2, startY);
                    //左伸叉
                    if (forkHorizontalPosition == 2 || forkHorizontalPosition == 3 || forkHorizontalPosition == 6 || forkHorizontalPosition == 7) {
                        context.lineTo(startX + (this.getWidth() - forkWidth) / 2, that.getColumnDeep() * forkScale);
                        context.lineTo(startX + (this.getWidth() - forkWidth) / 2 + forkWidth, that.getColumnDeep() * forkScale);
                    } else {
                        context.lineTo(startX + (this.getWidth() - forkWidth) / 2, startY - forkHeight);
                        context.lineTo(startX + (this.getWidth() - forkWidth) / 2 + forkWidth, startY - forkHeight);
                    }
                    context.lineTo(startX + (this.getWidth() - forkWidth) / 2 + forkWidth, startY);
                    context.lineTo(startX + this.getWidth(), startY);


                    context.lineTo(startX + this.getWidth(), startY + craneHeight);
                    context.lineTo(startX + (this.getWidth() - forkWidth) / 2 + forkWidth, startY + craneHeight);
                    //右伸叉
                    if (forkHorizontalPosition == 4 || forkHorizontalPosition == 5 || forkHorizontalPosition == 8 || forkHorizontalPosition == 9) {
                        context.lineTo(startX + (this.getWidth() - forkWidth) / 2 + forkWidth, this.getHeight() - that.getColumnDeep() * forkScale);
                        context.lineTo(startX + (this.getWidth() - forkWidth) / 2, this.getHeight() - that.getColumnDeep() * forkScale);
                    } else {
                        context.lineTo(startX + (this.getWidth() - forkWidth) / 2 + forkWidth, startY + craneHeight + forkHeight);
                        context.lineTo(startX + (this.getWidth() - forkWidth) / 2, startY + craneHeight + forkHeight);
                    }

                    context.lineTo(startX + (this.getWidth() - forkWidth) / 2, startY + craneHeight);
                    context.lineTo(startX, startY + craneHeight);
                    context.lineTo(startX, startY);

                    context.closePath();
                    try {
                        if (that.getStatus() && that.getStatus().State != null && that.getStatus().State != undefined) {
                            this.setFill(that.getStatusColors()[that.getStatus().State.toString()]);
                        }
                    } catch (e) {
                        console.warn(e.message);
                    }
                    context.fillStrokeShape(this);
                }
            });

            if (!Kinetic.isDesign && $.qtip) {

                craneDevice.on('click', function () {
                    var layer = this.getLayer();

                    $(that.getTip()).qtip('show');

                });


                var tip = $('<div />').qtip({
                    content: {
                        text: function () {
                            return '<div style="line-height:1.8">' +
                                        '<label class="title" style = "width: 80px;display:inline-block;";>名称：</label><label name="Name" class="content">-</label><br />' +
                                        '<label class="title" style = "width: 80px;display:inline-block;";>状态：</label><label name="State" class="content">-</label><br />' +
                                        '<label class="title" style = "width: 80px;display:inline-block;";>事件：</label><label name="Event" class="content">-</label><br />' +
                                        '<label class="title" style = "width: 80px;display:inline-block;";>是否在站点：</label><label name="AtStation" class="content">-</label><br />' +
                                        '<label class="title" style = "width: 80px;display:inline-block;";>当前列：</label><label name="UserColumn" class="content">-</label><br />' +
                                        '<label class="title" style = "width: 80px;display:inline-block;";>当前层：</label><label name="UserLevel" class="content">-</label><br />' +
                                        '<label class="title" style = "width: 80px;display:inline-block;";>货叉位置：</label><label name="ForkHorizontalPosition" class="content">-</label><br />' +
                                        '<label class="title" style = "width: 80px;display:inline-block;";>载货台位置：</label><label name="ForkVerticalPosition" class="content">-</label><br />' +
                                        '<label class="title" style = "width: 80px;display:inline-block;";>故障码：</label><label name="ErrorCode" class="content">-</label> <label name="ErrorName" class="content"></label><br />' +
                                        '<label class="title" style = "width: 80px;display:inline-block;";>任务号：</label><label name="TaskId" class="content">-</label><br />' +
                                    '</div>';
                            /*return '<div class="CraneControlBox">'+
                                   '     <div class="infoBox">'+
                                   '         <label name="Name" class="CraneName">-</label>' +
                                   '         <div class="userColumn">列：<label name="UserColumn">-</label></div>' +
                                   '         <div class="userLevel">层：<label name="UserLevel">-</label></div>' +
                                   '         <div class="taskId">任务号：<label name="TaskId">-</label></div>' +
                                   '         <div class="userColumn">货叉：<label name="ForkHorizontalPosition">-</label></div>' +
                                   '         <div class="userLevel">载货台：<label name="ForkVerticalPosition">-</label></div>' +
                                   '         <div class="taskId">错误码：<label name="ErrorCode">0000</label> <label name="ErrorName"></label></div>' +
                                   '         <div class="state">状态：<label name="State">-</label></div>' +
                                   '         <div class="event">事件：<label name="Event">-</label></div>' +
                                   '     </div>'+
                                   '     <table border="0" cellspacing="0" cellpadding="0" class="controlTable">'+
                                   '         <tbody><tr>'+
                                   '             <td style="width:20%"></td>'+
                                   '             <td style="width:20%">左</td>'+
                                   '             <td style="width:20%">列</td>'+
                                   '             <td style="width:20%">层</td>'+
                                   '             <td style="width:20%">右</td>'+
                                   '         </tr>'+
                                   '         <tr>'+
                                   '             <td style="width:20%">命令操作</td>'+
                                   '             <td style="width:20%"><input type="button" class="leftPick"></td>'+
                                   '             <td style="width:20%"><input type="text" class="num"></td>'+
                                   '             <td style="width:20%"><input type="text" class="num"></td>'+
                                   '             <td style="width:20%"><input type="button" class="rightPick"></td>'+
                                   '         </tr>'+
                                   '         <tr>'+
                                   '             <td style="width:20%"></td>'+
                                   '             <td style="width:20%"><input type="button" class="leftPut"></td>'+
                                   '             <td style="width:20%"></td>'+
                                   '             <td style="width:20%"><input type="button" class="running" value="运行"></td>'+
                                   '             <td style="width:20%"><input type="button" class="rightPut"></td>'+
                                   '         </tr>'+            
                                   '         <tr>'+
                                   '             <td style="width:20%">单步操作</td>'+
                                   '             <td style="width:20%"></td>'+
                                   '             <td style="width:20%"><input type="button" class="up"></td>'+
                                   '             <td style="width:20%"></td>'+
                                   '             <td style="width:20%"></td>'+
                                   '         </tr> '+
                                   '         <tr>'+
                                   '             <td style="width:20%"></td>'+
                                   '             <td style="width:20%"><input type="button" class="left"></td>'+
                                   '             <td style="width:20%"><input type="button" class="down"></td>'+
                                   '             <td style="width:20%"><input type="button" class="right"></td>'+
                                   '             <td style="width:20%"></td>'+
                                   '         </tr>'+            
                                   '         <tr style="height:100px;vertical-align:bottom;">'+
                                   '             <td style="width:20%"></td>'+
                                   '             <td style="width:20%"><input type="button" class="lock" value="锁定"></td>'+
                                   '             <td style="width:20%"><input type="button" class="jt" value="急停"></td>'+
                                   '             <td style="width:20%"></td>'+
                                   '             <td style="width:20%"><input type="button" class="yd" value="原点"></td>'+
                                   '         </tr>'+
                                   '     </tbody></table>' +
                                   ' </div>';*/
                        },         
                        title: { 
                            text: that.getName(),
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
                        //width: 250,
                        //width: 630,
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
                        hide: function (event, api)
                        {
                            //api.destroy();
                        }
                    }
                });


            }

            this.tip = tip;

            this.add(craneDevice);
        },
        getTip: function () {
            return this.tip;
        },
        getRackLayer: function () {
            return this.find('.rack')[0];
        },
        getCraneLayer: function () {
            return this.find('.craneDevice')[0];
        },
        getMaskLayer: function () {
            return this.find('.maskLayer')[0];
        },
        getForkHorizontalPositionName:function(v) {
            switch (v) {
                case 1:
                    return '中位';
                                case 2:
                    return '左位';
                                case 3:
                    return '左极限';
                                case 4:
                    return '右位';
                                case 5:
                    return '右极限';
                                case 6:
                    return '"左（外）';
                                case 7:
                    return '左（外）极限';
                                case 8:
                    return '右（外）';
                                case 9:
                    return '右（外）极限';
                                default:
                    return v;
                }
        },
        getForkVerticalPositionName:function(v)
        {
            switch (v) {
                case 0:
                    return '中位';
                case 1:
                    return '高位';
                case 2:
                    return '低位';
                default:
                    return v;
            }
        },
        getEventName:function(v) {
            switch (v) {
                case 0:
                    return '初始化';
                case 1:
                    return '开始运行';
                case 2:
                    return '开始取货';
                case 3:
                    return '取货完成'
                case 4:
                    return '开始放货';
                case 5:
                    return '放货完成';
                case 6:
                    return '完成';
                case 7:
                    return '急停';
                case 8:
                    return '出错完成';
                case 9:
                    return '回原点'
            }
        },
        getStateName:function(v) {
            switch (v) {
                case 0:
                    return '初始化';
                case 1:
                    return '回原点';
                case 2:
                    return '无货待命';
                case 3:
                    return '有货待命';
                case 4:
                    return '无货运行';
                case 5:
                    return '有货运行';
                case 6:
                    return '取货';
                case 7:
                    return '放货';
                case 8:
                    return '报警停机';
                case 9:
                    return '报警复位';
                case 10:
                    return '奇怪的状态';
                case 11:
                    return '未连接';
                case 12:
                    return '手动操作'
            }
        },
        //获取列当前的位置
        getColumnPosition: function () {
            //位置集合
            var columnsSettings = this.getRackLayer().columns;
            if (!columnsSettings) {
                return { x: this.getColumnStartOffsetX, y: 0 };
            }
            //用户设置的列序列
            var columns = new Array();
            if (this.getColumns() != "") {
                columns = this.getColumns().split(',');
            } else {
                for (var i = 0; i < this.getColumnCount(); i++) {
                    columns.push(i);
                }
            }
            if (!columns) {
                return { x: this.getColumnStartOffsetX, y: 0 };
            }

            if (!this.getStatus()) {
                return columnsSettings[0];
            }
            var currentColumn = this.getStatus().UserColumn == undefined ? this.getStatus().Column : this.getStatus().UserColumn;
            if (currentColumn == undefined && currentColumn == null) {
                return columnsSettings[0];
            }

            var index = 0;
            for (var i = 0; i < columns.length; i++) {
                var v = columns[i];
                //if (v == '') {
                //    continue;
                //}
                if (parseInt(v) == parseInt(currentColumn)) {
                    break;
                }
                index++;
            }

            if (index > columnsSettings.length - 1) {
                //console.log('用户设置的列数大于当前堆垛机的列数');
                return columnsSettings[0];
            } else {
                return columnsSettings[index];
            }
        },
        resize: function () {
            var h = this.getColumnDeep() * 2;
            h += this.getLanewayWidth();
            this.setHeight(h);

            if (this.getRackLayer()) {
                this.getRackLayer().getLayer().batchDraw();
            }

            if (this.getMaskLayer()) {
                this.getMaskLayer().setSize(this.getSize());
            }

            if (this.getCraneLayer()) {
                this.getCraneLayer().setHeight(this.getHeight());
                this.getCraneLayer().setWidth(this.getCraneWidth());
                this.getCraneLayer().getLayer().batchDraw();
            }
            
            if (!Kinetic.isDesign && $.qtip) {
                //更新提示内容
                var qtip = $(this.getTip()).qtip();
                var that = this;
                if (qtip && qtip.hasOwnProperty('_id')) {
                    var tipId = qtip._id;
                    var status = this.getStatus();
                    if (status) {
                        $('#' + tipId + ' label[name]').each(function (i, item) {
                            if (status.hasOwnProperty($(item).attr('name'))) {
                                var v = status[$(item).attr('name')];
                                switch ($(item).attr('name')) {
                                    case "ForkHorizontalPosition":
                                        v = that.getForkHorizontalPositionName(v);
                                        break;
                                    case "ForkVerticalPosition":
                                        v = that.getForkVerticalPositionName(v);
                                        break;
                                    case "Event":
                                        v = that.getEventName(v);
                                        break;
                                    case "State":
                                        v = that.getStateName(v);
                                        break;

                                }
                                $(item).text(v);
                            } else {
                                $(item).text('<属性不存在>');
                            }
                        });
                    } else {
                        $('#' + tipId + ' label[name]').each(function (i, item) {
                            $(item).text('-');
                        });
                    }
                }
            }
        },
        //重写序列化方法
        toObject: function () {
            var obj = Kinetic.Node.prototype.toObject.call(this);

            obj.children = [];

            return obj;
        },
        _addListeners: function (text) {
            var that = this;
            var func = function () {
                that._sync();
            };

            var properties = ['leftColumnStartOffsetX', 'leftColumnCount', 'rightColumnStartOffsetX', 'rightColumnCount', 'columnWidth', 'columnDeep', 'lanewayWidth', 'railWeight', 'craneWidth', 'status'];
            for (var i = 0; i < properties.length; i++) {
                this.on(properties[i] + 'Change.kinetic', function () {
                    this.resize();
                });
            }
        },
        _sync: function () {
        }
    };

    Kinetic.Util.extend(Kinetic.JSCrane, Kinetic.Group);

    //列数
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'columnCount', 10);
    //列起始位置偏移
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'columnStartOffsetX', 10);
    //列集合（按列数随便生成的格子对应的列）
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'columns', '0,1,2,3,4,5,6,7,9');
    //左侧隐藏列集合
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'leftHiddenColumns', '0');
    //右侧隐藏列集合
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'rightHiddenColumns', '0');
    //列宽
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'columnWidth', 10);
    //列深
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'columnDeep', 20);
    //巷道宽
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'lanewayWidth', 20);
    //轨道厚度
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'railWeight', 3);
    //机体长度
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'craneWidth', 40);
    //状态
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'status', {
        State: 0,
        Column: 0,
        Level: 0,
        ForkHorizontalPosition: 0,
        ForkVerticalPosition: 0,
        AtStation: true,
        ErrorCode: '0000',
        Event: 0,
        TaskId: '00000000'
    });

    //货
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'forkHorizontalPosition', 1);
    //

    //状态颜色
    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'statusColors',
    {
        //初始化
        "0": '#ccc',
        //回原点
        "1": 'green',
        //无货待命
        "2": 'blue',
        //有货待命
        "3": 'blue',
        //无货运行
        "4": 'green',
        //有货运行
        "5": 'green',
        //取货
        "6": 'green',
        //放货
        "7": 'green',
        //报警停机
        "8": 'red',
        //报警复位
        "9": 'red',
        //
        "10": 'red',
        //未连接
        "11": 'gray',
        //手动操作
        "12": 'yellow'
    });

    Kinetic.Factory.addGetterSetter(Kinetic.JSCrane, 'editor',
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
        "columnStartOffsetX": {
            text: '列起始x',
            getter: 'getColumnStartOffsetX',
            setter: 'setColumnStartOffsetX',
            parser: parseInt
        },
        "columnCount": {
            text: '列数',
            getter: 'getColumnCount',
            setter: 'setColumnCount',
            parser: parseInt
        },
        "columnWidth": {
            text: '列宽',
            getter: 'getColumnWidth',
            setter: 'setColumnWidth',
            parser: parseInt
        },
        "columnDeep": {
            text: '列深',
            getter: 'getColumnDeep',
            setter: 'setColumnDeep',
            parser: parseInt
        },
        "lanewayWidth": {
            text: '巷道宽',
            getter: 'getLanewayWidth',
            setter: 'setLanewayWidth',
            parser: parseInt
        },
        "railWeight": {
            text: '轨道厚度',
            getter: 'getRailWeight',
            setter: 'setRailWeight',
            parser: parseInt
        },
        "craneWidth": {
            text: '机体长度',
            getter: 'getCraneWidth',
            setter: 'setCraneWidth',
            parser: parseInt
        },
        "leftHiddenColumns": {
            text: '左侧隐藏列',
            getter: 'getLeftHiddenColumns',
            setter: 'setLeftHiddenColumns'
        },
        "rightHiddenColumns": {
            text: '右侧隐藏列',
            getter: 'getRightHiddenColumns',
            setter: 'setRightHiddenColumns'
        },
        "columns": {
            text: '列序',
            getter: 'getColumns',
            setter: 'setColumns'
        }
    });

    Kinetic.Collection.mapMethods(Kinetic.JSCrane);
})();
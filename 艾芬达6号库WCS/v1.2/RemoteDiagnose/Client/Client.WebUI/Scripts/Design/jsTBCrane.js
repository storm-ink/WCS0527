//泰邦堆垛机
(function () {
    Array.prototype.contains=function(item){
        for (var i = 0; i < this.length; i++) {
            if(this[i]==item){
                return true;
            }
        }

        return false;
    };

    Array.prototype.first = function () {
        return this[0];
    };

    Array.prototype.last = function () {
        return this[this.length - 1];
    };

    //列与坐标对应关系
    var columnsSettings = [];

    function drawRack(crane,obj, context) {
        var columnDeep = crane.getColumnDeep();
        var columnWidth = crane.getColumnWidth();
        var lanewayWidth = crane.getLanewayWidth();
        context.beginPath();
        var lines = [
            {
                lin: 1,
                minColumn: 0,
                maxColumn: 22,
                leftHiddenColumns: [0],
                rightHiddenColumns: [0],
                leftDisabledColumns: [11],
                rightDisabledColumns: [0]
            },
            {
                lin: 2,
                minColumn: 23,
                maxColumn: 45,
                leftHiddenColumns: [23],
                rightHiddenColumns: [23],
                leftDisabledColumns: [],
                rightDisabledColumns: [23]
            },
            {
                lin: 3,
                minColumn: 46,
                maxColumn: 68,
                leftHiddenColumns: [46],
                rightHiddenColumns: [46],
                leftDisabledColumns: [],
                rightDisabledColumns: [46]
            },
            {
                lin: 4,
                minColumn: 69,
                maxColumn: 91,
                leftHiddenColumns: [69],
                rightHiddenColumns: [69],
                leftDisabledColumns: [],
                rightDisabledColumns: [69]
            },
            {
                lin: 5,
                minColumn: 108,
                maxColumn: 130,
                leftHiddenColumns: [108],
                rightHiddenColumns: [108],
                leftDisabledColumns: [],
                rightDisabledColumns: [108]
            }
        ];
        
        columnsSettings = [];


        //最右（远）的X坐标
        var x_farset = 0;
        for (var j = 0; j < lines.length; j++) {
            var lin = lines[j].lin;
            var minColumn = lines[j].minColumn;
            var maxColumn = lines[j].maxColumn;
            for (var i = minColumn; i <= maxColumn; i++) {
                //左列
                var startX = columnWidth * (i - minColumn);
                var y = columnDeep + (columnDeep * 2 + lanewayWidth) * (lin - 1);

                columnsSettings.push(
                {
                    x: startX,
                    y: y,
                    column: i-minColumn,
                    laneway: lin
                });

                if (!lines[j].leftHiddenColumns.contains(i)) {
                    context.moveTo(startX, y);
                    context.lineTo(startX, y - columnDeep);
                    context.lineTo(startX + columnWidth, y - columnDeep);
                    context.lineTo(startX + columnWidth, y);
                }

                if (lines[j].leftDisabledColumns.contains(i)) {
                    context.moveTo(startX, y);
                    context.lineTo(startX, y - columnDeep);
                    context.lineTo(startX + columnWidth, y - columnDeep);
                    context.lineTo(startX + columnWidth, y);
                    context.lineTo(startX, y - columnDeep);
                    context.moveTo(startX + columnWidth, y - columnDeep);
                    context.lineTo(startX, y);
                }

                //右列
                y = lanewayWidth + columnDeep + (columnDeep * 2 + lanewayWidth) * (lin - 1);
                if (!lines[j].rightHiddenColumns.contains(i)) {
                    context.moveTo(startX, y);
                    context.lineTo(startX, y + columnDeep);
                    context.lineTo(startX + columnWidth, y + columnDeep);
                    context.lineTo(startX + columnWidth, y);
                }

                if (lines[j].rightDisabledColumns.contains(i)) {
                    context.moveTo(startX, y);
                    context.lineTo(startX, y + columnDeep);
                    context.lineTo(startX + columnWidth, y + columnDeep);
                    context.lineTo(startX + columnWidth, y);
                    context.lineTo(startX, y + columnDeep);
                    context.moveTo(startX + columnWidth, y + columnDeep);
                    context.lineTo(startX, y);
                }

                x_farset = startX + columnWidth;
            }
        }

        var areas = [
            {
                lin: 1,
                minColumn: 92,
                maxColumn: 96,
                hiddenColumns: [92],
                skipColumnCount:0
            },
            {
                lin: 2,
                minColumn: 97,
                maxColumn: 101,
                hiddenColumns: [97],
                skipColumnCount: 0
            },
            {
                lin: 3,
                minColumn: 102,
                maxColumn: 105,
                hiddenColumns: [102],
                skipColumnCount: 0
            },
            {
                lin: 4,
                minColumn: 106,
                maxColumn: 107,
                hiddenColumns: [106, 107],
                skipColumnCount: 0
            }
        ];

        //侧排
        var lastY = 0;
        for (var j = 0; j < areas.length; j++) {

            var minColumn = areas[j].minColumn;
            var maxColumn = areas[j].maxColumn;
            var hiddenColumns = areas[j].hiddenColumns;
            var skipColumnCount = areas[j].skipColumnCount;
            var startY = lastY+columnWidth * skipColumnCount;
            for (var i = minColumn; i <= maxColumn; i++) {
                //左列
                var startX = x_farset + lanewayWidth;
                var y = startY + columnWidth * (i - minColumn);


                columnsSettings.push(
                {
                    x: startX,
                    y: y,
                    column: i - minColumn,
                    laneway: 6//表示横排巷道
                });


                if (hiddenColumns.contains(i)) {
                    continue;
                }

                context.moveTo(startX, y);
                context.lineTo(startX + columnDeep, y);
                context.lineTo(startX + columnDeep, y + columnWidth);
                context.lineTo(startX, y + columnWidth);

                lastY = y + columnWidth;
            }
        }

        context.strokeShape(obj);
        context.closePath();
    }


    function drawRailway(crane, obj, context) {
        var columnDeep = crane.getColumnDeep();
        var columnWidth = crane.getColumnWidth();
        var lanewayWidth = crane.getLanewayWidth();
        var railWeight = crane.getRailWeight();

        context.beginPath();

        //最右（远）的X坐标
        var railWays = [];
        for (var j = 1; j <= 5; j++) {
            var lin = j;

            var y = columnDeep + (columnDeep * 2 + lanewayWidth) * (lin - 1) + (lanewayWidth - railWeight) / 2;
            var x = 0;

            context.rect(0, y, columnWidth*23, railWeight);
            railWays.push({ x: x + columnWidth * 23, y: y });
        }

        //横排
        var x = railWays.first().x + (lanewayWidth - railWeight) / 2
        var y = railWays.first().y + columnWidth;
        var height = railWays.last().y - railWays.first().y - columnWidth * 2;
        context.rect(x, y, railWeight, height);
        context.fillStrokeShape(obj);

        //弯轨处
        
        var oldstrokeWidth = obj.getStrokeWidth();
        var strokeWidth = obj.getStrokeWidth();
        railWeight = railWeight + strokeWidth;
        obj.setStrokeWidth(railWeight);
        for (var i = 0; i < railWays.length; i++) {

            context.beginPath();
            if (i == railWays.length - 1) {

                var height = railWays.last().y - railWays.first().y - columnWidth * 2;

                var x = railWays[i].x + lanewayWidth / 2;
                var y = railWays.first().y + columnWidth + height;
                context.moveTo(x, y);
                context.bezierCurveTo(
                                      x, y,
                                      railWays[i].x + lanewayWidth / 1.7, railWays[i].y,
                                      railWays[i].x, railWays[i].y + railWeight/2
                                      );

            } else {
                var x = railWays[i].x;
                var y = railWays[i].y + railWeight / 2;
                context.moveTo(x, y);
                context.bezierCurveTo(
                                      x, y,
                                      x + lanewayWidth / 1.7, y,
                                      x + lanewayWidth / 2, y + columnWidth
                                      );
            }
            context.strokeShape(obj);
        }
        obj.setStrokeWidth(oldstrokeWidth);
    }

    if (!Kinetic.JSCraneTypes) {
        Kinetic.JSCraneTypes = [];
    }

    Kinetic.JSCraneTypes.push('jsTBCrane');

    Kinetic.jsTBCrane = function (config) {
        this.____init(config);
    };

    Kinetic.jsTBCrane.prototype = {
        ____init: function (config) {

            var that = this;
            this.className = 'jsTBCrane';
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
                'fill': '#ccc',
                'stroke': '#ccc',
                'strokeWidth': 1,
                'width': this.getWidth(),
                'height': this.getHeight(),
                'opacity': 1,
                'name': 'rack',
                sceneFunc: function (context) {
                    
                    drawRack(that,this, context);

                    drawRailway(that, this, context);
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
                    
                    var columnDeep = that.getColumnDeep();
                    var forkHeight = that.getForkPadding();//货叉突出2px
                    var craneHeight = that.getLanewayWidth() - 2 * 2 - forkHeight * 2;//机体厚度等于巷道宽去掉两侧2px的空隙
                    var forkWidth = that.getColumnWidth(); //货叉宽与货加列宽一致
                    
                    //正常巷道画法
                    if (forkPosition.laneway != 6) {

                        var startX = forkPosition.x - (this.getWidth() - forkWidth) / 2;
                        var startY = forkPosition.y + forkHeight + 2;

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
                    } else {

                        //横向巷道画法

                        var startX = forkPosition.x - forkHeight - 2;
                        var startY = forkPosition.y - (this.getWidth() - forkWidth) / 2;

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
                                    forkScale = 1;
                                    break;
                                default:
                                    forkScale = 0;
                            }
                        }

                        context.beginPath();

                        context.moveTo(startX, startY);
                        context.lineTo(startX, startY + (this.getWidth() - forkWidth) / 2);

                        if (forkHorizontalPosition == 4 || forkHorizontalPosition == 5 || forkHorizontalPosition == 8 || forkHorizontalPosition == 9) {
                            context.lineTo(startX + forkHeight + columnDeep * forkScale, startY + (this.getWidth() - forkWidth) / 2);
                            context.lineTo(startX + forkHeight + columnDeep * forkScale, startY + (this.getWidth() - forkWidth) / 2 + forkWidth);
                        } else {
                            context.lineTo(startX + forkHeight, startY + (this.getWidth() - forkWidth) / 2);
                            context.lineTo(startX + forkHeight, startY + (this.getWidth() - forkWidth) / 2 + forkWidth);
                        } 

                        context.lineTo(startX, startY + (this.getWidth() - forkWidth) / 2 + forkWidth);
                        context.lineTo(startX, startY + ((this.getWidth() - forkWidth) / 2) * 2 + forkWidth);


                        context.lineTo(startX - craneHeight, startY + ((this.getWidth() - forkWidth) / 2) * 2 + forkWidth);
                        context.lineTo(startX - craneHeight, startY + ((this.getWidth() - forkWidth) / 2) + forkWidth);

                        context.lineTo(startX - craneHeight - forkHeight, startY + ((this.getWidth() - forkWidth) / 2) + forkWidth);
                        context.lineTo(startX - craneHeight - forkHeight, startY + ((this.getWidth() - forkWidth) / 2));


                        context.lineTo(startX - craneHeight, startY + ((this.getWidth() - forkWidth) / 2));
                        context.lineTo(startX - craneHeight, startY);

                        context.closePath();



                    }

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

            this.add(craneDevice);
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
        //获取列当前的位置
        getColumnPosition: function () {
            //用户设置的列序列
            var columns = columnsSettings;

            if (!this.getStatus()) {
                return columns[0];
            }
            var currentColumn = this.getStatus().UserColumn;
            if (currentColumn == undefined && currentColumn == null) {
                return columns[0];
            }

            var laneway = this.getStatus().Laneway;
            if (laneway == undefined && laneway == null) {
                return columns[0];
            }

            var index = 0;
            for (var i = 0; i < columns.length; i++) {
                var c = columns[i];
                if (!c) {
                    continue;
                }
                if (c.laneway==laneway && c.column == currentColumn) {
                    break;
                }

                index=i;
            }

            if (index > columns.length - 1) {
                console.log('用户设置的列数大于当前堆垛机的列数');
                return columns[0];
            } else {
                return columns[index];
            }
        },
        resize: function () {
            var h = (this.getColumnDeep() * 2 + this.getLanewayWidth()) * 5;
            var w = this.getColumnWidth()*23+this.getColumnDeep() + this.getLanewayWidth()
            this.setHeight(h);
            this.setWidth(w);

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

            var properties = ['columnWidth', 'columnDeep', 'lanewayWidth', 'railWeight', 'craneWidth', 'status','forkPadding'];
            for (var i = 0; i < properties.length; i++) {
                this.on(properties[i] + 'Change.kinetic', function () {
                    this.resize();
                });
            }
        },
        _sync: function () {
        }
    };

    Kinetic.Util.extend(Kinetic.jsTBCrane, Kinetic.Group);

    //列数
    Kinetic.Factory.addGetterSetter(Kinetic.jsTBCrane, 'columnCount', 10);
    //列宽
    Kinetic.Factory.addGetterSetter(Kinetic.jsTBCrane, 'columnWidth', 30);
    //列深
    Kinetic.Factory.addGetterSetter(Kinetic.jsTBCrane, 'columnDeep', 20);
    //巷道宽
    Kinetic.Factory.addGetterSetter(Kinetic.jsTBCrane, 'lanewayWidth', 45);
    //轨道厚度
    Kinetic.Factory.addGetterSetter(Kinetic.jsTBCrane, 'railWeight', 3);
    //机体长度
    Kinetic.Factory.addGetterSetter(Kinetic.jsTBCrane, 'craneWidth', 60);
    //货叉边距
    Kinetic.Factory.addGetterSetter(Kinetic.jsTBCrane, 'forkPadding', 5);
    //状态
    Kinetic.Factory.addGetterSetter(Kinetic.jsTBCrane, 'status', {
        State: 0,
        Column: 3,
        Level: 1,
        UserColumn: 3,
        UserLevel:1,
        Laneway: 1,
        ForkHorizontalPosition: 0,
        ForkVerticalPosition: 0,
        AtStation: true,
        ErrorCode: '0000',
        Event: 0,
        TaskId: '00000000'
    });

    //货
    Kinetic.Factory.addGetterSetter(Kinetic.jsTBCrane, 'forkHorizontalPosition', 1);
    //

    //状态颜色
    Kinetic.Factory.addGetterSetter(Kinetic.jsTBCrane, 'statusColors',
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

    Kinetic.Factory.addGetterSetter(Kinetic.jsTBCrane, 'editor',
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
        "forkPadding": {
            text: '货叉边距',
            getter: 'getForkPadding',
            setter: 'setForkPadding',
            parser: parseInt
        },
    });

    Kinetic.Collection.mapMethods(Kinetic.jsTBCrane);
})();
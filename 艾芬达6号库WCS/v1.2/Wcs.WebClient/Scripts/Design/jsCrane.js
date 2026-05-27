(function () {
    Kinetic.JSCraneTypes = [];

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
            //位置集合
            var columnsSettings = this.getRackLayer().columns;
            if (!columnsSettings) {
                return { x: this.getColumnStartOffsetX, y: 0 };
            }
            //用户设置的列序列
            var columns = this.getColumns().split(',');
            if (!columns) {
                return { x: this.getColumnStartOffsetX, y: 0 };
            }

            if (!this.getStatus()) {
                return columnsSettings[0];
            }
            var currentColumn = this.getStatus().UserColumn;
            if (currentColumn == undefined && currentColumn == null) {
                return columnsSettings[0];
            }

            var index = 0;
            for (var i = 0; i < columns.length; i++) {
                var v = columns[i];
                if (v == '') {
                    continue;
                }
                if (parseInt(v) == currentColumn) {
                    break;
                }
                index++;
            }

            if (index > columnsSettings.length - 1) {
                console.log('用户设置的列数大于当前堆垛机的列数');
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
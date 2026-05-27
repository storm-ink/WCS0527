var utils = {
    colors: {
        "手动": "#ccc",
        "自动": "rgb(255, 255, 255)",
        "有任务": "rgb(204, 255, 204)",
        "报警": "rgb(226, 200, 6)",
        "离线": "red",
        "运行中": "#00FF99",
        "路径起点": { color: "blue", alpha: 0.5 },
        "路径终点": { color: "#FF00FF", alpha: 0.5 }
    },
    //所有输送线对象
    conveyors: new Array(),
    //用于存放一些临时的变量，非安全对象
    cache: {
        currentTask: null
    },
    startWith: function (str, findStr) {
        var reg = new RegExp("^" + findStr);
        return reg.test(str);
    },
    endWith: function (str, findStr) {
        var reg = new RegExp(findStr + "$");
        return reg.test(str);
    },
    join: function (array, separator, displayCondition) {
        if (array) {
            var r = "";
            for (var i = 0; i < array.length; i++) {
                r += (r != "" ? separator : "") + displayCondition(array[i]);
            }

            return r;
        }
        return "";
    },
    firstOrDefault: function (array, matchCondition) {
        if (!array) return null;
        for (var i = 0; i < array.length; i++) {
            if (matchCondition(array[i])) {
                return array[i];
            }
        }
        return null;
    },
    any: function (array, matchCondition) {
        if (!array) return false;
        for (var i = 0; i < array.length; i++) {
            if (matchCondition(array[i])) {
                return true;
            }
        }
        return false;
    },
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
    getExceptionMessage: function (data) {
        if (data.Data) {
            if (data.Data.Message) {
                return data.Data.Message;
            } else {
                return data.Data;
            }
        } else {
            return data;
        }
    },
    /*
    enable 方法
    disable 方法
    */
    moduleManager: {
        modules: new Array(),
        add: function (obj) {
            this.modules.push(obj);
        },
        enable: function (obj) {
            if (typeof (obj) == 'string') {
                obj = utils.firstOrDefault(this.modules, function (item) { return item.getHashCode() == obj; });
            }
            if (obj) {
                obj.enable();
                USERDATA.set(obj.getHashCode(), 'true');
            }
        },
        disable: function (obj) {
            if (typeof (obj) == 'string') {
                obj = utils.firstOrDefault(this.modules, function (item) { return item.getHashCode() == obj; });
            }
            if (obj) {
                obj.disable();
                USERDATA.set(obj.getHashCode(), 'false');
            }
        },
        init: function () {
            for (var i = 0; i < this.modules.length; i++) {
                var v = USERDATA.get(this.modules[i].getHashCode());
                if (v == '' || v == null || v == undefined || v == 'true') {
                    this.modules[i].init();
                } else {
                    this.modules[i].disable();
                }
            }
        }
    }
};
var emunConvertor = {
    //任务状态
    toTaskStatus:function(v){
        switch(v)
        {
            case 0:
            return "新任务";
            case 1:
            return "已发送";
            case 2:
            return "执行中";
            case 3:
            return "已暂停";
            case 4:
            return "发生错误";
            case 5:
            return "已取消";
            case 6:
            return "已完成";
            default:
            return v;
        }
    },

    //逻辑动作状态
    toLogicMovementStatus: function (v) {
        switch (v) {
            case 0:
                return "新任务";
            case 1:
                return "执行中";
            case 2:
                return "已暂停";
            case 3:
                return "发生错误";
            case 4:
                return "已取消";
            case 5:
                return "已完成";
            default:
                return v;
        }
    },
    toBizType:function(v){
        switch(v){
            case 0:
            return "正常业务";
            case 1:
            return "盘点业务";
            default:
            return v;
        }
    },
    //货位状态
    toLocationStatus:function(v){
        switch (v) {
            case 0:
                return "初始化";
            case 1:
                return "报警";
            case 2:
                return "离线";
            case 3:
                return "手动";
            case 4:
                return "停止";
            case 5:
                return "运行中";
            default:
                return v;
        }
    }
}
var conveyor = function (obj, id, opt) {
    var result = {};
    result._obj = obj;
    result.firstOrDefault = function (array, matchCondition) {
        if (!array) return null;
        for (var i = 0; i < array.length; i++) {
            if (matchCondition(array[i])) {
                return array[i];
            }
        }
        return null;
    };
    result.any = function (array, matchCondition) {
        if (!array) return false;
        for (var i = 0; i < array.length; i++) {
            if (matchCondition(array[i])) {
                return true;
            }
        }
        return false;
    };
    result.join = function (array, separator, displayCondition) {
        return utils.join(array, separator, displayCondition);
    };
    result.occupy = {
        "PosNo": id,
        "PhocllUseStatus": 0,
        "FroProPotocell": false,
        "FroPosPotocell": false,
        "FroSloPotocell": false,
        "AftProPotocell": false,
        "AftPosPotocell": false,
        "AftSloPotocell": false,
        "UpPotocell": false,
        "DownPotocell": false,
        getLightOccupy: function () {
            var r = new Array();
            if (this.FroProPotocell) {
                r.push("保护光电");
            }
            if (this.FroPosPotocell) {
                r.push("前到位");
            }
            if (this.FroSloPotocell) {
                r.push("前减速");
            }
            if (this.AftProPotocell) {
                r.push("后保护");
            }
            if (this.AftPosPotocell) {
                r.push("后到位");
            }
            if (this.AftSloPotocell) {
                r.push("后减速");
            }
            if (this.UpPotocell) {
                r.push("后高位");
            }
            if (this.DownPotocell) {
                r.push("后低位");
            }
            return r;
        },
        toDescription: function () {
            var r = "";
            if (this.FroProPotocell) {
                r += " 保护光电";
            }
            if (this.FroPosPotocell) {
                r += " 前到位";
            }
            if (this.FroSloPotocell) {
                r += " 前减速";
            }
            if (this.AftProPotocell) {
                r += " 后保护";
            }
            if (this.AftPosPotocell) {
                r += " 后到位";
            }
            if (this.AftSloPotocell) {
                r += " 后减速";
            }
            if (this.UpPotocell) {
                r += " 后高位";
            }
            if (this.DownPotocell) {
                r += " 后低位";
            }

            return r;
        }
    };

    //货位号
    result.locationId = id;
    //设备任务号
    result.equipmentTaskId = 0;
    //任务
    result.task = null;
    result.getEquipmentAction = function () {
        if (this.equipmentTaskId != 0 && this.task && this.task.Movements) {
            for (var i = 0; i < this.task.Movements.length; i++) {
                if (this.task.Movements[i].Actions) {
                    for (var j = 0; j < this.task.Movements[i].Actions.length; j++) {
                        if (this.task.Movements[i].Actions[j].EquipmentTaskId == this.equipmentTaskId) {
                            return this.task.Movements[i].Actions[j];
                        }
                    }
                }
            }
        }

        return null;
    };

    result.getMovement = function () {
        if (this.equipmentTaskId != 0 && this.task && this.task.Movements) {
            var r = this.firstOrDefault(this.task.Movements, function (movement) {
                return movement.Actions && result.any(movement.Actions, function (action) {
                    return action.EquipmentTaskId == result.equipmentTaskId;
                });
            });

            return r;
        }

        return null;
    };
    //取方向
    result.direction = function () {
        return ($(result._obj).attr('direction') == null ? 'left' : $(result._obj).attr('direction')).toLowerCase();
    };
    result.getDirectionChar = function () {
        switch (result.direction()) {
            case 'left':
                return '←';
                break;
            case 'right':
                return '→';
                break;
            case 'up':
                return '↑';
                break;
            case 'down':
                return '↓';
                break;
            case 'left,right':
            case 'right,left':
                return '→←'
                break;
            case 'up,down':
            case 'down,up':
                return '↑↓'
                break;
            case 'up,down,left,right':
                return "*";
                break;
            default:
                alert('方向 ' + result.direction() + ' 未定义');
                return null;
        }
    };

    //取状态
    result.status = 0;
    result.getStatusDescription = function () {
        return emunConvertor.toLocationStatus(this.status);
    }


    result.fillStatusStyles = function () {
        var bgColor = "";
        switch (this.status) {
            case 0:
                bgColor = utils.colors["自动"];
                break;
            case 1:
                //return "报警";
                bgColor = utils.colors["报警"];
                break;
            case 2:
                //return "离线";
                bgColor = utils.colors["离线"];
                break;
            case 3:
                //return "手动";
                bgColor = utils.colors["手动"];
                break;
            case 4:
                //return "停止";
                bgColor = utils.colors["自动"];
                break;
            case 5:
                //return "运行中";
                bgColor = utils.colors["运行中"];
                break;
            default:
                bgColor = utils.colors["自动"];
                break;
        }

        $(this.conv).css('background-color', bgColor);
    };
    //画托盘
    result.drawBox = function () {
        if (this.box) {
            return;
        }
        /*z-index: 99;
        position: absolute;
        margin: 3px 0px 0px 5px;
        opacity: 0.8;*/
        this.box = $('<span class="box"></span>');
        $(this.box).width($(this.conv).width() - 8)
				  .height($(this.conv).height() - 8)
				  .css('display', 'block')
				  .css('background-color', '#CCFFCC')
				  .css('margin-top', ($(this.conv).height() - $(this.box).height()) / 2 + 'px')
				  .css('margin-left', ($(this.conv).width() - $(this.box).width()) / 2 + 'px')
				  .css('text-align', 'center')
				  .css('vertical-align', 'vertical-align:middle')
				  .css('line-height', '100%')
                  .css('position', 'absolute')
                  .css('opacity', '0.8')
                  .css('z-index', 1);
        $(this.conv).before(this.box);
        $(this.box).click(function () {
            $(result.conv).click();
        });
    };
    result.removeBox = function () {
        if (this.box) {
            $(this.box).remove();
            this.box = null;
        }
    };


    //取提示信息
    result.getTipText = function () {
        var tiptext = ''; //'货位号:' + result.locationId;

        if (this.equipmentTaskId != 0) {
            tiptext += '<fieldset><legend>任务信息</legend>';
            if (this.task) {
                tiptext += '<font color="green" style="cursor:pointer; text-decoration:underline;" title="单击查看任务 ' + this.task.TaskId + ' 的详细信息" onclick="showTaskDialog($(this).text())">' + this.task.TaskId + '</font> 从 <font color="green">' + this.task.StartLocation.UserCode + '</font> 将 <font color="green">' + this.task.ContainerCodes.join(',') + '</font> 输送到 <font color="green">' + this.task.EndLocation.UserCode + '</font>';
            }
            var action = this.getEquipmentAction();
            var movement = this.getMovement();
            tiptext += '<br />设备任务 ' + result.equipmentTaskId;
            if (movement) {
                if (movement.Routes[0].DeviceCode == "*") {
                    movement.Routes[0].DeviceCode = this.task.StartLocation.DeviceCode;
                    movement.Routes[0].UserCode = this.task.StartLocation.UserCode;
                }
                tiptext += ' 由路径 <font color="green">' + movement.RouteId + '</font> 驱动，全程经过 <font color="green" style="text-decoration:underline;cursor:pointer;" onmouseover="createRouteMask($(this).text())" onmouseout="removeRouteMask()">' + this.join(movement.Routes, ',', function (route) {
                    return route.DeviceCode;
                }) + '</font>';
            }
            tiptext += '</fieldset>';
        }


        tiptext += '<fieldset><legend>货位状态</legend>';
        tiptext += '<b>状态</b> <font color="red">' + this.getStatusDescription() + '</font>';
        var occpyStatus = this.occupy.toDescription();
        if (occpyStatus != "") {
            tiptext += '<br /><b>光电</b> <font color="red">' + occpyStatus + '</font>';
        }
        tiptext += '</fieldset>';
        return tiptext;
    };

    //更新状态
    result.updateCurrentTask = function (status) {
        var newStatus = this.firstOrDefault(status, function (item) {
            return item.PosNo == result.locationId;
        });

        if (newStatus == null) {
            this.equipmentTaskId = 0;
        } else {
            this.equipmentTaskId = newStatus.TaskNo;
        }

        this.update();
    };
    result.updateCurrentStatus = function (status) {
        var newStatus = this.firstOrDefault(status, function (item) {
            return item.PosNo == result.locationId;
        });

        if (newStatus == null) {
            this.status = 0;
        } else {
            this.status = newStatus.Status;
        }

        this.update();
    };
    result.updateOccupyStauts = function (status) {
        var newStatus = this.firstOrDefault(status, function (item) {
            return item.PosNo == result.locationId;
        });

        if (newStatus == null) {
            for (var prop in this.occupy) {
                if (typeof (this.occupy[prop]) == 'Boolean') {
                    this.occupy[prop] = false;
                }
            }
        } else {
            for (var prop in this.occupy) {
                if (newStatus[prop]) {
                    this.occupy[prop] = newStatus[prop];
                }
            }
        }

        this.update();
    };
    result.updateTask = function (tasks) {
        var newStatus = null;
        if (this.equipmentTaskId != 0) {
            newStatus = this.firstOrDefault(tasks, function (item) {
                return item.Movements && result.any(item.Movements, function (movement) {
                    return movement.Actions && result.any(movement.Actions, function (action) {
                        return action.EquipmentTaskId == result.equipmentTaskId;
                    });
                });
            });
        } else {
            newStatus = null;
        }

        this.task = newStatus;

        this.update();
    };
    //初始化配置
    result.opt = $.fn.extend(
		{
		    width: $(obj).width(),
		    height: $(obj).height(),
		    borderSize: 1,
		    borderColor: '#000',
		    borderStyle: 'solid',
		    bgColor: '#fff',
		    showId: false
		}, opt);

    result.fillOccupy = function () {
        var ctx = $(this.conv)[0].getContext('2d');
        ctx.clearRect(0, 0, $(this.conv).width(), $(this.conv).height());
        var occupies = this.occupy.getLightOccupy();
        if (this.occupy != null && occupies.length > 1) {
            ctx.fillStyle = "green";
            var j = 0;
            for (var i = 0; i < occupies.length; i++) {
                if (occupies[i].indexOf("高位") >= 0 || occupies[i].indexOf("低位") >= 0) {
                    continue;
                }
                ctx.fillRect(0, 0 + j * 7, 5, 5);
                j++;
            }
        }
    };
    result.update = function () {
        //$(this.conv).poshytip('update', this.getTipText());
        if (this.equipmentTaskId != 0) {
            this.drawBox();
        } else {
            this.removeBox();
        }

        this.fillOccupy();

        this.showDirection();

        this.fillStatusStyles();


        if (!this.opt.showId) {
            this.hiddenLocationId();
        } else {
            this.showLocationId();
        }

        $('#qtip-' + this.locationId + '-content').html(this.getTipText());
    };

    result.opt.width = result.opt.width - result.opt.borderSize * 2;
    result.opt.height = result.opt.height - result.opt.borderSize * 2;

    //创建主体
    result.conv = $('<canvas class="conveyor"></canvas>');
    $(result.conv).width(result.opt.width)
		.height(result.opt.height)
        .attr('width', result.opt.width)
        .attr('height', result.opt.height)
		.css('background-color', result.opt.bgColor)
		.css('border', result.opt.borderSize + 'px ' + result.opt.borderStyle + ' ' + result.opt.borderColor)
		.attr('locationId', id)
		.css('text-align', 'center')
		.css('vertical-align', 'vertical-align:middle')
		.css('line-height', '100%');
    //.text(result.getDirectionChar());

    result.showDirection = function () {
        var canvasContext = $(result.conv)[0].getContext('2d');
        canvasContext.fillStyle = "#000000";
        canvasContext.font = "15px Arial";
        canvasContext.fillText(result.getDirectionChar(), ($(result.conv).width() - 15 * result.getDirectionChar().length) / 2, 10);
    };
    result.showLocationId = function () {
        var cxt = $(this.conv)[0].getContext('2d');
        this.opt.showId = true;
        cxt.fillStyle = "#000000";
        cxt.font = "8px Arial";
        var textWidth = cxt.measureText($(this.conv).attr('locationId')).width;
        cxt.fillText($(this.conv).attr('locationId'), ($(this.conv).width() - textWidth) / 2, 28);
    };

    result.hiddenLocationId = function () {
        var cxt = $(this.conv)[0].getContext('2d');
        this.opt.showId = false;
        cxt.fillStyle = $(this.conv).css('background-color');
        cxt.font = "8px Arial";
        var textWidth = cxt.measureText($(this.conv).attr('locationId')).width;
        cxt.fillText($(this.conv).attr('locationId'), ($(this.conv).width() - textWidth) / 2, 28);
    };
    result.showDirection();
    if (result.opt) {
        result.showLocationId();
    }

    $(obj).text('');
    $(obj).append(result.conv);

    //提示信息
    //    $(result.conv).poshytip({
    //        content: result.getTipText(),
    //        showOn: 'hover',
    //        allowTipHover: true,
    //        showTimeout: 0,
    //        alignTo: 'target',
    //        alignX: 'inner-left',
    //        alignY: 'top',
    //        offsetX: 0,
    //        offsetY: 5,
    //        keepInViewport:true
    //    });
    $(result.conv).qtip({
        id: result.locationId.toString(),
        /*position: {
        my: 'bottom left',  // Position my top left...
        at: 'top right', // at the bottom right of...
        //target: $('.selector') // my target
        },*/
        show: {
            event: 'click',
            effect: function () {
                $(this).draggable({
                    handle: '.qtip-title',
                    cursor: 'move'
                });

                $(this).show();
            }
        },
        hide: 'unfocus',
        content: {
            button: true,
            title: '查看 ' + result.locationId.toString() + ' 货位信息',
            text: function (event, api) {
                //$(this).attr('qtip-content')
                return result.getTipText();
            }
        }
    });

    return result;
};
var conveyorArea = function (container, area) {
    var result = {
        _obj: container,
        areaName: area,
        conveyors: new Array(),
        isInit: false,
        timers: new Array(),
        updateLocationOccupyStauts: function () {
            $.getJSON(
				"/home/GetConveyorStatusFromLastReceivedPackage?devicename=" + encodeURIComponent(result.areaName) + "&type=Wcs.Framework.Devices.OccupyStatus, Wcs.Framework",
				function (data) {
				    var jsonData = utils.toJSON(data);
				    if (jsonData == null || !jsonData.Success) {
				        return;
				    }

				    for (var i = 0; i < result.conveyors.length; i++) {
				        result.conveyors[i].updateOccupyStauts(jsonData.Data);
				    };
				}
			);
        },
        updateLocationCurrentTask: function () {
            $.getJSON(
				"/home/GetConveyorCurrentTasks?devicename=" + encodeURIComponent(result.areaName),
				function (data) {
				    var jsonData = utils.toJSON(data);
				    if (jsonData == null || !jsonData.Success) {
				        return;
				    }

				    for (var i = 0; i < result.conveyors.length; i++) {
				        result.conveyors[i].updateCurrentTask(jsonData.Data);
				    };
				}
			);
        },
        updateLocationCurrentStatus: function () {
            $.getJSON(
				    "/home/GetConveyorStatus?devicename=" + encodeURIComponent(result.areaName),
				    function (data) {
				        var jsonData = utils.toJSON(data);
				        if (jsonData == null || !jsonData.Success) {
				            return;
				        }

				        for (var i = 0; i < result.conveyors.length; i++) {
				            result.conveyors[i].updateCurrentStatus(jsonData.Data);
				        };
				    }
			    );
        },
        isEnabled: false,
        disable: function () {
            if (this._obj instanceof Array) {
                for (var i = 0; i < this._obj.length; i++) {
                    $(this._obj[i]).css('visibility', 'hidden');
                }
            } else {
                $(this._obj).css('visibility', 'hidden');
            }

            if (this.timers) {
                while (this.timers.length > 0) {
                    clearInterval(this.timers.shift());
                }
            }
            this.isEnabled = false;
        },
        enable: function () {
            this.disable();

            if (this._obj instanceof Array) {
                for (var i = 0; i < this._obj.length; i++) {
                    $(this._obj[i]).css('visibility', 'visible');
                }
            } else {
                $(this._obj).css('visibility', 'visible');
            }
            if (!this.isInit) {
                this.init();
            } else {
                this.timers.push(setInterval(this.updateLocationOccupyStauts, 2000));
                this.timers.push(setInterval(this.updateLocationCurrentTask, 2000));
                this.timers.push(setInterval(this.updateLocationCurrentStatus, 2000));
            }

            this.isEnabled = true; ;
        },
        init: function () {
            $(this.obj).css('visibility', 'visible');
            if (!this.isInit) {
                if (this._obj instanceof Array) {   
                    for (var j = 0; j < this._obj.length; j++) {
                        $(this._obj[j]).find("td[locationId]").each(function (i, item) {
                            var conv = conveyor($(item), $(item).attr('locationId'));
                            result.conveyors.push(conv);

                            utils.conveyors.push(conv);
                        });
                    }
                } else {
                    $(this._obj).find("td[locationId]").each(function (i, item) {
                        var conv = conveyor($(item), $(item).attr('locationId'));
                        result.conveyors.push(conv);

                        utils.conveyors.push(conv);
                    });
                }

                this.isInit = true;

                this.isEnabled = true;

                this.timers.push(setInterval(this.updateLocationOccupyStauts, 2000));
                this.timers.push(setInterval(this.updateLocationCurrentTask, 2000));
                this.timers.push(setInterval(this.updateLocationCurrentStatus, 2000));
            }
        },
        getHashCode: function () {
            return this.areaName;
        },
        getDisplayName: function () {
            return this.areaName;
        }
    };
    result.areaName = area;


    return result;
};
var conveyorStyleManager = function () {
    this.isEnabled = true;
    this.disable = function () {
        for (var i = 0; i < utils.conveyors.length; i++) {
            utils.conveyors[i].hiddenLocationId();
        }
        this.isEnabled = false;
    };
    this.enable = function () {
        for (var i = 0; i < utils.conveyors.length; i++) {
            utils.conveyors[i].showLocationId();
        }
        this.isEnabled = true;
    };
    this.init = function () {
        this.isEnabled = true;
    };
    this.getHashCode = function () {
        return 'conveyorStyleManager';
    };
    this.getDisplayName = function () {
        return '显示货位号';
    };
};
var devicesStatusManager = function (container) {
    this._obj = container;
    this.width = 40;
    this.height = 40;
    this.drawState = function (ele, deviceName, ctx, isConnected, warnings) {
        if (isConnected && (!warnings || warnings.length == 0)) {
            ctx.fillStyle = "green";
            $(ele).attr('title', '联机');
        } else if (!isConnected) {
            ctx.fillStyle = utils.colors["离线"];
            $(ele).attr('title', '离线');
        } else if (isConnected && warnings && warnings.length > 0) {
            ctx.fillStyle = utils.colors["报警"];
            $(ele).attr('title', '报警\n\n'+utils.join(warnings,'\n',function(x){return x;}));
        } else {
            ctx.fillStyle = utils.colors["离线"];
            $(ele).attr('title', '离线');
        }
        ctx.arc(this.width / 2, 10, 10, 0, 2 * Math.PI);
        ctx.fill();
        ctx.fillStyle = "#000";
        ctx.fillText(deviceName, (this.width - ctx.measureText(deviceName).width) / 2, 10 + 10 * 2 + 3);
    };
    this.update = function (states) {
        var elements = $(this._obj).find('canvas[deviceName]');
        for (var i = 0; i < elements.length; i++) {
            var item = $(elements[i]);
            $(item).attr('width', this.width + 'px')
                    .attr('height', this.height + 'px');
            if (states) {
                var state = null;
                for (var j = 0; j < states.length; j++) {
                    state = utils.firstOrDefault(states[j].Devices, function (x) { return x.Name == $(item).attr('deviceName'); });
                    if (state) {
                        break;
                    }
                }
                if (state) {
                    this.drawState($(item),$(item).attr('deviceName'), $(item)[0].getContext("2d"), state.IsConnected, state.Warnings);
                } else {
                    this.drawState($(item),$(item).attr('deviceName'), $(item)[0].getContext("2d"), false, null);
                }
            } else {
                this.drawState($(item),$(item).attr('deviceName'), $(item)[0].getContext("2d"), false, null);
            }
        }
    };

    this.events = new Array();
    this.complete = function (func) {
        if (typeof (func) != 'function') {
            alert('func 必须为函数对象');
            return;
        }
        this.events.push(func);
    }
    var parentObj = this;
    this.updateStatus = function () {
        $.getJSON(
				"/home/GetDevicesConnectionStatus",
				function (data) {
				    var jsonData = utils.toJSON(data);
				    if (jsonData == null || !jsonData.Success) {
				        parentObj.update(null);
				        for (var i = 0; i < parentObj.events.length; i++) {
				            parentObj.events.length[i](null);
				        }
				        return;
				    }

				    parentObj.update(jsonData.Data);
				    for (var i = 0; i < parentObj.events.length; i++) {
				        parentObj.events.length[i](jsonData.Data);
				    }
				}
			);
    };
    this.timer = null;
    this.isEnabled = true;
    this.disable = function () {
        if (this.timer) {
            clearInterval(this.timer);
            this.timer = null;
        }
        this.isEnabled = false;
    };
    this.enable = function () {
        if (!this.timer) {
            setInterval(this.updateStatus, 3000);
        }
        this.isEnabled = true;
    };
    this.init = function () {

        if (!this.timer) {
            this.timer = setInterval(this.updateStatus, 3000);
        }
        this.isEnabled = true;

        this.update(null);
    };
    this.getHashCode = function () {
        return 'devicesStatusManager';
    };
    this.getDisplayName = function () {
        return '设备状态';
    };
};


function taskManager(container){
    this.updateTimer= null;
    this.obj= $(container);
    this.updateTasks=function () {
        $.getJSON(
				"/home/GetTasks",
				function (data) {
				    var jsonData = utils.toJSON(data);
				    if (jsonData == null || !jsonData.Success) {
				        $('#tasks .datarow').remove();
				        $('#tasks').append('<tr><td class="datarow loading" bgcolor="#fff" colspan="7"><img src="/images/loading.gif" alt="" /></td></tr>');
				        return;
				    }

				    //删除行
				    $('#tasks .datarow').each(function (i, item) {
				        var exists = false;
				        for (var i = 0; i < jsonData.Data.length; i++) {
				            if ($(item).attr('taskNo') == jsonData.Data[i].TaskId) {
				                exists = true;
				                break;
				            }
				        }

				        if (!exists) {
				            $(item).remove();
				        }
				    });

				    //添加行
				    for (var i = 0; i < jsonData.Data.length; i++) {
				        var data = jsonData.Data[i];
				        var row = $('#tasks .datarow[taskNo="' + data.TaskId + '"]');
				        if (row.length == 0) {
				            row = $('<tr> <td bgcolor="#FFFFFF">&nbsp;</td><td bgcolor="#FFFFFF">&nbsp;</td><td bgcolor="#FFFFFF">&nbsp;</td><td bgcolor="#FFFFFF">&nbsp;</td><td bgcolor="#FFFFFF">&nbsp;</td><td bgcolor="#FFFFFF">&nbsp;</td><td bgcolor="#FFFFFF">&nbsp;</td></tr>');
				            $(row).hover(function () {
				                $(this).addClass('highLight');
				            }, function () {
				                $(this).removeClass('highLight');
				            });
				            $('#tasks').append(row);
				        }
				        $(row).addClass('datarow')
                              .attr('taskNo', data.TaskId);
				        $(row).children('td').eq(0).text(i + 1);
				        $(row).children('td').eq(1).html('<font style="cursor:pointer; text-decoration:underline;" title="单击查看任务 ' + data.TaskId + ' 的详细信息" onclick="showTaskDialog($(this).text())">' + data.TaskId + '</font>');
				        $(row).children('td').eq(2).text(data.StartLocation.UserCode);
				        $(row).children('td').eq(3).text(data.EndLocation.UserCode);
				        $(row).children('td').eq(4).text(emunConvertor.toTaskStatus(data.Status));
				        switch (data.Status) {
				            case 0:
				                $(row).children('td').eq(4)
                                .removeClass('taskStateSuspend')
                                .removeClass('taskStateExecute')
                                .addClass('taskStateNormal');
				                break;
				            case 1:
				            case 2:
				                $(row).children('td').eq(4)
                                .removeClass('taskStateNormal')
                                .removeClass('taskStateSuspend')
                                .addClass('taskStateExecute');
				                break
				            case 3:
				            case 4:
				                $(row).children('td').eq(4)
                                .removeClass('taskStateNormal')
                                .removeClass('taskStateExecute')
                                .addClass('taskStateSuspend');
				                break
				            default:
				                $(row).children('td').eq(4)
                                .removeClass('taskStateSuspend')
                                .removeClass('taskStateExecute')
                                .addClass('taskStateNormal');
				                break
				        }
				        $(row).children('td').eq(5).text(data.ContainerCodes.join('.'));
				        $(row).children('td').eq(6).text(data.CurrentLocation.UserCode);
				    }

				    for (var i = 0; i < utils.conveyors.length; i++) {
				        utils.conveyors[i].updateTask(jsonData.Data);
				    };
				}
			);
};
    this.isEnabled = false;
    this.init = function () {
        $(this.obj).css('visibility', 'visible');
        this.updateTimer = setInterval(this.updateTasks, 2000);
        this.isEnabled = true;
    };
    this.disable=function () {
        $(this.obj).css('visibility', 'hidden');
        if (this.updateTimer) {
            clearInterval(this.updateTimer);
        }
    };
    this.enable= function () {
        this.init(this.obj);
    };
    this.getHashCode = function () {
        return 'tasksManager';
    },
    this.getDisplayName = function () {
        return '任务列表';
    };
};

function createRouteMask(path) {
    if (!path || path == '') {
        return;
    }

    var locations = path.split(',');
    for (var i = 0; i < locations.length; i++) {
        var convElement = $('.conveyor[locationid=' + locations[i] + ']');
        if ($(convElement).length == 0) {
            continue;
        }

        var mask = $('<div class="routeMask"></div>').css('display', 'block')
                   .width($(convElement).width() + 'px')
                   .height($(convElement).height() + 'px')
                   .css('z-index', '999')
                   .css('opacity', '0.5')
                   .css('position', 'absolute')
                   .offset($(convElement).offset());

        if (i == 0) {
            $(mask).css('background-color', utils.colors["路径起点"].color)
                   .css('opacity', utils.colors["路径起点"].alpha);
        }
        if (i == locations.length - 1) {
            $(mask).css('background-color', utils.colors["路径终点"].color)
                   .css('opacity', utils.colors["路径终点"].alpha);
        }
        $(document.body).append(mask);

    }
}

function removeRouteMask() {
    $('.routeMask').remove();
}

function showTaskDialog(taskNo) {
    $.ajax({
        type: "GET",
        url: "/home/GetTaskDetails?taskNo=" + taskNo,
        dataType: "json",
        success: function (data) {
            var jsonData = utils.toJSON(data);
            if (jsonData == null || !jsonData.Success) {
                alert('任务 ' + taskNo + ' 信息加载失败.\n\n' + utils.getExceptionMessage(jsonData));
                return;
            }
            utils.cache.currentTask = jsonData.Data;
            $('.dialog').attr('taskNo', jsonData.Data.TaskId);
            $('.dialog .taskNo').text(jsonData.Data.TaskId);
            $('.dialog .startLocation').text(jsonData.Data.StartLocation.UserCode);
            $('.dialog .endLocation').text(jsonData.Data.EndLocation.UserCode);
            $('.dialog .status').text(emunConvertor.toTaskStatus(jsonData.Data.Status));
            $('.dialog .bzType').text(emunConvertor.toBizType(jsonData.Data.BizType));
            $('.dialog .containerCodes').text(jsonData.Data.ContainerCodes.join(','));
            $('.dialog .description').text('从 ' + jsonData.Data.StartLocation.DeviceName + ' ' + jsonData.Data.StartLocation.UserCode + ' 货位运送到 ' + jsonData.Data.EndLocation.DeviceName + ' ' + jsonData.Data.EndLocation.UserCode + ' 货位，当前货位正停靠在 ' + jsonData.Data.CurrentLocation.UserCode + ' 上');

            $('.dialog .movements .datarow').remove();
            for (var i = 0; i < jsonData.Data.Movements.length; i++) {
                var row = $('<tr class="datarow movement"><td bgcolor="#FFFFFF">&nbsp;</td><td bgcolor="#FFFFFF">&nbsp;</td><td bgcolor="#FFFFFF">&nbsp;</td><td bgcolor="#FFFFFF">&nbsp;</td><td bgcolor="#FFFFFF">&nbsp;</td></tr>'); ;
                $(row).children('td').eq(0).text(jsonData.Data.Movements[i].Id);
                $(row).children('td').eq(1).text(jsonData.Data.Movements[i].StartLocation.UserCode);
                $(row).children('td').eq(2).text(jsonData.Data.Movements[i].EndLocation.UserCode);
                $(row).children('td').eq(3).html(jsonData.Data.Movements[i].RouteId + ':<font onmouseover="createRouteMask($(this).text())" onmouseout="removeRouteMask()" style="text-decoration:underline;">' + utils.join(jsonData.Data.Movements[i].Routes, ',', function (item) { return item.DeviceCode == "*" ? jsonData.Data.Movements[i].StartLocation.DeviceCode : item.DeviceCode; }) + "</font>")
                    .css('width', '350px')
                    .css('word-break', 'break-all')
                    .css('word-wrap', 'break-word');
                $(row).children('td').eq(4).text(emunConvertor.toLogicMovementStatus(jsonData.Data.Movements[i].Status));
                $('.dialog .movements').append(row);
                $(row).click(function () {
                    try {
                        var next = $(this).next();
                        while ($(next).hasClass('action')) {
                            $(next).css('display', $(next).css('display') == 'none' ? 'table-row' : 'none');
                            $(next) = $(next).next();
                        }
                    } catch (e) {
                        //
                    }
                });
                for (var j = 0; j < jsonData.Data.Movements[i].Actions.length; j++) {
                    var actionRow = $('<tr class="datarow action"><td bgcolor="#FFFFFF" colspan="5">&nbsp;</td></tr>');
                    $(actionRow).children('td').text('设备任务号 ' + jsonData.Data.Movements[i].Actions[j].EquipmentTaskId + ' 由 ' + jsonData.Data.Movements[i].Actions[j].ReadableDescription);
                    $('.dialog .movements').append(actionRow);
                }
            }

            var buttons = {};
            if (jsonData.Data.Status == 0 || jsonData.Data.Status == 1 || jsonData.Data.Status == 2) {
                buttons["暂停"] = function () {
                    if (confirm("确认要暂停这个任务吗？")) {
                        suspendTask($('.dialog').attr('taskNo'));
                    }
                };
            }
            if (jsonData.Data.Status == 3 || jsonData.Data.Status == 4) {
                buttons["继续执行"] = function () {
                    resumeTask($('.dialog').attr('taskNo'));
                };
            }
            if (jsonData.Data.Status == 3 || jsonData.Data.Status == 4) {
                buttons["强制完成"] = function () {
                    if (!utils.cache || !utils.cache.currentTask || !utils.cache.currentTask.Movements || utils.cache.currentTask.Movements.length == 0) {
                        alert('无逻辑动作,无法强制完成.');
                        return;
                    }

                    var lastMovement = utils.cache.currentTask.Movements[utils.cache.currentTask.Movements.length - 1];
                    var lastAction = lastMovement.Actions[lastMovement.Actions.length - 1];
                    $('#selectCompleteTask table tr').remove();
                    $('#selectCompleteTask table').append($('<tr><td style="background-color:#fff;white-space:nowrap;"><input name="competeTask" id="competeTask_0" type="radio" value="' + utils.cache.currentTask.Id + '@Wcs.Framework.Task, Wcs.Framework"><label for="competeTask_0">主任务 ' + utils.cache.currentTask.TaskId + '</label></td></tr>'));
                    $('#selectCompleteTask table').append($('<tr><td style="background-color:#fff;white-space:nowrap;"><input name="competeTask" id="competeTask_1" type="radio" value="' + lastMovement.Id + '@Wcs.Framework.LogicMovement, Wcs.Framework"><label for="competeTask_1">逻辑动作 从 ' + lastMovement.StartLocation.UserCode + ' 到 ' + lastMovement.EndLocation.UserCode + '</label></td></tr>'));
                    $('#selectCompleteTask table').append($('<tr><td style="background-color:#fff;white-space:nowrap;"><input name="competeTask" id="competeTask_2" type="radio" value="' + lastAction.Id + '@Wcs.Framework.EquipmentAction, Wcs.Framework"><label for="competeTask_2">物理动作 ' + lastAction.ReadableDescription + '</label></td></tr>'));

                    $('#selectCompleteTask').dialog({
                        modal: true,
                        minWidth: 440,
                        title: "请选择要强制完成的任务对象",
                        buttons: {
                            "确定": function () {
                                var checkItem = $('#selectCompleteTask [name="competeTask"]:checked');
                                if ($(checkItem).length == 0) {
                                    alert('请先选择您要完成的任务类型.');
                                    return;
                                }

                                var v = $(checkItem).val();
                                completeTask(v.split('@')[0], v.split('@')[1]);
                                $(this).dialog("close");
                            },
                            "取消": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                };
            }
            if (!(jsonData.Data.Status == 1 || jsonData.Data.Status == 2 || jsonData.Data.Status == 5 || jsonData.Data.Status == 6)) {
                buttons["取消任务"] = function () {
                    if (confirm("确认要取消这个任务吗？")) {
                        cancelTask($('.dialog').attr('taskNo'));
                    }
                };
            }

            buttons["返回"] = function () {
                $(this).dialog("close");
            };

            $('.dialog').dialog({
                title: '您正在查看任务 ' + taskNo,
                minWidth: 780,
                modal: true,
                buttons: buttons
            });
        }
    });
}
function suspendTask(taskNo) {
    $.ajax({
        url: "/home/SuspendTask?taskNo=" + taskNo,
        dataType: "json",
        type: "GET",
        success: function (data) {
            var jsonData = utils.toJSON(data);
            if (!jsonData) {
                alert('任务 ' + taskNo + ' 暂停失败.\n\n' + utils.getExceptionMessage(jsonData));
                return;
            }

            if (jsonData == true || jsonData.Success) {
                $('.dialog').dialog("close");
                alert('任务 ' + taskNo + ' 暂停成功.');
            } else {
                alert('任务 ' + taskNo + ' 暂停失败.\n\n' + utils.getExceptionMessage(jsonData));
            }
        }
    });
}

function completeTask(taskid, taskType) {
    $.ajax({
        url: "/home/CompleteTask?id=" + taskid + "&objectType=" + taskType,
        dataType: "json",
        type: "GET",
        success: function (data) {
            var jsonData = utils.toJSON(data);
            if (!jsonData) {
                alert('强制完成失败.\n\n' + utils.getExceptionMessage(jsonData));
                return;
            }

            if (jsonData == true || jsonData.Success) {
                $('.dialog').dialog("close");
                alert('强制完成成功.');
            } else {
                alert('强制完成失败.\n\n' + utils.getExceptionMessage(jsonData));
            }
        }
    });
}

function resumeTask(taskNo) {
    $.ajax({
        url: "/home/ResumeTask?taskNo=" + taskNo,
        dataType: "json",
        type: "GET",
        success: function (data) {
            var jsonData = utils.toJSON(data);
            if (!jsonData || !jsonData.Success) {
                alert('任务 ' + taskNo + ' 继续执行失败.\n\n' + utils.getExceptionMessage(jsonData));
                return;
            }

            if (jsonData.Success && !jsonData.Data) {
                alert('任务 ' + taskNo + ' 继续执行成功.');
                $('.dialog').dialog("close");
            } else if (jsonData.Success && jsonData.Data) {
                $('#selectTaskCurrentLocation table tr td').remove();
                for (var i = 0; i < jsonData.Data.Paths.length; i++) {
                    var td = $('<td></td>')
								.text(jsonData.Data.Paths[i].DeviceCode == "*" ? jsonData.Data.Paths[i].UserCode : jsonData.Data.Paths[i].DeviceCode)
								.attr('UserCode', jsonData.Data.Paths[i].UserCode)
								.attr('DeviceCode', jsonData.Data.Paths[i].DeviceCode)
								.attr('DeviceName', jsonData.Data.Paths[i].DeviceName)
								.attr('DeviceType', jsonData.Data.Paths[i].DeviceType)
								.addClass('pathNormal')
								.hover(function () {
								    $(this).removeClass('pathNormal')
                                            .addClass('pathSelected');
								}, function () {
								    if ($(this).attr('sel') == 'true') {
								        $(this).removeClass('pathNormal').addClass('pathSelected');
								    } else {
								        $(this).removeClass('pathSelected').addClass('pathNormal');
								    }
								})
								.click(function () {
								    $(this).parent().children('td').each(function (i, obj) {
								        $(obj).attr('sel', 'false')
                                              .removeClass('pathSelected')
                                              .addClass('pathNormal');
								    });
								    $(this).attr('sel', 'true')
                                            .removeClass('pathNormal')
                                           .addClass('pathSelected');
								});
                    //此处可以判断并处理默认选中
                    if (utils.cache.currentTask && utils.cache.currentTask.CurrentLocation) {
                        if (utils.cache.currentTask.CurrentLocation.DeviceCode == jsonData.Data.Paths[i].DeviceCode) 
                        {
                            $(td).removeClass('pathNormal')
                                .addClass('pathSelected');
                        }
                    }

                    $('#selectTaskCurrentLocation table tr').append(td);
                }
                $('#selectTaskCurrentLocation').dialog({
                    modal: true,
                    title: "请选择当前货物停靠位置",
                    buttons: {
                        "确定": function () {
                            var sel = $(this).find('.pathSelected');
                            if ($(sel).length == 0) {
                                alert('请先选择当前货位的停靠点');
                                return;
                            }

                            resumeTaskByCurrentLocation($('.dialog').attr('taskNo'), $(sel).attr('DeviceCode') + '@' + $(sel).attr('DeviceName'));

                            $(this).dialog("close");

                        },
                        "取消": function () {
                            $(this).dialog("close");
                        }
                    }
                });
            } else {
                alert('任务 ' + taskNo + ' 继续执行失败.\n\n' + utils.getExceptionMessage(jsonData));
            }
        }
    });
}

function resumeTaskByCurrentLocation(taskNo, currentLocation) {
    $.ajax({
        url: "/home/ResumeTaskWithCurrentLocation?taskNo=" + taskNo + "&currentLocation=" + encodeURIComponent(currentLocation),
        dataType: "json",
        type: "GET",
        success: function (data) {
            var jsonData = utils.toJSON(data);
            if (!jsonData || !jsonData.Success) {
                alert('任务 ' + taskNo + ' 继续执行失败.\n\n' + utils.getExceptionMessage(jsonData));
                return;
            }

            if (jsonData.Success) {
                alert('任务 ' + taskNo + ' 继续执行成功.');
                $('.dialog').dialog("close");
            } else {
                alert('任务 ' + taskNo + ' 继续执行失败.\n\n' + utils.getExceptionMessage(jsonData));
            }
        }
    });

}

function cancelTask(taskNo) {
    $.ajax({
        url: "/home/CancelTask?taskNo=" + taskNo,
        dataType: "json",
        type: "GET",
        success: function (data) {
            var jsonData = utils.toJSON(data);
            if (!jsonData || !jsonData.Success) {
                alert('任务 ' + taskNo + ' 继续取消失败.\n\n' + utils.getExceptionMessage(jsonData));
                return;
            }

            if (jsonData.Success) {
                alert('任务 ' + taskNo + ' 取消成功.');
                $('.dialog').dialog("close");
            } else {
                alert('任务 ' + taskNo + ' 取消失败.\n\n' + utils.getExceptionMessage(jsonData));
            }
        }
    });
}
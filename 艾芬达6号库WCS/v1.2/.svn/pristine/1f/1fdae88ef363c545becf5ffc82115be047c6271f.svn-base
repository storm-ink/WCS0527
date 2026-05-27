window.stage = new Kinetic.Stage({
    container: 'stage',
    width: 1200,
    height: 600
});


function formatStage(stage) {
    stage.getCurrentLayer = function () {
        return window.stage.currentLayer;
    };

    stage.setCurrentLayer = function (layer) {
        window.stage.currentLayer = layer;
        if (layer) {
            layer.moveToTop();
        }
        showProperties(layer);
    };

    stage.getCurrentElement = function () {
        return window.stage.currentElement;
    };

    stage.setCurrentElement = function (element) {
        window.stage.currentElement = element;
        showProperties(element);
    };

    stage.deleteObj = function (element) {

        //if (element.destroy) {
        //    element.destroy = function () {
        //        // remove from ids and names hashes
        //        Kinetic._removeId(this.getId());
        //        Kinetic._removeName(this.getName(), this._id);

        //        this.remove();
        //    }
        //}

        if (window.stage.setCurrentLayer() == element) {
            window.stage.setCurrentLayer(null);

            element.destroy();
            stage.draw();


            if (window.stage.getCurrentElement() && window.stage.getCurrentElement().getLayer() == element) {
                window.stage.setCurrentElement(null);
            }

        } else {

            var layer = element.getLayer();
            element.destroy();

            layer.draw();

            if (window.stage.getCurrentElement() == element) {
                window.stage.setCurrentElement(null);
            }
        }

    };

    $('input[name=stageName]').val(stage.getName());
    $('input[name=stageWidth]').val(stage.getWidth());
    $('input[name=stageHeight]').val(stage.getHeight());
}

function addLayer() {
    var num = window.stage.children.length;
    var layer = new Kinetic.Layer({
        name: 'layer' + num,
        draggable:true,
        'fill': 'red',
        'stroke': 'red',
        'strokeWidth': 1,
        'width': window.stage.getWidth(),
        'height': window.stage.getHeight()
    });

    formatLayer(layer);

    window.stage.add(layer);

    if ($('#layerList').children().length == 1) {
        $($('#layerList').children()[0]).attr('selected', 'selected').click();
    }
}

function formatLayer(layer) {
    layer.on('nameChange.kinetic', function () {
        var that = this;
        $($('#layerList').children()).each(function (i, item) {
            if (item.layer == that) {
                $(item).text(that.getName());
                return;
            }
        });
    });
    layer.on('widthChange.kinetic', function () {
        this.children.each(function (item) {
            if (item.getName() == 'maskLayer') {
                item.setSize(item.getParent().getSize());
            }
        });
    });
    layer.on('heightChange.kinetic', function () {
        this.children.each(function (item) {
            if (item.getName() == 'maskLayer') {
                item.setSize(item.getParent().getSize());
            }
        });
    });

    layer.getEditor = function () {
        return {
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
            }
        };
    }

    //重写序列化方法
    layer.toObject = function () {
        var obj = Kinetic.Node.prototype.toObject.call(this);

        obj.children = [];

        var children = this.getChildren();
        var len = children.length;
        for (var n = 0; n < len; n++) {
            var child = children[n];
            if (child.getName() != 'maskLayer') {
                obj.children.push(child.toObject());
            }
        }

        return obj;
    };

    var maskLayer=new Kinetic.Rect({
        draggable: false,
        'fill': 'rgba(233, 233, 233, 0.2)',
        'stroke': '#ccc',
        'strokeWidth': 1,
        'width': layer.getWidth(),
        'height': layer.getHeight(),
        'opacity': 1,
        'name': 'maskLayer',
        sceneFunc: function (context) {
            context.beginPath();
            context.fillRect(0, 0, this.getWidth(), this.height());
            context.closePath();
            context.fillStrokeShape(this);
        }
    });

    layer.add(maskLayer);
    
    maskLayer.moveToBottom();
    var layerListItem = $('<option></option>')
                       .text(layer.getName())
                       .click(function () {
                           window.stage.setCurrentLayer(this.layer);
                       });
    layerListItem[0].layer = layer;

    $('#layerList').append(layerListItem);

    layer.setDraggable(true);
}


function addJSConveyorLocation() {
    var layer = window.stage.getCurrentLayer();
    if (!layer) {
        alert('未选择任何层对象');
        return;
    }

    var conveyorLocation = new Kinetic.JSConveyorLocation({
        x: layer.getWidth()/2,
        y: layer.getHeight() / 2,
        width: 25,
        height: 50,
        fill: '#000',
        stroke: '#000',
        strokeWidth: 2,
        draggable: true
    });
    
    formatJSConveyorLocation(conveyorLocation);

    layer.add(conveyorLocation);

    layer.draw();

    window.stage.setCurrentElement(conveyorLocation);
}

function formatJSConveyorLocation(conveyorLocation){
    conveyorLocation.on('click', function () {
        window.stage.setCurrentElement(this);
    });
    conveyorLocation.on('dragmove', function () {
        window.stage.setCurrentElement(this);
    });
    conveyorLocation.setDraggable(true);
}

function addImage() {
    var layer = window.stage.getCurrentLayer();
    if (!layer) {
        alert('未选择任何层对象');
        return;
    }

    var jsImage = new Kinetic.JSImage({
        x: layer.getWidth() / 2,
        y: layer.getHeight() / 2,
        width: 50,
        height: 50,
        draggable: true
    });

    formatImage(jsImage);

    layer.add(jsImage);

    layer.draw();

    window.stage.setCurrentElement(jsImage);
}

function formatImage(image) {
    image.on('click', function () {
        window.stage.setCurrentElement(this);
    });
    image.on('dragmove', function () {
        window.stage.setCurrentElement(this);
    });
    image.setDraggable(true);
}

function addCrane() {
    var layer = window.stage.getCurrentLayer();
    if (!layer) {
        alert('未选择任何层对象');
        return;
    }

    var crane = new Kinetic.JSCrane({
        x: layer.getWidth() / 2,
        y: layer.getHeight() / 2,
        width: 300,
        height: 100,
        listening:true,
        draggable: true
    });

    formatCrane(crane);

    layer.add(crane);

    layer.draw();

    window.stage.setCurrentElement(crane);
}

function formatCrane(crane) {
    crane.on('click', function () {
        window.stage.setCurrentElement(this);
    });
    crane.on('dragmove', function () {
        window.stage.setCurrentElement(this);
    });
    crane.setDraggable(true);
}


function addTBCrane() {
    var layer = window.stage.getCurrentLayer();
    if (!layer) {
        alert('未选择任何层对象');
        return;
    }

    var crane = new Kinetic.jsTBCrane({
        x: layer.getWidth() / 2,
        y: layer.getHeight() / 2,
        width: 300,
        height: 100,
        listening: true,
        draggable: true
    });

    formatCrane(crane);

    layer.add(crane);

    layer.draw();

    window.stage.setCurrentElement(crane);
}

function addJSConveyorDataAdapter() {
    var layer = window.stage.getCurrentLayer();
    if (!layer) {
        alert('未选择任何层对象');
        return;
    }

    var conveyorDataAdapter = new Kinetic.JSConveyorDataAdapter({
        x: layer.getWidth() / 2,
        y: layer.getHeight() / 2,
        width: 50,
        height: 50,
        listening: true,
        draggable: true
    });

    formatJSConveyorDataAdapter(conveyorDataAdapter);

    layer.add(conveyorDataAdapter);

    layer.draw();

    window.stage.setCurrentElement(conveyorDataAdapter);
}

function formatJSConveyorDataAdapter(conveyorDataAdapter) {
    conveyorDataAdapter.on('click', function () {
        window.stage.setCurrentElement(this);
    });
    conveyorDataAdapter.on('dragmove', function () {
        window.stage.setCurrentElement(this);
    });
    conveyorDataAdapter.setDraggable(true);
}

function addJSCraneDataAdapter() {
    var layer = window.stage.getCurrentLayer();
    if (!layer) {
        alert('未选择任何层对象');
        return;
    }

    var craneDataAdapter = new Kinetic.JSCraneDataAdapter({
        x: layer.getWidth() / 2,
        y: layer.getHeight() / 2,
        width: 50,
        height: 50,
        listening: true,
        draggable: true
    });

    formatJSCraneDataAdapter(craneDataAdapter);

    layer.add(craneDataAdapter);

    layer.draw();

    window.stage.setCurrentElement(craneDataAdapter);
}

function formatJSCraneDataAdapter(craneDataAdapter) {
    craneDataAdapter.on('click', function () {
        window.stage.setCurrentElement(this);
    });
    craneDataAdapter.on('dragmove', function () {
        window.stage.setCurrentElement(this);
    });
    craneDataAdapter.setDraggable(true);
}

function addJSShuttle() {
    var layer = window.stage.getCurrentLayer();
    if (!layer) {
        alert('未选择任何层对象');
        return;
    }

    var jsShuttle = new Kinetic.JSShuttle({
        x: layer.getWidth() / 2,
        y: layer.getHeight() / 2,
        width: 50,
        height: 300,
        listening: true,
        draggable: true
    });

    formatJSShuttle(jsShuttle);

    layer.add(jsShuttle);

    layer.draw();

    window.stage.setCurrentElement(jsShuttle);
}

function formatJSShuttle(jsShuttle) {
    jsShuttle.on('click', function () {
        window.stage.setCurrentElement(this);
    });
    jsShuttle.on('dragmove', function () {
        window.stage.setCurrentElement(this);
    });
    jsShuttle.setDraggable(true);
}

function showProperties(obj) {
    $('#property').empty();
    $('#property')[0].obj = obj;
    $('#btnDeleteObj').hide();
    if (!obj) {
        return;
    }

    if (!obj["getEditor"]) {
        return;
    }


    $('#btnDeleteObj').show();

    var editor = obj.getEditor();
    for (var fieldName in editor) {
        var field = editor[fieldName];
        var text = $('<label>' + field.text + '</label>')
                    .css('width', 100)
                    .css('display', 'block')
                    .css('float', 'left');
        var f = $('<input name="' + fieldName + '" type="text" value="' + obj[field.getter]() + '"/>')
                    .keyup(function (e) {
                        if (e.keyCode == 13) {
                            if (this.field.parser && typeof (this.field.parser) == 'function') {
                                var v = this.field.parser($(this).val());
                                this.obj[this.field.setter](v);
                                $(this).val(v);
                            } else {
                                this.obj[this.field.setter]($(this).val());
                            }

                            this.obj.getLayer().draw();
                        }
                    });

        f[0].field = field;
        f[0].obj = obj;

        $('#property').append(text)
                    .append(f)
                    .append($('<br />'));
    }
}

function saveStage(name) {
    if (!name || $.trim(name) == '') {
        var dialog = new Dialog('名称：<input type="text" id="stageName" /> <button id="dglOK">确定</button>', { title: '请输入保存的画成名称' });

        dialog.show();

        $('#dglOK').click(function () {
            if ($.trim($('#stageName').val()) == '') {
                return;
            }

            var v = $.trim($('#stageName').val());


            $.ajax({
                url: '/Editor/Save',
                dataType: 'text',
                type: 'post',
                data: {
                    'name': v,
                    'contents': window.stage.toJSON()
                },
                cache: false,
                success: function (data) {
                    dialog.close();
                    alert('保存成功');
                },
                error: function (data) {
                    if (data.statusText != "OK") {
                        alert('保存失败');
                    }
                }
            });

        });
    } else {
        $.ajax({
            url: '/Editor/Save',
            dataType: 'text',
            type: 'post',
            data: {
                'name': name,
                'contents': window.stage.toJSON()
            },
            cache: false,
            success: function (data) {
                alert('保存成功');
            },
            error: function (data) {
                if (data.statusText != "OK") {
                    alert('保存失败');
                }
            }
        });
    }
}

function loadStage(name) {

    if (window.stage) {
        window.stage.destroy();
        window.stage = null;
    }

    $('#stage').empty();
    $('#layerList').empty();

    $.ajax({
        url: '/Editor/Load?name=' + name,
        dataType: 'text',
        type: 'get',
        cache:false,
        success: function (data) {

            window.stage = Kinetic.Node.create(data, 'stage');
            formatStage(window.stage);

            if (Kinetic.isDesign) {
                window.stage.getChildren().each(function (item) {

                    if (item.getClassName() == 'Layer') {
                        formatLayer(item);

                        item.getChildren().each(function (layerItem) {
                            if (layerItem.getClassName() == 'JSConveyorLocation') {
                                formatJSConveyorLocation(layerItem);
                            }

                            if (layerItem.getClassName() == 'JSImage') {
                                formatImage(layerItem);
                            }

                            if (Kinetic.JSCraneTypes.contains(layerItem.getClassName())) {
                                formatCrane(layerItem);
                            }

                            if (layerItem.getClassName() == 'JSConveyorDataAdapter') {
                                formatJSConveyorDataAdapter(layerItem);
                            }

                            if (layerItem.getClassName() == 'JSCraneDataAdapter') {
                                formatJSCraneDataAdapter(layerItem);
                            }

                            if (layerItem.getClassName() == 'JSShuttle') {
                                formatJSShuttle(layerItem);
                            }
                        });

                        item.draw();
                    }
                });
            } else {
                window.stage.getChildren().each(function (item) {
                    item.setDraggable(false);
                    item.getChildren().each(function (obj) {
                        obj.setDraggable(false);
                    });
                });
            }

        },
        error: function (e) {
            alert('加载失败');
        }
    });
}

function loadStageList() {
    $.ajax({
        url: '/Editor/LoadList',
        dataType: 'json',
        type: 'get',
        cache: false,
        success: function (data) {
            $(stageList).empty();
            for (var i = 0; i < data.length; i++) {
                var item = $('<option value="' + data[i] + '">' + data[i] + '</option>');
                $(item).dblclick(function () {
                    loadStage($(this).val());
                });
                $(stageList).append(item);
            }
        },
    });
}

function applyStage() {
    window.stage.setName($('input[name=stageName]').val());
    window.stage.setWidth(parseInt($('input[name=stageWidth]').val()));
    window.stage.setHeight(parseInt($('input[name=stageHeight]').val()));
}

$('input[name=stageWidth]').keyup(function (e) {
    window.stage.setWidth(parseInt($(this).val()));
})
.val(window.stage.getWidth());

$('input[name=stageHeight]').keyup(function (e) {
    window.stage.setHeight(parseInt($(this).val()));
})
.val(window.stage.getHeight());

formatStage(window.stage);

if (USERDATA.get('isDesign')) {
    if (USERDATA.get('isDesign') == 'true') {
        Kinetic.isDesign = true;
    } else {
        Kinetic.isDesign = false;
    }
}
else {

    Kinetic.isDesign = false;
}


$('#isDesign').click(function () {
    if (!confirm('切换视图模式将刷新本页，未保存的数据将全部丢失，是否继续？')) {
        $(this)[0].checked = !$(this)[0].checked;
        return;
    }
    USERDATA.set('isDesign', $(this)[0].checked);
    window.stage.isDesign = $(this)[0].checked;

    window.location.reload();
})
[0].checked = Kinetic.isDesign;

$('#btnDeleteObj').click(function () {
    window.stage.deleteObj($('#property')[0].obj);
});

loadStageList();
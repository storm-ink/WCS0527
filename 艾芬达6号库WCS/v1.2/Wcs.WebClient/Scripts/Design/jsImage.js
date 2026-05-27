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
        var image = group.getImage();
        var mask = group.getMask();
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
        mask.setPosition(topLeft.getPosition());

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


    Kinetic.JSImage = function (config) {
        this.____init(config);
    };

    Kinetic.JSImage.prototype = {
        ____init: function (config) {
            var that = this;
            this.className = 'JSImage';
            Kinetic.Group.call(this, config);

            this.on('add.kinetic', function (evt) {
                that._addListeners(evt.child);
                that._sync();
            });
            
            if (Kinetic.isDesign) {
                var maskLayer = new Kinetic.Rect({
                    draggable: false,
                    'fill': '#ccc',
                    'stroke': '#000',
                    'strokeWidth': 1,
                    'width': this.getWidth(),
                    'height': this.getHeight(),
                    'opacity': 0.3,
                    'name': 'maskLayer',
                    sceneFunc: function (context) {
                        context.beginPath();
                        context.fillRect(0, 0, this.getWidth(), this.height());
                        context.closePath();
                        context.fillStrokeShape(this);
                    }
                });

                this.add(maskLayer);
                maskLayer.moveToBottom();
            }

            //添加主体
            this.add(new Kinetic.Image({
                width: config.width,
                height: config.height,
                draggable: false,
                name: 'image'
            }));

            //加载图片
            if (this.getSrc() && this.getSrc() != '') {
                var img = new Image(this.getWidth(), this.getHeight());
                img.onload = function () {
                    that.getImage().setImage(img);
                    that.getImage().getLayer().draw();
                };

                img.src = this.getSrc();
            }

            if (Kinetic.isDesign) {
                //添加缩放角
                var anchorSize = 4;
                addAnchor(this, 0, 0, 'topLeft', anchorSize);
                addAnchor(this, config.width - anchorSize, 0, 'topRight', anchorSize);
                addAnchor(this, config.width - anchorSize, config.height - anchorSize, 'bottomRight', anchorSize);
                addAnchor(this, 0, config.height - anchorSize, 'bottomLeft', anchorSize);

            }
        },
        getImage: function () {
            return this.find('.image')[0];
        },
        getMask: function () {
            return this.find('.maskLayer')[0];
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

            this.on('widthChange.kinetic', function () {
                if (this.getImage()) {
                    this.getImage().setWidth(this.getWidth());
                    this.getMask().setWidth(this.getWidth());
                    updateAnchorPosition(this);
                }
            });

            this.on('heightChange.kinetic', function () {
                if (this.getImage()) {
                    this.getImage().setHeight(this.getHeight());
                    this.getMask().setHeight(this.getHeight());
                    updateAnchorPosition(this);
                }
            });

            this.on('srcChange.kinetic', function () {
                var kinImage = this.getImage();
                kinImage.setImage(null);
                var img = new Image(this.getWidth(), this.getHeight());
                img.onload = function () {
                    kinImage.setImage(img);
                    kinImage.getLayer().draw();
                };

                img.src = this.getSrc();
            });
        },
        _sync: function () {

        }
    };

    Kinetic.Util.extend(Kinetic.JSImage, Kinetic.Group);

    Kinetic.Factory.addGetterSetter(Kinetic.JSImage, 'src', '');

    Kinetic.Factory.addGetterSetter(Kinetic.JSImage, 'editor',
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
        "src": {
            text: '图片地址',
            getter: 'getSrc',
            setter: 'setSrc'
        }
    });

    Kinetic.Collection.mapMethods(Kinetic.JSImage);
})();
function confirmNoty(msg, onOK, onCancel) {
    noty({
        layout: 'center',
        text: msg,
        modal: true,
        buttons: [
            { addClass: 'btn btn-primary', text: '确定', onClick: function ($noty) {

                $noty.close();
                if (onOK) {
                    onOK();
                }
            }
            },
            { addClass: 'btn btn-danger', text: '取消', onClick: function ($noty) {
                $noty.close();
                if (onCancel) {
                    onCancel();
                }
            }
            }
          ]
    });
}

//function infoNoty(msg) {
//    noty({ layout: 'topCenter', type: 'alert', text: msg, timeout: 2000 });
//}

function infoNoty(msg) {
    noty(
            {
                //layout: 'bottomCenter',
                layout: 'topCenter',
                type: 'alert',
                text: msg,
                closeWith: 'button',
                buttons: [{
                    addClass: 'btn btn-primary',
                    text: '关闭',
                    onClick: function ($noty) {
                        $noty.close();
                    }
                }]
            });
}

function successNoty(msg) {
    noty({ layout: 'top', type: 'success', text: msg, timeout: 2000 });
}

function errorNoty(msg) {
    noty({ layout: 'top', type: 'error', text: msg });
}

function warningNoty(msg) {
    noty({ layout: 'top', type: 'warning', text: msg, timeout: 2000 });
}


$(document).ready(function () {
    ///Upload danh sách ảnh
    var max_fields = 2; //maximum input boxes allowed
    var wrapper = $("#myinput"); //Fields wrapper
    var add_button = $(".btnAddNew"); //Add button ID
    var x = 0; //initlal text box count
    $(add_button).click(function (e) { //on add input button click
        debugger;
        e.preventDefault();
        if (x < max_fields) { //max input box allowed
            x++; //text box increment
            debugger;

            $(wrapper).append('<div>   <input type="file" name="listFile[' + x + ']" required/><a href="#" class="btnRemove">Xóa</a></div>'); //add input box
        }
    });
    $(".myinputFile").change(function () {
        readURL(this);
    });
    DisplayChooseImg('#id-input-file-1');
});

//Hàm: Chọn hình ảnh và show ảnh
function DisplayChooseImg(e) {
    $(e).ace_file_input({
        style: 'well',
        btn_choose: 'Chọn hình ảnh',
        btn_change: null,
        no_icon: 'ace-icon fa fa-cloud-upload',
        droppable: true,
        thumbnail: 'small'//large | fit
                ,
        preview_error: function (filename, error_code) {
        }

    }).on('change', function () {
    });

    //dynamically change allowed formats by changing allowExt && allowMime function
    $('#id-file-format').removeAttr('checked').on('change', function () {
        debugger;
        var whitelist_ext, whitelist_mime;
        var btn_choose
        var no_icon
        if (this.checked) {
            btn_choose = "Drop images here or click to choose";
            no_icon = "ace-icon fa fa-picture-o";

            whitelist_ext = ["jpeg", "jpg", "png", "gif", "bmp"];
            whitelist_mime = ["image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp"];
        }
        else {
            btn_choose = "Drop files here or click to choose";
            no_icon = "ace-icon fa fa-cloud-upload";

            whitelist_ext = null;//all extensions are acceptable
            whitelist_mime = null;//all mimes are acceptable
        }
        var file_input = $(e);
        file_input
        .ace_file_input('update_settings',
        {
            'btn_choose': btn_choose,
            'no_icon': no_icon,
            'allowExt': whitelist_ext,
            'allowMime': whitelist_mime
        })
        file_input.ace_file_input('reset_input');
        file_input
        .off('file.error.ace')
        .on('file.error.ace', function (e, info) {
        });
    });
    $('#spinner1').ace_spinner({ value: 0, min: 0, max: 200, step: 10, btn_up_class: 'btn-info', btn_down_class: 'btn-info' })
    .closest('.ace-spinner')
    .on('changed.fu.spinbox', function () {
    });
    $('#spinner2').ace_spinner({ value: 0, min: 0, max: 10000, step: 100, touch_spinner: true, icon_up: 'ace-icon fa fa-caret-up bigger-110', icon_down: 'ace-icon fa fa-caret-down bigger-110' });
    $('#spinner3').ace_spinner({ value: 0, min: -100, max: 100, step: 10, on_sides: true, icon_up: 'ace-icon fa fa-plus bigger-110', icon_down: 'ace-icon fa fa-minus bigger-110', btn_up_class: 'btn-success', btn_down_class: 'btn-danger' });
    $('#spinner4').ace_spinner({ value: 0, min: -100, max: 100, step: 10, on_sides: true, icon_up: 'ace-icon fa fa-plus', icon_down: 'ace-icon fa fa-minus', btn_up_class: 'btn-purple', btn_down_class: 'btn-purple' });
    $('#modal-form input[type=file]').ace_file_input({
        style: 'well',
        btn_choose: 'Kéo hình ảnh bấm vào đây để chọn ảnh',
        btn_change: null,
        no_icon: 'ace-icon fa fa-cloud-upload',
        droppable: true,
        thumbnail: 'large'
    })
}
function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#blah').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}
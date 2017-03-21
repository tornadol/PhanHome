$(function () {
    $('.nav-list').find('#news').addClass("active");
    $('.nav-list').find('#news').find('b').addClass("arrow");
    //Xóa 
    $('a.red').each(function () {
        $(this).click(function () {
            debugger;
            if (confirm('Bạn có muốn xóa ?')) {
                var id = $(this).find('.id').val();
                $.ajax({
                    url: 'New/Delete',
                    type: 'POST',
                    data: { id: id },
                    success: function (e) {
                        if (e.ResultCode == 0) {
                            alert("Có lỗi xảy ra !!!")
                        }
                        if (e.ResultCode == 1) {
                            alert("Xóa thành công")
                            window.location = window.location.href;
                        }
                    }
                })
            }
        })
    });
    //Search 
    $('#nav-search-input').on('keydown', function (e) {
        if (e.which == 13) {
            debugger;
            var key = $(this).val();
            key = remove_unicode(key);
            if (key != "" && key != null) {
                var pathname = window.location.pathname;
                window.location = pathname + '?s=' + key;
            }
            else {
                window.location = window.location.pathname;
            }
        }
    });
    $('#editor1').ace_wysiwyg({
        toolbar:
		[
			'font',
			null,
			'fontSize',
			null,
			{ name: 'bold', className: 'btn-info' },
			{ name: 'italic', className: 'btn-info' },
			{ name: 'strikethrough', className: 'btn-info' },
			{ name: 'underline', className: 'btn-info' },
			null,
			{ name: 'insertunorderedlist', className: 'btn-success' },
			{ name: 'insertorderedlist', className: 'btn-success' },
			{ name: 'outdent', className: 'btn-purple' },
			{ name: 'indent', className: 'btn-purple' },
			null,
			{ name: 'justifyleft', className: 'btn-primary' },
			{ name: 'justifycenter', className: 'btn-primary' },
			{ name: 'justifyright', className: 'btn-primary' },
			{ name: 'justifyfull', className: 'btn-inverse' },
			null,
			{ name: 'createLink', className: 'btn-pink' },
			{ name: 'unlink', className: 'btn-pink' },
			null,
			{ name: 'insertImage', className: 'btn-success' },
			null,
			'foreColor',
			null,
			{ name: 'undo', className: 'btn-grey' },
			{ name: 'redo', className: 'btn-grey' }
		],
        'wysiwyg': {
            fileUploadError: showErrorAlert
        }
    }).prev().addClass('wysiwyg-style2');
    //RESIZE IMAGE
    if (typeof jQuery.ui !== 'undefined' && ace.vars['webkit']) {

        var lastResizableImg = null;
        function destroyResizable() {
            if (lastResizableImg == null) return;
            lastResizableImg.resizable("destroy");
            lastResizableImg.removeData('resizable');
            lastResizableImg = null;
        }

        var enableImageResize = function () {
            $('.wysiwyg-editor')
			.on('mousedown', function (e) {
			    var target = $(e.target);
			    if (e.target instanceof HTMLImageElement) {
			        if (!target.data('resizable')) {
			            target.resizable({
			                aspectRatio: e.target.width / e.target.height,
			            });
			            target.data('resizable', true);

			            if (lastResizableImg != null) {
			                //disable previous resizable image
			                lastResizableImg.resizable("destroy");
			                lastResizableImg.removeData('resizable');
			            }
			            lastResizableImg = target;
			        }
			    }
			})
			.on('click', function (e) {
			    if (lastResizableImg != null && !(e.target instanceof HTMLImageElement)) {
			        destroyResizable();
			    }
			})
			.on('keydown', function () {
			    destroyResizable();
			});
        }

        enableImageResize();
    }
    function remove_unicode(str) {
        str = str.toLowerCase();
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-");

        str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1- 
        str = str.replace(/^\-+|\-+$/g, "");

        return str;
    }
    function showErrorAlert(reason, detail) {
        var msg = '';
        if (reason === 'unsupported-file-type') { msg = "Unsupported format " + detail; }
        else {
            //console.log("error uploading file", reason, detail);
        }
        $('<div class="alert"> <button type="button" class="close" data-dismiss="alert">&times;</button>' +
		 '<strong>File upload error</strong> ' + msg + ' </div>').prependTo('#alerts');
    }
    
})
var lstImagesNew = "";
function SummitForm() {
    var check = false
    //var name = $('input[name=name]').val();
    //var cate = $('select[name=cate]').val();
    // file = $('input[name=file]').val();
    //var checkEdit = $('input[name=checkEdit]').val();
    //if (checkEdit == "yes" && checkBackspace) {
    //    $('input[name=checkBackspace]').val("yes");
    //}
    var form = $('form');
    var formdata = validate(form);
    var fileInput = $('input[name=file]');
    
    debugger;
    if (formdata == null) {
        return;
    }
    var data = $('#editor1').html();
    $('input[name=fulldes]').val(data);
    $('form').submit();
    //$.ajax({
    //    url: '/New/Save',
    //    type: 'POST',
    //    data: { formdata: formdata,  data: data}
    //})
}
function validate(frm) {
    var frmData = frm.serializeObject();
    var labelErr = frm.find(".error");
    var name = frm.find('input[name=name]');
    var cate = frm.find('select[name=cate]');
    var file = frm.find('input[name=file]');
    debugger;
    if (frmData.name == "") {
        name.addClass("err").focus();
        showErrorLabel(labelErr, "Chưa nhập tên bài viết.");
        return null;
    } else {
        name.removeClass("err");
    }
    labelErr.hide();
    return frmData;
}
function showErrorLabel(label, text) {
    label.html(text);
    label.css("display", "inline-block");
}
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
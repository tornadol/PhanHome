var checkBackspace = false;
$(function () {
    $('.nav-list').find('#item').addClass("active");
    $('.nav-list').find('#item').find('b').addClass("arrow");
    //Xóa 
    $('a.red').each(function () {
        $(this).click(function () {
            if (confirm('Bạn có muốn xóa ?')) {
                var id = $(this).find('.id').val();
                $.ajax({
                    url: 'Item/Delete',
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
    $(document).keyup(function (e) {
        if (e.which == 8) {
            checkBackspace = true;
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
function GetCateChild() {
    debugger;
    var idCate = $('select[name=cate]').val();
    $.ajax({
        url: "/Admin/Item/GetCateChild",
        type: "POST",
        data: { idCate: idCate },
        beforeSend: function () {

        },
        success: function (result) {
            $('#catechild').html(result);
        }
    })
}
function ChangeCate() {
    debugger;
    $('#catechild').html("");
}
var lstImagesNew = "";
function SummitForm() {
    var form = $('form');
    debugger;
    var formdata = validate(form);
    if (formdata == null) {
        return;
    }
    $('#description_0 img').each(function () {
        var lstimg = $(this).attr('src');
        if (!check) {
            lstImagesNew = lstimg;
            check = true;
        }
        else {
            lstImagesNew = lstImagesNew + ',' + lstimg;
        }
    })
    
    var file = $('input[name=file]').val();
    var checkEdit = $('input[name=checkEdit]').val();
    if (checkEdit == "yes" && checkBackspace) {
        $('input[name=checkBackspace]').val("yes");
    }
    debugger;
    if ((file != "" || checkEdit =="yes")) {
        $('input[name=lstImagesNew]').val(lstImagesNew);
        debugger;
        var data = $('#editor1').html();
        $('input[name=fulldes]').val(data);
        $('form').submit();
    } else {
        $(".error").show().delay(3000).fadeOut();
    }
    //lstImagesNew
}

function showErrorLabel(label, text) {
    label.html(text);
    label.css("display", "inline-block");
    label.show().delay(3000).fadeOut();
    return null;
}
function validate(frm) {
    var frmData = frm.serializeObject();
    var labelErr = frm.find(".error");
    var name = frm.find('input[name=name]');
    if (frmData.name == "") {
        name.addClass("err").focus();
        showErrorLabel(labelErr, "Chưa nhập tên sản phẩm.");
        return null;
    } else {
        name.removeClass("err");
    }

    var cate = frm.find('input[name=cate]');
    if (frmData.cate == "") {
        cate.addClass("err").focus();
        showErrorLabel(labelErr, "Chưa nhập số danh mục.");
        return null;
    }
    var price = frm.find('input[name=price]');
    if (frmData.price == "") {
        price.addClass("err").focus();
        showErrorLabel(labelErr, "Chưa nhập số tiền.");
        return null;
    } else {
        var regPhone = /^(0|[1-9][0-9]*)$/;
        if (!regPhone.test(frmData.price)) {
            price.addClass("err").focus();
            showErrorLabel(labelErr, "Chưa nhập đúng số tiền.");
            return null;
        } else {
            price.removeClass("err");
        }
    }
    labelErr.hide();
    return frmData;
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


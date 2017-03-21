$(function () {
    $('.nav-list').find('#customer').addClass("active");
    $('.nav-list').find('#customer').find('b').addClass("arrow");
    //Xóa 
    $('a.red').each(function () {
        $(this).click(function () {
            if (confirm('Bạn có muốn xóa ?')) {
                var id = $(this).find('.id').val();
                $.ajax({
                    url: 'Customer/Delete',
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
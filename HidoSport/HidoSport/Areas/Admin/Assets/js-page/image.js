$(function(){
    $('.nav-list').find('#image').addClass("active");
    $('.nav-list').find('#image').find('b').addClass("arrow");
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
function copyLink(e) {
    var element = $(e).closest('td').find('span');
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($(element).text()).select();
    document.execCommand("copy");
    $temp.remove();
    alert('Bạn đã copy link ảnh');
}
function copyLinkDetail(e){
    var element = $(e).closest('div').find('span');
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($(element).text()).select();
    document.execCommand("copy");
    $temp.remove();
    alert('Bạn đã copy link ảnh');
}
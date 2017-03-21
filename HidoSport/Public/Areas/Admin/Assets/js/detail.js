$(document).ready(function () {
    
    //Nhập số vào chuyển thành dạng tiền
    ConvertNumberToCurrency('.input-number');
    
});
function format(num) {
    debugger;
    var str = num.toString().replace("$", ""), parts = false, output = [], i = 1, formatted = null;
    if (str.indexOf(".") > 0) {
        parts = str.split(".");
        str = parts[0];
    }
    str = str.split("").reverse();
    for (var j = 0, len = str.length; j < len; j++) {
        if (str[j] != ",") {
            output.push(str[j]);
            if (i % 3 == 0 && j < (len - 1)) {
                output.push(",");
            }
            i++;
        }
    }
    formatted = output.reverse().join("");
    return (formatted + ((parts) ? "." + parts[1].substr(0, 3) : ""));
};
//Hàm: Nhập số vào chuyển thành dạng tiền tệ
function ConvertNumberToCurrency(e) {
    if ($(e).length > 0) {
        $(e).val($(e).val().toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"));
    }
    $(e).on('keydown', function (e) {
        var verified = (e.which == 8 || e.which == undefined || e.which == 0) ? null : String.fromCharCode(e.which).match(/[^0-9]/);
        if (verified) { e.preventDefault(); }
        e.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
        $(this).val(format($(this).val()));
    })
    $(e).on('keyup', function () {
        $(this).val(format($(this).val()));
    })
}



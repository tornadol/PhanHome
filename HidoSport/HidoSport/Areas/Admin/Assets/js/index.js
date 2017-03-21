$(function () {

});

function checkLogin() {
    var user = $('input[name=Username]').val();
    var pass = $('input[name=Password]').val();
    if (user == null && pass == null) {
        alert("Chưa nhập mật khẩu")
    }
    else if (user == null) {

    }
    else if (pass == null) {

    }
    else {

    }
}
document.addEventListener("DOMContentLoaded", function () {
    var elements = document.getElementsByTagName("INPUT");
    for (var i = 0; i < elements.length; i++) {
        elements[i].oninvalid = function (e) {
            e.target.setCustomValidity("");
            if (!e.target.validity.valid) {
                e.target.setCustomValidity("This field cannot be left blank abc");
            }
        };
        elements[i].oninput = function (e) {
            e.target.setCustomValidity("");
        };
    }
})
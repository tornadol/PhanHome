$(function () {
    $('.navbar-nav li').removeClass('active');
    $('.navbar-nav li:nth-of-type(2)').addClass('active');
    $('.dropdown-menu a').click(function () {
        var size = $(this).text();
        $('#size').html(size);
    })
    $('.add-active').removeClass('active');
    $('.add-active').eq('1').addClass('active');
    $(".qtybutton").on("click", function () {
        var $button = $(this);
        var oldValue = $button.parent().find("input").val();
        if ($button.text() == "+") {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            // Don't allow decrementing below zero
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        $button.parent().find("input").val(newVal);
    });
    //Add item into cart
    $('.add-cart').on('click', function () {
        var cart = $('.btn-cart-top');
        var imgtodrag = $('.simpleLens-container.active img');
        if (imgtodrag) {
            var imgclone = imgtodrag.clone()
                .offset({
                    top: imgtodrag.offset().top,
                    left: imgtodrag.offset().left
                })
                .css({
                    'opacity': '0.5',
                    'position': 'absolute',
                    'height': '150px',
                    'width': '150px',
                    'z-index': '100'
                })
                .appendTo($('body'))
                .animate({
                    'top': cart.offset().top + 10,
                    'left': cart.offset().left + 10,
                    'width': 75,
                    'height': 75
                }, 1000, 'easeInOutExpo');

            setTimeout(function () {
                cart.effect("shake", {
                    times: 2
                }, 200);
            }, 1500);

            imgclone.animate({
                'width': 0,
                'height': 0
            }, function () {
                $(this).detach()
            });
        }
        //Add secsion to cart
        var id = $('#productId').val();
        var price = $('#pd-price').val();
        var pricevnd = $('.price').html();
        var name = $('#name').val();
        var img = $('#image').val();
        var quantity = $('.plus-minus-box').val();
        var size = $('#size').text();
       
        var lstitem = JSON.parse(localStorage.getItem("lstitem"));
        var item = {
            'id': id,
            'idgen':randomNumberFromRange(),
            'price': price,
            'pricevnd':pricevnd,
            'name': name,
            'img':img,
            'quantity': quantity,
            'size': size,
        }
        if (lstitem == null) {
            lstitem = [];
        }
        lstitem.push(item);
        localStorage.setItem("lstitem", JSON.stringify(lstitem));
        var data = JSON.parse(localStorage.getItem("lstitem"));
        if (data != null) {
            var count = data.length;
            if (count > 0) {
                $('#count-number').html(count);
                $('#count-number').show();
            }
        }
    });

    $(window).on('scroll', function () {
        $(".menu-left").stick_in_parent();
    });

});
function randomNumberFromRange() {
    var max = 5000;
    var min = 1;
    var randomNumber = Math.floor(Math.random() * (max - min + 1) + min);
    return randomNumber;
}

var onSubmitOrder = false;
$('form').submit(function(event) {
    event.preventDefault();
    var me = $(this);
    if (onSubmitOrder) {
        return;
    }
    var frmData = validate(me);
    if (frmData == null) {
        return;
    }

    url = "/aj/Detail/SubmitText";
    debugger;
    $.ajax(url, {
        data: frmData,
        type: 'POST',
        beforeSend: function () {
            onSubmitOrder = true;
            $('#pay-preloader').show();
            $('#cboxOverlay').show();
        },
        success: function (result) {
            debugger;
            if (result.status < 0) {

                onSubmitOrder = false;
                return;
            }
            else if (result.status == 1) {
                $('#preload').remove();
                $('#success').show();
            }
          
        },
    });
})

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
}
function validate(frm) {
    var frmData = frm.serializeObject();
    var labelErr = frm.find(".lblerr");
    var email = frm.find('input[name=email]');
    if (frmData.email == "") {
        var a = (frmData.name);
        email.focus();
        showErrorLabel(labelErr, "Chưa nhập email.");
        return null;
    } else {
        var regEmail = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (!regEmail.test(frmData.email)) {
            showErrorLabel(labelErr, "Email không hợp lệ.");
        } 
    }

    var phone = frm.find('input[name=phone]');
    if (frmData.phone == "") {
        phone.addClass("err").focus();
        showErrorLabel(labelErr, "Chưa nhập số điện thoại.");
        return null;
    } else {
        var regPhone = /^(((09|08)[0-9]{8})|(01[0-9]{9}))$/;
        if (!regPhone.test(frmData.phone)) {
            phone.focus();
            showErrorLabel(labelErr, "Số điện thoại không hợp lệ.");
            return null;
        } 
    }
    var text = $('textarea').val().trim();
    if (text == "" || text == undefined) {
        showErrorLabel(labelErr, "Bạn chưa nhập ghi chú.");
        return null;
    }
    labelErr.hide();
    return frmData;
}
function showErrorLabel(label, text) {
    label.html(text);
    label.css("display", "inline-block");
}
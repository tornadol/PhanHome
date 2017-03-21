$(function () {
	var data = JSON.parse(localStorage.getItem("lstitem"));
	console.log(data);
	var dataHtml = "";
	var html = "";
	var totalPrice = 0;
	if (data != null && data.length > 0) {
		for (var i = 0; i < data.length; i++) {
			html = '<div class="form-group">';
			html += '<div class="col-sm-4 col-xs-4">';
			html += '<img class="img-responsive" src=' + data[i].img + ' />'
			html += '</div>';
			html += '<div class="col-sm-5 col-xs-5">';
			html += '<div class="col-xs-12">' + data[i].name + '</div>';
			html += '<div class="col-xs-12"><small>Số lượng:<span>' + data[i].quantity + '</span></small></div>';
			html += '</div>';
			html += '<div class="col-sm-3 col-xs-3 text-right">';
			html += '<h6>' + data[i].pricevnd + '</h6>';
			html += '</div>';
			html += '</div>';
			html += '<div class="form-group"><hr /></div>';
			dataHtml += html;
			totalPrice = parseInt(totalPrice) + parseInt(data[i].price * data[i].quantity);
		}
	}
	var textPrice = totalPrice.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") + 'đ';
	dataHtml += '<div class="form-group">';
	dataHtml += '<div class="col-xs-12">';
	dataHtml += '<strong>Tổng tiền</strong>';
	dataHtml += '<div class="pull-right"><span>' + textPrice + '</span></div>';
	dataHtml += '</div>';
	dataHtml += '</div>';
    
	$('#info-cart').html(dataHtml);

})
function showErrorLabel(label, text) {
    label.html(text);
    label.css("display", "inline-block");
    label.show().delay(3000).fadeOut();
    return null;
}
function validate(frm) {
    var frmData = frm.serializeObject();
    var labelErr = frm.find(".lblerr");
    var name = frm.find('input[name=name]');
    if (frmData.name == "") {
        name.addClass("err").focus();
        showErrorLabel(labelErr, "Chưa nhập họ tên.");
        return null;
    } else {
        name.removeClass("err");
    }

    var phone = frm.find('input[name=phone]');
    if (frmData.phone == "") {
        phone.addClass("err").focus();
        showErrorLabel(labelErr, "Chưa nhập số điện thoại.");
        return null;
    } else {
        var regPhone = /^(((09|08)[0-9]{8})|(01[0-9]{9}))$/;
        if (!regPhone.test(frmData.phone)) {
            phone.addClass("err").focus();
            showErrorLabel(labelErr, "Số điện thoại không hợp lệ.");
            return null;
        } else {
            phone.removeClass("err");
        }
    }
    labelErr.hide();
    return frmData;
}
function SubmitForm() {
    var form = $('form');
    var formdata = validate(form);
    if (formdata == null) {
        return;
    }
    var data = JSON.stringify(localStorage.getItem("lstitem"));
    var url = '/dat-hang-thanh-cong';
    $.ajax({
        url: url,
        type: 'POST',
        data: { frmData: formdata, dataJson: data},
        beforeSend: function () {
            $('#pay-preloader').show();
            $('#cboxOverlay').show();
        },
        success: function (result) {
            if (result.status == 1) {
                localStorage.removeItem("lstitem");
                $('#preload').remove();
                $('#success').show();
            }
        }
    })

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


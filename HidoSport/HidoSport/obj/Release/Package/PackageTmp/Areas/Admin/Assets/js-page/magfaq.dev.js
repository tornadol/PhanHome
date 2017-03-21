var usName = '';
var ava = 'https://cdn.dmx.vn/profile/default_avatar_large.png';
var isLogin = false;
var isLog = false;
var faqcustomerAllowedAddress = "";
var CookiehostName = window.location.hostname.replace("www", "");
var CookieNamePosi = "DMX_Location";
var rootUrl = "/";
var isLoading = false;
var charactersAllowed = 70;
var hostName = 'dmx.com';
var statisticStartDate;
var statisticEndDate;
var queryparammore = "";

$(function () {
    var pararr = window.location.href.split("?");
    if (pararr.length > 1)
    {
        queryparammore = "?" + pararr[1];
    }
    initForPostFAQ();
});

function openReadFile(event) {
    checkBackspace = true;
    var input = event.target;
    var ftype = input.files[0].type;
    var reader = new FileReader();
    if (ftype != "image/png" && ftype != "image/jpg" && ftype != "image/jpeg" && ftype != "image/bmp" && ftype != "image/gif") {
        alert("File bạn đăng không phù hợp vui lòng thử lại.");
        reader.abort();
        $("#frmFaq input[type=file]").val('');
    }
}

function initForPostFAQ()
{
    var FLAG_INPUT_IN_FOCUS = true;
    $(document).on('keyup', '#description_0', function (e) {
        var text = $('#description_0').text();
        var itext = $('#inputquestion').val();
        text = text.replace(/<[^>]*>/g, '');

    });

    $('#description_0').focusout(function () {
        FLAG_INPUT_IN_FOCUS = false;
    });
    var pos = 0;
    var fileTemplate = "<li id=\"{{id}}\" class=\"file\" data=\"{{data}}\">"
        + "<img src=\"{{url}}\" alt=\"{{name}}\" />"
                      + "<span class=\"process\"><span class=\"processbar\"></span></span>"
                      + "<span class=\"imgname\">{{name}}</span>"
                      + "</li>";

    $("#uploadfromicon").html5Uploader({
        autofocus: "autofocus",
        name: "upfile",
        postUrl: "/Item/upload",
        onServerLoadStart: function (e, file) { // dau tien
            $(".ul-formattext .li-formattext-Img .iconfaq_insertimg").addClass("waitingforuploadfaq");
            $(".ul-formattext .li-formattext-Img input").attr("disabled");


            $("#frmFaq #description_0").focus();

            $(".list-file-attach").append(fileTemplate.replace(/{{name}}/g, file.name.substring(0, 8) + "...").replace(/{{id}}/g, "img" + pos).replace(/{{data}}/g, file.name));
            pos = pos + 1;            
        },
        onServerError: function (e, file) {
            $("#frmFaq #description_0").focus();            
        },
        onClientAbort: function (e, file) {
            $("#frmFaq #description_0").focus();
        },
        onClientError: function (e, file) {
            $("#frmFaq #description_0").focus();            
        },
        onServerProgress: function (e, file) {// thu 2
            $("#frmFaq #description_0").focus();
        },
        onSuccess: function (e, file) {
            debugger;
            $("#frmFaq input[type=file]").val('');//reset

            $(".ul-formattext .li-formattext-Img .iconfaq_insertimg").removeClass("waitingforuploadfaq");
            $(".ul-formattext .li-formattext-Img input").removeAttr("disabled");


            console.log(e.currentTarget.response);
            var data = $.parseJSON(e.currentTarget.response);
            console.log(data);
            if (data.message == "upload sucessfully") {
                var id = $('li[data="' + file.name + '"]').attr('id');
                $('#' + id).find("img").attr('src', data.imageUrl);
                $('#' + id).find("img").attr('alt', file.name);
                $('#' + id).find('img').attr('id', data.ImageId);
                //$("#uploadfromicon").val("");
                //document.execCommand('insertHTML', false, '<img src="' + data.imageUrl + '" alt="" />');       
                $('#description_0').append('<img src="' + data.imageUrl + '" alt="" style="max-width:100px"/>');
            }
            if (data.message == "maximum size upload") {
                alert("Dung lượng file upload quá lớn. Tối đa 1MB.");
            }
            else if (data.message == "maximum file upload") {
                alert("Bạn chỉ được phép chèn tối đa 4 hình.");
            }
            else if (data.message == "upload failed") {
                alert("File bạn đăng không phù hợp vui lòng thử lại.");
            }
            else if (data.message == "upload failed max") {
                alert("Dung lượng file upload quá lớn. Tối đa 1MB.");
            }

            $("#frmFaq #description_0").focus();
            //$("#frmFaq #description_0").focusEndEditor();
            //$("#frmFaq #description_0").scrollTop($("#frmFaq #description_0")[0].scrollHeight);
        }
    });

    $("#frmFaq").submit(faqms.submitQuestionWithName);
}

$.fn.focusEndEditor = function () {
    $(this).focus();
    var tmp = $('<span />').appendTo($(this)),
        node = tmp.get(0),
        range = null,
        sel = null;

    if (document.selection) {
        range = document.body.createTextRange();
        range.moveToElementText(node);
        range.select();
    } else if (window.getSelection) {
        range = document.createRange();
        range.selectNode(node);
        sel = window.getSelection();
        sel.removeAllRanges();
        sel.addRange(range);
    }
    tmp.remove();
    return this;
}
function initForManageQTV()
{    
    var startDefaultDateforStat = $(".datetimepickerstart").attr("data-date");
    var endDefaultDateforStat = $(".datetimepickerend").attr("data-date");

    statisticStartDate = $(".datetimepickerstart").attr("data-date-orginal");
    statisticEndDate = $(".datetimepickerend").attr("data-date-orginal");

    $(".datetimepickerstart").datetimepicker({
        format: 'd/m/Y',
        lang: "vi",
        minDate: '2012/12/01',
        maxDate: 0,//hom nay
        value: startDefaultDateforStat,
        onShow: function (ct, $i) { $(".datetimepickerstart").blur(); },
        timepicker: false,
        scrollMonth: false,
        scrollInput: false,
        onClose: function (ct, $i) {
            statisticStartDate = ct.dateFormat('Y/m/d');
            $(".datetimepickerstart").blur();
        }
    });
    $(".datetimepickerend").datetimepicker({
        format: 'd/m/Y',
        lang: "vi",
        minDate: '2012/12/01',
        maxDate: 0,//hom nay 
        value: endDefaultDateforStat,
        timepicker: false,
        scrollMonth: false,
        scrollInput: false,
        onShow: function (ct, $i) { $(".datetimepickerend").blur(); },
        onClose: function (ct, $i) {
            statisticEndDate = ct.dateFormat('Y/m/d');
            $(".datetimepickerend").blur();
        }
    });

    // tool faq
    $(".datetimepickerstart2").datetimepicker({
        format: 'd/m/Y',
        lang: "vi",
        minDate: '2012/12/01',
        maxDate: 0,//hom nay
        value: "",
        onShow: function (ct, $i) { $(".datetimepickerstart2").blur(); },
        timepicker: false,
        scrollMonth: false,
        scrollInput: false,
        onClose: function (ct, $i) {
            //statisticStartDate = ct.dateFormat('Y/m/d');
            $(".datetimepickerstart2").blur();
            $(".datetimepickerstart2").attr("data-date", ct.dateFormat('Y/m/d'));
            createCookieDateTiViewFAQ(ct.dateFormat('Y/m/d'), 1);
        }
    });
    $(".datetimepickerend2").datetimepicker({
        format: 'd/m/Y',
        lang: "vi",
        minDate: '2012/12/01',
        maxDate: 0,//hom nay 
        value: "",
        timepicker: false,
        scrollMonth: false,
        scrollInput: false,
        onShow: function (ct, $i) { $(".datetimepickerend2").blur(); },
        onClose: function (ct, $i) {
            //statisticEndDate = ct.dateFormat('Y/m/d');
            $(".datetimepickerend2").blur();
            $(".datetimepickerend2").attr("data-date", ct.dateFormat('Y/m/d'));
            createCookieDateTiViewFAQ(ct.dateFormat('Y/m/d'), 2);
        }
    });
}

function Delete_CookiePosi(name, path, domain) {
    if (getCookie(name))
        document.cookie = name + "=" + ((path) ? ";path=" + path : "") + ((domain) ? ";domain=" + domain : "") + ";expires=Thu, 01 Jan 1970 00:00:01 GMT";
}
function ValidateEmail(input) {
    var emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}$/i;
    if (!emailRegex.test(input))
        return false;
    return true;
}

function isDoubleByte(str) {
    for (var i = 0, n = str.length; i < n; i++) {
        if (str.charCodeAt(i) > 125) { return true; }
    }
    return false;
}

function SetCustomerPositionNow(formdata) {

    var datacook = getCookiePosi(CookieNamePosi);
    if (datacook != null && datacook != "") {
        doSubmit(formdata);
        return;
    }

    $.dmxLocation({
        googleApiKey: googleApiKey,
        getFromCookie: false,
        callback: function (position, dmxLocation) {
            var error = dmxLocation.getError();
            if (error) {
                // Định vị thất bại                   
                console.warn(error);
                formdata.locationerror = error;
                //return;
            }
            else {
                // Định vị thành công 
                faqcustomerAllowedAddress = dmxLocation.getSubAddress();
                faqcustomerAllowedAddress = faqcustomerAllowedAddress.replace("Hồ Chí Minh", "Tp.HCM");
                formdata.faqcustomerAllowedAddress = faqcustomerAllowedAddress;
            }
            doSubmit(formdata);
        }
    });
}

function gettags(tagdata) {
    var outtag = '';
    var arrtag = tagdata.split(',');
    for (i = 0; i < arrtag.length; i++) {
        var tag = '';
        var word = /id=\"t(\d+)\"/i;
        var name = arrtag[i].match(word);
        if (name != null) {
            tag = '@' + name[1];
        } else {
            var word2 = /id=\"e(\d+)\"/i;
            var name2 = arrtag[i].match(word2);
            if (name2 != null) {
                tag = '@' + name2[1];
            } else {
                tag = arrtag[i];
            }
        }
        if (tag != '') {
            tag = tag.replace(/(<([^>]+)>)/ig, "");
            // "dient hoai<div><br></div>"
            outtag = outtag + tag + ',';
        }
    }
    return outtag;
}
var faqms = new function () {

    this.submitQuestionLogin = function (fm) {
        if (!validateForm(fm, true, false))
            return false;

        var $form_data = GetAllFormData(fm);
        $form_data.sendwithname = '';
        $form_data.sendwithemail = '';

        var fileUpload = '';
        $('.list-file-attach li').find("img").each(function () {
            if (fileUpload != "") fileUpload += ";";
            fileUpload += $(this).attr("id");
        });

        $form_data.lUpimage = '';
        $form_data.lUpimage = fileUpload;
        $form_data.description_0 = $('#description_0').html().replace(/</gi, '&lt;').replace(/>/gi, '&gt;');;
        $form_data.chkcheckmail = false;
        //doSubmit($form_data);
        return false;
    };

    this.submitQuestionWithName = function (event) {
        event.preventDefault();
        var fm = $(this);

        if (!validateForm(fm, false, true))
            return false;

        var $form_data = GetAllFormData(fm);
        $form_data.loginemail = '';
        $form_data.loginpass = '';

        var fileUpload = '';
        $('.list-file-attach li').find("img").each(function () {
            if (fileUpload != "") fileUpload += ";";
            fileUpload += $(this).attr("id");
        });

        $form_data.lUpimage = '';
        $form_data.lUpimage = fileUpload;
        $form_data.description_0 = $('#description_0').html().replace(/</gi, '&lt;').replace(/>/gi, '&gt;').replace(/[&]nbsp[;]/gi, " ").trim();
        $form_data.chkcheckmail = false;

        SetCustomerPositionNow($form_data);
        return false;
    };

    FLAG_ADD_QUESTION = true;
    this.submitQuestion = function (fm) {
        if (!validateForm(fm, false, false))
            return false;

        var $form_data = GetAllFormData(fm);
        $form_data.loginemail = '';
        $form_data.loginpass = '';
        $form_data.sendwithname = '';
        $form_data.sendwithemail = '';

        var fileUpload = '';
        $('.list-file-attach li').find("img").each(function () {
            if (fileUpload != "") fileUpload += ";";
            fileUpload += $(this).attr("id");
        });

        $form_data.lUpimage = '';
        $form_data.lUpimage = fileUpload;
        $form_data.description_0 = $('#description_0').html().replace(/</gi, '&lt;').replace(/>/gi, '&gt;');;
        $form_data.chkcheckmail = false;
        //doSubmit($form_data);
        return false;
    };

    this.logout = function () {
        $.ajax({
            url: '/hoi-dap/aj/ManageFAQ/Logout',
            type: 'POST',
            data: '',
            beforeSend: function () { },
            success: function (js) {
                clearCookie();
                window.location.reload();
            }
        });
    };
}

function validateForm(fm, checkLogin, withName) {
    var $form_data = GetAllFormData(fm);
        
    var dataContenttest = $('#frmFaq #description_0').text().trim();
    
    if ($('#frmFaq #description_0').find("img").length > 4)
    {
        alert('Bạn chỉ được phép chèn tối đa 4 hình');
        $('#frmFaq #description_0').html($('#frmFaq #description_0').html().replace(/[&]nbsp[;]/gi, " ").trim());
        $("#frmFaq #description_0").focus();
        $("#frmFaq #description_0").focusEndEditor();
        $("#frmFaq #description_0").scrollTop($("#frmFaq #description_0")[0].scrollHeight);
        return false;
    }
    else if (dataContenttest == "" && $('#frmFaq #description_0').find("img").length > 0) {
        alert('Vui lòng nhập nội dung câu hỏi');
        $('#frmFaq #description_0').html($('#frmFaq #description_0').html().replace(/[&]nbsp[;]/gi, " ").trim());
        $("#frmFaq #description_0").focus();
        $("#frmFaq #description_0").focusEndEditor();
        $("#frmFaq #description_0").scrollTop($("#frmFaq #description_0")[0].scrollHeight);
        return false;
    }
    else if (dataContenttest == "") {
        alert('Vui lòng nhập nội dung câu hỏi');
        $("#frmFaq #description_0").html("");

        $("#frmFaq #description_0").focus();
        $("#frmFaq #description_0").focusEndEditor();
        $("#frmFaq #description_0").scrollTop($("#frmFaq #description_0")[0].scrollHeight);
        return false;
    }
    else if (dataContenttest.length < 25) {
        alert('Nội dung câu hỏi quá ngắn');
        $('#frmFaq #description_0').html($('#frmFaq #description_0').html().replace(/[&]nbsp[;]/gi, " ").trim());
        $("#frmFaq #description_0").focus();
        $("#frmFaq #description_0").focusEndEditor();
        $("#frmFaq #description_0").scrollTop($("#frmFaq #description_0")[0].scrollHeight);
        return false;
    }
    else if (dataContenttest.length > 1500) {
        alert('Nội dung câu hỏi tối đa 1500 ký tự');
        $('#frmFaq #description_0').html($('#frmFaq #description_0').html().replace(/[&]nbsp[;]/gi, " ").trim());
        $("#frmFaq #description_0").focus();
        $("#frmFaq #description_0").focusEndEditor();
        $("#frmFaq #description_0").scrollTop($("#frmFaq #description_0")[0].scrollHeight);
        return false;
    }
    //if (checkLogin && !isLogin) {
    //    if ($form_data.loginemail.trim() == "") {
    //        alert('Vui lòng nhập thông tin email hoặc số điện thoại');
    //        $("input[name='loginemail']").focus();
    //        return false;
    //    }
    //    if ($form_data.loginpass.trim() == "") {
    //        alert('Vui lòng nhập mật khẩu');
    //        $("input[name='loginpass']").focus();
    //        return false;
    //    }
    //}
    //if (withName && !isLogin) {
        if ($form_data.sendwithname.trim() == "") {
            alert('Vui lòng nhập thông tin họ và tên');
            $("input[name='sendwithname']").focus();
            return false;
        }
    //}

    if ($form_data.sendwithemail.trim() != "") {
        if (!ValidateEmail($form_data.sendwithemail.trim())) {
            alert('Email bạn nhập không hợp lệ');
            $("input[name='sendwithemail']").focus();
            return false;
        }
    }
    return true;
}

function formatDoc(sCmd, sValue) {
    document.execCommand(sCmd, false, '#');
}

function GetAllFormData($f) {
    var dataElement = {};
    $f.find('input[type=text], input[type=password], input[type=radio]:checked, input[type=hidden], textarea').each(function () {
        dataElement[$(this).attr('name')] = $(this).val().trim();
    });
    $f.find('input[type=checkbox]').each(function () {
        dataElement[$(this).attr('name')] = $(this).attr('checked') == 'checked' ? true : false;
    });
    $f.find('select').each(function () {
        dataElement[$(this).attr('name')] = $(this).val();
        dataElement[$(this).attr('name') + 'text'] = $(this).find('option:selected').text();
    });
    var dataAttach = {};
    $f.find('input[type=text], input[type=password], input[type=radio]:checked, input[type=hidden], textarea, select option:selected').each(function () {
        dataAttach = $.extend({}, dataAttach, $(this).data());
    });
    var dataReturn = $.extend({}, dataElement, dataAttach);
    return dataReturn;
}
function checkLogin() {
    var sName = getCookie('dm_fullname');
    var sEmail = getCookie('dm_email');

    if (sName != null && sName != "") {
        isLogin = true;
    }
}

////////////////////////////////////
//var oCore = {
//    'core.is_admincp': false,
//    'core.section_module': 'core',
//    'log.security_token': ''
//};
//var oParams = {
//    'sJsHome': JsHome,
//    'sJsHostname': 'thegioididong.com',
//    'sSiteName': 'tgdd',
//    'sJsStatic': JsStatic,
//    'sJsStaticImage': JsStaticImage,
//    'sVersion': '3.4.0',
//    'sJsAjax': '/kinh-nghiem-hay/ajax/index',
//    'sStaticVersion': '977ce5003566dfb5058d954b0ea87d34',
//    'sGetMethod': 'do',
//    'sDateFormat': 'MDY',
//    'sGlobalTokenName': 'core',
//    'sController': 'core.index-member',
//    'bJsIsMobile': false,
//    'sJsCookiePath': '/',
//    'sJsCookieDomain': '',
//    'sJsCookiePrefix': 'core11',
//    'notification.notify_ajax_refresh': 2
//};

//function checkCkieUser() {
//    var sName = getCookie('dm_fullname');
//    var sEmail = getCookie('dm_email');
//    var sSocial = getCookie('dm_social');

//    var sHtml = "";
//    if (sName != null && sName != "") {
//        sHtml += "<i class=\"iconcom-user1\"></i>";
//        sHtml += "<span class=\"uname\">" + sName + " </span>";
//        sHtml += "<a onclick=\"editName();\" href=\"javascript:void(0)\">(Sửa tên)</a>";
//        if (sSocial != null && sSocial != "") {
//            var arrSocial = sSocial.split("_");
//            var idU = arrSocial[0];
//            var iType = arrSocial[1];

//            sHtml = "";
//            if (iType == 4) {
//                sHtml += "<img class='avaS' src=\"https://graph.facebook.com/" + idU + "/picture?width=25&height=25\" >";
//            }
//            else {
//                sHtml += "<i class=\"iconcom-user1\"></i>";
//            }

//            sHtml += "<span class=\"uname\">" + sName + " </span>";
//            sHtml += "<a onclick=\"logoutSocial();\" href=\"javascript:void(0)\">(Thoát)</a>";
//        }

//        $("#frmFaq input[name='username']").val(sName);
//        $("#frmFaq input[name='emailuser']").val(sEmail);

//        $(".asideright").hide();
//    }
//}

//function editName() {
//    $(".asideright").show();
//}

//function OpenLoginWindow(socialType) {
//    var my_base_url = domainName + '/auth?socialType=' + socialType;
//    var _width = screen.availWidth * 0.8;
//    var _height = screen.availHeight * 0.6;
//    var Xpos = ((screen.availWidth - _width) / 2);
//    var Ypos = ((screen.availHeight - _height) / 2);

//    window.socialWindow = window.open(my_base_url, "SignIn", 'width=' + _width + ',height=' + _height + ',toolbar=no,resizable=fixed,status=no,scrollbars=no,menubar=no,screenX=' + Xpos + ',screenY=' + Ypos);
//    socialWindow.focus();
//    return;
//}
//function SetUserSocial(userId, userName, userEmail, type) {
//    var sHtml = "<img class='avaS' src=\"https://graph.facebook.com/" + userId + "/picture?width=25&height=25\" >";
//    if (type != 4) {
//        sHtml = "<i class=\"iconcom-user1\"></i>";
//    }
//    sHtml += "<span class=\"uname\">" + userName + "  </span>";
//    sHtml += "<a onclick=\"logoutSocial();\" href=\"javascript:void(0)\">(Thoát)</a>";
//    $("#userinfoLog").html(sHtml);
//    $("#userinfoLog").removeClass("hide");
//    $("#userzone .asideleft").hide();
//    $("#userzone .asideright").hide();
//    $("#submitsocial").removeClass('hide');
//    saveCkieSocial(userId, userName, userEmail, type);
//}

//function saveCkieSocial(sID, sName, sEmail, sType) {
//    CreateCookie("dm_fullname", sName, 30);
//    CreateCookie("dm_email", sEmail, 30);
//    CreateCookie("dm_social", sID + "_" + sType, 30);

//    $("#frmFaq input[name='username']").val(sName);
//    $("#frmFaq input[name='emailuser']").val(sEmail);
//}

//function logoutSocial() {
//    clearCookie();

//    $("#frmFaq input[name='username']").val('');
//    $("#frmFaq input[name='emailuser']").val('');
//    $("#userzone .asideleft").show();
//    $("#userzone .asideright").show();
//    $("#submitsocial").addClass('hide');
//    $("#userinfoLog").html('');
//}

// set cookie
function CreateCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; visited=true; path=/; domain=" + hostName + "; expires=" + exdate.toUTCString() + ";");
    document.cookie = c_name + "=" + c_value;
}
// get cookie
function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
}

function Delete_Cookie(name, path, domain) {
    if (getCookie(name))
        document.cookie = name + "=" + ((path) ? ";path=" + path : "") + ((domain) ? ";domain=" + domain : "") + ";expires=Thu, 01 Jan 1970 00:00:01 GMT";
}

function clearCookie() {
    Delete_Cookie('dm_email', '/', hostName);
    Delete_Cookie('dm_fullname', '/', hostName);
    Delete_Cookie('dm_social', '/', hostName);
}

function addQuestion() {
    //if (!loadFancyBox) {
    //    $.getScript("/kinh-nghiem-hay/Scripts/jquery.fancybox.min.js").done(function () {
    //        loadFancyBox = true;
    //        addQuestion();
    //    });
    //    return;
    //}
    $(".addquestion").remove();
    $.ajax({
        url: rootUrl+"/aj/ManageFAQ/AddQuestion",
        type: 'GET',
        data: { "categoryId": faqCategoryId },
        cache: false,
        beforeSend: function () {
            isLoading = true;
            showLoading();
        },
        success: function (d) {
            $.fancybox(d,
                {
                    'showCloseButton': false,
                    'autoScale': false,
                    'modal': true,
                    'margin': 0,
                    'padding': 0,
                    'closeBtn': false,
                    'scrolling': 'no',
                    'helpers': {
                        'overlay': { 'closeClick': false }
                    },
                });
        },
        complete: function () {
            isLoading = false;
            hideLoading();
        }
    });
}
//edit question:
var isLoadinge = false;
//var loadFancyBoxe = false;
function editQuestion(questionId) {
    //if (!loadFancyBoxe) {
    //    $.getScript("/kinh-nghiem-hay/Scripts/jquery.fancybox.min.js").done(function () {
    //        loadFancyBoxe = true;
    //        addQuestion();
    //    });
    //    return;
    //}
    $(".addquestion").remove();
    $.ajax({
        url: rootUrl + "/aj/ManageFAQ/EditQuestion",
        type: 'GET',
        data: { "categoryId": faqCategoryId, "questionId": questionId },
        cache: false,
        beforeSend: function () {
            isLoadinge = true;
            showLoading();
        },
        success: function (d) {
            $.fancybox(d,
                {
                    'showCloseButton': false,
                    'autoScale': false,
                    'modal': true,
                    'margin': 0,
                    'padding': 0,
                    'closeBtn': false,
                    'scrolling': 'no',
                    'helpers': {
                        'overlay': { 'closeClick': false }
                    },
                });
        },
        complete: function () {
            isLoadinge = false;
            hideLoading();
        }
    });
}

function ValidateEmailGlobal(input) {
    var emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}$/i;
    if (!emailRegex.test(input))
        return false;
    return true;
}
function ValidatePhoneGlobal(input) {
    var regPhone = /^((09[0-9]{8})|(01[0-9]{9}))$/;
    if (!regPhone.test(input))
        return false;
    return true;
}

function loadCommentIntoDetail() {
    //isloadedcomment//biet js comment da load chưa?
    if ($("#divloadcommentdetail").length > 0) {
        $.ajax({
            url: rootUrl + '/aj/Comment/LoadCommentV3' + queryparammore,
            data: { CommentCategory: $("#divloadcommentdetail").data("commentcategory"), NewsID: $("#divloadcommentdetail").data("newsid"), Url: $("#divloadcommentdetail").data("url"), nameproduct: $("#divloadcommentdetail").data("nameproduct"), Page: $("#divloadcommentdetail").data("page") },

            type: 'POST',
            success: function (json) {
                //hideLoading();
                //$('#dlding').show();

                if (json != null && json != "") {
                    $("#divloadcommentdetail").replaceWith("<div id=\"comment\" class=\"comment\" cmtcategoryid=\"" + $("#divloadcommentdetail").data("commentcategory") + "\" siteid=\"2\" detailid=\"" + $("#divloadcommentdetail").data("newsid") + "\" cateid=\"" + $("#divloadcommentdetail").data("commentcategory") + "\" urlpage=\"" + $("#divloadcommentdetail").data("url") + "\" nameproduct=\"" + $("#divloadcommentdetail").data("nameproduct") + "\">" + json + "</div>");

                    $('#comment img[data-src],img[data-original]').lazyload({
                        load: function () {
                            this.style.opacity = 1;
                        }, threshold: 500
                    });

                    //Load javascript comment:
                    if (isloadedcomment == false) {
                        loadComment();
                    }
                }
            },
            beforeSend: function () {
                //showLoading();
            },
            error: function (e) {
                //hideLoading();               
            }
        });
    }
}
//xem faq theo time and cate:
function viewFAQbyDateCate() {
    var txt1 = $(".date_cateFAQmanagement .datetimepickerstart2").val();
    var txt2 = $(".date_cateFAQmanagement .datetimepickerend2").val();
    var url = $(".date_cateFAQmanagement #selchangecateviewhd option:selected").val();
    if (txt1.trim() == "") {
        alert("Vui lòng chọn ngày bắt đầu");
        return;
    }
    if (txt2.trim() == "") {
        alert("Vui lòng chọn ngày kết thúc");
        return;
    }

    var startdate = new Date($(".date_cateFAQmanagement .datetimepickerstart2").attr("data-date"));
    var enddate = new Date($(".date_cateFAQmanagement .datetimepickerend2").attr("data-date"));

    if (enddate < startdate) {
        alert("Vui lòng chọn ngày tháng kết thúc thống kê phải lớn hơn hoặc bằng ngày bắt đầu");
        return;
    }

    if (url.trim() == "") {
        alert("Không thể xác thực thông tin");
        return;
    }
    window.location.href = url;
}
function createCookieDateTiViewFAQ(datetime, type) {
    $.ajax({
        url: rootUrl + '/aj/ManageFAQ/GetFAQDateViewCookie',
        data: { datetime: datetime, type: type },
        type: 'GET'
    });
}
function activeCommentBest() {

    setTimeout(function () {
        try {
            listcomment(2, 3, 0);
        } catch (e) { }

    }, 1000);

}
//load lai du lieu statistic:
var FLAG_LOGGIN_MANAGEFAQStatistic = true;
function loaddatastatistic_again(startdate_p, enddate_p) {
    if (!FLAG_LOGGIN_MANAGEFAQStatistic) {
        return;
    }
    var startdate = new Date(startdate_p);
    var enddate = new Date(enddate_p);

    if (enddate < startdate) {
        alert("Vui lòng chọn ngày tháng kết thúc thống kê phải lớn hơn hoặc bằng ngày bắt đầu");
        return;
    }
    else {
        //load lai data:  
        var isclearcache = getParameterByName("clearcache");
        var queryclearcache = isclearcache.trim() != "" ? "?clearcache=1" : "";
        $.ajax({
            url: rootUrl + '/aj/ManageFAQ/GetFAQStaticAgain' + queryclearcache,
            data: { startdate_p: startdate_p, enddate_p: enddate_p, faqCategoryId: faqCategoryId },
            type: 'POST',
            success: function (json) {
                FLAG_LOGGIN_MANAGEFAQStatistic = true;
                hideLoading();

                if (json != null && json != "") {
                    $("#div_containerstatistic").html(json);
                }
            },
            beforeSend: function () {
                FLAG_LOGGIN_MANAGEFAQStatistic = false;
                showLoading();
            },
            error: function (e) {
                FLAG_LOGGIN_MANAGEFAQStatistic = true;
                hideLoading();
            }
        });
    }
}
function loaddatastatistic_load() {
    loaddatastatistic_again(statisticStartDate, statisticEndDate);
}

function showdnqtvfaqdmx() {
    $(".formLogifaqdmx").show();
}
var FLAG_LOGGIN_MANAGEFAQ = true;
function loginqtfaqdmx() {
    if (!FLAG_LOGGIN_MANAGEFAQ)
        return false;

    var username = $(".formLogifaqdmx input[name='txtusername']").val().trim();
    if (username == "") {
        alert("Vui lòng nhập email hoặc số điện thoại");
        $(".formLogifaqdmx input[name='txtusername']").focus();
        return false;
    }
    else {
        if (!ValidateEmailGlobal(username)) {
            if (!ValidatePhoneGlobal(username)) {
                alert("Email bạn nhập hoặc số điện thoại không hợp lệ");
                return false;
            }
        }
    }

    if ($(".formLogifaqdmx input[name='txtpassword']").val().trim() == "") {
        alert("Vui lòng nhập mật khẩu của bạn");
        $(".formLogifaqdmx input[name='txtpassword']").focus();
        return false;
    }
    var subdata = $(".formLogifaqdmx").serialize();
    //console.log(subdata);
    var isclearcache = getParameterByName("clearcache");
    var queryclearcache = isclearcache.trim() != "" ? "?clearcache=1" : "";

    $.ajax({
        url: rootUrl + '/aj/ManageFAQ/InsiteLogin' + queryclearcache,
        data: subdata,
        type: 'POST',
        success: function (json) {
            FLAG_LOGGIN_MANAGEFAQ = true;
            hideLoading();

            if (json != null && json != "") {
                if (json.tr <= 0) {
                    alert(json.ms);
                    return false;
                }
                else {
                    window.location.reload();
                }
            }
        },
        beforeSend: function () {
            FLAG_LOGGIN_MANAGEFAQ = false;
            showLoading();
        },
        error: function (e) {
            FLAG_LOGGIN_MANAGEFAQ = true;
            hideLoading();
        }
    });
    return false;
}
var FLAG_DELETE_MANAGEFAQ = true;
function deletefaqdmx(id) {
    if (!FLAG_DELETE_MANAGEFAQ)
        return;
    if (!$.isNumeric(id))
        return;


    if (!confirm("Bạn chắc muốn xóa câu hỏi với ID: " + id)) {
        return;
    }
    var isclearcache = getParameterByName("clearcache");

    var subdata = { id: id };
    $.ajax({
        url: rootUrl + '/aj/ManageFAQ/DeleteFAQ',
        data: subdata,
        type: 'POST',
        success: function (json) {
            FLAG_DELETE_MANAGEFAQ = true;
            hideLoading();

            if (json != null && json != "") {
                if (json.tr <= 0) {
                    alert(json.ms);
                    return false;
                }
                else {
                    if (isclearcache.trim() != "") {
                        window.location.reload();
                    }
                    else {
                        var url = window.location.href;
                        if (url.indexOf("?") < 0)
                            url += "?clearcache=1";
                        else
                            url += "&clearcache=1";

                        window.location.href = url;
                    }
                }
            }
        },
        beforeSend: function () {
            FLAG_DELETE_MANAGEFAQ = false;
            showLoading();
        },
        error: function (e) {
            FLAG_DELETE_MANAGEFAQ = true;
            hideLoading();
        }
    });
}
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)", "i"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
function logout() {
    $.ajax({
        url: rootUrl + '/aj/ManageFAQ/Logout',
        type: 'POST',
        data: '',
        beforeSend: function () { },
        success: function (js) {
            //alert("refresh");
            //Delete_Cookie('dm_email', '/', 'dimaxa.com');
            //Delete_Cookie('dm_fullname', '/', 'dimaxa.com');
            window.location = window.location;
        }
    });
    return false;
}
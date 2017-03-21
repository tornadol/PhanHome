$(function () {
    var data = JSON.parse(localStorage.getItem("lstitem"));
    
    if (data != null) {
        var count = data.length;
        if (count > 0) {
            $('#count-number').html(count);
        }
    } else {
        $('#count-number').hide();
    }
    var link = window.location.pathname;
    var hash = window.location.hash;
    if (link == "/") {
        $('.add-active').eq('0').addClass('active');
    }
    if (link == "/gioi-thieu") {
        if (hash == "" || hash == "#bao-hanh") {
            $('.add-active').eq('2').addClass('active');
        }
        if (hash == '#giao-hang') {
            $('.add-active').eq('3').addClass('active');
        }
    }
    $(".dropdown").hover(
        function () {
            $('.dropdown-menu', this).not('.open').stop(true, true).slideDown("10");
            $(this).toggleClass('open');
        },
        function () {
            $('.dropdown-menu', this).not('.open').stop(true, true).slideUp("10");
            $(this).toggleClass('open');
        }
    );
    $(".dropdown").click(function () {
        debugger;
        var link = $(this).find('.dropdown-toggle').attr('href');
        window.location.href = link;

    })
    
})